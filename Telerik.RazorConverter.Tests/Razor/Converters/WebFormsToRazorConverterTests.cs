namespace Telerik.RazorConverter.Tests.Razor.Converters
{
    using Moq;
    using Telerik.RazorConverter;
    using Telerik.RazorConverter.Razor.Converters;
    using Telerik.RazorConverter.Razor.DOM;
    using Telerik.RazorConverter.WebForms.DOM;
    using Xunit;
    
    public class WebFormsToRazorConverterTests
    {
        private readonly WebFormsToRazorConverter converter;
        private readonly Mock<IDocument<IWebFormsNode>> documentMock;
        private readonly Mock<IWebFormsNode> childNodeMock;
        private readonly Mock<IRazorNodeConverterProvider> converterProviderMock;
        private readonly Mock<INodeConverter<IRazorNode>> firstNodeConverter;

        public WebFormsToRazorConverterTests()
        {
            firstNodeConverter = new Mock<INodeConverter<IRazorNode>>();

            converterProviderMock = new Mock<IRazorNodeConverterProvider>();
            converterProviderMock.SetupGet(c => c.NodeConverters)
                .Returns(new INodeConverter<IRazorNode>[] { firstNodeConverter.Object });
            converter = new WebFormsToRazorConverter(converterProviderMock.Object);

            childNodeMock = new Mock<IWebFormsNode>();
            var rootNodeMock = new Mock<IWebFormsNode>();
            rootNodeMock.SetupGet(n => n.Children).Returns(new IWebFormsNode[] { childNodeMock.Object });
            documentMock = new Mock<IDocument<IWebFormsNode>>();
            documentMock.SetupGet(d => d.RootNode).Returns(rootNodeMock.Object);        
        }

        [Fact]
        public void Should_check_if_converter_supports_child_node()
        {
            firstNodeConverter.Setup(c => c.CanConvertNode(childNodeMock.Object)).Verifiable();
            converter.Convert(documentMock.Object);
            firstNodeConverter.Verify();
        }

        [Fact]
        public void Should_call_converter_if_supports_child_node()
        {
            firstNodeConverter.Setup(c => c.CanConvertNode(childNodeMock.Object)).Returns(true);
            firstNodeConverter.Setup(c => c.ConvertNode(childNodeMock.Object))
                .Returns(new IRazorNode[] { new Mock<IRazorNode>().Object })
                .Verifiable();
            converter.Convert(documentMock.Object);
            firstNodeConverter.Verify();
        }
    }
}
