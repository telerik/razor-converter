namespace RazorConverter.WebForms.Test
{
    using Moq;
    using Telerik.RazorConverter.WebForms.DOM;
    using Telerik.RazorConverter.WebForms.Filters;
    using Telerik.RazorConverter.WebForms.Parsing;
    using Xunit;
    
    public class WebFormsParserTests
    {
        private readonly Mock<IWebFormsNodeFilterProvider> filterProviderMock;
        private readonly WebFormsParser parser;

        public WebFormsParserTests()
        {
            filterProviderMock = new Mock<IWebFormsNodeFilterProvider>();
            filterProviderMock.Setup(fp => fp.Filters).Returns(new IWebFormsNodeFilter[] { });
            parser = new WebFormsParser(new WebFormsNodeFactory(), filterProviderMock.Object);
        }

        [Fact]
        public void Parse_should_return_Document_node()
        {
            var document = parser.Parse(@"<%@ Page %>");
            Assert.Equal(NodeType.Document, document.RootNode.Type);
        }

        [Fact]
        public void Parse_should_recognize_page_as_directive()
        {
            var document = parser.Parse(@"<%@ Page %>");
            Assert.Equal(NodeType.Directive, document.RootNode.Children[0].Type);
        }

        [Fact]
        public void Parse_should_parse_page_directive_type()
        {
            var document = parser.Parse(@"<%@ Page %>");
            Assert.Equal(DirectiveType.Page, ((IWebFormsDirectiveNode)document.RootNode.Children[0]).Directive);
        }

        [Fact]
        public void Parse_should_parse_page_directive_attributes()
        {
            var document = parser.Parse(@"<%@ Page Language=""C#"" Inherits=""System.Web.Mvc.ViewPage<IEnumerable<OrderDto>>""%>");
            Assert.Equal("C#", document.RootNode.Children[0].Attributes["Language"]);
            Assert.Equal("System.Web.Mvc.ViewPage<IEnumerable<OrderDto>>", document.RootNode.Children[0].Attributes["Inherits"]);
        }

        [Fact]
        public void Parse_should_parse_control_directive_type()
        {
            var document = parser.Parse(@"<%@ Control %>");
            Assert.Equal(DirectiveType.Control, ((IWebFormsDirectiveNode)document.RootNode.Children[0]).Directive);
        }

        [Fact]
        public void Parse_should_parse_control_directive_attributes()
        {
            var document = parser.Parse(@"<%@ Control Language=""C#"" Inherits=""System.Web.Mvc.ViewPage<IEnumerable<OrderDto>>""%>");
            Assert.Equal("C#", document.RootNode.Children[0].Attributes["Language"]);
            Assert.Equal("System.Web.Mvc.ViewPage<IEnumerable<OrderDto>>", document.RootNode.Children[0].Attributes["Inherits"]);
        }

        [Fact]
        public void Parse_should_parse_text_before_page_directive()
        {
            var document = parser.Parse("TEXT TEXT<%@ Page %>");
            ((IWebFormsTextNode)document.RootNode.Children[0]).Text.ShouldEqual("TEXT TEXT");
        }

        [Fact]
        public void Parse_should_parse_comments()
        {
            var document = parser.Parse("TEXT TEXT<%-- COMMENT --%>TEXT TEXT");
            ((IWebFormsCommentNode)document.RootNode.Children[1]).Text.ShouldEqual(" COMMENT ");
        }

        [Fact]
        public void Parse_should_parse_comments_with_dashes()
        {
            var document = parser.Parse("TEXT TEXT<%-- COMM-ENT --%>TEXT TEXT");
            ((IWebFormsCommentNode)document.RootNode.Children[1]).Text.ShouldEqual(" COMM-ENT ");
        }

        [Fact]
        public void Parse_should_parse_server_control_tagName()
        {
            var document = parser.Parse(@"<asp:Content runat=""server"">xxx</asp:Content>");
            ((IWebFormsServerControlNode)document.RootNode.Children[0]).TagName.ShouldEqual("asp:Content");
        }

        [Fact]
        public void Parse_should_parse_server_control_with_additional_attributes()
        {
            var document = parser.Parse(@"<asp:Content contentplaceholderid=""maincontent"" runat=""server"">xxx</asp:Content>");
            ((IWebFormsServerControlNode)document.RootNode.Children[0]).TagName.ShouldEqual("asp:Content");
        }

        [Fact]
        public void Parse_should_parse_server_control_attributes()
        {
            var document = parser.Parse(@"<asp:Content contentplaceholderid=""maincontent"" runat=""server"">xxx</asp:Content>");
            ((IWebFormsServerControlNode)document.RootNode.Children[0]).Attributes["ContentPlaceHolderID"].ShouldEqual("maincontent");
        }

        [Fact]
        public void Parse_should_parse_children_of_server_control()
        {
            var document = parser.Parse(@"<asp:Content runat=""server"">INNER TEXT</asp:Content>");
            ((IWebFormsTextNode)document.RootNode.Children[0].Children[0]).Text.ShouldEqual("INNER TEXT");
        }

        [Fact]
        public void Parse_should_treat_HTML_tags_nested_in_server_control_as_text()
        {
            var document = parser.Parse(@"<asp:Content runat=""server""><span>INNER TEXT</span></asp:Content>");
            ((IWebFormsTextNode)document.RootNode.Children[0].Children[0]).Text.ShouldEqual("<span>INNER TEXT</span>");
        }

        [Fact]
        public void Parse_should_parse_multiple_server_control_tags()
        {
            var document = parser.Parse(@"<asp:Content runat=""server""><span>INNER TEXT 1</span></asp:Content><asp:Content runat=""server""><span>INNER TEXT 2</span></asp:Content>");
            (document.RootNode.Children[0] is IWebFormsServerControlNode).ShouldBeTrue();
            (document.RootNode.Children[1] is IWebFormsServerControlNode).ShouldBeTrue();
            ((IWebFormsTextNode)document.RootNode.Children[0].Children[0]).Text.ShouldEqual("<span>INNER TEXT 1</span>");
            ((IWebFormsTextNode)document.RootNode.Children[1].Children[0]).Text.ShouldEqual("<span>INNER TEXT 2</span>");
        }

        [Fact]
        public void Parse_should_parse_text_after_server_control()
        {
            var document = parser.Parse(@"<asp:Content runat=""server"">INNER TEXT</asp:Content>TEXT AFTER SERVER CONTROL");
            ((IWebFormsTextNode)document.RootNode.Children[1]).Text.ShouldEqual("TEXT AFTER SERVER CONTROL");
        }

        [Fact]
        public void Parse_should_treat_HTML_tags_as_text()
        {
            var document = parser.Parse(@"<div>INNER TEXT</div>");
            ((IWebFormsTextNode)document.RootNode.Children[0]).Text.ShouldEqual("<div>INNER TEXT</div>");
        }

        [Fact]
        public void Parse_should_parse_expression_blocks()
        {
            var document = parser.Parse(@"<%= Html.Telerik().Grid(Model).Name(""Grid"") %>");
            ((IWebFormsExpressionBlockNode)document.RootNode.Children[0]).Expression.ShouldEqual(@" Html.Telerik().Grid(Model).Name(""Grid"") ");
        }

        [Fact]
        public void Parse_should_parse_expression_blocks_within_tags()
        {
            var document = parser.Parse(@"<link href=""<%= ResolveUrl(""~/favicon.ico"") %>"" type=""image/x-icon"" rel=""icon"" />");
            ((IWebFormsExpressionBlockNode)document.RootNode.Children[1])
                .Expression.Trim().ShouldEqual(@"ResolveUrl(""~/favicon.ico"")");
        }

        [Fact]
        public void Parse_should_parse_encoded_expressions_as_expression_blocks()
        {
            var document = parser.Parse(@"<%: Html.Telerik().Grid(Model).Name(""Grid"") %>");
            ((IWebFormsExpressionBlockNode)document.RootNode.Children[0]).Expression.ShouldEqual(@" Html.Telerik().Grid(Model).Name(""Grid"") ");
        }

        [Fact]
        public void Parse_should_parse_code_blocks()
        {
            var document = parser.Parse(@"<% Html.Telerik().Grid(Model).Name(""Grid"").Render(); %>");
            ((IWebFormsCodeBlockNode)document.RootNode.Children[0]).Code.ShouldEqual(@" Html.Telerik().Grid(Model).Name(""Grid"").Render(); ");
        }

        [Fact]
        public void Should_mark_code_block_as_opening_when_ending_with_open_block()
        {
            var document = parser.Parse(@"<% if (true) { %>");
            ((IWebFormsCodeBlockNode)document.RootNode.Children[0]).BlockType.ShouldEqual(CodeBlockNodeType.Opening);
        }

        [Fact]
        public void Should_mark_code_block_as_continued_when_closing_previous_block_and_opening_new_one()
        {
            var document = parser.Parse(@"<% } else { %>");
            ((IWebFormsCodeBlockNode)document.RootNode.Children[0]).BlockType.ShouldEqual(CodeBlockNodeType.Continued);
        }

        [Fact]
        public void Should_mark_code_block_as_closing_when_closing_previous_block()
        {
            var document = parser.Parse(@"<% } %>");
            ((IWebFormsCodeBlockNode)document.RootNode.Children[0]).BlockType.ShouldEqual(CodeBlockNodeType.Closing);
        }

        [Fact]
        public void Should_mark_code_block_as_complete_when_all_blocks_are_closed()
        {
            var document = parser.Parse(@"<% Html.Telerik().Grid(Model).Name(""Grid"").Render(); %>");
            ((IWebFormsCodeBlockNode)document.RootNode.Children[0]).BlockType.ShouldEqual(CodeBlockNodeType.Complete);
        }

        [Fact]
        public void Should_not_parse_script_content()
        {
            var script = @"<script type=""text/javascript"">alert('</a><img');</script>";
            var document = parser.Parse(@"<asp:Content runat=""server"">" + script + @"</asp:Content>");
            ((IWebFormsTextNode)document.RootNode.Children[0].Children[0]).Text.ShouldEqual(script);
        }

        [Fact]
        public void Should_treat_doctype_as_text()
        {
            var docType = @"<!DOCTYPE html PUBLIC   ""-//W3C//DTD XHTML 1.0 Strict//EN""
                                                    ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd"">";
            var document = parser.Parse(docType);
            ((IWebFormsTextNode)document.RootNode.Children[0]).Text.ShouldEqual(docType);
        }
        
    }
}

