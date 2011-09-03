namespace Telerik.RazorConverter.Razor.Rendering
{
    using System.Text;
    using Telerik.RazorConverter.Razor.DOM;

    public class SectionNodeRenderer : IRazorNodeRenderer
    {
        private IRazorNodeRendererProvider RendererProvider
        {
            get;
            set;
        }

        public SectionNodeRenderer(IRazorNodeRendererProvider NodeRendererProvider)
        {
            RendererProvider = NodeRendererProvider;
        }

        public string RenderNode(IRazorNode node)
        {
            var sectionNode = node as IRazorSectionNode;
            string childContent = null;
            
            if (!sectionNode.ShouldRender)
                childContent = RenderChildren(node);

            return (
                sectionNode.ShouldRender ?
                    string.Format("@RenderSection(\"{0}\", optional: true)", sectionNode.Name) :
                    string.Format("@section {0} {{\r\n{1}}}", sectionNode.Name, childContent) 
            );
        }

        private string RenderChildren(IRazorNode node)
        {
            var sb = new StringBuilder();
            foreach (var childNode in node.Children)
            {
                foreach (var renderer in RendererProvider.NodeRenderers)
                {
                    if (renderer.CanRenderNode(childNode))
                    {
                        sb.Append(renderer.RenderNode(childNode));
                        break;
                    }
                }
            }
            sb.AppendLine();
            return sb.ToString();
        }

        public bool CanRenderNode(IRazorNode node)
        {
            return node is IRazorSectionNode;
        }
    }
}
