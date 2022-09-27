namespace AssetLib.Formats
{
    public class CB1 : Format
    {
        public CB1()
        {
            Name = "CB1";
            DisplayName = "GI_CB1";
            Pattern = ("*.asb", "*.unity3d");
            Extension = (".unity3d", ".asb");
        }

        protected override void Decrypt(string input, string output)
        {
            var reader = new EndianReader(input);
            var buffer = Mark.Decrypt(reader);
            WriteOutput(output, buffer);
        }

        protected override void Encrypt(string input, string output)
        {
            var buffer = File.ReadAllBytes(input);
            buffer = Mark.Encrypt(buffer);
            WriteOutput(output, buffer);
        }
    }
}
