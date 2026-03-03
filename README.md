# Tiny Compiler

A compiler project for the **Tiny Language**, developed as part of a Compilers Design course. This project includes a scanner (lexical analyzer) and a graphical user interface for interacting with the compiler.

## Features

- **Lexical Analysis (Scanner)**: Automatically tokens source code based on the Tiny language specification.
- **GUI Interface**: Built with Windows Forms for easy interaction.
- **Support for Tiny Language**: Handles numbers, strings, reserved keywords, comments, identifiers, and various operators.

## Project Structure

- `Scanner.cs`: Contains the lexical analyzer logic, converting source code into tokens.
- `Tiny_Compiler.cs`: The main compiler orchestrator.
- `Form1.cs`: The graphical user interface.
- `Program.cs`: Entry point for the application.
- `TIny.md`: Detailed specification of the Tiny language (Lexical and Grammar rules).

## Language Specification

The Tiny language supports:
- **Data Types**: `int`, `float`, `string`.
- **Control Flow**: `if`, `elseif`, `else`, `repeat-until`.
- **Operations**: Arithmetic (`+`, `-`, `*`, `/`), Conditional (`<`, `>`, `=`, `<>`), and Boolean (`&&`, `||`).
- **Functions**: Support for function declarations, parameters, and calls.

For more details on the language syntax, please refer to [TIny.md](./TIny.md).

## Getting Started

### Prerequisites
- .NET 10.0 SDK
- Windows OS (for Windows Forms support)

### How to Run

1. Clone or download the repository.
2. Open the project in **Rider** or **Visual Studio**.
3. Build the solution to restore dependencies.
4. Run the project (`Tiny_Compiler.exe`).
5. Enter your Tiny code in the input area and start the scanning process.

## Credits

Developed for: **Design of Compilers (Sem 4)**.
