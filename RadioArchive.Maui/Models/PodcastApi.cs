namespace RadioArchive.Maui
{
    public class ShowApiModel
    {
        public string Url { get; set; }
        public string UrlReferer { get; set; }
        public bool IsBestOfTheWeek { get; set; }
        public ShowTime Time { get; set; }
        public DateTimeOffset Date { get; set; }

        public ShowApiModel(string url, 
            string urlReferer, 
            DateTimeOffset date,
            bool isBestOfTheWeek = false,
            ShowTime time = ShowTime.Morning)
        {
            Url = url;
            UrlReferer = urlReferer;
            IsBestOfTheWeek = isBestOfTheWeek;
            Time = time;
            Date = date;
        }
    }
}
