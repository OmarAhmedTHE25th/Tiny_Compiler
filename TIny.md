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


## TINY Language Context Free Grammar CFG

- Program → Program_Function_Statement Main_Function

- Main_Function → Data_Type main ( ) Function_Body

- Program_Function_Statement → Function_Statement Program_Function_Statement \| ɛ

  - Function_Statement → Function_Declaration Function_Body

  - Function_Declaration → Data_Type Function_Name ( Function_Parameters )

  - Datatype → int \| float \| string

  - Function_Name → identifier

  - Function_Parameters → Data_Type Identifier More_Parameters \| ɛ

  - More_Function_Parameters → , Data_Type Identifier More_Function_Parameters \| ɛ

  - Function_Body → { Statements Return_Statement }

- Statements → State Statements \| ɛ

  - Statement → Function_Call \| Assignment_Statement \| Declaration_Statement \| Write_Statement \| Read_Statement \| If_Statement \| Repeat_Statement

- Function_Call → Identifier ( Parameters) ;

  - Parameters → Expression More_Parameters \| ɛ
  - More_Parameters → , Expression More_Parameters \| ɛ

- Assignment_Statement → Identifier := Expression

- Expression → String \| Term \| Equation

- Term → number \| identifier \| Function_Call

- Equation → Term Operator_Equation \| (Equation) Operator_Equation 

- Operator_Equation → Arthematic_Operator Equation Operator_Equation \| ε

	- Arthematic_Operator → plus \| minus \| divide \| multiply 

- Declaration_Statement → Data_Type Identifier Declare_Rest1 Declare_Rest2 ;

  - Declare_Rest1 → , identifier Declare_Rest1 \| ɛ

  - Declare_Rest2 → Assignment_Statement \| ɛ

- Write_Statement → write Write_Rest ;

  - Write_Rest → Expression \| endl

- Read_Statement → read identifier ;

- If_Statement → if Condition_Statement then Statements Other_Conditions

  - Condition_statement → Condition

  - Condition → identifier Condition_Operator Term More_Conditions

  - Condition_Operator → less_than \| greater_than \| not_equal \| equal

  - More_Conditions → and Condition \| or Condition\| ɛ

  - Other_Conditions → Else_if_Statement \| Else_statement \| end


- Else_if_Statement → elseif Condition_statement then Statements Other_Conditions

- Else_statement → else Statements end

- Repeat_Statement → repeat Statements untill Condition_statement

- Return_Statement → return Expression ;

______________________________________________

## TINY Code Samples

### Sample program includes all 30 rules

```cpp
int sum(int a, int b)
{
	return a + b;
}
int main()
{
	int val, counter;
	read val;

	counter := 0;

	repeat                                                                                
		val := val - 1;
		write "Iteration number [";
		write counter;
		write "] the value of x = ";
		write val;
		write endl;                          
		counter := counter+1;                                                      
	until val = 1

	write endl;

	string s := "number of Iterations = ";
	write s; 

	counter := counter-1;

	write counter;

	/* complicated equation */    
	float z1 := 3*2*(2+1)/2-5.3;
	z1 := z1 + sum(a,y);

	if  z1 > 5 || z1 < counter && z1 = 1 
	then 
		write z1;
	elseif z1 < 5 
	then
		z1 := 5;
	else
	    z1 := counter;
	end

	return 0;
}

```

### Sample program in Tiny language – computes factorial

```cpp
/* Sample program in Tiny language – computes factorial*/
int main()
{
	int x;
	read x; /*input an integer*/
	if x > 0 /*don’t compute if x <= 0 */
	then 
		int fact := 1;

		repeat
			fact := fact * x;
			x := x – 1;
		until x = 0

		write fact; /*output factorial of x*/
	end
	return 0;
}
```