using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Windows.Controls;

namespace LightImage.Selections
{
    /// <summary>
    /// Extension methods for the <see cref="TreeView"/> class.
    /// </summary>
    public static class TreeViewExtensions
    {
        /// <summary>
        /// Synchronize the selection of a <see cref="TreeView"/> with a given other selection list.
        /// </summary>
        /// <typeparam name="T">Type of selected items.</typeparam>
        /// <param name="treeView">The treeview to be synchronized.</param>
        /// <param name="list">The other selection list.</param>
        /// <returns>Disposable representing the synchronization binding between <paramref name="list"/> and <paramref name="treeView"/>.</returns>
        public static IDisposable BindSelection<T>(this TreeView treeView, ISelectionList<T> list)
            where T : class
        {
            return list.Synchronize(treeView.GetSelectionList<T>());
        }

        /// <summary>
        /// Two-way-bind the selected item in a treeview to a property of a view model.
        /// </summary>
        /// <typeparam name="TViewModel">Type of view model.</typeparam>
        /// <typeparam name="TItem">Type of selected items.</typeparam>
        /// <param name="treeView">The treeview.</param>
        /// <param name="vm">The view model.</param>
        /// <param name="expr">Expression for the view model property.</param>
        /// <returns>Disposable representing the synchronization binding between <paramref name="list"/> and <paramref name="treeView"/>.</returns>
        public static IDisposable BindSelection<TViewModel, TItem>(this TreeView treeView, TViewModel vm, Expression<Func<TViewModel, TItem>> expr)
            where TItem : class
            where TViewModel : class
        {
            return treeView.BindSelection(vm.GetSelectionList(expr));
        }

        /// <summary>
        /// Get the <see cref="TreeViewItem"/> that represents a given view model.
        /// </summary>
        /// <param name="tree">Tree control in which to find the view model.</param>
        /// <param name="item">View model to search for.</param>
        /// <returns>The <see cref="TreeViewItem"/> that was constructed to represent the view model.</returns>
        public static TreeViewItem ContainerFromItem(this TreeView tree, object item)
        {
            return ContainerFromItem(tree.ItemContainerGenerator, item);
        }

        /// <summary>
        /// Get a <see cref="ISelectionList{T}"/> that represents the selection in a <see cref="TreeView"/>.
        /// </summary>
        /// <typeparam name="T">Type of selected items.</typeparam>
        /// <param name="treeView">The treeview to be represented.</param>
        /// <returns>Selection list representing the selection in the tree.</returns>
        public static ISelectionList<T> GetSelectionList<T>(this TreeView treeView)
            where T : class
        {
            return new TreeViewSelectionList<T>(treeView);
        }

        /// <summary>
        /// Get an observable stream showing the selected item in a <see cref="TreeView"/>.
        /// </summary>
        /// <typeparam name="T">Type of selected items.</typeparam>
        /// <param name="treeView">The treeview.</param>
        /// <returns>Observable stream emitting the selected item in the treeview.</returns>
        /// <remarks>
        /// Whenever the treeview has no selected item, or the selected item does not match <typeparamref name="T"/>,
        /// the stream will emit NULL.
        /// </remarks>
        public static IObservable<T> ObserveSelection<T>(this TreeView treeView)
            where T : class
        {
            var changes = new RxTreeViewEvents(treeView).SelectedItemChanged;
            return changes.Select(_ => treeView.SelectedItem as T).DistinctUntilChanged();
        }

        /// <summary>
        /// Select the <see cref="TreeViewItem"/> that represents a given view model.
        /// </summary>
        /// <param name="tree">Treeview in which to find the view model.</param>
        /// <param name="node">The view model to be selected.</param>
        public static void SelectNode(this TreeView tree, object node)
        {
            var item = tree.ContainerFromItem(node);
            if (item != null)
            {
                item.IsSelected = true;
            }
        }

        private static TreeViewItem ContainerFromItem(ItemContainerGenerator containerGenerator, object item)
        {
            var container = (TreeViewItem)containerGenerator.ContainerFromItem(item);
            if (container != null)
            {
                return container;
            }

            foreach (var childItem in containerGenerator.Items)
            {
                if (!(containerGenerator.ContainerFromItem(childItem) is TreeViewItem parent))
                {
                    continue;
                }

                container = ContainerFromItem(parent.ItemContainerGenerator, item);
                if (container != null)
                {
                    return container;
                }
            }

            return null;
        }
    }
}