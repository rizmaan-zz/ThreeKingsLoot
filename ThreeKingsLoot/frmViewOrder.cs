using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;

namespace ThreeKingsLoot
{
    public partial class frmViewOrder : Form
    {
        OleDbConnection oledbConn;
        public static string OrderID = "";

        public frmViewOrder()
        {
            InitializeComponent();
        }

        private void frmViewOrder_Load(object sender, EventArgs e)
        {
            ConnectToExcel();
            LoadDetails();
            dataGridDet.ClearSelection();
        }

        private void LoadDetails()
        {
            OleDbCommand cmd = new OleDbCommand();
            OleDbDataAdapter oleda = new OleDbDataAdapter();
            DataSet ds = new DataSet();
            cmd.Connection = oledbConn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT [OrderID],[OrderDate],[TotalPrice] FROM [Orders$]";
            oleda = new OleDbDataAdapter(cmd);
            oleda.Fill(ds);
            dataGridDet.DataSource = ds.Tables[0];
            oleda.Dispose();
        }


            private void ConnectToExcel()
        {
            try
            {
                string path = Path.GetFullPath("495 DB.xlsx");

                /* connection string  to work with excel file.
                 * HDR=Yes - indicates that the first row contains columnnames, not data. 
                 * "IMEX=1;" tells the driver to always read "intermixed" (numbers, dates, strings etc) data columns as text. 
                 * Note that this option might affect excel sheet write access negative. */

                if (Path.GetExtension(path) == ".xlsx")
                {
                    oledbConn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties = 'Excel 12.0;HDR=YES;'; ");
                }

                oledbConn.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void btnViewOrd_Click(object sender, EventArgs e)
        {

            MessageBox.Show(OrderID);
            frmOrderDetails frmOrdD = new frmOrderDetails();
            frmOrdD.ShowDialog();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridDet_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            DataGridViewRow selectedRow = dataGridDet.Rows[index];
            OrderID = selectedRow.Cells[0].Value.ToString();
        }
    }
}
