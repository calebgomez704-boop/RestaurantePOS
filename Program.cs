using System;
using System.Windows.Forms;
using RestaurantePOS.DAL;

namespace RestaurantePOS
{
    /// <summary>
    /// Clase principal 'Program': Contiene el punto de arranque de todo el sistema.
    /// Ejecuta la inicialización del esquema de base de datos e inicia en FormLogin.
    /// </summary>
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                // Crea la base de datos SQLite local (primera vez) y asegura el esquema.
                DatabaseInitializer.Inicializar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo iniciar la base de datos local.\n\n" + ex.Message +
                    "\n\nUbicación: " + Conexion.RutaBaseDatos,
                    "Sabores del Valle POS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Application.Run(new FormLogin());
        }
    }
}
