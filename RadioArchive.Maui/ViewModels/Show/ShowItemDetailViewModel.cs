namespace RadioArchive.Maui.ViewModels
{
    public partial class ShowItemDetailViewModel : BaseViewModel, IQueryAttributable
    {
        //readonly ApplicationStorgeService _storgeService;

        [ObservableProperty]
        ObservableCollection<ShowViewModel> _shows = new();

        public ShowItemDetailViewModel()
        {
            //_storgeService = storgeService;

            //_storgeService.ShowRemoved += OnShowRemoved;
        }

        //private void OnShowRemoved(ShowViewModel show)
        //{
        //    MainThread.BeginInvokeOnMainThread(() =>
        //    {
        //        if (Shows.Contains(show))
        //            Shows.Remove(show);
        //    });
        //}

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (!query.Any())
                return;

            var items = query["Items"] as List<ShowViewModel>;
            foreach (var item in items)
            {
                Shows.Add(item);
            };

            DisplayTitle = query["Title"] as string;
        }

        [RelayCommand]
        async void GoBack() => await Shell.Current.GoToAsync("..");


        [RelayCommand]
        private async void GoToPlayerDetailPage()
        {
            await Shell.Current.GoToAsync("..");
            await Shell.Current.GoToAsync($"//{nameof(PlayerDetailPage)}");
        }
    }
}
