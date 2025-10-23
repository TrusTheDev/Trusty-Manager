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
        
        //Inicializo el form y establezco la fecha y el documento mas reciente (ultimo)
        public Form1()
        {
            InitializeComponent();
            label1.Text = GetDate();
            var document = navigator.currentFile();
            dataGridView = documentToGridView(document, dataGridView);
            
        }

        //Inicializo la posicion en y del boton addRowbtn
        private void Form1_Load(object sender, EventArgs e)
        {
            addRowbtn.Location = new Point(addRowbtn.Location.X, addRowbtn.Parent.PointToClient(dataGridView.PointToScreen(dataGridView.GetCellDisplayRectangle(dataGridView.CurrentCell.ColumnIndex, dataGridView.CurrentCell.RowIndex, false).Location)).Y);
        }

        //Sin implementar
        private void button4_Click(object sender, EventArgs e)
        {

        }
        //utiliza la clase navegador para iterar al anterior archivo de la lista si hay cambios en el archivo reciente se guarda sino solamente carga el anterior
        private void anteriorbtn_Click(object sender, EventArgs e)
        {
            navigator.currentFile().SaveFile();
            dataGridView = documentToGridView(navigator.getPreviousFile(), dataGridView);    
        }

        //utiliza la clase navegador para iterar al siguiente archivo de la lista si hay cambios en el archivo reciente se guarda sino solamente carga el siguiente
        private void siguientebtn_Click(object sender, EventArgs e)
        {
            navigator.currentFile().SaveFile();
            dataGridView = documentToGridView(navigator.getNextFile(), dataGridView);
            
        }
        //Establece la posicion en Y de Rowbtn cuando se selecciona una celda del componente gridview
        private void CurrentCell(object sender, EventArgs e)
        {
                addRowbtn.Top = getSelectedmiddlePoint(dataGridView, addRowbtn);
        }
        //inserta la una fila vacia abajo de la fila seleccionada/s
        private void addRowbtn_Click(object sender, EventArgs e)
        {
            dataGridView.Rows.Insert(dataGridView.CurrentCell.RowIndex + 1);
        }
        //establece de manera automatica la posicion en y del boton cuando se expande una fila
        private void onRowHeight(object sender, DataGridViewRowEventArgs e)
        {
            addRowbtn.Top = getSelectedmiddlePoint(dataGridView, addRowbtn);
        }
    }
}
