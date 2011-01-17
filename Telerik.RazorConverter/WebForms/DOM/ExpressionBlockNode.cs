namespace Telerik.RazorConverter.WebForms.DOM
{
    public class ExpressionBlockNode : WebFormsNode, IWebFormsExpressionBlockNode
    {
        public string Expression
        {
            get;
            set;
        }

        string IWebFormsContentNode.Content
        {
            get
            {
                return Expression;
            }

            set
            {
                Expression = value;
            }
        }

        public ExpressionBlockNode()
        {
            Type = NodeType.ExpressionBlock;
        }
    }
}
