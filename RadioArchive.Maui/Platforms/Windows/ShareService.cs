using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioArchive.Maui.Platforms.Windows
{
    class ShareService : IShareService
    {
        public async void ShareText(string text, string title)
        {
            await Clipboard.Default.SetTextAsync(text);
        }
    }
}
