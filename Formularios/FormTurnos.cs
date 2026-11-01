using System;
using System.Drawing;
using System.Windows.Forms;
using RestaurantePOS.DAL;
using RestaurantePOS.Modelos;

namespace RestaurantePOS.Formularios
{
    public partial class FormTurnos : Form
    {
        private readonly TurnoDAL _dal = new TurnoDAL();
        private int _idSeleccionado = 0;

        public FormTurnos()
        {
            InitializeComponent();
        }

        private void FormTurnos_Load(object sender, EventArgs e)
        {
            if (!FormPrincipal.TienePermiso(Sesion.Puesto, "Turnos"))
            {
                MessageBox.Show($"Su puesto ({Sesion.Puesto}) no tiene autorización para acceder al módulo de Turnos.",
                    "Acceso Restringido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }

            CargarEmpleados();
            CargarDatos();

            UITheme.AplicarEstiloDataGridView(dgvTurnos);
            UITheme.ConfigurarBotonAccion(btnAgregar, UITheme.TipoBotonAccion.Secundario, "  Guardar Man.");
            UITheme.ConfigurarBotonAccion(btnEditar, UITheme.TipoBotonAccion.Editar, "  Editar");
            UITheme.ConfigurarBotonAccion(btnEliminar, UITheme.TipoBotonAccion.Eliminar, "  Eliminar");
            UITheme.ConfigurarBotonAccion(btnAbrirTurno, UITheme.TipoBotonAccion.Agregar, "  Abrir Turno");
            UITheme.ConfigurarBotonAccion(btnCerrarTurno, UITheme.TipoBotonAccion.Eliminar, "  Cerrar Turno");

            // Solo Administrador puede editar libremente turnos pasados
            bool esAdmin = string.Equals(Sesion.Puesto, "Administrador", StringComparison.OrdinalIgnoreCase);
            btnEditar.Visible = esAdmin;
            btnEliminar.Visible = esAdmin;
            btnAgregar.Visible = esAdmin;
        }

        private void CargarEmpleados()
        {
            try
            {
                var empleados = Repositorio.ObtenerEmpleados(true);
                cmbEmpleado.DataSource = empleados;
                cmbEmpleado.DisplayMember = "Nombre";
                cmbEmpleado.ValueMember = "IdEmpleado";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar empleados: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarDatos()
        {
            try
            {
                dgvTurnos.DataSource = null;
                dgvTurnos.DataSource = _dal.Listar();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar turnos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void chkEnCurso_CheckedChanged(object sender, EventArgs e)
        {
            dtpFin.Enabled = !chkEnCurso.Checked;
        }

        private void dgvTurnos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvTurnos.CurrentRow == null || dgvTurnos.CurrentRow.DataBoundItem == null)
            {
                _idSeleccionado = 0;
                return;
            }

            var item = dgvTurnos.CurrentRow.DataBoundItem as Turno;
            if (item != null)
            {
                _idSeleccionado = item.IdTurno;
                if (cmbEmpleado.Items.Count > 0)
                    cmbEmpleado.SelectedValue = item.IdEmpleado;

                dtpInicio.Value = item.FechaInicio >= dtpInicio.MinDate && item.FechaInicio <= dtpInicio.MaxDate ? item.FechaInicio : DateTime.Now;
                if (item.FechaFin.HasValue)
                {
                    chkEnCurso.Checked = false;
                    dtpFin.Enabled = true;
                    dtpFin.Value = item.FechaFin.Value >= dtpFin.MinDate && item.FechaFin.Value <= dtpFin.MaxDate ? item.FechaFin.Value : DateTime.Now;
                }
                else
                {
                    chkEnCurso.Checked = true;
                    dtpFin.Enabled = false;
                }
            }
        }

        private void btnAbrirTurno_Click(object sender, EventArgs e)
        {
            if (cmbEmpleado.SelectedValue == null)
            {
                MessageBox.Show("Debe seleccionar un empleado para abrir su turno.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idEmp = Convert.ToInt32(cmbEmpleado.SelectedValue);
            string nombreEmp = cmbEmpleado.Text;

            // Bloquear si el empleado ya tiene un turno abierto (FechaFin IS NULL)
            var turnoAbierto = _dal.ObtenerTurnoAbierto(idEmp);
            if (turnoAbierto != null)
            {
                MessageBox.Show($"El empleado '{nombreEmp}' ya tiene un turno abierto (Iniciado: {turnoAbierto.FechaInicio:dd/MM/yyyy HH:mm}).\nNo puede tener dos turnos abiertos simultáneamente. Debe cerrar el turno anterior primero.",
                    "Turno Ya Abierto", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var nuevoTurno = new Turno
                {
                    IdEmpleado = idEmp,
                    FechaInicio = DateTime.Now,
                    FechaFin = null
                };

                _dal.Insertar(nuevoTurno);
                MessageBox.Show($"Turno abierto exitosamente para {nombreEmp} a las {nuevoTurno.FechaInicio:HH:mm}.", "Turno Iniciado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarDatos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abrir turno: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCerrarTurno_Click(object sender, EventArgs e)
        {
            Turno turnoACerrar = null;

            if (_idSeleccionado > 0)
            {
                var seleccionado = _dal.BuscarPorId(_idSeleccionado);
                if (seleccionado != null && !seleccionado.FechaFin.HasValue)
                {
                    turnoACerrar = seleccionado;
                }
            }

            if (turnoACerrar == null && cmbEmpleado.SelectedValue != null)
            {
                int idEmp = Convert.ToInt32(cmbEmpleado.SelectedValue);
                turnoACerrar = _dal.ObtenerTurnoAbierto(idEmp);
            }

            if (turnoACerrar == null)
            {
                MessageBox.Show("No se encontró ningún turno abierto (en curso) para cerrar con el empleado o registro seleccionado.",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DateTime horaFin = DateTime.Now;
            if (horaFin < turnoACerrar.FechaInicio)
            {
                horaFin = turnoACerrar.FechaInicio.AddMinutes(1);
            }

            try
            {
                turnoACerrar.FechaFin = horaFin;
                _dal.Actualizar(turnoACerrar);

                // Obtener resumen de ventas durante este turno
                var resumen = _dal.ObtenerResumenVentasTurno(turnoACerrar.IdEmpleado, turnoACerrar.FechaInicio, horaFin);

                string mensajeResumen = $"========================================\n" +
                                        $"       RESUMEN DE CIERRE DE TURNO       \n" +
                                        $"========================================\n" +
                                        $"Empleado: {turnoACerrar.NombreEmpleado}\n" +
                                        $"Inicio: {turnoACerrar.FechaInicio:dd/MM/yyyy HH:mm:ss}\n" +
                                        $"Fin:    {horaFin:dd/MM/yyyy HH:mm:ss}\n" +
                                        $"Duración: {(horaFin - turnoACerrar.FechaInicio).TotalHours:N1} horas\n" +
                                        $"----------------------------------------\n" +
                                        $"Pedidos Pagados Atendidos: {resumen.CantidadPedidos}\n" +
                                        $"Total Vendido: C$ {resumen.TotalVendido:N2}\n" +
                                        $"========================================";

                MessageBox.Show(mensajeResumen, "Cierre de Turno Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarDatos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cerrar turno: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbEmpleado.SelectedValue == null)
                {
                    MessageBox.Show("Debe seleccionar un empleado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int idEmp = Convert.ToInt32(cmbEmpleado.SelectedValue);
                DateTime? fin = chkEnCurso.Checked ? (DateTime?)null : dtpFin.Value;

                if (!fin.HasValue)
                {
                    var existente = _dal.ObtenerTurnoAbierto(idEmp);
                    if (existente != null)
                    {
                        MessageBox.Show($"El empleado ya cuenta con un turno abierto. No puede registrar otro turno en curso.",
                            "Turno Duplicado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                var turno = new Turno
                {
                    IdEmpleado = idEmp,
                    FechaInicio = dtpInicio.Value,
                    FechaFin = fin
                };

                var errores = turno.Validar();
                if (errores.Count > 0)
                {
                    MessageBox.Show(string.Join("\n", errores), "Datos inválidos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _dal.Insertar(turno);
                MessageBox.Show("Turno registrado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarDatos();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Dato inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar turno: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (_idSeleccionado <= 0)
            {
                MessageBox.Show("Seleccione un turno de la lista primero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (cmbEmpleado.SelectedValue == null)
                {
                    MessageBox.Show("Debe seleccionar un empleado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DateTime? fin = chkEnCurso.Checked ? (DateTime?)null : dtpFin.Value;

                var turno = new Turno
                {
                    IdTurno = _idSeleccionado,
                    IdEmpleado = Convert.ToInt32(cmbEmpleado.SelectedValue),
                    FechaInicio = dtpInicio.Value,
                    FechaFin = fin
                };

                var errores = turno.Validar();
                if (errores.Count > 0)
                {
                    MessageBox.Show(string.Join("\n", errores), "Datos inválidos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _dal.Actualizar(turno);
                MessageBox.Show("Turno actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarDatos();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Dato inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar turno: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (_idSeleccionado <= 0)
            {
                MessageBox.Show("Seleccione un turno de la lista primero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show("¿Está seguro de que desea eliminar el turno seleccionado?",
                "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            try
            {
                _dal.Eliminar(_idSeleccionado);
                MessageBox.Show("Turno eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarDatos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar turno: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarCampos()
        {
            _idSeleccionado = 0;
            dtpInicio.Value = DateTime.Now;
            dtpFin.Value = DateTime.Now;
            chkEnCurso.Checked = true;
            dtpFin.Enabled = false;
        }
    }
}
