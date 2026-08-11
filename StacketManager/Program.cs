using StacketManager;
using Octokit;

string command;
string argsRepoName = null;
string argsPackageNameDelete = null;
StacketMan stackman = new StacketMan();
Console.WriteLine("Welcome to StacketManager. Type 'help' for help.");

while (true)
{
    command = Console.ReadLine();
    string[] arguments = command.Split(' '); // Разделение строки на аргументы (0 - сама команда, 1 и дальше - аргументы команды)

    if (arguments[0] == "help")
    {
        DisplayHelp();
    }
    else if (arguments[0] == "install")
    {
        try
        {
            string tempDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PukiHone", "temp");
            string targetDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PukiHone");

            await stackman.DownloadPackageFromGithub(arguments[1], arguments[2], tempDir);

            string[] downloadedFiles = Directory.GetFiles(tempDir);

            foreach (string downloadedFile in downloadedFiles)
            {
                string fileNameWithoutExt = Path.GetFileNameWithoutExtension(downloadedFile);
                string extension = Path.GetExtension(downloadedFile).ToLower(); // Вернет ".zip"

                if (extension == ".zip")
                {
                    string destUnzipFolder = Path.Combine(targetDir, fileNameWithoutExt);
                    await stackman.UnarchivePackage(downloadedFile, destUnzipFolder);
                    await stackman.DeleteArchive(downloadedFile);
                }
                else
                {
                    string destFile = Path.Combine(targetDir, Path.GetFileName(downloadedFile));
                    // Проверяем, существует ли файл, чтобы избежать конфликтов при перемещении
                    if (File.Exists(destFile)) File.Delete(destFile);
                    File.Move(downloadedFile, destFile);
                }
            }

            Console.WriteLine($"Package {arguments[2]} successfully installed!");
        }
        catch (Exception e) { DisplayError("Исключение: " + e.Message); }
    }
    else if (arguments[0] == "delete")
    {
        try
        {
            await stackman.DeletePackage(arguments[1]);
            Console.WriteLine($"Package {arguments[1]} successfully deleted!");
        }
        catch (Exception e) { DisplayError("Исключение: " + e.Message); }
    }
    else if (arguments[0] == "list")
    {
        try
        {
            foreach (string package in Directory.GetDirectories(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @$"\PukiHone"))
            {
                string cleanPackageName = package.Replace(Path.GetDirectoryName(package), "").Replace(@"\", "");
                if (cleanPackageName != "temp")
                {
                    Console.WriteLine(cleanPackageName);
                }
            }
        }
        catch (Exception e) { DisplayError("Исключение: " + e.Message); }
    }
    else if (arguments[0] == "online-packages")
    {
        await stackman.GetListFromFile();
    }
    else { DisplayError($"Command {arguments[0]} doesn't exist."); }

    Console.Write("> ");
}

void DisplayHelp()
{
    Console.WriteLine();
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("List of commands:");
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine("install [repo owner] [repo name] - install package");
    Console.WriteLine("delete [package name] - remove package");
    Console.WriteLine("list - get list of installed packages and it names");
    Console.WriteLine("online-packages - get list of online packages");
    Console.ResetColor();
    Console.WriteLine();
}

void DisplayError(string error)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(error);
    Console.ResetColor();
}