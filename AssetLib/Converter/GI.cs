namespace AssetLib.Converter
{
    public static class GI
    {
        public static void Convert(Bundle bundle)
        {
            foreach(var file in bundle.FileList)
            {
                if (!Path.HasExtension(file.FileName))
                {
                    DecodeClassID(file.Stream);
                }
            }
        }

        private static void DecodeClassID(Stream stream)
        {
            var reader = new BinaryReader(stream, Encoding.UTF8, true);
            reader.BaseStream.Position += 0x27;
            var typeCount = reader.ReadUInt32();
            for(int i = 0; i < typeCount; i++)
            {
                var classID = reader.ReadInt32();
                if ((classID > 0xFFFF || classID <= 0x0) && !Enum.IsDefined(typeof(ClassIDType), classID))
                {
                    var classIDBytes = BitConverter.GetBytes(classID);
                    Array.Reverse(classIDBytes);
                    classID = BitConverter.ToInt32(classIDBytes, 0);
                    classID = (classID ^ 0x23746FBE) - 3;
                    classIDBytes = BitConverter.GetBytes(classID);
                    reader.BaseStream.Position -= 4;
                    reader.BaseStream.Write(classIDBytes);
                }
                reader.BaseStream.Position += classID == (int)ClassIDType.MonoBehaviour ? 0x23 : 0x13;
            }
            reader.BaseStream.Position = 0;
        }
    }
}
