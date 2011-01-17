namespace Telerik.RazorConverter.WebForms.Filters
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using Telerik.RazorConverter;

    [Export(typeof(IWebFormsNodeFilterProvider))]
    public class WebFormsNodeFilterProvider : IWebFormsNodeFilterProvider
    {
        [ImportMany]
        public IEnumerable<Lazy<IWebFormsNodeFilter, IOrderMetadata>> FilterRegistrations
        {
            get;
            set;
        }
        
        private IList<IWebFormsNodeFilter> sortedNodeFilters;
        public IList<IWebFormsNodeFilter> Filters
        {
            get
            {
                if (sortedNodeFilters == null)
                {
                    sortedNodeFilters = FilterRegistrations.SortByOrder();
                }

                return sortedNodeFilters;
            }
        }
    }
}
