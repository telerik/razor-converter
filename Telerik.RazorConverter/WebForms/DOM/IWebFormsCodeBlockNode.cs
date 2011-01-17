namespace Telerik.RazorConverter.WebForms.DOM
{
    public interface IWebFormsCodeBlockNode : IWebFormsContentNode
    {
        string Code { get; set; }
        CodeBlockNodeType BlockType { get; }
    }
}
