using RestaurantePOS.DAL;
using RestaurantePOS.Modelos;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace RestaurantePOS
{
    /// <summary>
    /// Formulario 'FormEmpleados': Módulo administrativo de gestión de empleados.
    /// Solo accesible por Administradores.
    /// Permite crear, modificar y desactivar colaboradores, asignando roles restringidos
    /// y contraseñas hasheadas con BCrypt.Net-Next.
    /// </summary>
    public partial class FormEmpleados : Form
    {
        private List<Empleado> listaEmpleados = new List<Empleado>();
        private Empleado empleadoSeleccionado = null;

        public FormEmpleados()
        {
            InitializeComponent();
        }

        private void FormEmpleados_Load(object sender, EventArgs e)
        {
            // PARTE 5 (Punto 10 y 20): Solo el Administrador puede gestionar empleados
            if (!Sesion.EsAdministrador)
            {
                MessageBox.Show("Solo el Administrador tiene autorización para gestionar empleados.", "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.BeginInvoke(new Action(() => this.Close()));
                return;
            }

            // Aplicar estilo visual moderno al DataGridView de empleados
            UITheme.AplicarEstiloDataGridView(empleadosDataGridView);

            // Estilizar los botones CRUD
            UITheme.ConfigurarBotonAccion(btnAgregar, UITheme.TipoBotonAccion.Agregar, "  Agregar");
            UITheme.ConfigurarBotonAccion(btnEditar, UITheme.TipoBotonAccion.Editar, "  Editar");
            UITheme.ConfigurarBotonAccion(btnEliminar, UITheme.TipoBotonAccion.Eliminar, "  Desactivar");

            // Configurar columnas legibles en el DataGridView
            ConfigurarColumnasGrid();

            // Cargar datos
            CargarListaEmpleados();
        }

        private void ConfigurarColumnasGrid()
        {
            empleadosDataGridView.Columns.Clear();
            empleadosDataGridView.AutoGenerateColumns = false;

            empleadosDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "IdEmpleado",
                HeaderText = "ID",
                Width = 50
            });
            empleadosDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Nombre",
                HeaderText = "Nombre",
                Width = 160
            });
            empleadosDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Usuario",
                HeaderText = "Usuario",
                Width = 120
            });
            empleadosDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Puesto",
                HeaderText = "Puesto",
                Width = 120
            });
            empleadosDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Telefono",
                HeaderText = "Teléfono",
                Width = 110
            });
            empleadosDataGridView.Columns.Add(new DataGridViewCheckBoxColumn
            {
                DataPropertyName = "Activo",
                HeaderText = "Activo",
                Width = 70
            });
            empleadosDataGridView.Columns.Add(new DataGridViewCheckBoxColumn
            {
                DataPropertyName = "CambiarClave",
                HeaderText = "Debe Cambiar Clave",
                Width = 120
            });
        }

        private void CargarListaEmpleados()
        {
            listaEmpleados = Repositorio.ObtenerEmpleados(false);
            empleadosDataGridView.DataSource = null;
            empleadosDataGridView.DataSource = listaEmpleados;
            LimpiarCampos();
        }

        private void empleadosDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (empleadosDataGridView.CurrentRow == null || empleadosDataGridView.CurrentRow.DataBoundItem == null)
            {
                empleadoSeleccionado = null;
                return;
            }

            empleadoSeleccionado = (Empleado)empleadosDataGridView.CurrentRow.DataBoundItem;

            txtNombre.Text = empleadoSeleccionado.Nombre;
            txtUsuario.Text = empleadoSeleccionado.Usuario;
            cmbPuesto.SelectedItem = empleadoSeleccionado.Puesto;
            txtTelefono.Text = empleadoSeleccionado.Telefono;
            chkActivo.Checked = empleadoSeleccionado.Activo;
            txtContrasena.Clear(); // La contraseña real NUNCA se muestra al admin (Regla 11)
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            string nombre = txtNombre.Text.Trim();
            string usuario = txtUsuario.Text.Trim();
            string puesto = cmbPuesto.SelectedItem != null ? cmbPuesto.SelectedItem.ToString() : "";
            string telefono = txtTelefono.Text.Trim();
            string claveInicial = txtContrasena.Text;

            // Validar campos básicos
            if (string.IsNullOrWhiteSpace(nombre))
            {
                MessageBox.Show("El nombre del empleado es obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(usuario))
            {
                MessageBox.Show("El nombre de usuario es obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Regla 15: Validar que el usuario sea único
            if (Repositorio.ExisteUsuario(usuario))
            {
                MessageBox.Show($"El nombre de usuario '{usuario}' ya está en uso. Elija otro diferente.", "Usuario Duplicado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Regla 9: Puesto obligatorio
            if (string.IsNullOrWhiteSpace(puesto))
            {
                MessageBox.Show("Seleccione un puesto de la lista.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Regla 15: Validar que la contraseña tenga mínimo 6 caracteres
            if (string.IsNullOrWhiteSpace(claveInicial) || claveInicial.Length < 6)
            {
                MessageBox.Show("La contraseña inicial debe tener al menos 6 caracteres.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var nuevoEmp = new Empleado
            {
                Nombre = nombre,
                Usuario = usuario,
                Puesto = puesto,
                Telefono = telefono,
                Activo = chkActivo.Checked,
                CambiarClave = true // Regla 12: Empleado nuevo debe cambiar clave en primer inicio
            };

            var errores = nuevoEmp.Validar();
            if (errores.Count > 0)
            {
                MessageBox.Show(string.Join("\n", errores), "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Regla 11: Se guarda SIEMPRE con hash BCrypt
                Repositorio.GuardarEmpleado(nuevoEmp, claveInicial);
                MessageBox.Show("Empleado registrado correctamente.\nSe solicitará cambio de contraseña en su primer inicio de sesión.",
                                "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarListaEmpleados();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar empleado: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (empleadoSeleccionado == null)
            {
                MessageBox.Show("Seleccione un empleado de la lista primero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string nombre = txtNombre.Text.Trim();
            string usuario = txtUsuario.Text.Trim();
            string puesto = cmbPuesto.SelectedItem != null ? cmbPuesto.SelectedItem.ToString() : "";
            string telefono = txtTelefono.Text.Trim();
            bool activo = chkActivo.Checked;
            string nuevaClave = txtContrasena.Text;

            // Regla 14: No permitir eliminar, desactivar ni cambiar el puesto del último Administrador activo
            bool eraAdminActivo = empleadoSeleccionado.Puesto == "Administrador" && empleadoSeleccionado.Activo;
            if (eraAdminActivo && (puesto != "Administrador" || !activo))
            {
                int totalAdmins = Repositorio.ContarAdministradoresActivos();
                if (totalAdmins <= 1)
                {
                    MessageBox.Show("Operación denegada: No se puede desactivar ni cambiar el puesto del último Administrador activo del sistema.",
                                    "Seguridad", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }

            // Regla 15: Validar usuario único
            if (Repositorio.ExisteUsuario(usuario, empleadoSeleccionado.IdEmpleado))
            {
                MessageBox.Show($"El nombre de usuario '{usuario}' ya está en uso por otro empleado.", "Usuario Duplicado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Si se ingresó una nueva contraseña, validar mínimo 6 caracteres
            if (!string.IsNullOrWhiteSpace(nuevaClave))
            {
                if (nuevaClave.Length < 6)
                {
                    MessageBox.Show("La nueva contraseña debe tener al menos 6 caracteres.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                // Si el admin restablece la clave, forzar cambio en próximo inicio (Regla 12)
                empleadoSeleccionado.CambiarClave = true;
            }

            empleadoSeleccionado.Nombre = nombre;
            empleadoSeleccionado.Usuario = usuario;
            empleadoSeleccionado.Puesto = puesto;
            empleadoSeleccionado.Telefono = telefono;
            empleadoSeleccionado.Activo = activo;

            var errores = empleadoSeleccionado.Validar();
            if (errores.Count > 0)
            {
                MessageBox.Show(string.Join("\n", errores), "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Repositorio.GuardarEmpleado(empleadoSeleccionado, string.IsNullOrWhiteSpace(nuevaClave) ? null : nuevaClave);
                MessageBox.Show("Datos del empleado actualizados correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarListaEmpleados();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar empleado: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (empleadoSeleccionado == null)
            {
                MessageBox.Show("Seleccione un empleado de la lista primero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Regla 14: No permitir eliminar ni desactivar al último Administrador activo
            if (empleadoSeleccionado.Puesto == "Administrador" && empleadoSeleccionado.Activo)
            {
                int totalAdmins = Repositorio.ContarAdministradoresActivos();
                if (totalAdmins <= 1)
                {
                    MessageBox.Show("Operación denegada: No se puede desactivar al único Administrador activo del sistema.",
                                    "Seguridad", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }

            // Regla 13: "Eliminar" solo lo desactiva (Activo = 0), no lo borra
            var confirmacion = MessageBox.Show($"¿Desea desactivar al empleado '{empleadoSeleccionado.Nombre}'?\n(El empleado no podrá iniciar sesión pero se mantendrá su historial de pedidos)",
                                               "Confirmar Desactivación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmacion == DialogResult.Yes)
            {
                try
                {
                    Repositorio.DesactivarEmpleado(empleadoSeleccionado.IdEmpleado);
                    MessageBox.Show("El empleado ha sido desactivado.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarListaEmpleados();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al desactivar empleado: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtUsuario.Clear();
            if (cmbPuesto.Items.Count > 0) cmbPuesto.SelectedIndex = 2; // Mesero por defecto
            txtTelefono.Clear();
            txtContrasena.Clear();
            chkActivo.Checked = true;
            empleadoSeleccionado = null;
        }
    }
}
