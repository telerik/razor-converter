namespace Telerik.RazorConverter.Tests.Razor
{
    using Moq;
    using Telerik.RazorConverter.Razor.DOM;
    using Telerik.RazorConverter.Razor.Rendering;
    using Xunit;

    public class TextNodeRendererTests
    {
        private readonly TextNodeRenderer renderer;
        private readonly Mock<IRazorTextNode> nodeMock;

        public TextNodeRendererTests()
        {
            renderer = new TextNodeRenderer();
            nodeMock = new Mock<IRazorTextNode>();
        }

        [Fact]
        public void Should_support_text_node()
        {
            renderer.CanRenderNode(nodeMock.Object).ShouldBeTrue();
        }

        [Fact]
        public void Should_not_support_generic_node()
        {
            renderer.CanRenderNode(new Mock<IRazorNode>().Object).ShouldBeFalse();
        }

        [Fact]
        public void Should_render_text_as_it_is()
        {
            nodeMock.SetupGet(n => n.Text).Returns("TEXT");

            renderer.RenderNode(nodeMock.Object).ShouldEqual("TEXT");
        }
    }
}
