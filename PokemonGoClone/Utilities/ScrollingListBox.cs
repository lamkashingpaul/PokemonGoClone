using System.Windows.Controls;

namespace PokemonGoClone.Utilities
{
    public class ScrollingListBox : ListBox
    {
        protected override void OnItemsChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                int newItemCount = e.NewItems.Count;
                if (newItemCount > 0)
                {
                    ScrollIntoView(e.NewItems[newItemCount - 1]);
                }
                base.OnItemsChanged(e);
            }
        }
    }
}
