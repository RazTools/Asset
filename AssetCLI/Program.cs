namespace AssetCLI
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            var rootCommand = RegisterOptions();
            rootCommand.Invoke(args);
        }
        public static RootCommand RegisterOptions()
        {
            var optionsBinder = new OptionsBinder();
            var rootCommand = new RootCommand()
            {
                optionsBinder.Format,
                optionsBinder.IsEncrypt,
                optionsBinder.Input,
                optionsBinder.Output
            };

            rootCommand.SetHandler((Options o) =>
            {
                try
                {
                    var format = FormatManager.GetFormat(o.Format);
                    format.Process(o.Input.ToString(), o.Output.ToString(), o.IsEncrypt);
                }
                catch (NotImplementedException)
                {
                    Console.WriteLine("This feature is not supported yet !!");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.StackTrace);
                }
            }, optionsBinder);

            return rootCommand;
        }
    }

    public class Options
    {
        public string Format { get; set; }
        public bool IsEncrypt { get; set; }
        public FileInfo Input { get; set; }
        public DirectoryInfo Output { get; set; }
    }

    public class OptionsBinder : BinderBase<Options>
    {
        public readonly Option<string> Format;
        public readonly Option<bool> IsEncrypt;
        public readonly Argument<FileInfo> Input;
        public readonly Argument<DirectoryInfo> Output;

        public OptionsBinder()
        {
            Format = new Option<string>("--format", $"Specify Format.") { IsRequired = true };
            IsEncrypt = new Option<bool>("--encrypt", "Perform Ecnryption.");
            Input = new Argument<FileInfo>("input_path", "Input file/folder.").LegalFilePathsOnly();
            Output = new Argument<DirectoryInfo>("output_path", "Output folder.").LegalFilePathsOnly();

            Format.FromAmong(FormatManager.GetSupportedFormats());
        }

        protected override Options GetBoundValue(BindingContext bindingContext) =>
        new()
        {
            Format = bindingContext.ParseResult.GetValueForOption(Format),
            IsEncrypt = bindingContext.ParseResult.GetValueForOption(IsEncrypt),
            Input = bindingContext.ParseResult.GetValueForArgument(Input),
            Output = bindingContext.ParseResult.GetValueForArgument(Output)
        };
    }
}
