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
    public partial class frmOrderDetails : Form
    {
        OleDbConnection oledbConn;
        public string total = "";
        public string OrderValue = "";

        public frmOrderDetails()
        {
            InitializeComponent();
        }

        private void frmOrderDetails_Load(object sender, EventArgs e)
        {
            ConnectToExcel();
            if (frmPlaceOrder.CurrentOrderID == "")
            {
                OrderValue = frmViewOrder.OrderID;
                btnSaveOrd.Visible = false;
            }
            else
            {
                OrderValue = frmPlaceOrder.CurrentOrderID;
            }

            LoadDetails();
            


        }

        private void LoadDetails()
        {
            OleDbCommand cmd = new OleDbCommand();
            OleDbDataAdapter oleda = new OleDbDataAdapter();
            DataSet ds = new DataSet();
            cmd.Connection = oledbConn;
            cmd.CommandType = CommandType.Text;

            // Passing values to the combo boxes
            // selecting the Supplier Names from Sheet = Suppliers HDR = Supllier

            cmd.CommandText = "SELECT [CardName],[Quantity],[Supplier],[Price] FROM [Order Details$] WHERE OrderID='" + OrderValue + "'";
            oleda = new OleDbDataAdapter(cmd);
            oleda.Fill(ds);
            dataGridDet.DataSource = ds.Tables[0];
            oleda.Dispose();

            ds = new DataSet();
            cmd.Connection = oledbConn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT  SUM([Price]) AS [TotalAmount] FROM [Order Details$] WHERE [OrderID] = '" + OrderValue + "'";
            oleda = new OleDbDataAdapter(cmd);
           oleda.Fill(ds);
            oleda.Dispose();
            total ="$"+  ds.Tables[0].Rows[0]["TotalAmount"].ToString();
            lblOrderID.Text = OrderValue;
            lblTotal.Text = total;
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

        private void CloseExcelConnection()
        {
            oledbConn.Close();
        }

        private void btnSaveOrd_Click(object sender, EventArgs e)
        {
            //save order details
            OleDbCommand cmd = new OleDbCommand();
            OleDbDataAdapter oleda = new OleDbDataAdapter();
            DataSet ds = new DataSet();
            cmd = new OleDbCommand();
            ds = new DataSet();
            cmd.Connection = oledbConn;
            cmd.CommandText = "INSERT INTO [Orders$] VALUES('" + OrderValue + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + total +"')";
            cmd.ExecuteNonQuery();
            MessageBox.Show("Order " + OrderValue + " Done");
            this.Close();

        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
