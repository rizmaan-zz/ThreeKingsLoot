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
    public partial class frmPlaceOrder : Form
    {
        OleDbConnection oledbConn;
      
        public static string CurrentOrderID = "";
        

        public frmPlaceOrder()
        {
            InitializeComponent();
        }

        private void frmPlaceOrder_Load(object sender, EventArgs e)
        {
            ConnectToExcel();
            GetSupplierTypes();
            getOrderID();
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
                    oledbConn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties = 'Excel 12.0;ReadOnly=False;HDR=YES;'; ");
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

        private void GetSupplierTypes()
        {

            OleDbCommand cmd = new OleDbCommand();
            OleDbDataAdapter oleda = new OleDbDataAdapter();
            DataSet ds = new DataSet();
            cmd.Connection = oledbConn;
            cmd.CommandType = CommandType.Text;

            // Passing values to the combo boxes
            // selecting the Supplier Names from Sheet = Suppliers HDR = Supllier

            cmd.CommandText = "SELECT [ID],[Supplier] FROM [Suppliers$] WHERE Supplier IS NOT NULL";
            oleda = new OleDbDataAdapter(cmd);
            oleda.Fill(ds, "Supplier");
            oleda.Dispose();
            oleda.Dispose();

            cmbSup.DataSource = ds.Tables[0].DefaultView;
            cmbSup.ValueMember = "ID";
            cmbSup.DisplayMember = "Supplier";

            //By default nothing will be selected
            cmbSup.SelectedIndex = -1;
        }

        public void getOrderID()
        {
            try
            {
                //get the last order ID
                OleDbCommand cmd = new OleDbCommand();
                OleDbDataAdapter oleda = new OleDbDataAdapter();
                DataSet ds = new DataSet();
                cmd.Connection = oledbConn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT TOP 1 [OrderID] FROM [Order Details$] ORDER BY [OrderID] DESC";
                oleda = new OleDbDataAdapter(cmd);
                oleda.Fill(ds);
                oleda.Dispose();
                string OrderID = ds.Tables[0].Rows[0]["OrderID"].ToString();
                int orID = Convert.ToInt32(OrderID) + 1;

                //strring up order ID with the 0 padding
                CurrentOrderID = orID.ToString().PadLeft(6, '0');
            }
            catch (Exception OrderIDs)
            {
                MessageBox.Show(OrderIDs.ToString());
            }
        }

        private void btnAddCard_Click(object sender, EventArgs e)
        {
            //Check if all data is entered
            string CardID = txtCardID.Text;
            string Qty = txtQty.Text;
            string CardSupplier= this.cmbSup.GetItemText(this.cmbSup.SelectedItem);
            string selected = this.cmbSup.GetItemText(this.cmbSup.SelectedValue);

            string[] strs = new string[] { CardID, Qty, CardSupplier };
            if (strs.All(str => string.IsNullOrEmpty(str)))
            {
                MessageBox.Show("Please fill all the values!");
            }
            else
            {

                //Add to the OrderDetails


                //setup order details
                try
                {
                    OleDbCommand cmd = new OleDbCommand();
                    OleDbDataAdapter oleda = new OleDbDataAdapter();
                    DataSet ds = new DataSet();
                    cmd.Connection = oledbConn;
                    cmd.CommandType = CommandType.Text;

                    //get the details for the Quatity 
                    cmd.CommandText = "SELECT [Name],[Price],[Quantity] FROM [" + selected + "$] WHERE [ID]='" + CardID + "'";
                    oleda = new OleDbDataAdapter(cmd);
                    oleda.Fill(ds);
                    oleda.Dispose();
                    string CardName = ds.Tables[0].Rows[0]["Name"].ToString();
                    string Price = ds.Tables[0].Rows[0]["Price"].ToString();
                    string DatabaseQty = ds.Tables[0].Rows[0]["Quantity"].ToString();
                    int DQty = Convert.ToInt32(DatabaseQty) + Convert.ToInt32(Qty);
                    //    

                    cmd = new OleDbCommand();
                    ds = new DataSet();
                    cmd.Connection = oledbConn;
                    //cmd.CommandText = "INSERT INTO [Order Details$] VALUES('" + CurrentOrderID + "','" + CardName + "','" + Qty + "','" + CardSupplier + "','" + Price + "','" + selected + "')";
                    cmd.CommandText = "INSERT INTO [Order Details$] VALUES (@CurrentOrdID,@CardName,@Qty,@CardSupplier,@Price,@SupID)";
                    cmd.Parameters.AddWithValue("@CurrentOrderID", CurrentOrderID);
                    cmd.Parameters.AddWithValue("@CardName", CardName);
                    cmd.Parameters.AddWithValue("@Qty", Qty);
                    cmd.Parameters.AddWithValue("@CardSupplier", CardSupplier);
                    cmd.Parameters.AddWithValue("@Price", Price);
                    cmd.Parameters.AddWithValue("@SupID", selected);
                    //"UPDATE [" + selected + "$] SET [Quantity] = '" + DatabaseQty + "' WHERE [ID]='" + CardID + "'";
                    cmd.ExecuteNonQuery();
                    //Update the database with quantities
                    cmd = new OleDbCommand();
                    ds = new DataSet();
                    cmd.Connection = oledbConn;
                    cmd.CommandText = "UPDATE [" + selected + "$] SET [Quantity] = '" + DQty.ToString() + "' WHERE [ID]='" + CardID + "'";
                    cmd.ExecuteNonQuery();

                    MessageBox.Show(CardID + ": Added to the Order List");

                    txtCardID.Text = "";
                    txtQty.Text = "";
                    cmbSup.SelectedIndex = -1;
                }
                catch (Exception OrdIns)
                {
                    MessageBox.Show(OrdIns.ToString());
                }
            }

        }

        private void btnOrdDet_Click(object sender, EventArgs e)
        {
            frmOrderDetails frmOrdD = new frmOrderDetails();
            frmOrdD.ShowDialog();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
