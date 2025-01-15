namespace TPVApp
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.erabiltzaileaTextBox = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.pasahitzaTextBox = new System.Windows.Forms.TextBox();
            this.saioaHasiButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // erabiltzaileaTextBox
            // 
            this.erabiltzaileaTextBox.Location = new System.Drawing.Point(1108, 641);
            this.erabiltzaileaTextBox.Name = "erabiltzaileaTextBox";
            this.erabiltzaileaTextBox.Size = new System.Drawing.Size(247, 31);
            this.erabiltzaileaTextBox.TabIndex = 0;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // pasahitzaTextBox
            // 
            this.pasahitzaTextBox.Location = new System.Drawing.Point(1108, 720);
            this.pasahitzaTextBox.Name = "pasahitzaTextBox";
            this.pasahitzaTextBox.Size = new System.Drawing.Size(247, 31);
            this.pasahitzaTextBox.TabIndex = 2;
            // 
            // saioaHasiButton
            // 
            this.saioaHasiButton.Location = new System.Drawing.Point(1155, 810);
            this.saioaHasiButton.Name = "saioaHasiButton";
            this.saioaHasiButton.Size = new System.Drawing.Size(144, 49);
            this.saioaHasiButton.TabIndex = 3;
            this.saioaHasiButton.Text = "Saioa hasi";
            this.saioaHasiButton.UseVisualStyleBackColor = true;
            this.saioaHasiButton.Click += new System.EventHandler(this.saioaHasiButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2278, 1137);
            this.Controls.Add(this.saioaHasiButton);
            this.Controls.Add(this.pasahitzaTextBox);
            this.Controls.Add(this.erabiltzaileaTextBox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox erabiltzaileaTextBox;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.TextBox pasahitzaTextBox;
        private System.Windows.Forms.Button saioaHasiButton;
    }
}

