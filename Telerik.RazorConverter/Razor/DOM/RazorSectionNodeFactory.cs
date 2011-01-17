namespace Telerik.RazorConverter.Razor.DOM
{
    using System.ComponentModel.Composition;

    [Export(typeof(IRazorSectionNodeFactory))]
    public class RazorSectionNodeFactory : IRazorSectionNodeFactory
    {
        public IRazorSectionNode CreateSectionNode(string sectionName)
        {
            return new RazorSectionNode(sectionName);
        }
    }
}
