using System;
using System.Data.SQLite;
using System.IO;

namespace RestaurantePOS.DAL
{
    /// <summary>
    /// Fabrica de conexiones SQLite. La base de datos vive en
    /// %LocalAppData%\SaboresDelValle\restaurante.db (una por usuario de Windows / instalacion).
    /// </summary>
    internal class Conexion
    {
        private const string NombreCarpeta = "SaboresDelValle";
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
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        NombreCarpeta);
                    Directory.CreateDirectory(carpeta);
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
