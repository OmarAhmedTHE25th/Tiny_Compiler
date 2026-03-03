namespace Tiny_Compiler;

public class Tiny_Compiler
{
    static Scanner JasonScanner = new ();
    public static List<Token> TokenStream = [];

    public static void StartCompiler(string sourceCode)
    {
        JasonScanner.StartScanning(sourceCode);
    }

}