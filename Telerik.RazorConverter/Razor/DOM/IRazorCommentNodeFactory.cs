namespace Telerik.RazorConverter.Razor.DOM
{
    public interface IRazorCommentNodeFactory
    {
        IRazorCommentNode CreateCommentNode(string text);
    }
}
