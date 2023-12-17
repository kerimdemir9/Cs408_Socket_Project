namespace Server
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
            this.button_listen = new System.Windows.Forms.Button();
            this.richTextBox_logs = new System.Windows.Forms.RichTextBox();
            this.textBox_port = new System.Windows.Forms.TextBox();
            this.label_port = new System.Windows.Forms.Label();
            this.richTextBox_if100 = new System.Windows.Forms.RichTextBox();
            this.richTextBox_sps101 = new System.Windows.Forms.RichTextBox();
            this.if100 = new System.Windows.Forms.Label();
            this.sps101 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button_listen
            // 
            this.button_listen.AccessibleName = "button_listen";
            this.button_listen.Location = new System.Drawing.Point(250, 24);
            this.button_listen.Name = "button_listen";
            this.button_listen.Size = new System.Drawing.Size(103, 31);
            this.button_listen.TabIndex = 0;
            this.button_listen.Text = "Listen";
            this.button_listen.UseVisualStyleBackColor = true;
            this.button_listen.Click += new System.EventHandler(this.button_listen_Click);
            // 
            // richTextBox_logs
            // 
            this.richTextBox_logs.AccessibleName = "richTextBox_logs";
            this.richTextBox_logs.Location = new System.Drawing.Point(12, 63);
            this.richTextBox_logs.Name = "richTextBox_logs";
            this.richTextBox_logs.ReadOnly = true;
            this.richTextBox_logs.Size = new System.Drawing.Size(341, 462);
            this.richTextBox_logs.TabIndex = 2;
            this.richTextBox_logs.Text = "";
            // 
            // textBox_port
            // 
            this.textBox_port.AccessibleName = "textBox_port";
            this.textBox_port.Location = new System.Drawing.Point(93, 31);
            this.textBox_port.Name = "textBox_port";
            this.textBox_port.Size = new System.Drawing.Size(130, 22);
            this.textBox_port.TabIndex = 5;
            // 
            // label_port
            // 
            this.label_port.AccessibleName = "label_port";
            this.label_port.Location = new System.Drawing.Point(12, 31);
            this.label_port.Name = "label_port";
            this.label_port.Size = new System.Drawing.Size(47, 26);
            this.label_port.TabIndex = 7;
            this.label_port.Text = "Port:";
            // 
            // richTextBox_if100
            // 
            this.richTextBox_if100.AccessibleName = "richTextBox_if100";
            this.richTextBox_if100.Location = new System.Drawing.Point(411, 63);
            this.richTextBox_if100.Name = "richTextBox_if100";
            this.richTextBox_if100.ReadOnly = true;
            this.richTextBox_if100.Size = new System.Drawing.Size(629, 206);
            this.richTextBox_if100.TabIndex = 8;
            this.richTextBox_if100.Text = "";
            // 
            // richTextBox_sps101
            // 
            this.richTextBox_sps101.AccessibleName = "richTextBox_sps101";
            this.richTextBox_sps101.Location = new System.Drawing.Point(411, 319);
            this.richTextBox_sps101.Name = "richTextBox_sps101";
            this.richTextBox_sps101.ReadOnly = true;
            this.richTextBox_sps101.Size = new System.Drawing.Size(629, 206);
            this.richTextBox_sps101.TabIndex = 9;
            this.richTextBox_sps101.Text = "";
            // 
            // if100
            // 
            this.if100.AccessibleName = "label_port";
            this.if100.Location = new System.Drawing.Point(411, 35);
            this.if100.Name = "if100";
            this.if100.Size = new System.Drawing.Size(76, 28);
            this.if100.TabIndex = 10;
            this.if100.Text = "IF100";
            // 
            // sps101
            // 
            this.sps101.AccessibleName = "label_port";
            this.sps101.Location = new System.Drawing.Point(411, 288);
            this.sps101.Name = "sps101";
            this.sps101.Size = new System.Drawing.Size(79, 28);
            this.sps101.TabIndex = 11;
            this.sps101.Text = "SPS101";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1075, 537);
            this.Controls.Add(this.sps101);
            this.Controls.Add(this.if100);
            this.Controls.Add(this.richTextBox_sps101);
            this.Controls.Add(this.richTextBox_if100);
            this.Controls.Add(this.label_port);
            this.Controls.Add(this.textBox_port);
            this.Controls.Add(this.richTextBox_logs);
            this.Controls.Add(this.button_listen);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.RichTextBox richTextBox_if100;
        private System.Windows.Forms.RichTextBox richTextBox_sps101;
        private System.Windows.Forms.Label if100;
        private System.Windows.Forms.Label sps101;

        private System.Windows.Forms.RichTextBox richTextBox_logs;
        private System.Windows.Forms.TextBox textBox_port;

        private System.Windows.Forms.Button button_listen;
        private System.Windows.Forms.Label label_port;

        #endregion
    }
}