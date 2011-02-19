namespace Telerik.RazorConverter.Razor.DOM
{
    using System.ComponentModel.Composition;

    [Export(typeof(IRazorCommentNodeFactory))]
    public class RazorCommentNodeFactory : IRazorCommentNodeFactory
    {
        public IRazorCommentNode CreateCommentNode(string text)
        {
            return new RazorCommentNode { Text = text };
        }
    }
}
