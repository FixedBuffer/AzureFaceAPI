namespace PostAzureFaceAPI
{
    partial class MainForm
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
            this.btn_Train = new System.Windows.Forms.Button();
            this.btn_Find = new System.Windows.Forms.Button();
            this.pct_Imagen = new System.Windows.Forms.PictureBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lbl_Status = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pct_Imagen)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_Train
            // 
            this.btn_Train.Location = new System.Drawing.Point(12, 12);
            this.btn_Train.Name = "btn_Train";
            this.btn_Train.Size = new System.Drawing.Size(223, 43);
            this.btn_Train.TabIndex = 0;
            this.btn_Train.Text = "Entrenar grupo";
            this.btn_Train.UseVisualStyleBackColor = true;
            this.btn_Train.Click += new System.EventHandler(this.btn_Train_Click);
            // 
            // btn_Find
            // 
            this.btn_Find.Enabled = false;
            this.btn_Find.Location = new System.Drawing.Point(320, 12);
            this.btn_Find.Name = "btn_Find";
            this.btn_Find.Size = new System.Drawing.Size(223, 43);
            this.btn_Find.TabIndex = 1;
            this.btn_Find.Text = "Buscar";
            this.btn_Find.UseVisualStyleBackColor = true;
            // 
            // pct_Imagen
            // 
            this.pct_Imagen.Location = new System.Drawing.Point(12, 71);
            this.pct_Imagen.Name = "pct_Imagen";
            this.pct_Imagen.Size = new System.Drawing.Size(531, 370);
            this.pct_Imagen.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pct_Imagen.TabIndex = 2;
            this.pct_Imagen.TabStop = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lbl_Status});
            this.statusStrip1.Location = new System.Drawing.Point(0, 451);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(556, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lbl_Status
            // 
            this.lbl_Status.Name = "lbl_Status";
            this.lbl_Status.Size = new System.Drawing.Size(0, 17);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(556, 473);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.pct_Imagen);
            this.Controls.Add(this.btn_Find);
            this.Controls.Add(this.btn_Train);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Post Azure FaceAPI";
            ((System.ComponentModel.ISupportInitialize)(this.pct_Imagen)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_Train;
        private System.Windows.Forms.Button btn_Find;
        private System.Windows.Forms.PictureBox pct_Imagen;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lbl_Status;
    }
}

