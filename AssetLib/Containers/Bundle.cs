namespace AssetLib.Containers
{
    public class Bundle
    {
        public const string Signature = "UnityFS";
        private const int BlockSize = 0x20000;

        public Header Header;
        public StorageBlock[] BlocksInfo;
        public Node[] DirectoryInfo;
        public StreamFile[] FileList;
        private MemoryStream BlocksStream;
        private bool HasDataHash;

        private readonly byte[] ExtensionKey;
        private readonly byte[] Key;
        private readonly byte[] ConstKey;
        private readonly byte[] SBox;
        private readonly byte[] BlockKey;

        private int TotalCompressedSize => BlocksInfo.Sum(x => x.CompressedSize);
        public Bundle(Header header, bool hasDataHash, byte[] expansionKey = null, byte[] key = null, byte[] constKey = null, byte[] sbox = null, byte[] blockKey = null)
        {
            Header = header;
            HasDataHash = hasDataHash;
            ExtensionKey = expansionKey;
            Key = key;
            ConstKey = constKey;
            SBox = sbox;
            BlockKey = blockKey;
        }

        public void InitFromMhy0(Mhy0 mhy0)
        {
            BlocksInfo = mhy0.BlocksInfo;
            DirectoryInfo = mhy0.DirectoryInfo;
            FileList = mhy0.FileList;
        }

        public void Process(EndianReader reader)
        {
            ReadMetadata(reader);
            BlocksStream = CreateBlocksStream(false);
            ReadBlocks(reader);
            ReadNodes();
        }

        public void WriteToStream(MemoryStream stream) => stream.Write(Write());

        public void WriteToFile(string output) => File.WriteAllBytes(output, Write());

        private byte[] Write()
        {
            BlocksStream = CreateBlocksStream(true);
            WriteNodes();
            WriteBlocks();
            var buffer = WriteAndCompressBlocksInfo();

            var writer = new EndianWriter(0, EndianType.BigEndian);
            WriteHeader(writer);
            writer.Write(buffer);
            writer.Write(BlocksStream.ToArray(), 0, TotalCompressedSize);
            return writer.ToArray();
        }

        private void ReadMetadata(EndianReader reader)
        {
            var buffer = ReadAndDecompressBlocksInfo(reader);
            ReadBlocksInfoAndDirectory(buffer);
        }

        private void WriteHeader(EndianWriter writer)
        {
            writer.WriteString(Header.Signature, true);
            writer.Write(Header.Version);
            writer.WriteString(Header.UnityVersion, true);
            writer.WriteString(Header.UnityRevision, true);
            writer.Write(Header.Size);
            writer.Write(Header.CompressedBlocksInfoSize);
            writer.Write(Header.UncompressedBlocksInfoSize);
            writer.Write(Header.Flags);
        }

        private byte[] ReadAndDecompressBlocksInfo(EndianReader reader)
        {
            var compressedSize = Header.CompressedBlocksInfoSize;
            var compressedBytes = reader.ReadBytes(compressedSize);
            var uncompressedSize = Header.UncompressedBlocksInfoSize;
            var uncompressedBytes = new byte[uncompressedSize];
            var compressionType = (CompressionType)(Header.Flags % 0x40);
            switch (compressionType)
            {
                case CompressionType.Lz4:
                case CompressionType.Lz4HC:
                    var numWrite = LZ4Codec.Decode(compressedBytes, 0, compressedSize, uncompressedBytes, 0, uncompressedSize);
                    if (numWrite != uncompressedSize)
                    {
                        throw new IOException($"Lz4 decompression error, write {numWrite} bytes but expected {uncompressedSize} bytes");
                    }
                    break;
                case CompressionType.Lz4Mr0k:
                    if (Mr0k.IsMr0k(compressedBytes))
                    {
                        compressedBytes = Mr0k.Decrypt(compressedBytes, ref compressedSize, ExtensionKey, Key, ConstKey, SBox, BlockKey);
                    }
                    if (compressedSize < 0x10)
                    {
                        throw new Exception($"Lz4 decompression error, wrong compressed length: {compressedSize}");
                    }
                    goto case CompressionType.Lz4;
            }
            return uncompressedBytes;
        }

        private byte[] WriteAndCompressBlocksInfo()
        {
            var buffer = WriteBlocksInfoAndDirectory();
            var uncompressedSize = buffer.Length;
            var uncompressed = buffer;

            var maxCompressedSize = LZ4Codec.MaximumOutputSize(uncompressedSize);
            buffer = new byte[maxCompressedSize];
            var compressedSize = LZ4Codec.Encode(uncompressed, 0, uncompressedSize, buffer, 0, maxCompressedSize, LZ4Level.L06_HC);
            if (compressedSize == -1)
            {
                throw new Exception($"Lz4 decompression error, wrong compressed length: {compressedSize}");
            }

            var compressed = new byte[compressedSize];
            Array.Copy(buffer, compressed, compressedSize);

            Header.UncompressedBlocksInfoSize = uncompressedSize;
            Header.CompressedBlocksInfoSize = compressedSize;
            Header.Size = TotalCompressedSize + compressedSize + Header.HeaderSize;
            Header.Flags = 0x43;

            return compressed;
        }

        private void ReadBlocksInfoAndDirectory(byte[] blocksInfo)
        {
            var reader = new EndianReader(blocksInfo, 0, EndianType.BigEndian);
            if (HasDataHash)
            {
                reader.Position += 0x10;
            }
            var blocksInfoCount = reader.ReadInt32();
            BlocksInfo = new StorageBlock[blocksInfoCount];
            for (int i = 0; i < blocksInfoCount; i++)
            {
                BlocksInfo[i] = new StorageBlock
                {
                    UncompressedSize = reader.ReadInt32(),
                    CompressedSize = reader.ReadInt32(),
                    Flags = reader.ReadInt16()
                };
            }
            var directoryInfoCount = reader.ReadInt32();
            DirectoryInfo = new Node[directoryInfoCount];
            for (int i = 0; i < directoryInfoCount; i++)
            {
                DirectoryInfo[i] = new Node
                {
                    Offset = reader.ReadInt64(),
                    Size = reader.ReadInt64(),
                    Flags = reader.ReadInt32(),
                    Path = reader.ReadStringToNull(),
                };
            }
        }

        private byte[] WriteBlocksInfoAndDirectory()
        {
            var writer = new EndianWriter(0, EndianType.BigEndian);
            writer.Write(0, 0x10);
            writer.Write(BlocksInfo.Length);
            foreach (var block in BlocksInfo)
            {
                writer.Write(block.UncompressedSize);
                writer.Write(block.CompressedSize);
                writer.Write(block.Flags);
            }

            writer.Write(DirectoryInfo.Length);
            foreach (var node in DirectoryInfo)
            {
                writer.Write(node.Offset);
                writer.Write(node.Size);
                writer.Write(node.Flags);
                writer.WriteString(node.Path, true);
            }

            return writer.ToArray();
        }

        private MemoryStream CreateBlocksStream(bool isProcessed)
        {
            var uncompressedSizeSum = isProcessed ? (int)DirectoryInfo.Sum(x => x.Size) : BlocksInfo.Sum(x => x.UncompressedSize);
            return new MemoryStream(uncompressedSizeSum);
        }

        private void ReadBlocks(EndianReader reader)
        {
            foreach (var blockInfo in BlocksInfo)
            {
                var compressedSize = blockInfo.CompressedSize;
                var compressedBytes = new byte[compressedSize];
                var uncompressedSize = blockInfo.UncompressedSize;
                var uncompressedBytes = new byte[uncompressedSize];
                reader.Read(compressedBytes, 0, compressedSize);
                var compressionType = (CompressionType)(blockInfo.Flags % 0x40);
                switch (compressionType)
                {
                    case CompressionType.Lz4:
                    case CompressionType.Lz4HC:
                        var numWrite = LZ4Codec.Decode(compressedBytes, 0, compressedSize, uncompressedBytes, 0, uncompressedSize);
                        if (numWrite != uncompressedSize)
                        {
                            throw new IOException($"Lz4 decompression error, write {numWrite} bytes but expected {uncompressedSize} bytes");
                        }
                        break;
                    case CompressionType.Lz4Mr0k:
                        if (Mr0k.IsMr0k(compressedBytes))
                        {
                            compressedBytes = Mr0k.Decrypt(compressedBytes, ref compressedSize, ExtensionKey, Key, ConstKey, SBox, BlockKey);
                        }
                        if (compressedSize < 0x10)
                        {
                            throw new Exception($"Lz4 decompression error, wrong compressed length: {compressedSize}");
                        }
                        goto case CompressionType.Lz4;
                }
                BlocksStream.Write(uncompressedBytes, 0, uncompressedSize);
            }
        }

        private void WriteBlocks()
        {
            var blocksInfo = new List<StorageBlock>();
            var totalUncompressedSize = DirectoryInfo.Sum(x => x.Size);
            var uncompressedBytes = new byte[BlockSize];
            var compressedBytes = new byte[LZ4Codec.MaximumOutputSize(BlockSize)];
            long writePos = 0;
            while (totalUncompressedSize > 0)
            {
                var uncompressedSize = BlocksStream.Read(uncompressedBytes, 0, BlockSize);
                var readPos = BlocksStream.Position;

                var maxCompressedSize = LZ4Codec.MaximumOutputSize(uncompressedSize);
                var compressedSize = LZ4Codec.Encode(uncompressedBytes, 0, uncompressedSize, compressedBytes, 0, maxCompressedSize, LZ4Level.L06_HC);
                if (compressedSize == -1)
                {
                    throw new InvalidOperationException($"LZ4 error: Invalid compressed length: {compressedSize}");
                }

                BlocksStream.Position = writePos;
                BlocksStream.Write(compressedBytes, 0, compressedSize);
                writePos = BlocksStream.Position;
                BlocksStream.Position = readPos;

                var blockInfo = new StorageBlock() { UncompressedSize = uncompressedSize, CompressedSize = compressedSize, Flags = 0x43 };
                blocksInfo.Add(blockInfo);

                totalUncompressedSize -= uncompressedSize;
            }

            BlocksStream.Position = 0;
            BlocksInfo = blocksInfo.ToArray();
        }

        private void ReadNodes()
        {
            FileList = new StreamFile[DirectoryInfo.Length];
            for (int i = 0; i < DirectoryInfo.Length; i++)
            {
                var node = DirectoryInfo[i];
                var file = new StreamFile();
                file.Path = node.Path;
                file.FileName = Path.GetFileName(node.Path);
                file.Stream = new MemoryStream();
                FileList[i] = file;

                BlocksStream.Position = node.Offset;
                BlocksStream.CopyTo(file.Stream, node.Size);
                file.Stream.Position = 0;
            }
            BlocksStream.Dispose();
        }

        private void WriteNodes()
        {
            for (int i = 0; i < FileList.Length; i++)
            {
                var file = FileList[i];
                var node = DirectoryInfo[i];

                file.Stream.CopyTo(BlocksStream, node.Size);
                file.Stream.Dispose();
            }
            BlocksStream.Position = 0;
        }
    }
}
