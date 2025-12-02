# Product Overview

Super Kiro World is a 2D platformer game built for the AWS Re:Invent workshop. The game features a character (using the Kiro logo as a sprite) that navigates through platforms, collects coins, and reaches a goal flag.

Originally built with JavaScript/HTML5 Canvas, now reimplemented in C# with MonoGame for cross-platform desktop deployment.

## Core Features
- Side-scrolling platformer with smooth camera following
- Coin collection system with scoring
- Lives system with respawn mechanics
- Level completion with final score calculation
- Japanese language UI (日本語)
- Cross-platform support (Windows, Mac, Linux)

## Gameplay
Players control a character using arrow keys or WASD to move and jump across platforms. The objective is to collect all coins and reach the goal flag while avoiding falling off platforms. Players have 3 lives, and bonus points are awarded based on remaining lives at level completion.

## Technical Implementation
- Built with MonoGame (DesktopGL)
- 60 FPS fixed timestep
- Physics-based movement with gravity and friction
- Smooth camera interpolation
