namespace Telerik.RazorConverter.Razor.DOM
{
    public class RazorDirectiveNode : RazorNode, IRazorDirectiveNode
    {
        public string Directive
        {
            get;
            set;
        }

        public string Parameters
        {
            get;
            set;
        }

        public RazorDirectiveNode()
        {
        }

        public RazorDirectiveNode(string directive, string parameters)
        {
            Directive = directive;
            Parameters = parameters;
        }
    }
}
