namespace RazorConverter.Core.Tests
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;
    using System.IO;
    using System.Linq;
    using Telerik.RazorConverter;
    using Telerik.RazorConverter.Razor.DOM;
    using Xunit;
    using Xunit.Extensions;

    public class IntegrationTests
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

        public IntegrationTests()
        {
            var catalog = new AssemblyCatalog(typeof(IWebFormsParser).Assembly);
            var container = new CompositionContainer(catalog);
            container.ComposeParts(this);
        }

        public static IEnumerable<object[]> TestFiles
        {
            get
            {
                var testCasesFolder = new DirectoryInfo("..\\..\\TestCases");
                foreach (var inputFile in testCasesFolder.EnumerateFiles("*.aspx.txt"))
                {
                    var referenceFileName = inputFile.FullName.Replace(".aspx.txt", ".cshtml");
                    yield return new object[] {
                        inputFile.Name,
                        File.ReadAllText(inputFile.FullName),
                        File.ReadAllText(referenceFileName)
                    };
                }
            }
        }

        [Theory]
        [PropertyData("TestFiles")]
        public void Should_produce_expected_result(string caseName, string inputFile, string expectedResult)
        {
            var webFormsDocument = Parser.Parse(inputFile);
            var razorDom = Converter.Convert(webFormsDocument);
            var actualResult = Renderer.Render(razorDom);

            // Compare only non-empty lines
            var actualLines = GetNonEmptyLines(actualResult);
            var referenceLines = GetNonEmptyLines(expectedResult);
            for (int lineNum = 0; lineNum < actualLines.Count; lineNum++)
            {
                (lineNum > referenceLines.Count - 1)
                    .ShouldBeFalse("Reference output is shorter than actual output");

                string annotatedFormatString = "line #{0}:{1}";
                var annotatedActualLine = string.Format(annotatedFormatString, lineNum, actualLines[lineNum].Trim());
                var annotatedReferenceLine = string.Format(annotatedFormatString, lineNum, referenceLines[lineNum].Trim());

                annotatedActualLine.ShouldEqual(annotatedReferenceLine);                    
            }

            // Length verification done last in order to show line by line comparision first
            actualLines.Count.ShouldEqual(referenceLines.Count);
        }

        private static IList<string> GetNonEmptyLines(string input)
        {
            return input.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                        .Where(l => !string.IsNullOrWhiteSpace(l))
                        .ToList();
        }
    }
}
