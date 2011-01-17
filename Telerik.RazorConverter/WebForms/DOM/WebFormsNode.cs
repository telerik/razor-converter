namespace Telerik.RazorConverter.WebForms.DOM
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;

    public class WebFormsNode : IWebFormsNode
    {
        public NodeType Type 
        { 
            get; 
            set;
        }

        public IWebFormsNode Parent 
        { 
            get;
            set;
        }

        public IList<IWebFormsNode> Children
        {
            get;
            private set;
        }

        public IDictionary<string, string> Attributes
        {
            get;
            private set;
        }

        public WebFormsNode()
        {
            Children = new ObservableCollection<IWebFormsNode>();
            ((ObservableCollection<IWebFormsNode>) Children).CollectionChanged += OnChildrenCollectionChanged;

            Attributes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        protected virtual void OnChildrenCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (IWebFormsNode childItem in e.NewItems)
                {
                    childItem.Parent = this;
                }
            }
        }
    }
}
