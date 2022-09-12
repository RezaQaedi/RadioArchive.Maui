namespace RadioArchive.Maui.ViewModels;

public partial class ShowItemListViewModel : BaseViewModel
{
    #region private properties 
    /// <summary>
    /// Indicates this list is shorter version of another 
    /// </summary>
    [ObservableProperty]
    private bool _hasMoreContent;

    /// <summary>
    /// Observable list of <see cref="ShowItemViewModel"/>
    /// </summary>
    [ObservableProperty]
    private List<ShowItemViewModel> _items = new();

    #endregion

    public ShowItemListViewModel()
    {
    }

    [RelayCommand]
    private async void OpenList()
    {
        if (Items.Any())
        {
            var month = Items[0].Date.Month;
            var year = Items[0].Date.Year;

            // Navigate to this month page  
            var navParam = new Dictionary<string, object>
            {
            {"Year", year},
            {"Month", month}
            };

            await Shell.Current.GoToAsync($"{nameof(ShowItemListDetailPage)}", navParam);
        }
    }
}
