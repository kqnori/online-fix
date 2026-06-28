using System.Diagnostics;
using Newtonsoft.Json.Linq;
using System.Net;
using System;




string gamepath = "";
string gamename = "";
string gameexe = "";

Console.WriteLine(args[0]);
string userProfilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
string tarGamePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/games";

if (!Directory.Exists(userProfilePath + "/.config/OFME-Linux"))
{
    Directory.CreateDirectory(userProfilePath + "/.config/OFME-Linux");
}
if (!Directory.Exists(userProfilePath + "/.config/OFME-Linux/icons"))
{
    Directory.CreateDirectory(userProfilePath + "/.config/OFME-Linux/icons");
}
if (!Directory.Exists(userProfilePath + "/.config/OFME-Linux/banners"))
{
    Directory.CreateDirectory(userProfilePath + "/.config/OFME-Linux/banners");
}
if (!Directory.Exists(userProfilePath + "/.local/share/OnlineFix Linux Launcher/prefixes"))
{
    Directory.CreateDirectory(userProfilePath + "/.local/share/OnlineFix Linux Launcher/prefixes");
}
if (!Directory.Exists(tarGamePath))
{
    Directory.CreateDirectory(tarGamePath);
}

if (args[0].EndsWith(".exe"))
{
    AddToFix(args[0]);
}
else if (args[0].EndsWith(".rar"))
{
    unrarfix(args[0]);
}
else
{
    Environment.Exit(1);
}


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

void unrarfix(string fixfile)
{


    Directory.SetCurrentDirectory(Directory.GetParent(fixfile).FullName);

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
    MoveDirectoryOverwrite(fixfolder, gamefolder);


    static void MoveDirectoryOverwrite(string sourceDir, string destDir)
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
    string dirtomove = Directory.GetDirectories(Directory.GetCurrentDirectory() + "/game")[0];
    gamename = Path.GetFileName(dirtomove);
    Console.WriteLine($"Game name: {gamename}");
    

    gamepath = Path.Combine(userProfilePath, "Documents", "games", gamename);

    foreach (var file in Directory.GetFiles(dirtomove))
    {
        Console.WriteLine($"File: {file}");
    }

    Directory.Move(dirtomove, gamepath);

    // delete old folders and rars
    Directory.Delete(Directory.GetCurrentDirectory() + "/game", true);
    Directory.Delete(Directory.GetCurrentDirectory() + "/Fix Repair", true);
    File.Delete(mainfile);
    foreach (var file in Directory.GetFiles(gamepath, "*.exe"))
    {
        if (!file.Contains("Unity"))
        {
            gameexe = file;

            break;
        }
    }
    AddToFix(gameexe);
}





void AddToFix(string gameexe)
{


    gamepath = Path.GetFullPath(gameexe).Replace(Path.GetFileName(gameexe), "");
    gamename = Path.GetFileName(gameexe);
    string gameid = "";


    string fixinit = Directory.GetFiles(gamepath, "OnlineFix.ini", SearchOption.AllDirectories)[0];
    foreach (var line in File.ReadLines(fixinit))
    {
        if (line.Contains("RealAppId"))
        {
            gameid = line.Split('=')[1].Trim();

            break;
        }
    }
    ExtractIcon(gameexe, $"{userProfilePath}/.config/OFME-Linux/icons/{gameid}ico");

    // Image image = Image.FromFile($"{userProfilePath}/.config/OFME-Linux/icons/{gameid}ico");
    // using var image = SixLabors.ImageSharp.Image.Load($"{userProfilePath}/.config/OFME-Linux/icons/{gameid}ico");
  
    var icoPath = $"{userProfilePath}/.config/OFME-Linux/icons/{gameid}ico";
    var pngPath = $"{userProfilePath}/.config/OFME-Linux/icons/{gameid}";

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
    File.WriteAllText($"{userProfilePath}/{gameid}.json", get);


    dynamic json = JObject.Parse(get);
    string headerImage = json[gameid.ToString()]["data"]["header_image"];
    Console.WriteLine(headerImage);

    using (WebClient client = new WebClient())
    {
        client.DownloadFile(headerImage, $"{userProfilePath}/.config/OFME-Linux/banners/{gameid}header.png");
    }




    // image.SaveAsPng($"{userProfilePath}/.config/OFME-Linux/icons/{gameid}");




    File.Delete($"{userProfilePath}/.config/OFME-Linux/icons/{gameid}ico");


    Console.WriteLine($"Fix ini file: {fixinit}");
    // string gameid = File.ReadAllText(gamepath + "/steam_appid.txt");

    Directory.CreateDirectory($"{userProfilePath}/.local/share/OnlineFix Linux Launcher/prefixes/{gamename.Split('.')[0]}");
    string prefix = @$"[{gamename.Split('.')[0]}]
fakeSteamID=480
steamID={gameid}
icon={userProfilePath}/.config/OFME-Linux/icons/{gameid}
overrides=steam_api64=n;stubdrm64=n;winmm=n,b;onlinefix64=n;steamoverlay64=n
executable={gameexe}
mainPath={gamepath}
prefixPath={userProfilePath}/.local/share/OnlineFix Linux Launcher/prefixes/{gamename.Split('.')[0]}
proton=GE-Proton10-34
steamRuntime=1
steamOverlay=1
wined3d=
nativeWayland=
fixPath=
banner={userProfilePath}/.config/OFME-Linux/banners/{gameid}header.png"+"\n\n";

    File.AppendAllText(userProfilePath + "/.config/OFME-Linux/Games.ini", prefix);
}



void unrar(string archivePath, string outputDir)
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






void ExtractIcon(string exePath, string outputPngPath)
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
