namespace EMVS
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
            this.btnStart = new System.Windows.Forms.Button();
            this.topLLabe = new System.Windows.Forms.Label();
            this.topRLabe = new System.Windows.Forms.Label();
            this.bottomRLabe = new System.Windows.Forms.Label();
            this.bottomLLabe = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(312, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "label1";
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(12, 12);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 2;
            this.btnStart.Text = "Go";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // topLLabe
            // 
            this.topLLabe.AutoSize = true;
            this.topLLabe.Location = new System.Drawing.Point(1, 38);
            this.topLLabe.Name = "topLLabe";
            this.topLLabe.Size = new System.Drawing.Size(41, 12);
            this.topLLabe.TabIndex = 3;
            this.topLLabe.Text = "label2";
            // 
            // topRLabe
            // 
            this.topRLabe.AutoSize = true;
            this.topRLabe.Location = new System.Drawing.Point(889, 24);
            this.topRLabe.Name = "topRLabe";
            this.topRLabe.Size = new System.Drawing.Size(41, 12);
            this.topRLabe.TabIndex = 4;
            this.topRLabe.Text = "label3";
            // 
            // bottomRLabe
            // 
            this.bottomRLabe.AutoSize = true;
            this.bottomRLabe.Location = new System.Drawing.Point(889, 446);
            this.bottomRLabe.Name = "bottomRLabe";
            this.bottomRLabe.Size = new System.Drawing.Size(41, 12);
            this.bottomRLabe.TabIndex = 5;
            this.bottomRLabe.Text = "label4";
            // 
            // bottomLLabe
            // 
            this.bottomLLabe.AutoSize = true;
            this.bottomLLabe.Location = new System.Drawing.Point(10, 446);
            this.bottomLLabe.Name = "bottomLLabe";
            this.bottomLLabe.Size = new System.Drawing.Size(41, 12);
            this.bottomLLabe.TabIndex = 6;
            this.bottomLLabe.Text = "label5";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(989, 506);
            this.Controls.Add(this.bottomLLabe);
            this.Controls.Add(this.bottomRLabe);
            this.Controls.Add(this.topRLabe);
            this.Controls.Add(this.topLLabe);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label topLLabe;
        private System.Windows.Forms.Label topRLabe;
        private System.Windows.Forms.Label bottomRLabe;
        private System.Windows.Forms.Label bottomLLabe;
    }
}

