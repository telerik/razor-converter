namespace Telerik.RazorConverter.WebForms.Parsing
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Telerik.RazorConverter.WebForms.DOM;   

    [Export(typeof(IWebFormsNodeFactory))]
    public class WebFormsNodeFactory : IWebFormsNodeFactory
    {
        private IAttributesReader attributesReader;

        public IDictionary<NodeType, Func<Match, IWebFormsNode>> NodeBuilders
        { 
            get; 
            private set;
        }

        public WebFormsNodeFactory()
        {
            attributesReader = new AttributesReader();
            NodeBuilders = new Dictionary<NodeType, Func<Match, IWebFormsNode>>();
            NodeBuilders.Add(NodeType.Directive, DirectiveNodeBuilder);
            NodeBuilders.Add(NodeType.Text, TextNodeBuilder);
            NodeBuilders.Add(NodeType.Comment, CommentNodeBuilder);
            NodeBuilders.Add(NodeType.ServerControl, ServerControlNodeBuilder);
            NodeBuilders.Add(NodeType.CodeBlock, CodeBlockNodeBuilder);
            NodeBuilders.Add(NodeType.ExpressionBlock, ExpressionBlockNodeBuilder);
            NodeBuilders.Add(NodeType.EncodedExpressionBlock, ExpressionBlockNodeBuilder);
        }

        public IWebFormsNode CreateNode(Match match, NodeType type)
        {
            Func<Match, IWebFormsNode> builder;
            if (NodeBuilders.TryGetValue(type, out builder))
            {
                return builder(match);
            }
            else
            {
                var genericNode = new WebFormsNode { Type = type };
                attributesReader.ReadAttributes(match, genericNode.Attributes);
                return genericNode;
            }
        }

        private IWebFormsNode DirectiveNodeBuilder(Match match)
        {
            var node = new DirectiveNode { 
                Directive = DirectiveType.Unknown,
                Type = NodeType.Directive
            };

            attributesReader.ReadAttributes(match, node.Attributes);

            if (match.Groups.Count > 1 && match.Groups[1].Captures.Count > 0)
            {
                string directiveType= match.Groups[1].Captures[0].Value.ToLowerInvariant();
                if (directiveType.Contains("page"))
                {
                    node.Directive = DirectiveType.Page;
                }
                else if (directiveType.Contains("control"))
                {
                    node.Directive = DirectiveType.Control;
                }
                else if (directiveType.Contains("import"))
                {
                    node.Directive = DirectiveType.Import;
                }
            }

            return node;
        }

        private IWebFormsNode TextNodeBuilder(Match match)
        {
            var node = new TextNode { Text = match.Value };
            attributesReader.ReadAttributes(match, node.Attributes);
            return node;
        }

        private IWebFormsNode CommentNodeBuilder(Match match)
        {
            var node = new CommentNode { Text = match.Groups["comment"].Value };
            attributesReader.ReadAttributes(match, node.Attributes);
            return node;
        }

        private IWebFormsNode ServerControlNodeBuilder(Match match)
        {
            var node = new ServerControlNode();
            attributesReader.ReadAttributes(match, node.Attributes);

            if (match.Groups["tagname"].Success)
            {
                node.TagName = match.Groups["tagname"].Captures[0].Value;
            }

            if (match.Groups["attributes"].Success)
            {
                var attributeRegex = new Regex(
                    @"((?<attrname>\w[-\w:]*)(\s*=\s*\""(?<attrval>[^\""]*)\""|\s*=\s*'(?<attrval>[^']*)'|\s*=\s*(?<attrval><%#.*?%>)|\s*=\s*(?<attrval>[^\s=/>]*)|(?<attrval>\s*?)))",
                    RegexOptions.Singleline | RegexOptions.Multiline);

                var attributeMatch = attributeRegex.Match(match.Groups["attributes"].Value);
                while (attributeMatch.Success)
                {
                    node.Attributes[attributeMatch.Groups["attrname"].Value] = attributeMatch.Groups["attrval"].Value;
                    attributeMatch = attributeMatch.NextMatch();
                }
            }

            return node;
        }

        private IWebFormsNode CodeBlockNodeBuilder(Match match)
        {
            var node = new WebFormsCodeBlockNode();
            attributesReader.ReadAttributes(match, node.Attributes);

            if (match.Groups["code"].Success)
            {
                node.Code = match.Groups["code"].Captures[0].Value;
            }

            var openingBracesCount = match.Value.Count(c => c == '{');
            var closingBracesCount = match.Value.Count(c => c == '}');
            var closingBraceFirst = match.Value.IndexOf('}') < match.Value.IndexOf('{');

            if (openingBracesCount > closingBracesCount)
            {
                node.BlockType = CodeBlockNodeType.Opening;
            }
            else if (openingBracesCount < closingBracesCount)
            {
                node.BlockType = CodeBlockNodeType.Closing;
            }
            else if (closingBraceFirst)
            {
                node.BlockType = CodeBlockNodeType.Continued;
            }

            return node;
        }

        private IWebFormsNode ExpressionBlockNodeBuilder(Match match)
        {
            var node = new ExpressionBlockNode();
            attributesReader.ReadAttributes(match, node.Attributes);

            if (match.Groups["code"].Success)
            {
                node.Expression = match.Groups["code"].Captures[0].Value;
            }

            return node;
        }
    }
}
