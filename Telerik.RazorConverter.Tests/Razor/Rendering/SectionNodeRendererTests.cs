namespace Telerik.RazorConverter.Tests.Razor.Rendering
{
    using Moq;
    using System.Collections.Generic;
    using Telerik.RazorConverter.Razor.DOM;
    using Telerik.RazorConverter.Razor.Rendering;
    using Xunit;

    public class SectionNodeRendererTests
    {
        private readonly SectionNodeRenderer renderer;
        private readonly Mock<IRazorNodeRenderer> nodeRendererMock;
        private readonly IList<IRazorNodeRenderer> nodeRenderers;
        private readonly Mock<IRazorNodeRendererProvider> rendererProviderMock;
        private readonly Mock<IRazorSectionNode> sectionNodeMock;

        public SectionNodeRendererTests()
        {
            nodeRendererMock = new Mock<IRazorNodeRenderer>();
            nodeRenderers = new List<IRazorNodeRenderer> { nodeRendererMock.Object };
            rendererProviderMock = new Mock<IRazorNodeRendererProvider>();
            rendererProviderMock.SetupGet(p => p.NodeRenderers).Returns(nodeRenderers);
            renderer = new SectionNodeRenderer(rendererProviderMock.Object);
            sectionNodeMock = new Mock<IRazorSectionNode>();
            sectionNodeMock.SetupGet(n => n.Name).Returns("HeadContent");
        }

        [Fact]
        public void Should_support_code_node()
        {
            renderer.CanRenderNode(sectionNodeMock.Object).ShouldBeTrue();
        }

        [Fact]
        public void Should_not_support_other_nodes()
        {
            renderer.CanRenderNode(new Mock<IRazorNode>().Object).ShouldBeFalse();
        }

        [Fact]
        public void Should_render_empty_section()
        {
            sectionNodeMock.SetupGet(n => n.Children).Returns(new IRazorNode[] {});
            renderer.RenderNode(sectionNodeMock.Object).ShouldEqual(
@"@section HeadContent {

}");
        }

        [Fact]
        public void Should_render_children_in_section()
        {
            nodeRendererMock.Setup(r => r.CanRenderNode(It.IsAny<IRazorNode>())).Returns(true);
            nodeRendererMock.Setup(r => r.RenderNode(It.IsAny<IRazorNode>())).Returns("Text");

            sectionNodeMock.SetupGet(n => n.Children).Returns(new IRazorNode[] { new Mock<IRazorNode>().Object });

            renderer.RenderNode(sectionNodeMock.Object).ShouldEqual(
@"@section HeadContent {
Text
}");
        }
    }
}
