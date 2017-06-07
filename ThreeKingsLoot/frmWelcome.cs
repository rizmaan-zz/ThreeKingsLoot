using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThreeKingsLoot
{
    public partial class frmWelcome : Form
    {
        public frmWelcome()
        {
            InitializeComponent();
        }

        private void btnTransaction_Click(object sender, EventArgs e)
        {
            frmTransaction ftr = new frmTransaction();
            ftr.ShowDialog();
        }

        private void btnCardFinder_Click(object sender, EventArgs e)
        {
             frmCardFinder fCard = new frmCardFinder();
             fCard.ShowDialog();
        }

        private void btnPlaceOrder_Click(object sender, EventArgs e)
        {
            frmPlaceOrder fPlaceOrd = new frmPlaceOrder();
            fPlaceOrd.ShowDialog();
        }

        private void btnViewOrder_Click(object sender, EventArgs e)
        {
            frmViewOrder fViewOrd = new frmViewOrder();
            fViewOrd.ShowDialog();
        }
    }
}
