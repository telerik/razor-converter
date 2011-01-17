namespace Telerik.RazorConverter.Tests.Razor.DOM
{
    using Telerik.RazorConverter.Razor.DOM;
    using Xunit;

    public class RazorDirectiveNodeFactoryTests
    {
        private readonly RazorDirectiveNodeFactory razorDirectiveNodeFactory;

        public RazorDirectiveNodeFactoryTests()
        {
            razorDirectiveNodeFactory = new RazorDirectiveNodeFactory();
        }

        [Fact]
        public void Should_set_directive()
        {
            var codeNode = razorDirectiveNodeFactory.CreateDirectiveNode("directive", "");
            codeNode.Directive.ShouldEqual("directive");
        }

        [Fact]
        public void Should_set_parameters()
        {
            var codeNode = razorDirectiveNodeFactory.CreateDirectiveNode("", " params ");
            codeNode.Parameters.ShouldEqual(" params ");
        }
    }
}
