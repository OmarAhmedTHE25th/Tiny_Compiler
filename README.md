# Tiny Compiler

A compiler project for the **Tiny Language**, developed as part of a Compilers Design course. This project includes a scanner (lexical analyzer) and a parser (symentic analyzer) with a graphical user interface for visual representation.

## Features

- **Lexical Analysis (Scanner)**: Tokenizes Tiny source code according to the language specification.
- **Syntax Analysis (Parser)**: Parses the token stream using a recursive-descent parser and builds a parse tree.
- **Tabbed Output UI**: Shows compiler output in separate tabs for **Tokens** and **Parse Tree**.
- **Error Reporting**: Displays lexical/parsing errors in a dedicated error panel.
- **Reset-Safe Runs**: Clears tokens, errors, and parse-tree state before each run to avoid stale output.
- **Tiny Language Coverage**: Supports data types, functions, assignments, I/O, conditionals, loops, and expressions.

## Project Structure

- `Scanner.cs`: Contains the lexical analyzer logic, converting source code into tokens.
- `Parser.cs`: Contains the recursive-descent parser and parse-tree node model (`Node`).
- `Tiny_Compiler.cs`: The main compiler orchestrator.
- `Form1.cs`: Main UI logic (scan/parse flow, token display, parse-tree rendering).
- `Form1.Designer.cs`: Windows Forms layout, including tabbed output with `DataGridView` and `TreeView`.
- `Program.cs`: Entry point for the application.
- `TIny.md`: Detailed Tiny language reference (lexical rules, CFG, and sample programs).

## Language Specification

The Tiny language supports:
- **Data Types**: `int`, `float`, `string`.
- **Control Flow**: `if`, `elseif`, `else`, `repeat-until`.
- **Operations**: Arithmetic (`+`, `-`, `*`, `/`), Conditional (`<`, `>`, `=`, `<>`), and Boolean (`&&`, `||`).
- **Functions**: Support for function declarations, parameters, and calls.

For more details on the language syntax, please refer to [TIny.md](./TIny.md).

## Getting Started

### Prerequisites
- .NET 9.0 SDK
- Windows OS (for Windows Forms support)

### How to Run

1. Clone or download the repository.
2. Open the project in **Rider** or **Visual Studio**.
3. Build the solution to restore dependencies.
4. Run the project (`Tiny_Compiler.exe`).
5. Enter Tiny code in the input area and click **Scan/Run**.
6. Check the **Tokens** tab for lexical output.
7. If there are no lexical errors, check the **Parse Tree** tab for syntax-tree visualization.
8. Review the **Errors** panel for diagnostics.

## Credits

Developed for: **Design of Compilers (Sem 4)**.
