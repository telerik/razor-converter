namespace Telerik.RazorConverter.Tests.Converters
{
    using Moq;
    using System;
    using System.Collections.Generic;
    using Telerik.RazorConverter.Razor.Converters;
    using Telerik.RazorConverter.Razor.DOM;
    using Telerik.RazorConverter.WebForms.DOM;
    using Xunit;

    public class DirectiveConverterTests
    {
        private readonly DirectiveConverter converter;
        private readonly Mock<IRazorDirectiveNodeFactory> nodeFactoryMock;
        private readonly Mock<IWebFormsDirectiveNode> pageDirectiveMock;
        private readonly IDictionary<string, string> attributesDictionary;

        public DirectiveConverterTests()
        {
            nodeFactoryMock = new Mock<IRazorDirectiveNodeFactory>();
            converter = new DirectiveConverter(nodeFactoryMock.Object);
            attributesDictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            pageDirectiveMock = new Mock<IWebFormsDirectiveNode>();
            pageDirectiveMock.SetupGet(n => n.Attributes).Returns(attributesDictionary);
        }

        [Fact]
        public void Should_be_able_to_convert_directive_node()
        {
            converter.CanConvertNode(pageDirectiveMock.Object).ShouldBeTrue();
        }

        [Fact]
        public void Should_drop_page_directive_when_no_base_type_is_specified()
        {
            pageDirectiveMock.SetupGet(d => d.Directive).Returns(DirectiveType.Page);
            attributesDictionary.Add("Inherits", "System.Web.Mvc.ViewPage");
            var nodes = converter.ConvertNode(pageDirectiveMock.Object);
            nodes.Count.ShouldEqual(0);
        }

        [Fact]
        public void Should_convert_Page_directive_to_model_when_inheriting_from_generic_ViewPage()
        {
            pageDirectiveMock.SetupGet(d => d.Directive).Returns(DirectiveType.Page);
            attributesDictionary.Add("Inherits", "System.Web.Mvc.ViewPage<IEnumerable<OrderDto>>");
            nodeFactoryMock.Setup(f => f.CreateDirectiveNode("model", It.IsAny<string>())).Verifiable();            
            converter.ConvertNode(pageDirectiveMock.Object);
            nodeFactoryMock.Verify();
        }

        [Fact]
        public void Should_extract_model_type_from_generic_ViewPage()
        {
            pageDirectiveMock.SetupGet(d => d.Directive).Returns(DirectiveType.Page);
            attributesDictionary.Add("Inherits", "System.Web.Mvc.ViewPage<IEnumerable<OrderDto>>");
            nodeFactoryMock.Setup(f => f.CreateDirectiveNode(It.IsAny<string>(), "IEnumerable<OrderDto>")).Verifiable();
            converter.ConvertNode(pageDirectiveMock.Object);
            nodeFactoryMock.Verify();
        }

        [Fact]
        public void Should_drop_control_directive_when_no_base_type_is_specified()
        {
            pageDirectiveMock.SetupGet(d => d.Directive).Returns(DirectiveType.Page);
            attributesDictionary.Add("Inherits", "System.Web.Mvc.ViewUserControl");
            var nodes = converter.ConvertNode(pageDirectiveMock.Object);
            nodes.Count.ShouldEqual(0);
        }

        [Fact]
        public void Should_convert_Control_directive_to_model_when_inheriting_from_generic_ViewPage()
        {
            pageDirectiveMock.SetupGet(d => d.Directive).Returns(DirectiveType.Control);
            attributesDictionary.Add("Inherits", "System.Web.Mvc.ViewUserControl<IEnumerable<OrderDto>>");
            nodeFactoryMock.Setup(f => f.CreateDirectiveNode("model", It.IsAny<string>())).Verifiable();
            converter.ConvertNode(pageDirectiveMock.Object);
            nodeFactoryMock.Verify();
        }

        [Fact]
        public void Should_extract_model_type_from_generic_ViewUserControl()
        {
            pageDirectiveMock.SetupGet(d => d.Directive).Returns(DirectiveType.Control);
            attributesDictionary.Add("Inherits", "System.Web.Mvc.ViewUserControl<IEnumerable<OrderDto>>");
            nodeFactoryMock.Setup(f => f.CreateDirectiveNode(It.IsAny<string>(), "IEnumerable<OrderDto>")).Verifiable();
            converter.ConvertNode(pageDirectiveMock.Object);
            nodeFactoryMock.Verify();
        }

        [Fact]
        public void Should_convert_Page_directive_to_inherit_when_inheriting_from_unknown_type()
        {
            pageDirectiveMock.SetupGet(d => d.Directive).Returns(DirectiveType.Page);
            attributesDictionary.Add("Inherits", "MyViewPageType");
            nodeFactoryMock.Setup(f => f.CreateDirectiveNode("inherits", It.IsAny<string>())).Verifiable();
            converter.ConvertNode(pageDirectiveMock.Object);
            nodeFactoryMock.Verify();
        }

        [Fact]
        public void Should_extract_custom_model_type()
        {
            pageDirectiveMock.SetupGet(d => d.Directive).Returns(DirectiveType.Page);
            attributesDictionary.Add("Inherits", "MyViewPageType");
            nodeFactoryMock.Setup(f => f.CreateDirectiveNode(It.IsAny<string>(), "MyViewPageType")).Verifiable();
            converter.ConvertNode(pageDirectiveMock.Object);
            nodeFactoryMock.Verify();
        }
    }
}
