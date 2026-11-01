using System;
using System.Windows.Forms;
using RestaurantePOS.DAL;
using RestaurantePOS.Modelos;

namespace RestaurantePOS
{
    /// <summary>
    /// Formulario 'FormMetodosPago': Módulo de mantenimiento para las formas de pago del restaurante
    /// (Efectivo, Tarjeta de Crédito, Transferencia, etc.).
    /// Realiza operaciones CRUD conectándose a SQL Server mediante MetodoPagoDAL.
    /// </summary>
    public partial class FormMetodosPago : Form
    {
        private readonly MetodoPagoDAL _dal = new MetodoPagoDAL();
        private int _idSeleccionado = 0;

        public FormMetodosPago()
        {
            InitializeComponent();
        }

        private void FormMetodosPago_Load(object sender, EventArgs e)
        {
            CargarDatos();

            // Aplicar estilo visual moderno al DataGridView de métodos de pago
            UITheme.AplicarEstiloDataGridView(metodosPagoDataGridView);

            // Estilizar los botones CRUD con iconos y colores consistentes
            UITheme.ConfigurarBotonAccion(btnAgregar, UITheme.TipoBotonAccion.Agregar, "  Agregar");
            UITheme.ConfigurarBotonAccion(btnEditar, UITheme.TipoBotonAccion.Editar, "  Editar");
            UITheme.ConfigurarBotonAccion(btnEliminar, UITheme.TipoBotonAccion.Eliminar, "  Eliminar");
        }

        private void CargarDatos()
        {
            try
            {
                metodosPagoDataGridView.DataSource = null;
                metodosPagoDataGridView.DataSource = _dal.Listar();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los métodos de pago: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void metodosPagoDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (metodosPagoDataGridView.CurrentRow == null || metodosPagoDataGridView.CurrentRow.DataBoundItem == null)
            {
                _idSeleccionado = 0;
                return;
            }

            var fila = metodosPagoDataGridView.CurrentRow.DataBoundItem as MetodoPago;
            if (fila != null)
            {
                _idSeleccionado = fila.IdMetodoPago;
                txtNombre.Text = fila.Nombre;
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                MetodoPago metodoPago = new MetodoPago
                {
                    Nombre = txtNombre.Text
                };

                var errores = metodoPago.Validar();
                if (errores.Count > 0)
                {
                    MessageBox.Show(string.Join("\n", errores), "Datos inválidos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _dal.Insertar(metodoPago);
                MessageBox.Show("Método de pago agregado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarDatos();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Dato inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar el método de pago: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (_idSeleccionado <= 0)
            {
                MessageBox.Show("Seleccioná un método de pago de la lista primero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                MetodoPago metodoPago = new MetodoPago
                {
                    IdMetodoPago = _idSeleccionado,
                    Nombre = txtNombre.Text
                };

                var errores = metodoPago.Validar();
                if (errores.Count > 0)
                {
                    MessageBox.Show(string.Join("\n", errores), "Datos inválidos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _dal.Actualizar(metodoPago);
                MessageBox.Show("Método de pago actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarDatos();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Dato inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar el método de pago: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (_idSeleccionado <= 0)
            {
                MessageBox.Show("Seleccioná un método de pago de la lista primero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmacion = MessageBox.Show("¿Seguro que deseas eliminar este método de pago?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmacion == DialogResult.Yes)
            {
                try
                {
                    _dal.Eliminar(_idSeleccionado);
                    MessageBox.Show("Método de pago eliminado.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarDatos();
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar el método de pago: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LimpiarCampos()
        {
            _idSeleccionado = 0;
            txtNombre.Clear();
        }
    }
}
