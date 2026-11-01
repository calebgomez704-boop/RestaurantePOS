using System;
using System.Collections.Generic;
using System.Windows.Forms;
using RestaurantePOS.DAL;
using RestaurantePOS.Modelos;

namespace RestaurantePOS.Formularios
{
    public partial class FormPromociones : Form
    {
        private readonly PromocionDAL _dal = new PromocionDAL();
        private readonly ProductoDAL _productoDAL = new ProductoDAL();
        private int _idSeleccionado = 0;

        public FormPromociones()
        {
            InitializeComponent();
        }

        private void FormPromociones_Load(object sender, EventArgs e)
        {
            if (!FormPrincipal.TienePermiso(Sesion.Puesto, "Promociones"))
            {
                MessageBox.Show($"Su puesto ({Sesion.Puesto}) no tiene autorización para acceder al módulo de Promociones.",
                    "Acceso Restringido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }

            CargarProductos();
            CargarDatos();

            UITheme.AplicarEstiloDataGridView(dgvPromociones);
            UITheme.ConfigurarBotonAccion(btnAgregar, UITheme.TipoBotonAccion.Agregar, "  Agregar");
            UITheme.ConfigurarBotonAccion(btnEditar, UITheme.TipoBotonAccion.Editar, "  Editar");
            UITheme.ConfigurarBotonAccion(btnEliminar, UITheme.TipoBotonAccion.Eliminar, "  Eliminar");
        }

        private void CargarProductos()
        {
            try
            {
                var prods = _productoDAL.Listar();
                cmbProducto.DataSource = prods;
                cmbProducto.DisplayMember = "Nombre";
                cmbProducto.ValueMember = "IdProducto";
            }
            catch { }
        }

        private void CargarDatos()
        {
            try
            {
                dgvPromociones.DataSource = null;
                dgvPromociones.DataSource = _dal.Listar();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar promociones: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvPromociones_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvPromociones.CurrentRow == null || dgvPromociones.CurrentRow.DataBoundItem == null)
            {
                _idSeleccionado = 0;
                return;
            }

            var item = dgvPromociones.CurrentRow.DataBoundItem as Promocion;
            if (item != null)
            {
                _idSeleccionado = item.IdPromocion;
                txtNombre.Text = item.Nombre;
                txtDescripcion.Text = item.Descripcion;
                txtDescuento.Text = item.Descuento.ToString("0.##");
                dtpInicio.Value = item.FechaInicio >= dtpInicio.MinDate && item.FechaInicio <= dtpInicio.MaxDate ? item.FechaInicio : DateTime.Today;
                dtpFin.Value = item.FechaFin >= dtpFin.MinDate && item.FechaFin <= dtpFin.MaxDate ? item.FechaFin : DateTime.Today;
                if (item.IdProducto.HasValue)
                    cmbProducto.SelectedValue = item.IdProducto.Value;
                chkActiva.Checked = item.Activa;
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!decimal.TryParse(txtDescuento.Text.Trim(), out decimal descuento))
                {
                    MessageBox.Show("El descuento debe ser un valor numérico.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var promo = new Promocion
                {
                    Nombre = txtNombre.Text,
                    Descripcion = txtDescripcion.Text,
                    Descuento = descuento,
                    FechaInicio = dtpInicio.Value.Date,
                    FechaFin = dtpFin.Value.Date,
                    IdProducto = cmbProducto.SelectedValue != null ? (int?)Convert.ToInt32(cmbProducto.SelectedValue) : null,
                    Activa = chkActiva.Checked
                };

                var errores = promo.Validar();
                if (errores.Count > 0)
                {
                    MessageBox.Show(string.Join("\n", errores), "Datos inválidos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _dal.Insertar(promo);
                MessageBox.Show("Promoción agregada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarDatos();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Dato inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar promoción: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (_idSeleccionado <= 0)
            {
                MessageBox.Show("Seleccione una promoción de la lista primero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (!decimal.TryParse(txtDescuento.Text.Trim(), out decimal descuento))
                {
                    MessageBox.Show("El descuento debe ser un valor numérico.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var promo = new Promocion
                {
                    IdPromocion = _idSeleccionado,
                    Nombre = txtNombre.Text,
                    Descripcion = txtDescripcion.Text,
                    Descuento = descuento,
                    FechaInicio = dtpInicio.Value.Date,
                    FechaFin = dtpFin.Value.Date,
                    IdProducto = cmbProducto.SelectedValue != null ? (int?)Convert.ToInt32(cmbProducto.SelectedValue) : null,
                    Activa = chkActiva.Checked
                };

                var errores = promo.Validar();
                if (errores.Count > 0)
                {
                    MessageBox.Show(string.Join("\n", errores), "Datos inválidos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _dal.Actualizar(promo);
                MessageBox.Show("Promoción actualizada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarDatos();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Dato inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar promoción: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (_idSeleccionado <= 0)
            {
                MessageBox.Show("Seleccione una promoción de la lista primero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show("¿Está seguro de que desea eliminar la promoción seleccionada?",
                "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            try
            {
                _dal.Eliminar(_idSeleccionado);
                MessageBox.Show("Promoción eliminada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarDatos();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "No se puede eliminar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar promoción: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarCampos()
        {
            _idSeleccionado = 0;
            txtNombre.Clear();
            txtDescripcion.Clear();
            txtDescuento.Clear();
            dtpInicio.Value = DateTime.Today;
            dtpFin.Value = DateTime.Today.AddDays(7);
            chkActiva.Checked = true;
            if (cmbProducto.Items.Count > 0)
                cmbProducto.SelectedIndex = 0;
        }
    }
}
