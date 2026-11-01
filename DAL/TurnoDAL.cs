using System;
using System.Collections.Generic;
using System.Data.SQLite;
using RestaurantePOS.Modelos;

namespace RestaurantePOS.DAL
{
    public class TurnoDAL
    {
        private readonly Conexion _conexion = new Conexion();

        public List<Turno> Listar()
        {
            var lista = new List<Turno>();
            using (SQLiteConnection cn = _conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = @"
SELECT t.IdTurno, t.IdEmpleado, t.FechaInicio, t.FechaFin,
       IFNULL(e.Nombre, 'Sin empleado') AS NombreEmpleado
FROM Turnos t
LEFT JOIN Empleados e ON e.IdEmpleado = t.IdEmpleado
ORDER BY t.FechaInicio DESC";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    using (SQLiteDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            var turno = new Turno
                            {
                                IdTurno = Convert.ToInt32(r.GetValue(0)),
                                IdEmpleado = Convert.ToInt32(r.GetValue(1)),
                                FechaInicio = Convert.ToDateTime(r.GetValue(2)),
                                FechaFin = r.IsDBNull(3) ? (DateTime?)null : Convert.ToDateTime(r.GetValue(3)),
                                NombreEmpleado = r.GetString(4)
                            };
                            lista.Add(turno);
                        }
                    }
                }
            }
            return lista;
        }

        public Turno BuscarPorId(int id)
        {
            using (SQLiteConnection cn = _conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = @"
SELECT t.IdTurno, t.IdEmpleado, t.FechaInicio, t.FechaFin,
       IFNULL(e.Nombre, 'Sin empleado') AS NombreEmpleado
FROM Turnos t
LEFT JOIN Empleados e ON e.IdEmpleado = t.IdEmpleado
WHERE t.IdTurno = @Id";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (SQLiteDataReader r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            return new Turno
                            {
                                IdTurno = Convert.ToInt32(r.GetValue(0)),
                                IdEmpleado = Convert.ToInt32(r.GetValue(1)),
                                FechaInicio = Convert.ToDateTime(r.GetValue(2)),
                                FechaFin = r.IsDBNull(3) ? (DateTime?)null : Convert.ToDateTime(r.GetValue(3)),
                                NombreEmpleado = r.GetString(4)
                            };
                        }
                    }
                }
            }
            return null;
        }

        public int Insertar(Turno turno)
        {
            if (turno == null) throw new ArgumentNullException(nameof(turno));

            using (SQLiteConnection cn = _conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = @"INSERT INTO Turnos (IdEmpleado, FechaInicio, FechaFin) 
                               VALUES (@IdEmpleado, @FechaInicio, @FechaFin); SELECT last_insert_rowid();";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@IdEmpleado", turno.IdEmpleado);
                    cmd.Parameters.AddWithValue("@FechaInicio", turno.FechaInicio);
                    cmd.Parameters.AddWithValue("@FechaFin", turno.FechaFin.HasValue ? (object)turno.FechaFin.Value : DBNull.Value);
                    int id = Convert.ToInt32(cmd.ExecuteScalar());
                    turno.IdTurno = id;
                    return id;
                }
            }
        }

        public bool Actualizar(Turno turno)
        {
            if (turno == null) throw new ArgumentNullException(nameof(turno));

            using (SQLiteConnection cn = _conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = @"UPDATE Turnos 
                               SET IdEmpleado = @IdEmpleado, FechaInicio = @FechaInicio, FechaFin = @FechaFin 
                               WHERE IdTurno = @IdTurno";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@IdEmpleado", turno.IdEmpleado);
                    cmd.Parameters.AddWithValue("@FechaInicio", turno.FechaInicio);
                    cmd.Parameters.AddWithValue("@FechaFin", turno.FechaFin.HasValue ? (object)turno.FechaFin.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@IdTurno", turno.IdTurno);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Eliminar(int id)
        {
            using (SQLiteConnection cn = _conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = "DELETE FROM Turnos WHERE IdTurno = @Id";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public Turno ObtenerTurnoAbierto(int idEmpleado)
        {
            using (SQLiteConnection cn = _conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = @"SELECT t.IdTurno, t.IdEmpleado, t.FechaInicio, t.FechaFin,
                                      IFNULL(e.Nombre, 'Sin empleado') AS NombreEmpleado
                               FROM Turnos t
                               LEFT JOIN Empleados e ON e.IdEmpleado = t.IdEmpleado
                               WHERE t.IdEmpleado = @IdEmpleado AND t.FechaFin IS NULL
                               ORDER BY t.FechaInicio DESC LIMIT 1";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@IdEmpleado", idEmpleado);
                    using (SQLiteDataReader r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            return new Turno
                            {
                                IdTurno = Convert.ToInt32(r.GetValue(0)),
                                IdEmpleado = Convert.ToInt32(r.GetValue(1)),
                                FechaInicio = Convert.ToDateTime(r.GetValue(2)),
                                FechaFin = null,
                                NombreEmpleado = r.GetString(4)
                            };
                        }
                    }
                }
            }
            return null;
        }

        public (int CantidadPedidos, decimal TotalVendido) ObtenerResumenVentasTurno(int idEmpleado, DateTime inicio, DateTime fin)
        {
            using (SQLiteConnection cn = _conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = @"SELECT COUNT(p.IdPedido) AS CantidadPedidos,
                                      IFNULL(SUM(p.Total), 0) AS TotalVendido
                               FROM Pedidos p
                               WHERE p.IdEmpleado = @IdEmpleado
                                 AND p.Estado = 'Pagado'
                                 AND p.Fecha >= @Inicio
                                 AND p.Fecha <= @Fin";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@IdEmpleado", idEmpleado);
                    cmd.Parameters.AddWithValue("@Inicio", inicio);
                    cmd.Parameters.AddWithValue("@Fin", fin);

                    using (SQLiteDataReader r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            int cant = r.IsDBNull(0) ? 0 : Convert.ToInt32(r.GetValue(0));
                            decimal total = r.IsDBNull(1) ? 0m : Convert.ToDecimal(r.GetValue(1));
                            return (cant, total);
                        }
                    }
                }
            }
            return (0, 0m);
        }
}
}