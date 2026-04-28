namespace Tiny_Compiler;

public class Node
{
    public List<Node> Children = new List<Node>();
    public string Name;

    public Node(string name)
    {
        Name = name;
    }
}

public class Parser
{
    private int inputPointer = 0;
    private List<Token> tokenStream;
    public Node Root;

    public Parser()
    {
    }

    public void Parse(List<Token> tokens)
    {
        tokenStream = tokens;
        inputPointer = 0;
        Root = Program();
        if (inputPointer < tokenStream.Count)
        {
            Errors.ErrorList.Add($"Unexpected token at index {inputPointer}: {tokenStream[inputPointer].Lex}");
        }
    }

    private Node Program()
    {
        Node node = new Node("Program");
        node.Children.Add(Program_Function_Statement());
        node.Children.Add(Main_Function());
        return node;
    }

    private Node Program_Function_Statement()
    {
        Node node = new Node("Program_Function_Statement");
        if (inputPointer < tokenStream.Count && 
            (tokenStream[inputPointer].TokenType == TokenClass.T_Int || 
             tokenStream[inputPointer].TokenType == TokenClass.T_FloatType || 
             tokenStream[inputPointer].TokenType == TokenClass.T_StringType))
        {
            if (inputPointer + 1 < tokenStream.Count && tokenStream[inputPointer + 1].TokenType != TokenClass.T_Main)
            {
                node.Children.Add(Function_Statement());
                node.Children.Add(Program_Function_Statement());
                return node;
            }
        }
        return node; // Epsilon
    }

    private Node Main_Function()
    {
        Node node = new Node("Main_Function");
        node.Children.Add(Data_Type());
        node.Children.Add(Match(TokenClass.T_Main));
        node.Children.Add(Match(TokenClass.T_LParanthesis));
        node.Children.Add(Match(TokenClass.T_RParanthesis));
        node.Children.Add(Function_Body());
        return node;
    }

    private Node Function_Statement()
    {
        Node node = new Node("Function_Statement");
        node.Children.Add(Function_Declaration());
        node.Children.Add(Function_Body());
        return node;
    }

    private Node Function_Declaration()
    {
        Node node = new Node("Function_Declaration");
        node.Children.Add(Data_Type());
        node.Children.Add(Function_Name());
        node.Children.Add(Match(TokenClass.T_LParanthesis));
        node.Children.Add(Function_Parameters());
        node.Children.Add(Match(TokenClass.T_RParanthesis));
        return node;
    }

    private Node Data_Type()
    {
        Node node = new Node("Data_Type");
        if (inputPointer < tokenStream.Count)
        {
            if (tokenStream[inputPointer].TokenType == TokenClass.T_Int ||
                tokenStream[inputPointer].TokenType == TokenClass.T_FloatType ||
                tokenStream[inputPointer].TokenType == TokenClass.T_StringType)
            {
                node.Children.Add(Match(tokenStream[inputPointer].TokenType));
                return node;
            }
        }
        return node;
    }

    private Node Function_Name()
    {
        Node node = new Node("Function_Name");
        node.Children.Add(Match(TokenClass.T_Identifier));
        return node;
    }

    private Node Function_Parameters()
    {
        Node node = new Node("Function_Parameters");
        if (inputPointer < tokenStream.Count && 
            (tokenStream[inputPointer].TokenType == TokenClass.T_Int || 
             tokenStream[inputPointer].TokenType == TokenClass.T_FloatType || 
             tokenStream[inputPointer].TokenType == TokenClass.T_StringType))
        {
            node.Children.Add(Data_Type());
            node.Children.Add(Match(TokenClass.T_Identifier));
            node.Children.Add(More_Parameters());
        }
        return node;
    }

    private Node More_Parameters()
    {
        Node node = new Node("More_Parameters");
        if (inputPointer < tokenStream.Count && tokenStream[inputPointer].TokenType == TokenClass.T_Comma)
        {
            node.Children.Add(Match(TokenClass.T_Comma));
            node.Children.Add(Data_Type());
            node.Children.Add(Match(TokenClass.T_Identifier));
            node.Children.Add(More_Parameters());
        }
        return node;
    }

    private Node Function_Body()
    {
        Node node = new Node("Function_Body");
        node.Children.Add(Match(TokenClass.T_LCurlyBracket));
        node.Children.Add(Statements());
        node.Children.Add(Return_Statement());
        node.Children.Add(Match(TokenClass.T_RCurlyBracket));
        return node;
    }

    private Node Statements()
    {
        Node node = new Node("Statements");
        if (inputPointer < tokenStream.Count)
        {
            var type = tokenStream[inputPointer].TokenType;
            if (type == TokenClass.T_Identifier || 
                type == TokenClass.T_Int || type == TokenClass.T_FloatType || type == TokenClass.T_StringType ||
                type == TokenClass.T_Write || type == TokenClass.T_Read ||
                type == TokenClass.T_If || type == TokenClass.T_Repeat)
            {
                node.Children.Add(Statement());
                node.Children.Add(Statements());
            }
        }
        return node;
    }

    private Node Statement()
    {
        Node node = new Node("Statement");
        if (inputPointer < tokenStream.Count)
        {
            var type = tokenStream[inputPointer].TokenType;
            if (type == TokenClass.T_Int || type == TokenClass.T_FloatType || type == TokenClass.T_StringType)
            {
                node.Children.Add(Declaration_Statement());
            }
            else if (type == TokenClass.T_Write)
            {
                node.Children.Add(Write_Statement());
            }
            else if (type == TokenClass.T_Read)
            {
                node.Children.Add(Read_Statement());
            }
            else if (type == TokenClass.T_If)
            {
                node.Children.Add(If_Statement());
            }
            else if (type == TokenClass.T_Repeat)
            {
                node.Children.Add(Repeat_Statement());
            }
            else if (type == TokenClass.T_Identifier)
            {
                if (inputPointer + 1 < tokenStream.Count && tokenStream[inputPointer + 1].TokenType == TokenClass.T_LParanthesis)
                {
                    node.Children.Add(Function_Call());
                    node.Children.Add(Match(TokenClass.T_Semicolon));
                }
                else
                {
                    node.Children.Add(Assignment_Statement());
                }
            }
        }
        return node;
    }

    private Node Declaration_Statement()
    {
        Node node = new Node("Declaration_Statement");
        node.Children.Add(Data_Type());
        node.Children.Add(Match(TokenClass.T_Identifier));
        node.Children.Add(Declare_Rest1());
        node.Children.Add(Declare_Rest2());
        node.Children.Add(Match(TokenClass.T_Semicolon));
        return node;
    }

    private Node Declare_Rest1()
    {
        Node node = new Node("Declare_Rest1");
        if (inputPointer < tokenStream.Count && tokenStream[inputPointer].TokenType == TokenClass.T_Comma)
        {
            node.Children.Add(Match(TokenClass.T_Comma));
            node.Children.Add(Match(TokenClass.T_Identifier));
            node.Children.Add(Declare_Rest1());
        }
        return node;
    }

    private Node Declare_Rest2()
    {
        Node node = new Node("Declare_Rest2");
        if (inputPointer < tokenStream.Count && tokenStream[inputPointer].TokenType == TokenClass.T_Assign)
        {
            node.Children.Add(Match(TokenClass.T_Assign));
            node.Children.Add(Expression());
        }
        return node;
    }

    private Node Write_Statement()
    {
        Node node = new Node("Write_Statement");
        node.Children.Add(Match(TokenClass.T_Write));
        if (inputPointer < tokenStream.Count && tokenStream[inputPointer].TokenType == TokenClass.T_Endl)
            node.Children.Add(Match(TokenClass.T_Endl));
        else
            node.Children.Add(Expression());
        node.Children.Add(Match(TokenClass.T_Semicolon));
        return node;
    }

    private Node Read_Statement()
    {
        Node node = new Node("Read_Statement");
        node.Children.Add(Match(TokenClass.T_Read));
        node.Children.Add(Match(TokenClass.T_Identifier));
        node.Children.Add(Match(TokenClass.T_Semicolon));
        return node;
    }

    private Node If_Statement()
    {
        Node node = new Node("If_Statement");
        node.Children.Add(Match(TokenClass.T_If));
        node.Children.Add(Condition()); // Condition_Statement simplified
        node.Children.Add(Match(TokenClass.T_Then));
        node.Children.Add(Statements());
        node.Children.Add(Other_Conditions());
        return node;
    }

    private Node Other_Conditions()
    {
        Node node = new Node("Other_Conditions");
        if (inputPointer < tokenStream.Count)
        {
            if (tokenStream[inputPointer].TokenType == TokenClass.T_Elseif)
            {
                node.Children.Add(Match(TokenClass.T_Elseif));
                node.Children.Add(Condition());
                node.Children.Add(Match(TokenClass.T_Then));
                node.Children.Add(Statements());
                node.Children.Add(Other_Conditions());
            }
            else if (tokenStream[inputPointer].TokenType == TokenClass.T_Else)
            {
                node.Children.Add(Match(TokenClass.T_Else));
                node.Children.Add(Statements());
                node.Children.Add(Match(TokenClass.T_Identifier)); // "end" is missing from tokens, assuming it's identifier or I skip. We should match T_End or identifier if not provided. Wait, Tiny doesn't have T_End? Ah, keywords might parse as T_Identifier for 'end'.
                // Using T_Identifier for 'end' fallback
            }
            else // 'end'
            {
                // Just match 'end' if it's an identifier or so.
                if (tokenStream[inputPointer].Lex == "end")
                    node.Children.Add(Match(TokenClass.T_Identifier));
            }
        }
        return node;
    }

    private Node Repeat_Statement()
    {
        Node node = new Node("Repeat_Statement");
        node.Children.Add(Match(TokenClass.T_Repeat));
        node.Children.Add(Statements());
        node.Children.Add(Match(TokenClass.T_Until));
        node.Children.Add(Condition());
        return node;
    }

    private Node Assignment_Statement()
    {
        Node node = new Node("Assignment_Statement");
        node.Children.Add(Match(TokenClass.T_Identifier));
        node.Children.Add(Match(TokenClass.T_Assign));
        node.Children.Add(Expression());
        node.Children.Add(Match(TokenClass.T_Semicolon));
        return node;
    }

    private Node Condition()
    {
        Node node = new Node("Condition");
        node.Children.Add(Match(TokenClass.T_Identifier));
        node.Children.Add(Condition_Operator());
        node.Children.Add(Term());
        node.Children.Add(More_Conditions());
        return node;
    }

    private Node Condition_Operator()
    {
        Node node = new Node("Condition_Operator");
        if (inputPointer < tokenStream.Count)
        {
            var type = tokenStream[inputPointer].TokenType;
            if (type == TokenClass.T_LessThan || type == TokenClass.T_GreaterThan || type == TokenClass.T_Equal || type == TokenClass.T_NotEqual)
                node.Children.Add(Match(type));
        }
        return node;
    }

    private Node More_Conditions()
    {
        Node node = new Node("More_Conditions");
        if (inputPointer < tokenStream.Count)
        {
            var type = tokenStream[inputPointer].TokenType;
            if (type == TokenClass.T_And || type == TokenClass.T_Or)
            {
                node.Children.Add(Match(type));
                node.Children.Add(Condition());
            }
        }
        return node;
    }

    private Node Expression()
    {
        Node node = new Node("Expression");
        if (inputPointer < tokenStream.Count)
        {
            var type = tokenStream[inputPointer].TokenType;
            if (type == TokenClass.T_String)
            {
                node.Children.Add(Match(TokenClass.T_String));
            }
            else
            {
                node.Children.Add(Equation());
            }
        }
        return node;
    }

    private Node Term()
    {
        Node node = new Node("Term");
        if (inputPointer < tokenStream.Count)
        {
            var type = tokenStream[inputPointer].TokenType;
            if (type == TokenClass.T_Integer || type == TokenClass.T_Float)
            {
                node.Children.Add(Match(type));
            }
            else if (type == TokenClass.T_Identifier)
            {
                if (inputPointer + 1 < tokenStream.Count && tokenStream[inputPointer + 1].TokenType == TokenClass.T_LParanthesis)
                {
                    node.Children.Add(Function_Call());
                }
                else
                {
                    node.Children.Add(Match(TokenClass.T_Identifier));
                }
            }
        }
        return node;
    }

    private Node Equation()
    {
        Node node = new Node("Equation");
        if (inputPointer < tokenStream.Count && tokenStream[inputPointer].TokenType == TokenClass.T_LParanthesis)
        {
            node.Children.Add(Match(TokenClass.T_LParanthesis));
            node.Children.Add(Equation());
            node.Children.Add(Match(TokenClass.T_RParanthesis));
            node.Children.Add(Operator_Equation());
        }
        else
        {
            node.Children.Add(Term());
            node.Children.Add(Operator_Equation());
        }
        return node;
    }

    private Node Operator_Equation()
    {
        Node node = new Node("Operator_Equation");
        if (inputPointer < tokenStream.Count)
        {
            var type = tokenStream[inputPointer].TokenType;
            if (type == TokenClass.T_Plus || type == TokenClass.T_Minus || type == TokenClass.T_Multiply || type == TokenClass.T_Divide)
            {
                node.Children.Add(Match(type));
                node.Children.Add(Equation());
                node.Children.Add(Operator_Equation());
            }
        }
        return node;
    }

    private Node Function_Call()
    {
        Node node = new Node("Function_Call");
        node.Children.Add(Match(TokenClass.T_Identifier));
        node.Children.Add(Match(TokenClass.T_LParanthesis));
        node.Children.Add(Call_Parameters());
        node.Children.Add(Match(TokenClass.T_RParanthesis));
        return node;
    }

    private Node Call_Parameters()
    {
        Node node = new Node("Call_Parameters");
        if (inputPointer < tokenStream.Count && tokenStream[inputPointer].TokenType != TokenClass.T_RParanthesis)
        {
            node.Children.Add(Expression());
            node.Children.Add(More_Call_Parameters());
        }
        return node;
    }

    private Node More_Call_Parameters()
    {
        Node node = new Node("More_Call_Parameters");
        if (inputPointer < tokenStream.Count && tokenStream[inputPointer].TokenType == TokenClass.T_Comma)
        {
            node.Children.Add(Match(TokenClass.T_Comma));
            node.Children.Add(Expression());
            node.Children.Add(More_Call_Parameters());
        }
        return node;
    }

    private Node Return_Statement()
    {
        Node node = new Node("Return_Statement");
        node.Children.Add(Match(TokenClass.T_Return));
        node.Children.Add(Expression());
        node.Children.Add(Match(TokenClass.T_Semicolon));
        return node;
    }

    private Node Match(TokenClass expectedToken)
    {
        if (inputPointer < tokenStream.Count && tokenStream[inputPointer].TokenType == expectedToken)
        {
            Node node = new Node(tokenStream[inputPointer].Lex);
            inputPointer++;
            return node;
        }
        else
        {
            return new Node(""); // Error handling could be added here
        }
    }
}
