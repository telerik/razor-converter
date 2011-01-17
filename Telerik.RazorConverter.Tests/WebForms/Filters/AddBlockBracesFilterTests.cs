namespace Telerik.RazorConverter.Tests.WebForms.Filters
{
    using Moq;
    using System.Collections.Generic;
    using System.Linq;
    using Telerik.RazorConverter.WebForms.DOM;
    using Telerik.RazorConverter.WebForms.Filters;
    using Xunit;

    public class AddBlockBracesFilterTests
    {
        private Mock<IWebFormsCodeBlockNode> codeNodeMock;        
        private Mock<IWebFormsCodeGroupNode> codeGroupNodeMock;

        private AddBlockBracesFilter filter;

        public AddBlockBracesFilterTests()
        {
            codeNodeMock = new Mock<IWebFormsCodeBlockNode>();
            codeGroupNodeMock = new Mock<IWebFormsCodeGroupNode>();
            filter = new AddBlockBracesFilter();
        }

        [Fact]
        public void Should_not_transform_generic_block()
        {
            var textNode = new Mock<IWebFormsNode>();
            
            var filterResult = filter.Filter(textNode.Object, null);
                        
            filterResult[0].ShouldBeSameAs(textNode.Object);
        }

        [Fact]
        public void Should_not_transform_opening_code_block()
        {
            codeNodeMock.SetupGet(cn => cn.BlockType).Returns(CodeBlockNodeType.Opening);
            codeNodeMock.SetupGet(cn => cn.Content).Returns("if (true) {");
            
            filter.Filter(codeNodeMock.Object, null);
            
            codeNodeMock.VerifySet(cb => cb.Content = It.IsAny<string>(), Times.Never());
        }

        [Fact]
        public void Should_not_transform_continued_code_block()
        {
            codeNodeMock.SetupGet(cn => cn.BlockType).Returns(CodeBlockNodeType.Continued);
            codeNodeMock.SetupGet(cn => cn.Content).Returns("} else {");
            
            filter.Filter(codeNodeMock.Object, null);

            codeNodeMock.VerifySet(cb => cb.Content = It.IsAny<string>(), Times.Never());
        }

        [Fact]
        public void Should_not_transform_closing_code_block()
        {
            codeNodeMock.SetupGet(cn => cn.BlockType).Returns(CodeBlockNodeType.Closing);
            codeNodeMock.SetupGet(cn => cn.Content).Returns("}");
            
            filter.Filter(codeNodeMock.Object, null);

            codeNodeMock.VerifySet(cb => cb.Content = It.IsAny<string>(), Times.Never());
        }

        [Fact]
        public void Should_add_braces_to_non_keyword_statement()
        {
            codeNodeMock.SetupGet(cn => cn.BlockType).Returns(CodeBlockNodeType.Complete);
            codeNodeMock.Setup(cb => cb.Content).Returns(@"someCode();");
            codeNodeMock.SetupSet(cb => cb.Content = "{someCode();}").Verifiable();
            
            filter.Filter(codeNodeMock.Object, null);

            codeNodeMock.Verify();
        }

        [Fact]
        public void Should_not_add_braces_to_if_statement()
        {
            codeNodeMock.SetupGet(cn => cn.BlockType).Returns(CodeBlockNodeType.Complete);
            codeNodeMock.Setup(cb => cb.Content).Returns(@" if (true) { } ");

            filter.Filter(codeNodeMock.Object, null);

            codeNodeMock.VerifySet(cb => cb.Content = It.IsAny<string>(), Times.Never());
        }

        [Fact]
        public void Should_not_add_braces_to_Html_RenderPartial_statement()
        {
            codeNodeMock.SetupGet(cn => cn.BlockType).Returns(CodeBlockNodeType.Complete);
            codeNodeMock.Setup(cb => cb.Content).Returns(@" Html.RenderPartial(""AccessibilityValidation""); ");

            filter.Filter(codeNodeMock.Object, null);

            codeNodeMock.VerifySet(cb => cb.Content = It.IsAny<string>(), Times.Never());
        }

        [Fact]
        public void Should_add_braces_to_blocks_with_multiple_statements()
        {
            codeNodeMock.SetupGet(cn => cn.BlockType).Returns(CodeBlockNodeType.Complete);
            codeNodeMock.Setup(cb => cb.Content).Returns("if (true) { }\r\nHtml.Telerik().Grid(data);");
            codeNodeMock.SetupSet(cb => cb.Content = "{if (true) { }\r\nHtml.Telerik().Grid(data);}").Verifiable();

            filter.Filter(codeNodeMock.Object, null);

            codeNodeMock.Verify();
        }

        [Fact]
        public void Should_add_braces_to_code_group()
        {
            codeGroupNodeMock.SetupGet(g => g.Content)
                .Returns("ScriptRegistrar().OnDocumentReady(----$$@<text>alert(1);</text>----$$)");

            codeGroupNodeMock
                .SetupSet(g => g.Content = "{ScriptRegistrar().OnDocumentReady(----$$@<text>alert(1);</text>----$$)}").Verifiable();

            filter.Filter(codeGroupNodeMock.Object, null);

            codeGroupNodeMock.Verify();
        }
    }
}

