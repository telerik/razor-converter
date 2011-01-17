namespace Telerik.RazorConverter
{
    public interface IDocument<TNode>
    {
        TNode RootNode { get; }
    }
}
