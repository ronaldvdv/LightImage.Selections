namespace LightImage.Selections
{
    /// <summary>
    /// Contract for any view model that allows its selection state to be queried and changed directly.
    /// </summary>
    public interface ISelectable
    {
        /// <summary>
        /// Gets or sets a value indicating whether the item is selected.
        /// </summary>
        bool IsSelected { get; set; }
    }
}