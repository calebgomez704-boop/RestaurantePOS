namespace RestaurantePOS.Formularios
{
    partial class FormTurnos
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
            this.lblTitulo = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbEmpleado = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpInicio = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.dtpFin = new System.Windows.Forms.DateTimePicker();
            this.chkEnCurso = new System.Windows.Forms.CheckBox();
            this.btnAbrirTurno = new System.Windows.Forms.Button();
            this.btnCerrarTurno = new System.Windows.Forms.Button();
            this.btnAgregar = new System.Windows.Forms.Button();
            this.btnEditar = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.dgvTurnos = new System.Windows.Forms.DataGridView();
            this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEmpleado = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInicio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTurnos)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.lblTitulo.Location = new System.Drawing.Point(20, 12);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(225, 32);
            this.lblTitulo.TabIndex = 20;
            this.lblTitulo.Text = "Control de Turnos";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.label1.Location = new System.Drawing.Point(20, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 19);
            this.label1.TabIndex = 1;
            this.label1.Text = "Empleado";
            // 
            // cmbEmpleado
            // 
            this.cmbEmpleado.BackColor = System.Drawing.Color.White;
            this.cmbEmpleado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEmpleado.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbEmpleado.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.cmbEmpleado.FormattingEnabled = true;
            this.cmbEmpleado.Location = new System.Drawing.Point(95, 53);
            this.cmbEmpleado.Name = "cmbEmpleado";
            this.cmbEmpleado.Size = new System.Drawing.Size(200, 25);
            this.cmbEmpleado.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.label2.Location = new System.Drawing.Point(305, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 19);
            this.label2.TabIndex = 3;
            this.label2.Text = "Inicio";
            // 
            // dtpInicio
            // 
            this.dtpInicio.CustomFormat = "yyyy-MM-dd HH:mm";
            this.dtpInicio.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dtpInicio.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpInicio.Location = new System.Drawing.Point(350, 53);
            this.dtpInicio.Name = "dtpInicio";
            this.dtpInicio.Size = new System.Drawing.Size(150, 25);
            this.dtpInicio.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.label3.Location = new System.Drawing.Point(510, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(27, 19);
            this.label3.TabIndex = 5;
            this.label3.Text = "Fin";
            // 
            // dtpFin
            // 
            this.dtpFin.CustomFormat = "yyyy-MM-dd HH:mm";
            this.dtpFin.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dtpFin.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFin.Location = new System.Drawing.Point(545, 53);
            this.dtpFin.Name = "dtpFin";
            this.dtpFin.Size = new System.Drawing.Size(150, 25);
            this.dtpFin.TabIndex = 6;
            // 
            // chkEnCurso
            // 
            this.chkEnCurso.AutoSize = true;
            this.chkEnCurso.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.chkEnCurso.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.chkEnCurso.Location = new System.Drawing.Point(710, 55);
            this.chkEnCurso.Name = "chkEnCurso";
            this.chkEnCurso.Size = new System.Drawing.Size(81, 23);
            this.chkEnCurso.TabIndex = 7;
            this.chkEnCurso.Text = "En curso";
            this.chkEnCurso.UseVisualStyleBackColor = true;
            this.chkEnCurso.CheckedChanged += new System.EventHandler(this.chkEnCurso_CheckedChanged);
            // 
            // btnAbrirTurno
            // 
            this.btnAbrirTurno.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(117)))), ((int)(((byte)(89)))));
            this.btnAbrirTurno.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAbrirTurno.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAbrirTurno.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnAbrirTurno.ForeColor = System.Drawing.Color.White;
            this.btnAbrirTurno.Location = new System.Drawing.Point(95, 93);
            this.btnAbrirTurno.Name = "btnAbrirTurno";
            this.btnAbrirTurno.Size = new System.Drawing.Size(145, 36);
            this.btnAbrirTurno.TabIndex = 8;
            this.btnAbrirTurno.Text = "Abrir Turno";
            this.btnAbrirTurno.UseVisualStyleBackColor = false;
            this.btnAbrirTurno.Click += new System.EventHandler(this.btnAbrirTurno_Click);
            // 
            // btnCerrarTurno
            // 
            this.btnCerrarTurno.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnCerrarTurno.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCerrarTurno.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCerrarTurno.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCerrarTurno.ForeColor = System.Drawing.Color.White;
            this.btnCerrarTurno.Location = new System.Drawing.Point(250, 93);
            this.btnCerrarTurno.Name = "btnCerrarTurno";
            this.btnCerrarTurno.Size = new System.Drawing.Size(145, 36);
            this.btnCerrarTurno.TabIndex = 9;
            this.btnCerrarTurno.Text = "Cerrar Turno";
            this.btnCerrarTurno.UseVisualStyleBackColor = false;
            this.btnCerrarTurno.Click += new System.EventHandler(this.btnCerrarTurno_Click);
            // 
            // btnAgregar
            // 
            this.btnAgregar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAgregar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.btnAgregar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAgregar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAgregar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnAgregar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(175)))), ((int)(((byte)(55)))));
            this.btnAgregar.Location = new System.Drawing.Point(550, 93);
            this.btnAgregar.Name = "btnAgregar";
            this.btnAgregar.Size = new System.Drawing.Size(125, 36);
            this.btnAgregar.TabIndex = 10;
            this.btnAgregar.Text = "Agregar";
            this.btnAgregar.UseVisualStyleBackColor = false;
            this.btnAgregar.Click += new System.EventHandler(this.btnAgregar_Click);
            // 
            // btnEditar
            // 
            this.btnEditar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEditar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.btnEditar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEditar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnEditar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(175)))), ((int)(((byte)(55)))));
            this.btnEditar.Location = new System.Drawing.Point(685, 93);
            this.btnEditar.Name = "btnEditar";
            this.btnEditar.Size = new System.Drawing.Size(125, 36);
            this.btnEditar.TabIndex = 11;
            this.btnEditar.Text = "Editar";
            this.btnEditar.UseVisualStyleBackColor = false;
            this.btnEditar.Click += new System.EventHandler(this.btnEditar_Click);
            // 
            // btnEliminar
            // 
            this.btnEliminar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEliminar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.btnEliminar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEliminar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEliminar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnEliminar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(175)))), ((int)(((byte)(55)))));
            this.btnEliminar.Location = new System.Drawing.Point(820, 93);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(140, 36);
            this.btnEliminar.TabIndex = 12;
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.UseVisualStyleBackColor = false;
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);
            // 
            // dgvTurnos
            // 
            this.dgvTurnos.AllowUserToAddRows = false;
            this.dgvTurnos.AllowUserToDeleteRows = false;
            this.dgvTurnos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvTurnos.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(230)))), ((int)(((byte)(211)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvTurnos.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvTurnos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTurnos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colId,
            this.colEmpleado,
            this.colInicio,
            this.colFin});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvTurnos.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvTurnos.Location = new System.Drawing.Point(20, 145);
            this.dgvTurnos.MultiSelect = false;
            this.dgvTurnos.Name = "dgvTurnos";
            this.dgvTurnos.ReadOnly = true;
            this.dgvTurnos.RowHeadersVisible = false;
            this.dgvTurnos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTurnos.Size = new System.Drawing.Size(960, 435);
            this.dgvTurnos.TabIndex = 13;
            this.dgvTurnos.SelectionChanged += new System.EventHandler(this.dgvTurnos_SelectionChanged);
            // 
            // colId
            // 
            this.colId.DataPropertyName = "IdTurno";
            this.colId.HeaderText = "Id";
            this.colId.Name = "colId";
            this.colId.ReadOnly = true;
            this.colId.Width = 60;
            // 
            // colEmpleado
            // 
            this.colEmpleado.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colEmpleado.DataPropertyName = "NombreEmpleado";
            this.colEmpleado.HeaderText = "Empleado";
            this.colEmpleado.Name = "colEmpleado";
            this.colEmpleado.ReadOnly = true;
            // 
            // colInicio
            // 
            this.colInicio.DataPropertyName = "FechaInicio";
            this.colInicio.DefaultCellStyle.Format = "g";
            this.colInicio.HeaderText = "Inicio del Turno";
            this.colInicio.Name = "colInicio";
            this.colInicio.ReadOnly = true;
            this.colInicio.Width = 200;
            // 
            // colFin
            // 
            this.colFin.DataPropertyName = "FechaFin";
            this.colFin.DefaultCellStyle.Format = "g";
            this.colFin.HeaderText = "Fin del Turno";
            this.colFin.Name = "colFin";
            this.colFin.ReadOnly = true;
            this.colFin.Width = 200;
            // 
            // FormTurnos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(230)))), ((int)(((byte)(211)))));
            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.Controls.Add(this.dgvTurnos);
            this.Controls.Add(this.btnEliminar);
            this.Controls.Add(this.btnEditar);
            this.Controls.Add(this.btnAgregar);
            this.Controls.Add(this.btnCerrarTurno);
            this.Controls.Add(this.btnAbrirTurno);
            this.Controls.Add(this.chkEnCurso);
            this.Controls.Add(this.dtpFin);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dtpInicio);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbEmpleado);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblTitulo);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.Name = "FormTurnos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RestaurantePOS - Turnos";
            this.Load += new System.EventHandler(this.FormTurnos_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTurnos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbEmpleado;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpInicio;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtpFin;
        private System.Windows.Forms.CheckBox chkEnCurso;
        private System.Windows.Forms.Button btnAbrirTurno;
        private System.Windows.Forms.Button btnCerrarTurno;
        private System.Windows.Forms.Button btnAgregar;
        private System.Windows.Forms.Button btnEditar;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.DataGridView dgvTurnos;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEmpleado;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInicio;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFin;
    }
}
