namespace Client
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
            this.textBox_port = new System.Windows.Forms.TextBox();
            this.button_connect = new System.Windows.Forms.Button();
            this.textBox_name = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button_send = new System.Windows.Forms.Button();
            this.richTextBox_if100 = new System.Windows.Forms.RichTextBox();
            this.richTextBox_sps101 = new System.Windows.Forms.RichTextBox();
            this.button_sub_if100 = new System.Windows.Forms.Button();
            this.button_sub_sps101 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_if100 = new System.Windows.Forms.TextBox();
            this.textBox_sps101 = new System.Windows.Forms.TextBox();
            this.button_send_if100 = new System.Windows.Forms.Button();
            this.button_send_sps101 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(166, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Port:";
            // 
            // textBox_port
            // 
            this.textBox_port.AccessibleName = "textBox_port";
            this.textBox_port.Location = new System.Drawing.Point(310, 38);
            this.textBox_port.Name = "textBox_port";
            this.textBox_port.Size = new System.Drawing.Size(150, 22);
            this.textBox_port.TabIndex = 1;
            // 
            // button_connect
            // 
            this.button_connect.AccessibleName = "button_connect";
            this.button_connect.Location = new System.Drawing.Point(542, 35);
            this.button_connect.Name = "button_connect";
            this.button_connect.Size = new System.Drawing.Size(102, 28);
            this.button_connect.TabIndex = 2;
            this.button_connect.Text = "connect";
            this.button_connect.UseVisualStyleBackColor = true;
            this.button_connect.Click += new System.EventHandler(this.button_connect_Click);
            // 
            // textBox_name
            // 
            this.textBox_name.AccessibleName = "textBox_name";
            this.textBox_name.Location = new System.Drawing.Point(310, 93);
            this.textBox_name.Name = "textBox_name";
            this.textBox_name.Size = new System.Drawing.Size(150, 22);
            this.textBox_name.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(166, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "Name:";
            // 
            // button_send
            // 
            this.button_send.AccessibleName = "button_send";
            this.button_send.Location = new System.Drawing.Point(542, 90);
            this.button_send.Name = "button_send";
            this.button_send.Size = new System.Drawing.Size(102, 28);
            this.button_send.TabIndex = 5;
            this.button_send.Text = "send";
            this.button_send.UseVisualStyleBackColor = true;
            this.button_send.Click += new System.EventHandler(this.button_send_Click);
            // 
            // richTextBox_if100
            // 
            this.richTextBox_if100.Location = new System.Drawing.Point(53, 243);
            this.richTextBox_if100.Name = "richTextBox_if100";
            this.richTextBox_if100.Size = new System.Drawing.Size(140, 166);
            this.richTextBox_if100.TabIndex = 6;
            this.richTextBox_if100.Text = "";
            // 
            // richTextBox_sps101
            // 
            this.richTextBox_sps101.Location = new System.Drawing.Point(422, 243);
            this.richTextBox_sps101.Name = "richTextBox_sps101";
            this.richTextBox_sps101.Size = new System.Drawing.Size(140, 166);
            this.richTextBox_sps101.TabIndex = 7;
            this.richTextBox_sps101.Text = "";
            // 
            // button_sub_if100
            // 
            this.button_sub_if100.AccessibleName = "button_sub_if100";
            this.button_sub_if100.Location = new System.Drawing.Point(72, 415);
            this.button_sub_if100.Name = "button_sub_if100";
            this.button_sub_if100.Size = new System.Drawing.Size(102, 28);
            this.button_sub_if100.TabIndex = 8;
            this.button_sub_if100.Text = "subscribe";
            this.button_sub_if100.UseVisualStyleBackColor = true;
            // 
            // button_sub_sps101
            // 
            this.button_sub_sps101.AccessibleName = "button_sub_sps101";
            this.button_sub_sps101.Location = new System.Drawing.Point(441, 415);
            this.button_sub_sps101.Name = "button_sub_sps101";
            this.button_sub_sps101.Size = new System.Drawing.Size(102, 28);
            this.button_sub_sps101.TabIndex = 9;
            this.button_sub_sps101.Text = "subscribe";
            this.button_sub_sps101.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(72, 225);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 15);
            this.label3.TabIndex = 10;
            this.label3.Text = "IF100";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(441, 225);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 15);
            this.label4.TabIndex = 11;
            this.label4.Text = "SPS101";
            // 
            // textBox_if100
            // 
            this.textBox_if100.AccessibleName = "textBox_if100";
            this.textBox_if100.Location = new System.Drawing.Point(199, 287);
            this.textBox_if100.Name = "textBox_if100";
            this.textBox_if100.Size = new System.Drawing.Size(206, 22);
            this.textBox_if100.TabIndex = 12;
            // 
            // textBox_sps101
            // 
            this.textBox_sps101.AccessibleName = "textBox_sps101";
            this.textBox_sps101.Location = new System.Drawing.Point(568, 287);
            this.textBox_sps101.Name = "textBox_sps101";
            this.textBox_sps101.Size = new System.Drawing.Size(206, 22);
            this.textBox_sps101.TabIndex = 13;
            // 
            // button_send_if100
            // 
            this.button_send_if100.AccessibleName = "button_send_if100";
            this.button_send_if100.Location = new System.Drawing.Point(199, 315);
            this.button_send_if100.Name = "button_send_if100";
            this.button_send_if100.Size = new System.Drawing.Size(102, 28);
            this.button_send_if100.TabIndex = 14;
            this.button_send_if100.Text = "send";
            this.button_send_if100.UseVisualStyleBackColor = true;
            // 
            // button_send_sps101
            // 
            this.button_send_sps101.AccessibleName = "button_send_sps101";
            this.button_send_sps101.Location = new System.Drawing.Point(568, 315);
            this.button_send_sps101.Name = "button_send_sps101";
            this.button_send_sps101.Size = new System.Drawing.Size(102, 28);
            this.button_send_sps101.TabIndex = 15;
            this.button_send_sps101.Text = "send";
            this.button_send_sps101.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button_send_sps101);
            this.Controls.Add(this.button_send_if100);
            this.Controls.Add(this.textBox_sps101);
            this.Controls.Add(this.textBox_if100);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button_sub_sps101);
            this.Controls.Add(this.button_sub_if100);
            this.Controls.Add(this.richTextBox_sps101);
            this.Controls.Add(this.richTextBox_if100);
            this.Controls.Add(this.button_send);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox_name);
            this.Controls.Add(this.button_connect);
            this.Controls.Add(this.textBox_port);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Button button_send_if100;
        private System.Windows.Forms.Button button_send_sps101;

        private System.Windows.Forms.TextBox textBox_sps101;

        private System.Windows.Forms.TextBox textBox_if100;

        private System.Windows.Forms.RichTextBox richTextBox_if100;
        private System.Windows.Forms.RichTextBox richTextBox_sps101;
        private System.Windows.Forms.Button button_sub_if100;
        private System.Windows.Forms.Button button_sub_sps101;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_port;
        private System.Windows.Forms.Button button_connect;
        private System.Windows.Forms.TextBox textBox_name;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button_send;

        #endregion
    }
}