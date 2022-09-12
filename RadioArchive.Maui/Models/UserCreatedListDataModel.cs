namespace RadioArchive.Maui
{
    public class UserCreatedListDataModel
    {
        /// <summary>
        /// Uinqe ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Time of creation
        /// </summary>
        public DateTimeOffset Date { get; set; }

        /// <summary>
        /// Title of this playlist 
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Collection of shows this play list has 
        /// </summary>
        public ICollection<ShowDataModel> Shows { get; set; }

        public UserCreatedListDataModel(DateTimeOffset date, string title)
        {
            Date = date;
            Title = title;
        }
    }
}
