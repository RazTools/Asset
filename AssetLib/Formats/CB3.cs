namespace AssetLib.Formats
{
    public class CB3 : Format
    {
        private const string Signature = "blk";

        private static readonly byte[] ExpansionKey = new byte[] { 0x3C, 0x5E, 0xAD, 0x0F, 0xD5, 0x09, 0x27, 0x3F, 0xB8, 0x70, 0x00, 0x9A, 0xCD, 0x30, 0x1B, 0xEB, 0xB7, 0x04, 0x71, 0xD9, 0x39, 0x80, 0x21, 0x29, 0xB5, 0xCC, 0x7A, 0xB2, 0xAE, 0xB6, 0x75, 0x14, 0x63, 0x2A, 0x82, 0x34, 0x70, 0xA5, 0x40, 0xEB, 0xF9, 0x4E, 0x95, 0x1C, 0x0A, 0xA4, 0xD0, 0xF6, 0x56, 0x1E, 0x0E, 0xE3, 0x7B, 0xBF, 0x0D, 0xC5, 0xD3, 0x04, 0xF3, 0x43, 0xDA, 0x76, 0x37, 0xDD, 0xAD, 0xE9, 0xF6, 0x97, 0x54, 0xD2, 0x56, 0xA2, 0x00, 0xBE, 0x96, 0xD0, 0x61, 0x4F, 0x8A, 0xBE, 0x5C, 0x32, 0x74, 0xC8, 0xFD, 0x7F, 0x2C, 0xFC, 0x5D, 0x4E, 0xD0, 0x6B, 0x2A, 0x2B, 0xF8, 0xDE, 0x12, 0x5B, 0xA2, 0x58, 0x8C, 0x4E, 0x02, 0xE5, 0x3C, 0xA6, 0xDB, 0x02, 0xBF, 0xAA, 0xE5, 0x12, 0xE0, 0xEF, 0x09, 0x36, 0xF6, 0xA0, 0xE5, 0x60, 0xE1, 0x62, 0xE4, 0x54, 0x02, 0xA7, 0xD1, 0x71, 0xC0, 0xF6, 0xE0, 0xFF, 0xDD, 0x01, 0xBA, 0xD5, 0x26, 0x94, 0x2D, 0x85, 0xA3, 0x7D, 0xDF, 0x0F, 0x94, 0x2F, 0xD6, 0x39, 0xE6, 0xEC, 0xCA, 0x86, 0x73, 0xD5, 0x66, 0x6A, 0x98, 0x92, 0x86, 0xCE, 0x20, 0xB4, 0xF0, 0x4C, 0xAA, 0xDD, 0x5A, 0xD5, 0x78, 0x2C, 0x81, 0xBE, 0xAE, 0x3A, 0x31, 0x14 };
        private static readonly byte[] ConstKey = new byte[] { 0xA2, 0x25, 0x25, 0x99, 0xB7, 0x62, 0xF4, 0x39, 0x28, 0xE1, 0xB7, 0x73, 0x91, 0x05, 0x25, 0x87 };
        private static readonly ulong Const = 0xCEAC3B5A867837AC;

        public CB3()
        {
            Name = "CB3";
            DisplayName = "GI_CB3";
            Pattern = ("*.blk", "*.unity3d");
            Extension = (".unity3d", ".blk");
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
            var files = isEncrypt ? CollectDirectories(input) : Directory.GetFiles(input, Pattern.Item1, SearchOption.AllDirectories);
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

        private string[] CollectDirectories(string input)
        {
            var blkDirs = new List<string>();
            var dirInfo = new DirectoryInfo(input);
            var dirs = dirInfo.GetDirectories("*.*", SearchOption.AllDirectories);
            foreach (var item in dirs)
            {
                var files = item.GetFiles();
                if (files.Length == 0 || files.Any(x => !(x.Extension == Extension.Item1)))
                {
                    continue;
                }
                blkDirs.Add(item.FullName);
            }
            return blkDirs.ToArray();
        }

        protected override void Decrypt(string input, string output)
        {
            var reader = new EndianReader(input);
            var signature = reader.ReadStringToNull();
            if (signature != Signature)
            {
                throw new InvalidOperationException($"Expected signature blk, got {signature} instead !!");
            }

            var blk = new Blk(reader, ExpansionKey, ConstKey, Const);
            var buffer = blk.Decrypt(reader);
            reader = new EndianReader(buffer, 0, EndianType.BigEndian);
            var i = 0;
            while (reader.Remaining > 0)
            {
                signature = reader.ReadStringToNull();
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
            var dir = new DirectoryInfo(input);
            var files = dir.GetFiles(Pattern.Item2, SearchOption.TopDirectoryOnly);
            var bundles = files.OrderBy(x => int.Parse(Path.GetFileNameWithoutExtension(x.FullName))).ToList();
            using var stream = new MemoryStream();
            foreach (var b in bundles)
            {
                using var bundleReader = new EndianReader(b.FullName, 0, EndianType.BigEndian);
                var header = new Header()
                {
                    Signature = bundleReader.ReadStringToNull(),
                    Version = bundleReader.ReadUInt32(),
                    UnityVersion = bundleReader.ReadStringToNull(),
                    UnityRevision = bundleReader.ReadStringToNull(),
                    Size = bundleReader.ReadInt64(),
                    CompressedBlocksInfoSize = bundleReader.ReadInt32(),
                    UncompressedBlocksInfoSize = bundleReader.ReadInt32(),
                    Flags = bundleReader.ReadInt32(),
                };
                var bundle = new Bundle(header, true);
                bundle.Process(bundleReader);
                bundle.WriteToStream(stream);
            }
            var reader = new EndianReader(stream);
            var writer = new EndianWriter();
            var blk = new Blk((int)reader.Length, ExpansionKey, ConstKey, Const);
            blk.Encrypt((ulong)Random.Shared.NextInt64(), writer, reader);
            WriteOutput(output, writer.ToArray());
        }
    }
}
