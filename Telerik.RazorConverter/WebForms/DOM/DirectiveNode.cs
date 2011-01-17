namespace Telerik.RazorConverter.WebForms.DOM
{
    public class DirectiveNode : WebFormsNode, IWebFormsDirectiveNode
    {
        public DirectiveType Directive
        {
            get;
            set;
        }
    }
}
