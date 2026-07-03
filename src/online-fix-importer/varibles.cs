namespace OnlineFixImporter
{

    public struct Varibles
    {
        static public string gamepath { get; set; } = "";
        static public string gamename { get; set; } = "";
        static public string gameexe { get; set; } = "";
        static public string[] args { get; } = Environment.GetCommandLineArgs();
        static public string fileFromArgs { get; set; } = args[1];
        static public string userProfilePath { get; } = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        static public string tarGamePath { get; } = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/games";

        static public void writeOutAllVar()
        {
            Console.WriteLine($"gamepath: {gamepath}");
            Console.WriteLine($"gamename: {gamename}");
            Console.WriteLine($"gameexe: {gameexe}");
            Console.WriteLine($"userProfilePath: {userProfilePath}");
            Console.WriteLine($"tarGamePath: {tarGamePath}");
            Console.WriteLine($"args: {string.Join(", ", args)}");
        }
    }
}
