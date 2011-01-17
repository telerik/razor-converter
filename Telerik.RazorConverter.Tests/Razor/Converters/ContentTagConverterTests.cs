namespace Telerik.RazorConverter.Tests.Razor.Converters
{
    using Moq;
    using System.Collections.Generic;
    using Telerik.RazorConverter;
    using Telerik.RazorConverter.Razor.Converters;
    using Telerik.RazorConverter.Razor.DOM;
    using Telerik.RazorConverter.WebForms.DOM;
    using Xunit;

    public class ContentTagConverterTests
    {
        private readonly ContentTagConverter converter;
        private readonly Mock<IRazorNodeConverterProvider> nodeConverterProviderMock;
        private readonly Mock<IRazorSectionNodeFactory> sectionNodeFactoryMock;
        private readonly Mock<IContentTagConverterConfiguration> configurationMock;
        private readonly Mock<INodeConverter<IRazorNode>> childNodeConverterMock;
        private readonly Mock<IWebFormsServerControlNode> contentTagMock;
        private readonly Dictionary<string, string> contentTagMockAttributes;
        private readonly IList<IWebFormsNode> contentTagChildren;

        public ContentTagConverterTests()
        {
            childNodeConverterMock = new Mock<INodeConverter<IRazorNode>>();
            sectionNodeFactoryMock = new Mock<IRazorSectionNodeFactory>();
            nodeConverterProviderMock = new Mock<IRazorNodeConverterProvider>();
            nodeConverterProviderMock.SetupGet(p => p.NodeConverters)
                .Returns(new INodeConverter<IRazorNode>[] { childNodeConverterMock.Object });

            configurationMock = new Mock<IContentTagConverterConfiguration>();
            configurationMock.SetupGet(c => c.BodyContentPlaceHolderID).Returns("MainContent");

            converter = new ContentTagConverter(nodeConverterProviderMock.Object, sectionNodeFactoryMock.Object, configurationMock.Object);

            contentTagChildren = new List<IWebFormsNode>();
            contentTagMockAttributes = new Dictionary<string, string>() { { "ContentPlaceHolderID", "HeadContent" } };

            contentTagMock = new Mock<IWebFormsServerControlNode>();
            contentTagMock.SetupGet(scn => scn.Type).Returns(NodeType.ServerControl);
            contentTagMock.SetupGet(scn => scn.TagName).Returns("asp:Content");
            contentTagMock.SetupGet(scn => scn.Attributes).Returns(contentTagMockAttributes);
            contentTagMock.SetupGet(scn => scn.Children).Returns(contentTagChildren);
        }

        [Fact]
        public void Should_be_able_to_convert_content_tag_node()
        {
            converter.CanConvertNode(contentTagMock.Object).ShouldBeTrue();
        }

        [Fact]
        public void Should_not_be_able_to_convert_other_server_control_nodes()
        {
            var serverControlMock = new Mock<IWebFormsServerControlNode>();
            serverControlMock.SetupGet(scn => scn.Type).Returns(NodeType.ServerControl);
            serverControlMock.SetupGet(scn => scn.TagName).Returns("asp:Something");
            converter.CanConvertNode(serverControlMock.Object).ShouldBeFalse();
        }

        [Fact]
        public void Should_not_be_able_to_convert_other_nodes()
        {
            var serverControlMock = new Mock<IWebFormsNode>();
            serverControlMock.SetupGet(scn => scn.Type).Returns(NodeType.Text);
            converter.CanConvertNode(serverControlMock.Object).ShouldBeFalse();
        }

        [Fact]
        public void Should_convert_content_placeholders_to_sections()
        {
            converter.ConvertNode(contentTagMock.Object);

            sectionNodeFactoryMock.Verify(f => f.CreateSectionNode("HeadContent"));
        }

        [Fact]
        public void Should_use_chid_converter_for_child_nodes()
        {
            var childNodeMock = new Mock<IWebFormsTextNode>();
            childNodeMock.SetupGet(c => c.Text).Returns("TEXT");
            contentTagChildren.Add(childNodeMock.Object);
            childNodeConverterMock.Setup(c => c.CanConvertNode(childNodeMock.Object)).Returns(true).Verifiable();
            childNodeConverterMock.Setup(c => c.ConvertNode(childNodeMock.Object)).Returns(new IRazorNode[] { new RazorTextNode() });

            var sectionNodeMock = new Mock<IRazorSectionNode>();
            sectionNodeMock.SetupGet(n => n.Children).Returns(new List<IRazorNode>());
            sectionNodeFactoryMock.Setup(f => f.CreateSectionNode(It.IsAny<string>())).Returns(sectionNodeMock.Object);

            var result = converter.ConvertNode(contentTagMock.Object);

            result[0].Children[0].ShouldBeType(typeof(RazorTextNode));
            childNodeConverterMock.Verify();
        }

        [Fact]
        public void Should_unwrap_child_nodes_from_main_placeholder()
        {
            contentTagMockAttributes["ContentPlaceHolderID"] = "MainContent";

            var childNodeMock = new Mock<IWebFormsTextNode>();
            contentTagChildren.Add(childNodeMock.Object);

            childNodeConverterMock.Setup(c => c.CanConvertNode(It.IsAny<IWebFormsNode>())).Returns(true);
            childNodeConverterMock.Setup(c => c.ConvertNode(It.IsAny<IWebFormsNode>()))
                .Returns(new IRazorNode[] { new Mock<IRazorTextNode>().Object });

            var result = converter.ConvertNode(contentTagMock.Object);

            (result[0] is IRazorTextNode).ShouldBeTrue();
        }
    }
}
