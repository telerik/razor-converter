namespace Telerik.RazorConverter.Razor.Converters
{
    using System.Collections.Generic;
    using Telerik.RazorConverter.Razor.DOM;
    using Telerik.RazorConverter.WebForms.DOM;

    public class TextNodeConverter : INodeConverter<IRazorNode>
    {
        private IRazorTextNodeFactory TextNodeFactory
        {
            get;
            set;
        }

        public TextNodeConverter(IRazorTextNodeFactory nodeFactory)
        {
            TextNodeFactory = nodeFactory;
        }
        
        public IList<IRazorNode> ConvertNode(IWebFormsNode node)
        {
            var srcNode = node as IWebFormsTextNode;
            var destNode = TextNodeFactory.CreateTextNode(srcNode.Text);
            return new IRazorNode[] { destNode };
        }

        public bool CanConvertNode(IWebFormsNode node)
        {
            return node is IWebFormsTextNode;
        }
    }
}
