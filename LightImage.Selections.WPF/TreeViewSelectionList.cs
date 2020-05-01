using DynamicData;
using System.Windows.Controls;

namespace LightImage.Selections
{
    public class TreeViewSelectionList<T> : SelectionListAbstract<T> where T : class
    {
        private readonly TreeView _treeView;

        public TreeViewSelectionList(TreeView treeView) : base(treeView.ObserveSelection<T>().ToZeroOneChangeSet())
        {
            _treeView = treeView;
        }

        public override void Update(params T[] items)
        {
            if (items != null && items.Length > 0)
            {
                _treeView.SelectNode(items[0]);
            }
        }
    }
}