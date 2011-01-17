namespace Telerik.RazorConverter.Tests.Razor.Converters
{
    using Moq;
    using Telerik.RazorConverter;
    using Telerik.RazorConverter.Razor.Converters;
    using Telerik.RazorConverter.Razor.DOM;
    using Telerik.RazorConverter.WebForms.DOM;
    using Xunit;

    public class CodeGroupConverterTests
    {
        private readonly CodeGroupConverter converter;
        private readonly Mock<IRazorNodeConverterProvider> nodeConverterProviderMock;
        private readonly Mock<INodeConverter<IRazorNode>> childNodeConverterMock;
        private readonly Mock<IWebFormsCodeGroupNode> codeGroupNodeMock;

        public CodeGroupConverterTests()
        {
            childNodeConverterMock = new Mock<INodeConverter<IRazorNode>>();
            nodeConverterProviderMock = new Mock<IRazorNodeConverterProvider>();
            nodeConverterProviderMock.SetupGet(p => p.NodeConverters)
                .Returns(new INodeConverter<IRazorNode>[] { childNodeConverterMock.Object });

            converter = new CodeGroupConverter(nodeConverterProviderMock.Object);
            codeGroupNodeMock = new Mock<IWebFormsCodeGroupNode>();
        }

        [Fact]
        public void Should_be_able_to_convert_code_block_group_node()
        {
            converter.CanConvertNode(codeGroupNodeMock.Object).ShouldBeTrue();
        }

        [Fact]
        public void Should_drop_code_block_group_node_when_it_has_no_child_nodes()
        {
            codeGroupNodeMock.SetupGet(cg => cg.Children).Returns(new IWebFormsNode[] { });
            converter.ConvertNode(codeGroupNodeMock.Object).Count.ShouldEqual(0);
        }

        [Fact]
        public void Should_use_provided_converters_for_child_nodes()
        {
            var childNode = new TextNode { Text = "TEXT" };
            codeGroupNodeMock.SetupGet(cg => cg.Children).Returns(new IWebFormsNode[] {childNode });
            childNodeConverterMock.Setup(c => c.CanConvertNode(childNode)).Returns(true).Verifiable();
            childNodeConverterMock.Setup(c => c.ConvertNode(childNode)).Returns(new IRazorNode[] { new RazorTextNode() });

            var result = converter.ConvertNode(codeGroupNodeMock.Object);

            result.Count.ShouldEqual(1);
            result[0].ShouldBeType(typeof(RazorTextNode));
            childNodeConverterMock.Verify();
        }
    }
}
