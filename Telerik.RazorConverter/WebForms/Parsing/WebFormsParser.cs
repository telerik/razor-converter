namespace Telerik.RazorConverter.WebForms.Parsing
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web.RegularExpressions;
    using Telerik.RazorConverter.WebForms.DOM;
    using Telerik.RazorConverter.WebForms.Filters;

    [Export(typeof(IWebFormsParser))]
    public class WebFormsParser : IWebFormsParser
    {
        private static Regex directiveRegex;
        private static Regex commentRegex;
        private static Regex startTagOpeningBracketRegex;
        private static Regex endTagRegex;
        private static Regex aspCodeRegex;
        private static Regex aspExprRegex;
        private static Regex aspEncodedExprRegex;
        private static Regex textRegex;
        private static Regex runatServerTagRegex;
        private static Regex scriptRegex;
        private static Regex doctypeRegex;

        static WebFormsParser()
        {
            startTagOpeningBracketRegex = new Regex(@"\G<[^%\/]", RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase);
            directiveRegex = new DirectiveRegex();
            commentRegex = new Regex(@"\G<%--(?<comment>.*?)--%>", RegexOptions.Multiline | RegexOptions.Singleline);
            endTagRegex = new EndTagRegex();
            aspCodeRegex = new AspCodeRegex();
            aspExprRegex = new AspExprRegex();
            aspEncodedExprRegex = new AspEncodedExprRegex();
            textRegex = new TextRegex();
            runatServerTagRegex = new RunatServerTagRegex();
            scriptRegex = new Regex(@"\G\s*\<script.*?\>.*?\<\/script\>\s*", RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase);
            doctypeRegex = new Regex(@"\G\s*\<!DOCTYPE.*?\>\s*", RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase);
        }

        private IWebFormsNodeFilterProvider NodeFilterProvider
        {
            get;
            set;
        }

        private IWebFormsNodeFactory NodeFactory
        {
            get;
            set;
        }

        [ImportingConstructor]
        public WebFormsParser(IWebFormsNodeFactory factory, IWebFormsNodeFilterProvider postprocessingFilterProvider)
        {
            NodeFactory = factory;
            NodeFilterProvider = postprocessingFilterProvider;
        }

        public IDocument<IWebFormsNode> Parse(string input)
        {
            Match match;
            int startAt = 0;

            var root = new WebFormsNode { Type = NodeType.Document };
            IWebFormsNode parentNode = root;

            do
            {
                if ((match = textRegex.Match(input, startAt)).Success)
                {
                    AppendTextNode(parentNode, match);
                    startAt = match.Index + match.Length;
                }

                if (startAt != input.Length)
                {
                    if ((match = directiveRegex.Match(input, startAt)).Success)
                    {
                        var directiveNode = NodeFactory.CreateNode(match, NodeType.Directive);
                        parentNode.Children.Add(directiveNode);
                    }
                    else if ((match = commentRegex.Match(input, startAt)).Success)
                    {
                        var commentNode = NodeFactory.CreateNode(match, NodeType.Comment);
                        parentNode.Children.Add(commentNode);
                    }
                    else if ((match = runatServerTagRegex.Match(input, startAt)).Success)
                    {
                        var serverControlNode = NodeFactory.CreateNode(match, NodeType.ServerControl);
                        parentNode.Children.Add(serverControlNode);
                        parentNode = serverControlNode;
                    }
                    else if ((match = doctypeRegex.Match(input, startAt)).Success)
                    {
                        AppendTextNode(parentNode, match);
                    }
                    else if ((match = scriptRegex.Match(input, startAt)).Success)
                    {
                        AppendTextNode(parentNode, match);
                    }
                    else if ((match = startTagOpeningBracketRegex.Match(input, startAt)).Success)
                    {
                        AppendTextNode(parentNode, match);
                    }
                    else if ((match = endTagRegex.Match(input, startAt)).Success)
                    {
                        var tagName = match.Groups["tagname"].Captures[0].Value;
                        var serverControlParent = parentNode as IWebFormsServerControlNode;
                        if (serverControlParent != null && tagName.ToLowerInvariant() == serverControlParent.TagName.ToLowerInvariant())
                        {
                            parentNode = parentNode.Parent;
                        }
                        else
                        {
                            AppendTextNode(parentNode, match);
                        }
                    }
                    else if ((match = aspExprRegex.Match(input, startAt)).Success || (match = aspEncodedExprRegex.Match(input, startAt)).Success)
                    {
                        var expressionBlockNode = NodeFactory.CreateNode(match, NodeType.ExpressionBlock);
                        parentNode.Children.Add(expressionBlockNode);
                    }
                    else if ((match = aspCodeRegex.Match(input, startAt)).Success)
                    {
                        var codeBlockNode = NodeFactory.CreateNode(match, NodeType.CodeBlock);
                        parentNode.Children.Add(codeBlockNode);
                    }
                    else
                    {
                        throw new Exception(
                            string.Format("Unrecognized page element: {0}...", input.Substring(startAt, 20)));
                    }

                    startAt = match.Index + match.Length;
                }
            }
            while (startAt != input.Length);

            ApplyPostprocessingFilters(root);

            return new Document<IWebFormsNode>(root);
        }

        private void ApplyPostprocessingFilters(IWebFormsNode rootNode)
        {
            foreach (var filter in NodeFilterProvider.Filters)
            {
                FilterChildNodes(rootNode, filter);
            }
        }

        private void FilterChildNodes(IWebFormsNode rootNode, IWebFormsNodeFilter filter)
        {
            if (rootNode.Children.Count > 0)
            {
                var filterOutput = new List<IWebFormsNode>();
                foreach (var childNode in rootNode.Children)
                {
                    FilterChildNodes(childNode, filter);
                    filterOutput.AddRange(filter.Filter(childNode, filterOutput.LastOrDefault()));
                }

                rootNode.Children.Clear();
                foreach (var filteredNode in filterOutput)
                {
                    rootNode.Children.Add(filteredNode);
                }
            }
        }

        private void AppendTextNode(IWebFormsNode parentNode, Match match)
        {
            var currentTextNode = parentNode.Children.LastOrDefault() as IWebFormsTextNode;
            if (currentTextNode == null)
            {
                currentTextNode = (IWebFormsTextNode) NodeFactory.CreateNode(match, NodeType.Text);
                parentNode.Children.Add(currentTextNode);
            }
            else
            {
                currentTextNode.Text += match.Value;
            }
        }
    }
}
