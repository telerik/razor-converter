namespace Telerik.RazorConverter.Tests.Razor.DOM
{
    using Telerik.RazorConverter.Razor.DOM;
    using Xunit;

    public class RazorCodeNodeFactoryTests
    {
        private readonly RazorCodeNodeFactory razorCodeNodeFactory;

        public RazorCodeNodeFactoryTests()
        {
            razorCodeNodeFactory = new RazorCodeNodeFactory();
        }

        [Fact]
        public void Should_set_code()
        {
            var codeNode = razorCodeNodeFactory.CreateCodeNode(" CODE ", true, false);
            codeNode.Code.ShouldEqual(" CODE ");
        }

        [Fact]
        public void Should_set_requires_prefix()
        {
            var codeNode = razorCodeNodeFactory.CreateCodeNode("", true, false);
            codeNode.RequiresPrefix.ShouldEqual(true);
        }
    }
}
