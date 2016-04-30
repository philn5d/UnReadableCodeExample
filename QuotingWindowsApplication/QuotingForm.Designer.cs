namespace QuotingWindowsApplication
{
    partial class QuotingForm
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
            this.ItemIdLabel = new System.Windows.Forms.Label();
            this.ItemId = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.AccountId = new System.Windows.Forms.TextBox();
            this.QuoteDetails = new System.Windows.Forms.TextBox();
            this.QuoteDetailsLabel = new System.Windows.Forms.Label();
            this.GetQuoteButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ItemIdLabel
            // 
            this.ItemIdLabel.AutoSize = true;
            this.ItemIdLabel.Location = new System.Drawing.Point(33, 16);
            this.ItemIdLabel.Name = "ItemIdLabel";
            this.ItemIdLabel.Size = new System.Drawing.Size(42, 13);
            this.ItemIdLabel.TabIndex = 0;
            this.ItemIdLabel.Text = "Item Id:";
            // 
            // ItemId
            // 
            this.ItemId.Location = new System.Drawing.Point(77, 13);
            this.ItemId.Name = "ItemId";
            this.ItemId.Size = new System.Drawing.Size(183, 20);
            this.ItemId.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Account Id:";
            // 
            // AccountId
            // 
            this.AccountId.Location = new System.Drawing.Point(77, 39);
            this.AccountId.Name = "AccountId";
            this.AccountId.Size = new System.Drawing.Size(183, 20);
            this.AccountId.TabIndex = 3;
            // 
            // QuoteDetails
            // 
            this.QuoteDetails.Location = new System.Drawing.Point(16, 113);
            this.QuoteDetails.Multiline = true;
            this.QuoteDetails.Name = "QuoteDetails";
            this.QuoteDetails.Size = new System.Drawing.Size(244, 137);
            this.QuoteDetails.TabIndex = 4;
            // 
            // QuoteDetailsLabel
            // 
            this.QuoteDetailsLabel.AutoSize = true;
            this.QuoteDetailsLabel.Location = new System.Drawing.Point(13, 97);
            this.QuoteDetailsLabel.Name = "QuoteDetailsLabel";
            this.QuoteDetailsLabel.Size = new System.Drawing.Size(71, 13);
            this.QuoteDetailsLabel.TabIndex = 5;
            this.QuoteDetailsLabel.Text = "Quote Details";
            // 
            // GetQuoteButton
            // 
            this.GetQuoteButton.Location = new System.Drawing.Point(185, 65);
            this.GetQuoteButton.Name = "GetQuoteButton";
            this.GetQuoteButton.Size = new System.Drawing.Size(75, 23);
            this.GetQuoteButton.TabIndex = 6;
            this.GetQuoteButton.Text = "Get Quote";
            this.GetQuoteButton.UseVisualStyleBackColor = true;
            this.GetQuoteButton.Click += new System.EventHandler(this.GetQuoteButton_Click);
            // 
            // QuotingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.GetQuoteButton);
            this.Controls.Add(this.QuoteDetailsLabel);
            this.Controls.Add(this.QuoteDetails);
            this.Controls.Add(this.AccountId);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ItemId);
            this.Controls.Add(this.ItemIdLabel);
            this.Name = "QuotingForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ItemIdLabel;
        private System.Windows.Forms.TextBox ItemId;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox AccountId;
        private System.Windows.Forms.TextBox QuoteDetails;
        private System.Windows.Forms.Label QuoteDetailsLabel;
        private System.Windows.Forms.Button GetQuoteButton;
    }
}

