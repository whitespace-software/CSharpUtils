namespace MIReport
{
    partial class Form1
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
            this.txtSettingsFile = new System.Windows.Forms.TextBox();
            this.btnPickSettingsFile = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblSayuser = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSaveTo = new System.Windows.Forms.TextBox();
            this.btnPickSaveTo = new System.Windows.Forms.Button();
            this.btnRun = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 20);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Settings File";
            // 
            // txtSettingsFile
            // 
            this.txtSettingsFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSettingsFile.Location = new System.Drawing.Point(126, 20);
            this.txtSettingsFile.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtSettingsFile.Name = "txtSettingsFile";
            this.txtSettingsFile.Size = new System.Drawing.Size(658, 26);
            this.txtSettingsFile.TabIndex = 1;
            // 
            // btnPickSettingsFile
            // 
            this.btnPickSettingsFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPickSettingsFile.Location = new System.Drawing.Point(795, 14);
            this.btnPickSettingsFile.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnPickSettingsFile.Name = "btnPickSettingsFile";
            this.btnPickSettingsFile.Size = new System.Drawing.Size(39, 35);
            this.btnPickSettingsFile.TabIndex = 2;
            this.btnPickSettingsFile.Text = "...";
            this.btnPickSettingsFile.UseVisualStyleBackColor = true;
            this.btnPickSettingsFile.Click += new System.EventHandler(this.btnPickSettingsFile_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.lblSayuser);
            this.groupBox1.Location = new System.Drawing.Point(20, 163);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(814, 74);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            // 
            // lblSayuser
            // 
            this.lblSayuser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSayuser.Location = new System.Drawing.Point(10, 31);
            this.lblSayuser.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSayuser.Name = "lblSayuser";
            this.lblSayuser.Size = new System.Drawing.Size(795, 35);
            this.lblSayuser.TabIndex = 0;
            this.lblSayuser.Text = "lblSayuser";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 69);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Save To";
            // 
            // txtSaveTo
            // 
            this.txtSaveTo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSaveTo.Location = new System.Drawing.Point(126, 69);
            this.txtSaveTo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtSaveTo.Name = "txtSaveTo";
            this.txtSaveTo.Size = new System.Drawing.Size(658, 26);
            this.txtSaveTo.TabIndex = 5;
            // 
            // btnPickSaveTo
            // 
            this.btnPickSaveTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPickSaveTo.Location = new System.Drawing.Point(795, 69);
            this.btnPickSaveTo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnPickSaveTo.Name = "btnPickSaveTo";
            this.btnPickSaveTo.Size = new System.Drawing.Size(39, 35);
            this.btnPickSaveTo.TabIndex = 6;
            this.btnPickSaveTo.Text = "...";
            this.btnPickSaveTo.UseVisualStyleBackColor = true;
            this.btnPickSaveTo.Click += new System.EventHandler(this.btnPickSaveTo_Click);
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(369, 109);
            this.btnRun.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(112, 35);
            this.btnRun.TabIndex = 7;
            this.btnRun.Text = "Run Report";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(248, 109);
            this.btnTest.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(112, 35);
            this.btnTest.TabIndex = 8;
            this.btnTest.Text = "Test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(126, 109);
            this.btnConnect.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(112, 35);
            this.btnConnect.TabIndex = 9;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(852, 255);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.btnPickSaveTo);
            this.Controls.Add(this.txtSaveTo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnPickSettingsFile);
            this.Controls.Add(this.txtSettingsFile);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Form1";
            this.Text = "Whitespace MI Report";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSettingsFile;
        private System.Windows.Forms.Button btnPickSettingsFile;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblSayuser;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSaveTo;
        private System.Windows.Forms.Button btnPickSaveTo;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnConnect;
    }
}

