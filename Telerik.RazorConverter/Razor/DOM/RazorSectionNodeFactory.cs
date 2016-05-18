namespace Telerik.RazorConverter.Razor.DOM
{
    using System.ComponentModel.Composition;

    [Export(typeof(IRazorSectionNodeFactory))]
    public class RazorSectionNodeFactory : IRazorSectionNodeFactory
    {
        public IRazorSectionNode CreateSectionNode(string sectionName, bool shouldRender = false)
        {
            return new RazorSectionNode(sectionName, shouldRender);
        }
    }
}
