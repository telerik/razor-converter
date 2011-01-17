namespace Telerik.RazorConverter.WebForms.DOM
{
    public class TextNode : WebFormsNode, IWebFormsTextNode
    {
        public string Text
        {
            get;
            set;
        }

        string IWebFormsContentNode.Content
        {
            get
            {
                return Text;
            }

            set
            {
                Text = value;
            }
        }

        public TextNode()
        {
            Type = NodeType.Text;
        }
    }
}
