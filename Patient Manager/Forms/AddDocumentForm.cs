using Patient_Manager.Controllers;
using Patient_Manager.Models;
using System;
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

        private void BtnOk_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                fileController.CreateFile(textBox1.Text);
                this.FileModelList = fileController.DocumentModelList;
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Por favor ingrese un nombre válido para el archivo.", "Nombre inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
