namespace Telerik.RazorConverter.Tests.WebForms.Filters
{
    using Moq;
    using System.Collections.Generic;
    using System.Linq;
    using Telerik.RazorConverter.WebForms.DOM;
    using Telerik.RazorConverter.WebForms.Filters;
    using Xunit;

    public class CodeBlockGroupingFilterTests
    {
        private Mock<IWebFormsNode> parentNode;
        private List<IWebFormsNode> children;
        private Mock<IWebFormsTextNode> outerTextNodeBefore;
        private Mock<IWebFormsCodeBlockNode> openingCodeNode;
        private Mock<IWebFormsTextNode> innerTextNode;
        private Mock<IWebFormsCodeBlockNode> contCodeNode;

        private Mock<IWebFormsCodeBlockNode> closingCodeNode;
        private Mock<IWebFormsTextNode> outerTextNodeAfter;

        private IWebFormsCodeGroupNodeFactory nodeFactory;
        private CodeBlockGroupingFilter filter;

        public CodeBlockGroupingFilterTests()
        {
            parentNode = new Mock<IWebFormsNode>();
            children = new List<IWebFormsNode>();
            parentNode.SetupGet(p => p.Children).Returns(children);

            outerTextNodeBefore = new Mock<IWebFormsTextNode>();
            openingCodeNode = new Mock<IWebFormsCodeBlockNode>();
            openingCodeNode.SetupGet(n => n.BlockType).Returns(CodeBlockNodeType.Opening);
            contCodeNode = new Mock<IWebFormsCodeBlockNode>();
            contCodeNode.SetupGet(n => n.BlockType).Returns(CodeBlockNodeType.Continued);
            innerTextNode = new Mock<IWebFormsTextNode>();
            closingCodeNode = new Mock<IWebFormsCodeBlockNode>();
            closingCodeNode.SetupGet(n => n.BlockType).Returns(CodeBlockNodeType.Closing);
            outerTextNodeAfter = new Mock<IWebFormsTextNode>();
            
            nodeFactory = new WebFormsCodeGroupFactory();
            filter = new CodeBlockGroupingFilter(nodeFactory);
        }

        [Fact]
        public void Should_not_transform_noncode_block()
        {
            var textNode = new Mock<IWebFormsNode>();

            filter.Filter(textNode.Object, null)[0].ShouldBeSameAs(textNode.Object);
        }

        [Fact]
        public void Should_preserve_surrounding_noncode_blocks()
        {
            children.Add(outerTextNodeBefore.Object);
            children.Add(openingCodeNode.Object);
            children.Add(closingCodeNode.Object);
            children.Add(outerTextNodeAfter.Object); 

            var filteredChildren = GetFilteredChildren();

            filteredChildren[0].ShouldBeSameAs(outerTextNodeBefore.Object);
            filteredChildren[2].ShouldBeSameAs(outerTextNodeAfter.Object);
        }

        [Fact]
        public void Should_not_transform_complete_code_block()
        {
            var codeNode = new Mock<IWebFormsCodeBlockNode>();
            codeNode.SetupGet(c => c.BlockType).Returns(CodeBlockNodeType.Complete);

            filter.Filter(codeNode.Object, null)[0].ShouldBeSameAs(codeNode.Object);
        }

        [Fact]
        public void Should_add_opening_code_block_to_group()
        {
            children.Add(openingCodeNode.Object);
            children.Add(closingCodeNode.Object);

            IList<IWebFormsNode> groupedNodes = GetGroupedNodes();

            groupedNodes[0].ShouldBeSameAs(openingCodeNode.Object);
        }

        [Fact]
        public void Should_add_inner_text_block_to_group()
        {
            children.Add(openingCodeNode.Object);
            children.Add(innerTextNode.Object);
            children.Add(closingCodeNode.Object);

            IList<IWebFormsNode> groupedNodes = GetGroupedNodes();

            groupedNodes[1].ShouldBeSameAs(innerTextNode.Object);
        }

        [Fact]
        public void Should_add_continued_code_block_to_group()
        {
            children.Add(openingCodeNode.Object);
            children.Add(contCodeNode.Object);
            children.Add(closingCodeNode.Object);

            IList<IWebFormsNode> groupedNodes = GetGroupedNodes();

            groupedNodes[1].ShouldBeSameAs(contCodeNode.Object);
        }

        [Fact]
        public void Should_add_closing_code_block_to_group()
        {
            children.Add(openingCodeNode.Object);
            children.Add(closingCodeNode.Object);

            IList<IWebFormsNode> groupedNodes = GetGroupedNodes();

            groupedNodes[1].ShouldBeSameAs(closingCodeNode.Object);
        }

        [Fact]
        public void Should_not_add_outer_text_block_to_group()
        {
            children.Add(outerTextNodeBefore.Object);
            children.Add(openingCodeNode.Object);
            children.Add(closingCodeNode.Object);
            children.Add(outerTextNodeAfter.Object); 

            IList<IWebFormsNode> groupedNodes = GetGroupedNodes();

            groupedNodes.Count.ShouldEqual(2);
        }

        [Fact]
        public void Should_group_nested_code_block()
        {
            var nestedOpeningCodeNode = new Mock<IWebFormsCodeBlockNode>();
            nestedOpeningCodeNode.SetupGet(n => n.BlockType).Returns(CodeBlockNodeType.Opening);
            var nestedClosingCodeNode = new Mock<IWebFormsCodeBlockNode>();
            nestedClosingCodeNode.SetupGet(n => n.BlockType).Returns(CodeBlockNodeType.Closing);

            children.Add(openingCodeNode.Object);
            children.Add(nestedOpeningCodeNode.Object);
            children.Add(nestedClosingCodeNode.Object);
            children.Add(closingCodeNode.Object);

            IList<IWebFormsNode> groupedNodes = GetGroupedNodes();

            (groupedNodes.Count).ShouldEqual(4);
        }

        private IList<IWebFormsNode> GetFilteredChildren()
        {
            var filterOutput = new List<IWebFormsNode>();
            foreach (var childNode in children)
            {
                filterOutput.AddRange(filter.Filter(childNode, filterOutput.LastOrDefault()));
            }

            return filterOutput;
        }

        private IList<IWebFormsNode> GetGroupedNodes()
        {
            return GetFilteredChildren().First(n => n is IWebFormsCodeGroupNode).Children;
        }
    }
}

