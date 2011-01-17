namespace Telerik.RazorConverter.Tests.Razor.Converters
{
    using Moq;
    using Telerik.RazorConverter.Razor.Converters;
    using Telerik.RazorConverter.Razor.DOM;
    using Telerik.RazorConverter.WebForms.DOM;
    using Xunit;

    public class TextNodeConverterTests
    {
        private readonly TextNodeConverter converter;
        private readonly Mock<IWebFormsTextNode> textNodeMock;
        private readonly Mock<IRazorTextNodeFactory> nodeFactoryMock;

        public TextNodeConverterTests()
        {
            nodeFactoryMock = new Mock<IRazorTextNodeFactory>();
            converter = new TextNodeConverter(nodeFactoryMock.Object);
            textNodeMock = new Mock<IWebFormsTextNode>();
        }               

        [Fact]
        public void Should_be_able_to_convert_text_node()
        {
            converter.CanConvertNode(textNodeMock.Object).ShouldBeTrue();
        }

        [Fact]
        public void Should_not_be_able_to_convert_generic_node()
        {
            converter.CanConvertNode(new Mock<IWebFormsNode>().Object).ShouldBeFalse();
        }

        [Fact]
        public void Should_convert_text_as_it_is()
        {
            textNodeMock.Setup(n => n.Text).Returns(" TEXT ");
            nodeFactoryMock.Setup(f => f.CreateTextNode(" TEXT ")).Verifiable();

            converter.ConvertNode(textNodeMock.Object);

            nodeFactoryMock.Verify();
        }
    }
}
