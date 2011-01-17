namespace Telerik.RazorConverter.WebForms.Filters
{
    using System.Collections.Generic;
    using Telerik.RazorConverter.WebForms.DOM;

    public interface IWebFormsNodeFilter
    {
        IList<IWebFormsNode> Filter(IWebFormsNode node, IWebFormsNode previousFilteredNode);
    }
}
