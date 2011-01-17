namespace Telerik.RazorConverter.Tests.WebForms.DOM
{
    using Moq;
    using System;
    using Telerik.RazorConverter.WebForms.DOM;
    using Xunit;

    public class WebFormsCodeGroupNodeTests
    {
        private Mock<IWebFormsCodeBlockNode> openingCodeNode;
        private Mock<IWebFormsTextNode> innerTextNode;
        private Mock<IWebFormsCodeBlockNode> closingCodeNode;
        private WebFormsCodeGroupNode node;

        public WebFormsCodeGroupNodeTests()
        {
            openingCodeNode = new Mock<IWebFormsCodeBlockNode>();
            openingCodeNode.SetupGet(n => n.BlockType).Returns(CodeBlockNodeType.Opening);
            innerTextNode = new Mock<IWebFormsTextNode>();
            closingCodeNode = new Mock<IWebFormsCodeBlockNode>();
            closingCodeNode.SetupGet(n => n.BlockType).Returns(CodeBlockNodeType.Closing);
            node = new WebFormsCodeGroupNode();
            node.Children.Add(openingCodeNode.Object);
            node.Children.Add(innerTextNode.Object);
            node.Children.Add(closingCodeNode.Object);
        }

        [Fact]
        public void Should_combine_content_of_children()
        {
            openingCodeNode.SetupGet(n => n.Content).Returns("if(true){");
            innerTextNode.SetupGet(n => n.Content).Returns("<span>Hello</span>");
            closingCodeNode.SetupGet(n => n.Content).Returns("}");

            node.Content.ShouldEqual("if(true){----$$<span>Hello</span>----$$}");
        }

        [Fact]
        public void Should_replace_content_of_individual_nodes()
        {
            openingCodeNode.SetupSet(n => n.Content = "@if(true){").Verifiable();
            innerTextNode.SetupSet(n => n.Content = "<span>Hello</span>").Verifiable();
            closingCodeNode.SetupSet(n => n.Content = "}").Verifiable();

            node.Content = "@if(true){----$$<span>Hello</span>----$$}";

            openingCodeNode.Verify();
            innerTextNode.Verify();
            closingCodeNode.Verify();
        }

        [Fact]
        public void Should_throw_exception_when_adding_non_content_node()
        {
            var genericNode = new Mock<IWebFormsNode>().Object;
            Assert.Throws<InvalidOperationException>(() => node.Children.Add(genericNode));
        }

        [Fact]
        public void Should_throw_exception_when_trying_to_replace_with_fewer_parts()
        {
            Assert.Throws<InvalidOperationException>(() => node.Content = "if(true){----$$<span>Hello</span>");
        }
    }
}
