using System;
using System.Windows.Forms;
using RestaurantePOS.DAL;
using RestaurantePOS.Modelos;

namespace RestaurantePOS.Formularios
{
    public partial class FormReservaciones : Form
    {
        private readonly ReservacionDAL _dal = new ReservacionDAL();
        private readonly ClienteDAL _clienteDAL = new ClienteDAL();
        private readonly MesaDAL _mesaDAL = new MesaDAL();
        private int _idSeleccionado = 0;

        public FormReservaciones()
        {
            InitializeComponent();
        }

        private void FormReservaciones_Load(object sender, EventArgs e)
        {
            if (!FormPrincipal.TienePermiso(Sesion.Puesto, "Reservaciones"))
            {
                MessageBox.Show($"Su puesto ({Sesion.Puesto}) no tiene autorización para acceder al módulo de Reservaciones.",
                    "Acceso Restringido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }

            CargarCombos();
            CargarDatos();

            if (cmbEstado.Items.Count > 0 && cmbEstado.SelectedIndex < 0)
                cmbEstado.SelectedIndex = 0;

            UITheme.AplicarEstiloDataGridView(dgvReservaciones);
            UITheme.ConfigurarBotonAccion(btnAgregar, UITheme.TipoBotonAccion.Agregar, "  Agregar");
            UITheme.ConfigurarBotonAccion(btnEditar, UITheme.TipoBotonAccion.Editar, "  Editar");
            UITheme.ConfigurarBotonAccion(btnEliminar, UITheme.TipoBotonAccion.Eliminar, "  Eliminar");
        }

        private void CargarCombos()
        {
            try
            {
                cmbCliente.DataSource = _clienteDAL.Listar();
                cmbCliente.DisplayMember = "Nombre";
                cmbCliente.ValueMember = "IdCliente";

                cmbMesa.DataSource = _mesaDAL.Listar();
                cmbMesa.DisplayMember = "Numero";
                cmbMesa.ValueMember = "IdMesa";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar clientes o mesas: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarDatos()
        {
            try
            {
                dgvReservaciones.DataSource = null;
                dgvReservaciones.DataSource = _dal.Listar();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar reservaciones: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvReservaciones_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvReservaciones.CurrentRow == null || dgvReservaciones.CurrentRow.DataBoundItem == null)
            {
                _idSeleccionado = 0;
                return;
            }

            var item = dgvReservaciones.CurrentRow.DataBoundItem as Reservacion;
            if (item != null)
            {
                _idSeleccionado = item.IdReservacion;
                if (cmbCliente.Items.Count > 0)
                    cmbCliente.SelectedValue = item.IdCliente;
                if (cmbMesa.Items.Count > 0)
                    cmbMesa.SelectedValue = item.IdMesa;
                dtpFechaHora.Value = item.FechaHora >= dtpFechaHora.MinDate && item.FechaHora <= dtpFechaHora.MaxDate ? item.FechaHora : DateTime.Now;
                txtPersonas.Text = item.NumeroPersonas.ToString();
                cmbEstado.SelectedItem = item.Estado;
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbCliente.SelectedValue == null)
                {
                    MessageBox.Show("Debe seleccionar un cliente.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cmbMesa.SelectedValue == null)
                {
                    MessageBox.Show("Debe seleccionar una mesa.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!int.TryParse(txtPersonas.Text.Trim(), out int personas) || personas <= 0)
                {
                    MessageBox.Show("El número de personas debe ser un entero mayor a 0.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var res = new Reservacion
                {
                    IdCliente = Convert.ToInt32(cmbCliente.SelectedValue),
                    IdMesa = Convert.ToInt32(cmbMesa.SelectedValue),
                    FechaHora = dtpFechaHora.Value,
                    NumeroPersonas = personas,
                    Estado = cmbEstado.SelectedItem?.ToString() ?? "Pendiente"
                };

                var errores = res.Validar();
                if (errores.Count > 0)
                {
                    MessageBox.Show(string.Join("\n", errores), "Datos inválidos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _dal.Insertar(res);
                MessageBox.Show("Reservación agregada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarDatos();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Dato inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar reservación: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (_idSeleccionado <= 0)
            {
                MessageBox.Show("Seleccione una reservación de la lista primero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (cmbCliente.SelectedValue == null || cmbMesa.SelectedValue == null)
                {
                    MessageBox.Show("Debe seleccionar cliente y mesa.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!int.TryParse(txtPersonas.Text.Trim(), out int personas) || personas <= 0)
                {
                    MessageBox.Show("El número de personas debe ser un entero mayor a 0.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var res = new Reservacion
                {
                    IdReservacion = _idSeleccionado,
                    IdCliente = Convert.ToInt32(cmbCliente.SelectedValue),
                    IdMesa = Convert.ToInt32(cmbMesa.SelectedValue),
                    FechaHora = dtpFechaHora.Value,
                    NumeroPersonas = personas,
                    Estado = cmbEstado.SelectedItem?.ToString() ?? "Pendiente"
                };

                var errores = res.Validar();
                if (errores.Count > 0)
                {
                    MessageBox.Show(string.Join("\n", errores), "Datos inválidos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _dal.Actualizar(res);
                MessageBox.Show("Reservación actualizada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarDatos();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Dato inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar reservación: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (_idSeleccionado <= 0)
            {
                MessageBox.Show("Seleccione una reservación de la lista primero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmacion = MessageBox.Show("¿Seguro que desea eliminar esta reservación?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmacion == DialogResult.Yes)
            {
                try
                {
                    _dal.Eliminar(_idSeleccionado);
                    MessageBox.Show("Reservación eliminada.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarDatos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar reservación: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LimpiarCampos()
        {
            _idSeleccionado = 0;
            txtPersonas.Text = "2";
            dtpFechaHora.Value = DateTime.Now;
            if (cmbEstado.Items.Count > 0)
                cmbEstado.SelectedIndex = 0;
        }
    }
}
