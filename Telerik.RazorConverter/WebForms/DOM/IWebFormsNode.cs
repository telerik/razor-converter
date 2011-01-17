namespace Telerik.RazorConverter.WebForms.DOM
{
    using System.Collections.Generic;

    public interface IWebFormsNode
    {
        NodeType Type { get; }
        IWebFormsNode Parent { get; set;  }
        IList<IWebFormsNode> Children { get; }
        IDictionary<string, string> Attributes { get; }
    }
}
