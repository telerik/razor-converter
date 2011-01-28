namespace Telerik.RazorConverter.Razor.Converters
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using Telerik.RazorConverter.Razor.DOM;
    using Telerik.RazorConverter.WebForms.DOM;

    public class CodeBlockConverter : INodeConverter<IRazorNode>
    {
        private IRazorCodeNodeFactory CodeNodeFactory
        {
            get;
            set;
        }

        public CodeBlockConverter(IRazorCodeNodeFactory nodeFactory)
        {
            CodeNodeFactory = nodeFactory;
        }

        public IList<IRazorNode> ConvertNode(IWebFormsNode node)
        {
            var srcNode = node as IWebFormsCodeBlockNode;
            var requiresPrefix = srcNode.BlockType == CodeBlockNodeType.Complete || srcNode.BlockType == CodeBlockNodeType.Opening;

            // Not implemented: Detection requires analysis of the C# code to determine if it contains more than one statement
            // See CodeBlockConverterTests.Should_require_block_for_multi_statement_code_block
            var requiresBlock = false;

            var code = srcNode.Code;
            code = ReplaceRenderPartial(code);

            if (code.TrimStart().StartsWith("@"))
            {
                requiresPrefix = false;
            }

            var codeNode = CodeNodeFactory.CreateCodeNode(code, requiresPrefix, requiresBlock);

            return new IRazorNode[] { codeNode };
        }

        public bool CanConvertNode(IWebFormsNode node)
        {
            return node is IWebFormsCodeBlockNode;
        }

        private string ReplaceRenderPartial(string input)
        {
            var searchRegex = new Regex(@"Html.RenderPartial\((?<page>.*?)\);", RegexOptions.Singleline | RegexOptions.Multiline);
            return searchRegex.Replace(input, m =>
            {
                return string.Format("@Html.Partial({0})", m.Groups["page"].Value.Trim());
            });
        }
    }
}
