namespace Telerik.RazorConverter
{
    using System.ComponentModel;

    public interface IOrderMetadata
    {
        [DefaultValue(int.MaxValue)]
        int Order { get; }
    }
}
