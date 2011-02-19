namespace Telerik.RazorConverter.Tests.Razor
{
    using Moq;
    using Telerik.RazorConverter.Razor.DOM;
    using Telerik.RazorConverter.Razor.Rendering;
    using Xunit;

    public class CommentNodeRendererTests
    {
        private readonly CommentNodeRenderer renderer;
        private readonly Mock<IRazorCommentNode> nodeMock;

        public CommentNodeRendererTests()
        {
            renderer = new CommentNodeRenderer();
            nodeMock = new Mock<IRazorCommentNode>();
        }

        [Fact]
        public void Should_support_comment_node()
        {
            renderer.CanRenderNode(nodeMock.Object).ShouldBeTrue();
        }

        [Fact]
        public void Should_not_support_generic_node()
        {
            renderer.CanRenderNode(new Mock<IRazorNode>().Object).ShouldBeFalse();
        }

        [Fact]
        public void Should_render_comment_with_razor_syntax()
        {
            nodeMock.SetupGet(n => n.Text).Returns("TEXT");

            renderer.RenderNode(nodeMock.Object).ShouldEqual("@*TEXT*@");
        }
    }
}
