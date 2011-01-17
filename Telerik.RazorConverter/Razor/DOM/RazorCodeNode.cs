namespace Telerik.RazorConverter.Razor.DOM
{
    public class RazorCodeNode : RazorNode, IRazorCodeNode
    {
        public string Code
        {
            get;
            set;
        }

        public bool RequiresPrefix
        {
            get;
            set;
        }

        public bool RequiresBlock
        {
            get;
            set;
        }
    }
}
