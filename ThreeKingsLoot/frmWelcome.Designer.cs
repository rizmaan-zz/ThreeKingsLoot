namespace ThreeKingsLoot
{
    partial class frmWelcome
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmWelcome));
            this.pBThreeKingsLoot = new System.Windows.Forms.PictureBox();
            this.btnTransaction = new System.Windows.Forms.Button();
            this.btnCardFinder = new System.Windows.Forms.Button();
            this.btnPlaceOrder = new System.Windows.Forms.Button();
            this.btnViewOrder = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pBThreeKingsLoot)).BeginInit();
            this.SuspendLayout();
            // 
            // pBThreeKingsLoot
            // 
            this.pBThreeKingsLoot.Image = ((System.Drawing.Image)(resources.GetObject("pBThreeKingsLoot.Image")));
            this.pBThreeKingsLoot.Location = new System.Drawing.Point(12, 92);
            this.pBThreeKingsLoot.Name = "pBThreeKingsLoot";
            this.pBThreeKingsLoot.Size = new System.Drawing.Size(400, 139);
            this.pBThreeKingsLoot.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pBThreeKingsLoot.TabIndex = 0;
            this.pBThreeKingsLoot.TabStop = false;
            // 
            // btnTransaction
            // 
            this.btnTransaction.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTransaction.Location = new System.Drawing.Point(452, 35);
            this.btnTransaction.Name = "btnTransaction";
            this.btnTransaction.Size = new System.Drawing.Size(159, 39);
            this.btnTransaction.TabIndex = 1;
            this.btnTransaction.Text = "Transaction";
            this.btnTransaction.UseVisualStyleBackColor = true;
            this.btnTransaction.Click += new System.EventHandler(this.btnTransaction_Click);
            // 
            // btnCardFinder
            // 
            this.btnCardFinder.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCardFinder.Location = new System.Drawing.Point(452, 102);
            this.btnCardFinder.Name = "btnCardFinder";
            this.btnCardFinder.Size = new System.Drawing.Size(159, 39);
            this.btnCardFinder.TabIndex = 2;
            this.btnCardFinder.Text = "Card Finder";
            this.btnCardFinder.UseVisualStyleBackColor = true;
            this.btnCardFinder.Click += new System.EventHandler(this.btnCardFinder_Click);
            // 
            // btnPlaceOrder
            // 
            this.btnPlaceOrder.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPlaceOrder.Location = new System.Drawing.Point(452, 170);
            this.btnPlaceOrder.Name = "btnPlaceOrder";
            this.btnPlaceOrder.Size = new System.Drawing.Size(159, 39);
            this.btnPlaceOrder.TabIndex = 3;
            this.btnPlaceOrder.Text = "Place Order";
            this.btnPlaceOrder.UseVisualStyleBackColor = true;
            this.btnPlaceOrder.Click += new System.EventHandler(this.btnPlaceOrder_Click);
            // 
            // btnViewOrder
            // 
            this.btnViewOrder.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnViewOrder.Location = new System.Drawing.Point(452, 241);
            this.btnViewOrder.Name = "btnViewOrder";
            this.btnViewOrder.Size = new System.Drawing.Size(159, 39);
            this.btnViewOrder.TabIndex = 4;
            this.btnViewOrder.Text = "View Order";
            this.btnViewOrder.UseVisualStyleBackColor = true;
            this.btnViewOrder.Click += new System.EventHandler(this.btnViewOrder_Click);
            // 
            // frmWelcome
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(624, 309);
            this.Controls.Add(this.btnViewOrder);
            this.Controls.Add(this.btnPlaceOrder);
            this.Controls.Add(this.btnCardFinder);
            this.Controls.Add(this.btnTransaction);
            this.Controls.Add(this.pBThreeKingsLoot);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmWelcome";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Welcome Page";
            ((System.ComponentModel.ISupportInitialize)(this.pBThreeKingsLoot)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pBThreeKingsLoot;
        private System.Windows.Forms.Button btnTransaction;
        private System.Windows.Forms.Button btnCardFinder;
        private System.Windows.Forms.Button btnPlaceOrder;
        private System.Windows.Forms.Button btnViewOrder;
    }
}

