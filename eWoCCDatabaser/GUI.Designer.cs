namespace eWoCCDatabaser
{
    partial class GUI
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
            this.label2 = new System.Windows.Forms.Label();
            this.selectRootFolder = new System.Windows.Forms.Button();
            this.addEventLogs = new System.Windows.Forms.CheckBox();
            this.schemaName = new System.Windows.Forms.TextBox();
            this.schemaSuffix = new System.Windows.Forms.Label();
            this.modelInputsNames = new System.Windows.Forms.TextBox();
            this.modelInputsLabel = new System.Windows.Forms.Label();
            this.mergeData = new System.Windows.Forms.TextBox();
            this.mergeLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(425, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Welcome to the HVCCC eWoCC Files to Database System";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(402, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Please select the parent directory of the eWoCC system";
            // 
            // selectRootFolder
            // 
            this.selectRootFolder.ForeColor = System.Drawing.SystemColors.ControlText;
            this.selectRootFolder.Location = new System.Drawing.Point(16, 88);
            this.selectRootFolder.Name = "selectRootFolder";
            this.selectRootFolder.Size = new System.Drawing.Size(156, 44);
            this.selectRootFolder.TabIndex = 2;
            this.selectRootFolder.Text = "Select Root Folder";
            this.selectRootFolder.UseVisualStyleBackColor = true;
            this.selectRootFolder.Click += new System.EventHandler(this.selectRootFolder_Click);
            // 
            // addEventLogs
            // 
            this.addEventLogs.AutoSize = true;
            this.addEventLogs.Location = new System.Drawing.Point(16, 161);
            this.addEventLogs.Name = "addEventLogs";
            this.addEventLogs.Size = new System.Drawing.Size(148, 24);
            this.addEventLogs.TabIndex = 4;
            this.addEventLogs.Text = "Add Event Logs";
            this.addEventLogs.UseVisualStyleBackColor = true;
            // 
            // schemaName
            // 
            this.schemaName.Location = new System.Drawing.Point(340, 97);
            this.schemaName.Name = "schemaName";
            this.schemaName.Size = new System.Drawing.Size(225, 26);
            this.schemaName.TabIndex = 5;
            this.schemaName.Text = "SENG4800Rail1_04122016";
            this.schemaName.TextChanged += new System.EventHandler(this.schemaName_TextChanged);
            // 
            // schemaSuffix
            // 
            this.schemaSuffix.AutoSize = true;
            this.schemaSuffix.Location = new System.Drawing.Point(187, 97);
            this.schemaSuffix.Name = "schemaSuffix";
            this.schemaSuffix.Size = new System.Drawing.Size(112, 20);
            this.schemaSuffix.TabIndex = 6;
            this.schemaSuffix.Text = "Schema Suffix";
            // 
            // modelInputsNames
            // 
            this.modelInputsNames.Location = new System.Drawing.Point(340, 165);
            this.modelInputsNames.Multiline = true;
            this.modelInputsNames.Name = "modelInputsNames";
            this.modelInputsNames.Size = new System.Drawing.Size(225, 212);
            this.modelInputsNames.TabIndex = 8;
            this.modelInputsNames.Text = "runParameter,logSwitch";
            // 
            // modelInputsLabel
            // 
            this.modelInputsLabel.AutoSize = true;
            this.modelInputsLabel.Location = new System.Drawing.Point(187, 168);
            this.modelInputsLabel.Name = "modelInputsLabel";
            this.modelInputsLabel.Size = new System.Drawing.Size(136, 120);
            this.modelInputsLabel.TabIndex = 9;
            this.modelInputsLabel.Text = "Model Inputs \r\nto import\r\n\r\nDelimeter is \r\na comma followed\r\nby a space (, )";
            // 
            // mergeData
            // 
            this.mergeData.Location = new System.Drawing.Point(657, 165);
            this.mergeData.Multiline = true;
            this.mergeData.Name = "mergeData";
            this.mergeData.Size = new System.Drawing.Size(225, 212);
            this.mergeData.TabIndex = 10;
            this.mergeData.Text = "runParameter,logSwitch";
            // 
            // mergeLabel
            // 
            this.mergeLabel.AutoSize = true;
            this.mergeLabel.Location = new System.Drawing.Point(653, 73);
            this.mergeLabel.Name = "mergeLabel";
            this.mergeLabel.Size = new System.Drawing.Size(329, 80);
            this.mergeLabel.TabIndex = 11;
            this.mergeLabel.Text = "Post processing data to merge\r\n\r\nDelimeter is a comma followed by a space (, ) \r\n" +
    "for pairs\r\n";
            // 
            // GUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1019, 450);
            this.Controls.Add(this.mergeLabel);
            this.Controls.Add(this.mergeData);
            this.Controls.Add(this.modelInputsLabel);
            this.Controls.Add(this.modelInputsNames);
            this.Controls.Add(this.schemaSuffix);
            this.Controls.Add(this.schemaName);
            this.Controls.Add(this.addEventLogs);
            this.Controls.Add(this.selectRootFolder);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "GUI";
            this.Text = "HVCCC eWoCC Files to Database";
            this.Load += new System.EventHandler(this.GUI_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button selectRootFolder;
        private System.Windows.Forms.CheckBox addEventLogs;
        private System.Windows.Forms.TextBox schemaName;
        private System.Windows.Forms.Label schemaSuffix;
        private System.Windows.Forms.TextBox modelInputsNames;
        private System.Windows.Forms.Label modelInputsLabel;
        private System.Windows.Forms.TextBox mergeData;
        private System.Windows.Forms.Label mergeLabel;
    }
}

