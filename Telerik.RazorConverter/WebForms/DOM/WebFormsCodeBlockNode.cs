namespace Telerik.RazorConverter.WebForms.DOM
{
    public class WebFormsCodeBlockNode : WebFormsNode, IWebFormsCodeBlockNode
    {
        public string Code
        {
            get;
            set;
        }

        string IWebFormsContentNode.Content
        {
            get
            {
                return Code;
            }

            set
            {
                Code = value;
            }
        }

        public CodeBlockNodeType BlockType
        {
            get;
            set;
        }

        public WebFormsCodeBlockNode()
        {
            Type = NodeType.CodeBlock;
            BlockType = CodeBlockNodeType.Complete;
        }
    }
}
