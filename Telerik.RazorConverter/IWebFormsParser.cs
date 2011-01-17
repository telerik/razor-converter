namespace Telerik.RazorConverter
{
    using Telerik.RazorConverter.WebForms.DOM;

    public interface IWebFormsParser
    {
        IDocument<IWebFormsNode> Parse(string input);
    }
}
