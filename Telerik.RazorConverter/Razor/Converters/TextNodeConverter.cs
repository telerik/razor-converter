namespace Telerik.RazorConverter.Razor.Converters
{
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using Telerik.RazorConverter.Razor.DOM;
    using Telerik.RazorConverter.WebForms.DOM;

    [Export(typeof(INodeConverter<IRazorNode>))]
    [ExportMetadata("Order", 40)]
    public class TextNodeConverter : INodeConverter<IRazorNode>
    {
        private IRazorTextNodeFactory TextNodeFactory
        {
            get;
            set;
        }

        [ImportingConstructor]
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
