using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.OleDb;




namespace ThreeKingsLoot
{
   
    public partial class frmCardFinder : Form
    {
        OleDbConnection oledbConn;
        public static string DisplayName = "";
        public static string DisplayType = "";
        public static string DisplaySet = "";
        public static string DisplayQty = "";
        public static string DisplayPrice = "";
        public static string ImagePath = "";


        public frmCardFinder()
        {
            InitializeComponent();
        }

        private void frmCardFinder_Load(object sender, EventArgs e)
        {
            ConnectToExcel();
            GetSupplierTypes();

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

            cmbType.DataSource = ds.Tables[0].DefaultView;
            cmbType.ValueMember = "ID";
            cmbType.DisplayMember = "Supplier";

            //By default nothing will be selected
            cmbType.SelectedIndex = -1;
        }


        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            
        }

        private void cmbType_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //get the Value from Type
            string selected = this.cmbType.GetItemText(this.cmbType.SelectedValue);

            OleDbCommand cmd = new OleDbCommand();
            OleDbDataAdapter oleda = new OleDbDataAdapter();
            DataSet ds = new DataSet();
            cmd.Connection = oledbConn;
            cmd.CommandType = CommandType.Text;

            // Passing values to the combo boxes
            // selecting the Set for the suppliers

            try
            {
                cmd.CommandText = "SELECT DISTINCT [Set] FROM [" + selected + "$] WHERE Set IS NOT NULL";
                oleda = new OleDbDataAdapter(cmd);
                oleda.Fill(ds);
                oleda.Dispose();
                oleda.Dispose();

                cmbSet.DataSource = ds.Tables[0].DefaultView;
                cmbSet.ValueMember = "Set";
                cmbSet.DisplayMember = "Set";
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.ToString());
            }

            //By default nothing will be selected
            cmbSet.SelectedIndex = -1;
            
        }

        private void btnFindCard_Click(object sender, EventArgs e)
        {
            string selected = this.cmbType.GetItemText(this.cmbType.SelectedValue);
            string CardName = txtName.Text;
            string CardType = this.cmbType.GetItemText(this.cmbType.SelectedItem);
            string CardSet = cmbSet.GetItemText(this.cmbSet.SelectedValue);

            string[] strs = new string[] { CardName, CardSet, CardType };
            if (strs.All(str => string.IsNullOrEmpty(str)))
            {
                MessageBox.Show("Please fill all the values!");
            }
            else
            {
                OleDbCommand cmd = new OleDbCommand();
                OleDbDataAdapter oleda = new OleDbDataAdapter();
                DataSet ds = new DataSet();
                cmd.Connection = oledbConn;
                cmd.CommandType = CommandType.Text;

                try
                {
                    cmd.CommandText = "SELECT [Name],[Type],[Set],[Quantity],[Price] FROM [" + selected + "$] WHERE [Name]=@CardName AND [Type]=@CardType AND [Set]=@CardSet";
                    cmd.Parameters.AddWithValue("@CardName", CardName);
                    cmd.Parameters.AddWithValue("@CardType", CardType);
                    cmd.Parameters.AddWithValue("@CardSet", CardSet);
                    oleda = new OleDbDataAdapter(cmd);
                    oleda.Fill(ds);
                    oleda.Dispose();
                    DisplayName = ds.Tables[0].Rows[0]["Name"].ToString();
                    DisplayType = ds.Tables[0].Rows[0]["Type"].ToString();
                    DisplaySet = ds.Tables[0].Rows[0]["Set"].ToString();
                    DisplayQty = ds.Tables[0].Rows[0]["Quantity"].ToString();
                    DisplayPrice = ds.Tables[0].Rows[0]["Price"].ToString()+ " $";
                    ImagePath = "Images for Three Kings\\" + selected + "\\" + CardName + ".jpg";


                    //open the view dialog
                    // this.Hide();
                    if (File.Exists(ImagePath))
                    {
                        frmCardDisplay frmDisp = new frmCardDisplay();
                        frmDisp.ShowDialog();
                        if (frmDisp.DialogResult == DialogResult.OK)
                        {
                            txtName.Text = "";
                            cmbType.SelectedIndex = -1;
                            cmbSet.SelectedIndex = -1;

                        }
                        else
                        {
                            this.Close();
                            CloseExcelConnection();
                        }
                    }
                    else
                    {
                        MessageBox.Show("The Card " + DisplayName + " of " + DisplayType + " Does not exist");
                    }
                    //MessageBox.Show(frmDisp.ToString());
                    
                    //come back and reset values
                    
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.ToString());
                }

                
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            CloseExcelConnection();
        }
    }
}
