namespace Telerik.RazorConverter.WebForms.Filters
{
    using System.Collections.Generic;

    public interface IWebFormsNodeFilterProvider
    {
        IList<IWebFormsNodeFilter> Filters { get; }
    }
}
