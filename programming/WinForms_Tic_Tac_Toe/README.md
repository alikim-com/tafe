## Tic-tac-toe game based on Windows Forms framework.

This project features a small game engine for playing turn-based board games. It supports multiple players, including humans and AI, competing one-on-one for ownership of the board tiles, aiming to achieve a certain pattern on the board.

After the initial setup, each player makes a move in sequential order until the game resolves either in a win or a tie.

### Technical overview:

The code is separated into two parts: a game engine that controls players' moves and the state of the game, and the UI part that visualizes the game state.

Most of the communication between these two parts is done via the built-in event system and with the help of the translation unit that maps the game-related states into the UI ones.

The engine mostly consists of `Game` and `TurnWheel` classes, where `AI` controls AI players, and the translation is handled by the `VBridge` class. The main app class is `Form`, and most of the UI states are processed in `CellWrapper` and `LabelManager`.

### Features:
- Multiple forms interaction
- Preserving the aspect ratio of the main app window
- AI players can have different levels of difficulty
- Saving and loading games via JsonSerializer
- Dynamic menu content
---
- Scalable fonts
- Image utilities for creating dynamic backgrounds, applying transparency & blending
- Double-buffered, scalable UI components with support for percentage-based positioning
- A menu with fully customizable look and feel
- AI-generated images

## How to play
> Download [the stand-alone win-x64 version](https://raw.githubusercontent.com/alikim-com/tafe/main/programming/WinForms_Tic_Tac_Toe/WinForms_Tic_Tac_Toe_standalone.zip)

> Download files from [the repository](https://github.com/alikim-com/tafe/tree/main/programming/WinForms_Tic_Tac_Toe) and compile your own executable.

### How to save a game: 
Enter a desired game name into the "Game name" field and then use the "Save as..." menu option to save the game. The name should be unique, i.e., not present on the "Saved games" menu list.<br/>Saving and loading games is disabled during the countdown.
