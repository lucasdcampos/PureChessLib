# PureChessLib

A C# library for chess logic, providing a reusable set of classes and methods to handle chess game mechanics.

<img src="https://github-production-user-asset-6210df.s3.amazonaws.com/74553272/261683900-3c815c5b-723c-455f-8f3c-8d7b9955322f.png?X-Amz-Algorithm=AWS4-HMAC-SHA256&X-Amz-Credential=AKIAIWNJYAX4CSVEH53A%2F20230818%2Fus-east-1%2Fs3%2Faws4_request&X-Amz-Date=20230818T182218Z&X-Amz-Expires=300&X-Amz-Signature=335888f6d4dd048f62279752c802d4b0aae0bd6aa8efb46b83cd9f773f0a59c1&X-Amz-SignedHeaders=host&actor_id=74553272&key_id=0&repo_id=678973306" height="240px">

## Introduction

PureChessLib is a C# library that encapsulates the logic and mechanics of a chess game. It's designed to be a reusable and flexible solution for developers who want to integrate chess functionality into their applications, games, or projects.

This library provides classes and methods to manage the chessboard, player turns, legal moves, and other core chess-related features. By using PureChessLib, you can focus on building user interfaces, AI opponents, or other game features without having to reimplement the fundamental rules of chess.

## Features

- Chessboard representation and manipulation.
- Move validation and legality checks.
- Player turn management.
- Support for standard chess rules, including castling, en passant, and pawn promotion.
- Easily extensible for custom chess variants or additional rules.

## Usage

To use PureChessLib in your C# project, follow these steps:

1. Download or clone this repository to your local machine.
2. Reference the `PureChessLib.dll` in your project.
3. Import the necessary namespaces and start using the library's classes and methods.

```csharp
using PureChess;

// Example usage
string startPosition = "RNBQKBNR/PPPPPPPP/......../......../......../......../pppppppp/rnbqkbnr"; //Fen isn't supported yet!
Game.StartGame()

// ... Continue with your code

```
For more detailed information on how to use specific features, refer to the documentation.

## Contributing

Contributions to PureChessLib are welcome! If you find any issues, have suggestions for improvements, or want to add new features, please open an issue or create a pull request. Make sure to read the contribution guidelines before getting started.

## License
This project is licensed under the MIT License. Check LICENSE.MD for more details.