namespace Telerik.RazorConverter
{
    using Telerik.RazorConverter.WebForms.DOM;

    public interface IWebFormsConverter<TNode>
    {
        IDocument<TNode> Convert(IDocument<IWebFormsNode> rootNode);
    }
}
