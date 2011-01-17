namespace Telerik.RazorConverter.Razor.DOM
{
    using System.ComponentModel.Composition;

    [Export(typeof(IRazorExpressionNodeFactory))]
    public class RazorExpressionNodeFactory : IRazorExpressionNodeFactory
    {
        public IRazorExpressionNode CreateExpressionNode(string expression, bool isMultiline)
        {
            return new RazorExpressionNode { Expression = expression, IsMultiline = isMultiline };
        }
    }
}
