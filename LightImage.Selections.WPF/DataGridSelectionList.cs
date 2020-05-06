using System.Windows.Controls;

namespace LightImage.Selections
{
    /// <summary>
    /// Selection list representing the selection in a datagrid.
    /// </summary>
    /// <remarks>
    /// The <see cref="DataGrid.SelectionMode"/> property is taken into account. Note that if the
    /// datagrid is in <see cref="DataGridSelectionMode.Single"/> mode and synchronized with a selection list
    /// holding more than one selected item, only the last will be selected in the <see cref="DataGrid"/>.
    /// </remarks>
    /// <typeparam name="T">Type of items in the selection.</typeparam>
    public class DataGridSelectionList<T> : SelectionListAbstract<T>
        where T : class
    {
        private readonly DataGrid _grid;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataGridSelectionList{T}"/> class.
        /// </summary>
        /// <param name="grid">Datagrid to be bound.</param>
        public DataGridSelectionList(DataGrid grid)
            : base(grid.ObserveSelection<T>())
        {
            _grid = grid;
        }

        /// <inheritdoc/>
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