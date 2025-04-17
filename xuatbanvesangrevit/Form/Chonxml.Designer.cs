namespace xuatbanvesangrevit
{
    partial class Chonxml
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
            this.btnXml = new System.Windows.Forms.Button();
            this.txtXml = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnXml
            // 
            this.btnXml.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnXml.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnXml.Location = new System.Drawing.Point(260, 12);
            this.btnXml.Name = "btnXml";
            this.btnXml.Size = new System.Drawing.Size(30, 22);
            this.btnXml.TabIndex = 3;
            this.btnXml.Text = "...";
            this.btnXml.UseVisualStyleBackColor = true;
            this.btnXml.Click += new System.EventHandler(this.btnXml_Click);
            // 
            // txtXml
            // 
            this.txtXml.Location = new System.Drawing.Point(12, 12);
            this.txtXml.Name = "txtXml";
            this.txtXml.Size = new System.Drawing.Size(242, 22);
            this.txtXml.TabIndex = 2;
            // 
            // Chonxml
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(309, 50);
            this.Controls.Add(this.btnXml);
            this.Controls.Add(this.txtXml);
            this.Name = "Chonxml";
            this.Text = "Chonxml";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Button btnXml;
        internal System.Windows.Forms.TextBox txtXml;
    }
}