namespace Telerik.RazorConverter.Tests.Razor.Rendering
{
    using Moq;
    using Telerik.RazorConverter.Razor.DOM;
    using Telerik.RazorConverter.Razor.Rendering;
    using Xunit;

    public class CodeNodeRendererTests
    {
        private readonly CodeNodeRenderer renderer;
        private readonly Mock<IRazorCodeNode> codeNodeMock;

        public CodeNodeRendererTests()
        {
            renderer = new CodeNodeRenderer();
            codeNodeMock = new Mock<IRazorCodeNode>();
        }

        [Fact]
        public void Should_support_code_node()
        {
            renderer.CanRenderNode(codeNodeMock.Object).ShouldBeTrue();
        }

        [Fact]
        public void Should_not_support_other_nodes()
        {
            renderer.CanRenderNode(new Mock<IRazorNode>().Object).ShouldBeFalse();
        }

        [Fact]
        public void Should_prefix_code()
        {
            codeNodeMock.SetupGet(n => n.Code).Returns("if (true) { doSomething(); }");
            codeNodeMock.SetupGet(n => n.RequiresPrefix).Returns(true);

            renderer.RenderNode(codeNodeMock.Object).ShouldEqual("@if (true) { doSomething(); }");
        }

        [Fact]
        public void Should_trim_start_of_code_when_adding_prefix()
        {
            codeNodeMock.SetupGet(n => n.Code).Returns(" if (true) { doSomething(); }");
            codeNodeMock.SetupGet(n => n.RequiresPrefix).Returns(true);

            renderer.RenderNode(codeNodeMock.Object).ShouldEqual("@if (true) { doSomething(); }");
        }

        [Fact]
        public void Should_not_prefix_continued_code()
        {
            codeNodeMock.SetupGet(n => n.Code).Returns("} else {");
            codeNodeMock.SetupGet(n => n.RequiresPrefix).Returns(false);

            renderer.RenderNode(codeNodeMock.Object).ShouldEqual("} else {");
        }

        [Fact]
        public void Should_not_trim_start_of_continued_code()
        {
            codeNodeMock.SetupGet(n => n.Code).Returns(" }");
            codeNodeMock.SetupGet(n => n.RequiresPrefix).Returns(false);

            renderer.RenderNode(codeNodeMock.Object).ShouldEqual(" }");
        }

        [Fact]
        public void Should_add_block()
        {
            codeNodeMock.SetupGet(n => n.Code).Returns(" code ");
            codeNodeMock.SetupGet(n => n.RequiresBlock).Returns(true);

            renderer.RenderNode(codeNodeMock.Object).ShouldEqual("@{\r\n code \r\n}");
        }
    }
}
