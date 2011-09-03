namespace Telerik.RazorConverter.Razor.DOM
{
    public class RazorSectionNode : RazorNode, IRazorSectionNode
    {
        public string Name
        {
            get;
            set;
        }

        public bool ShouldRender 
        { 
            get; 
            set; 
        }

        public RazorSectionNode()
        {
        }

        public RazorSectionNode(string sectionName, bool shouldRender)
        {
            Name = sectionName;
            ShouldRender = shouldRender;
        }
    }
}
