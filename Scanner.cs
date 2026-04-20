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
    //Symbols
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
    // Accumulated list of tokens produced from the source code.
    List<Token> Tokens = [];
    // Lookup table mapping keyword strings to their token classes.
    Dictionary<string, TokenClass> ReservedWords = [];
    // Lookup table mapping operator/symbol strings to their token classes.
    Dictionary<string, TokenClass> Operators = [];

    public Scanner()
    {
        // ── Reserved Words Initialization ─────────────────────────────────────
        // Register all language keywords so they are recognized before
        // falling through to the identifier check.
        ReservedWords.Add("int", TokenClass.T_Int);
        ReservedWords.Add("float", TokenClass.T_FloatType);
        ReservedWords.Add("string", TokenClass.T_StringType);
        ReservedWords.Add("if", TokenClass.T_If);
        ReservedWords.Add("elseif", TokenClass.T_Elseif);
        ReservedWords.Add("else", TokenClass.T_Else);
        ReservedWords.Add("then", TokenClass.T_Then);
        ReservedWords.Add("read", TokenClass.T_Read);
        ReservedWords.Add("write", TokenClass.T_Write);
        ReservedWords.Add("repeat", TokenClass.T_Repeat);
        ReservedWords.Add("until", TokenClass.T_Until);
        ReservedWords.Add("endl", TokenClass.T_Endl);
        ReservedWords.Add("return", TokenClass.T_Return);
        ReservedWords.Add("main", TokenClass.T_Main);

        // ── Operators & Symbols Initialization ────────────────────────────────
        // Register arithmetic, relational, logical, and punctuation symbols.
        Operators.Add("+", TokenClass.T_Plus);
        Operators.Add("-",TokenClass.T_Minus);
        Operators.Add("*",TokenClass.T_Multiply);
        Operators.Add("/",TokenClass.T_Divide);
        Operators.Add("<",TokenClass.T_LessThan);
        Operators.Add(">",TokenClass.T_GreaterThan);
        Operators.Add("<>",TokenClass.T_NotEqual);
        Operators.Add("=",TokenClass.T_Equal);
        Operators.Add("&&",TokenClass.T_And);
        Operators.Add("||",TokenClass.T_Or);
        Operators.Add("(", TokenClass.T_LParanthesis);
        Operators.Add(")", TokenClass.T_RParanthesis);
        Operators.Add("{", TokenClass.T_LCurlyBracket);
        Operators.Add("}", TokenClass.T_RCurlyBracket);
        Operators.Add(";", TokenClass.T_Semicolon);
        Operators.Add(",", TokenClass.T_Comma);
        Operators.Add(":=", TokenClass.T_Assign);
    }

    public void StartScanning(string sourceCode) //int x 
    {
        for (int i = 0; i < sourceCode.Length; i++)
        {
            Char currentChar = sourceCode[i];
            int j = i;

            // ── Skip Whitespace ───────────────────────────────────────────────
            if (Char.IsWhiteSpace(currentChar))continue;
            string currentLexeme = currentChar.ToString();

            // ── Identifiers and Keywords ──────────────────────────────────────
            // Consume a run of letters and digits, then classify the lexeme
            // as either a reserved keyword or a user-defined identifier.
            if (Char.IsLetter(currentChar))
            {
                j = i + 1;
                while (j < sourceCode.Length && char.IsLetterOrDigit(sourceCode[j])) 
                {
                        currentLexeme += sourceCode[j].ToString();
                        j++;
                    
                }
                FindTokenClass(currentLexeme); 
                i = j - 1;
            }

            // ── Numeric Literals ──────────────────────────────────────────────
            // Consume a run of digits (and at most one '.') to form an integer
            // or floating-point constant.
            else if (Char.IsDigit(currentChar))
            {
                j = i + 1;
                while (j < sourceCode.Length && (char.IsDigit(sourceCode[j]) || sourceCode[j] == '.'))
                {
                    currentLexeme += sourceCode[j].ToString();
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
                while (j + 1 < sourceCode.Length - 1 && !(sourceCode[j] == '*' && sourceCode[j + 1] == '/'))
                {
                    j++;
                }
                if (j + 1 >= sourceCode.Length - 1)
                    Errors.ErrorList.Add("Unterminated block comment");
                i = j+1;
            }

            // ── String Literals ───────────────────────────────────────────────
            // Consume all characters between opening and closing double-quotes
            // and emit the entire quoted text as a string token.
            else if (currentChar == '"')
            {
                j = i + 1;
                while (j < sourceCode.Length && sourceCode[j] != '"')
                    j++;
                if (j >= sourceCode.Length)Errors.ErrorList.Add("Unterminated String literal");
                else
                {
                    currentLexeme = sourceCode.Substring(i, j - i + 1);
                    FindTokenClass(currentLexeme);
                    i = j;
                }
            }

            // ── Assignment Operator ':=' ──────────────────────────────────────
            else if (currentChar == ':' && i+1 < sourceCode.Length && sourceCode[i + 1] == '=')
            {
                currentLexeme = ":=";
                FindTokenClass(currentLexeme);
                i++;
            }

            // ── Logical OR Operator '||' ──────────────────────────────────────
            else if (currentChar == '|' && i+1 < sourceCode.Length &&sourceCode[i + 1] == '|')
            {
                currentLexeme = "||";
                FindTokenClass(currentLexeme);
                i++;
            }

            // ── Logical AND Operator '&&' ─────────────────────────────────────
            else if (currentChar == '&'  && i+1 < sourceCode.Length&& sourceCode[i + 1] == '&')
            {
                currentLexeme = "&&";
                FindTokenClass(currentLexeme);
                i++;
            }

            // ── Not-Equal Operator '<>' ───────────────────────────────────────
            else if (currentChar == '<' && sourceCode[i + 1] == '>')
            {
                currentLexeme = "<>";
                FindTokenClass(currentLexeme);
                i++;
            }

            // ── Single-Character Operators / Symbols ──────────────────────────
            // Anything that didn't match the cases above is treated as a
            // single-character operator or symbol (e.g. '+', '-', ';', etc.).
            else
            {
                FindTokenClass(currentLexeme);
            }
            
        }
        TinyCompiler.TokenStream = Tokens;
    }

    // ── Token Classification ──────────────────────────────────────────────────
    // Given a lexeme string, determines its token class and appends the
    // resulting token to the token list, or records an error if unrecognized.
    private void FindTokenClass(string currentLexeme)
    {
        TokenClass tokenClass;
        var token = new Token();
        token.Lex = currentLexeme;
        // Is it a reserved word?
        if (ReservedWords.TryGetValue(currentLexeme, out tokenClass))
        {
            token.TokenType = tokenClass;
            Tokens.Add(token);
        }
        // Is it an identifier?
        else if (IsIdentifier(currentLexeme))
        {
            tokenClass = TokenClass.T_Identifier;
            token.TokenType = tokenClass;
            Tokens.Add(token);
        }
        // Is it an operator?
        else if (Operators.TryGetValue(currentLexeme, out tokenClass))
        {
            token.TokenType = tokenClass;
            Tokens.Add(token);
        }
        
        // Is it a constant?
        else if (IsConstant(currentLexeme))
        {
            if (currentLexeme.StartsWith('"') && currentLexeme.EndsWith('"'))
            {
                tokenClass = TokenClass.T_String;
                token.TokenType = tokenClass;
                Tokens.Add(token);
                return;
            }
            if (currentLexeme.Contains('.'))
            {
                tokenClass = TokenClass.T_Float;
                token.TokenType = tokenClass;
                Tokens.Add(token);
                return;
            }
            tokenClass = TokenClass.T_Integer;
            token.TokenType = tokenClass;
            Tokens.Add(token);
        }
       
        else
        {
            Errors.ErrorList.Add("Unidentified Token "+ currentLexeme );
        }
        
    }

    // ── Constant Detection ────────────────────────────────────────────────────
    // Returns true if the lexeme is a numeric literal (integer or float)
    // or a string literal enclosed in double quotes.
    private static bool IsConstant(string currentLexeme)
    {
      bool isConstant = false;
      if (IsConstantString(currentLexeme)) return true;
      if (int.TryParse(currentLexeme, out _) || double.TryParse(currentLexeme, out _))
      {
          isConstant = true;
      }
      return isConstant;
    }

    // ── String Constant Detection ─────────────────────────────────────────────
    // Returns true when the lexeme starts and ends with a double-quote,
    // indicating it is a string literal.
    private static bool IsConstantString(string currentLexeme)
    {
        return (currentLexeme[0] == '"' && currentLexeme[^1] == '"');
    }

    // ── Identifier Detection ──────────────────────────────────────────────────
    // Returns true when the lexeme starts with a letter and is not a
    // reserved keyword, making it a valid user-defined identifier.
    private bool IsIdentifier(string currentLexeme)
    {
        var isIdentifier = false;
        if (Char.IsLetter(currentLexeme[0]) && !ReservedWords.ContainsKey(currentLexeme))
        {
            isIdentifier = true;
        }
        return isIdentifier;
    }
}

// ─── Error Collection ─────────────────────────────────────────────────────────
// Accumulates error messages encountered during scanning and later
// compiler phases so they can be reported to the user.
public static class Errors
{
    public static List<string> ErrorList = new();
}