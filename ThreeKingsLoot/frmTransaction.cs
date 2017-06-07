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
    public partial class frmTransaction : Form
    {
        public static List<string> trans = new List<string>();
        OleDbConnection oledbConn;

        public frmTransaction()
        {
            InitializeComponent();
        }

        private void btnAddCard_Click(object sender, EventArgs e)
        {
            string CardID = txtCardID.Text;
            string Qty = txtQuantity.Text;
            string DBQty = "";
            string[] CardIDSplit = CardID.Split('-');
            string DBCardID = CardIDSplit[0];
            int RemainQty = 0;

            ConnectToExcel();
            OleDbCommand cmd = new OleDbCommand();
            OleDbDataAdapter oleda = new OleDbDataAdapter();
            DataSet ds = new DataSet();
            cmd.Connection = oledbConn;
            cmd.CommandType = CommandType.Text;

            string[] strs = new string[] { CardID, Qty};
            if (strs.All(str => string.IsNullOrEmpty(str)))
            {
                MessageBox.Show("Please fill all the values!");
            }
            else
            {
                cmd.CommandText = "SELECT [Quantity] FROM [" + DBCardID + "$] WHERE [ID]='" + CardID + "'";
                oleda = new OleDbDataAdapter(cmd);
                oleda.Fill(ds);
                oleda.Dispose();
                DBQty = ds.Tables[0].Rows[0]["Quantity"].ToString();
                RemainQty = Convert.ToInt32(DBQty);

                for (int i = 0; i < trans.Count(); i++)
                {
                    string[] T = trans[i].Split('@');
                    RemainQty = Convert.ToInt32(DBQty) - Convert.ToInt32(T[1]);

                }

                if (Convert.ToInt32(Qty)>RemainQty)
                {

                    MessageBox.Show("Cannot continue with the transaction, Only " + RemainQty.ToString() + " Cards Remaining");
                }
                else
                {
                    trans.Add(CardID+"@"+ Qty );
                    MessageBox.Show(CardID + " Added to the transaction queue");
                }
                txtCardID.Text = "";
                txtQuantity.Text = "";
                CardID = "";
                Qty = "";
                
            }
        }

        private void frmTransaction_Load(object sender, EventArgs e)
        { 

        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            //Check if values are entered so that can continue
            string CardID = txtCardID.Text;
            string Qty = txtQuantity.Text;
            string[] strs = new string[] { CardID, Qty };
            if (strs.All(str => string.IsNullOrEmpty(str)))
            {
               
                //MessageBox.Show("Please fill all the values! To Continue");
            }
            else
            {
                DialogResult drs = MessageBox.Show("There are unsaved values in the form, Press Yes to save,  No to Continue", "Unsaved Data", MessageBoxButtons.YesNo);

                if (drs == DialogResult.Yes)
                {
                    btnAddCard.PerformClick();
                }
                else if (drs == DialogResult.No)
                {

                }
               
            }
            frmTransactionDetails frmTD = new frmTransactionDetails();
                frmTD.ShowDialog();
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
