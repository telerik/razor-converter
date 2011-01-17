namespace Telerik.RazorConverter.Razor.Rendering
{
    using System.Collections.Generic;

    public interface IRazorNodeRendererProvider
    {
        IList<IRazorNodeRenderer> NodeRenderers { get; }
    }
}
