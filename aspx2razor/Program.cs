namespace aspx2razor
{
    using System;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;
    using System.IO;
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
            if (args.Length < 1)
            {
                DisplayUsage();
                return;
            }

            var currentDirectory = Directory.GetCurrentDirectory();
            var outputDirectory = (args.Length == 2) ? args[1] : currentDirectory;
            foreach (var file in Directory.GetFiles(currentDirectory, args[0]))
            {
                Console.Write("Converting {0}... ", Path.GetFileName(file));
                
                var webFormsPageSource = File.ReadAllText(file, Encoding.UTF8);
                var webFormsDocument = Parser.Parse(webFormsPageSource);
                var razorDom = Converter.Convert(webFormsDocument);
                var razorPage = Renderer.Render(razorDom);

                var outputFile = Path.Combine(outputDirectory, Path.GetFileNameWithoutExtension(file) + ".cshtml");
                File.WriteAllText(outputFile, razorPage, Encoding.UTF8);
                
                Console.WriteLine("done");
            }

        }

        private void DisplayUsage()
        {
            Console.WriteLine("Converts WebForms pages (.aspx, .ascx) into a Razor views (.cshtml)");
            Console.WriteLine("Usage: aspx2razor <input file / wildcard> [output-directory]");
        }
    }
}
