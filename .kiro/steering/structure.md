# Project Structure

## Root Files
- `SuperKiroWorld.csproj` - Project configuration and dependencies
- `Program.cs` - Application entry point
- `Game1.cs` - Main game class with Update/Draw loop
- `js-version/` - Original JavaScript implementation

## MonoGame Project Structure
```
SuperKiroWorld/
├── Content/
│   ├── Content.mgcb          # Content Pipeline configuration
│   ├── kiro-logo.png         # Player sprite
│   └── Font.spritefont       # UI font (optional)
├── Game1.cs                  # Main game logic
├── Program.cs                # Entry point
└── SuperKiroWorld.csproj     # Project file
```

## Code Organization in Game1.cs

### Game State Classes/Structs
- `GameState` - Global game state (score, lives, coins, camera position)
- `Player` - Player properties (position, velocity, physics constants)
- `Platform` - Platform struct with position, size, and color
- `Coin` - Collectible item with position and collection state
- `Goal` - Level completion trigger

### Core Methods (MonoGame Lifecycle)
- `Initialize()` - Game initialization
- `LoadContent()` - Load sprites, fonts, and assets
- `Update(GameTime)` - Game logic, physics, input (60 FPS)
- `Draw(GameTime)` - Rendering with SpriteBatch

### Game Logic Methods
- `UpdatePlayer()` - Player movement, physics, and collision detection
- `CheckCollision()` - AABB collision detection helper
- `UpdateCamera()` - Smooth camera following with interpolation
- `DrawUI()` - Render score, lives, coins display
- `LoseLife()` - Handle player death
- `ResetPlayerPosition()` - Respawn player at start
- `GameOver()` - End game state
- `LevelComplete()` - Victory state with bonus scoring
- `RestartGame()` - Reset all game state

## Conventions
- Japanese comments throughout the code (日本語)
- Kiro brand colors (Color(121, 14, 203) purple) for platforms and UI
- Dark theme (Color(26, 26, 26) background)
- PascalCase for classes, methods, and properties
- camelCase for local variables and fields
- Class-based entity system with structs for simple data
