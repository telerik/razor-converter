namespace Telerik.RazorConverter.Razor.DOM
{
    using System.ComponentModel.Composition;

    [Export(typeof(IRazorDirectiveNodeFactory))]
    public class RazorDirectiveNodeFactory : IRazorDirectiveNodeFactory
    {
        public IRazorDirectiveNode CreateDirectiveNode(string directive, string parameters)
        {
            return new RazorDirectiveNode { Directive = directive, Parameters = parameters };
        }
    }
}
