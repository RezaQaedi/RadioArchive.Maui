namespace RadioArchive.Maui
{
    public interface IApplicationApiService
    {
        Task<List<ShowApiModel>> GetLastShowsAsync();
        Task<List<ShowApiModel>> GetTopRatedShowsAsync();
        Task<List<ShowApiModel>> GetShowsWithOffsetAsync(string offset);
        Task<List<ShowApiModel>> GetShowsWithSpecificDateAsync(int year, int month);
    }
}
