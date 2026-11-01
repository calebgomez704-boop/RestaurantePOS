using System;
using System.Collections.Generic;
using System.Data.SQLite;
using RestaurantePOS.Modelos;

namespace RestaurantePOS.DAL
{
    public class InsumoDAL
    {
        private readonly Conexion _conexion = new Conexion();

        public List<Insumo> Listar()
        {
            var lista = new List<Insumo>();
            using (SQLiteConnection cn = _conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = @"
SELECT i.IdInsumo, i.Nombre, i.Unidad, i.Stock, i.IdProveedor, IFNULL(p.Nombre, 'Sin proveedor') AS NombreProveedor
FROM Insumos i
LEFT JOIN Proveedores p ON p.IdProveedor = i.IdProveedor
ORDER BY i.Nombre";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    using (SQLiteDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            var ins = new Insumo
                            {
                                IdInsumo = Convert.ToInt32(r.GetValue(0)),
                                Nombre = r.GetString(1),
                                Unidad = r.GetString(2),
                                Stock = Convert.ToDecimal(r.GetValue(3)),
                                IdProveedor = Convert.ToInt32(r.GetValue(4)),
                                NombreProveedor = r.GetString(5)
                            };
                            lista.Add(ins);
                        }
                    }
                }
            }
            return lista;
        }

        public Insumo BuscarPorId(int id)
        {
            using (SQLiteConnection cn = _conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = @"
SELECT i.IdInsumo, i.Nombre, i.Unidad, i.Stock, i.IdProveedor, IFNULL(p.Nombre, 'Sin proveedor') AS NombreProveedor
FROM Insumos i
LEFT JOIN Proveedores p ON p.IdProveedor = i.IdProveedor
WHERE i.IdInsumo = @Id";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (SQLiteDataReader r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            return new Insumo
                            {
                                IdInsumo = Convert.ToInt32(r.GetValue(0)),
                                Nombre = r.GetString(1),
                                Unidad = r.GetString(2),
                                Stock = Convert.ToDecimal(r.GetValue(3)),
                                IdProveedor = Convert.ToInt32(r.GetValue(4)),
                                NombreProveedor = r.GetString(5)
                            };
                        }
                    }
                }
            }
            return null;
        }

        public int Insertar(Insumo insumo)
        {
            if (insumo == null) throw new ArgumentNullException(nameof(insumo));

            using (SQLiteConnection cn = _conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = @"INSERT INTO Insumos (Nombre, Unidad, Stock, IdProveedor) 
                               VALUES (@Nombre, @Unidad, @Stock, @IdProveedor); SELECT last_insert_rowid();";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", insumo.Nombre.Trim());
                    cmd.Parameters.AddWithValue("@Unidad", insumo.Unidad.Trim());
                    cmd.Parameters.AddWithValue("@Stock", insumo.Stock);
                    cmd.Parameters.AddWithValue("@IdProveedor", insumo.IdProveedor);
                    int id = Convert.ToInt32(cmd.ExecuteScalar());
                    insumo.IdInsumo = id;
                    return id;
                }
            }
        }

        public bool Actualizar(Insumo insumo)
        {
            if (insumo == null) throw new ArgumentNullException(nameof(insumo));

            using (SQLiteConnection cn = _conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = @"UPDATE Insumos 
                               SET Nombre = @Nombre, Unidad = @Unidad, Stock = @Stock, IdProveedor = @IdProveedor 
                               WHERE IdInsumo = @IdInsumo";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", insumo.Nombre.Trim());
                    cmd.Parameters.AddWithValue("@Unidad", insumo.Unidad.Trim());
                    cmd.Parameters.AddWithValue("@Stock", insumo.Stock);
                    cmd.Parameters.AddWithValue("@IdProveedor", insumo.IdProveedor);
                    cmd.Parameters.AddWithValue("@IdInsumo", insumo.IdInsumo);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Eliminar(int id)
        {
            using (SQLiteConnection cn = _conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = "DELETE FROM Insumos WHERE IdInsumo = @Id";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    try
                    {
                        return cmd.ExecuteNonQuery() > 0;
                    }
                    catch (SQLiteException ex) when (ex.ResultCode == SQLiteErrorCode.Constraint)
                    {
                        throw new InvalidOperationException("No se puede eliminar el insumo porque está registrado en una receta.", ex);
                    }
                }
            }
        }
    }
}
