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

namespace ThreeKingsLoot
{
    public partial class frmCardDisplay : Form
    {
        public frmCardDisplay()
        {
            InitializeComponent();
        }

        private void frmCardDisplay_Load(object sender, EventArgs e)
        {
            lblDisName.Text = frmCardFinder.DisplayName;
            lblDisType.Text = frmCardFinder.DisplayType;
            lblDisSet.Text = frmCardFinder.DisplaySet;
            lblDisQty.Text = frmCardFinder.DisplayQty;
            lblDisPrice.Text = frmCardFinder.DisplayPrice;
            string path = Path.GetFullPath(frmCardFinder.ImagePath);
            Image img = Image.FromFile(path);
            displaybox.Image = img;
         
         }

        private void btnNewSearch_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
