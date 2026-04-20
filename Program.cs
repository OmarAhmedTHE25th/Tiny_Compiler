namespace Tiny_Compiler;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {

        string source = """
                        /* Sample program includes all 30 rules */
                        
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
                                counter := counter + 1;
                            until val = 1
                        
                            write endl;
                            string s := "number of Iterations = ";
                            write s;
                        
                            counter := counter - 1;
                            write counter;
                        
                            /* complicated equation */
                            float z1 := 3*2*(2+1)/2 - 5.3;
                            z1 := z1 + sum(1, y);
                        
                            if z1 > 5 || z1 < counter && z1 = 1 then
                                write z1;
                            else if z1 < 5 then
                                z1 := 5;
                            else
                                z1 := counter;
                            end
                        
                            return 0;
                        }  
                        
                        """;

         var scanner = new Scanner();
         scanner.StartScanning(source);

         foreach (var token in TinyCompiler.TokenStream)
         {
             Console.WriteLine($"{token.Lex,-20} {token.TokenType}");
         }

         if (Errors.ErrorList.Count > 0)
         {
             Console.WriteLine("\n--- Errors ---");
             foreach (var err in Errors.ErrorList)
                 Console.WriteLine(err);
         }
    //     // To customize application configuration such as set high DPI settings or default font,
    //     // see https://aka.ms/applicationconfiguration.
    //     ApplicationConfiguration.Initialize();
    //     Application.Run(new Form1());
        
    }
}