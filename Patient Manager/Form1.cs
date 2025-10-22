using DocumentFormat.OpenXml.Wordprocessing;
using Patient_Manager.Controllers;
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

using System.Threading;


namespace Patient_Manager
{
    public partial class Form1 : Form
    {
        static String PatientDocPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\PatientDocs\";
        static DocumentModelList documentList = new DocumentModelList(PatientDocPath);
        static NavigatorController navigator = new NavigatorController(documentList);
        
        public Form1()
        {
            InitializeComponent();
            label1.Text = GetDate();
            var document = navigator.currentFile();
            dataGridView = documentToGridView(document, dataGridView);
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            addRowbtn.Location = new Point(addRowbtn.Location.X, addRowbtn.Parent.PointToClient(dataGridView.PointToScreen(dataGridView.GetCellDisplayRectangle(dataGridView.CurrentCell.ColumnIndex, dataGridView.CurrentCell.RowIndex, false).Location)).Y);
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
        
        private void anteriorbtn_Click(object sender, EventArgs e)
        {
            dataGridView = documentToGridView(navigator.getPreviousFile(), dataGridView);
        }

        private void siguientebtn_Click(object sender, EventArgs e)
        {
            dataGridView = documentToGridView(navigator.getNextFile(), dataGridView);
        }

        private void CurrentCell(object sender, EventArgs e)
        {
            if (dataGridView.CurrentCell != null)
            {
                addRowbtn.Location = new Point(addRowbtn.Location.X, addRowbtn.Parent.PointToClient(dataGridView.PointToScreen(dataGridView.GetCellDisplayRectangle(dataGridView.CurrentCell.ColumnIndex, dataGridView.CurrentCell.RowIndex, false).Location)).Y);
            }
        }

        private void addRowbtn_Click(object sender, EventArgs e)
        {
            dataGridView.Rows.Insert(dataGridView.CurrentCell.RowIndex + 1);
        }
    }
}
