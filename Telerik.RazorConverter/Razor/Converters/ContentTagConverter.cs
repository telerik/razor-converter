namespace Telerik.RazorConverter.Razor.Converters
{
    using System.Collections.Generic;
    using Telerik.RazorConverter;
    using Telerik.RazorConverter.Razor.DOM;
    using Telerik.RazorConverter.WebForms.DOM;

    public class ContentTagConverter : INodeConverter<IRazorNode>
    {
        private IRazorNodeConverterProvider NodeConverterProvider
        {
            get;
            set;
        }

        private IRazorSectionNodeFactory SectionNodeFactory
        {
            get;
            set;
        }

        private IContentTagConverterConfiguration Configuration
        {
            get;
            set;
        }

        public ContentTagConverter( IRazorNodeConverterProvider converterProvider,
                                    IRazorSectionNodeFactory sectionFactory,
                                    IContentTagConverterConfiguration converterConfiguration)
        {
            NodeConverterProvider = converterProvider;
            SectionNodeFactory = sectionFactory;
            Configuration = converterConfiguration;
        }

        public IList<IRazorNode> ConvertNode(IWebFormsNode node)
        {
            var contentTag = node as IWebFormsServerControlNode;
            var contentPlaceHolderID = contentTag.Attributes["ContentPlaceHolderID"];
            var convertedChildren = new List<IRazorNode>();

            foreach (var childNode in node.Children)
            {
                foreach (var converter in NodeConverterProvider.NodeConverters)
                {
                    if (converter.CanConvertNode(childNode))
                    {
                        convertedChildren.AddRange(converter.ConvertNode(childNode));
                    }
                }
            }

            if (string.Compare(contentPlaceHolderID, Configuration.BodyContentPlaceHolderID, true) != 0)
            {
                var sectionNode = SectionNodeFactory.CreateSectionNode(contentPlaceHolderID);
                foreach (var convertedNode in convertedChildren)
                {
                    sectionNode.Children.Add(convertedNode);
                }

                return new IRazorSectionNode[] { sectionNode };
            }
            else
            {
                return convertedChildren;
            }
        }

        public bool CanConvertNode(IWebFormsNode node)
        {
            var serverControlNode = node as IWebFormsServerControlNode;
            return serverControlNode != null && serverControlNode.TagName.ToLowerInvariant() == "asp:content";
        }
    }
}
