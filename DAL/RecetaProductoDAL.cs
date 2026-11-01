using System;
using System.Collections.Generic;
using System.Data.SQLite;
using RestaurantePOS.Modelos;

namespace RestaurantePOS.DAL
{
    public class RecetaProductoDAL
    {
        private readonly Conexion _conexion = new Conexion();

        public List<RecetaProducto> Listar()
        {
            var lista = new List<RecetaProducto>();
            using (SQLiteConnection cn = _conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = @"
SELECT r.IdRecetaProducto, r.IdProducto, r.IdInsumo, r.CantidadNecesaria,
       IFNULL(p.Nombre, 'Sin producto') AS NombreProducto,
       IFNULL(i.Nombre, 'Sin insumo') AS NombreInsumo,
       IFNULL(i.Unidad, '') AS UnidadInsumo
FROM RecetaProductos r
LEFT JOIN Productos p ON p.IdProducto = r.IdProducto
LEFT JOIN Insumos   i ON i.IdInsumo   = r.IdInsumo
ORDER BY p.Nombre, i.Nombre";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    using (SQLiteDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            var rp = new RecetaProducto
                            {
                                IdRecetaProducto = Convert.ToInt32(r.GetValue(0)),
                                IdProducto = Convert.ToInt32(r.GetValue(1)),
                                IdInsumo = Convert.ToInt32(r.GetValue(2)),
                                CantidadNecesaria = Convert.ToDecimal(r.GetValue(3)),
                                NombreProducto = r.GetString(4),
                                NombreInsumo = r.GetString(5),
                                UnidadInsumo = r.GetString(6)
                            };
                            lista.Add(rp);
                        }
                    }
                }
            }
            return lista;
        }

        public RecetaProducto BuscarPorId(int id)
        {
            using (SQLiteConnection cn = _conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = @"
SELECT r.IdRecetaProducto, r.IdProducto, r.IdInsumo, r.CantidadNecesaria,
       IFNULL(p.Nombre, 'Sin producto') AS NombreProducto,
       IFNULL(i.Nombre, 'Sin insumo') AS NombreInsumo,
       IFNULL(i.Unidad, '') AS UnidadInsumo
FROM RecetaProductos r
LEFT JOIN Productos p ON p.IdProducto = r.IdProducto
LEFT JOIN Insumos   i ON i.IdInsumo   = r.IdInsumo
WHERE r.IdRecetaProducto = @Id";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (SQLiteDataReader r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            return new RecetaProducto
                            {
                                IdRecetaProducto = Convert.ToInt32(r.GetValue(0)),
                                IdProducto = Convert.ToInt32(r.GetValue(1)),
                                IdInsumo = Convert.ToInt32(r.GetValue(2)),
                                CantidadNecesaria = Convert.ToDecimal(r.GetValue(3)),
                                NombreProducto = r.GetString(4),
                                NombreInsumo = r.GetString(5),
                                UnidadInsumo = r.GetString(6)
                            };
                        }
                    }
                }
            }
            return null;
        }

        public int Insertar(RecetaProducto receta)
        {
            if (receta == null) throw new ArgumentNullException(nameof(receta));

            using (SQLiteConnection cn = _conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = @"INSERT INTO RecetaProductos (IdProducto, IdInsumo, CantidadNecesaria) 
                               VALUES (@IdProducto, @IdInsumo, @CantidadNecesaria); SELECT last_insert_rowid();";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@IdProducto", receta.IdProducto);
                    cmd.Parameters.AddWithValue("@IdInsumo", receta.IdInsumo);
                    cmd.Parameters.AddWithValue("@CantidadNecesaria", receta.CantidadNecesaria);
                    int id = Convert.ToInt32(cmd.ExecuteScalar());
                    receta.IdRecetaProducto = id;
                    return id;
                }
            }
        }

        public bool Actualizar(RecetaProducto receta)
        {
            if (receta == null) throw new ArgumentNullException(nameof(receta));

            using (SQLiteConnection cn = _conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = @"UPDATE RecetaProductos 
                               SET IdProducto = @IdProducto, IdInsumo = @IdInsumo, CantidadNecesaria = @CantidadNecesaria 
                               WHERE IdRecetaProducto = @IdRecetaProducto";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@IdProducto", receta.IdProducto);
                    cmd.Parameters.AddWithValue("@IdInsumo", receta.IdInsumo);
                    cmd.Parameters.AddWithValue("@CantidadNecesaria", receta.CantidadNecesaria);
                    cmd.Parameters.AddWithValue("@IdRecetaProducto", receta.IdRecetaProducto);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Eliminar(int id)
        {
            using (SQLiteConnection cn = _conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = "DELETE FROM RecetaProductos WHERE IdRecetaProducto = @Id";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}
