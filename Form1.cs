namespace Tiny_Compiler
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ConfigureTokenGrid();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            errorTextBox.Clear();

            // Reset shared state before each scan so output does not accumulate.
            TinyCompiler.TokenStream.Clear();
            Errors.ErrorList.Clear();

            string code = textBox1.Text;
            TinyCompiler.StartCompiler(code);

            PrintTokens();
            PrintErrors();
        }

        private void ConfigureTokenGrid()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns.Add("Lexeme", "Lexeme");
            dataGridView1.Columns.Add("TokenClass", "Token Class");
        }

        private void PrintTokens()
        {
            foreach (var token in TinyCompiler.TokenStream)
            {
                dataGridView1.Rows.Add(token.Lex, token.TokenType);
            }
        }

        private void PrintErrors()
        {
            foreach (var error in Errors.ErrorList)
            {
                errorTextBox.AppendText(error + Environment.NewLine);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            errorTextBox.Clear();
            TinyCompiler.TokenStream.Clear();
            dataGridView1.Rows.Clear();
            Errors.ErrorList.Clear();
        }
    }
}
