using System.Windows.Controls;

namespace LightImage.Selections
{
    /// <summary>
    /// Selection list representing the selection in a <see cref="ListBox"/>.
    /// </summary>
    /// <remarks>
    /// The <see cref="ListBox.SelectionMode"/> property is taken into account. Note that if the
    /// list is in <see cref="SelectionMode.Single"/> mode while the selection list is synchronized
    /// with a list holding more than one item, only the last will be selected in the <see cref="ListBox"/>.
    /// </remarks>
    /// <typeparam name="T">Type of items allowed in the selection.</typeparam>
    public class ListBoxSelectionList<T> : SelectionListAbstract<T>
        where T : class
    {
        private readonly ListBox _listBox;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListBoxSelectionList{T}"/> class.
        /// </summary>
        /// <param name="listBox">The listbox to be represented.</param>
        public ListBoxSelectionList(ListBox listBox)
            : base(listBox.ObserveSelection<T>())
        {
            _listBox = listBox;
        }

        /// <inheritdoc/>
        public override void Update(params T[] items)
        {
            switch (_listBox.SelectionMode)
            {
                case SelectionMode.Extended:
                    UpdateExtended(items);
                    break;

                default:
                    UpdateSingle(items);
                    break;
            }
        }

        private void UpdateExtended(T[] items)
        {
            _listBox.SelectedItems.Update(items);
        }

        private void UpdateSingle(T[] items)
        {
            _listBox.SelectedItem = items.LastOrDefaultSafe();
        }
    }
}