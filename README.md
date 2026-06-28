# online-fix-importer

A tool for automatically extracting and importing [online-fix.me](https://online-fix.me) games into **OnlineFix Linux Launcher**.

Right-click a `.rar` game archive or `.exe` in Dolphin and it will:
- Extract the game and fix files using 7zip
- Merge the online-fix patch into the game folder
- Download the Steam banner and extract the game icon
- Register the game in OnlineFix Linux Launcher with the correct Proton prefix config

---

## Requirements

- KDE Plasma with Dolphin file manager
- `7zip`
- `icoextract`
- `icoutils` (for `icotool`)
- `dotnet-runtime` (if not using self-contained build)
- [OnlineFix Linux Launcher](https://github.com/ZzEdovec/onlinefix-linux) — for launching the games after import

---

## Installation

### Option 1 — Manual with makepkg

```bash
git clone https://github.com/kqnori/online-fix.git
cd online-fix
makepkg -si
```

### Option 2 — Manual dependency install first

```bash
sudo pacman -S 7zip icoextract icoutils
git clone https://github.com/kqnori/online-fix.git
cd online-fix
makepkg -si
```

---

## Usage

### Via Dolphin right-click menu

After installing, right-click any file in Dolphin:

| File type | Action |
|---|---|
| `.rar` game archive | Extracts game + fix, imports to launcher |
| `.exe` game executable | Adds existing installed game to launcher |

Look for **"Add to onlinefix launcher"** in the context menu.

<img src="https://github.com/kqnori/online-fix/blob/main/assets/howtouse.gif" alt="how to use it" width="1000px"/>

### Via terminal

```bash
# Extract and import a game from a rar archive
online-fix-importer "Megabonk.v1.0.rar"

# Add an already-installed game by exe
online-fix-importer "Megabonk.exe"
```

---

## What it does

```
GameName.rar
    │
    ├── extracts game  →  ~/Documents/games/GameName/
    ├── extracts fix   →  merges into game folder
    ├── downloads      →  Steam banner image
    ├── extracts       →  game icon from .exe
    └── registers      →  ~/.config/OFME-Linux/Games.ini
```

Config and data is stored in:
```
~/.config/OFME-Linux/
├── Games.ini          ← game registry
├── icons/             ← extracted game icons
└── banners/           ← Steam banner images
```

---

## File structure

```
online-fix/
├── Program.cs               ← main app logic
├── onlinefix-out.csproj     ← .NET project file
├── online-fix.desktop       ← KDE service menu
├── oflogo.png               ← app icon
├── PKGBUILD                 ← Arch package build script
└── README.md
```

---

## Building from source

Requires `dotnet-sdk-10.0` or later.

```bash
dotnet publish onlinefix-out.csproj \
    --configuration Release \
    --runtime linux-x64 \
    --self-contained true \
    -p:PublishSingleFile=true \
    --output ./publish
```

Binary will be at `./publish/onlinefix-out`.

---

## License

MIT
