namespace aspx2razor
{
    using System;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Telerik.RazorConverter;
    using Telerik.RazorConverter.Razor.DOM;

    class Program
    {
        [Import]
        private IWebFormsParser Parser
        {
            get;
            set;
        }

        [Import]
        private IWebFormsConverter<IRazorNode> Converter
        {
            get;
            set;
        }

        [Import]
        private IRenderer<IRazorNode> Renderer
        {
            get;
            set;
        }

        private static void Main(string[] args)
        {
            var p = new Program();
            p.Run(args);
        }

        private Program()
        {
            var catalog = new AssemblyCatalog(typeof(IWebFormsParser).Assembly);
            var container = new CompositionContainer(catalog);
            container.ComposeParts(this);
        }

        public void Run(string[] args)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            if (args.Length < 1)
            {
                DisplayUsage();
                return;
            }

            var outputDirectory = (args.Length >= 2 && !args[1].StartsWith("-")) ? args[1] : "";
            var directoryHandler = new DirectoryHandler(args[0], outputDirectory);

            var recursive = args.Contains("-r", StringComparer.InvariantCultureIgnoreCase);
            var files = directoryHandler.GetFiles(recursive);
            foreach (var file in files)
            {
                Console.WriteLine("Converting {0}", file);
                
                var webFormsPageSource = File.ReadAllText(file, Encoding.UTF8);
                var webFormsDocument = Parser.Parse(webFormsPageSource);
                var razorDom = Converter.Convert(webFormsDocument);
                var razorPage = Renderer.Render(razorDom);

                var outputFileName = ReplaceExtension(directoryHandler.GetOutputFileName(file), ".cshtml");
                Console.WriteLine("Writing    {0}", outputFileName);
                EnsureDirectory(Path.GetDirectoryName(outputFileName));
                File.WriteAllText(outputFileName, razorPage, Encoding.UTF8);

                Console.WriteLine("Done\n");
            }

            var elapsed = stopwatch.Elapsed;
            Console.WriteLine();
            Console.WriteLine("{0} files converted", files.Length);
            Console.WriteLine("Elapsed: {0} seconds", elapsed.TotalSeconds);
        }

        private static void DisplayUsage()
        {
            Console.WriteLine("Converts WebForms pages (.aspx, .ascx) into a Razor views (.cshtml)");
            Console.WriteLine("Usage: aspx2razor <input file / wildcard> [output-directory] [options]");
            Console.WriteLine("Options available:\r");
            Console.WriteLine("-r: Convert directories and their contents recursively");
        }

        private static void EnsureDirectory(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        private static string ReplaceExtension(string fileName, string newExtension)
        {
            var targetFolder = Path.GetDirectoryName(fileName);
            return Path.Combine(targetFolder, Path.GetFileNameWithoutExtension(fileName) + newExtension);
        }
    }
}
