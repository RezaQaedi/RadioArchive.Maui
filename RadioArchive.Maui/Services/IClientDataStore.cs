namespace RadioArchive.Maui
{
    public interface IClientDataStore
    {
        /// <summary>
        /// Make shure data base set up correctly 
        /// </summary>
        /// <returns></returns>
        Task EnsureDataStoreAsync();

        /// <summary>
        /// Get's all shows 
        /// </summary>
        /// <returns></returns>
        IEnumerable<ShowDataModel> GetShows();

        /// <summary>
        /// Gets Collection of <see cref="UserCreatedListDataModel"/> that user has created 
        /// </summary>
        /// <returns></returns>
        IEnumerable<UserCreatedListDataModel> GetAllPlayLists();

        /// <summary>
        /// Gets collecton of <see cref="UserNotesDataModel"/> Exist in DB
        /// </summary>
        /// <returns></returns>
        IEnumerable<UserNotesDataModel> GetAllNotes();

        /// <summary>
        /// Updates specefic shows properties 
        /// </summary>
        /// <param name="showDataModel">Updated show</param>
        /// <returns></returns>
        void UpdateShow(ShowDataModel showDataModel);

        /// <summary>
        /// Creates  new play list 
        /// </summary>
        /// <param name="DataModel">data model to add (Title should be uinqe)</param>
        /// <returns></returns>
        void CreatePlayList(UserCreatedListDataModel DataModel);

        /// <summary>
        /// Remove specefic play list from db 
        /// </summary>
        /// <param name="title">play list title</param>
        /// <returns></returns>
        void RemovePlaylist(string title);

        /// <summary>
        /// Adds specefic show to given user list 
        /// </summary>
        /// <param name="showDataModel">show to add</param>
        /// <returns></returns>
        void AddToPlayList(ShowDataModel showDataModel, string listTitle);

        /// <summary>
        /// Remove specefic show from given play list 
        /// </summary>
        /// <param name="showDataModel">Show to remove</param>
        /// <param name="userCreatedList">Play list to remove from</param>
        /// <returns></returns>
        void RemoveFromPlayList(ShowDataModel showDataModel, UserCreatedListDataModel userCreatedList);

        /// <summary>
        /// Add notes to specefic show
        /// </summary>
        /// <param name="showDataModel">Show to add to</param>
        /// <param name="userNote">Note to add</param>
        /// <returns></returns>
        void AddNoteToShow(ShowDataModel showDataModel, UserNotesDataModel userNote);

        /// <summary>
        /// Removes specifid note
        /// </summary>
        /// <param name="userNote">Note to remove</param>
        /// <returns></returns>
        void RemoveNote(UserNotesDataModel userNote);

        /// <summary>
        /// Updates already existed note in DB
        /// </summary>
        /// <param name="userNote">Note to update</param>
        /// <returns></returns>
        void UpadteNote(UserNotesDataModel userNote);

    }
}