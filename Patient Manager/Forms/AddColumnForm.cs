using System;
using System.Windows.Forms;

namespace Patient_Manager.Forms
{
    public partial class AddColumnForm : Form
    {
        public AddColumnForm()
        {
            InitializeComponent();
        }
        public String columnName { get; set; }
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                DialogResult = DialogResult.OK;
                this.columnName = textBox1.Text;
                this.Close();
            }
            else
            {
                MessageBox.Show("Por favor ingrese un nombre válido para la columna.", "Nombre inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
