namespace Telerik.RazorConverter.Razor.Rendering
{
    using Telerik.RazorConverter.Razor.DOM;

    public interface IRazorNodeRenderer
    {
        string RenderNode(IRazorNode node);
        bool CanRenderNode(IRazorNode node);
    }
}
