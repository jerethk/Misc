
namespace ShopDB2
{
    partial class PurchaseScreen
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.productTable = new System.Windows.Forms.DataGridView();
            this.listBoxPurchaseList = new System.Windows.Forms.ListBox();
            this.btnAddToPurchase = new System.Windows.Forms.Button();
            this.btnShowProducts2 = new System.Windows.Forms.Button();
            this.labelTotalPrice = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnRemoveFromPurchase = new System.Windows.Forms.Button();
            this.tbInvoice = new System.Windows.Forms.TextBox();
            this.btnCheckoutSql = new System.Windows.Forms.Button();
            this.buttonClearPurchase = new System.Windows.Forms.Button();
            this.btnViewTransactions = new System.Windows.Forms.Button();
            this.labelItemCount = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cbCustomers = new System.Windows.Forms.ComboBox();
            this.btnEditCustomers = new System.Windows.Forms.Button();
            this.displayBox = new System.Windows.Forms.PictureBox();
            this.cbCategory = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.productTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.displayBox)).BeginInit();
            this.SuspendLayout();
            // 
            // productTable
            // 
            this.productTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.productTable.Location = new System.Drawing.Point(22, 102);
            this.productTable.MultiSelect = false;
            this.productTable.Name = "productTable";
            this.productTable.ReadOnly = true;
            this.productTable.RowTemplate.Height = 25;
            this.productTable.Size = new System.Drawing.Size(521, 272);
            this.productTable.TabIndex = 1;
            this.productTable.SelectionChanged += new System.EventHandler(this.productTable_SelectionChanged);
            // 
            // listBoxPurchaseList
            // 
            this.listBoxPurchaseList.FormattingEnabled = true;
            this.listBoxPurchaseList.ItemHeight = 15;
            this.listBoxPurchaseList.Location = new System.Drawing.Point(127, 426);
            this.listBoxPurchaseList.Name = "listBoxPurchaseList";
            this.listBoxPurchaseList.Size = new System.Drawing.Size(134, 184);
            this.listBoxPurchaseList.TabIndex = 4;
            // 
            // btnAddToPurchase
            // 
            this.btnAddToPurchase.Location = new System.Drawing.Point(22, 426);
            this.btnAddToPurchase.Name = "btnAddToPurchase";
            this.btnAddToPurchase.Size = new System.Drawing.Size(76, 30);
            this.btnAddToPurchase.TabIndex = 5;
            this.btnAddToPurchase.Text = "Add";
            this.btnAddToPurchase.UseVisualStyleBackColor = true;
            this.btnAddToPurchase.Click += new System.EventHandler(this.btnAddToPurchase_Click);
            // 
            // btnShowProducts2
            // 
            this.btnShowProducts2.Location = new System.Drawing.Point(426, 68);
            this.btnShowProducts2.Name = "btnShowProducts2";
            this.btnShowProducts2.Size = new System.Drawing.Size(117, 23);
            this.btnShowProducts2.TabIndex = 6;
            this.btnShowProducts2.Text = "Refresh";
            this.btnShowProducts2.UseVisualStyleBackColor = true;
            this.btnShowProducts2.Click += new System.EventHandler(this.btnShowProducts_Click);
            // 
            // labelTotalPrice
            // 
            this.labelTotalPrice.AutoSize = true;
            this.labelTotalPrice.Location = new System.Drawing.Point(127, 625);
            this.labelTotalPrice.Name = "labelTotalPrice";
            this.labelTotalPrice.Size = new System.Drawing.Size(34, 15);
            this.labelTotalPrice.TabIndex = 9;
            this.labelTotalPrice.Text = "$0.00";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(127, 405);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 15);
            this.label2.TabIndex = 10;
            this.label2.Text = "Shopping cart:";
            // 
            // btnRemoveFromPurchase
            // 
            this.btnRemoveFromPurchase.Location = new System.Drawing.Point(22, 462);
            this.btnRemoveFromPurchase.Name = "btnRemoveFromPurchase";
            this.btnRemoveFromPurchase.Size = new System.Drawing.Size(76, 30);
            this.btnRemoveFromPurchase.TabIndex = 11;
            this.btnRemoveFromPurchase.Text = "Remove";
            this.btnRemoveFromPurchase.UseVisualStyleBackColor = true;
            this.btnRemoveFromPurchase.Click += new System.EventHandler(this.btnRemoveFromPurchase_Click);
            // 
            // tbInvoice
            // 
            this.tbInvoice.Location = new System.Drawing.Point(308, 462);
            this.tbInvoice.Multiline = true;
            this.tbInvoice.Name = "tbInvoice";
            this.tbInvoice.Size = new System.Drawing.Size(205, 148);
            this.tbInvoice.TabIndex = 13;
            // 
            // btnCheckoutSql
            // 
            this.btnCheckoutSql.Location = new System.Drawing.Point(308, 426);
            this.btnCheckoutSql.Name = "btnCheckoutSql";
            this.btnCheckoutSql.Size = new System.Drawing.Size(97, 23);
            this.btnCheckoutSql.TabIndex = 14;
            this.btnCheckoutSql.Text = "Checkout";
            this.btnCheckoutSql.UseVisualStyleBackColor = true;
            this.btnCheckoutSql.Click += new System.EventHandler(this.btnCheckout_Click);
            // 
            // buttonClearPurchase
            // 
            this.buttonClearPurchase.Location = new System.Drawing.Point(22, 551);
            this.buttonClearPurchase.Name = "buttonClearPurchase";
            this.buttonClearPurchase.Size = new System.Drawing.Size(76, 30);
            this.buttonClearPurchase.TabIndex = 15;
            this.buttonClearPurchase.Text = "Clear";
            this.buttonClearPurchase.UseVisualStyleBackColor = true;
            this.buttonClearPurchase.Click += new System.EventHandler(this.buttonClearPurchase_Click);
            // 
            // btnViewTransactions
            // 
            this.btnViewTransactions.Location = new System.Drawing.Point(744, 15);
            this.btnViewTransactions.Name = "btnViewTransactions";
            this.btnViewTransactions.Size = new System.Drawing.Size(92, 23);
            this.btnViewTransactions.TabIndex = 16;
            this.btnViewTransactions.Text = "Transactions";
            this.btnViewTransactions.UseVisualStyleBackColor = true;
            this.btnViewTransactions.Click += new System.EventHandler(this.btnViewTransactions_Click);
            // 
            // labelItemCount
            // 
            this.labelItemCount.AutoSize = true;
            this.labelItemCount.Location = new System.Drawing.Point(234, 404);
            this.labelItemCount.Name = "labelItemCount";
            this.labelItemCount.Size = new System.Drawing.Size(13, 15);
            this.labelItemCount.TabIndex = 17;
            this.labelItemCount.Text = "0";
            this.labelItemCount.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(210, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 15);
            this.label3.TabIndex = 18;
            this.label3.Text = "Customer";
            // 
            // cbCustomers
            // 
            this.cbCustomers.FormattingEnabled = true;
            this.cbCustomers.Location = new System.Drawing.Point(275, 15);
            this.cbCustomers.Name = "cbCustomers";
            this.cbCustomers.Size = new System.Drawing.Size(189, 23);
            this.cbCustomers.TabIndex = 19;
            // 
            // btnEditCustomers
            // 
            this.btnEditCustomers.Location = new System.Drawing.Point(744, 44);
            this.btnEditCustomers.Name = "btnEditCustomers";
            this.btnEditCustomers.Size = new System.Drawing.Size(92, 23);
            this.btnEditCustomers.TabIndex = 20;
            this.btnEditCustomers.Text = "Customers";
            this.btnEditCustomers.UseVisualStyleBackColor = true;
            this.btnEditCustomers.Click += new System.EventHandler(this.btnEditCustomers_Click);
            // 
            // displayBox
            // 
            this.displayBox.BackColor = System.Drawing.Color.Black;
            this.displayBox.Location = new System.Drawing.Point(555, 102);
            this.displayBox.Name = "displayBox";
            this.displayBox.Size = new System.Drawing.Size(281, 272);
            this.displayBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.displayBox.TabIndex = 21;
            this.displayBox.TabStop = false;
            // 
            // cbCategory
            // 
            this.cbCategory.FormattingEnabled = true;
            this.cbCategory.Location = new System.Drawing.Point(140, 69);
            this.cbCategory.Name = "cbCategory";
            this.cbCategory.Size = new System.Drawing.Size(150, 23);
            this.cbCategory.TabIndex = 22;
            this.cbCategory.SelectedIndexChanged += new System.EventHandler(this.cbCategory_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 15);
            this.label1.TabIndex = 23;
            this.label1.Text = "Product category";
            // 
            // PurchaseScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(861, 668);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbCategory);
            this.Controls.Add(this.displayBox);
            this.Controls.Add(this.btnEditCustomers);
            this.Controls.Add(this.cbCustomers);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.labelItemCount);
            this.Controls.Add(this.btnViewTransactions);
            this.Controls.Add(this.buttonClearPurchase);
            this.Controls.Add(this.btnCheckoutSql);
            this.Controls.Add(this.tbInvoice);
            this.Controls.Add(this.btnRemoveFromPurchase);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelTotalPrice);
            this.Controls.Add(this.btnShowProducts2);
            this.Controls.Add(this.btnAddToPurchase);
            this.Controls.Add(this.listBoxPurchaseList);
            this.Controls.Add(this.productTable);
            this.Name = "PurchaseScreen";
            this.Text = "Shop";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.productTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.displayBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataGridView productTable;
        private System.Windows.Forms.ListBox listBoxPurchaseList;
        private System.Windows.Forms.Button btnAddToPurchase;
        private System.Windows.Forms.Button btnShowProducts2;
        private System.Windows.Forms.Label labelTotalPrice;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnRemoveFromPurchase;
        private System.Windows.Forms.TextBox tbInvoice;
        private System.Windows.Forms.Button btnCheckoutSql;
        private System.Windows.Forms.Button buttonClearPurchase;
        private System.Windows.Forms.Button btnViewTransactions;
        private System.Windows.Forms.Label labelItemCount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbCustomers;
        private System.Windows.Forms.Button btnEditCustomers;
        private System.Windows.Forms.PictureBox displayBox;
        private System.Windows.Forms.ComboBox cbCategory;
        private System.Windows.Forms.Label label1;
    }
}

