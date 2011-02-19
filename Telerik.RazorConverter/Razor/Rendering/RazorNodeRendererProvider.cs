namespace Telerik.RazorConverter.Razor.Rendering
{
    using System.Collections.Generic;
    using System.ComponentModel.Composition;

    [Export(typeof(IRazorNodeRendererProvider))]
    public class RazorNodeRendererProvider : IRazorNodeRendererProvider
    {
        public IList<IRazorNodeRenderer> NodeRenderers 
        { 
            get;
            private set;
        }

        public RazorNodeRendererProvider()
        {
            NodeRenderers = new List<IRazorNodeRenderer>
            {
                new DirectiveNodeRenderer(),
                new CodeNodeRenderer(),
                new ExpressionNodeRenderer(),
                new TextNodeRenderer(),
                new CommentNodeRenderer(),
                new SectionNodeRenderer(this)
            };
        }
    }
}
