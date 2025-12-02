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
    public partial class SaveOnExit : Form
    {
        public SaveOnExit()
        {
            InitializeComponent();
        }

        private void BtnSi_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
        }

        private void BtnNo_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

    }
}
