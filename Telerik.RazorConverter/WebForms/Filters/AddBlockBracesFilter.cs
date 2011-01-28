namespace Telerik.RazorConverter.WebForms.Filters
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using Telerik.RazorConverter.WebForms.DOM;

    public class AddBlockBracesFilter : IWebFormsNodeFilter
    {
        public IList<IWebFormsNode> Filter(IWebFormsNode node, IWebFormsNode previousFilteredNode)
        {
            var isCodeGroupNode = node is IWebFormsCodeGroupNode;
            var isCompleteCodeNode = node is IWebFormsCodeBlockNode && ((IWebFormsCodeBlockNode)node).BlockType == CodeBlockNodeType.Complete;
            if (isCodeGroupNode || isCompleteCodeNode)
            {
                var codeContentNode = node as IWebFormsContentNode;
                if (codeContentNode != null && RequiresBlock(codeContentNode.Content))
                {
                    codeContentNode.Content = string.Format("{{{0}}}", codeContentNode.Content);
                }
            }

            return new IWebFormsNode[] { node };
        }

        private bool RequiresBlock(string code)
        {
            var statementRegex = new Regex(
                @"^\s*(?<op>if|using|Html\.RenderPartial)\s*
            (?<param>\((?>[^()]+|\((?<Depth>)|\)(?<-Depth>))*(?(Depth)(?!))\)){1}\s*
            (;)?\s*
            (?<block>\{(?>[^{}]+|\{(?<Depth>)|\}(?<-Depth>))*(?(Depth)(?!))\})?\s*
            (else\s*
                (?<elseblock>\{(?>[^{}]+|\{(?<Depth>)|\}(?<-Depth>))*(?(Depth)(?!))\})?\s*
            )?\s*
            (?<extra>\S*) $",
                RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);

            var match = statementRegex.Match(code);
            bool matchesOperator = match.Groups["op"].Success;
            bool hasExtraStatements = match.Groups["extra"].Length > 0;
            return !matchesOperator || (matchesOperator && hasExtraStatements);
        }
    }
}
