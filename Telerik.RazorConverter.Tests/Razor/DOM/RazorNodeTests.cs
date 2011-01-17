namespace Telerik.RazorConverter.Tests.Razor.DOM
{
    using Telerik.RazorConverter.Razor.DOM;
    using Xunit;

    public class RazorNodeTests
    {
        private readonly RazorNode razorNode;

        public RazorNodeTests()
        {
            razorNode = new RazorNode();
        }

        [Fact]
        public void Should_set_parent_upon_adding()
        {
            var childNode = new RazorNode();
            razorNode.Children.Add(childNode);

            childNode.Parent.ShouldBeSameAs(razorNode);
        }
    }
}
