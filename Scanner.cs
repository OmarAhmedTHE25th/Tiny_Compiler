namespace Tiny_Compiler;

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
    T_Dot,
    T_LCurlyBracket,
    T_RCurlyBracket,
    T_Semicolon,
    T_Comma,
    T_Assign,
   
}
public class Token
{
    public string Lex;
    public TokenClass TokenType;
}
public class Scanner
{
    List<Token> Tokens = [];
    Dictionary<string, TokenClass> ReservedWords = [];
    Dictionary<string, TokenClass> Operators = [];

    public Scanner()
    {
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
        Operators.Add(".", TokenClass.T_Dot);
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
            Char currentChar = sourceCode[i];
            int j = i;
            if (Char.IsWhiteSpace(currentChar))continue;
            string currentLexeme = currentChar.ToString();
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
            else if (currentChar == '/' && sourceCode[j + 1] == '*' )
            {
                while (!(sourceCode[j] == '*' && sourceCode[j+1] == '/'))
                {
                    j++;
                }
                i = j+1;
            }
            else if (currentChar == '"')
            {
                j = i + 1;
                while (j < sourceCode.Length && sourceCode[j] != '"')
                    j++;
                currentLexeme = sourceCode.Substring(i, j - i + 1);
                FindTokenClass(currentLexeme);
                i = j;
            }
            else if (currentChar == ':' && sourceCode[i + 1] == '=')
            {
                currentLexeme = ":=";
                FindTokenClass(currentLexeme);
                i++;
            }
            else if (currentChar == '|' && sourceCode[i + 1] == '|')
            {
                currentLexeme = "||";
                FindTokenClass(currentLexeme);
                i++;
            }
            else if (currentChar == '&' && sourceCode[i + 1] == '&')
            {
                currentLexeme = "&&";
                FindTokenClass(currentLexeme);
                i++;
            }
            else if (currentChar == '<' && sourceCode[i + 1] == '>')
            {
                currentLexeme = "<>";
                FindTokenClass(currentLexeme);
                i++;
            }
            else
            {
                FindTokenClass(currentLexeme);
            }
            
        }
        Tiny_Compiler.TokenStream = Tokens;
    }

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

    private static bool IsConstantString(string currentLexeme)
    {
        return (currentLexeme[0] == '"' && currentLexeme[^1] == '"');
    }

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

public static class Errors
{
    public static List<string> ErrorList = new();
}