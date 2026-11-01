namespace RestaurantePOS
{
    partial class FormPedidos
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblCliente = new System.Windows.Forms.Label();
            this.lblMeseroAtiende = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.cmbMesa = new System.Windows.Forms.ComboBox();
            this.cmbProducto = new System.Windows.Forms.ComboBox();
            this.cmbCliente = new System.Windows.Forms.ComboBox();
            this.numCantidad = new System.Windows.Forms.NumericUpDown();
            this.btnGuardarPedido = new System.Windows.Forms.Button();
            this.btnAgregarDetalle = new System.Windows.Forms.Button();
            this.dgvDetalle = new System.Windows.Forms.DataGridView();
            this.pnlContent = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.numCantidad)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetalle)).BeginInit();
            this.pnlContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlContent
            // 
            this.pnlContent.AutoScroll = true;
            this.pnlContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(230)))), ((int)(((byte)(211)))));
            this.pnlContent.Controls.Add(this.lblMeseroAtiende);
            this.pnlContent.Controls.Add(this.cmbCliente);
            this.pnlContent.Controls.Add(this.lblCliente);
            this.pnlContent.Controls.Add(this.label1);
            this.pnlContent.Controls.Add(this.cmbMesa);
            this.pnlContent.Controls.Add(this.label2);
            this.pnlContent.Controls.Add(this.cmbProducto);
            this.pnlContent.Controls.Add(this.label3);
            this.pnlContent.Controls.Add(this.numCantidad);
            this.pnlContent.Controls.Add(this.btnAgregarDetalle);
            this.pnlContent.Controls.Add(this.dgvDetalle);
            this.pnlContent.Controls.Add(this.lblTotal);
            this.pnlContent.Controls.Add(this.btnGuardarPedido);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(0, 0);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(1024, 600);
            this.pnlContent.TabIndex = 20;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.label1.Location = new System.Drawing.Point(20, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "Mesa";
            // 
            // cmbMesa
            // 
            this.cmbMesa.BackColor = System.Drawing.Color.White;
            this.cmbMesa.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMesa.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbMesa.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.cmbMesa.FormattingEnabled = true;
            this.cmbMesa.Location = new System.Drawing.Point(20, 42);
            this.cmbMesa.Name = "cmbMesa";
            this.cmbMesa.Size = new System.Drawing.Size(240, 25);
            this.cmbMesa.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.label2.Location = new System.Drawing.Point(290, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 19);
            this.label2.TabIndex = 2;
            this.label2.Text = "Productos";
            // 
            // cmbProducto
            // 
            this.cmbProducto.BackColor = System.Drawing.Color.White;
            this.cmbProducto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProducto.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbProducto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.cmbProducto.FormattingEnabled = true;
            this.cmbProducto.Location = new System.Drawing.Point(290, 42);
            this.cmbProducto.Name = "cmbProducto";
            this.cmbProducto.Size = new System.Drawing.Size(280, 25);
            this.cmbProducto.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.label3.Location = new System.Drawing.Point(600, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 19);
            this.label3.TabIndex = 4;
            this.label3.Text = "Cantidad";
            // 
            // numCantidad
            // 
            this.numCantidad.BackColor = System.Drawing.Color.White;
            this.numCantidad.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.numCantidad.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.numCantidad.Location = new System.Drawing.Point(600, 42);
            this.numCantidad.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numCantidad.Name = "numCantidad";
            this.numCantidad.Size = new System.Drawing.Size(80, 25);
            this.numCantidad.TabIndex = 5;
            this.numCantidad.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btnAgregarDetalle
            // 
            this.btnAgregarDetalle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.btnAgregarDetalle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAgregarDetalle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAgregarDetalle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnAgregarDetalle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(175)))), ((int)(((byte)(55)))));
            this.btnAgregarDetalle.Location = new System.Drawing.Point(710, 36);
            this.btnAgregarDetalle.Name = "btnAgregarDetalle";
            this.btnAgregarDetalle.Size = new System.Drawing.Size(210, 38);
            this.btnAgregarDetalle.TabIndex = 6;
            this.btnAgregarDetalle.Text = "Agregar a la comanda";
            this.btnAgregarDetalle.UseVisualStyleBackColor = false;
            this.btnAgregarDetalle.Click += new System.EventHandler(this.btnAgregarDetalle_Click);
            // 
            // lblCliente
            // 
            this.lblCliente.AutoSize = true;
            this.lblCliente.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblCliente.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.lblCliente.Location = new System.Drawing.Point(20, 80);
            this.lblCliente.Name = "lblCliente";
            this.lblCliente.Size = new System.Drawing.Size(55, 19);
            this.lblCliente.TabIndex = 7;
            this.lblCliente.Text = "Cliente";
            // 
            // cmbCliente
            // 
            this.cmbCliente.BackColor = System.Drawing.Color.White;
            this.cmbCliente.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCliente.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbCliente.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.cmbCliente.FormattingEnabled = true;
            this.cmbCliente.Location = new System.Drawing.Point(20, 102);
            this.cmbCliente.Name = "cmbCliente";
            this.cmbCliente.Size = new System.Drawing.Size(240, 25);
            this.cmbCliente.TabIndex = 8;
            // 
            // lblMeseroAtiende
            // 
            this.lblMeseroAtiende.AutoSize = true;
            this.lblMeseroAtiende.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblMeseroAtiende.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.lblMeseroAtiende.Location = new System.Drawing.Point(290, 104);
            this.lblMeseroAtiende.Name = "lblMeseroAtiende";
            this.lblMeseroAtiende.Size = new System.Drawing.Size(225, 20);
            this.lblMeseroAtiende.TabIndex = 9;
            this.lblMeseroAtiende.Text = "Atendido por: [Nombre Mesero]";
            // 
            // dgvDetalle
            // 
            this.dgvDetalle.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvDetalle.BackgroundColor = System.Drawing.Color.White;
            this.dgvDetalle.Location = new System.Drawing.Point(20, 140);
            this.dgvDetalle.Name = "dgvDetalle";
            this.dgvDetalle.Size = new System.Drawing.Size(984, 365);
            this.dgvDetalle.TabIndex = 10;
            // 
            // lblTotal
            // 
            this.lblTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.lblTotal.Location = new System.Drawing.Point(460, 528);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(260, 25);
            this.lblTotal.TabIndex = 11;
            this.lblTotal.Text = "Total a Pagar: C$ 0.00";
            // 
            // btnGuardarPedido
            // 
            this.btnGuardarPedido.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGuardarPedido.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.btnGuardarPedido.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGuardarPedido.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGuardarPedido.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnGuardarPedido.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(175)))), ((int)(((byte)(55)))));
            this.btnGuardarPedido.Location = new System.Drawing.Point(740, 518);
            this.btnGuardarPedido.Name = "btnGuardarPedido";
            this.btnGuardarPedido.Size = new System.Drawing.Size(264, 50);
            this.btnGuardarPedido.TabIndex = 12;
            this.btnGuardarPedido.Text = "Guardar y Enviar Pedido";
            this.btnGuardarPedido.UseVisualStyleBackColor = false;
            this.btnGuardarPedido.Click += new System.EventHandler(this.btnGuardarPedido_Click);
            // 
            // FormPedidos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.ClientSize = new System.Drawing.Size(1024, 600);
            this.Controls.Add(this.pnlContent);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.Name = "FormPedidos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RestaurantePOS - Gestión de Pedidos";
            this.Load += new System.EventHandler(this.FormPedidos_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numCantidad)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetalle)).EndInit();
            this.pnlContent.ResumeLayout(false);
            this.pnlContent.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblCliente;
        private System.Windows.Forms.Label lblMeseroAtiende;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.ComboBox cmbMesa;
        private System.Windows.Forms.ComboBox cmbProducto;
        private System.Windows.Forms.ComboBox cmbCliente;
        private System.Windows.Forms.NumericUpDown numCantidad;
        private System.Windows.Forms.Button btnGuardarPedido;
        private System.Windows.Forms.Button btnAgregarDetalle;
        private System.Windows.Forms.DataGridView dgvDetalle;
        private System.Windows.Forms.Panel pnlContent;
    }
}
