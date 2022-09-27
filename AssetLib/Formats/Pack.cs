﻿namespace AssetLib.Formats
{
    public class Pack : Format
    {
        private const string PackSignature = "pack";
        private const int MaxEntrySize = 0x880;

        private static readonly byte[] ExpansionKey = new byte[] { 0x3C, 0x5E, 0xAD, 0x0F, 0xD5, 0x09, 0x27, 0x3F, 0xB8, 0x70, 0x00, 0x9A, 0xCD, 0x30, 0x1B, 0xEB, 0xB7, 0x04, 0x71, 0xD9, 0x39, 0x80, 0x21, 0x29, 0xB5, 0xCC, 0x7A, 0xB2, 0xAE, 0xB6, 0x75, 0x14, 0x63, 0x2A, 0x82, 0x34, 0x70, 0xA5, 0x40, 0xEB, 0xF9, 0x4E, 0x95, 0x1C, 0x0A, 0xA4, 0xD0, 0xF6, 0x56, 0x1E, 0x0E, 0xE3, 0x7B, 0xBF, 0x0D, 0xC5, 0xD3, 0x04, 0xF3, 0x43, 0xDA, 0x76, 0x37, 0xDD, 0xAD, 0xE9, 0xF6, 0x97, 0x54, 0xD2, 0x56, 0xA2, 0x00, 0xBE, 0x96, 0xD0, 0x61, 0x4F, 0x8A, 0xBE, 0x5C, 0x32, 0x74, 0xC8, 0xFD, 0x7F, 0x2C, 0xFC, 0x5D, 0x4E, 0xD0, 0x6B, 0x2A, 0x2B, 0xF8, 0xDE, 0x12, 0x5B, 0xA2, 0x58, 0x8C, 0x4E, 0x02, 0xE5, 0x3C, 0xA6, 0xDB, 0x02, 0xBF, 0xAA, 0xE5, 0x12, 0xE0, 0xEF, 0x09, 0x36, 0xF6, 0xA0, 0xE5, 0x60, 0xE1, 0x62, 0xE4, 0x54, 0x02, 0xA7, 0xD1, 0x71, 0xC0, 0xF6, 0xE0, 0xFF, 0xDD, 0x01, 0xBA, 0xD5, 0x26, 0x94, 0x2D, 0x85, 0xA3, 0x7D, 0xDF, 0x0F, 0x94, 0x2F, 0xD6, 0x39, 0xE6, 0xEC, 0xCA, 0x86, 0x73, 0xD5, 0x66, 0x6A, 0x98, 0x92, 0x86, 0xCE, 0xC5, 0x24, 0x91, 0x5A, 0x4F, 0x4D, 0x3B, 0xC3, 0x9D, 0xBC, 0xE0, 0xA8, 0x4B, 0xAA, 0x50, 0x02 };
        private static readonly byte[] Key = new byte[] { 0x5B, 0xF5, 0x87, 0x75, 0x53, 0x7F, 0xE6, 0x07, 0x36, 0x71, 0xCD, 0xF6, 0x54, 0x3F, 0x33, 0xEB, 0xC3, 0x23, 0xFE, 0x8F, 0x40, 0xC9, 0x91, 0xC3, 0xF0, 0x0C, 0x02, 0x43, 0xC4, 0xFA, 0xA5, 0x42, 0x87, 0xA0, 0x2D, 0x89, 0xB2, 0xC0, 0x24, 0x95, 0x67, 0x98, 0xE1, 0xB8, 0x3D, 0x5D, 0xF9, 0xD9, 0x05, 0xB3, 0x42, 0xE5, 0x0C, 0x18, 0x26, 0xE2, 0xD8, 0x0E, 0x86, 0x72, 0x68, 0xAA, 0xD5, 0x1B, 0x50, 0xCF, 0xA3, 0xC5, 0xBF, 0xFC, 0xCF, 0xA9, 0x27, 0x59, 0x6E, 0x74, 0xA6, 0x3B, 0xB1, 0x17, 0x75, 0x3E, 0x8A, 0x86, 0x1C, 0x1A, 0xCA, 0xED, 0xBC, 0x27, 0xB2, 0x72, 0x9A, 0x2C, 0x47, 0xA1, 0x8A, 0x7D, 0xFF, 0x3A, 0x25, 0x03, 0xD5, 0x79, 0x03, 0x4B, 0x3D, 0xA0, 0x1F, 0x47, 0x96, 0xB5, 0x0E, 0x59, 0xF3, 0x13, 0x80, 0x2A, 0xC8, 0x8A, 0xEA, 0xA3, 0x4D, 0xDE, 0xFD, 0xF5, 0x09, 0x96, 0x18, 0xA3, 0x51, 0x18, 0x2C, 0x05, 0x62, 0xF2, 0x07, 0x3B, 0x00, 0x92, 0x95, 0x9F, 0x8F, 0x81, 0x09, 0x17, 0x79, 0x07, 0xED, 0x8E, 0xF2, 0x84, 0xDE, 0xD3, 0x61, 0x24, 0x57, 0xD8, 0x94, 0x38, 0x77, 0xAB, 0x8B, 0x86, 0x24, 0xD8, 0x45, 0xD5, 0xF7, 0x90, 0x50, 0x86, 0x7B, 0x30, 0x5B, 0x0A, 0x59, 0x67, 0x23, 0x4D, 0x4A, 0x14, 0xE1, 0x61, 0x5B, 0x28, 0xB9, 0xEF, 0x1B, 0x06, 0x73, 0x9E, 0xBB, 0x6F, 0xD2, 0x81, 0x75, 0x63, 0x06, 0x97, 0x2A, 0xE0, 0xF3, 0x83, 0xC4, 0x9D, 0x4E, 0x3C, 0x99, 0x1D, 0xDD, 0xF1, 0x51, 0x64, 0x1E, 0x6A, 0x77, 0x0E, 0x05, 0x86, 0xE4, 0x01, 0xCE, 0x09, 0x45, 0x0E, 0xB1, 0xB2, 0x87, 0x8F, 0xF9, 0x4A, 0xEE, 0x37, 0xCD, 0x41, 0xB6, 0x11, 0xDC, 0x7A, 0xB0, 0xE3, 0x40, 0x31, 0xEF, 0x2C, 0x64, 0x92, 0x30, 0x31, 0x5B, 0x56, 0x36, 0x3C, 0xEF, 0xDA, 0x3E, 0x72, 0x8E, 0xA9, 0x7E, 0x1C, 0xDE, 0xF6, 0x01, 0x52, 0x8F, 0x5C, 0xF0, 0x0A, 0x05, 0x23, 0xD1, 0xEA, 0xC2, 0x56, 0x6F, 0x99, 0x20, 0x63, 0xEF, 0x4B, 0x9B, 0x14, 0x72, 0x6D, 0x24, 0x02, 0xB3, 0x4B, 0x5F, 0x9B, 0xB0, 0x0A, 0x60, 0x7F, 0x87, 0xA4, 0x86, 0xF7, 0x08, 0xD8, 0xFF, 0x4D, 0xBF, 0xA2, 0x7A, 0x63, 0x69, 0x8E, 0x83, 0x85, 0x2B, 0xE6, 0x02, 0x71, 0x1F, 0x28, 0x3F, 0x07, 0xBB, 0xBD, 0xB3, 0xE4, 0x06, 0x3C, 0x91, 0x52, 0x33, 0x08, 0x89, 0x3D, 0xA5, 0xEC, 0x16, 0x69, 0x4C, 0xAC, 0xD7, 0xA6, 0xA4, 0x16, 0x37, 0x1A, 0x77, 0x34, 0x48, 0xDF, 0x3E, 0x9D, 0x3A, 0x4E, 0xB7, 0x85, 0x9B, 0x3F, 0x02, 0xFA, 0xB2, 0xD7, 0x42, 0xE2, 0x72, 0xBB, 0xC4, 0x14, 0x1C, 0x55, 0xEA, 0x69, 0x6F, 0x45, 0x76, 0x1B, 0x5D, 0xB2, 0x56, 0xF6, 0xA0, 0xE9, 0xED, 0x53, 0xB7, 0x69, 0x32, 0x05, 0xBC, 0xDC, 0xEA, 0xE5, 0xD1, 0xF5, 0x59, 0xF9, 0xB2, 0x63, 0x7D, 0x86, 0xD1, 0x66, 0xF6, 0x15, 0x51, 0xC7, 0x91, 0xBF, 0xAA, 0x76, 0x51, 0x9A, 0x71, 0x31, 0xB5, 0x90, 0x18, 0xA7, 0x4F, 0x66, 0xA7, 0x4D, 0x17, 0x64, 0xE5, 0x86, 0xBE, 0xBD, 0xD5, 0x4F, 0x9A, 0x8F, 0xBE, 0x9F, 0x29, 0xE7, 0x3B, 0xB2, 0x54, 0x88, 0x7D, 0x7F, 0xAC, 0x03, 0xD5, 0x7D, 0x73, 0x63, 0x7B, 0x72, 0x78, 0x4B, 0x21, 0xD0, 0xA0, 0x08, 0x6F, 0xBA, 0xE1, 0xFA, 0x69, 0x72, 0x4C, 0x51, 0x60, 0xAB, 0xEE, 0xBD, 0x9A, 0x10, 0x81, 0x8D, 0xAE, 0x86, 0xEB, 0x18, 0x90, 0x5E, 0x7B, 0x27, 0x32, 0xAF, 0x00, 0xF5, 0xA4, 0xBD, 0x58, 0x0F, 0x16, 0xE4, 0x64, 0xD8, 0x8D, 0xE4, 0x83, 0x7B, 0x9A, 0x2B, 0xE1, 0xA1, 0xEC, 0xFD, 0x3B, 0xEF, 0x5C, 0x2A, 0x26, 0x4E, 0x56, 0x91, 0x04, 0x76, 0x4E, 0xF6, 0x2C, 0x37, 0x4E, 0xFA, 0xF6, 0xE5, 0x97, 0x54, 0x1D, 0x40, 0x26, 0xF7, 0x2B, 0xDD, 0x40, 0x9B, 0x7C, 0xEF, 0x7B, 0x18, 0x98, 0x6C, 0xBB, 0x03, 0x56, 0x7F, 0x60, 0xD8, 0xDA, 0x81, 0x74, 0xB2, 0x3C, 0x97, 0x20, 0x5F, 0xC8, 0x2E, 0xE9, 0xC5, 0xEA, 0x34, 0x77, 0xD3, 0xEF, 0x83, 0xBB, 0x8A, 0x62, 0x0E, 0xB8, 0xAA, 0xFD, 0x73, 0x6A, 0xB7, 0x02, 0xDB, 0xAC, 0x41, 0xB2, 0xF1, 0x02, 0xBF, 0x48, 0x0C, 0x1D, 0x14, 0x50, 0xC9, 0x28, 0x28, 0xCC, 0x19, 0x80, 0x02, 0xF7, 0x31, 0xD1, 0x51, 0x16, 0x02, 0xD9, 0x02, 0x2A, 0x6F, 0x71, 0x44, 0x05, 0xC5, 0x2C, 0xFA, 0x52, 0x2B, 0x77, 0x10, 0x95, 0x35, 0xB0, 0x5C, 0x67, 0x03, 0x42, 0x72, 0x51, 0x06, 0x7C, 0xBF, 0x8E, 0xDE, 0xA0, 0x20, 0xA8, 0x32, 0xEA, 0x7C, 0x62, 0x18, 0x2A, 0x58, 0x4F, 0x8C, 0xE8, 0x29, 0x2F, 0x4C, 0x5C, 0x9D, 0xCD, 0x15, 0x8A, 0x92, 0x00, 0x89, 0xAA, 0x5F, 0x10, 0x85, 0x5A, 0xE1, 0xDB, 0xBB, 0x9C, 0x71, 0x06, 0xCC, 0xF8, 0x18, 0x78, 0x09, 0x52, 0x13, 0x38, 0xB3, 0xFA, 0x23, 0x1C, 0x5D, 0x23, 0x58, 0x63, 0xA3, 0xE4, 0x37, 0x56, 0xBA, 0xD2, 0xAC, 0x86, 0x81, 0x70, 0x75, 0x9B, 0xC2, 0xEA, 0x82, 0x57, 0xC0, 0x82, 0x6F, 0x0C, 0x82, 0x48, 0x1B, 0x9E, 0xB3, 0xEA, 0x06, 0xA1, 0xBA, 0x6B, 0xC9, 0xC0, 0x50, 0x50, 0x6E, 0x41, 0x8E, 0xB6, 0xC0, 0x8E, 0xA5, 0xE7, 0xC7, 0x4D, 0xBD, 0x7A, 0x04, 0x81, 0x57, 0x08, 0x1D, 0xC3, 0xB4, 0x79, 0x41, 0x16, 0xC2, 0x00, 0x86, 0xEF, 0x48, 0x07, 0x4F, 0x60, 0xAC, 0xEB, 0x91, 0x5B, 0x5F, 0x84, 0x36, 0x75, 0xB3, 0xAE, 0x94, 0x12, 0xE6, 0xAF, 0x53, 0x80, 0x20, 0xDA, 0x28, 0x8D, 0x7D, 0x23, 0xA2, 0xF7, 0x56, 0xAD, 0xF9, 0x4C, 0x45, 0xF6, 0xC7, 0x8A, 0x78, 0xCE, 0xE7, 0x3D, 0xAA, 0xC9, 0x8B, 0x70, 0x53, 0x64, 0xEE, 0x2E, 0x3B, 0xED, 0xFB, 0x77, 0x61, 0xF8, 0x85, 0x4B, 0xFF, 0xA7, 0x20, 0x5F, 0x9B, 0x6E, 0x9D, 0x70, 0x9A, 0x56, 0x2A, 0xD3, 0xB3, 0xD0, 0xD4, 0x11, 0x9B, 0x36, 0xA8, 0x71, 0x6A, 0x95, 0x3C, 0xDD, 0x98, 0x8F, 0x04, 0x81, 0x8D, 0x78, 0x4C, 0xE0, 0x3A, 0x0F, 0x0F, 0xC0, 0xE2, 0x80, 0xC4, 0xEA, 0x40, 0xC7, 0x4F, 0x52, 0x8F, 0x73, 0x05, 0x5D, 0x53, 0xB0, 0x86, 0x8B, 0x8D, 0x64, 0xBD, 0x3B, 0xBA, 0xAD, 0x15, 0x59, 0xB2, 0x81, 0x36, 0x8B, 0xD5, 0x7B, 0xC9, 0xD6, 0x17, 0xEB, 0x7C, 0xA1, 0xEF, 0x1E, 0xC8, 0x40, 0xA7, 0x94, 0x73, 0x20, 0xB8, 0x4B, 0x8C, 0x23, 0xD7, 0x4A, 0x4B, 0x44, 0x5A, 0xB7, 0x61, 0xD1, 0xDC, 0xDB, 0xB7, 0x7C, 0xBF, 0x10, 0xA0, 0x0D, 0x5D, 0x98, 0xE6, 0x1B, 0x47, 0xCF, 0x99, 0x20, 0xC5, 0xBF, 0x02, 0x46, 0x5C, 0xE2, 0x1D, 0x8B, 0x86, 0x07, 0x1A, 0x6A, 0xFC, 0x2F, 0x98, 0x7F, 0xF4, 0x2A, 0xF8, 0x4C, 0x70, 0x9C, 0x49, 0x1C, 0x7E, 0xB3, 0x9E, 0x98, 0x02, 0x35, 0xE0, 0xC0, 0x9B, 0x63, 0xED, 0x52, 0x8D, 0x5A, 0x25, 0xF9, 0x36, 0xD1, 0x2C, 0x19, 0x8E, 0xE2, 0xA1, 0x82, 0x25, 0xAC, 0x7D, 0x52, 0x9D, 0xF1, 0x47, 0x77, 0xCE, 0x5E, 0x4F, 0x4F, 0x26, 0xE5, 0x36, 0xDB, 0xE5, 0x76, 0x03, 0x0B, 0x99, 0x5C, 0xE5, 0x9E, 0x20, 0x7E, 0xB4, 0xAD, 0xF6, 0xA6, 0xC4, 0x6A, 0x99, 0x11, 0x54, 0x75, 0x3D, 0x70, 0x77, 0xDF, 0x41, 0xEC, 0x0C, 0x6B, 0x9D, 0xB6, 0x1C, 0xA1, 0x7D, 0xF1, 0x6B, 0xF1, 0xF6, 0x58, 0x73, 0x56, 0xC6, 0x93, 0x58, 0xE0, 0xD6, 0xCD, 0x74, 0x07, 0xDD, 0xB6, 0xE4, 0x15, 0x97, 0x32, 0x02, 0x6E, 0x71, 0x8A, 0x1A, 0x4C, 0x9F, 0xFA, 0x61 };

        public Pack()
        {
            Name = "PACK";
            DisplayName = "GI_Distency";
            Pattern = ("*.blk", "");
            Extension = (".unity3d", "");
        }

        protected override (string, string)[] CollectPaths(string input, string output, bool isEncrypt)
        {
            if (File.Exists(input))
            {
                var extension = isEncrypt ? Extension.Item2 : "";
                var fileName = $"{Path.GetFileNameWithoutExtension(input)}{extension}";
                var outPath = Path.Combine(output, fileName);
                Directory.CreateDirectory(outPath);
                return new (string, string)[] { (input, outPath) };
            }
            var files = Directory.GetFiles(input, isEncrypt ? Pattern.Item2 : Pattern.Item1, SearchOption.AllDirectories);
            var paths = new List<(string, string)>();
            foreach (var file in files)
            {
                var relativePath = Path.GetRelativePath(input, file);
                relativePath = isEncrypt ? Path.ChangeExtension(relativePath, Extension.Item2) : Path.Combine(Path.GetDirectoryName(relativePath), Path.GetFileNameWithoutExtension(relativePath));
                var outPath = Path.Combine(output, relativePath);
                if (!isEncrypt)
                {
                    Directory.CreateDirectory(outPath);
                }
                paths.Add((file, outPath));
            }
            return paths.ToArray();
        }

        protected override void Decrypt(string input, string output)
        {
            var packs = new List<PackEntry>();
            var reader = new EndianReader(input);
            while (reader.Position != reader.Length)
            {
                PackEntry pack = new();
                var signature = reader.ReadStringToNull(PackSignature.Length);
                if (signature == PackSignature)
                {
                    pack.IsMr0k = reader.ReadBoolean();
                    pack.BlockSize = reader.ReadInt32();
                    pack.Data = reader.ReadBytes(pack.BlockSize);
                    packs.Add(pack);
                    if (pack.BlockSize >= MaxEntrySize - 0x80 && reader.Position != reader.Length)
                        reader.Position += MaxEntrySize - 9 - pack.BlockSize;
                    continue;
                }

                reader.Position -= PackSignature.Length;
                var pos = reader.Position;
                signature = reader.ReadStringToNull(Bundle.Signature.Length);
                if (signature == Bundle.Signature)
                {
                    var tempData = reader.ReadBytes(MaxEntrySize - 0x80);
                    var UnityFSBlock = tempData.Search(Bundle.Signature);
                    var PackBlock = tempData.Search(PackSignature);
                    var firstMatch = Math.Min(UnityFSBlock, PackBlock);
                    if (UnityFSBlock == int.MaxValue && PackBlock == int.MaxValue)
                        pack.BlockSize = (int)reader.Length;
                    else
                        pack.BlockSize = firstMatch + signature.Length;
                    reader.Position = pos;
                    pack.Data = reader.ReadBytes(pack.BlockSize);
                    pack.IsMr0k = false;
                    packs.Add(pack);
                    continue;
                }
                throw new InvalidOperationException($"Expected signature {PackSignature} or {Bundle.Signature}, got {signature} instead !!");
            }
            packs.ForEach(x => x.DecryptMr0k());
            var buffer = packs.SelectMany(x => x.Data).ToArray();
            reader = new EndianReader(buffer, 0, EndianType.BigEndian);
            var i = 0;
            while (reader.Remaining > 0)
            {
                var signature = reader.ReadStringToNull();
                if (signature != Bundle.Signature)
                {
                    throw new InvalidOperationException($"Expected signature UnityFS, got {signature} instead !!");
                }

                var header = new Header()
                {
                    Signature = signature,
                    Version = reader.ReadUInt32(),
                    UnityVersion = reader.ReadStringToNull(),
                    UnityRevision = reader.ReadStringToNull(),
                    Size = reader.ReadInt64(),
                    CompressedBlocksInfoSize = reader.ReadInt32(),
                    UncompressedBlocksInfoSize = reader.ReadInt32(),
                    Flags = reader.ReadInt32(),
                };
                var bundle = new Bundle(header, true);
                var outputPath = Path.Combine(output, i + Extension.Item1);
                bundle.Process(reader);
                bundle.WriteToFile(outputPath);
                i++;
            }
        }

        protected override void Encrypt(string input, string output)
        {
            throw new NotImplementedException();
        }

        public class PackEntry
        {
            public bool IsMr0k;
            public int BlockSize;
            public byte[] Data;

            public void DecryptMr0k()
            {
                if (IsMr0k)
                {
                    Data = Mr0k.Decrypt(Data, ref BlockSize, ExpansionKey, Key);
                }
            }
        }
    }
}