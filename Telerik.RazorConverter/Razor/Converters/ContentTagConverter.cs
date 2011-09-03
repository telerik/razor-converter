namespace Telerik.RazorConverter.Razor.Converters
{
    using System;
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
            var shouldRender = false;
            var contentPlaceHolderID = string.Empty; 
            var convertedChildren = new List<IRazorNode>();

            if (contentTag.Attributes.ContainsKey("ID") && contentTag.TagName.ToLower() == "asp:contentplaceholder")
            {
                shouldRender = true;
            }
            else
            {
                contentPlaceHolderID = contentTag.Attributes["ContentPlaceHolderID"];
            }

            Action<IEnumerable<IRazorNode>, IRazorNode> iterateOverChildrens = (childrens, parentNode) =>
            {
                foreach (var convertedNode in childrens)
                {
                    parentNode.Children.Add(convertedNode);
                }
            };

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

            IRazorSectionNode sectionNode = null;

            if (shouldRender)
            {
                sectionNode = SectionNodeFactory.CreateSectionNode(contentTag.Attributes["ID"], true);
            }
            else if (string.Compare(contentPlaceHolderID, Configuration.BodyContentPlaceHolderID, true) != 0)
            {
                sectionNode = SectionNodeFactory.CreateSectionNode(contentPlaceHolderID);

                iterateOverChildrens(convertedChildren, sectionNode);
            }
            
            if (sectionNode != null) 
            {
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

            if (serverControlNode != null)
            {
                var tagName = serverControlNode.TagName.ToLowerInvariant();
                return (tagName == "asp:content" || tagName == "asp:contentplaceholder");
            }
            else
            {
                return false;
            }
        }
    }
}
