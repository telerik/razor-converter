namespace Telerik.RazorConverter.Razor.DOM
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;

    public class RazorNode : IRazorNode
    {
        public IRazorNode Parent
        {
            get;
            set;
        }

        public IList<IRazorNode> Children
        {
            get;
            set;
        }
        
        public RazorNode()
        {
            Children = new ObservableCollection<IRazorNode>();
            ((ObservableCollection<IRazorNode>) Children).CollectionChanged += OnChildrenCollectionChanged;
        }

        protected virtual void OnChildrenCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (IRazorNode childNode in e.NewItems)
                {
                    childNode.Parent = this;
                }
            }
        }
    }
}
