using System;
using System.Collections.Generic;
using System.Data.SQLite;
using RestaurantePOS.Modelos;

namespace RestaurantePOS.DAL
{
    public class MesaDAL
    {
        private readonly Conexion _conexion = new Conexion();

        public List<Mesa> Listar()
        {
            var lista = new List<Mesa>();
            using (SQLiteConnection cn = _conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = "SELECT IdMesa, Numero, Capacidad, Estado FROM Mesas ORDER BY Numero";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    using (SQLiteDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            var m = new Mesa(
                                Convert.ToInt32(r.GetValue(0)),
                                Convert.ToInt32(r.GetValue(1)),
                                Convert.ToInt32(r.GetValue(2)),
                                r.GetString(3)
                            );
                            lista.Add(m);
                        }
                    }
                }
            }
            return lista;
        }

        public Mesa BuscarPorId(int id)
        {
            using (SQLiteConnection cn = _conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = "SELECT IdMesa, Numero, Capacidad, Estado FROM Mesas WHERE IdMesa = @Id";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (SQLiteDataReader r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            return new Mesa(
                                Convert.ToInt32(r.GetValue(0)),
                                Convert.ToInt32(r.GetValue(1)),
                                Convert.ToInt32(r.GetValue(2)),
                                r.GetString(3)
                            );
                        }
                    }
                }
            }
            return null;
        }

        public int Insertar(Mesa mesa)
        {
            if (mesa == null) throw new ArgumentNullException(nameof(mesa));

            using (SQLiteConnection cn = _conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = @"INSERT INTO Mesas (Numero, Capacidad, Estado) 
                               VALUES (@Numero, @Capacidad, @Estado); SELECT last_insert_rowid();";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@Numero", mesa.Numero);
                    cmd.Parameters.AddWithValue("@Capacidad", mesa.Capacidad);
                    cmd.Parameters.AddWithValue("@Estado", string.IsNullOrWhiteSpace(mesa.Estado) ? "Disponible" : mesa.Estado.Trim());
                    int id = Convert.ToInt32(cmd.ExecuteScalar());
                    mesa.IdMesa = id;
                    return id;
                }
            }
        }

        public bool Actualizar(Mesa mesa)
        {
            if (mesa == null) throw new ArgumentNullException(nameof(mesa));

            using (SQLiteConnection cn = _conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = @"UPDATE Mesas 
                               SET Numero = @Numero, Capacidad = @Capacidad, Estado = @Estado 
                               WHERE IdMesa = @IdMesa";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@Numero", mesa.Numero);
                    cmd.Parameters.AddWithValue("@Capacidad", mesa.Capacidad);
                    cmd.Parameters.AddWithValue("@Estado", string.IsNullOrWhiteSpace(mesa.Estado) ? "Disponible" : mesa.Estado.Trim());
                    cmd.Parameters.AddWithValue("@IdMesa", mesa.IdMesa);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Eliminar(int id)
        {
            using (SQLiteConnection cn = _conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = "DELETE FROM Mesas WHERE IdMesa = @Id";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    try
                    {
                        return cmd.ExecuteNonQuery() > 0;
                    }
                    catch (SQLiteException ex) when (ex.ResultCode == SQLiteErrorCode.Constraint)
                    {
                        throw new InvalidOperationException("No se puede eliminar la mesa porque tiene pedidos o reservaciones asociadas.", ex);
                    }
                }
            }
        }
    }
}
