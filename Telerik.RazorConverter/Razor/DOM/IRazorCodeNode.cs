namespace Telerik.RazorConverter.Razor.DOM
{
    public interface IRazorCodeNode : IRazorNode
    {
        string Code { get; }
        bool RequiresPrefix { get; }
        bool RequiresBlock { get; }
    }
}
