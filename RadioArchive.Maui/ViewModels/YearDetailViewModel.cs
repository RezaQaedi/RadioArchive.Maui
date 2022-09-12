namespace RadioArchive.Maui.ViewModels
{
    public partial class YearDetailViewModel : BaseViewModel, IQueryAttributable
    {
        [ObservableProperty]
        ObservableCollection<DateItemViewModel> _items = new();

        public YearDetailViewModel()
        {
            SetMonth();
        }

        /// <summary>
        /// Sets given month as selected and Opens page to selected date 
        /// </summary>
        /// <param name="month">Month to Select</param>
        public async void SelectMonth(int month)
        {

            // Navigate to this month page  
            var navParam = new Dictionary<string, object>
            {
                {"Year", DisplayTitle},
                {"Month", month}
            };

            await Shell.Current.GoToAsync($"{nameof(ShowItemListDetailPage)}", navParam);
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (!query.Any())
                return;

            DisplayTitle = query["year"].ToString();
        }

        /// <summary>
        /// Gets list of <see cref="DateItemViewModel"/> for Invariant months
        /// </summary>
        private void SetMonth()
        {
            Items.Clear();
            for (int i = 1; i <= 12; i++)
            {
                var item = new DateItemViewModel(TimeHelper.GetInvariantMonthName(i), i)
                {
                    SelectAction = SelectMonth
                };
                Items.Add(item);
            }
        }

        [RelayCommand]
        public async void GoBack() => await Shell.Current.GoToAsync("..");
    }
}
