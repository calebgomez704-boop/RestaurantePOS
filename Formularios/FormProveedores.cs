using System;
using System.Windows.Forms;
using RestaurantePOS.DAL;
using RestaurantePOS.Modelos;

namespace RestaurantePOS
{
    /// <summary>
    /// Formulario 'FormProveedores': Módulo para la gestión de proveedores del restaurante (CRUD).
    /// Permite dar de alta nuevos distribuidores, consultar su información en un DataGridView,
    /// actualizar datos de contacto (teléfono, email) y eliminarlos si ya no tienen relación comercial.
    /// Utiliza 'ProveedorDAL' para comunicarse con SQL Server.
    /// </summary>
    public partial class FormProveedores : Form
    {
        private readonly ProveedorDAL _dal = new ProveedorDAL();
        private int _idSeleccionado = 0;

        public FormProveedores()
        {
            InitializeComponent();
        }

        private void FormProveedores_Load(object sender, EventArgs e)
        {
            CargarDatos();

            // Aplicar estilo visual moderno al DataGridView de proveedores
            UITheme.AplicarEstiloDataGridView(proveedoresDataGridView);

            // Estilizar los botones CRUD con iconos y colores consistentes
            UITheme.ConfigurarBotonAccion(btnAgregar, UITheme.TipoBotonAccion.Agregar, "  Agregar");
            UITheme.ConfigurarBotonAccion(btnEditar, UITheme.TipoBotonAccion.Editar, "  Editar");
            UITheme.ConfigurarBotonAccion(btnEliminar, UITheme.TipoBotonAccion.Eliminar, "  Eliminar");
        }

        private void CargarDatos()
        {
            try
            {
                proveedoresDataGridView.DataSource = null;
                proveedoresDataGridView.DataSource = _dal.Listar();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los proveedores: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void proveedoresDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (proveedoresDataGridView.CurrentRow == null || proveedoresDataGridView.CurrentRow.DataBoundItem == null)
            {
                _idSeleccionado = 0;
                return;
            }

            var fila = proveedoresDataGridView.CurrentRow.DataBoundItem as Proveedor;
            if (fila != null)
            {
                _idSeleccionado = fila.IdProveedor;
                txtNombre.Text = fila.Nombre;
                txtTelefono.Text = fila.Telefono ?? string.Empty;
                txtEmail.Text = fila.Email ?? string.Empty;
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                Proveedor proveedor = new Proveedor
                {
                    Nombre = txtNombre.Text,
                    Telefono = txtTelefono.Text,
                    Email = txtEmail.Text
                };

                var errores = proveedor.Validar();
                if (errores.Count > 0)
                {
                    MessageBox.Show(string.Join("\n", errores), "Datos inválidos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _dal.Insertar(proveedor);
                MessageBox.Show("Proveedor agregado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarDatos();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Dato inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar el proveedor: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (_idSeleccionado <= 0)
            {
                MessageBox.Show("Seleccioná un proveedor de la lista primero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Proveedor proveedor = new Proveedor
                {
                    IdProveedor = _idSeleccionado,
                    Nombre = txtNombre.Text,
                    Telefono = txtTelefono.Text,
                    Email = txtEmail.Text
                };

                var errores = proveedor.Validar();
                if (errores.Count > 0)
                {
                    MessageBox.Show(string.Join("\n", errores), "Datos inválidos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _dal.Actualizar(proveedor);
                MessageBox.Show("Proveedor actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarDatos();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Dato inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar el proveedor: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (_idSeleccionado <= 0)
            {
                MessageBox.Show("Seleccioná un proveedor de la lista primero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmacion = MessageBox.Show("¿Seguro que deseas eliminar este proveedor?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmacion == DialogResult.Yes)
            {
                try
                {
                    _dal.Eliminar(_idSeleccionado);
                    MessageBox.Show("Proveedor eliminado.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarDatos();
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar el proveedor: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LimpiarCampos()
        {
            _idSeleccionado = 0;
            txtNombre.Clear();
            txtTelefono.Clear();
            txtEmail.Clear();
        }
    }
}
