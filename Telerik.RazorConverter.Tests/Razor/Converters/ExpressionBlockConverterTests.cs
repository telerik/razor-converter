namespace Telerik.RazorConverter.Tests.Razor.Converters
{
    using Moq;
    using Telerik.RazorConverter.Razor.Converters;
    using Telerik.RazorConverter.Razor.DOM;
    using Telerik.RazorConverter.WebForms.DOM;
    using Xunit;

    public class ExpressionBlockConverterTests
    {
        private readonly ExpressionBlockConverter converter;
        private readonly Mock<IWebFormsExpressionBlockNode> expressionBlockMock;
        private readonly Mock<IRazorExpressionNodeFactory> nodeFactoryMock;

        public ExpressionBlockConverterTests()
        {
            nodeFactoryMock = new Mock<IRazorExpressionNodeFactory>();
            converter = new ExpressionBlockConverter(nodeFactoryMock.Object);

            expressionBlockMock = new Mock<IWebFormsExpressionBlockNode>();
        }

        [Fact]
        public void Should_be_able_to_convert_expressionlock_node()
        {
            Assert.True(converter.CanConvertNode(expressionBlockMock.Object));
        }

        [Fact]
        public void Should_extract_expression()
        {
            expressionBlockMock.Setup(cb => cb.Expression).Returns("DateTime.Now");
            nodeFactoryMock.Setup(f => f.CreateExpressionNode("DateTime.Now", false)).Verifiable();

            converter.ConvertNode(expressionBlockMock.Object);

            nodeFactoryMock.Verify();
        }

        [Fact]
        public void Should_trim_expression_whitespace()
        {
            expressionBlockMock.Setup(cb => cb.Expression).Returns("\t DateTime.Now ");
            nodeFactoryMock.Setup(f => f.CreateExpressionNode("DateTime.Now", false)).Verifiable();

            converter.ConvertNode(expressionBlockMock.Object);

            nodeFactoryMock.Verify();
        }

        [Fact]
        public void Should_preserve_expression_newlines()
        {
            expressionBlockMock.Setup(cb => cb.Expression).Returns(" DateTime.Now\r\n");
            nodeFactoryMock.Setup(f => f.CreateExpressionNode("DateTime.Now\r\n", true)).Verifiable();

            converter.ConvertNode(expressionBlockMock.Object);

            nodeFactoryMock.Verify();
        }

        [Fact]
        public void Should_recognize_multiline_expression_block()
        {
            expressionBlockMock.Setup(cb => cb.Expression).Returns("Html.Telerik().Grid(Model)\r\n.Name(\"Grid\")");
            nodeFactoryMock.Setup(f => f.CreateExpressionNode("Html.Telerik().Grid(Model)\r\n.Name(\"Grid\")", true)).Verifiable();

            converter.ConvertNode(expressionBlockMock.Object);

            nodeFactoryMock.Verify();
        }

        [Fact]
        public void Should_convert_ResolveUrl_to_UrlContent()
        {
            expressionBlockMock.Setup(cb => cb.Expression).Returns("ResolveUrl(\"x\")");
            nodeFactoryMock.Setup(f => f.CreateExpressionNode("Url.Content(\"x\")", false)).Verifiable();

            converter.ConvertNode(expressionBlockMock.Object);

            nodeFactoryMock.Verify();
        }

        [Fact]
        public void Should_wrap_HtmlDecode_in_Raw()
        {
            expressionBlockMock.Setup(cb => cb.Expression).Returns("HttpUtility.HtmlDecode(\"x()\")");
            nodeFactoryMock.Setup(f => f.CreateExpressionNode("Html.Raw(HttpUtility.HtmlDecode(\"x()\"))", false)).Verifiable();

            converter.ConvertNode(expressionBlockMock.Object);

            nodeFactoryMock.Verify();
        }

        [Fact]
        public void Should_remove_HtmlEncode()
        {
            expressionBlockMock.Setup(cb => cb.Expression).Returns("Html.Encode(\"x\")");
            nodeFactoryMock.Setup(f => f.CreateExpressionNode("\"x\"", false)).Verifiable();

            converter.ConvertNode(expressionBlockMock.Object);

            nodeFactoryMock.Verify();
        }

        [Fact]
        public void Should_remove_HttpUtility_HtmlEncode()
        {
            expressionBlockMock.Setup(cb => cb.Expression).Returns("HttpUtility.HtmlEncode(\"x\")");
            nodeFactoryMock.Setup(f => f.CreateExpressionNode("\"x\"", false)).Verifiable();

            converter.ConvertNode(expressionBlockMock.Object);

            nodeFactoryMock.Verify();
        }

        [Fact]
        public void Should_remove_cast_to_string_while_removing_HttpUtility_HtmlEncode()
        {
            expressionBlockMock.Setup(cb => cb.Expression).Returns("HttpUtility.HtmlEncode((string)\"x\")");
            nodeFactoryMock.Setup(f => f.CreateExpressionNode("\"x\"", false)).Verifiable();

            converter.ConvertNode(expressionBlockMock.Object);

            nodeFactoryMock.Verify();
        }
    }
}
