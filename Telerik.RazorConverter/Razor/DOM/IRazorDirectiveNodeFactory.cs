namespace Telerik.RazorConverter.Razor.DOM
{
    public interface IRazorDirectiveNodeFactory
    {
        IRazorDirectiveNode CreateDirectiveNode(string directive, string parameters);
    }
}
