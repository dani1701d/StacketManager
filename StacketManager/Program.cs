using StacketManager;

string command;
string argsURL = null;
StacketMan stackman = new StacketMan();
Console.WriteLine("Welcome to StacketManager. Type 'help' for help.");

for (int i = 0; i < args.Length; i++)
{
    // Проверяем, равен ли текущий элемент --file и есть ли следующий элемент
    if (args[i] == "-install" && i + 1 < args.Length)
    {
        argsURL = args[i + 1];
        break;
    }
    else { break; }
}

if (argsURL != null)
{
    try
    {
        string fullfilename = argsURL.Split('/')[^1];
        string filename = fullfilename.Split('.')[0];

        await stackman.DownloadPackage(argsURL, Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\PukiHone\temp", fullfilename);
        await stackman.UnarchivePackage(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @$"\PukiHone\temp\{fullfilename}", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @$"\PukiHone\{filename}");
        await stackman.DeleteArchive(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @$"\PukiHone\temp\{fullfilename}");

        Console.WriteLine($"Package {filename} successfully installed!");
    }
    catch (Exception e) { DisplayError("Exception: " + e.Message); }
}
else
{
    while (true)
    {
        Console.Write("> ");
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
                string fullfilename = arguments[1].Split('/')[^1];
                string filename = fullfilename.Split('.')[0];

                await stackman.DownloadPackage(arguments[1], Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\PukiHone\temp", fullfilename);
                await stackman.UnarchivePackage(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @$"\PukiHone\temp\{fullfilename}", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @$"\PukiHone\{filename}");
                await stackman.DeleteArchive(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @$"\PukiHone\temp\{fullfilename}");
                Console.WriteLine($"Package {arguments[1]} successfully installed!");
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
        else { DisplayError($"Command {arguments[0]} doesn't exist."); }
    }
}

void DisplayHelp()
{
    Console.WriteLine();
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("List of commands:");
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine("install [url] - install package");
    Console.WriteLine("delete [package name] - remove package");
    Console.WriteLine("list - get list of installed packages");
    Console.ResetColor();
    Console.WriteLine();
}

void DisplayError(string error)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(error);
    Console.ResetColor();
}