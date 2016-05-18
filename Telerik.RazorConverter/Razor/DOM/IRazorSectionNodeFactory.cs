namespace Telerik.RazorConverter.Razor.DOM
{
    public interface IRazorSectionNodeFactory
    {
        IRazorSectionNode CreateSectionNode(string name, bool shouldRender = false);
    }
}
