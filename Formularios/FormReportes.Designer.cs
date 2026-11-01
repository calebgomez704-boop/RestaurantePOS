namespace RestaurantePOS.Modelos
{
    partial class FormReportes
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.btnFacturaPDF = new System.Windows.Forms.Button();
            this.btnExportarExcel = new System.Windows.Forms.Button();
            this.btnExportarPDF = new System.Windows.Forms.Button();
            this.btnModoPedidos = new System.Windows.Forms.Button();
            this.btnModoDetallado = new System.Windows.Forms.Button();
            this.btnFiltrar = new System.Windows.Forms.Button();
            this.dtpHasta = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpDesde = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblTotales = new System.Windows.Forms.Label();
            this.dgvReportes = new System.Windows.Forms.DataGridView();
            this.pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReportes)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.White;
            this.pnlTop.Controls.Add(this.btnFacturaPDF);
            this.pnlTop.Controls.Add(this.btnExportarExcel);
            this.pnlTop.Controls.Add(this.btnExportarPDF);
            this.pnlTop.Controls.Add(this.btnModoPedidos);
            this.pnlTop.Controls.Add(this.btnModoDetallado);
            this.pnlTop.Controls.Add(this.btnFiltrar);
            this.pnlTop.Controls.Add(this.dtpHasta);
            this.pnlTop.Controls.Add(this.label2);
            this.pnlTop.Controls.Add(this.dtpDesde);
            this.pnlTop.Controls.Add(this.label1);
            this.pnlTop.Controls.Add(this.lblTitulo);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1200, 68);
            this.pnlTop.TabIndex = 0;
            // 
            // btnFacturaPDF
            // 
            this.btnFacturaPDF.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(105)))), ((int)(((byte)(145)))));
            this.btnFacturaPDF.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFacturaPDF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFacturaPDF.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnFacturaPDF.ForeColor = System.Drawing.Color.White;
            this.btnFacturaPDF.Location = new System.Drawing.Point(1065, 18);
            this.btnFacturaPDF.Name = "btnFacturaPDF";
            this.btnFacturaPDF.Size = new System.Drawing.Size(110, 32);
            this.btnFacturaPDF.TabIndex = 10;
            this.btnFacturaPDF.Text = "Factura PDF";
            this.btnFacturaPDF.UseVisualStyleBackColor = false;
            this.btnFacturaPDF.Click += new System.EventHandler(this.btnFacturaPDF_Click);
            // 
            // btnExportarExcel
            // 
            this.btnExportarExcel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(117)))), ((int)(((byte)(89)))));
            this.btnExportarExcel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExportarExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportarExcel.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnExportarExcel.ForeColor = System.Drawing.Color.White;
            this.btnExportarExcel.Location = new System.Drawing.Point(942, 18);
            this.btnExportarExcel.Name = "btnExportarExcel";
            this.btnExportarExcel.Size = new System.Drawing.Size(115, 32);
            this.btnExportarExcel.TabIndex = 9;
            this.btnExportarExcel.Text = "Exportar Excel";
            this.btnExportarExcel.UseVisualStyleBackColor = false;
            this.btnExportarExcel.Click += new System.EventHandler(this.btnExportarExcel_Click);
            // 
            // btnExportarPDF
            // 
            this.btnExportarPDF.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.btnExportarPDF.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExportarPDF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportarPDF.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnExportarPDF.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(175)))), ((int)(((byte)(55)))));
            this.btnExportarPDF.Location = new System.Drawing.Point(819, 18);
            this.btnExportarPDF.Name = "btnExportarPDF";
            this.btnExportarPDF.Size = new System.Drawing.Size(115, 32);
            this.btnExportarPDF.TabIndex = 8;
            this.btnExportarPDF.Text = "Exportar PDF";
            this.btnExportarPDF.UseVisualStyleBackColor = false;
            this.btnExportarPDF.Click += new System.EventHandler(this.btnExportarPDF_Click);
            // 
            // btnModoPedidos
            // 
            this.btnModoPedidos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(230)))), ((int)(((byte)(211)))));
            this.btnModoPedidos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnModoPedidos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnModoPedidos.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnModoPedidos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.btnModoPedidos.Location = new System.Drawing.Point(706, 18);
            this.btnModoPedidos.Name = "btnModoPedidos";
            this.btnModoPedidos.Size = new System.Drawing.Size(105, 32);
            this.btnModoPedidos.TabIndex = 7;
            this.btnModoPedidos.Text = "Ver Pedidos";
            this.btnModoPedidos.UseVisualStyleBackColor = false;
            this.btnModoPedidos.Click += new System.EventHandler(this.btnModoPedidos_Click);
            // 
            // btnModoDetallado
            // 
            this.btnModoDetallado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.btnModoDetallado.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnModoDetallado.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnModoDetallado.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnModoDetallado.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(175)))), ((int)(((byte)(55)))));
            this.btnModoDetallado.Location = new System.Drawing.Point(593, 18);
            this.btnModoDetallado.Name = "btnModoDetallado";
            this.btnModoDetallado.Size = new System.Drawing.Size(105, 32);
            this.btnModoDetallado.TabIndex = 6;
            this.btnModoDetallado.Text = "Ver Detalle";
            this.btnModoDetallado.UseVisualStyleBackColor = false;
            this.btnModoDetallado.Click += new System.EventHandler(this.btnModoDetallado_Click);
            // 
            // btnFiltrar
            // 
            this.btnFiltrar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.btnFiltrar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFiltrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFiltrar.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnFiltrar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(175)))), ((int)(((byte)(55)))));
            this.btnFiltrar.Location = new System.Drawing.Point(505, 18);
            this.btnFiltrar.Name = "btnFiltrar";
            this.btnFiltrar.Size = new System.Drawing.Size(80, 32);
            this.btnFiltrar.TabIndex = 5;
            this.btnFiltrar.Text = "Filtrar";
            this.btnFiltrar.UseVisualStyleBackColor = false;
            this.btnFiltrar.Click += new System.EventHandler(this.btnFiltrar_Click);
            // 
            // dtpHasta
            // 
            this.dtpHasta.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.dtpHasta.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpHasta.Location = new System.Drawing.Point(395, 21);
            this.dtpHasta.Name = "dtpHasta";
            this.dtpHasta.Size = new System.Drawing.Size(100, 24);
            this.dtpHasta.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.label2.Location = new System.Drawing.Point(348, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Hasta:";
            // 
            // dtpDesde
            // 
            this.dtpDesde.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.dtpDesde.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDesde.Location = new System.Drawing.Point(240, 21);
            this.dtpDesde.Name = "dtpDesde";
            this.dtpDesde.Size = new System.Drawing.Size(100, 24);
            this.dtpDesde.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.label1.Location = new System.Drawing.Point(190, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Desde:";
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.lblTitulo.Location = new System.Drawing.Point(15, 18);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(170, 28);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Reportes y Ventas";
            // 
            // lblTotales
            // 
            this.lblTotales.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.lblTotales.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblTotales.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblTotales.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(175)))), ((int)(((byte)(55)))));
            this.lblTotales.Location = new System.Drawing.Point(0, 560);
            this.lblTotales.Name = "lblTotales";
            this.lblTotales.Padding = new System.Windows.Forms.Padding(15, 0, 15, 0);
            this.lblTotales.Size = new System.Drawing.Size(1200, 40);
            this.lblTotales.TabIndex = 8;
            this.lblTotales.Text = "Total Neto: C$ 0.00 | IVA (15%): C$ 0.00 | Venta Total + IVA: C$ 0.00";
            this.lblTotales.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dgvReportes
            // 
            this.dgvReportes.AllowUserToAddRows = false;
            this.dgvReportes.AllowUserToDeleteRows = false;
            this.dgvReportes.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(230)))), ((int)(((byte)(211)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvReportes.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvReportes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvReportes.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvReportes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvReportes.Location = new System.Drawing.Point(0, 68);
            this.dgvReportes.MultiSelect = false;
            this.dgvReportes.Name = "dgvReportes";
            this.dgvReportes.ReadOnly = true;
            this.dgvReportes.RowHeadersVisible = false;
            this.dgvReportes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvReportes.Size = new System.Drawing.Size(1200, 492);
            this.dgvReportes.TabIndex = 9;
            this.dgvReportes.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvReportes_CellFormatting);
            // 
            // FormReportes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(230)))), ((int)(((byte)(211)))));
            this.ClientSize = new System.Drawing.Size(1200, 600);
            this.Controls.Add(this.dgvReportes);
            this.Controls.Add(this.lblTotales);
            this.Controls.Add(this.pnlTop);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.Name = "FormReportes";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RestaurantePOS - Reportes de Ventas";
            this.Load += new System.EventHandler(this.FormReportes_Load);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReportes)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpDesde;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpHasta;
        private System.Windows.Forms.Button btnFiltrar;
        private System.Windows.Forms.Button btnModoDetallado;
        private System.Windows.Forms.Button btnModoPedidos;
        private System.Windows.Forms.Button btnExportarPDF;
        private System.Windows.Forms.Button btnExportarExcel;
        private System.Windows.Forms.Button btnFacturaPDF;
        private System.Windows.Forms.Label lblTotales;
        private System.Windows.Forms.DataGridView dgvReportes;
    }
}
