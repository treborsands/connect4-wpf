# Connect 4 (WPF)

A two-player Connect 4 game for Windows, built with WPF and .NET 8.

## Features

- Classic 6×7 Connect 4 board
- Two-player local gameplay (Red vs Yellow)
- Win detection for horizontal, vertical, and diagonal lines
- Winning discs highlighted in white
- Draw detection when the board is full
- Click a column header (▼) to drop a disc

## Requirements

- Windows 10 or later
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (for building from source)
- [.NET 8 Desktop Runtime](https://dotnet.microsoft.com/download/dotnet/8.0) (for running published builds)

## Build and Run

```bash
dotnet build Connect4.sln
dotnet run --project Connect4
```

Or open `Connect4.sln` in Visual Studio 2022 and press F5.

## Publish (single-file executable)

```bash
dotnet publish Connect4/Connect4.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

The executable will be in `Connect4/bin/Release/net8.0-windows/win-x64/publish/`.

## How to Play

1. Red always goes first.
2. Click the ▼ above a column to drop your disc.
3. Discs fall to the lowest open slot in that column.
4. Connect four discs in a row — horizontally, vertically, or diagonally — to win.
5. Click **New Game** to start over.

## Project Structure

```
Connect4/
├── Models/GameBoard.cs      # Game logic and win detection
├── ViewModels/MainViewModel.cs  # UI state and commands
├── Converters/              # WPF value converters
├── MainWindow.xaml          # Main game UI
└── App.xaml                 # Application entry point
```
