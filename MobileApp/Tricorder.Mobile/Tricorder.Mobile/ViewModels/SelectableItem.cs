namespace Tricorder.Mobile.ViewModels
{
    public class SelectableItem<T> : Observable
    {
        private bool _isSelected;

        public SelectableItem(T item)
        {
            this.Item = item;
        }

        public bool IsSelected { get => _isSelected; set => SetProperty(ref _isSelected, value); }
        public T Item { get ; }
    }
}
