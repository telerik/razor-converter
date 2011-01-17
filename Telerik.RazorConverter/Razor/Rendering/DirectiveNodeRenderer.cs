namespace Telerik.RazorConverter.Razor.Rendering
{
    using Telerik.RazorConverter.Razor.DOM;

    public class DirectiveNodeRenderer : IRazorNodeRenderer
    {
        public string RenderNode(IRazorNode node)
        {
            var directiveNode = node as IRazorDirectiveNode;
            return string.Format("@{0} {1}", directiveNode.Directive, directiveNode.Parameters).Trim();
        }

        public bool CanRenderNode(IRazorNode node)
        {
            return node is IRazorDirectiveNode;
        }
    }
}
