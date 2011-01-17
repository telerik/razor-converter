namespace Telerik.RazorConverter.Tests.Razor
{
    using Moq;
    using System;
    using System.Collections.Generic;
    using Telerik.RazorConverter;
    using Telerik.RazorConverter.Razor.Converters;
    using Telerik.RazorConverter.Razor.DOM;
    using Xunit;

    public class RazorNodeConverterProviderTests
    {
        private readonly RazorNodeConverterProvider provider;
        private readonly Mock<INodeConverter<IRazorNode>> firstNodeConverter;
        private readonly Mock<INodeConverter<IRazorNode>> secondNodeConverter;
        private readonly Mock<IOrderMetadata> firstNodeConverterMetadata;
        private readonly Mock<IOrderMetadata> secondNodeConverterMetadata;

        public RazorNodeConverterProviderTests()
        {
            provider = new RazorNodeConverterProvider();

            firstNodeConverter = new Mock<INodeConverter<IRazorNode>>();
            secondNodeConverter = new Mock<INodeConverter<IRazorNode>>();
            firstNodeConverterMetadata = new Mock<IOrderMetadata>();
            secondNodeConverterMetadata = new Mock<IOrderMetadata>();


            // Define the order of the renderers
            firstNodeConverterMetadata.SetupGet(m => m.Order).Returns(1);
            secondNodeConverterMetadata.SetupGet(m => m.Order).Returns(2);

            // Add the renderers in reverse order
            provider.NodeConverterRegistrations = new List<Lazy<INodeConverter<IRazorNode>, IOrderMetadata>>()
            {
                new Lazy<INodeConverter<IRazorNode>, IOrderMetadata>(() => firstNodeConverter.Object, firstNodeConverterMetadata.Object),
                new Lazy<INodeConverter<IRazorNode>, IOrderMetadata>(() => secondNodeConverter.Object, secondNodeConverterMetadata.Object)
            };
        }

        [Fact]
        public void Should_sort_renderers_in_order()
        {
            provider.NodeConverters[0].ShouldBeSameAs(firstNodeConverter.Object);
            provider.NodeConverters[1].ShouldBeSameAs(secondNodeConverter.Object);
        }
    }
}
