namespace RadioArchive.Maui
{
    public class ApplicationStorgeService
    {
        #region Private filds 
        private readonly IClientDataStore _clientDataStore;

        /// <summary>
        /// list of shows loaded from db  
        /// </summary>
        public List<ShowDataModel> Shows { get; private set; } = new();

        /// <summary>
        /// list of user created list 
        /// </summary>
        private List<UserCreatedListDataModel> _userPlaylist = new();

        /// <summary>
        /// List of notes user created 
        /// </summary>
        private List<UserNotesDataModel> _notes = new();
        #endregion

        public event Action<ShowNoteViewModel> NoteRemoved;
        public event Action<ShowViewModel> ShowRemoved;

        public ApplicationStorgeService(IClientDataStore clientDataStore)
        {
            _clientDataStore = clientDataStore;

            Load();
        }

        /// <summary>
        /// Storge Data base data
        /// </summary>
        public void Load()
        {
            var shows = _clientDataStore.GetShows().ToList();

            Shows.AddRange(shows);
            _userPlaylist.AddRange(_clientDataStore.GetAllPlayLists().ToList());
            _notes.AddRange(_clientDataStore.GetAllNotes().ToList());

            var history = shows.Where(s => s.LastVisit != null);
            foreach (var show in history)
            {
                System.Diagnostics.Debug.WriteLine($"{show.Date} has {show.UserProggresion} proggression");
            }
        }

        #region Show methods  
        public IEnumerable<ShowDataModel> GetLikedShows()
        {
            return Shows.Where(s => s.LikeDate != null).OrderBy(s => s.LikeDate);
        }

        public IEnumerable<ShowDataModel> GetVisitedShows()
        {
            return Shows.Where(s => s.LastVisit != null).OrderBy(s => s.LastVisit);
        }

        public ShowDataModel GetShow(ShowViewModel viewModel) => Shows.FirstOrDefault(s => viewModel.Equals(s));

        public void UpdateLike(ShowViewModel podcastView)
        {
            var match = Shows.FirstOrDefault(s => podcastView.Equals(s));

            if (match == null)
            {
                match = podcastView.ToDataModel();
                Shows.Add(match);
            }

            match.LikeDate = podcastView.IsLiked ? DateTimeOffset.UtcNow : null;
            _clientDataStore.UpdateShow(match);
        }

        public void UpdateVisitDate(ShowViewModel podcastView)
        {
            var match = Shows.FirstOrDefault(s => podcastView.Equals(s));
            if (match == null)
            {
                match = podcastView.ToDataModel();
                // Add new one 
                Shows.Add(match);
            }

            // Update
            match.LastVisit = DateTimeOffset.UtcNow;
            // Update db 
            _clientDataStore.UpdateShow(match);
        }

        public void UpdateProggress(ShowViewModel podcastView)
        {
            var data = Shows.FirstOrDefault(s => podcastView.Equals(s));

            if (data != null)
            {
                data.UserProggresion = podcastView.Proggress;
                System.Diagnostics.Debug.WriteLine($"I'm Updating {podcastView.DisplayDate} with {podcastView.Proggress}");
                _clientDataStore.UpdateShow(data);
            }
        }
        #endregion

        #region Playlist Methods 
        public IEnumerable<ShowDataModel> GetShows(string Title)
        {
            return _userPlaylist.FirstOrDefault(s => string.Equals(s.Title, Title))?.Shows;
        }

        public void AddShow(ShowViewModel show, string ListTitle)
        {
            var plaList = _userPlaylist.FirstOrDefault(s => string.Equals(s.Title, ListTitle));

            if (plaList.Shows == null)
                plaList.Shows = new List<ShowDataModel>();

            // TODO : Let user know we already had this item 
            if (plaList.Shows.FirstOrDefault(s => show.Equals(s)) != null)
                return;

            //plaList.Shows.Add(show.ToDataModel()); // it add to Database as well ? why ?
            _clientDataStore.AddToPlayList(show.ToDataModel(), ListTitle);
        }

        public void RemoveShowFromPlayList(ShowViewModel podcast, string title)
        {
            var matchList = _userPlaylist.FirstOrDefault(l => string.Equals(title, l.Title));

            var matchShow = matchList?.Shows.FirstOrDefault(s => podcast.Equals(s));

            if (matchShow != null)
            {
                _clientDataStore.RemoveFromPlayList(matchShow, matchList);
                matchList.Shows.Remove(matchShow);
                ShowRemoved?.Invoke(podcast);
            }
        }

        public bool CreateNewList(string title)
        {
            // Check if we had list with same title 
            if (_userPlaylist.FirstOrDefault(l => l.Title == title) != null)
                return false;

            var newList = new UserCreatedListDataModel(DateTimeOffset.UtcNow, title);
            _userPlaylist.Add(newList);

            // update db
            _clientDataStore.CreatePlayList(newList);

            return true;
        }

        public UserCreatedListDataModel GetUserCreatedList(string key) => _userPlaylist.FirstOrDefault(l => l.Title == key);

        public IEnumerable<UserCreatedListDataModel> GetUserList() => _userPlaylist;

        public void RemoveList(string title)
        {
            var list = _userPlaylist.FirstOrDefault(s => string.Equals(s.Title, title));

            if (list != null)
            {
                _clientDataStore.RemovePlaylist(title);
                _userPlaylist.Remove(list);
            }
        }
        #endregion

        #region Note methods 

        public IEnumerable<UserNotesDataModel> GetUserNotes(ShowViewModel podcastViewModel)
        {
            return _notes.Where(s => podcastViewModel.Equals(s.Show));
        }

        public void AddNotes(ShowViewModel podcastViewModel, ShowNoteViewModel noteViewModel)
        {
            var NoteData = noteViewModel.ToDataModel();
            NoteData.Show = podcastViewModel.ToDataModel();
            // Update DB
            _clientDataStore.AddNoteToShow(podcastViewModel.ToDataModel(), noteViewModel.ToDataModel());
            // Update storge 
            _notes.Add(NoteData);
        }

        public void RemoveNote(ShowNoteViewModel noteViewModel)
        {
            var match = _notes.Find(n => n.Date == noteViewModel.Date && string.Equals(n.TextNote, noteViewModel.TextNote));

            if (match != null)
            {
                _clientDataStore.RemoveNote(match);
                _notes.Remove(match);
                NoteRemoved?.Invoke(noteViewModel);
            }
        }

        public void UpdateNote(ShowNoteViewModel noteItemViewModel)
        {
            var match = _notes.FirstOrDefault(n => n.Date == noteItemViewModel.Date);

            // Update chach
            match.TextNote = noteItemViewModel.TextNote;
            _clientDataStore.UpadteNote(match);
        }
        #endregion
    }
}
