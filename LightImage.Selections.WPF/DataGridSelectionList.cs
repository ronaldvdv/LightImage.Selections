using System.Windows.Controls;

namespace LightImage.Selections
{
    public class DataGridSelectionList<T> : SelectionListAbstract<T> where T : class
    {
        private readonly DataGrid _grid;

        public DataGridSelectionList(DataGrid grid) : base(grid.ObserveSelection<T>())
        {
            _grid = grid;
        }

        public override void Update(params T[] items)
        {
            switch (_grid.SelectionMode)
            {
                case DataGridSelectionMode.Extended:
                    UpdateExtended(items);
                    break;

                default:
                    UpdateSingle(items);
                    break;
            }
        }

        private void UpdateExtended(T[] items)
        {
            _grid.SelectedItems.Update(items);
        }

        private void UpdateSingle(T[] items)
        {
            _grid.SelectedItem = items.LastOrDefaultSafe();
        }
    }
}