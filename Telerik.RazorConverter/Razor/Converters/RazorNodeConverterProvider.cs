namespace Telerik.RazorConverter.Razor.Converters
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using Telerik.RazorConverter;
    using Telerik.RazorConverter.Razor.DOM;

    [Export(typeof(IRazorNodeConverterProvider))]
    public class RazorNodeConverterProvider : IRazorNodeConverterProvider
    {
        [ImportMany]
        public IEnumerable<Lazy<INodeConverter<IRazorNode>, IOrderMetadata>> NodeConverterRegistrations
        {
            get;
            set;
        }

        private IList<INodeConverter<IRazorNode>> sortedNodeConverters;
        public IList<INodeConverter<IRazorNode>> NodeConverters
        {
            get
            {
                if (sortedNodeConverters == null)
                {
                    sortedNodeConverters = NodeConverterRegistrations.SortByOrder();
                }

                return sortedNodeConverters;
            }
        }
    }
}
