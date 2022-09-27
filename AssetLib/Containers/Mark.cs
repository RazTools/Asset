namespace AssetLib.Containers
{
    public static class Mark
    {
        private const string Signature = "mark";
        private const int BlockSize = 0xA00;
        private const int ChunkSize = 0x264;
        private const int ChunkPadding = 4;

        private static readonly int BlockPadding = ((BlockSize / ChunkSize) + 1) * ChunkPadding;
        private static readonly int ChunkSizeWithPadding = ChunkSize + ChunkPadding;
        private static readonly int BlockSizeWithPadding = BlockSize + BlockPadding;

        private static readonly byte[] Key = new byte[] { 0x71, 0x98, 0xAA, 0xE6, 0xCE, 0x1B, 0x05, 0x4A, 0xE9, 0xFF, 0x45, 0x21, 0xC3, 0x38, 0x5E, 0x3C, 0x0F, 0xFB, 0xF5, 0xBB, 0xF6, 0x81, 0x48, 0x15, 0xFA, 0xD7, 0x77, 0x35, 0x82, 0x17, 0xD9, 0x9D, 0x56, 0x28, 0x2E, 0xA9, 0x51, 0xBA, 0x66, 0x2F, 0x22, 0xDD, 0xBB, 0x8A, 0x3B, 0xAD, 0x90, 0x63, 0xC6, 0x64, 0xFB, 0xD6, 0xCF, 0xA8, 0xBC, 0x48, 0x02, 0xC3, 0xBE, 0x36, 0xB2, 0x93, 0xBC, 0xD9 };

        public static byte[] Decrypt(EndianReader reader)
        {
            var signature = reader.ReadStringToNull(4);
            if (signature != Signature)
            {
                throw new InvalidOperationException($"Expected signature mark, got {signature} instead !!");
            }

            var block = new byte[BlockSizeWithPadding];
            var writer = new EndianWriter();
            while (reader.Length != reader.Position)
            {
                var blockSize = (int)Math.Min(reader.Remaining, BlockSizeWithPadding);
                var readBytes = reader.Read(block, 0, blockSize);
                if (readBytes != blockSize)
                {
                    throw new InvalidOperationException($"Expected {blockSize} but got {readBytes} !!");
                }

                var offset = 0;
                while (offset != blockSize)
                {
                    var chunkSize = Math.Min(readBytes, ChunkSizeWithPadding);
                    if (!(blockSize == BlockSizeWithPadding || chunkSize == ChunkSizeWithPadding))
                    {
                        writer.Write(block, offset, chunkSize);
                    }
                    else
                    {
                        writer.Write(block, offset, chunkSize - ChunkPadding);
                    } 
                    readBytes -= chunkSize;
                    offset += chunkSize;
                }
            }
            var buffer = writer.ToArray();
            XORBlock(buffer);
            return buffer;
        }

        public static byte[] Encrypt(byte[] inBuffer)
        {
            var blockCount = inBuffer.Length / BlockSize;

            XORBlock(inBuffer);
            var writer = new EndianWriter();
            writer.WriteString(Signature);

            var curBlock = 0;
            while (!(curBlock > blockCount))
            {
                var blockOffset = curBlock * BlockSize;
                var blockSize = Math.Min(inBuffer.Length - blockOffset, BlockSize);

                var chunkCount = blockSize / ChunkSize;
                var curChunk = 0;
                while (!(curChunk > chunkCount))
                {
                    var chunkOffset = curChunk * ChunkSize;
                    var chunkSize = Math.Min(blockSize - chunkOffset, ChunkSize);
                    writer.Write(inBuffer, blockOffset + chunkOffset, chunkSize);
                    if (blockSize == BlockSize || chunkSize == ChunkSize)
                    {
                        writer.Write(Random.Shared.Next());
                    }
                    curChunk++;
                }
                curBlock++;
            }
            return writer.ToArray();
        }

        private static void XORBlock(byte[] buffer)
        {
            for (int j = 0; j < buffer.Length; j++)
            {
                buffer[j] ^= Key[j % Key.Length];
            }
        }
    }
}
