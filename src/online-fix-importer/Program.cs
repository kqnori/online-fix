using System.Diagnostics;



namespace OnlineFixImporter
{
    class Program
    {



        static void Main(string[] args)
        {
            Console.WriteLine(Varibles.fileFromArgs);

            Varibles.writeOutAllVar();

            Functions.checkFiles();
            if (Varibles.fileFromArgs.EndsWith(".exe"))
            {
                if (Directory.GetFiles(Varibles.gamepath, "OnlineFix.ini", SearchOption.AllDirectories).Length == 0)
                {
                    Console.WriteLine($"This is not a valid OnlineFix game");
                    Functions.Notify("This is not a valid OnlineFix game", "error");
                    Environment.Exit(1);

                }
                Varibles.gameexe = Varibles.fileFromArgs;
                Functions.AddToFix(Varibles.gameexe);

            }
            else if (Varibles.fileFromArgs.EndsWith(".rar"))
            {

                if (!Directory.Exists(Directory.GetCurrentDirectory() + "/Fix Repair"))
                {
                    Console.WriteLine($"This is not a valid OnlineFix rar file or Fix Repair folder is missing");
                    Functions.Notify("This is not a valid OnlineFix rar file or Fix Repair folder is missing", "error");
                    Environment.Exit(1);
                }
                Archives.unrarfix(Varibles.fileFromArgs);
                Functions.AddToFix(Varibles.gameexe);

            }
            else
            {
                Console.WriteLine($"Invalid argument. Please provide a .exe or .rar file.");
                Environment.Exit(1);
            }

        }











    }
}





// Directory.SetCurrentDirectory(Directory.GetParent(fixfile).FullName);






// if (args[0] == "create")
// {
//     if (!args[1].EndsWith(".rar"))
//     {
//         Environment.Exit(1);
//     }
//     unrarfix(args[1]);
// }
// else if (args[0] == "add")
// {
//         if (!args[1].EndsWith(".exe"))
//     {
//         Environment.Exit(1);
//     }
//     AddToFix(args[1]);
// }












