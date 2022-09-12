namespace RadioArchive.Maui
{
    public class ShowDataModel
    {
        /// <summary>
        /// Uinqe ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Date of release 
        /// </summary>
        public DateTimeOffset Date { get; set; }

        /// <summary>
        /// Time of day 
        /// </summary>
        public ShowTime Time { get; set; }

        /// <summary>
        /// Proggrestion between 0 and 1 of user watached this show 
        /// </summary>
        public float UserProggresion { get; set; }

        /// <summary>
        /// Date when user liked this item 
        /// </summary>
        public DateTimeOffset? LikeDate { get; set; }

        /// <summary>
        /// last Date when user visited this item
        /// </summary>
        public DateTimeOffset? LastVisit { get; set; }

        /// <summary>
        /// Is this best of the week 
        /// </summary>
        public bool IsReplay { get; set; }

        /// <summary>
        /// Collection of notes that this item has 
        /// </summary>
        public ICollection<UserNotesDataModel> Notes { get; set; }

        /// <summary>
        /// Collection of playlist this show has been added to
        /// </summary>
        public ICollection<UserCreatedListDataModel> PlayList { get; set; }


        public ShowDataModel(DateTimeOffset date, ShowTime time)
        {
            Date = date;
            Time = time;
        }
    }
}
