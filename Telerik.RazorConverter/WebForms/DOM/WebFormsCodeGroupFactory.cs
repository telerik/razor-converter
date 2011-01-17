namespace Telerik.RazorConverter.WebForms.DOM
{
    using System.ComponentModel.Composition;

    [Export(typeof(IWebFormsCodeGroupNodeFactory))]
    public class WebFormsCodeGroupFactory : IWebFormsCodeGroupNodeFactory
    {
        public IWebFormsCodeGroupNode CreatCodeGroupNode(IWebFormsCodeBlockNode firstCodeNode)
        {
            var groupNode = new WebFormsCodeGroupNode();
            groupNode.Children.Add(firstCodeNode);

            return groupNode;
        }
    }
}
