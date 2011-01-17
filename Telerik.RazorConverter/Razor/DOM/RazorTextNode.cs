namespace Telerik.RazorConverter.Razor.DOM
{
    public class RazorTextNode : RazorNode, IRazorTextNode
    {
        public string Text
        {
            get;
            set;
        }

        public RazorTextNode()
        {
        }

        public RazorTextNode(string text)
        {
            Text = text;
        }
    }
}
