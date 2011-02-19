namespace Telerik.RazorConverter.Tests.Razor.Converters
{
    using Moq;
    using Telerik.RazorConverter.Razor.Converters;
    using Telerik.RazorConverter.Razor.DOM;
    using Telerik.RazorConverter.WebForms.DOM;
    using Xunit;

    public class CommentNodeConverterTests
    {
        private readonly CommentNodeConverter converter;
        private readonly Mock<IWebFormsCommentNode> commentNodeMock;
        private readonly Mock<IRazorCommentNodeFactory> nodeFactoryMock;

        public CommentNodeConverterTests()
        {
            nodeFactoryMock = new Mock<IRazorCommentNodeFactory>();
            converter = new CommentNodeConverter(nodeFactoryMock.Object);
            commentNodeMock = new Mock<IWebFormsCommentNode>();
        }               

        [Fact]
        public void Should_be_able_to_convert_comment_node()
        {
            converter.CanConvertNode(commentNodeMock.Object).ShouldBeTrue();
        }

        [Fact]
        public void Should_not_be_able_to_convert_generic_node()
        {
            converter.CanConvertNode(new Mock<IWebFormsNode>().Object).ShouldBeFalse();
        }

        [Fact]
        public void Should_convert_text_as_it_is()
        {
            commentNodeMock.Setup(n => n.Text).Returns(" TEXT ");
            nodeFactoryMock.Setup(f => f.CreateCommentNode(" TEXT ")).Verifiable();

            converter.ConvertNode(commentNodeMock.Object);

            nodeFactoryMock.Verify();
        }
    }
}
