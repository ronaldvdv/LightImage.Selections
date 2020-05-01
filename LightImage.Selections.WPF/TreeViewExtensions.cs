using LightImage.Selections;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Windows.Controls;

namespace LightImage.Selections
{
    public static class TreeViewExtensions
    {
        public static IDisposable BindSelection<T>(this TreeView treeView, ISelectionList<T> list) where T : class
        {
            return list.Synchronize(treeView.GetSelectionList<T>());
        }

        /// <summary>
        /// Two-way-bind the selected item in a treeview to a property of a view model
        /// </summary>
        /// <typeparam name="TViewModel">Type of view model</typeparam>
        /// <typeparam name="TItem">Type of selectable item</typeparam>
        /// <param name="treeView">Treeview</param>
        /// <param name="vm">View model</param>
        /// <param name="expr">Expression for the view model property</param>
        /// <returns>Disposable that can be used to destroy the binding</returns>
        public static IDisposable BindSelection<TViewModel, TItem>(this TreeView treeView, TViewModel vm, Expression<Func<TViewModel, TItem>> expr)
            where TItem : class
            where TViewModel : class
        {
            return treeView.BindSelection(vm.GetSelectionList(expr));
        }

        /// <summary>
        /// Get the TreeViewItem that represents a given view model
        /// </summary>
        /// <param name="tree">Tree control in which to find the view model</param>
        /// <param name="item">View model to search for</param>
        /// <returns>Item that was constructed to represent the view model</returns>
        public static TreeViewItem ContainerFromItem(this TreeView tree, object item)
        {
            return ContainerFromItem(tree.ItemContainerGenerator, item);
        }

        public static ISelectionList<T> GetSelectionList<T>(this TreeView treeView) where T : class
        {
            return new TreeViewSelectionList<T>(treeView);
        }

        public static IObservable<T> ObserveSelection<T>(this TreeView treeView) where T : class
        {
            var changes = new RxTreeViewEvents(treeView).SelectedItemChanged;
            return changes.Select(_ => treeView.SelectedItem as T).DistinctUntilChanged();
        }

        /// <summary>
        /// Select the TreeViewItem that represents a given view model
        /// </summary>
        /// <param name="tree">Tree control in which to find the view model</param>
        /// <param name="item">View model to search for</param>
        public static void SelectNode(this TreeView tree, object node)
        {
            var item = tree.ContainerFromItem(node);
            if (item != null)
                item.IsSelected = true;
        }

        private static TreeViewItem ContainerFromItem(ItemContainerGenerator containerGenerator, object item)
        {
            var container = (TreeViewItem)containerGenerator.ContainerFromItem(item);
            if (container != null)
                return container;

            foreach (var childItem in containerGenerator.Items)
            {
                var parent = containerGenerator.ContainerFromItem(childItem) as TreeViewItem;
                if (parent == null)
                    continue;

                container = ContainerFromItem(parent.ItemContainerGenerator, item);
                if (container != null)
                    return container;
            }

            return null;
        }
    }
}