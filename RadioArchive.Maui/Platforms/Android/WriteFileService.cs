using Application = Android.App.Application;

namespace RadioArchive.Maui.Platforms.Android
{
    public class WriteFileService : IWriteService
    {
        public string PlatformFileDirectory() 
            => Application.Context.GetExternalFilesDir(null).AbsolutePath;

        public void WriteFile(string name, string text)
        {
            string filesPath = Application.Context.GetExternalFilesDir(null).AbsolutePath;
            string filePath = Path.Combine(filesPath, name);
            File.WriteAllText(filePath, text);
        }
    }
}
