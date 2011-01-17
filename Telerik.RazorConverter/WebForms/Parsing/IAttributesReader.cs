namespace Telerik.RazorConverter.WebForms.Parsing
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;   

    public interface IAttributesReader
    {
        void ReadAttributes(Match match, IDictionary<string, string> attributes);
    }
}
