namespace Telerik.RazorConverter.Tests.Razor.DOM
{
    using Telerik.RazorConverter.Razor.DOM;
    using Xunit;

    public class RazorCommentNodeFactoryTests
    {
        private readonly RazorCommentNodeFactory razorCommentNodeFactory;

        public RazorCommentNodeFactoryTests()
        {
            razorCommentNodeFactory = new RazorCommentNodeFactory();
        }

        [Fact]
        public void Should_set_text()
        {
            var commentNode = razorCommentNodeFactory.CreateCommentNode("text");
            commentNode.Text.ShouldEqual("text");
        }
    }
}
