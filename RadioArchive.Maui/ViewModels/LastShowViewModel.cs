namespace RadioArchive.Maui.ViewModels
{
    public partial class DateItemViewModel
    {
        public string Name { get; private set; }
        public int Value { get; private set; }

        public Action<int> SelectAction { get; set; }

        public DateItemViewModel(string name, int value)
        {
            Name = name;
            Value = value;
        }

        [RelayCommand]
        public void Select() => SelectAction?.Invoke(Value);
    }

    public partial class LastShowViewModel : BaseViewModel
    {
        #region Private filds 
        private const int MAXYEAR = 2022;
        private const int MINYEAR = 2007;
        #endregion

        #region Public properties 

        /// <summary>
        /// Indicates if years has been selected 
        /// </summary>
        [ObservableProperty]
        bool _isYearSelected;

        [ObservableProperty]
        ObservableCollection<DateItemViewModel> _items = new();

        #endregion

        #region Counstractor
        public LastShowViewModel()
        { 
            SetYears();
        }
        #endregion

        #region Private Methods  

        /// <summary>
        /// Gets list of <see cref="DateItemViewModel"/> for Invariant Years
        /// </summary>
        private void SetYears()
        {
            Items = new();
            for (int i = MAXYEAR; i >= MINYEAR; i--)
            {
                var item = new DateItemViewModel(i.ToString(), i)
                {
                    SelectAction = async y => await Shell.Current.GoToAsync($"{nameof(YearDetailPage)}?year={y}")
                };

                Items.Add(item);
            }
        }
        #endregion
    }
}
