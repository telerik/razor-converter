namespace Telerik.RazorConverter.WebForms.DOM
{
    public class ServerControlNode : WebFormsNode, IWebFormsServerControlNode
    {
        public string TagName
        {
            get;
            set;
        }

        public ServerControlNode()
        {
            Type = NodeType.ServerControl;
        }
    }
}
