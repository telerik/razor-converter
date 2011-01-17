namespace Telerik.RazorConverter.Tests.WebForms.Filters
{
    using Moq;
    using Telerik.RazorConverter.WebForms.DOM;
    using Telerik.RazorConverter.WebForms.Filters;
    using Xunit;

    public class WrapNestedTemplatesFilterTests
    {
        private Mock<IWebFormsCodeGroupNode> codeGroupNodeMock;

        private WrapNestedTemplatesFilter filter;

        public WrapNestedTemplatesFilterTests()
        {
            codeGroupNodeMock = new Mock<IWebFormsCodeGroupNode>();
            filter = new WrapNestedTemplatesFilter();
        }

        [Fact]
        public void Should_not_transform_generic_block()
        {
            var textNode = new Mock<IWebFormsNode>();
            
            var filterResult = filter.Filter(textNode.Object, null);
                        
            filterResult[0].ShouldBeSameAs(textNode.Object);
        }

        [Fact]
        public void Should_wrap_nested_template()
        {
            codeGroupNodeMock.SetupGet(g => g.Content)
                .Returns(@"ScriptRegistrar().OnDocumentReady(() => {----$$alert(1);----$$});");

            codeGroupNodeMock
                .SetupSet(g => g.Content = It.IsAny<string>())
                .Callback<string>(content => content.ShouldEqual("ScriptRegistrar().OnDocumentReady(@<text>----$$alert(1);----$$</text>);"));

            filter.Filter(codeGroupNodeMock.Object, null);

            codeGroupNodeMock.Verify();
        }
    }
}

