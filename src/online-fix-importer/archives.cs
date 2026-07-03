using System.Diagnostics;


namespace OnlineFixImporter
{
    public class Archives
    {
         public static void unrarfix(string fixfile)
        {



            Console.WriteLine($"Current directory: {Directory.GetCurrentDirectory()}");

            string mainfile = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.rar")[0];
            // string fixfile = Directory.GetFiles(Directory.GetCurrentDirectory() + "/Fix Repair", "*.rar")[0];
            Console.WriteLine($"Main file: {mainfile}");

            unrar(mainfile, Directory.GetCurrentDirectory() + "/game");

            unrar(fixfile, Directory.GetCurrentDirectory() + "/fix");

            string gamefolder = Directory.GetDirectories(Directory.GetCurrentDirectory() + "/game")[0];

            Console.WriteLine($"Game folder: {gamefolder}");
            string fixfolder = Directory.GetCurrentDirectory() + "/fix";

            Console.WriteLine($"Fix folder: {fixfolder}");
            Functions.MoveDirectoryOverwrite(fixfolder, gamefolder);



            string dirtomove = Directory.GetDirectories(Directory.GetCurrentDirectory() + "/game")[0];
            Varibles.gamename = Path.GetFileName(dirtomove);
            Console.WriteLine($"Game name: {Varibles.gamename}");


            Varibles.gamepath = Path.Combine(Varibles.tarGamePath, Varibles.gamename);

            foreach (var file in Directory.GetFiles(dirtomove))
            {
                Console.WriteLine($"File: {file}");
            }

            Directory.Move(dirtomove, Varibles.gamepath);

            // delete old folders and rars
            Directory.Delete(Directory.GetCurrentDirectory() + "/game", true);
            Directory.Delete(Directory.GetCurrentDirectory() + "/Fix Repair", true);
            File.Delete(mainfile);
            foreach (var file in Directory.GetFiles(Varibles.gamepath, "*.exe"))
            {
                if (!file.Contains("Unity"))
                {
                    Varibles.gameexe = file;

                    break;
                }
            }
        }
        static void unrar(string archivePath, string outputDir)
        {
            var sevenZipPath = "7z";
            // var archivePath = @"C:\temp\archive.rar";
            // var outputDir = @"\out";
            var password = "online-fix.me";

            var psi = new ProcessStartInfo
            {
                FileName = sevenZipPath,
                Arguments = $"x \"{archivePath}\" -o\"{outputDir}\" -p\"{password}\" -y",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using var process = Process.Start(psi);
            string stdout = process!.StandardOutput.ReadToEnd();
            string stderr = process.StandardError.ReadToEnd();
            process.WaitForExit();

            Console.WriteLine(stdout);
            Console.WriteLine(stderr);
            Console.WriteLine($"Exit code: {process.ExitCode}");
        }
    }
}