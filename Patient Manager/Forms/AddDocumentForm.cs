using Patient_Manager.Controllers;
using Patient_Manager.Interfaces;
using Patient_Manager.Models;
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
    public partial class AddDocumentForm : Form

    {
        FileController fileController = new FileController();
        public DocumentModelList FileModelList { get; set; }
        public AddDocumentForm()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                fileController.createFile(textBox1.Text);
                this.FileModelList = fileController.documentModelList;
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Por favor ingrese un nombre válido para el archivo.", "Nombre inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
