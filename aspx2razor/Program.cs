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

            var outputDirectory = (args.Length >= 2 && !args[1].StartsWith("-")) ? args[1] : args[0];
            var directoryHandler = new DirectoryHandler(args[0], outputDirectory);

            var files = directoryHandler.GetFiles(args.Contains("-s"));

            if (files.Length == 0)
            {
                Console.WriteLine("No files found to convert");
                return;
            }

            foreach (var file in files)
            {

                var subfolder = directoryHandler.CalculateOutputSubfolder(file);

                Console.Write("Converting {0}... ", subfolder + Path.GetFileName(file));
                
                var webFormsPageSource = File.ReadAllText(file, Encoding.UTF8);
                var webFormsDocument = Parser.Parse(webFormsPageSource);
                var razorDom = Converter.Convert(webFormsDocument);
                var razorPage = Renderer.Render(razorDom);

                var outputFile = directoryHandler.CalculateOutputfilename(file, ".cshtml");
                File.WriteAllText(outputFile, razorPage, Encoding.UTF8);
                
                Console.WriteLine("done");
            }

            var elapsed = stopwatch.Elapsed;
            Console.Out.WriteLine();
            Console.Out.WriteLine("Statistics:");
            Console.Out.WriteLine("{0} files converted", files.Length);
            Console.Out.WriteLine("Total time: {0} seconds", elapsed.TotalSeconds);
            Console.Out.WriteLine("Avg time: {0} ms per file", Math.Round(elapsed.TotalMilliseconds / (double)files.Length, 2));

        }

        private void DisplayUsage()
        {
            Console.WriteLine("Converts WebForms pages (.aspx, .ascx) into a Razor views (.cshtml)");
            Console.WriteLine("Usage: aspx2razor <input file / wildcard> [output-directory] [Options]");
            Console.WriteLine("Options available:");
            Console.WriteLine("");
            Console.WriteLine("-s: Inspect all subdirectories");
        }
    }
}
