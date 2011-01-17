namespace Telerik.RazorConverter.Tests.Razor.Converters
{
    using Moq;
    using Telerik.RazorConverter.Razor.Converters;
    using Telerik.RazorConverter.Razor.DOM;
    using Telerik.RazorConverter.WebForms.DOM;
    using Xunit;

    public class CodeBlockConverterTests
    {
        private readonly CodeBlockConverter converter;
        private readonly Mock<IWebFormsCodeBlockNode> codeBlockMock;
        private readonly Mock<IRazorCodeNodeFactory> nodeFactoryMock;

        public CodeBlockConverterTests()
        {
            nodeFactoryMock = new Mock<IRazorCodeNodeFactory>();
            converter = new CodeBlockConverter(nodeFactoryMock.Object);
            codeBlockMock = new Mock<IWebFormsCodeBlockNode>();
        }

        [Fact]
        public void Should_be_able_to_convert_codeblock_node()
        {
            converter.CanConvertNode(codeBlockMock.Object).ShouldBeTrue();
        }

        [Fact]
        public void Should_not_be_able_to_convert_generic_node()
        {
            converter.CanConvertNode(new Mock<IWebFormsNode>().Object).ShouldBeFalse();
        }

        [Fact]
        public void Should_require_prefix_for_complete_code_block()
        {
            codeBlockMock.Setup(cb => cb.Code).Returns("if (true) { doSomething(); }");
            codeBlockMock.Setup(cb => cb.BlockType).Returns(CodeBlockNodeType.Complete);
            nodeFactoryMock.Setup(f => f.CreateCodeNode("if (true) { doSomething(); }", true, false)).Verifiable();

            converter.ConvertNode(codeBlockMock.Object);

            nodeFactoryMock.Verify();
        }

        [Fact]
        public void Should_require_prefix_for_opening_code_block()
        {
            codeBlockMock.Setup(cb => cb.Code).Returns("if (true) {");
            codeBlockMock.Setup(cb => cb.BlockType).Returns(CodeBlockNodeType.Opening);
            nodeFactoryMock.Setup(f => f.CreateCodeNode("if (true) {", true, false)).Verifiable();

            converter.ConvertNode(codeBlockMock.Object);

            nodeFactoryMock.Verify();
        }

        [Fact]
        public void Should_not_require_prefix_for_continued_code_block()
        {
            codeBlockMock.Setup(cb => cb.Code).Returns("} else {");
            codeBlockMock.Setup(cb => cb.BlockType).Returns(CodeBlockNodeType.Continued);
            nodeFactoryMock.Setup(f => f.CreateCodeNode("} else {", false, false)).Verifiable();

            converter.ConvertNode(codeBlockMock.Object);

            nodeFactoryMock.Verify();
        }

        [Fact]
        public void Should_not_require_prefix_for_closing_code_block()
        {
            codeBlockMock.Setup(cb => cb.Code).Returns("}");
            codeBlockMock.Setup(cb => cb.BlockType).Returns(CodeBlockNodeType.Closing);
            nodeFactoryMock.Setup(f => f.CreateCodeNode("}", false, false)).Verifiable();

            converter.ConvertNode(codeBlockMock.Object);

            nodeFactoryMock.Verify();
        }

        [Fact]
        public void Should_transform_RenderPartial_to_RenderPage()
        {
            codeBlockMock.Setup(cb => cb.Code).Returns(@"Html.RenderPartial(""AccessibilityValidation"");");

            codeBlockMock.Setup(cb => cb.BlockType).Returns(CodeBlockNodeType.Complete);
            nodeFactoryMock.Setup(f => f.CreateCodeNode(@"@Html.Partial(""AccessibilityValidation"")", false, false)).Verifiable();

            converter.ConvertNode(codeBlockMock.Object);

            nodeFactoryMock.Verify();
        }
    }
}
