using Microsoft.EntityFrameworkCore;

namespace RadioArchive.Maui
{
    public class ClientDataStore : IClientDataStore
    {
        protected ClientDataStoreDbContext _dbContext;

        public ClientDataStore(ClientDataStoreDbContext DbContext)
        {
            _dbContext = DbContext;
        }

        /// <summary>
        /// Make sure data base correctly set up
        /// </summary>
        /// <returns></returns>
        public async Task EnsureDataStoreAsync()
        {
            await _dbContext.Database.EnsureCreatedAsync();
        }

        public void AddNoteToShow(ShowDataModel showDataModel, UserNotesDataModel userNote)
        {
            var show =  _dbContext.Shows.Include(s => s.Notes)
                .FirstOrDefault(s => s.Date == showDataModel.Date && s.Time == showDataModel.Time);

            if (show == null)
            {
                show = showDataModel;
                show.Notes = new List<UserNotesDataModel>()
                {
                    userNote
                };

                _dbContext.Shows.Add(show);
            }
            else
            {
                userNote.CurrentShowId = show.Id;
                show.Notes!.Add(userNote);
            }

            _dbContext.SaveChanges();
        }

        public void AddToPlayList(ShowDataModel showDataModel, string userCreatedList)
        {
            var playlist =  _dbContext.UserPlayLists.FirstOrDefault(s => s.Title == userCreatedList);
            var show =  _dbContext.Shows.FirstOrDefault(s => s.Date == showDataModel.Date && s.Time == showDataModel.Time);

            if (show == null)
                playlist!.Shows!.Add(showDataModel);
            else
                playlist!.Shows!.Add(show);

            var a = _dbContext.SaveChanges();
        }

        public void CreatePlayList(UserCreatedListDataModel DataModel)
        {
            _dbContext.UserPlayLists.Add(DataModel);
            _dbContext.SaveChanges();
        }


        public IEnumerable<UserCreatedListDataModel> GetAllPlayLists()
        {
            return _dbContext.UserPlayLists.Include(l => l.Shows);
        }

        public void RemoveFromPlayList(ShowDataModel showDataModel, UserCreatedListDataModel userCreatedList)
        {
            var playList = _dbContext.UserPlayLists.Include(u => u.Shows)
                .FirstOrDefault(u => u.Title == userCreatedList.Title);

            if (playList == null)
                return;

            var show = playList.Shows!.FirstOrDefault(s => s.Date == showDataModel.Date && s.Time == showDataModel.Time);

            var resualt = playList.Shows!.Remove(show!);
            _dbContext.SaveChanges();
        }

        public void RemovePlaylist(string title)
        {
            var list = _dbContext.UserPlayLists.FirstOrDefault(l => l.Title == title);

            if(list != null)
                _dbContext.UserPlayLists.Remove(list);

            _dbContext.SaveChanges();
        }

        public void RemoveNote(UserNotesDataModel userNote)
        {
            var match =  _dbContext.UserNotes.FirstOrDefault(n => n.Date == userNote.Date && string.Equals(n.TextNote, userNote.TextNote));

            if (match != null)
            {
                _dbContext.UserNotes.Remove(match);
                _dbContext.SaveChanges();
            }
        }

        public void UpdateShow(ShowDataModel showDataModel)
        {
            var show = _dbContext.Shows.FirstOrDefault(s => s.Date == showDataModel.Date && s.Time == showDataModel.Time);

            if (show == null) 
            {
                // If we couldnt find this show just create new one 
                show = AddNewShow(showDataModel);
            }

            // Update propeties 
            if (showDataModel.LastVisit != null)
                show!.LastVisit = showDataModel.LastVisit;

            if (showDataModel.LikeDate != show!.LikeDate)
                show.LikeDate = showDataModel.LikeDate;

            if(showDataModel.UserProggresion != show.UserProggresion)
                show.UserProggresion = showDataModel.UserProggresion;

            _dbContext.SaveChanges();
        }

        public IEnumerable<ShowDataModel> GetShows()
        {
            return _dbContext.Shows;
        }

        public IEnumerable<UserNotesDataModel> GetAllNotes()
        {
            return _dbContext.UserNotes.Include(n => n.Show);
        }

        public void UpadteNote(UserNotesDataModel userNote)
        {
            var match =  _dbContext.UserNotes.FirstOrDefault(s => s.Date == userNote.Date);

            if (match != null)
            {
                if (userNote.TextNote == match.TextNote)
                    return;

                match.TextNote = userNote.TextNote;
                _dbContext.SaveChanges();
            }
        }

        private ShowDataModel AddNewShow(ShowDataModel showDataModel)
        {
            _dbContext.Shows.Add(showDataModel);
            _dbContext.SaveChanges();
            return _dbContext.Shows.FirstOrDefault(s => s.Equals(showDataModel));
        }
    }
}