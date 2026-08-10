using System.IO.Compression;
using Octokit;

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

        public async Task GetListFromFile()
        {
            // 1. Define the raw file URL
            string url = "https://raw.githubusercontent.com/dani1701d/Packages-List-for-PukiHone/refs/heads/main/PackagesList.txt";

            using HttpClient client = new HttpClient();

            // 2. GitHub API requires a User-Agent header
            client.DefaultRequestHeaders.Add("User-Agent", "C#-App");

            try
            {
                // 3. Fetch the content as a string
                string fileContent = await client.GetStringAsync(url);
                Console.WriteLine(fileContent);
            }
            catch (Exception e) { DisplayError("Исключение: " + e.Message); }
        }

        /* Пока не нужно.
        public async Task UpdatePackage(string owner, string reponame, string path) // путь БЕЗ САМОГО ФАЙЛА, ТОЛЬКО ДО ДИРЕКТОРИИ!!!
        {
            var client = new GitHubClient(new ProductHeaderValue("MyApp"));
            // client.Credentials = new Credentials("ВАШ_ТОКЕН"); // Если репозиторий приватный

            var latestRelease = await client.Repository.Release.GetLatest(owner, reponame);

            foreach (var asset in latestRelease.Assets)
            {
                string fileName = asset.Name;

                // Решение: Запрашиваем байты файла напрямую через URI ресурса API
                var response = await client.Connection.Get<byte[]>(
                    new System.Uri(asset.Url),
                    new System.Collections.Generic.Dictionary<string, string>(),
                    "application/octet-stream" // Обязательный заголовок для получения файла
                );

                // В ответе будет содержаться массив байт (Body)
                byte[] fileData = response.Body;

                if (!path.EndsWith(@"\")) { path = path + @"\"; }

                await File.WriteAllBytesAsync(path + fileName, fileData);
                Console.WriteLine($"Файл успешно скачан.");
            }
        }
        */

        void DisplayError(string error)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(error);
            Console.ResetColor();
        }
    }
}
