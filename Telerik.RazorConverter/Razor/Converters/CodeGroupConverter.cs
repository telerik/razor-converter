namespace Telerik.RazorConverter.Razor.Converters
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using Telerik.RazorConverter;
    using Telerik.RazorConverter.Razor.DOM;
    using Telerik.RazorConverter.WebForms.DOM;

    [Export(typeof(INodeConverter<IRazorNode>))]
    [ExportMetadata("Order", 25)]
    public class CodeGroupConverter : INodeConverter<IRazorNode>
    {
        private IRazorNodeConverterProvider NodeConverterProvider
        {
            get;
            set;
        }

        [ImportingConstructor]
        public CodeGroupConverter(IRazorNodeConverterProvider converterProvider)
        {
            NodeConverterProvider = converterProvider;
        }

        public IList<IRazorNode> ConvertNode(IWebFormsNode node)
        {
            var result = new List<IRazorNode>();

            foreach (var childNode in node.Children)
            {
                foreach (var converter in NodeConverterProvider.NodeConverters)
                {
                    if (converter.CanConvertNode(childNode))
                    {
                        foreach (var convertedChildNode in converter.ConvertNode(childNode))
                        {
                            result.Add(convertedChildNode);
                        }
                    }
                }
            }

            return result;
        }

        public bool CanConvertNode(IWebFormsNode node)
        {
            return node as IWebFormsCodeGroupNode != null;
        }
    }
}
