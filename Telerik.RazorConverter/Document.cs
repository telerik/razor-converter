namespace Telerik.RazorConverter
{
    public class Document<TNode> : IDocument<TNode>
    {
        public TNode RootNode 
        { 
            get;
            private set;
        }

        public Document(TNode root)
        {
            RootNode = root;
        }
    }
}
