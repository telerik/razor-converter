namespace Telerik.RazorConverter.Razor.DOM
{
    public interface IRazorCodeNodeFactory
    {
        IRazorCodeNode CreateCodeNode(string code, bool requiresPrefix, bool requiresBlock);
    }
}
