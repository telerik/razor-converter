namespace Telerik.RazorConverter.WebForms.Filters
{
    using System.Collections.Generic;
    using System.Linq;
    using Telerik.RazorConverter.WebForms.DOM;

    public class CodeBlockGroupingFilter : IWebFormsNodeFilter
    {
        private IWebFormsCodeGroupNodeFactory MultiPartNodeFactory
        {
            get;
            set;
        }

        public CodeBlockGroupingFilter(IWebFormsCodeGroupNodeFactory nodeFactory)
        {
            MultiPartNodeFactory = nodeFactory;
        }

        public IList<IWebFormsNode> Filter(IWebFormsNode node, IWebFormsNode previousFilteredNode)
        {
            var codeNode = node as IWebFormsCodeBlockNode;
            var lastGroup = FindLastOpenGroup(previousFilteredNode);
            var result = new List<IWebFormsNode>();

            if (lastGroup != null)
            {
                lastGroup.Children.Add(node);
            }
            else
            {
                if (codeNode != null && codeNode.BlockType == CodeBlockNodeType.Opening)
                {
                    result.Add(MultiPartNodeFactory.CreatCodeGroupNode(codeNode));
                }
                else
                {
                    result.Add(node);
                }
            }

            return result;
        }

        private IWebFormsCodeGroupNode FindLastOpenGroup(IWebFormsNode previousFilteredNode)
        {
            var groupNode = previousFilteredNode as IWebFormsCodeGroupNode;
            if (groupNode != null)
            {
                var codeBlocks = groupNode.Children.Where(c => c is IWebFormsCodeBlockNode);
                var openingBlocks = codeBlocks.Count(c => ((IWebFormsCodeBlockNode)c).BlockType == CodeBlockNodeType.Opening);
                var closingBlocks = codeBlocks.Count(c => ((IWebFormsCodeBlockNode)c).BlockType == CodeBlockNodeType.Closing);

                if (openingBlocks > 0 && openingBlocks != closingBlocks)
                {
                    return groupNode;
                }
            }

            return null;
        }
    }
}
