using Patient_Manager.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xceed.Document.NET;
using Xceed.Words.NET;
using static Patient_Manager.Controllers.DateController;
using static Patient_Manager.Controllers.GridViewController;



namespace Patient_Manager
{
    public partial class Form1 : Form
    {
        String PatientDocPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\PatientDocs\";
        public Form1()
        {
            InitializeComponent();
            label1.Text = GetDate();
            DocumentModelList documentList = new DocumentModelList(PatientDocPath);
            var document = documentList[1] ;
            dataGridView = documentToGridView(document, dataGridView);
            
                
         
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
