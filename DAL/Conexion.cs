using System;
using System.Data.SQLite;
using System.IO;

namespace RestaurantePOS.DAL
{
    /// <summary>
    /// Fabrica de conexiones SQLite. La base de datos vive en
    /// C:\ProgramData\RestaurantePOS\Datos\restaurante.db (compartida por los usuarios del equipo).
    /// </summary>
    internal class Conexion
    {
        private const string NombreCarpeta = "RestaurantePOS";
        private const string NombreArchivo = "restaurante.db";

        private static string _rutaBaseDatos;

        /// <summary>Ruta completa del archivo de base de datos (crea la carpeta si no existe).</summary>
        public static string RutaBaseDatos
        {
            get
            {
                if (_rutaBaseDatos == null)
                {
                    string carpeta = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                        NombreCarpeta, "Datos");
                    try
                    {
                        Directory.CreateDirectory(carpeta);
                    }
                    catch (Exception ex)
                    {
                        throw new IOException("No se puede crear o escribir en la carpeta de datos " + carpeta + ". " + ex.Message, ex);
                    }
                    _rutaBaseDatos = Path.Combine(carpeta, NombreArchivo);
                }
                return _rutaBaseDatos;
            }
        }

        public static string CadenaConexion
        {
            get { return "Data Source=" + RutaBaseDatos + ";Version=3;Foreign Keys=True;"; }
        }

        public SQLiteConnection ObtenerConexion()
        {
            return new SQLiteConnection(CadenaConexion);
        }
    }
}
