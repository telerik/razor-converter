namespace Telerik.RazorConverter.Razor.Converters
{
    using System.ComponentModel.Composition;

    [Export(typeof(IContentTagConverterConfiguration))]
    public class ContentTagConverterConfiguration : IContentTagConverterConfiguration
    {
        public string BodyContentPlaceHolderID
        {
            get;
            set;
        }

        public ContentTagConverterConfiguration()
        {
            BodyContentPlaceHolderID = "MainContent";
        }
    }
}
