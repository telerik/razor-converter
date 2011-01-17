namespace Telerik.RazorConverter.Razor.Rendering
{
    using Telerik.RazorConverter.Razor.DOM;

    public class CodeNodeRenderer : IRazorNodeRenderer
    {
        public string RenderNode(IRazorNode node)
        {
            var srcNode = node as IRazorCodeNode;
            var prefix = "";
            var code = srcNode.Code;

            if (srcNode.RequiresBlock)
            {
                code = "{\r\n" + code + "\r\n}";
            }

            if (srcNode.RequiresPrefix || srcNode.RequiresBlock)
            {
                prefix = "@";
                code = code.TrimStart();
            }

            return prefix + code;
        }

        public bool CanRenderNode(IRazorNode node)
        {
            return node is IRazorCodeNode;
        }
    }
}
