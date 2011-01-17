namespace Telerik.RazorConverter.WebForms.Parsing
{
    using System.Text.RegularExpressions;
    using Telerik.RazorConverter.WebForms.DOM;   

    public interface IWebFormsNodeFactory
    {
        IWebFormsNode CreateNode(Match match, NodeType type);
    }
}
