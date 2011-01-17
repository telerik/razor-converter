namespace Telerik.RazorConverter.Razor.DOM
{
    public class RazorExpressionNode : RazorNode, IRazorExpressionNode
    {
        public string Expression
        {
            get;
            set;
        }

        public bool IsMultiline
        {
            get;
            set;
        }
    }
}
