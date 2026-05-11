using System.Globalization;

namespace Tiny_Compiler;

// ─── Token Classification Enum ───────────────────────────────────────────────
// Defines all valid token categories that the scanner can produce.
public enum TokenClass
{
    // Literals
    T_String,
    T_Float,
    T_Integer,
    T_Identifier,

    // Keywords
    T_Int,
    T_FloatType,
    T_StringType,
    T_Read,
    T_Write,
    T_Repeat,
    T_Until,
    T_If,
    T_Elseif,
    T_Else,
    T_Then,
    T_Return,
    T_Endl,
    T_Main,
    T_End,

    // Arithmetic
    T_Plus,
    T_Minus,
    T_Multiply,
    T_Divide,

    // Condition
    T_LessThan,
    T_GreaterThan,
    T_Equal,
    T_NotEqual,

    // Boolean
    T_And,
    T_Or,

    // Symbols
    T_LParanthesis,
    T_RParanthesis,
    T_LCurlyBracket,
    T_RCurlyBracket,
    T_Semicolon,
    T_Comma,
    T_Assign,
}

// ─── Token Data Structure ─────────────────────────────────────────────────────
// Represents a single token produced by the scanner,
// holding the lexeme (raw text) and its classified type.
public class Token
{
    public string Lex;
    public TokenClass TokenType;
}

// ─── Scanner ──────────────────────────────────────────────────────────────────
// Reads source code character-by-character, groups characters into lexemes,
// classifies each lexeme as a token, and builds the token stream.
public class Scanner
{
    // ── Scanner State ─────────────────────────────────────────────────────────
    List<Token> Tokens = [];
    Dictionary<string, TokenClass> ReservedWords = [];
    Dictionary<string, TokenClass> Operators = [];

    public Scanner()
    {
        // ── Reserved Words Initialization ─────────────────────────────────────
        ReservedWords.Add("int", TokenClass.T_Int);
        ReservedWords.Add("float", TokenClass.T_FloatType);
        ReservedWords.Add("string", TokenClass.T_StringType);
        ReservedWords.Add("if", TokenClass.T_If);
        ReservedWords.Add("elseif", TokenClass.T_Elseif);
        ReservedWords.Add("else", TokenClass.T_Else);
        ReservedWords.Add("then", TokenClass.T_Then);
        ReservedWords.Add("end", TokenClass.T_End);      // FIX 1: registers "end" as reserved
        ReservedWords.Add("read", TokenClass.T_Read);
        ReservedWords.Add("write", TokenClass.T_Write);
        ReservedWords.Add("repeat", TokenClass.T_Repeat);
        ReservedWords.Add("until", TokenClass.T_Until);
        ReservedWords.Add("endl", TokenClass.T_Endl);
        ReservedWords.Add("return", TokenClass.T_Return);
        ReservedWords.Add("main", TokenClass.T_Main);

        // ── Operators & Symbols Initialization ────────────────────────────────
        Operators.Add("+", TokenClass.T_Plus);
        Operators.Add("-", TokenClass.T_Minus);
        Operators.Add("*", TokenClass.T_Multiply);
        Operators.Add("/", TokenClass.T_Divide);
        Operators.Add("<", TokenClass.T_LessThan);
        Operators.Add(">", TokenClass.T_GreaterThan);
        Operators.Add("<>", TokenClass.T_NotEqual);
        Operators.Add("=", TokenClass.T_Equal);
        Operators.Add("&&", TokenClass.T_And);
        Operators.Add("||", TokenClass.T_Or);
        Operators.Add("(", TokenClass.T_LParanthesis);
        Operators.Add(")", TokenClass.T_RParanthesis);
        Operators.Add("{", TokenClass.T_LCurlyBracket);
        Operators.Add("}", TokenClass.T_RCurlyBracket);
        Operators.Add(";", TokenClass.T_Semicolon);
        Operators.Add(",", TokenClass.T_Comma);
        Operators.Add(":=", TokenClass.T_Assign);
    }

    public void StartScanning(string sourceCode)
    {
        for (int i = 0; i < sourceCode.Length; i++)
        {
            char currentChar = sourceCode[i];
            int j = i;

            // ── Skip Whitespace ───────────────────────────────────────────────
            if (char.IsWhiteSpace(currentChar)) continue;

            string currentLexeme = currentChar.ToString();

            // ── Identifiers and Keywords ──────────────────────────────────────
            if (char.IsLetter(currentChar))
            {
                j = i + 1;
                while (j < sourceCode.Length && char.IsLetterOrDigit(sourceCode[j]))
                {
                    currentLexeme += sourceCode[j];
                    j++;
                }
                FindTokenClass(currentLexeme);
                i = j - 1;
            }

            // ── Numeric Literals ──────────────────────────────────────────────
            else if (char.IsDigit(currentChar))
            {
                j = i + 1;
                while (j < sourceCode.Length && (char.IsDigit(sourceCode[j]) || sourceCode[j] == '.'))
                {
                    currentLexeme += sourceCode[j];
                    j++;
                }
                FindTokenClass(currentLexeme);
                i = j - 1;
            }

            // ── Block Comments /* … */ ────────────────────────────────────────
            // Skip every character between the opening '/*' and closing '*/'
            // without producing any token.
            else if (currentChar == '/' &&  j + 1 < sourceCode.Length && sourceCode[j + 1] == '*' )
            {
                j += 2; // step past the opening /*
                while (j + 1 < sourceCode.Length && !(sourceCode[j] == '*' && sourceCode[j + 1] == '/'))
                    j++;

                if (j + 1 >= sourceCode.Length)
                    Errors.ErrorList.Add("Unterminated block comment");
                else
                    i = j + 1; // step past the closing */
            }

            // ── String Literals ───────────────────────────────────────────────
            else if (currentChar == '"')
            {
                j = i + 1;
                while (j < sourceCode.Length && sourceCode[j] != '"')
                    j++;

                if (j >= sourceCode.Length)
                {
                    Errors.ErrorList.Add("Unterminated string literal");
                    i = sourceCode.Length - 1;
                }
                else
                {
                    currentLexeme = sourceCode.Substring(i, j - i + 1);
                    FindTokenClass(currentLexeme);
                    i = j;
                }
            }

            // ── Assignment Operator ':=' ──────────────────────────────────────
            else if (currentChar == ':' && i + 1 < sourceCode.Length && sourceCode[i + 1] == '=')
            {
                FindTokenClass(":=");
                i++;
            }

            // ── Logical OR Operator '||' ──────────────────────────────────────
            else if (currentChar == '|' && i + 1 < sourceCode.Length && sourceCode[i + 1] == '|')
            {
                FindTokenClass("||");
                i++;
            }

            // ── Logical AND Operator '&&' ─────────────────────────────────────
            else if (currentChar == '&' && i + 1 < sourceCode.Length && sourceCode[i + 1] == '&')
            {
                FindTokenClass("&&");
                i++;
            }

            // ── Not-Equal Operator '<>' ───────────────────────────────────────
            else if (currentChar == '<' && sourceCode[i + 1] == '>')
            {
                FindTokenClass("<>");
                i++;
            }

            // ── Single-Character Operators / Symbols ──────────────────────────
            else
            {
                FindTokenClass(currentLexeme);
            }
        }

        TinyCompiler.TokenStream = Tokens;
    }

    // ── Token Classification ──────────────────────────────────────────────────
    private void FindTokenClass(string currentLexeme)
    {
        TokenClass tokenClass;
        var token = new Token { Lex = currentLexeme };

        if (ReservedWords.TryGetValue(currentLexeme, out tokenClass))
        {
            token.TokenType = tokenClass;
            Tokens.Add(token);
        }
        else if (IsIdentifier(currentLexeme))
        {
            token.TokenType = TokenClass.T_Identifier;
            Tokens.Add(token);
        }
        else if (Operators.TryGetValue(currentLexeme, out tokenClass))
        {
            token.TokenType = tokenClass;
            Tokens.Add(token);
        }
        else if (IsConstant(currentLexeme))
        {
            if (currentLexeme.StartsWith('"') && currentLexeme.EndsWith('"'))
                token.TokenType = TokenClass.T_String;
            else if (currentLexeme.Contains('.'))
                token.TokenType = TokenClass.T_Float;
            else
                token.TokenType = TokenClass.T_Integer;

            Tokens.Add(token);
        }
        else
        {
            Errors.ErrorList.Add("Unrecognised token: " + currentLexeme);
        }
    }

    // ── Constant Detection ────────────────────────────────────────────────────
    // Returns true if the lexeme is a numeric literal (integer or float)
    // or a string literal enclosed in double quotes.
    private static bool IsConstant(string currentLexeme)
    {
        if (IsConstantString(currentLexeme)) return true;
        if (int.TryParse(currentLexeme, out _)) return true;
        if (double.TryParse(currentLexeme, NumberStyles.Float, CultureInfo.InvariantCulture, out _)) return true;
        return false;
    }

    // ── String Constant Detection ─────────────────────────────────────────────
    private static bool IsConstantString(string currentLexeme)
    {
        return currentLexeme.Length >= 2 && currentLexeme[0] == '"' && currentLexeme[^1] == '"';
    }

    // ── Identifier Detection ──────────────────────────────────────────────────
    private bool IsIdentifier(string currentLexeme)
    {
        return char.IsLetter(currentLexeme[0]) && !ReservedWords.ContainsKey(currentLexeme);
    }
}

// ─── Error Collection ─────────────────────────────────────────────────────────
public static class Errors
{
    public static List<string> ErrorList = new();
}