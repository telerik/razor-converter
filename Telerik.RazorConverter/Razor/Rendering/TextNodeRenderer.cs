namespace Telerik.RazorConverter.Razor.Rendering
{
    using Telerik.RazorConverter.Razor.DOM;

    public class TextNodeRenderer : IRazorNodeRenderer
    {
        public string RenderNode(IRazorNode node)
        {
            var textNode = node as IRazorTextNode;
            return textNode.Text;
        }

        public bool CanRenderNode(IRazorNode node)
        {
            return node is IRazorTextNode;
        }
    }
}
