namespace Telerik.RazorConverter.WebForms.DOM
{
    public class CommentNode : WebFormsNode, IWebFormsCommentNode
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

        public CommentNode()
        {
            Type = NodeType.Comment;
        }
    }
}
