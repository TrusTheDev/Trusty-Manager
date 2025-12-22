using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Patient_Manager.Forms
{
    
    public partial class AddColumn : Form
    {
        String nameColumns { get; set; }
        public AddColumn()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                DialogResult = DialogResult.OK;
                this.nameColumns = textBox1.Text;
                Close();
            }
            else
            {
                MessageBox.Show("Por favor ingrese un nombre válido para la columna.", "Nombre inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
