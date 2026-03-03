# Tiny Language Specification

This document outlines the lexical and grammatical rules for the Tiny language.

---

## 1. Lexical Rules

| Rule                    | Description                                                                                        | Examples                                                                                                       |
|:------------------------|:---------------------------------------------------------------------------------------------------|:---------------------------------------------------------------------------------------------------------------|
| **Number**              | Any sequence of digits, including floating-point numbers.                                          | `123`, `554`, `205`, `0.23`                                                                                    |
| **String**              | Starts with double quotes `"`, followed by any characters/digits, and ends with double quotes `"`. | `"Hello"`, `"2nd + 3rd"`                                                                                       |
| **Reserved Keywords**   | Core language keywords.                                                                            | `int`, `float`, `string`, `read`, `write`, `repeat`, `until`, `if`, `elseif`, `else`, `then`, `return`, `endl` |
| **Comment Statement**   | Starts with `/*` and ends with `*/`.                                                               | `/* this is a comment */`                                                                                      |
| **Identifiers**         | Starts with a letter, followed by letters or digits.                                               | `x`, `val`, `counter1`, `str1`, `s2`                                                                           |
| **Arithmetic Operator** | Standard mathematical operators.                                                                   | `+`, `-`, `*`, `/`                                                                                             |
| **Condition Operator**  | Comparison operators.                                                                              | `<`, `>`, `=`, `<>`                                                                                            |
| **Boolean Operator**    | Logical operators.                                                                                 | `&&` (AND), `\|\|` (OR)                                                                                        |
| **Datatype**            | Keywords specifying data types.                                                                    | `int`, `float`, `string`                                                                                       |

---

## 2. Grammar Rules

### Expressions & Equations
- **Function Call**: `Identifier` + `(` + zero or more `Identifier` (separated by `,`) + `)`.
  - *Example*: `sum(a,b)`, `factorial(c)`, `rand()`
- **Term**: A `Number`, `Identifier`, or `Function Call`.
  - *Example*: `441`, `var1`, `sum(a,b)`
- **Equation**: Starts with `Term` or `(` + one or more `Arithmetic_Operator` and `Term` + matched `)`.
  - *Example*: `3+5`, `x + 1`, `(2+3)*10`
- **Expression**: A `String`, `Term`, or `Equation`.
  - *Example*: `"hi"`, `counter`, `404`, `2+3`

### Statements
- **Assignment**: `Identifier` + `:=` + `Expression`.
  - *Example*: `x := 1`, `y := 2+3`, `z := 2+3*2+(2-3)/1`
- **Declaration**: `Datatype` + one or more `Identifier` (with optional assignment) separated by `,` + `;`.
  - *Example*: `int x;`, `float x1, x2:=1, xy:=3;`
- **Write**: `write` + (`Expression` or `endl`) + `;`.
  - *Example*: `write x;`, `write 5;`, `write "Hello World";`
- **Read**: `read` + `Identifier` + `;`.
  - *Example*: `read x;`
- **Return**: `return` + `Expression` + `;`.
  - *Example*: `return a+b;`, `return 5;`

### Control Flow
- **Condition**: `Identifier` + `Condition_Operator` + `Term`.
  - *Example*: `z1 <> 10`
- **Condition Statement**: `Condition` followed by zero or more `Boolean_Operator` and `Condition`.
  - *Example*: `x < 5 && x > 1`
- **If Statement**: `if` + `Condition_Statement` + `then` + `Statements` + (`Else_If_Statement` | `Else_Statement` | `end`).
- **Else-If Statement**: `elseif` + `Condition_Statement` + `then` + `Statements` ...
- **Else Statement**: `else` + `Statements` + `end`.
- **Repeat Statement**: `repeat` + `Statements` + `until` + `Condition_Statement`.

### Functions & Program Structure
- **FunctionName**: Same as `Identifier`.
- **Parameter**: `Datatype` + `Identifier`.
  - *Example*: `int x`
- **Function Declaration**: `Datatype` + `FunctionName` + `(` + zero or more `Parameter` + `)`.
  - *Example*: `int sum(int a, int b)`
- **Function Body**: `{` + `Statements` + `Return_Statement` + `}`.
- **Function Statement**: `Function_Declaration` + `Function_Body`.
- **Main Function**: `Datatype` + `main` + `()` + `Function_Body`.
- **Program**: Zero or more `Function_Statement` followed by `Main_Function`.