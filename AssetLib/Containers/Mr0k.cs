namespace AssetLib.Containers
{
    public static class Mr0k
    {
        public static byte[] Decrypt(byte[] bytes, ref int size, byte[] expansionKey, byte[] key, byte[] constKey = null, byte[] sbox = null, byte[] blockKey = null)
        {
			var key1 = new byte[0x10];
			var key2 = new byte[0x10];
			var key3 = new byte[0x10];

			Buffer.BlockCopy(bytes, 4, key1, 0, key1.Length);
			Buffer.BlockCopy(bytes, 0x74, key2, 0, key2.Length);
			Buffer.BlockCopy(bytes, 0x84, key3, 0, key3.Length);

			var encryptedBlockSize = Math.Min(0x10 * ((size - 0x94) >> 7), 0x400);
			var encryptedBlock = new byte[encryptedBlockSize];

			Buffer.BlockCopy(bytes, 0x94, encryptedBlock, 0, encryptedBlockSize);

			if (constKey != null)
            {
				for (int i = 0; i < constKey.Length; i++)
					key2[i] ^= constKey[i];
			}

			if (sbox != null)
            {
				for (int i = 0; i < 0x10; i++)
					key1[i] = sbox[(i % 4 * 0x100) | key1[i]];
			}

			AES.Decrypt(key1, expansionKey);
			AES.Decrypt(key3, expansionKey);

			for (int i = 0; i < key1.Length; i++)
            {
				key1[i] ^= key3[i];
			}

			Buffer.BlockCopy(key1, 0, bytes, 0x84, key1.Length);

			var seed1 = BitConverter.ToUInt64(key2, 0);
			var seed2 = BitConverter.ToUInt64(key3, 0);
			var seed = seed2 ^ seed1 ^ (seed1 + (uint)size - 20);
			var seedBytes = BitConverter.GetBytes(seed);

			for (var i = 0; i < encryptedBlockSize; i++)
            {
				encryptedBlock[i] ^= (byte)(seedBytes[i % 8] ^ key[i]);
			}

			Buffer.BlockCopy(encryptedBlock, 0, bytes, 0x94, encryptedBlockSize);

			size -= 0x14;
			bytes = bytes.AsSpan(0x14).ToArray();

            if (blockKey != null)
            {
                for (int i = 0; i < 0xC00; i++)
                {
                    bytes[i] ^= blockKey[i % blockKey.Length];
                }

            }

            return bytes;
		}

        public static bool IsMr0k(byte[] bytes)
        {
			var reader = new EndianReader(bytes);
            var header = reader.ReadStringToNull(4);
            return header == "mr0k";
        }
    }
}
