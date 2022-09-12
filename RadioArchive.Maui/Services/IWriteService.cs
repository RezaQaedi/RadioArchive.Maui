namespace RadioArchive.Maui.Services
{
    public interface IWriteService
    {
        string PlatformFileDirectory();

        void WriteFile(string name, string text);
    }
}
