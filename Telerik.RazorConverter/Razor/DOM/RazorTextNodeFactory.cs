namespace Telerik.RazorConverter.Razor.DOM
{
    using System.ComponentModel.Composition;

    [Export(typeof(IRazorTextNodeFactory))]
    public class RazorTextNodeFactory : IRazorTextNodeFactory
    {
        public IRazorTextNode CreateTextNode(string text)
        {
            return new RazorTextNode { Text = text };
        }
    }
}
