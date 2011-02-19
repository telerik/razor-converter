namespace Telerik.RazorConverter.Razor.DOM
{
    public class RazorCommentNode : RazorNode, IRazorCommentNode
    {
        public string Text
        {
            get;
            set;
        }

        public RazorCommentNode()
        {
        }

        public RazorCommentNode(string text)
        {
            Text = text;
        }
    }
}
