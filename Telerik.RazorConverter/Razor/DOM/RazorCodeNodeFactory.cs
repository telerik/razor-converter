namespace Telerik.RazorConverter.Razor.DOM
{
    using System.ComponentModel.Composition;

    [Export(typeof(IRazorCodeNodeFactory))]
    public class RazorCodeNodeFactory : IRazorCodeNodeFactory
    {
        public IRazorCodeNode CreateCodeNode(string code, bool requiresPrefix, bool requiresBlock)
        {
            return new RazorCodeNode { Code = code, RequiresPrefix = requiresPrefix, RequiresBlock = requiresBlock };
        }
    }
}
