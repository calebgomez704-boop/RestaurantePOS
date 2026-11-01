using System;
using System.Windows.Forms;
using RestaurantePOS.DAL;
using RestaurantePOS.Modelos;

namespace RestaurantePOS.Formularios
{
    public partial class FormInsumos : Form
    {
        private readonly InsumoDAL _dal = new InsumoDAL();
        private readonly ProveedorDAL _proveedorDAL = new ProveedorDAL();
        private int _idSeleccionado = 0;

        public FormInsumos()
        {
            InitializeComponent();
        }

        private void FormInsumos_Load(object sender, EventArgs e)
        {
            if (!FormPrincipal.TienePermiso(Sesion.Puesto, "Insumos"))
            {
                MessageBox.Show($"Su puesto ({Sesion.Puesto}) no tiene autorización para acceder al módulo de Insumos.",
                    "Acceso Restringido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }

            CargarProveedores();
            CargarDatos();

            UITheme.AplicarEstiloDataGridView(dgvInsumos);
            UITheme.ConfigurarBotonAccion(btnAgregar, UITheme.TipoBotonAccion.Agregar, "  Agregar");
            UITheme.ConfigurarBotonAccion(btnEditar, UITheme.TipoBotonAccion.Editar, "  Editar");
            UITheme.ConfigurarBotonAccion(btnEliminar, UITheme.TipoBotonAccion.Eliminar, "  Eliminar");
        }

        private void CargarProveedores()
        {
            try
            {
                var proveedores = _proveedorDAL.Listar();
                cmbProveedor.DataSource = proveedores;
                cmbProveedor.DisplayMember = "Nombre";
                cmbProveedor.ValueMember = "IdProveedor";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar proveedores: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarDatos()
        {
            try
            {
                dgvInsumos.DataSource = null;
                dgvInsumos.DataSource = _dal.Listar();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar insumos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvInsumos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvInsumos.CurrentRow == null || dgvInsumos.CurrentRow.DataBoundItem == null)
            {
                _idSeleccionado = 0;
                return;
            }

            var item = dgvInsumos.CurrentRow.DataBoundItem as Insumo;
            if (item != null)
            {
                _idSeleccionado = item.IdInsumo;
                txtNombre.Text = item.Nombre;
                txtUnidad.Text = item.Unidad;
                txtStock.Text = item.Stock.ToString("0.##");
                if (cmbProveedor.Items.Count > 0)
                    cmbProveedor.SelectedValue = item.IdProveedor;
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbProveedor.SelectedValue == null)
                {
                    MessageBox.Show("Debe seleccionar un proveedor.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!decimal.TryParse(txtStock.Text.Trim(), out decimal stock))
                {
                    MessageBox.Show("El stock debe ser un valor numérico.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var insumo = new Insumo
                {
                    Nombre = txtNombre.Text,
                    Unidad = txtUnidad.Text,
                    Stock = stock,
                    IdProveedor = Convert.ToInt32(cmbProveedor.SelectedValue)
                };

                var errores = insumo.Validar();
                if (errores.Count > 0)
                {
                    MessageBox.Show(string.Join("\n", errores), "Datos inválidos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _dal.Insertar(insumo);
                MessageBox.Show("Insumo agregado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarDatos();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Dato inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar insumo: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (_idSeleccionado <= 0)
            {
                MessageBox.Show("Seleccione un insumo de la lista primero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (cmbProveedor.SelectedValue == null)
                {
                    MessageBox.Show("Debe seleccionar un proveedor.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!decimal.TryParse(txtStock.Text.Trim(), out decimal stock))
                {
                    MessageBox.Show("El stock debe ser un valor numérico.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var insumo = new Insumo
                {
                    IdInsumo = _idSeleccionado,
                    Nombre = txtNombre.Text,
                    Unidad = txtUnidad.Text,
                    Stock = stock,
                    IdProveedor = Convert.ToInt32(cmbProveedor.SelectedValue)
                };

                var errores = insumo.Validar();
                if (errores.Count > 0)
                {
                    MessageBox.Show(string.Join("\n", errores), "Datos inválidos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _dal.Actualizar(insumo);
                MessageBox.Show("Insumo actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarDatos();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Dato inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar insumo: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (_idSeleccionado <= 0)
            {
                MessageBox.Show("Seleccione un insumo de la lista primero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmacion = MessageBox.Show("¿Seguro que desea eliminar este insumo?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmacion == DialogResult.Yes)
            {
                try
                {
                    _dal.Eliminar(_idSeleccionado);
                    MessageBox.Show("Insumo eliminado.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarDatos();
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar insumo: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LimpiarCampos()
        {
            _idSeleccionado = 0;
            txtNombre.Clear();
            txtUnidad.Clear();
            txtStock.Clear();
        }
    }
}
