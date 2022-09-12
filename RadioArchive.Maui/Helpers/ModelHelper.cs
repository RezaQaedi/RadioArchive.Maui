namespace RadioArchive.Maui
{
    internal static class ModelHelper
    {
        /// <summary>
        /// Create's <see cref="ShowViewModel"/> from a <see cref="ShowApiModel"/>
        /// </summary>
        /// <param name="showApi">Podcast api model</param>
        /// <returns></returns>
        public static ShowViewModel ToViewModel(this ShowApiModel showApi, PlayerService mediaService,ApplicationStorgeService storgeService ,Color color = default)
        {
            var backgroundColor = color == default ? ColorHelper.GetRandomColor() : color;

            return new ShowViewModel(showApi.Date, showApi.Time, backgroundColor, mediaService,storgeService,showApi.IsBestOfTheWeek);
        }

        /// <summary>
        /// Create's <see cref="ShowApiModel"/> from a <see cref="ShowViewModel"/>
        /// </summary>
        /// <returns></returns>
        public static ShowApiModel ToPodcastUrl(this ShowViewModel showViewModel)
        {
            // Get urls 
            RouteHelper.GetUrlForSpeceficDate(showViewModel.Date, showViewModel.Time, out var filurl, out var fileUrlReferrer);

            return new ShowApiModel(filurl, fileUrlReferrer, showViewModel.Date, showViewModel.IsReplay, showViewModel.Time);
        }

        /// <summary>
        /// Matches string to specefic <see cref="ShowTime"/> returns none if none matched 
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static ShowTime ToShowTime(this string time)
        {
            return time.ToLower() switch
            {
                "morning" => ShowTime.Morning,
                "evening" => ShowTime.Evening,
                "afternoon" => ShowTime.Afternoon,
                _ => ShowTime.None,
            };
        }


        public static List<ShowItemViewModel> GetViewModels(this List<ShowApiModel> showApiModels, PlayerService mediaService, ApplicationStorgeService storgeService)
        {
            List<ShowItemViewModel> viewModels = new();

            foreach (var showApiModel in showApiModels)
            {
                var showItemViewModel = viewModels.FirstOrDefault(p => p.Date == showApiModel.Date);
                var matchShow = storgeService.Shows.FirstOrDefault(s => s.Time == showApiModel.Time && s.Date == showApiModel.Date);
                var show = matchShow?.ToViewModel(storgeService, mediaService) ??
                    showApiModel.ToViewModel(mediaService, storgeService);
                var randomColor = ColorHelper.GetRandomColor();
                //var newstWatchedShow = DI.StorgeService.GetVisitedShows().OrderBy(s => s.Date).LastOrDefault();
                //var isNew = newstWatchedShow?.Date < show.Date;

                // if we haven't any view model with that date then add it 
                if (showItemViewModel == null)
                {
                    // Create new podcast item 
                    showItemViewModel = new ShowItemViewModel(randomColor)
                    {
                        //IsNew = isNew,
                    };
                    showItemViewModel.Shows.Add(show);
                    viewModels.Add(showItemViewModel);
                }
                // if we have the same podcast with same date 
                else
                {
                    // Check for duplication 
                    var itemAlreadyExist = showItemViewModel.Shows.FirstOrDefault(p => p.Date == show.Date && p.Time == show.Time);

                    // If we already had this item do nothing 
                    if (itemAlreadyExist == null)
                    {
                        showItemViewModel.Shows.Add(show);
                    }
                }
            }


            return viewModels;
        }

        public static ShowViewModel ToViewModel(this ShowDataModel dataModel, ApplicationStorgeService storgeService, PlayerService playerService, bool removble = false)
        {
            var view = new ShowViewModel(dataModel.Date,
                   dataModel.Time,
                   ColorHelper.GetRandomColor(),playerService,storgeService,
                   dataModel.IsReplay, isRemovble: removble);

            if (dataModel.LikeDate != null)
                view.IsLiked = true;

            view.Proggress = dataModel.UserProggresion;

            return view;

        }


        public static ShowDataModel ToDataModel(this ShowViewModel viewModel, bool isVisited = false)
        {
            var data = new ShowDataModel(viewModel.Date, viewModel.Time)
            {
                IsReplay = viewModel.IsReplay,
                UserProggresion = viewModel.Proggress,
            };

            if (isVisited)
                data.LastVisit = DateTimeOffset.UtcNow;

            return data;
        }

        public static UserNotesDataModel ToDataModel(this ShowNoteViewModel viewModel)
             => new(viewModel.TextNote, viewModel.Date) { ShowPostion = viewModel.PodcastTime };


        public static ShowNoteViewModel ToViewModel(this UserNotesDataModel dataModel, ApplicationStorgeService storgeService, PlayerService playerService)
            => new(dataModel.TextNote, dataModel.Date, storgeService, playerService) { PodcastTime = dataModel.ShowPostion };

    }
}
