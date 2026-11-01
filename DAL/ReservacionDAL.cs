using System;
using System.Collections.Generic;
using System.Data.SQLite;
using RestaurantePOS.Modelos;

namespace RestaurantePOS.DAL
{
    public class ReservacionDAL
    {
        private readonly Conexion _conexion = new Conexion();

        public List<Reservacion> Listar()
        {
            var lista = new List<Reservacion>();
            using (SQLiteConnection cn = _conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = @"
SELECT r.IdReservacion, r.IdCliente, r.IdMesa, r.FechaHora, r.NumeroPersonas, r.Estado,
       IFNULL(c.Nombre, 'Sin cliente') AS NombreCliente,
       IFNULL(CAST(m.Numero AS TEXT), 'Sin mesa') AS NumeroMesa
FROM Reservaciones r
LEFT JOIN Clientes c ON c.IdCliente = r.IdCliente
LEFT JOIN Mesas    m ON m.IdMesa    = r.IdMesa
ORDER BY r.FechaHora DESC";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    using (SQLiteDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            var res = new Reservacion
                            {
                                IdReservacion = Convert.ToInt32(r.GetValue(0)),
                                IdCliente = Convert.ToInt32(r.GetValue(1)),
                                IdMesa = Convert.ToInt32(r.GetValue(2)),
                                FechaHora = Convert.ToDateTime(r.GetValue(3)),
                                NumeroPersonas = Convert.ToInt32(r.GetValue(4)),
                                Estado = r.GetString(5),
                                NombreCliente = r.GetString(6),
                                NumeroMesa = r.GetString(7)
                            };
                            lista.Add(res);
                        }
                    }
                }
            }
            return lista;
        }

        public Reservacion BuscarPorId(int id)
        {
            using (SQLiteConnection cn = _conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = @"
SELECT r.IdReservacion, r.IdCliente, r.IdMesa, r.FechaHora, r.NumeroPersonas, r.Estado,
       IFNULL(c.Nombre, 'Sin cliente') AS NombreCliente,
       IFNULL(CAST(m.Numero AS TEXT), 'Sin mesa') AS NumeroMesa
FROM Reservaciones r
LEFT JOIN Clientes c ON c.IdCliente = r.IdCliente
LEFT JOIN Mesas    m ON m.IdMesa    = r.IdMesa
WHERE r.IdReservacion = @Id";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (SQLiteDataReader r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            return new Reservacion
                            {
                                IdReservacion = Convert.ToInt32(r.GetValue(0)),
                                IdCliente = Convert.ToInt32(r.GetValue(1)),
                                IdMesa = Convert.ToInt32(r.GetValue(2)),
                                FechaHora = Convert.ToDateTime(r.GetValue(3)),
                                NumeroPersonas = Convert.ToInt32(r.GetValue(4)),
                                Estado = r.GetString(5),
                                NombreCliente = r.GetString(6),
                                NumeroMesa = r.GetString(7)
                            };
                        }
                    }
                }
            }
            return null;
        }

        public int Insertar(Reservacion reservacion)
        {
            if (reservacion == null) throw new ArgumentNullException(nameof(reservacion));

            using (SQLiteConnection cn = _conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = @"INSERT INTO Reservaciones (IdCliente, IdMesa, FechaHora, NumeroPersonas, Estado) 
                               VALUES (@IdCliente, @IdMesa, @FechaHora, @NumeroPersonas, @Estado); SELECT last_insert_rowid();";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@IdCliente", reservacion.IdCliente);
                    cmd.Parameters.AddWithValue("@IdMesa", reservacion.IdMesa);
                    cmd.Parameters.AddWithValue("@FechaHora", reservacion.FechaHora);
                    cmd.Parameters.AddWithValue("@NumeroPersonas", reservacion.NumeroPersonas);
                    cmd.Parameters.AddWithValue("@Estado", string.IsNullOrWhiteSpace(reservacion.Estado) ? "Pendiente" : reservacion.Estado.Trim());
                    int id = Convert.ToInt32(cmd.ExecuteScalar());
                    reservacion.IdReservacion = id;
                    return id;
                }
            }
        }

        public bool Actualizar(Reservacion reservacion)
        {
            if (reservacion == null) throw new ArgumentNullException(nameof(reservacion));

            using (SQLiteConnection cn = _conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = @"UPDATE Reservaciones 
                               SET IdCliente = @IdCliente, IdMesa = @IdMesa, FechaHora = @FechaHora, 
                                   NumeroPersonas = @NumeroPersonas, Estado = @Estado 
                               WHERE IdReservacion = @IdReservacion";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@IdCliente", reservacion.IdCliente);
                    cmd.Parameters.AddWithValue("@IdMesa", reservacion.IdMesa);
                    cmd.Parameters.AddWithValue("@FechaHora", reservacion.FechaHora);
                    cmd.Parameters.AddWithValue("@NumeroPersonas", reservacion.NumeroPersonas);
                    cmd.Parameters.AddWithValue("@Estado", string.IsNullOrWhiteSpace(reservacion.Estado) ? "Pendiente" : reservacion.Estado.Trim());
                    cmd.Parameters.AddWithValue("@IdReservacion", reservacion.IdReservacion);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Eliminar(int id)
        {
            using (SQLiteConnection cn = _conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = "DELETE FROM Reservaciones WHERE IdReservacion = @Id";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}
