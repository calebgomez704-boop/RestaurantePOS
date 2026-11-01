namespace RestaurantePOS.Formularios
{
    partial class FormCaja
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
            this.pnlTop = new System.Windows.Forms.Panel();
            this.btnRefrescar = new System.Windows.Forms.Button();
            this.lblSubtitulo = new System.Windows.Forms.Label();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.splMain = new System.Windows.Forms.SplitContainer();
            this.gbPedidos = new System.Windows.Forms.GroupBox();
            this.dgvPedidosPendientes = new System.Windows.Forms.DataGridView();
            this.gbDetalle = new System.Windows.Forms.GroupBox();
            this.dgvDetalles = new System.Windows.Forms.DataGridView();
            this.lblPedidoInfo = new System.Windows.Forms.Label();
            this.pnlCobro = new System.Windows.Forms.Panel();
            this.btnDescargarPDF = new System.Windows.Forms.Button();
            this.btnCobrar = new System.Windows.Forms.Button();
            this.pnlEfectivo = new System.Windows.Forms.Panel();
            this.lblCambio = new System.Windows.Forms.Label();
            this.txtMontoRecibido = new System.Windows.Forms.TextBox();
            this.lblRecibido = new System.Windows.Forms.Label();
            this.cmbMetodoPago = new System.Windows.Forms.ComboBox();
            this.lblMetodo = new System.Windows.Forms.Label();
            this.lblTotalPagar = new System.Windows.Forms.Label();
            this.pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splMain)).BeginInit();
            this.splMain.Panel1.SuspendLayout();
            this.splMain.Panel2.SuspendLayout();
            this.splMain.SuspendLayout();
            this.gbPedidos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPedidosPendientes)).BeginInit();
            this.gbDetalle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetalles)).BeginInit();
            this.pnlCobro.SuspendLayout();
            this.pnlEfectivo.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.White;
            this.pnlTop.Controls.Add(this.btnRefrescar);
            this.pnlTop.Controls.Add(this.lblSubtitulo);
            this.pnlTop.Controls.Add(this.lblTitulo);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Padding = new System.Windows.Forms.Padding(12, 6, 12, 6);
            this.pnlTop.Size = new System.Drawing.Size(1024, 52);
            this.pnlTop.TabIndex = 0;
            // 
            // btnRefrescar
            // 
            this.btnRefrescar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefrescar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.btnRefrescar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRefrescar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefrescar.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnRefrescar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(175)))), ((int)(((byte)(55)))));
            this.btnRefrescar.Location = new System.Drawing.Point(865, 8);
            this.btnRefrescar.Name = "btnRefrescar";
            this.btnRefrescar.Size = new System.Drawing.Size(145, 34);
            this.btnRefrescar.TabIndex = 2;
            this.btnRefrescar.Text = "Refrescar";
            this.btnRefrescar.UseVisualStyleBackColor = false;
            this.btnRefrescar.Click += new System.EventHandler(this.btnRefrescar_Click);
            // 
            // lblSubtitulo
            // 
            this.lblSubtitulo.AutoSize = true;
            this.lblSubtitulo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSubtitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.lblSubtitulo.Location = new System.Drawing.Point(15, 30);
            this.lblSubtitulo.Name = "lblSubtitulo";
            this.lblSubtitulo.Size = new System.Drawing.Size(243, 15);
            this.lblSubtitulo.TabIndex = 1;
            this.lblSubtitulo.Text = "Selecciona un pedido pendiente para cobrar";
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.lblTitulo.Location = new System.Drawing.Point(12, 5);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(232, 25);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Caja y Facturación (POS)";
            this.lblTitulo.Click += new System.EventHandler(this.lblTitulo_Click);
            // 
            // splMain
            // 
            this.splMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splMain.Location = new System.Drawing.Point(0, 52);
            this.splMain.Name = "splMain";
            // 
            // splMain.Panel1
            // 
            this.splMain.Panel1.Controls.Add(this.gbPedidos);
            this.splMain.Panel1.Padding = new System.Windows.Forms.Padding(8);
            // 
            // splMain.Panel2
            // 
            this.splMain.Panel2.Controls.Add(this.gbDetalle);
            this.splMain.Panel2.Controls.Add(this.pnlCobro);
            this.splMain.Panel2.Padding = new System.Windows.Forms.Padding(8);
            this.splMain.Size = new System.Drawing.Size(1024, 548);
            this.splMain.SplitterDistance = 475;
            this.splMain.TabIndex = 1;
            // 
            // gbPedidos
            // 
            this.gbPedidos.BackColor = System.Drawing.Color.White;
            this.gbPedidos.Controls.Add(this.dgvPedidosPendientes);
            this.gbPedidos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbPedidos.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.gbPedidos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.gbPedidos.Location = new System.Drawing.Point(8, 8);
            this.gbPedidos.Name = "gbPedidos";
            this.gbPedidos.Padding = new System.Windows.Forms.Padding(8);
            this.gbPedidos.Size = new System.Drawing.Size(459, 532);
            this.gbPedidos.TabIndex = 0;
            this.gbPedidos.TabStop = false;
            this.gbPedidos.Text = "Comandas / Pedidos Pendientes de Pago";
            // 
            // dgvPedidosPendientes
            // 
            this.dgvPedidosPendientes.AllowUserToAddRows = false;
            this.dgvPedidosPendientes.AllowUserToDeleteRows = false;
            this.dgvPedidosPendientes.BackgroundColor = System.Drawing.Color.White;
            this.dgvPedidosPendientes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPedidosPendientes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPedidosPendientes.Location = new System.Drawing.Point(8, 25);
            this.dgvPedidosPendientes.MultiSelect = false;
            this.dgvPedidosPendientes.Name = "dgvPedidosPendientes";
            this.dgvPedidosPendientes.ReadOnly = true;
            this.dgvPedidosPendientes.RowHeadersVisible = false;
            this.dgvPedidosPendientes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPedidosPendientes.Size = new System.Drawing.Size(443, 499);
            this.dgvPedidosPendientes.TabIndex = 0;
            this.dgvPedidosPendientes.SelectionChanged += new System.EventHandler(this.dgvPedidosPendientes_SelectionChanged);
            // 
            // gbDetalle
            // 
            this.gbDetalle.BackColor = System.Drawing.Color.White;
            this.gbDetalle.Controls.Add(this.dgvDetalles);
            this.gbDetalle.Controls.Add(this.lblPedidoInfo);
            this.gbDetalle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbDetalle.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.gbDetalle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.gbDetalle.Location = new System.Drawing.Point(8, 8);
            this.gbDetalle.Name = "gbDetalle";
            this.gbDetalle.Padding = new System.Windows.Forms.Padding(8);
            this.gbDetalle.Size = new System.Drawing.Size(529, 387);
            this.gbDetalle.TabIndex = 0;
            this.gbDetalle.TabStop = false;
            this.gbDetalle.Text = "Desglose de la Comanda";
            // 
            // dgvDetalles
            // 
            this.dgvDetalles.AllowUserToAddRows = false;
            this.dgvDetalles.AllowUserToDeleteRows = false;
            this.dgvDetalles.BackgroundColor = System.Drawing.Color.White;
            this.dgvDetalles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDetalles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDetalles.Location = new System.Drawing.Point(8, 45);
            this.dgvDetalles.MultiSelect = false;
            this.dgvDetalles.Name = "dgvDetalles";
            this.dgvDetalles.ReadOnly = true;
            this.dgvDetalles.RowHeadersVisible = false;
            this.dgvDetalles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDetalles.Size = new System.Drawing.Size(513, 334);
            this.dgvDetalles.TabIndex = 1;
            // 
            // lblPedidoInfo
            // 
            this.lblPedidoInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblPedidoInfo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblPedidoInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.lblPedidoInfo.Location = new System.Drawing.Point(8, 25);
            this.lblPedidoInfo.Name = "lblPedidoInfo";
            this.lblPedidoInfo.Size = new System.Drawing.Size(513, 20);
            this.lblPedidoInfo.TabIndex = 0;
            this.lblPedidoInfo.Text = "Ningún pedido seleccionado";
            // 
            // pnlCobro
            // 
            this.pnlCobro.BackColor = System.Drawing.Color.White;
            this.pnlCobro.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCobro.Controls.Add(this.btnDescargarPDF);
            this.pnlCobro.Controls.Add(this.btnCobrar);
            this.pnlCobro.Controls.Add(this.pnlEfectivo);
            this.pnlCobro.Controls.Add(this.cmbMetodoPago);
            this.pnlCobro.Controls.Add(this.lblMetodo);
            this.pnlCobro.Controls.Add(this.lblTotalPagar);
            this.pnlCobro.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlCobro.Location = new System.Drawing.Point(8, 395);
            this.pnlCobro.Name = "pnlCobro";
            this.pnlCobro.Padding = new System.Windows.Forms.Padding(6);
            this.pnlCobro.Size = new System.Drawing.Size(529, 145);
            this.pnlCobro.TabIndex = 1;
            // 
            // btnDescargarPDF
            // 
            this.btnDescargarPDF.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDescargarPDF.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(117)))), ((int)(((byte)(89)))));
            this.btnDescargarPDF.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDescargarPDF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDescargarPDF.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnDescargarPDF.ForeColor = System.Drawing.Color.White;
            this.btnDescargarPDF.Location = new System.Drawing.Point(308, 86);
            this.btnDescargarPDF.Name = "btnDescargarPDF";
            this.btnDescargarPDF.Size = new System.Drawing.Size(208, 48);
            this.btnDescargarPDF.TabIndex = 5;
            this.btnDescargarPDF.Text = "Descargar PDF";
            this.btnDescargarPDF.UseVisualStyleBackColor = false;
            this.btnDescargarPDF.Click += new System.EventHandler(this.btnDescargarPDF_Click);
            // 
            // btnCobrar
            // 
            this.btnCobrar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCobrar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.btnCobrar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCobrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCobrar.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnCobrar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(175)))), ((int)(((byte)(55)))));
            this.btnCobrar.Location = new System.Drawing.Point(8, 86);
            this.btnCobrar.Name = "btnCobrar";
            this.btnCobrar.Size = new System.Drawing.Size(292, 48);
            this.btnCobrar.TabIndex = 4;
            this.btnCobrar.Text = "Confirmar Cobro y Facturar";
            this.btnCobrar.UseVisualStyleBackColor = false;
            this.btnCobrar.Click += new System.EventHandler(this.btnCobrar_Click);
            // 
            // pnlEfectivo
            // 
            this.pnlEfectivo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlEfectivo.Controls.Add(this.lblCambio);
            this.pnlEfectivo.Controls.Add(this.txtMontoRecibido);
            this.pnlEfectivo.Controls.Add(this.lblRecibido);
            this.pnlEfectivo.Location = new System.Drawing.Point(8, 42);
            this.pnlEfectivo.Name = "pnlEfectivo";
            this.pnlEfectivo.Size = new System.Drawing.Size(510, 36);
            this.pnlEfectivo.TabIndex = 3;
            // 
            // lblCambio
            // 
            this.lblCambio.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCambio.AutoSize = true;
            this.lblCambio.Font = new System.Drawing.Font("Segoe UI", 11.5F, System.Drawing.FontStyle.Bold);
            this.lblCambio.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
            this.lblCambio.Location = new System.Drawing.Point(260, 7);
            this.lblCambio.Name = "lblCambio";
            this.lblCambio.Size = new System.Drawing.Size(128, 21);
            this.lblCambio.TabIndex = 2;
            this.lblCambio.Text = "Cambio: C$ 0.00";
            // 
            // txtMontoRecibido
            // 
            this.txtMontoRecibido.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.txtMontoRecibido.Location = new System.Drawing.Point(125, 4);
            this.txtMontoRecibido.Name = "txtMontoRecibido";
            this.txtMontoRecibido.Size = new System.Drawing.Size(120, 27);
            this.txtMontoRecibido.TabIndex = 1;
            this.txtMontoRecibido.TextChanged += new System.EventHandler(this.txtMontoRecibido_TextChanged);
            // 
            // lblRecibido
            // 
            this.lblRecibido.AutoSize = true;
            this.lblRecibido.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblRecibido.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.lblRecibido.Location = new System.Drawing.Point(2, 8);
            this.lblRecibido.Name = "lblRecibido";
            this.lblRecibido.Size = new System.Drawing.Size(122, 17);
            this.lblRecibido.TabIndex = 0;
            this.lblRecibido.Text = "Monto Recibido C$:";
            // 
            // cmbMetodoPago
            // 
            this.cmbMetodoPago.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbMetodoPago.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMetodoPago.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbMetodoPago.FormattingEnabled = true;
            this.cmbMetodoPago.Location = new System.Drawing.Point(345, 9);
            this.cmbMetodoPago.Name = "cmbMetodoPago";
            this.cmbMetodoPago.Size = new System.Drawing.Size(172, 25);
            this.cmbMetodoPago.TabIndex = 2;
            this.cmbMetodoPago.SelectedIndexChanged += new System.EventHandler(this.cmbMetodoPago_SelectedIndexChanged);
            // 
            // lblMetodo
            // 
            this.lblMetodo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMetodo.AutoSize = true;
            this.lblMetodo.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblMetodo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.lblMetodo.Location = new System.Drawing.Point(280, 12);
            this.lblMetodo.Name = "lblMetodo";
            this.lblMetodo.Size = new System.Drawing.Size(60, 17);
            this.lblMetodo.TabIndex = 1;
            this.lblMetodo.Text = "Método:";
            // 
            // lblTotalPagar
            // 
            this.lblTotalPagar.AutoSize = true;
            this.lblTotalPagar.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTotalPagar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.lblTotalPagar.Location = new System.Drawing.Point(8, 7);
            this.lblTotalPagar.Name = "lblTotalPagar";
            this.lblTotalPagar.Size = new System.Drawing.Size(203, 25);
            this.lblTotalPagar.TabIndex = 0;
            this.lblTotalPagar.Text = "Total a Pagar: C$ 0.00";
            // 
            // FormCaja
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(230)))), ((int)(((byte)(211)))));
            this.ClientSize = new System.Drawing.Size(1024, 600);
            this.Controls.Add(this.splMain);
            this.Controls.Add(this.pnlTop);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.Name = "FormCaja";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Caja y Facturación";
            this.Load += new System.EventHandler(this.FormCaja_Load);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.splMain.Panel1.ResumeLayout(false);
            this.splMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splMain)).EndInit();
            this.splMain.ResumeLayout(false);
            this.gbPedidos.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPedidosPendientes)).EndInit();
            this.gbDetalle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetalles)).EndInit();
            this.pnlCobro.ResumeLayout(false);
            this.pnlCobro.PerformLayout();
            this.pnlEfectivo.ResumeLayout(false);
            this.pnlEfectivo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Button btnRefrescar;
        private System.Windows.Forms.Label lblSubtitulo;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.SplitContainer splMain;
        private System.Windows.Forms.GroupBox gbPedidos;
        private System.Windows.Forms.DataGridView dgvPedidosPendientes;
        private System.Windows.Forms.GroupBox gbDetalle;
        private System.Windows.Forms.DataGridView dgvDetalles;
        private System.Windows.Forms.Label lblPedidoInfo;
        private System.Windows.Forms.Panel pnlCobro;
        private System.Windows.Forms.Button btnCobrar;
        private System.Windows.Forms.Button btnDescargarPDF;
        private System.Windows.Forms.Panel pnlEfectivo;
        private System.Windows.Forms.Label lblCambio;
        private System.Windows.Forms.TextBox txtMontoRecibido;
        private System.Windows.Forms.Label lblRecibido;
        private System.Windows.Forms.ComboBox cmbMetodoPago;
        private System.Windows.Forms.Label lblMetodo;
        private System.Windows.Forms.Label lblTotalPagar;
    }
}
