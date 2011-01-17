namespace Telerik.RazorConverter.Razor.Converters
{
    using System.Collections.Generic;
    using Telerik.RazorConverter.Razor.DOM;

    public interface IRazorNodeConverterProvider
    {
        IList<INodeConverter<IRazorNode>> NodeConverters { get; }
    }
}
