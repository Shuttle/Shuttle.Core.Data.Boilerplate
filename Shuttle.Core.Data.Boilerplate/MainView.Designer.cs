namespace Shuttle.Core.Data.Boilerplate
{
    partial class MainView
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
            this.label1 = new System.Windows.Forms.Label();
            this.Table = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.GenerateOption = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Result = new System.Windows.Forms.TextBox();
            this.CopyButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.Filter = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.Copy = new System.Windows.Forms.ComboBox();
            this.ConnectionString = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.ConnectButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 132);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Table";
            // 
            // Table
            // 
            this.Table.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Table.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Table.FormattingEnabled = true;
            this.Table.Location = new System.Drawing.Point(18, 150);
            this.Table.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Table.Name = "Table";
            this.Table.Size = new System.Drawing.Size(878, 23);
            this.Table.TabIndex = 3;
            this.Table.SelectedIndexChanged += new System.EventHandler(this.Table_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 187);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "Generate";
            // 
            // GenerateOption
            // 
            this.GenerateOption.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GenerateOption.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.GenerateOption.FormattingEnabled = true;
            this.GenerateOption.Items.AddRange(new object[] {
            "Columns",
            "Value",
            "PropertyMappedFrom",
            "Contains",
            "Select",
            "Insert",
            "Update",
            "Properties",
            "Arguments",
            "Object"});
            this.GenerateOption.Location = new System.Drawing.Point(18, 207);
            this.GenerateOption.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.GenerateOption.Name = "GenerateOption";
            this.GenerateOption.Size = new System.Drawing.Size(510, 23);
            this.GenerateOption.TabIndex = 5;
            this.GenerateOption.SelectedIndexChanged += new System.EventHandler(this.GenerateOption_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 243);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 15);
            this.label3.TabIndex = 6;
            this.label3.Text = "Code";
            // 
            // Result
            // 
            this.Result.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Result.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Result.Location = new System.Drawing.Point(18, 263);
            this.Result.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Result.Multiline = true;
            this.Result.Name = "Result";
            this.Result.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.Result.Size = new System.Drawing.Size(879, 417);
            this.Result.TabIndex = 8;
            // 
            // CopyButton
            // 
            this.CopyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CopyButton.Location = new System.Drawing.Point(807, 200);
            this.CopyButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.CopyButton.Name = "CopyButton";
            this.CopyButton.Size = new System.Drawing.Size(88, 37);
            this.CopyButton.TabIndex = 7;
            this.CopyButton.Text = "copy";
            this.CopyButton.UseVisualStyleBackColor = true;
            this.CopyButton.Click += new System.EventHandler(this.CopyButton_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 75);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 15);
            this.label4.TabIndex = 0;
            this.label4.Text = "Filter";
            // 
            // Filter
            // 
            this.Filter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Filter.Location = new System.Drawing.Point(18, 93);
            this.Filter.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Filter.Name = "Filter";
            this.Filter.Size = new System.Drawing.Size(878, 23);
            this.Filter.TabIndex = 1;
            this.Filter.TextChanged += new System.EventHandler(this.Filter_TextChanged);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(544, 188);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 15);
            this.label5.TabIndex = 9;
            this.label5.Text = "Copy";
            // 
            // Copy
            // 
            this.Copy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Copy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Copy.FormattingEnabled = true;
            this.Copy.Items.AddRange(new object[] {
            "",
            "ClassName",
            "ObjectName",
            "*Columns"});
            this.Copy.Location = new System.Drawing.Point(545, 208);
            this.Copy.Margin = new System.Windows.Forms.Padding(2);
            this.Copy.Name = "Copy";
            this.Copy.Size = new System.Drawing.Size(245, 23);
            this.Copy.TabIndex = 10;
            this.Copy.SelectedIndexChanged += new System.EventHandler(this.Copy_SelectedIndexChanged);
            // 
            // ConnectionString
            // 
            this.ConnectionString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ConnectionString.Location = new System.Drawing.Point(18, 37);
            this.ConnectionString.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ConnectionString.Name = "ConnectionString";
            this.ConnectionString.Size = new System.Drawing.Size(770, 23);
            this.ConnectionString.TabIndex = 12;
            this.ConnectionString.Text = "data source=.\\sqlexpress;initial catalog=database;integrated security=true";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(18, 18);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(228, 15);
            this.label6.TabIndex = 11;
            this.label6.Text = "Connection string (System.Data.SqlClient)";
            // 
            // ConnectButton
            // 
            this.ConnectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ConnectButton.Location = new System.Drawing.Point(805, 29);
            this.ConnectButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(88, 37);
            this.ConnectButton.TabIndex = 13;
            this.ConnectButton.Text = "connect";
            this.ConnectButton.UseVisualStyleBackColor = true;
            this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // MainView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(913, 700);
            this.Controls.Add(this.ConnectButton);
            this.Controls.Add(this.ConnectionString);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.Copy);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Filter);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.CopyButton);
            this.Controls.Add(this.Result);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.GenerateOption);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Table);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MinimumSize = new System.Drawing.Size(929, 739);
            this.Name = "MainView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Shuttle.Core.Data.Boilerplate";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox Table;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox GenerateOption;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox Result;
        private System.Windows.Forms.Button CopyButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox Filter;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox Copy;
        private System.Windows.Forms.TextBox ConnectionString;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button ConnectButton;
    }
}

