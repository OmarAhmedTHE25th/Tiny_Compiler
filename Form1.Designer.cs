namespace Tiny_Compiler;

partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    private TextBox textBox1;
    private TextBox errorTextBox;
    private Label errorsLabel;
    private DataGridView dataGridView1;
    private Button button1;
    private Button button2;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        textBox1 = new TextBox();
        errorTextBox = new TextBox();
        errorsLabel = new Label();
        dataGridView1 = new DataGridView();
        button1 = new Button();
        button2 = new Button();
        ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
        SuspendLayout();
        // 
        // textBox1
        // 
        textBox1.Location = new Point(12, 12);
        textBox1.Multiline = true;
        textBox1.Name = "textBox1";
        textBox1.ScrollBars = ScrollBars.Vertical;
        textBox1.Size = new Size(420, 220);
        textBox1.TabIndex = 0;
        // 
        // errorTextBox
        // 
        errorTextBox.Location = new Point(12, 284);
        errorTextBox.Multiline = true;
        errorTextBox.Name = "errorTextBox";
        errorTextBox.ReadOnly = true;
        errorTextBox.ScrollBars = ScrollBars.Vertical;
        errorTextBox.Size = new Size(420, 154);
        errorTextBox.TabIndex = 2;
        // 
        // errorsLabel
        // 
        errorsLabel.AutoSize = true;
        errorsLabel.Location = new Point(12, 266);
        errorsLabel.Name = "errorsLabel";
        errorsLabel.Size = new Size(39, 15);
        errorsLabel.TabIndex = 1;
        errorsLabel.Text = "Errors";
        // 
        // dataGridView1
        // 
        dataGridView1.AllowUserToAddRows = false;
        dataGridView1.AllowUserToDeleteRows = false;
        dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dataGridView1.Location = new Point(448, 12);
        dataGridView1.Name = "dataGridView1";
        dataGridView1.ReadOnly = true;
        dataGridView1.RowHeadersVisible = false;
        dataGridView1.Size = new Size(340, 426);
        dataGridView1.TabIndex = 2;
        dataGridView1.CellContentClick += dataGridView1_CellContentClick;
        // 
        // button1
        // 
        button1.Location = new Point(12, 245);
        button1.Name = "button1";
        button1.Size = new Size(100, 30);
        button1.TabIndex = 3;
        button1.Text = "Scan";
        button1.UseVisualStyleBackColor = true;
        button1.Click += button1_Click;
        // 
        // button2
        // 
        button2.Location = new Point(120, 245);
        button2.Name = "button2";
        button2.Size = new Size(100, 30);
        button2.TabIndex = 4;
        button2.Text = "Clear";
        button2.UseVisualStyleBackColor = true;
        button2.Click += button2_Click;
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        Controls.Add(errorsLabel);
        Controls.Add(button2);
        Controls.Add(button1);
        Controls.Add(dataGridView1);
        Controls.Add(errorTextBox);
        Controls.Add(textBox1);
        Name = "Form1";
        Text = "Tiny Compiler";
        ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}