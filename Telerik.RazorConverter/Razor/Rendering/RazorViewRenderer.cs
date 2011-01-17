namespace Telerik.RazorConverter.Razor.Rendering
{
    using System.ComponentModel.Composition;
    using System.Text;
    using Telerik.RazorConverter;
    using Telerik.RazorConverter.Razor.DOM;

    [Export(typeof(IRenderer<IRazorNode>))]
    public class RazorViewRenderer : IRenderer<IRazorNode>
    {
        private IRazorNodeRendererProvider RendererProvider
        {
            get;
            set;
        }

        [ImportingConstructor]
        public RazorViewRenderer(IRazorNodeRendererProvider NodeRendererProvider)
        {
            RendererProvider = NodeRendererProvider;
        }

        public string Render(IDocument<IRazorNode> document)
        {
            var sb = new StringBuilder();
            foreach (var node in document.RootNode.Children)
            {
                foreach (var renderer in RendererProvider.NodeRenderers)
                {
                    if (renderer.CanRenderNode(node))
                    {
                        sb.Append(renderer.RenderNode(node));
                        break;
                    }
                }
            }
            sb.AppendLine();
            return sb.ToString();
        }
    }
}
