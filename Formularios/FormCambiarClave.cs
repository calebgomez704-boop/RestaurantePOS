using System;
using System.Windows.Forms;
using RestaurantePOS.DAL;

namespace RestaurantePOS.Formularios
{
    public partial class FormCambiarClave : Form
    {
        private readonly int idEmpleado;

        public FormCambiarClave(int idEmpleado)
        {
            InitializeComponent();
            this.idEmpleado = idEmpleado;
            AplicarTema();
        }

        private void AplicarTema()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = UITheme.ColorFondoCrema;
            pnlPrincipal.BackColor = UITheme.ColorFondoCrema;
            lblTitulo.ForeColor = UITheme.ColorVinoPrincipal;
            lblDescripcion.ForeColor = UITheme.ColorTextoOscuro;
            lblNuevaClave.ForeColor = UITheme.ColorVinoPrincipal;
            lblNuevaClave.Font = UITheme.FuenteEtiquetas;
            lblConfirmarClave.ForeColor = UITheme.ColorVinoPrincipal;
            lblConfirmarClave.Font = UITheme.FuenteEtiquetas;
            txtNuevaClave.Font = UITheme.FuenteCampos;
            txtConfirmarClave.Font = UITheme.FuenteCampos;

            UITheme.ConfigurarBotonAccion(btnAceptar, UITheme.TipoBotonAccion.Guardar, "Guardar y Entrar");
            UITheme.ConfigurarBotonAccion(btnCancelar, UITheme.TipoBotonAccion.Secundario, "Cancelar");
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            string nueva = txtNuevaClave.Text;
            string confirmar = txtConfirmarClave.Text;

            // Validación de mínimo 6 caracteres
            if (string.IsNullOrWhiteSpace(nueva) || nueva.Length < 6)
            {
                MessageBox.Show("La nueva contraseña debe tener al menos 6 caracteres.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNuevaClave.Focus();
                return;
            }

            if (nueva != confirmar)
            {
                MessageBox.Show("Las contraseñas ingresadas no coinciden. Verifíquelas nuevamente.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtConfirmarClave.Focus();
                return;
            }

            try
            {
                // Hashear con BCrypt.Net-Next
                string nuevoHash = BCrypt.Net.BCrypt.HashPassword(nueva);

                // Actualizar contraseña en base de datos y marcar CambiarClave = 0
                Repositorio.ActualizarContrasena(idEmpleado, nuevoHash, false);

                MessageBox.Show("Su contraseña ha sido actualizada con éxito.", "Contraseña Actualizada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar la contraseña: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
