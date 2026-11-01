namespace RestaurantePOS.Formularios
{
    partial class FormCambiarClave
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.pnlPrincipal = new System.Windows.Forms.Panel();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblDescripcion = new System.Windows.Forms.Label();
            this.lblNuevaClave = new System.Windows.Forms.Label();
            this.txtNuevaClave = new System.Windows.Forms.TextBox();
            this.lblConfirmarClave = new System.Windows.Forms.Label();
            this.txtConfirmarClave = new System.Windows.Forms.TextBox();
            this.btnAceptar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.pnlPrincipal.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlPrincipal
            // 
            this.pnlPrincipal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(230)))), ((int)(((byte)(211)))));
            this.pnlPrincipal.Controls.Add(this.btnCancelar);
            this.pnlPrincipal.Controls.Add(this.btnAceptar);
            this.pnlPrincipal.Controls.Add(this.txtConfirmarClave);
            this.pnlPrincipal.Controls.Add(this.lblConfirmarClave);
            this.pnlPrincipal.Controls.Add(this.txtNuevaClave);
            this.pnlPrincipal.Controls.Add(this.lblNuevaClave);
            this.pnlPrincipal.Controls.Add(this.lblDescripcion);
            this.pnlPrincipal.Controls.Add(this.lblTitulo);
            this.pnlPrincipal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPrincipal.Location = new System.Drawing.Point(0, 0);
            this.pnlPrincipal.Name = "pnlPrincipal";
            this.pnlPrincipal.Padding = new System.Windows.Forms.Padding(25);
            this.pnlPrincipal.Size = new System.Drawing.Size(460, 320);
            this.pnlPrincipal.TabIndex = 0;
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.lblTitulo.Location = new System.Drawing.Point(25, 20);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(306, 25);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Cambio Obligatorio de Contraseña";
            // 
            // lblDescripcion
            // 
            this.lblDescripcion.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblDescripcion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.lblDescripcion.Location = new System.Drawing.Point(26, 55);
            this.lblDescripcion.Name = "lblDescripcion";
            this.lblDescripcion.Size = new System.Drawing.Size(410, 40);
            this.lblDescripcion.TabIndex = 1;
            this.lblDescripcion.Text = "Por seguridad institucional, debe establecer una contraseña personal de al menos " +
    "6 caracteres antes de continuar.";
            // 
            // lblNuevaClave
            // 
            this.lblNuevaClave.AutoSize = true;
            this.lblNuevaClave.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblNuevaClave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.lblNuevaClave.Location = new System.Drawing.Point(26, 105);
            this.lblNuevaClave.Name = "lblNuevaClave";
            this.lblNuevaClave.Size = new System.Drawing.Size(130, 19);
            this.lblNuevaClave.TabIndex = 2;
            this.lblNuevaClave.Text = "Nueva Contraseña:";
            // 
            // txtNuevaClave
            // 
            this.txtNuevaClave.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtNuevaClave.Location = new System.Drawing.Point(30, 130);
            this.txtNuevaClave.Name = "txtNuevaClave";
            this.txtNuevaClave.Size = new System.Drawing.Size(400, 27);
            this.txtNuevaClave.TabIndex = 3;
            this.txtNuevaClave.UseSystemPasswordChar = true;
            // 
            // lblConfirmarClave
            // 
            this.lblConfirmarClave.AutoSize = true;
            this.lblConfirmarClave.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblConfirmarClave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.lblConfirmarClave.Location = new System.Drawing.Point(26, 170);
            this.lblConfirmarClave.Name = "lblConfirmarClave";
            this.lblConfirmarClave.Size = new System.Drawing.Size(155, 19);
            this.lblConfirmarClave.TabIndex = 4;
            this.lblConfirmarClave.Text = "Confirmar Contraseña:";
            // 
            // txtConfirmarClave
            // 
            this.txtConfirmarClave.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtConfirmarClave.Location = new System.Drawing.Point(30, 195);
            this.txtConfirmarClave.Name = "txtConfirmarClave";
            this.txtConfirmarClave.Size = new System.Drawing.Size(400, 27);
            this.txtConfirmarClave.TabIndex = 5;
            this.txtConfirmarClave.UseSystemPasswordChar = true;
            // 
            // btnAceptar
            // 
            this.btnAceptar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.btnAceptar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAceptar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAceptar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnAceptar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(175)))), ((int)(((byte)(55)))));
            this.btnAceptar.Location = new System.Drawing.Point(120, 245);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(180, 42);
            this.btnAceptar.TabIndex = 6;
            this.btnAceptar.Text = "Guardar y Entrar";
            this.btnAceptar.UseVisualStyleBackColor = false;
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnCancelar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelar.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnCancelar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.btnCancelar.Location = new System.Drawing.Point(310, 245);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(120, 42);
            this.btnCancelar.TabIndex = 7;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = false;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // FormCambiarClave
            // 
            this.AcceptButton = this.btnAceptar;
            this.CancelButton = this.btnCancelar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(230)))), ((int)(((byte)(211)))));
            this.ClientSize = new System.Drawing.Size(460, 320);
            this.Controls.Add(this.pnlPrincipal);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCambiarClave";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RestaurantePOS - Seguridad";
            this.pnlPrincipal.ResumeLayout(false);
            this.pnlPrincipal.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlPrincipal;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblDescripcion;
        private System.Windows.Forms.Label lblNuevaClave;
        private System.Windows.Forms.TextBox txtNuevaClave;
        private System.Windows.Forms.Label lblConfirmarClave;
        private System.Windows.Forms.TextBox txtConfirmarClave;
        private System.Windows.Forms.Button btnAceptar;
        private System.Windows.Forms.Button btnCancelar;
    }
}
