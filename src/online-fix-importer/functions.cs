using Newtonsoft.Json.Linq;
using System.Diagnostics;

using System.Net;
using System;

namespace OnlineFixImporter
{
    public class Functions
    {
        static public void checkFiles()
        {

            Directory.CreateDirectory(Varibles.userProfilePath + "/.config/OFME-Linux/icons");
            Directory.CreateDirectory(Varibles.userProfilePath + "/.config/OFME-Linux/banners");
            Directory.CreateDirectory(Varibles.userProfilePath + "/.local/share/OnlineFix Linux Launcher/prefixes");
            Directory.CreateDirectory(Varibles.tarGamePath);



        }
        static public void MoveDirectoryOverwrite(string sourceDir, string destDir)
        {
            Directory.CreateDirectory(destDir);


            foreach (var file in Directory.GetFiles(sourceDir, "*", SearchOption.AllDirectories))
            {
                var target = Path.Combine(destDir, Path.GetRelativePath(sourceDir, file));
                Directory.CreateDirectory(Path.GetDirectoryName(target)!);

                if (File.Exists(target))
                    File.Delete(target);

                File.Move(file, target);
            }

            Directory.Delete(sourceDir, true);
        }
        static void ExtractIcon(string exePath, string outputPngPath)
        {
            var process = Process.Start(new ProcessStartInfo
            {
                FileName = "icoextract",
                Arguments = $"\"{exePath}\" \"{outputPngPath}\"",
                RedirectStandardOutput = true,
                UseShellExecute = false
            });
            process?.WaitForExit();
        }
        static public void AddToFix(string gameexe)
        {


            Varibles.gamepath = Path.GetFullPath(gameexe).Replace(Path.GetFileName(gameexe), "");
            Varibles.gamename = Path.GetFileName(gameexe);
            string gameid = "";


            string fixinit = Directory.GetFiles(Varibles.gamepath, "OnlineFix.ini", SearchOption.AllDirectories)[0];
            foreach (var line in File.ReadLines(fixinit))
            {
                if (line.Contains("RealAppId"))
                {
                    gameid = line.Split('=')[1].Trim();

                    break;
                }
            }
            ExtractIcon(gameexe, $"{Varibles.userProfilePath}/.config/OFME-Linux/icons/{gameid}ico");

            // Image image = Image.FromFile($"{userProfilePath}/.config/OFME-Linux/icons/{gameid}ico");
            // using var image = SixLabors.ImageSharp.Image.Load($"{userProfilePath}/.config/OFME-Linux/icons/{gameid}ico");

            var icoPath = $"{Varibles.userProfilePath}/.config/OFME-Linux/icons/{gameid}ico";
            var pngPath = $"{Varibles.userProfilePath}/.config/OFME-Linux/icons/{gameid}";

            var result = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = "icotool",
                Arguments = $"-x -o {pngPath} {icoPath}",  // extracts largest image as png
                RedirectStandardOutput = true,
                UseShellExecute = false
            });
            result!.WaitForExit();

            string get;
            using (WebClient client = new WebClient())
            {
                get = client.DownloadString($"https://store.steampowered.com/api/appdetails?appids={gameid}");
            }
            //File.WriteAllText($"{userProfilePath}/{gameid}.json", get);


            dynamic json = JObject.Parse(get);
            string headerImage = json[gameid.ToString()]["data"]["header_image"];
            Console.WriteLine(headerImage);


            using (WebClient client = new WebClient())
            {
                client.DownloadFile(headerImage, $"{Varibles.userProfilePath}/.config/OFME-Linux/banners/{gameid}header.png");
            }




            // image.SaveAsPng($"{userProfilePath}/.config/OFME-Linux/icons/{gameid}");




            File.Delete($"{Varibles.userProfilePath}/.config/OFME-Linux/icons/{gameid}ico");


            Console.WriteLine($"Fix ini file: {fixinit}");
            // string gameid = File.ReadAllText(Varibles.gamepath + "/steam_appid.txt");

            Directory.CreateDirectory($"{Varibles.userProfilePath}/.local/share/OnlineFix Linux Launcher/prefixes/{Varibles.gamename.Split('.')[0]}");
            string prefix = @$"[{Varibles.gamename.Split('.')[0]}]
fakeSteamID=480
steamID={gameid}
icon={Varibles.userProfilePath}/.config/OFME-Linux/icons/{gameid}
overrides=steam_api64=n;stubdrm64=n;winmm=n,b;onlinefix64=n;steamoverlay64=n
executable={gameexe}
mainPath={Varibles.gamepath}
prefixPath={Varibles.userProfilePath}/.local/share/OnlineFix Linux Launcher/prefixes/{Varibles.gamename.Split('.')[0]}
proton=GE-Proton10-34
steamRuntime=1
steamOverlay=1
wined3d=
nativeWayland=
fixPath=
banner={Varibles.userProfilePath}/.config/OFME-Linux/banners/{gameid}header.png" + "\n\n";

            File.AppendAllText(Varibles.userProfilePath + "/.config/OFME-Linux/Games.ini", prefix);
            Notify($"Game {Varibles.gamename} has been added to OnlineFix Linux Launcher", "info");
        }
        static public void Notify(string message, string type)
        {
            // string type = typeI switch
            // {
            //     0 => "info",
            //     1 => "error",
            // };
            Process.Start(new ProcessStartInfo
            {
                FileName = "zenity",
                Arguments = $"--{type} --title=\"Onlinefix importer\" --text=\"{message}\" --no-wrap",
                UseShellExecute = false
            })?.WaitForExit();
        }
    }
    
}