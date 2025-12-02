# 🎮 Super Kiro World

A 2D platformer game created during the AWS Re:Invent workshop. Built with C# and MonoGame for cross-platform desktop gaming.

![Game Screenshot](SuperKiroWorld/Content/kiro-logo.png)

## 🌟 Features

- **Classic Platformer**: Side-scrolling action gameplay
- **Coin Collection System**: Gather coins throughout the stage to earn points
- **Lives System**: Challenge yourself with 3 lives
- **Smooth Camera Follow**: Fluid camera tracking that follows the player
- **High Score Tracking**: Save and beat your best scores
- **Cross-Platform**: Runs on Windows, Mac, and Linux

## 🎯 Objective

Control your character using arrow keys or WASD to jump across platforms, collect all coins, and reach the goal flag. Avoid falling off platforms while gathering every coin!

## 🕹️ Controls

- **Move**: ← → or A D
- **Jump**: ↑ or W or Space
- **Restart**: R key
- **Exit**: Esc key

## 🏆 Scoring

- Coin Collection: **100 points** per coin
- Level Clear Life Bonus: **500 points** per remaining life
- High scores are automatically saved

## 🛠️ Tech Stack

- **C# (.NET 8.0)** - Game logic and physics
- **MonoGame 3.8+** - Cross-platform game framework
- **DesktopGL** - OpenGL-based rendering

## 🚀 Setup and Running

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- MonoGame (included in project dependencies)

### Running the Game

```bash
# Navigate to project directory
cd SuperKiroWorld

# Restore dependencies
dotnet restore

# Run the game
dotnet run
```

### Release Build (Better Performance)

```bash
# Build in Release mode
dotnet build -c Release

# Run Release build
dotnet run -c Release
```

## 📁 Project Structure

```
SuperKiroWorld/
├── Content/              # Game assets
│   ├── Content.mgcb     # MonoGame Content Pipeline config
│   └── kiro-logo.png    # Player sprite
├── Game1.cs             # Main game logic
├── Program.cs           # Entry point
├── ScoreManager.cs      # Score management system
├── EffectManager.cs     # Visual effects (screen shake, slow-mo)
├── Particle.cs          # Particle effects
├── ParticleEmitter.cs   # Particle emission system
├── ParticleConfig.cs    # Particle configuration
└── SuperKiroWorld.csproj # Project configuration
```

## 🎨 Design

- **Color Scheme**: Kiro brand purple (#790ECB)
- **Dark Theme**: Background #1A1A1A
- **60 FPS**: Smooth gameplay experience
- **Responsive Input**: Immediate control feedback

## 🎓 About the AWS Re:Invent Workshop

This game was developed as part of the AWS Re:Invent workshop, demonstrating game development fundamentals using MonoGame and C#.

## 📝 License

This project was created for educational purposes as part of a workshop.

## 🙏 Acknowledgments

Thanks to the AWS Re:Invent workshop team and all participants!

---

**Enjoy playing!** 🎮✨
