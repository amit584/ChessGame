# ChessGame

A player-vs-computer chess game built in **Unity** with **C#**, featuring an AI opponent.

> Originally built as a high school project — one of my first dives into game development and AI.

## About

A classic chess experience where you play against a computer opponent. The game implements standard chess rules and a decision-making AI that evaluates board positions to choose its moves.

## Tech Stack

- **Engine:** Unity
- **Language:** C#
- **Platform:** Desktop (Windows/Mac/Linux via Unity build targets)

## Features

- Full chess rules implementation (movement, captures, check, checkmate)
- Single-player mode against an AI opponent
- Interactive board with click-to-move piece interaction
- Visual feedback for legal moves and game state

## How the AI Works

The computer opponent evaluates possible moves and selects one based on a scoring function that considers piece values and board position.
Uses the Minimax algorithm to look ahead several moves, evaluating each possible board position with a scoring function based on piece values and positional factors, then choosing the move that maximizes its advantage while assuming the opponent plays optimally.

## Getting Started

### Prerequisites

- [Unity](https://unity.com/download) (the version this project was built with — check `ProjectSettings/ProjectVersion.txt`)

### Running the Project

1. Clone the repository:
   ```bash
   git clone https://github.com/amit584/ChessGame.git
   ```
2. Open Unity Hub and click **Add** → select the cloned `ChessGame` folder.
3. Open the project in Unity.
4. Open the main scene from the `Assets` folder.
5. Press the **Play** button to start the game.

## Project Structure

```
ChessGame/
├── Assets/           # Game assets, scripts, scenes, sprites
├── Packages/         # Unity package dependencies
├── ProjectSettings/  # Unity project configuration
└── README.md
```

## Controls

- **Left Click** — Select a piece, then click a highlighted square to move.

## What I Learned

This was my first serious project combining game logic, UI, and AI decision-making. Key takeaways:

- Translating real-world rules (chess) into code structures
- Working within Unity's component-based architecture
- Implementing decision-making logic for an AI opponent
- Managing game state and turn-based flow
- 
