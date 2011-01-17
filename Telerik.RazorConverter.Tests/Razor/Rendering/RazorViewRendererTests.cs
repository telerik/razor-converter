namespace Telerik.RazorConverter.Tests.Razor
{
    using Moq;
    using System.Collections.Generic;
    using Telerik.RazorConverter;
    using Telerik.RazorConverter.Razor.DOM;
    using Telerik.RazorConverter.Razor.Rendering;
    using Xunit;

    public class RazorViewRendererTests
    {
        private readonly RazorViewRenderer viewRenderer;
        private readonly Mock<IDocument<IRazorNode>> documentMock;
        private readonly Mock<IRazorNode> rootNodeMock;
        private readonly Mock<IRazorNode> childNodeMock;
        private readonly Mock<IRazorNodeRenderer> firstNodeRenderer;
        private readonly Mock<IRazorNodeRenderer> secondNodeRenderer;
        private readonly IList<IRazorNodeRenderer> renderers;
        private readonly Mock<IRazorNodeRendererProvider> rendererProvider;

        public RazorViewRendererTests()
        {
            firstNodeRenderer = new Mock<IRazorNodeRenderer>();
            secondNodeRenderer = new Mock<IRazorNodeRenderer>();

            renderers = new List<IRazorNodeRenderer> { firstNodeRenderer.Object, secondNodeRenderer.Object };
            rendererProvider = new Mock<IRazorNodeRendererProvider>();
            rendererProvider.SetupGet(p => p.NodeRenderers).Returns(renderers);

            viewRenderer = new RazorViewRenderer(rendererProvider.Object);

            rootNodeMock = new Mock<IRazorNode>();
            childNodeMock = new Mock<IRazorNode>();
            rootNodeMock.SetupGet(n => n.Children).Returns(new IRazorNode[] { childNodeMock.Object });

            documentMock = new Mock<IDocument<IRazorNode>>();
            documentMock.SetupGet(d => d.RootNode).Returns(rootNodeMock.Object);
        }

        [Fact]
        public void Should_check_if_renderer_supports_child_node()
        {
            firstNodeRenderer.Setup(r => r.CanRenderNode(childNodeMock.Object)).Verifiable();
            viewRenderer.Render(documentMock.Object);
            firstNodeRenderer.Verify();
        }

        [Fact]
        public void Should_call_renderer_if_supports_child_node()
        {
            firstNodeRenderer.Setup(r => r.CanRenderNode(childNodeMock.Object)).Returns(true);
            firstNodeRenderer.Setup(r => r.RenderNode(childNodeMock.Object)).Verifiable();
            viewRenderer.Render(documentMock.Object);
            firstNodeRenderer.Verify();
        }
    }
}
