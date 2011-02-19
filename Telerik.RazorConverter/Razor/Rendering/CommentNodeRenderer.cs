namespace Telerik.RazorConverter.Razor.Rendering
{
    using Telerik.RazorConverter.Razor.DOM;

    public class CommentNodeRenderer : IRazorNodeRenderer
    {
        public string RenderNode(IRazorNode node)
        {
            var commentNode = node as IRazorCommentNode;
            return "@*" + commentNode.Text + "*@";
        }

        public bool CanRenderNode(IRazorNode node)
        {
            return node is IRazorCommentNode;
        }
    }
}
