namespace Telerik.RazorConverter
{
    using System.Collections.Generic;
    using Telerik.RazorConverter.WebForms.DOM;

    public interface INodeConverter<TOut>
    {
        IList<TOut> ConvertNode(IWebFormsNode node);
        bool CanConvertNode(IWebFormsNode node);
    }
}
