namespace Telerik.RazorConverter.WebForms.Filters
{
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using Telerik.RazorConverter.WebForms.DOM;

    [Export(typeof(IWebFormsNodeFilterProvider))]
    public class WebFormsNodeFilterProvider : IWebFormsNodeFilterProvider
    {
        [ImportingConstructor]
        public WebFormsNodeFilterProvider(IWebFormsCodeGroupNodeFactory nodeFactory)
        {
            Filters = new IWebFormsNodeFilter[] {
                new CodeBlockGroupingFilter(nodeFactory),
                new AddBlockBracesFilter(),
                new WrapNestedTemplatesFilter()
            };
        }

        public IList<IWebFormsNodeFilter> Filters
        {
            get;
            private set;
        }
    }
}
