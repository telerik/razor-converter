namespace Telerik.RazorConverter.Razor.Converters
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Text.RegularExpressions;
    using Telerik.RazorConverter.Razor.DOM;
    using Telerik.RazorConverter.WebForms.DOM;

    [Export(typeof(INodeConverter<IRazorNode>))]
    [ExportMetadata("Order", 10)]
    public class DirectiveConverter : INodeConverter<IRazorNode>
    {
        private IRazorDirectiveNodeFactory DirectiveNodeFactory
        {
            get;
            set;
        }

        [ImportingConstructor]
        public DirectiveConverter(IRazorDirectiveNodeFactory nodeFactory)
        {
            DirectiveNodeFactory = nodeFactory;
        }

        public IList<IRazorNode> ConvertNode(IWebFormsNode node)
        {
            var result = new List<IRazorNode>();

            var directiveNode = node as IWebFormsDirectiveNode;
            if (directiveNode != null &&
                directiveNode.Attributes.ContainsKey("inherits"))
            {
                var inheritsFrom = directiveNode.Attributes["inherits"];
                var viewPageGenericType = new Regex("System.Web.Mvc.(?:ViewPage|ViewUserControl)<(?<type>.*)>");
                var typeMatch = viewPageGenericType.Match(inheritsFrom);
                if (typeMatch.Success)
                {
                    result.Add(DirectiveNodeFactory.CreateDirectiveNode("model", typeMatch.Result("${type}")));
                }
                else if (inheritsFrom != "System.Web.Mvc.ViewPage" && inheritsFrom != "System.Web.Mvc.ViewUserControl")
                {
                    result.Add(DirectiveNodeFactory.CreateDirectiveNode("inherits", directiveNode.Attributes["inherits"]));
                }
            }

            return result;
        }

        public bool CanConvertNode(IWebFormsNode node)
        {
            return node as IWebFormsDirectiveNode != null;
        }
    }
}
