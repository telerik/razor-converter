namespace Telerik.RazorConverter.WebForms.Filters
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Text.RegularExpressions;
    using Telerik.RazorConverter.WebForms.DOM;

    [Export(typeof(IWebFormsNodeFilter))]
    [ExportMetadata("Order", 30)]
    public class WrapNestedTemplatesFilter : IWebFormsNodeFilter
    {
        public IList<IWebFormsNode> Filter(IWebFormsNode node, IWebFormsNode previousFilteredNode)
        {
            var isCodeGroupNode = node is IWebFormsCodeGroupNode;
            if (isCodeGroupNode)
            {
                var codeContentNode = node as IWebFormsContentNode;
                codeContentNode.Content = WrapNestedTemplates(codeContentNode.Content);
            }

            return new IWebFormsNode[] { node };
        }

        private string WrapNestedTemplates(string content)
        {
            var nestedTemplateRegex = new Regex(
                @"\(\)\s*\=\>\s*\{(?<template>(?>[^{}]+|\{(?<Depth>)|\}(?<-Depth>))*(?(Depth)(?!)))\}", RegexOptions.Singleline);

            return nestedTemplateRegex.Replace(content, m =>
            {
                return string.Format("@<text>{0}</text>", m.Groups["template"].Value);
            });
        }
    }
}
