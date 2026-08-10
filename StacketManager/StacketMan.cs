using System.Net;
using System.IO.Compression;

namespace StacketManager
{
    public class StacketMan
    {
        // Reuse HttpClient instance to prevent socket exhaustion
        private static readonly HttpClient client = new HttpClient();

        public async Task DownloadPackage(string url, string folderPath, string fileName)
        {
            // Гарантируем, что папка существует на компьютере
            Directory.CreateDirectory(folderPath);

            // Объединяем путь к папке и имя файла (например, AppData\Roaming\PukiHone\LuaForPukiTerminal.zip)
            string fullOutputPath = Path.Combine(folderPath, fileName);

            using (HttpClient client = new HttpClient())
            {
                // Мимикрируем под обычный браузер (на случай базовых проверок)
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");

                byte[] fileBytes = await client.GetByteArrayAsync(url);
                await File.WriteAllBytesAsync(fullOutputPath, fileBytes);
            }
        }

        public async Task UnarchivePackage(string zipPath, string outputPath)
        { 
            ZipFile.ExtractToDirectory(zipPath, outputPath);    
        }

        public async Task DeleteArchive(string zipPath)
        {
            File.Delete(zipPath);
        }

        public async Task DeletePackage(string packageName)
        {
            Directory.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @$"\PukiHone\{packageName}", true);
        }

        public async Task UpdatePackage(string packageName)
        {

        }
    }
}
