namespace Telerik.RazorConverter.Tests.Razor
{
    using Moq;
    using Telerik.RazorConverter.Razor.DOM;
    using Telerik.RazorConverter.Razor.Rendering;
    using Xunit;

    public class DirectiveNodeRendererTests
    {
        private readonly DirectiveNodeRenderer renderer;
        private readonly Mock<IRazorDirectiveNode> directiveNodeMock;

        public DirectiveNodeRendererTests()
        {
            renderer = new DirectiveNodeRenderer();
            directiveNodeMock = new Mock<IRazorDirectiveNode>();
        }

        [Fact]
        public void Should_be_able_to_render_directive_node()
        {
            renderer.CanRenderNode(directiveNodeMock.Object).ShouldBeTrue();
        }

        [Fact]
        public void Should_prefix_directive()
        {
            directiveNodeMock.SetupGet(n => n.Directive).Returns("inherits");
            renderer.RenderNode(directiveNodeMock.Object).ShouldEqual("@inherits");
        }

        [Fact]
        public void Should_render_parameters()
        {
            directiveNodeMock.SetupGet(n => n.Directive).Returns("inherits");
            directiveNodeMock.SetupGet(n => n.Parameters).Returns("MyViewDataType");
            renderer.RenderNode(directiveNodeMock.Object).ShouldEqual("@inherits MyViewDataType");
        }
    }
}
