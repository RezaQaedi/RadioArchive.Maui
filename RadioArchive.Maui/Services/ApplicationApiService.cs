using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;

namespace RadioArchive.Maui
{
    public class ApplicationApiService : IApplicationApiService
    {
        private const string TOPSHOWS = "top-shows/";
        private const string PAGEOFFSET = "?&offset=";
        private const string Archiv = "?blog=HolakoueeArchiv&archive=";
        private const string PATTERN = @"Listen to (?<date>\w{3,10} \d{0,2}, \d{4})\s?(?<time>(Morning|Evening|Afternoon))?\s?(?<isReplay>\(best of week\))?";
        private Regex _regex;
        private readonly HttpClient _httpClient;

        public ApplicationApiService()
        {
            _httpClient = new HttpClient();
            _regex = new(PATTERN, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }

        /// <summary>
        /// Gets List of Last <see cref="ShowApiModel"/> Show's
        /// </summary>
        /// <returns></returns>
        public async Task<List<ShowApiModel>> GetLastShowsAsync()
        {
            return await GetShowsAsync(RouteHelper.GetAbsoluteRoute(""));
        }

        /// <summary>
        /// Get Show's list with offset 
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public async Task<List<ShowApiModel>> GetShowsWithOffsetAsync(string offset)
        {
            return await GetShowsAsync($"{RouteHelper.GetAbsoluteRoute(PAGEOFFSET)}{offset}");
        }


        /// <summary>
        /// Get Shows with specefic year and month 
        /// </summary>
        public async Task<List<ShowApiModel>> GetShowsWithSpecificDateAsync(int year, int month)
        {
            return await GetShowsAsync($"{RouteHelper.GetAbsoluteRoute(Archiv)}{year}-{month}");
        }

        /// <summary>
        /// Gets List of Top rated <see cref="ShowApiModel"/> Show's
        /// </summary>
        public async Task<List<ShowApiModel>> GetTopRatedShowsAsync()
        {
            return await GetShowsAsync(RouteHelper.GetAbsoluteRoute(TOPSHOWS));
        }

        /// <summary>
        /// Get request for <paramref name="url"/>
        /// </summary>
        /// <param name="url">Url for get request</param>
        /// <returns>List of <see cref="ShowApiModel"/> in <paramref name="url"/></returns>
        private async Task<List<ShowApiModel>> GetShowsAsync(string url)
        {
            List<ShowApiModel> podcastUrlList = null;

            try
            {
                //var html = await _httpClient.GetStringAsync(url).ConfigureAwait(false);
                using var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:99.0) Gecko/20100101 Firefox/99.0");


                var response = await _httpClient.SendAsync(request);

                var contentStr = await response.Content.ReadAsStringAsync();


                //using var responseMessage = await _httpClient.GetAsync(url);

                //responseMessage.EnsureSuccessStatusCode();

                //var contentStr = await responseMessage.Content.ReadAsStringAsync();

                podcastUrlList = GetShowsList(contentStr);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Attempted to get data from {url} with [{ex.Message}] Error");
            }


            return podcastUrlList;
        }

        /// <summary>
        /// Get list of <see cref="ShowApiModel"/> from a <paramref name="html"/> url
        /// </summary>
        /// <param name="html">Url adrees containg the speceifc format of html items</param>
        /// <returns></returns>
        private List<ShowApiModel> GetShowsList(string html)
        {
            var podcastUrlList = new List<ShowApiModel>();

            var matches = _regex.Matches(html);

            Debugger.Log(0, "Info", $"Found {matches.Count} show \n");

            var culture = CultureInfo.InvariantCulture;
            var styles = DateTimeStyles.None;
            var format = "MMMM d, yyyy";

            foreach (Match match in matches)
            {
                var groups = match.Groups;
                var isBestOfTheWeek = !string.IsNullOrEmpty(groups["isReplay"].Value);
                var time = groups["time"].Value.ToShowTime();

                if (DateTime.TryParseExact(groups["date"].Value, format, culture, styles, out var dateTime))
                {
                    // Get urls  
                    RouteHelper.GetUrlForSpeceficDate(dateTime, time, out var url, out var urlR);
                    podcastUrlList.Add(new ShowApiModel(url, urlR, dateTime, isBestOfTheWeek, time));
                }
                else
                {
                    Console.WriteLine($"Couldent pars Show in date of {groups["date"].Value} " +
                                        $"at {groups["time"].Value} " +
                                        $"is replay = {groups["isReplay"].Value} \n");

                    continue;
                }
            }

            //var htmlDocument = new HtmlDocument();
            //htmlDocument.LoadHtml(html);

            //var podcastHtml = htmlDocument.DocumentNode.Descendants("h6")
            //    .Where(node => node.GetAttributeValue("class", "")
            //    .Equals("item")).ToList();



            //foreach (var items in podcastHtml)
            //{
            //    var stringItem = items.SelectSingleNode("a").InnerText
            //        .Replace("Listen to", "")
            //        .Trim();

            //    // Make sure we have something to work with 
            //    if (string.IsNullOrEmpty(stringItem))
            //        continue;

            //    // Date text 
            //    var strDate = stringItem;
            //    var isBestOfTheWeek = false;
            //    var time = ShowTime.None;

            //    // Determine time and remove extra strings from strDate 
            //    if (stringItem.Contains("Evening"))
            //    {
            //        time = ShowTime.Evening;
            //        strDate = strDate.Replace("Evening", string.Empty);
            //    }
            //    else if (stringItem.Contains("Morning"))
            //    {
            //        time = ShowTime.Morning;
            //        strDate = strDate.Replace("Morning", string.Empty);
            //    }
            //    else if (stringItem.Contains("Afternoon"))
            //    {
            //        time = ShowTime.Afternoon;
            //        strDate = strDate.Replace("Afternoon", string.Empty);
            //    }
            //    // Best of the week 
            //    if (stringItem.Contains("(best of week)"))
            //    {
            //        isBestOfTheWeek = true;
            //        strDate = strDate.Replace("(best of week)", string.Empty);
            //    }

            //    var culture = CultureInfo.InvariantCulture;
            //    var styles = DateTimeStyles.None;

            //    var format = "MMMM d, yyyy";

            //    if (DateTime.TryParseExact(strDate.Trim(), format, culture, styles, out var dateTime))
            //    {

            //        // Get urls  
            //        RouteHelper.GetUrlForSpeceficDate(dateTime, time, out var url, out var urlR);

            //        podcastUrlList.Add(new ShowApiModel(url, urlR, dateTime, isBestOfTheWeek, time));

            //        // Link 
            //        //var a = items.Descendants("a").FirstOrDefault().GetAttributes("href", "");
            //    }
            //    else
            //        continue;
            //}

            return podcastUrlList;
        }
    }
}
