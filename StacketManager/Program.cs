using StacketManager;

string command;
StacketMan stackman = new StacketMan();
Console.WriteLine("Welcome to StacketManager. Type 'help' for help.");

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
            await stackman.DeletePackage(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @$"\PukiHone\temp\{fullfilename}");
        }
        catch (Exception e) { DisplayError("Исключение: " + e.Message); }
    }
    else { DisplayError($"Command {arguments[0]} doesn't exist."); }
}

void DisplayHelp()
{
    Console.WriteLine();
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("List of commands:");
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine("install [url] - install zip and unarchive it");
    Console.ResetColor();
    Console.WriteLine();
}

void DisplayError(string error)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(error);
    Console.ResetColor();
}