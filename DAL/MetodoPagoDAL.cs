using System;
using System.Collections.Generic;
using System.Data.SQLite;
using RestaurantePOS.Modelos;

namespace RestaurantePOS.DAL
{
    public class MetodoPagoDAL
    {
        private readonly Conexion _conexion = new Conexion();

        public List<MetodoPago> Listar()
        {
            var lista = new List<MetodoPago>();
            using (SQLiteConnection cn = _conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = "SELECT IdMetodoPago, Nombre FROM MetodosPago ORDER BY IdMetodoPago";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    using (SQLiteDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            var m = new MetodoPago(Convert.ToInt32(r.GetValue(0)), r.GetString(1));
                            lista.Add(m);
                        }
                    }
                }
            }
            return lista;
        }

        public MetodoPago BuscarPorId(int id)
        {
            using (SQLiteConnection cn = _conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = "SELECT IdMetodoPago, Nombre FROM MetodosPago WHERE IdMetodoPago = @Id";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (SQLiteDataReader r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            return new MetodoPago(Convert.ToInt32(r.GetValue(0)), r.GetString(1));
                        }
                    }
                }
            }
            return null;
        }

        public int Insertar(MetodoPago metodoPago)
        {
            if (metodoPago == null) throw new ArgumentNullException(nameof(metodoPago));

            using (SQLiteConnection cn = _conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = @"INSERT INTO MetodosPago (Nombre) 
                               VALUES (@Nombre); SELECT last_insert_rowid();";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", metodoPago.Nombre.Trim());
                    int id = Convert.ToInt32(cmd.ExecuteScalar());
                    metodoPago.IdMetodoPago = id;
                    return id;
                }
            }
        }

        public bool Actualizar(MetodoPago metodoPago)
        {
            if (metodoPago == null) throw new ArgumentNullException(nameof(metodoPago));

            using (SQLiteConnection cn = _conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = @"UPDATE MetodosPago 
                               SET Nombre = @Nombre 
                               WHERE IdMetodoPago = @IdMetodoPago";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", metodoPago.Nombre.Trim());
                    cmd.Parameters.AddWithValue("@IdMetodoPago", metodoPago.IdMetodoPago);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Eliminar(int id)
        {
            using (SQLiteConnection cn = _conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = "DELETE FROM MetodosPago WHERE IdMetodoPago = @Id";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    try
                    {
                        return cmd.ExecuteNonQuery() > 0;
                    }
                    catch (SQLiteException ex) when (ex.ResultCode == SQLiteErrorCode.Constraint)
                    {
                        throw new InvalidOperationException("No se puede eliminar el método de pago porque tiene transacciones asociadas.", ex);
                    }
                }
            }
        }
    }
}
