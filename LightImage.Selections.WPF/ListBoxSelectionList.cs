using LightImage.Selections;
using System.Windows.Controls;

namespace LightImage.Selections
{
    public class ListBoxSelectionList<T> : SelectionListAbstract<T> where T : class
    {
        private readonly ListBox _listBox;

        public ListBoxSelectionList(ListBox listBox) : base(listBox.ObserveSelection<T>())
        {
            _listBox = listBox;
        }

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