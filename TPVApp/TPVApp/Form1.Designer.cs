﻿namespace TPVApp
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
            this.erabiltzaileaTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.erabiltzaileaTextBox.Location = new System.Drawing.Point(761, 429);
            this.erabiltzaileaTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.erabiltzaileaTextBox.Name = "erabiltzaileaTextBox";
            this.erabiltzaileaTextBox.Size = new System.Drawing.Size(166, 22);
            this.erabiltzaileaTextBox.TabIndex = 0;
            this.erabiltzaileaTextBox.Text = "Erabiltzailea";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // pasahitzaTextBox
            // 
            this.pasahitzaTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.pasahitzaTextBox.Location = new System.Drawing.Point(761, 479);
            this.pasahitzaTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.pasahitzaTextBox.Name = "pasahitzaTextBox";
            this.pasahitzaTextBox.Size = new System.Drawing.Size(166, 22);
            this.pasahitzaTextBox.TabIndex = 2;
            this.pasahitzaTextBox.Text = "Pasahitza";
            this.pasahitzaTextBox.TextChanged += new System.EventHandler(this.pasahitzaTextBox_TextChanged);
            // 
            // saioaHasiButton
            // 
            this.saioaHasiButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(102)))), ((int)(((byte)(0)))));
            this.saioaHasiButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.saioaHasiButton.Location = new System.Drawing.Point(761, 537);
            this.saioaHasiButton.Margin = new System.Windows.Forms.Padding(2);
            this.saioaHasiButton.Name = "saioaHasiButton";
            this.saioaHasiButton.Size = new System.Drawing.Size(166, 31);
            this.saioaHasiButton.TabIndex = 3;
            this.saioaHasiButton.Text = "Saioa hasi";
            this.saioaHasiButton.UseVisualStyleBackColor = false;
            this.saioaHasiButton.Click += new System.EventHandler(this.saioaHasiButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(214)))), ((int)(((byte)(203)))));
            this.ClientSize = new System.Drawing.Size(1393, 703);
            this.Controls.Add(this.saioaHasiButton);
            this.Controls.Add(this.pasahitzaTextBox);
            this.Controls.Add(this.erabiltzaileaTextBox);
            this.Margin = new System.Windows.Forms.Padding(2);
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

