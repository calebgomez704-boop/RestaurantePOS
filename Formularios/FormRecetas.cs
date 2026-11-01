using System;
using System.Windows.Forms;
using RestaurantePOS.DAL;
using RestaurantePOS.Modelos;

namespace RestaurantePOS.Formularios
{
    public partial class FormRecetas : Form
    {
        private readonly RecetaProductoDAL _dal = new RecetaProductoDAL();
        private readonly ProductoDAL _productoDAL = new ProductoDAL();
        private readonly InsumoDAL _insumoDAL = new InsumoDAL();
        private int _idSeleccionado = 0;

        public FormRecetas()
        {
            InitializeComponent();
        }

        private void FormRecetas_Load(object sender, EventArgs e)
        {
            if (!FormPrincipal.TienePermiso(Sesion.Puesto, "Recetas"))
            {
                MessageBox.Show($"Su puesto ({Sesion.Puesto}) no tiene autorización para acceder al módulo de Recetas.",
                    "Acceso Restringido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }

            CargarCombos();
            CargarDatos();

            UITheme.AplicarEstiloDataGridView(dgvRecetas);
            UITheme.ConfigurarBotonAccion(btnAgregar, UITheme.TipoBotonAccion.Agregar, "  Agregar");
            UITheme.ConfigurarBotonAccion(btnEditar, UITheme.TipoBotonAccion.Editar, "  Editar");
            UITheme.ConfigurarBotonAccion(btnEliminar, UITheme.TipoBotonAccion.Eliminar, "  Eliminar");
        }

        private void CargarCombos()
        {
            try
            {
                cmbProducto.DataSource = _productoDAL.Listar();
                cmbProducto.DisplayMember = "Nombre";
                cmbProducto.ValueMember = "IdProducto";

                cmbInsumo.DataSource = _insumoDAL.Listar();
                cmbInsumo.DisplayMember = "Nombre";
                cmbInsumo.ValueMember = "IdInsumo";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar productos o insumos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarDatos()
        {
            try
            {
                dgvRecetas.DataSource = null;
                dgvRecetas.DataSource = _dal.Listar();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar recetas: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvRecetas_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvRecetas.CurrentRow == null || dgvRecetas.CurrentRow.DataBoundItem == null)
            {
                _idSeleccionado = 0;
                return;
            }

            var item = dgvRecetas.CurrentRow.DataBoundItem as RecetaProducto;
            if (item != null)
            {
                _idSeleccionado = item.IdRecetaProducto;
                if (cmbProducto.Items.Count > 0)
                    cmbProducto.SelectedValue = item.IdProducto;
                if (cmbInsumo.Items.Count > 0)
                    cmbInsumo.SelectedValue = item.IdInsumo;
                txtCantidad.Text = item.CantidadNecesaria.ToString("0.##");
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbProducto.SelectedValue == null)
                {
                    MessageBox.Show("Debe seleccionar un producto.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cmbInsumo.SelectedValue == null)
                {
                    MessageBox.Show("Debe seleccionar un insumo.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!decimal.TryParse(txtCantidad.Text.Trim(), out decimal cant))
                {
                    MessageBox.Show("La cantidad debe ser un número válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var receta = new RecetaProducto
                {
                    IdProducto = Convert.ToInt32(cmbProducto.SelectedValue),
                    IdInsumo = Convert.ToInt32(cmbInsumo.SelectedValue),
                    CantidadNecesaria = cant
                };

                var errores = receta.Validar();
                if (errores.Count > 0)
                {
                    MessageBox.Show(string.Join("\n", errores), "Datos inválidos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _dal.Insertar(receta);
                MessageBox.Show("Receta agregada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarDatos();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Dato inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar receta: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (_idSeleccionado <= 0)
            {
                MessageBox.Show("Seleccione una receta de la lista primero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (cmbProducto.SelectedValue == null || cmbInsumo.SelectedValue == null)
                {
                    MessageBox.Show("Debe seleccionar producto e insumo.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!decimal.TryParse(txtCantidad.Text.Trim(), out decimal cant))
                {
                    MessageBox.Show("La cantidad debe ser un número válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var receta = new RecetaProducto
                {
                    IdRecetaProducto = _idSeleccionado,
                    IdProducto = Convert.ToInt32(cmbProducto.SelectedValue),
                    IdInsumo = Convert.ToInt32(cmbInsumo.SelectedValue),
                    CantidadNecesaria = cant
                };

                var errores = receta.Validar();
                if (errores.Count > 0)
                {
                    MessageBox.Show(string.Join("\n", errores), "Datos inválidos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _dal.Actualizar(receta);
                MessageBox.Show("Receta actualizada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarDatos();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Dato inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar receta: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (_idSeleccionado <= 0)
            {
                MessageBox.Show("Seleccione una receta de la lista primero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmacion = MessageBox.Show("¿Seguro que desea eliminar esta receta?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmacion == DialogResult.Yes)
            {
                try
                {
                    _dal.Eliminar(_idSeleccionado);
                    MessageBox.Show("Receta eliminada.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarDatos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar receta: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LimpiarCampos()
        {
            _idSeleccionado = 0;
            txtCantidad.Clear();
        }
    }
}
