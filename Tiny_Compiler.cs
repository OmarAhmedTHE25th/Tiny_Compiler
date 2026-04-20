namespace Tiny_Compiler;

public class TinyCompiler
{
    static Scanner TinyScanner = new ();
    public static List<Token> TokenStream = [];

    public static void StartCompiler(string sourceCode)
    {
        TinyScanner.StartScanning(sourceCode);
    }
    

}
