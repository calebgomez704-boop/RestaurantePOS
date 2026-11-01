using System;
using System.Collections.Generic;
using System.Data.SQLite;
using RestaurantePOS.Modelos;

namespace RestaurantePOS.DAL
{
    public class PromocionDAL
    {
        private readonly Conexion _conexion = new Conexion();

        public List<Promocion> Listar()
        {
            var lista = new List<Promocion>();
            using (SQLiteConnection cn = _conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = "SELECT IdPromocion, Nombre, Descripcion, Descuento, FechaInicio, FechaFin, IdProducto, Activa FROM Promociones ORDER BY FechaInicio DESC";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    using (SQLiteDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            var p = new Promocion
                            {
                                IdPromocion = Convert.ToInt32(r.GetValue(0)),
                                Nombre = r.GetString(1),
                                Descripcion = r.IsDBNull(2) ? string.Empty : r.GetString(2),
                                Descuento = Convert.ToDecimal(r.GetValue(3)),
                                FechaInicio = Convert.ToDateTime(r.GetValue(4)),
                                FechaFin = Convert.ToDateTime(r.GetValue(5)),
                                IdProducto = r.IsDBNull(6) ? (int?)null : Convert.ToInt32(r.GetValue(6)),
                                Activa = r.IsDBNull(7) ? true : Convert.ToBoolean(r.GetValue(7))
                            };
                            lista.Add(p);
                        }
                    }
                }
            }
            return lista;
        }

        public Promocion BuscarPorId(int id)
        {
            using (SQLiteConnection cn = _conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = "SELECT IdPromocion, Nombre, Descripcion, Descuento, FechaInicio, FechaFin, IdProducto, Activa FROM Promociones WHERE IdPromocion = @Id";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (SQLiteDataReader r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            return new Promocion
                            {
                                IdPromocion = Convert.ToInt32(r.GetValue(0)),
                                Nombre = r.GetString(1),
                                Descripcion = r.IsDBNull(2) ? string.Empty : r.GetString(2),
                                Descuento = Convert.ToDecimal(r.GetValue(3)),
                                FechaInicio = Convert.ToDateTime(r.GetValue(4)),
                                FechaFin = Convert.ToDateTime(r.GetValue(5)),
                                IdProducto = r.IsDBNull(6) ? (int?)null : Convert.ToInt32(r.GetValue(6)),
                                Activa = r.IsDBNull(7) ? true : Convert.ToBoolean(r.GetValue(7))
                            };
                        }
                    }
                }
            }
            return null;
        }

        public Promocion BuscarPromocionVigentePorProducto(int idProducto, DateTime fecha)
        {
            using (SQLiteConnection cn = _conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = @"SELECT IdPromocion, Nombre, Descripcion, Descuento, FechaInicio, FechaFin, IdProducto, Activa 
                               FROM Promociones 
                               WHERE IdProducto = @IdProducto 
                                 AND Activa = 1 
                                 AND substr(@Fecha, 1, 10) >= substr(FechaInicio, 1, 10) 
                                 AND substr(@Fecha, 1, 10) <= substr(FechaFin, 1, 10)
                               ORDER BY Descuento DESC LIMIT 1";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@IdProducto", idProducto);
                    cmd.Parameters.AddWithValue("@Fecha", fecha.Date);
                    using (SQLiteDataReader r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            return new Promocion
                            {
                                IdPromocion = Convert.ToInt32(r.GetValue(0)),
                                Nombre = r.GetString(1),
                                Descripcion = r.IsDBNull(2) ? string.Empty : r.GetString(2),
                                Descuento = Convert.ToDecimal(r.GetValue(3)),
                                FechaInicio = Convert.ToDateTime(r.GetValue(4)),
                                FechaFin = Convert.ToDateTime(r.GetValue(5)),
                                IdProducto = r.IsDBNull(6) ? (int?)null : Convert.ToInt32(r.GetValue(6)),
                                Activa = r.IsDBNull(7) ? true : Convert.ToBoolean(r.GetValue(7))
                            };
                        }
                    }
                }
            }
            return null;
        }

        public int Insertar(Promocion promocion)
        {
            if (promocion == null) throw new ArgumentNullException(nameof(promocion));

            using (SQLiteConnection cn = _conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = @"INSERT INTO Promociones (Nombre, Descripcion, Descuento, FechaInicio, FechaFin, IdProducto, Activa) 
                               VALUES (@Nombre, @Descripcion, @Descuento, @FechaInicio, @FechaFin, @IdProducto, @Activa); SELECT last_insert_rowid();";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", promocion.Nombre.Trim());
                    cmd.Parameters.AddWithValue("@Descripcion", string.IsNullOrWhiteSpace(promocion.Descripcion) ? (object)DBNull.Value : promocion.Descripcion.Trim());
                    cmd.Parameters.AddWithValue("@Descuento", promocion.Descuento);
                    cmd.Parameters.AddWithValue("@FechaInicio", promocion.FechaInicio);
                    cmd.Parameters.AddWithValue("@FechaFin", promocion.FechaFin);
                    cmd.Parameters.AddWithValue("@IdProducto", promocion.IdProducto.HasValue ? (object)promocion.IdProducto.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@Activa", promocion.Activa);

                    int id = Convert.ToInt32(cmd.ExecuteScalar());
                    promocion.IdPromocion = id;
                    return id;
                }
            }
        }

        public bool Actualizar(Promocion promocion)
        {
            if (promocion == null) throw new ArgumentNullException(nameof(promocion));

            using (SQLiteConnection cn = _conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = @"UPDATE Promociones 
                               SET Nombre = @Nombre, 
                                   Descripcion = @Descripcion, 
                                   Descuento = @Descuento, 
                                   FechaInicio = @FechaInicio, 
                                   FechaFin = @FechaFin,
                                   IdProducto = @IdProducto,
                                   Activa = @Activa 
                               WHERE IdPromocion = @Id";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@Id", promocion.IdPromocion);
                    cmd.Parameters.AddWithValue("@Nombre", promocion.Nombre.Trim());
                    cmd.Parameters.AddWithValue("@Descripcion", string.IsNullOrWhiteSpace(promocion.Descripcion) ? (object)DBNull.Value : promocion.Descripcion.Trim());
                    cmd.Parameters.AddWithValue("@Descuento", promocion.Descuento);
                    cmd.Parameters.AddWithValue("@FechaInicio", promocion.FechaInicio);
                    cmd.Parameters.AddWithValue("@FechaFin", promocion.FechaFin);
                    cmd.Parameters.AddWithValue("@IdProducto", promocion.IdProducto.HasValue ? (object)promocion.IdProducto.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@Activa", promocion.Activa);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Eliminar(int id)
        {
            using (SQLiteConnection cn = _conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = "DELETE FROM Promociones WHERE IdPromocion = @Id";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    try
                    {
                        return cmd.ExecuteNonQuery() > 0;
                    }
                    catch (SQLiteException ex) when (ex.ResultCode == SQLiteErrorCode.Constraint)
                    {
                        throw new InvalidOperationException("No se puede eliminar la promoción porque está referenciada en pedidos históricos.");
                    }
                }
            }
        }
    }
}
