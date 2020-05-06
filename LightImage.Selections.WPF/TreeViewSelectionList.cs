using System.Windows.Controls;
using DynamicData;

namespace LightImage.Selections
{
    /// <summary>
    /// Selection list representing the selection in a <see cref="TreeView"/>.
    /// </summary>
    /// <remarks>
    /// Only a single item from the selection list will be selected in the <see cref="TreeView"/>.
    /// </remarks>
    /// <typeparam name="T">Type of items in the selection.</typeparam>
    public class TreeViewSelectionList<T> : SelectionListAbstract<T>
        where T : class
    {
        private readonly TreeView _treeView;

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeViewSelectionList{T}"/> class.
        /// </summary>
        /// <param name="treeView">The treeview to be bound.</param>
        public TreeViewSelectionList(TreeView treeView)
            : base(treeView.ObserveSelection<T>().ToZeroOneChangeSet())
        {
            _treeView = treeView;
        }

        /// <inheritdoc/>
        public override void Update(params T[] items)
        {
            if (items != null && items.Length > 0)
            {
                _treeView.SelectNode(items[0]);
            }
        }
    }
}