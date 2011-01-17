namespace Telerik.RazorConverter.Razor.Rendering
{
    using Telerik.RazorConverter.Razor.DOM;

    public class ExpressionNodeRenderer : IRazorNodeRenderer
    {
        public string RenderNode(IRazorNode node)
        {
            var srcNode = node as IRazorExpressionNode;
            var formatString = "@{0}";
            var expression = srcNode.Expression;

            if (srcNode.IsMultiline)
            {
                formatString = "@({0})";
            }

            return string.Format(formatString, expression);
        }

        public bool CanRenderNode(IRazorNode node)
        {
            return node is IRazorExpressionNode;
        }
    }
}
