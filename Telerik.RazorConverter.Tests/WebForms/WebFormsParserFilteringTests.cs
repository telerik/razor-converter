namespace Telerik.RazorConverter.Tests.WebForms
{
    using Moq;
    using System.Collections.Generic;
    using Telerik.RazorConverter.WebForms.DOM;
    using Telerik.RazorConverter.WebForms.Filters;
    using Telerik.RazorConverter.WebForms.Parsing;
    using Xunit;
    
    public class WebFormsParserFilteringTests
    {
        private readonly Mock<IWebFormsNodeFilterProvider> filterProviderMock;
        private readonly IList<IWebFormsNodeFilter> postprocessingFilters;
        private readonly Mock<IWebFormsNodeFilter> filterMock;
        private readonly WebFormsParser parser;

        public WebFormsParserFilteringTests()
        {
            filterProviderMock = new Mock<IWebFormsNodeFilterProvider>();
            postprocessingFilters = new List<IWebFormsNodeFilter>();
            filterProviderMock.Setup(fp => fp.Filters).Returns(postprocessingFilters);
            filterMock = new Mock<IWebFormsNodeFilter>();
            postprocessingFilters.Add(filterMock.Object);
            parser = new WebFormsParser(new WebFormsNodeFactory(), filterProviderMock.Object);
        }

        [Fact]
        public void Should_executes_postprocessing_filters()
        {
            filterMock.Setup(f => f.Filter(It.IsAny<IWebFormsDirectiveNode>(), null))
                .Returns<IWebFormsNode, IWebFormsNode>((node, prevNode) => new IWebFormsNode[] { node })
                .Verifiable();

            parser.Parse(@"<%@ Page %>");

            filterMock.Verify();
        }

        [Fact]
        public void Should_replace_nodes_with_filter_output()
        {
            var textNodeMock = new Mock<IWebFormsTextNode>();
            textNodeMock.SetupGet(n => n.Text).Returns("Text");
            
            filterMock.Setup(f => f.Filter(It.IsAny<IWebFormsDirectiveNode>(), null))
                .Returns(new IWebFormsNode[] { textNodeMock.Object });

            var document = parser.Parse(@"<%@ Page %>");

            ((IWebFormsTextNode)document.RootNode.Children[0]).Text.ShouldEqual("Text");
            document.RootNode.Children.Count.ShouldEqual(1);
        }

        [Fact]
        public void Should_apply_filters_recursively()
        {
            filterMock.Setup(f => f.Filter(It.IsAny<IWebFormsServerControlNode>(), null))
                .Returns<IWebFormsNode, IWebFormsNode>((node, prevNode) => new IWebFormsNode[] { node })
                .Verifiable();

            filterMock.Setup(f => f.Filter(It.IsAny<IWebFormsTextNode>(), It.IsAny<IWebFormsServerControlNode>()))
                .Returns<IWebFormsNode, IWebFormsNode>((node, prevNode) => new IWebFormsNode[] { node })
                .Verifiable();

            parser.Parse(@"<asp:Content runat=""server"">xxx</asp:Content>");

            filterMock.Verify();
        }
    }
}

