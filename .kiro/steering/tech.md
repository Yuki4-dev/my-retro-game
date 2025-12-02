# Tech Stack

## Core Technologies
- **C# (.NET 8.0)** - Game logic and physics
- **MonoGame 3.8+** - Cross-platform game framework
- **DesktopGL** - OpenGL-based rendering for Windows/Mac/Linux

## Architecture
- Desktop application with .NET build system
- MonoGame Content Pipeline for asset management
- Object-oriented design with game components

## Key Libraries
- **MonoGame.Framework.DesktopGL** - Core game framework
- **.NET 8.0 SDK** - Runtime and build tools

## Running the Game

### Build and Run
```bash
# Restore dependencies
dotnet restore

# Build the project
dotnet build

# Run the game
dotnet run
```

### Development
```bash
# Build in Release mode for better performance
dotnet build -c Release

# Run Release build
dotnet run -c Release
```

## Project Structure
- `SuperKiroWorld.csproj` - Project file with dependencies
- `Game1.cs` - Main game class (initialization, update, draw)
- `Content/` - Game assets (sprites, fonts, etc.)
- `Content/Content.mgcb` - MonoGame Content Pipeline configuration
- `Program.cs` - Entry point

## Performance Target
- 60 FPS using MonoGame's fixed timestep
- Smooth camera interpolation for scrolling
- Immediate input response with Keyboard state polling
