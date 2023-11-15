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
            this.SuspendLayout();
            // 
            // button_listen
            // 
            this.button_listen.AccessibleName = "button_listen";
            this.button_listen.Location = new System.Drawing.Point(515, 114);
            this.button_listen.Name = "button_listen";
            this.button_listen.Size = new System.Drawing.Size(101, 25);
            this.button_listen.TabIndex = 0;
            this.button_listen.Text = "listen";
            this.button_listen.UseVisualStyleBackColor = true;
            this.button_listen.Click += new System.EventHandler(this.button_listen_Click);
            // 
            // richTextBox_logs
            // 
            this.richTextBox_logs.AccessibleName = "richTextBox_logs";
            this.richTextBox_logs.Location = new System.Drawing.Point(142, 154);
            this.richTextBox_logs.Name = "richTextBox_logs";
            this.richTextBox_logs.Size = new System.Drawing.Size(474, 214);
            this.richTextBox_logs.TabIndex = 2;
            this.richTextBox_logs.Text = "";
            // 
            // textBox_port
            // 
            this.textBox_port.AccessibleName = "textBox_port";
            this.textBox_port.Location = new System.Drawing.Point(234, 117);
            this.textBox_port.Name = "textBox_port";
            this.textBox_port.Size = new System.Drawing.Size(253, 22);
            this.textBox_port.TabIndex = 5;
            // 
            // label_port
            // 
            this.label_port.AccessibleName = "label_port";
            this.label_port.Location = new System.Drawing.Point(142, 114);
            this.label_port.Name = "label_port";
            this.label_port.Size = new System.Drawing.Size(86, 28);
            this.label_port.TabIndex = 7;
            this.label_port.Text = "Port:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label_port);
            this.Controls.Add(this.textBox_port);
            this.Controls.Add(this.richTextBox_logs);
            this.Controls.Add(this.button_listen);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.RichTextBox richTextBox_logs;
        private System.Windows.Forms.TextBox textBox_port;

        private System.Windows.Forms.Button button_listen;
        private System.Windows.Forms.Label label_port;

        #endregion
    }
}