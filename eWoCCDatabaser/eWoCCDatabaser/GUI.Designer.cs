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
            // GUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
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
    }
}

