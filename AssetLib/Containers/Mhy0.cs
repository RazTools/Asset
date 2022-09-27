using AssetLib.Utils;
using System;

namespace AssetLib.Containers
{
    public class Mhy0
    {
        private const string Signature = "mhy0";
        private const int BlockSize = 0x20000;

        private static readonly byte[] GF256Exp = new byte[] { 0x01, 0x03, 0x05, 0x0F, 0x11, 0x33, 0x55, 0xFF, 0x1A, 0x2E, 0x72, 0x96, 0xA1, 0xF8, 0x13, 0x35, 0x5F, 0xE1, 0x38, 0x48, 0xD8, 0x73, 0x95, 0xA4, 0xF7, 0x02, 0x06, 0x0A, 0x1E, 0x22, 0x66, 0xAA, 0xE5, 0x34, 0x5C, 0xE4, 0x37, 0x59, 0xEB, 0x26, 0x6A, 0xBE, 0xD9, 0x70, 0x90, 0xAB, 0xE6, 0x31, 0x53, 0xF5, 0x04, 0x0C, 0x14, 0x3C, 0x44, 0xCC, 0x4F, 0xD1, 0x68, 0xB8, 0xD3, 0x6E, 0xB2, 0xCD, 0x4C, 0xD4, 0x67, 0xA9, 0xE0, 0x3B, 0x4D, 0xD7, 0x62, 0xA6, 0xF1, 0x08, 0x18, 0x28, 0x78, 0x88, 0x83, 0x9E, 0xB9, 0xD0, 0x6B, 0xBD, 0xDC, 0x7F, 0x81, 0x98, 0xB3, 0xCE, 0x49, 0xDB, 0x76, 0x9A, 0xB5, 0xC4, 0x57, 0xF9, 0x10, 0x30, 0x50, 0xF0, 0x0B, 0x1D, 0x27, 0x69, 0xBB, 0xD6, 0x61, 0xA3, 0xFE, 0x19, 0x2B, 0x7D, 0x87, 0x92, 0xAD, 0xEC, 0x2F, 0x71, 0x93, 0xAE, 0xE9, 0x20, 0x60, 0xA0, 0xFB, 0x16, 0x3A, 0x4E, 0xD2, 0x6D, 0xB7, 0xC2, 0x5D, 0xE7, 0x32, 0x56, 0xFA, 0x15, 0x3F, 0x41, 0xC3, 0x5E, 0xE2, 0x3D, 0x47, 0xC9, 0x40, 0xC0, 0x5B, 0xED, 0x2C, 0x74, 0x9C, 0xBF, 0xDA, 0x75, 0x9F, 0xBA, 0xD5, 0x64, 0xAC, 0xEF, 0x2A, 0x7E, 0x82, 0x9D, 0xBC, 0xDF, 0x7A, 0x8E, 0x89, 0x80, 0x9B, 0xB6, 0xC1, 0x58, 0xE8, 0x23, 0x65, 0xAF, 0xEA, 0x25, 0x6F, 0xB1, 0xC8, 0x43, 0xC5, 0x54, 0xFC, 0x1F, 0x21, 0x63, 0xA5, 0xF4, 0x07, 0x09, 0x1B, 0x2D, 0x77, 0x99, 0xB0, 0xCB, 0x46, 0xCA, 0x45, 0xCF, 0x4A, 0xDE, 0x79, 0x8B, 0x86, 0x91, 0xA8, 0xE3, 0x3E, 0x42, 0xC6, 0x51, 0xF3, 0x0E, 0x12, 0x36, 0x5A, 0xEE, 0x29, 0x7B, 0x8D, 0x8C, 0x8F, 0x8A, 0x85, 0x94, 0xA7, 0xF2, 0x0D, 0x17, 0x39, 0x4B, 0xDD, 0x7C, 0x84, 0x97, 0xA2, 0xFD, 0x1C, 0x24, 0x6C, 0xB4, 0xC7, 0x52, 0xF6, 0x01 };
        private static readonly byte[] GF256Log = new byte[] { 0x00, 0x00, 0x19, 0x01, 0x32, 0x02, 0x1A, 0xC6, 0x4B, 0xC7, 0x1B, 0x68, 0x33, 0xEE, 0xDF, 0x03, 0x64, 0x04, 0xE0, 0x0E, 0x34, 0x8D, 0x81, 0xEF, 0x4C, 0x71, 0x08, 0xC8, 0xF8, 0x69, 0x1C, 0xC1, 0x7D, 0xC2, 0x1D, 0xB5, 0xF9, 0xB9, 0x27, 0x6A, 0x4D, 0xE4, 0xA6, 0x72, 0x9A, 0xC9, 0x09, 0x78, 0x65, 0x2F, 0x8A, 0x05, 0x21, 0x0F, 0xE1, 0x24, 0x12, 0xF0, 0x82, 0x45, 0x35, 0x93, 0xDA, 0x8E, 0x96, 0x8F, 0xDB, 0xBD, 0x36, 0xD0, 0xCE, 0x94, 0x13, 0x5C, 0xD2, 0xF1, 0x40, 0x46, 0x83, 0x38, 0x66, 0xDD, 0xFD, 0x30, 0xBF, 0x06, 0x8B, 0x62, 0xB3, 0x25, 0xE2, 0x98, 0x22, 0x88, 0x91, 0x10, 0x7E, 0x6E, 0x48, 0xC3, 0xA3, 0xB6, 0x1E, 0x42, 0x3A, 0x6B, 0x28, 0x54, 0xFA, 0x85, 0x3D, 0xBA, 0x2B, 0x79, 0x0A, 0x15, 0x9B, 0x9F, 0x5E, 0xCA, 0x4E, 0xD4, 0xAC, 0xE5, 0xF3, 0x73, 0xA7, 0x57, 0xAF, 0x58, 0xA8, 0x50, 0xF4, 0xEA, 0xD6, 0x74, 0x4F, 0xAE, 0xE9, 0xD5, 0xE7, 0xE6, 0xAD, 0xE8, 0x2C, 0xD7, 0x75, 0x7A, 0xEB, 0x16, 0x0B, 0xF5, 0x59, 0xCB, 0x5F, 0xB0, 0x9C, 0xA9, 0x51, 0xA0, 0x7F, 0x0C, 0xF6, 0x6F, 0x17, 0xC4, 0x49, 0xEC, 0xD8, 0x43, 0x1F, 0x2D, 0xA4, 0x76, 0x7B, 0xB7, 0xCC, 0xBB, 0x3E, 0x5A, 0xFB, 0x60, 0xB1, 0x86, 0x3B, 0x52, 0xA1, 0x6C, 0xAA, 0x55, 0x29, 0x9D, 0x97, 0xB2, 0x87, 0x90, 0x61, 0xBE, 0xDC, 0xFC, 0xBC, 0x95, 0xCF, 0xCD, 0x37, 0x3F, 0x5B, 0xD1, 0x53, 0x39, 0x84, 0x3C, 0x41, 0xA2, 0x6D, 0x47, 0x14, 0x2A, 0x9E, 0x5D, 0x56, 0xF2, 0xD3, 0xAB, 0x44, 0x11, 0x92, 0xD9, 0x23, 0x20, 0x2E, 0x89, 0xB4, 0x7C, 0xB8, 0x26, 0x77, 0x99, 0xE3, 0xA5, 0x67, 0x4A, 0xED, 0xDE, 0xC5, 0x31, 0xFE, 0x18, 0x0D, 0x63, 0x8C, 0x80, 0xC0, 0xF7, 0x70, 0x07 };
        private static readonly byte[] ShiftRow = new byte[] { 0x0B, 0x02, 0x08, 0x0C, 0x01, 0x05, 0x00, 0x0F, 0x06, 0x07, 0x09, 0x03, 0x0D, 0x04, 0x0E, 0x0A, 0x04, 0x05, 0x07, 0x0A, 0x02, 0x0F, 0x0B, 0x08, 0x0E, 0x0D, 0x09, 0x06, 0x0C, 0x03, 0x00, 0x01, 0x08, 0x00, 0x0C, 0x06, 0x04, 0x0B, 0x07, 0x09, 0x05, 0x03, 0x0F, 0x01, 0x0D, 0x0A, 0x02, 0x0E };
        private static readonly byte[] Key = new byte[] { 0x48, 0x14, 0x36, 0xED, 0x8E, 0x44, 0x5B, 0xB6 };
        private static readonly byte[] Mul = new byte[] { 0xA7, 0x99, 0x66, 0x50, 0xB9, 0x2D, 0xF0, 0x78 };

        public Header Header;
        public StorageBlock[] BlocksInfo;
        public Node[] DirectoryInfo;
        public StreamFile[] FileList;
        private MemoryStream BlocksStream;

        private const uint mhy0HeaderMagic = 0xDADADAD9;
        private uint mhy0EntryMagic = mhy0HeaderMagic + 1;

        private int TotalSize => (int)DirectoryInfo.Sum(x => x.Size);
        private int MaxCompressedSize => BlocksInfo.Max(x => x.CompressedSize);
        private int TotalCompressedSize => BlocksInfo.Sum(x => x.CompressedSize);
        private int TotalUncompressedSize => BlocksInfo.Sum(x => x.UncompressedSize);

        public Mhy0(EndianReader reader)
        {
            ReadMetadata(reader);
            BlocksStream = CreateBlocksStream(false);
            ReadBlocks(reader);
            ReadNodes();
        }

        public Mhy0(Bundle bundle)
        {
            Header = bundle.Header;
            BlocksInfo = bundle.BlocksInfo;
            DirectoryInfo = bundle.DirectoryInfo;
            FileList = bundle.FileList;
        }

        public void Write(MemoryStream mhy0Stream)
        {
            BlocksStream = CreateBlocksStream(true);
            WriteNodes();
            WriteBlocks();
            WriteMetadata(mhy0Stream);
        }

        public void ReadMetadata(EndianReader reader)
        {
            var signature = reader.ReadStringToNull(4);
            if (signature != Signature)
            {
                throw new InvalidOperationException($"Expected signature mhy0, got {signature} instead !!");
            }

            Header = new Header()
            {
                Signature = Bundle.Signature,
                Version = 6,
                UnityVersion = "5.x.x",
                UnityRevision = "2017.4.30f1"
            };

            var buffer = ReadAndDecompressHeader(ref reader);
            ReadBlocksInfoAndDirectory(buffer);
        }

        private void WriteMetadata(MemoryStream mhy0Stream)
        {
            var buffer = WriteAndCompressBlocksInfo();
            ScrambleHeader(buffer);

            var writer = new EndianWriter();
            writer.WriteString(Signature);
            writer.WriteUInt8Array(buffer);
            writer.Write(BlocksStream.ToArray(), 0, TotalCompressedSize);
            BlocksStream.Dispose();
            mhy0Stream.Write(writer.ToArray());
        }

        public byte[] ReadAndDecompressHeader(ref EndianReader reader)
        {
            var headerSize = reader.ReadInt32();
            var header = reader.ReadBytes(headerSize);

            DescrambleHeader(header);
            var headerReader = new EndianReader(header);
            headerReader.Position += 0x20;
            var uncompressedSize = headerReader.ReadMhy0Int1();
            var compressed = headerReader.ReadRemaining();
            var uncompressed = new byte[uncompressedSize];

            var numWrite = LZ4Codec.Decode(compressed, uncompressed);
            if (numWrite != uncompressedSize)
            {
                throw new InvalidOperationException($"LZ4 error: Expected {uncompressedSize} bytes, got {numWrite} instead !!");
            }

            return uncompressed;
        }

        public byte[] WriteAndCompressBlocksInfo()
        {
            var buffer = WriteBlocksInfoAndDirectory();
            var uncompressedSize = buffer.Length;
            var uncompressed = buffer;

            var maxCompressedSize = LZ4Codec.MaximumOutputSize(uncompressedSize);
            var compressed = new byte[maxCompressedSize];
            var compressedSize = LZ4Codec.Encode(uncompressed, 0, uncompressedSize, compressed, 0, maxCompressedSize, LZ4Level.L06_HC);
            if (compressedSize == -1)
            {
                throw new Exception($"Lz4 decompression error, wrong compressed length: {compressedSize}");
            }

            var writer = new EndianWriter();
            writer.Write(mhy0HeaderMagic);
            writer.Write(Random.Shared.NextInt64());
            writer.Write(Random.Shared.NextInt64());
            writer.Write(Random.Shared.NextInt64());
            writer.Write(Random.Shared.Next());
            writer.WriteMhy0Int1(uncompressedSize);
            writer.Write(compressed, 0, compressedSize);

            return writer.ToArray();
        }

        private void ReadBlocksInfoAndDirectory(byte[] buffer)
        {
            var reader = new EndianReader(buffer);
            var bundleCount = reader.ReadMhy0Int2();
            DirectoryInfo = new Node[bundleCount];
            for (int i = 0; i < bundleCount; i++)
            {
                DirectoryInfo[i] = new Node
                {
                    Path = reader.ReadMhy0String(),
                    IsAssetFile = reader.ReadMhy0Bool(),
                    Offset = reader.ReadMhy0Int2(),
                    Size = reader.ReadMhy0Int1()
                };
            }

            var blockCount = reader.ReadMhy0Int2();
            BlocksInfo = new StorageBlock[blockCount];
            for (int i = 0; i < blockCount; i++)
            {
                BlocksInfo[i] = new StorageBlock
                {
                    CompressedSize = reader.ReadMhy0Int2(),
                    UncompressedSize = reader.ReadMhy0Int1()
                };
            }
        }
        private byte[] WriteBlocksInfoAndDirectory()
        {
            var writer = new EndianWriter();
            writer.WriteMhy0Int2(DirectoryInfo.Length);
            foreach (var node in DirectoryInfo)
            {
                writer.WriteMhy0String(node.Path);
                writer.WriteMhy0Bool(node.IsAssetFile);
                writer.WriteMhy0Int2((int)node.Offset);
                writer.WriteMhy0Int1((int)node.Size);
            }
        
            writer.WriteMhy0Int2(BlocksInfo.Length);
            foreach (var block in BlocksInfo)
            {
                writer.WriteMhy0Int2(block.CompressedSize);
                writer.WriteMhy0Int1(block.UncompressedSize);
            }
            return writer.ToArray();
        }

        private MemoryStream CreateBlocksStream(bool isProcess) => isProcess ? new(TotalSize) : new(TotalUncompressedSize);

        private void ReadBlocks(EndianReader reader)
        {
            var compressedBytes = new byte[MaxCompressedSize];
            var uncompressedBytes = new byte[BlockSize];
            foreach (var blockInfo in BlocksInfo)
            {
                var compressedSize = blockInfo.CompressedSize;
                reader.Read(compressedBytes, 0, compressedSize);
                if (compressedSize < 0x10)
                {
                    throw new InvalidOperationException($"LZ4 error: Invalid compressed length: {compressedSize}");
                }
                DescrambleEntry(compressedBytes);
                var uncompressedSize = blockInfo.UncompressedSize;
                var numWrite = LZ4Codec.Decode(compressedBytes, 0xC, compressedSize - 0xC, uncompressedBytes, 0, uncompressedSize);
                if (numWrite != uncompressedSize)
                {
                    throw new InvalidOperationException($"LZ4 error: Expected {uncompressedSize} bytes, got {numWrite} instead !!");
                }
                BlocksStream.Write(uncompressedBytes, 0, uncompressedSize);
            }
        }

        private void WriteBlocks()
        {
            var blocksInfo = new List<StorageBlock>();
            var totalUncompressedSize = TotalSize;
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

                var writer = new EndianWriter();
                writer.Write(mhy0EntryMagic++);
                writer.Write(Random.Shared.NextInt64());
                writer.Write(compressedBytes, 0, compressedSize);
                var buffer = writer.ToArray();
                ScrambleEntry(buffer);

                BlocksStream.Position = writePos;
                BlocksStream.Write(buffer, 0, buffer.Length);
                writePos = BlocksStream.Position;
                BlocksStream.Position = readPos;
        
                var blockInfo = new StorageBlock() { UncompressedSize = uncompressedSize, CompressedSize = buffer.Length };
                blocksInfo.Add(blockInfo);
        
                totalUncompressedSize -= uncompressedSize;
            }

            BlocksStream.Position = 0;
            BlocksInfo = blocksInfo.ToArray();
        }

        public void ReadNodes()
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
        private static int GF256Mul(int a, int b) => (a == 0 || b == 0) ? 0 : GF256Exp[(GF256Log[a] + GF256Log[b]) % 0xFF];
        private static int GF256Div(int a, int b) => (a == 0) ? 0 : GF256Exp[(0xFF + GF256Log[a] - GF256Log[b]) % 0xFF];
        private static void DescrambleChunk(byte[] input, int offset)
        {
            byte[] vector = new byte[0x10];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 0x10; j++)
                {
                    int k = ShiftRow[(2 - i) * 0x10 + j];
                    int idx = j % 8;
                    vector[j] = (byte)(Key[idx] ^ GI.SBox[(j % 4 * 0x100) | GF256Mul(Mul[idx], input[offset + k])]);
                }
                Buffer.BlockCopy(vector, 0, input, offset, 0x10);
            }
        }
        private static void ScrambleChunk(byte[] input, int offset)
        {
            byte[] vector = new byte[0x10];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 0x10; j++)
                {
                    int k = ShiftRow[i * 0x10 + j];
                    int idx = j % 8;
                    var reverseIV = Array.IndexOf(GI.SBox, (byte)(Key[idx] ^ input[offset + j]), j % 4 * 0x100, 0x100) % 0x100;
                    vector[k] = (byte)GF256Div(reverseIV, Mul[idx]);
                }
                Buffer.BlockCopy(vector, 0, input, offset, 0x10);
            }
        }
        private static void Descramble(byte[] input, ulong blockSize, ulong entrySize)
        {
            var roundedEntrySize = (int)((entrySize + 0xF) / 0x10 * 0x10);
            for (int i = 0; i < roundedEntrySize; i += 0x10)
            {
                DescrambleChunk(input, i + 4);
            }

            for (int i = 0; i < 4; i++)
            {
                input[i] ^= input[i + 4];
            }

            ulong currentEntry = (ulong)roundedEntrySize + 4;
            var finished = false;
            while (currentEntry < blockSize && !finished)
            {
                for (ulong i = 0; i < entrySize; i++)
                {
                    input[i + currentEntry] ^= input[i + 4];
                    if (i + currentEntry >= blockSize - 1)
                    {
                        finished = true;
                        break;
                    }
                }
                currentEntry += entrySize;
            }
        }
        private static void Scramble(byte[] input, ulong blockSize, ulong entrySize)
        {
            var roundedEntrySize = (int)((entrySize + 0xF) / 0x10 * 0x10);
            ulong currentEntry = (ulong)roundedEntrySize + 4;
            var finished = false;
            while (currentEntry < blockSize && !finished)
            {
                for (ulong i = 0; i < entrySize; i++)
                {
                    input[i + currentEntry] ^= input[i + 4];
                    if (i + currentEntry >= blockSize - 1)
                    {
                        finished = true;
                        break;
                    }
                }
                currentEntry += entrySize;
            }

            for (int i = 0; i < 4; i++)
            {
                input[i] ^= input[i + 4];
            }

            for (int i = 0; i < roundedEntrySize; i += 0x10)
            {
                ScrambleChunk(input, i + 4);
            }
        }
        public static void DescrambleHeader(byte[] input) => Descramble(input, 0x39, 0x1C);
        public static void DescrambleEntry(byte[] input) => Descramble(input, (ulong)Math.Min(input.Length, 0x21), 8);
        public static void ScrambleHeader(byte[] input) => Scramble(input, 0x39, 0x1C);
        public static void ScrambleEntry(byte[] input) => Scramble(input, (ulong)Math.Min(input.Length, 0x21), 8);
    } 
}