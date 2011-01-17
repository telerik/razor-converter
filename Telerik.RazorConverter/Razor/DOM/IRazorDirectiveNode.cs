namespace Telerik.RazorConverter.Razor.DOM
{
    public interface IRazorDirectiveNode : IRazorNode
    {
        string Directive { get; }
        string Parameters { get; }
    }
}
