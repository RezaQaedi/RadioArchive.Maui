namespace RadioArchive.Maui.ViewModels
{
    public partial class IconTextViewModel
    {
        #region Public properties
        /// <summary>
        /// Text to display
        /// </summary>
        public string DisplayText { get; set; }

        /// <summary>
        /// Inidcates if this item can be removed 
        /// </summary>
        public bool IsRemoveble { get; set; }

        /// <summary>
        /// Icon to dispay 
        /// </summary>
        public IconType Icon { get; set; }
        #endregion

        public Action SelectAction { get; set; }
        public Action RemoveAction { get; set; }

        [RelayCommand]
        void Select() => SelectAction?.Invoke();

        [RelayCommand]
        void Remove() => RemoveAction?.Invoke();
    }

    public partial class UserPlayListViewModel : BaseViewModel
    {
        readonly ApplicationStorgeService _storgeService;
        readonly PlayerService _playerService;

        [ObservableProperty]
        ObservableCollection<IconTextViewModel> _userCreatedPlaylist = new();

        public UserPlayListViewModel(ApplicationStorgeService storgeService, PlayerService playerService)
        {
            _storgeService = storgeService;
            _playerService = playerService;

            // Add default items 
            UserCreatedPlaylist.Add(new IconTextViewModel() { DisplayText = "Liked", Icon = IconType.Heart, SelectAction = () => ShowLikedList() });
            UserCreatedPlaylist.Add(new IconTextViewModel() { DisplayText = "History", Icon = IconType.History, SelectAction = () => ShowUserHistoryList() });
            UserCreatedPlaylist.Add(new IconTextViewModel() { DisplayText = "", Icon = IconType.Add, SelectAction = () => CreateNewPlayList() });


            // Add already created items 
            foreach (var item in _storgeService.GetUserList())
            {
                AddNewUserCreatedItem(item.Title);
            }
        }

        private async void ShowUserHistoryList()
        {
            var viewModels = new List<ShowViewModel>();
            foreach (var dataModel in _storgeService.GetVisitedShows())
            {
                viewModels.Insert(0, dataModel.ToViewModel(_storgeService, _playerService));
            }

            var navParam = new Dictionary<string, object>
            {
                {"Items", viewModels},
                {"Title", "History"},
            };

            await Shell.Current.GoToAsync($"{nameof(ShowItemDetailPage)}", navParam);
        }

        private async void ShowLikedList()
        {
            var viewModels = new List<ShowViewModel>();
            foreach (var dataModel in _storgeService.GetLikedShows())
            {
                viewModels.Insert(0, dataModel.ToViewModel(_storgeService, _playerService));
            }

            var navParam = new Dictionary<string, object>
            {
                {"Items", viewModels},
                {"Title", "For you"},
            };

            await Shell.Current.GoToAsync($"{nameof(ShowItemDetailPage)}", navParam);
        }

        #region Private methods 
        /// <summary>
        /// Adds new item by getting name from user 
        /// </summary>
        private async void CreateNewPlayList()
        {
            var userInput = await Shell.Current.DisplayPromptAsync("List Title", "Give it a name", placeholder: "name...", maxLength: 50);

            if (string.IsNullOrEmpty(userInput))
                return;

            userInput = userInput.Trim();
            var respond = false;

            await RunCommand(() => IsBusy, async () =>
            {
                await Task.Run(() =>
                {
                    respond = _storgeService.CreateNewList(userInput);
                });
            });

            if (respond)
                // Create new item 
                AddNewUserCreatedItem(userInput);
            else
            {
                await Shell.Current.DisplayAlert("Failed to new playlist", "Something went wrong..", "ok");
            }
        }

        private void AddNewUserCreatedItem(string title)
        {
            var iconText = new IconTextViewModel()
            {
                DisplayText = title,
                Icon = IconType.UserList,
                IsRemoveble = true,
                SelectAction = () => ShowList(title),
            };

            iconText.RemoveAction = () => RemoveList(iconText);

            UserCreatedPlaylist.Insert(UserCreatedPlaylist.Count - 1, iconText);
        }

        private async void ShowList(string title)
        {
            var viewModels = new List<ShowViewModel>();
            var data = _storgeService.GetUserCreatedList(title).Shows;

            if (data is null)
            {
                await Shell.Current.DisplayAlert("Empty list", "maybe add something first", "ok");
                return;
            }

            foreach (var dataModel in data)
            {
                viewModels.Add(dataModel.ToViewModel(_storgeService, _playerService, true));
            }

            var navParam = new Dictionary<string, object>
            {
                {"Items", viewModels},
                {"Title", title},
            };

            await Shell.Current.GoToAsync($"{nameof(ShowItemDetailPage)}", navParam);
        }

        private async void RemoveList(IconTextViewModel iconText)
        {
            var respond = await Shell.Current.DisplayAlert($"Remove {iconText.DisplayText}", $"this can't be undone, are you sure ?", "yes", "no");

            if (respond)
            {
                await RunCommand(() => IsBusy, async () =>
                {
                    await Task.Run(() => _storgeService.RemoveList(iconText.DisplayText));
                });

                UserCreatedPlaylist.Remove(iconText);
            }
        }
        #endregion
    }
}
