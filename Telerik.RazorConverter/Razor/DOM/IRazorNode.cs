namespace Telerik.RazorConverter.Razor.DOM
{
    using System.Collections.Generic;

    public interface IRazorNode
    {
        IRazorNode Parent { get; set;  }
        IList<IRazorNode> Children { get; }
    }
}
