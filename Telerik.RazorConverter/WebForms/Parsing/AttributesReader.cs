namespace Telerik.RazorConverter.WebForms.Parsing
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;   

    public class AttributesReader : IAttributesReader
    {
        public void ReadAttributes(Match match, IDictionary<string, string> attributes)
        {
            var nameCaptures = match.Groups["attrname"].Captures;
            var valueCaptures = match.Groups["attrval"].Captures;

            for (var i = 0; i < nameCaptures.Count; i++)
            {
                var name = nameCaptures[i].ToString().ToLowerInvariant();
                var value = valueCaptures[i].ToString();

                attributes.Add(name, value);
            }
        }
    }
}
