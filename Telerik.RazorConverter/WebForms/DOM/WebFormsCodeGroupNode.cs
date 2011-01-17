namespace Telerik.RazorConverter.WebForms.DOM
{
    using System;
    using System.Collections.Specialized;
    using System.Linq;

    public class WebFormsCodeGroupNode : WebFormsNode, IWebFormsCodeGroupNode
    {
        public const string Boundary = "----$$";

        public string Content
        {
            get
            {
                return string.Join(Boundary, Children.Select(c => ((IWebFormsContentNode) c).Content));
            }

            set
            {
                ReplaceContent(value);
            }
        }

        private void ReplaceContent(string content)
        {
            var parts = content.Split(new string[] { Boundary }, StringSplitOptions.None);
            if (parts.Length != Children.Count)
            {
                throw new InvalidOperationException("Replacement string parts do no match number of child nodes");
            }

            for (var i = 0; i < parts.Length; i++)
            {
                var node = (IWebFormsContentNode) Children[i];
                node.Content = parts[i];
            }
        }

        protected override void OnChildrenCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            base.OnChildrenCollectionChanged(sender, e);

            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var newItem in e.NewItems)
                {
                    if (!(newItem is IWebFormsContentNode))
                    {
                        throw new InvalidOperationException("Can only add child notes implementing IWebFormsContentNode");
                    }
                }
            }
        }
    }
}
