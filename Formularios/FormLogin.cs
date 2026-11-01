using System;
using System.Drawing;
using System.Windows.Forms;
using RestaurantePOS.DAL;
using RestaurantePOS.Formularios;
using RestaurantePOS.Modelos;

namespace RestaurantePOS
{
    /// <summary>
    /// Formulario 'FormLogin': Pantalla de autenticación y acceso al sistema.
    /// Valida credenciales contra la tabla Empleados, comprueba contraseñas mediante BCrypt.Net-Next,
    /// bloquea intentos fallidos (5 intentos = 5 minutos de bloqueo), exige cambio de contraseña
    /// inicial y guarda la sesión en la clase estática 'Sesion'.
    /// </summary>
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            // Inicializar base de datos y esquema si es necesario
            DatabaseInitializer.Inicializar();

            // Estilos visuales con UITheme
            this.BackColor = UITheme.ColorFondoCrema;
            lblTitulo.ForeColor = UITheme.ColorVinoPrincipal;
            label1.ForeColor = UITheme.ColorTextoOscuro;
            label2.ForeColor = UITheme.ColorTextoOscuro;
            txtUsuario.ForeColor = UITheme.ColorTextoOscuro;
            txtContrasena.ForeColor = UITheme.ColorTextoOscuro;
            txtUsuario.Font = UITheme.FuenteCampos;
            txtContrasena.Font = UITheme.FuenteCampos;

            // Estilizar botón de ingreso
            btnIngresar.FlatStyle = FlatStyle.Flat;
            btnIngresar.FlatAppearance.BorderSize = 0;
            btnIngresar.BackColor = UITheme.ColorVinoPrincipal;
            btnIngresar.ForeColor = UITheme.ColorDorado;
            btnIngresar.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            btnIngresar.Cursor = Cursors.Hand;

            btnIngresar.MouseEnter += (s, args) =>
            {
                btnIngresar.BackColor = UITheme.ColorVinoClaro;
                btnIngresar.ForeColor = UITheme.ColorBlanco;
            };

            btnIngresar.MouseLeave += (s, args) =>
            {
                btnIngresar.BackColor = UITheme.ColorVinoPrincipal;
                btnIngresar.ForeColor = UITheme.ColorDorado;
            };
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            string usuarioIngresado = txtUsuario.Text.Trim();
            string contrasenaIngresada = txtContrasena.Text;

            // Validación de campos vacíos
            if (string.IsNullOrWhiteSpace(usuarioIngresado) || string.IsNullOrWhiteSpace(contrasenaIngresada))
            {
                MessageBox.Show("Por favor, ingresá usuario y contraseña.", "Campos requeridos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Consultar empleado por usuario
            Empleado empleado = Repositorio.ObtenerEmpleadoPorUsuario(usuarioIngresado);

            if (empleado == null)
            {
                MessageBox.Show("Usuario o contraseña incorrectos.", "Error de Autenticación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validación de empleado activo (Regla 13)
            if (!empleado.Activo)
            {
                MessageBox.Show("El usuario se encuentra inactivo. Comuníquese con el Administrador del restaurante.", "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validación de bloqueo por intentos fallidos (Regla 16: 5 minutos tras 5 intentos)
            if (empleado.BloqueadoHasta.HasValue && empleado.BloqueadoHasta.Value > DateTime.Now)
            {
                var minutosRestantes = Math.Ceiling((empleado.BloqueadoHasta.Value - DateTime.Now).TotalMinutes);
                MessageBox.Show($"Esta cuenta ha sido bloqueada temporalmente por haber superado los 5 intentos fallidos.\nIntente de nuevo en {minutosRestantes} minuto(s).",
                                "Cuenta Bloqueada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validación de contraseña mediante hash BCrypt (Regla 11 y 17)
            bool contrasenaValida = false;
            try
            {
                contrasenaValida = BCrypt.Net.BCrypt.Verify(contrasenaIngresada, empleado.ContrasenaHash);
            }
            catch
            {
                contrasenaValida = false;
            }

            if (!contrasenaValida)
            {
                empleado.IntentosFallidos++;
                if (empleado.IntentosFallidos >= 5)
                {
                    DateTime bloqueo = DateTime.Now.AddMinutes(5);
                    Repositorio.RegistrarIntentoFallido(empleado.IdEmpleado, empleado.IntentosFallidos, bloqueo);
                    MessageBox.Show("Ha superado los 5 intentos fallidos. El acceso a su cuenta ha sido bloqueado por 5 minutos.", "Acceso Bloqueado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    Repositorio.RegistrarIntentoFallido(empleado.IdEmpleado, empleado.IntentosFallidos, null);
                    int restantes = 5 - empleado.IntentosFallidos;
                    MessageBox.Show($"Usuario o contraseña incorrectos. Le quedan {restantes} intento(s) antes del bloqueo temporal.", "Error de Autenticación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return;
            }

            // Contraseña correcta: resetear intentos fallidos
            Repositorio.ResetearIntentos(empleado.IdEmpleado);

            // Requerir cambio de clave si CambiarClave = 1 (Regla 12)
            if (empleado.CambiarClave)
            {
                using (var frmCambio = new FormCambiarClave(empleado.IdEmpleado))
                {
                    if (frmCambio.ShowDialog(this) != DialogResult.OK)
                    {
                        MessageBox.Show("Debe cambiar su contraseña para poder acceder al sistema.", "Seguridad", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }

            // Guardar en la clase estática Sesion (Regla 18)
            Sesion.Iniciar(empleado.IdEmpleado, empleado.Nombre, empleado.Puesto, empleado.Usuario);

            MessageBox.Show($"¡Bienvenido(a), {empleado.Nombre}!\nPuesto: {empleado.Puesto}", "Acceso Concedido", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Abrir menú principal
            var formPrincipal = new FormPrincipal();
            formPrincipal.Show();
            this.Hide();

            formPrincipal.FormClosed += (s, args) =>
            {
                Sesion.CerrarSesion();
                txtContrasena.Clear();
                txtUsuario.Clear();
                this.Show();
            };
        }
    }
}