namespace RadioArchive.Maui
{
    public class UserNotesDataModel
    {
        /// <summary>
        /// Uinqe ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Content
        /// </summary>
        public string TextNote { get; set; }

        /// <summary>
        /// Show id of this note 
        /// </summary>
        public int CurrentShowId { get; set; }

        private ShowDataModel _show;

        /// <summary>
        /// Show that this note belonged to
        /// </summary>
        public ShowDataModel Show 
        { 
            get => _show ?? throw new InvalidOperationException("Uninitialized property: " + nameof(Show));
            set => _show = value; 
        }

        /// <summary>
        /// The time this note been written
        /// </summary>
        public DateTimeOffset Date { get; set; }

        /// <summary>
        /// Postion of show when this note been written 
        /// </summary>
        public TimeSpan ShowPostion { get; set; }

        public UserNotesDataModel(string textNote, DateTimeOffset date)
        {
            Date = date;
            TextNote = textNote;
        }
    }
}
