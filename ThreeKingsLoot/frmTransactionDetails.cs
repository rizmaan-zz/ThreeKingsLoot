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
    public partial class frmTransactionDetails : Form
    {
        OleDbConnection oledbConn;
        DataTable DT;

        public frmTransactionDetails()
        {
            InitializeComponent();
        }

        private void frmTransactionDetails_Load(object sender, EventArgs e)
        {
            //hide the labels
            lblDate.Visible = false;
            lblDateDisp.Visible = false;
            lblTrans.Visible = false;
            btnExit.Visible = false;

            
            //Creation of the data Table

            DT = new DataTable();
            DT.Columns.Add("Name", typeof(string));
            DT.Columns.Add("Quantity", typeof(int));
            DT.Columns.Add("Price", typeof(decimal));
            DT.Columns.Add("ID", typeof(string));
            DT.Columns.Add("Total", typeof(decimal));
            
            ConnectToExcel();
            getDataFromList();
        }

        private void getDataFromList()
        {
            // loop through the list
            string CardID = "";
            string Qty = "";
            string Table = "";
            string[] words;
            string DBName = "";
            string DBQty = "";
            string DBPrice = "";
            string DBCardID = "";
            decimal DBTotal = 0.0m;

            ConnectToExcel();
            OleDbCommand cmd = new OleDbCommand();
            OleDbDataAdapter oleda = new OleDbDataAdapter();
            DataSet ds = new DataSet();
            BindingSource bs = new BindingSource();
            cmd.Connection = oledbConn;
            cmd.CommandType = CommandType.Text;

            

            //loop through the global list
            for (int i = 0; i < frmTransaction.trans.Count(); i++)
            {
                words = frmTransaction.trans[i].Split('@');
                CardID = words[0];
                Qty = words[1];
                words = CardID.Split('-');
                Table = words[0];

                cmd = new OleDbCommand();
                oleda = new OleDbDataAdapter();
                ds = new DataSet();
                cmd.Connection = oledbConn;
                cmd.CommandType = CommandType.Text;
                dgView.ReadOnly = false;
                //get the details for the Quatity 
                cmd.CommandText = "SELECT [Name],[Price],[Quantity],[ID] FROM [" + Table + "$] WHERE [ID]='" + CardID + "'";
                oleda = new OleDbDataAdapter(cmd);
                oleda.Fill(ds);
                oleda.Dispose();
                DBName = ds.Tables[0].Rows[0]["Name"].ToString();
                DBQty = ds.Tables[0].Rows[0]["Quantity"].ToString();
                DBPrice = ds.Tables[0].Rows[0]["Price"].ToString();
                DBCardID = ds.Tables[0].Rows[0]["ID"].ToString();
                DBTotal = Convert.ToDecimal(ds.Tables[0].Rows[0]["Price"].ToString()) * Convert.ToDecimal(Qty);

                DT.Rows.Add(DBName, Convert.ToInt32(Qty), Convert.ToDecimal(DBPrice),DBCardID, Convert.ToDecimal(DBTotal));
            }
            bs.DataSource = DT;
            dgView.DataSource = bs;
            dgView.Columns["ID"].Visible = false;
            dgView.Columns["Total"].Visible = false;
            DBTotal = Convert.ToDecimal(DT.Compute("SUM(Total)",string.Empty));
            lblTotal.Text = "$" + DBTotal.ToString();

            dgView.ReadOnly = true;
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

        private void btmCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddCard_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure to delete row? Selecting No will Change to Edit Mode", "Confirmation", MessageBoxButtons.YesNo);

            DataView DV = DT.DefaultView;

            if (dr == DialogResult.Yes)
            {
                foreach (DataGridViewRow item in this.dgView.SelectedRows)
                {
                    DataGridViewRow row = dgView.Rows[item.Index];
                    string Name = row.Cells[0].Value.ToString();
                    string Qty = row.Cells[1].Value.ToString();
                    string id = row.Cells[3].Value.ToString();
                    dgView.ReadOnly = false;
                    dgView.Rows.RemoveAt(item.Index);
                    ////Update the datatable
                    //DV.RowFilter = "Name='" + Name + "' AND Quantity='" + Qty+"'";
                    frmTransaction.trans.Remove(id + "@" + Qty);


                }

                // dgView.DataSource.;

            }
            else if (dr == DialogResult.No)
            {
                foreach (DataGridViewRow item in this.dgView.SelectedRows)
                {
                    DataGridViewRow row = dgView.Rows[item.Index];
                    string Name = row.Cells[0].Value.ToString();
                    string Qty = row.Cells[1].Value.ToString();
                    string price = row.Cells[2].Value.ToString();

                    string value = Qty;
                    InputBox ip = new InputBox();
                    if (InputBox.InputBox1("Edit Quantity", "Pease enter New Quantity:", ref Qty) == DialogResult.OK)
                    {
                        row.Cells[1].Value= Qty;
                        decimal total = Convert.ToDecimal(Qty) * Convert.ToDecimal(price);
                        row.Cells[4].Value = total;
                    }
                    dgView.Refresh();

                }
            }
            try
            {
                lblTotal.Text = "$" + Convert.ToDecimal(DT.Compute("SUM(Total)", string.Empty)).ToString();
            }
            catch (Exception ee)
            {
                lblTotal.Text = "$0";
            }
            
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            //Conformation Message to Process the tranactions

            DialogResult proc = MessageBox.Show("All transctions will be commited to DB, Continue!", "Commit Transactions", MessageBoxButtons.YesNo);
            if(proc ==DialogResult.Yes)
            {
                //loop through the datagridiew and update the qty in transactios
                    OleDbCommand cmd = new OleDbCommand();
                    OleDbDataAdapter oleda = new OleDbDataAdapter();

               
                DataSet ds = new DataSet();
                cmd.Connection = oledbConn;
                cmd.CommandType = CommandType.Text;
                try
                {
                    foreach (DataGridViewRow item in this.dgView.SelectedRows)
                    {
                        DataGridViewRow row = dgView.Rows[item.Index];
                        string ID = row.Cells[3].Value.ToString();
                        string Name = row.Cells[0].Value.ToString();
                        string Qty = row.Cells[1].Value.ToString();
                        string[] idVal = ID.Split('-');
                        string DBQty = "";
                        int RemainQty = 0;


                        //get the quantity
                        cmd.CommandText = "SELECT [Quantity] FROM [" + idVal[0] + "$] WHERE [ID]='" + ID + "'";
                        oleda = new OleDbDataAdapter(cmd);
                        oleda.Fill(ds);
                        oleda.Dispose();
                        DBQty = ds.Tables[0].Rows[0]["Quantity"].ToString();
                        RemainQty = Convert.ToInt32(DBQty) - Convert.ToInt32(Qty);


                        //update the quantity

                        cmd.Connection = oledbConn;
                        cmd.CommandText = "UPDATE [" + idVal[0] + "$] SET [Quantity] = '" + RemainQty.ToString() + "' WHERE [ID]='" + ID + "'";
                        cmd.ExecuteNonQuery();
                    }

                }
                catch (Exception exr)
                {
                    MessageBox.Show(exr.Message.ToString());
                }
                finally
                {
                    lblDate.Visible = true;
                    lblDateDisp.Visible = true;
                    lblTrans.Visible = true;
                    btnExit.Visible = true;

                    btnAddCard.Visible = false;
                    btnDelete.Visible = false;
                    btnProcess.Visible = false;
                    btmCancel.Visible = false;

                    lblTrans.Text = "Transaction Completed!";
                    lblDateDisp.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
