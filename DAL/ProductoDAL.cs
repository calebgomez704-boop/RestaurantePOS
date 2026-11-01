using System;
using System.Collections.Generic;
using System.Data.SQLite;
using RestaurantePOS.Modelos;

namespace RestaurantePOS.DAL
{
    public class ProductoDAL
    {
        private readonly Conexion _conexion = new Conexion();

        public List<Producto> Listar()
        {
            var lista = new List<Producto>();
            using (SQLiteConnection cn = _conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = "SELECT IdProducto, Nombre, IdCategoria, Precio, Stock FROM Productos ORDER BY Nombre";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    using (SQLiteDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            var p = new Producto(
                                Convert.ToInt32(r.GetValue(0)),
                                r.GetString(1),
                                Convert.ToInt32(r.GetValue(2)),
                                Convert.ToDecimal(r.GetValue(3)),
                                Convert.ToInt32(r.GetValue(4))
                            );
                            lista.Add(p);
                        }
                    }
                }
            }
            return lista;
        }

        public Producto BuscarPorId(int id)
        {
            using (SQLiteConnection cn = _conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = "SELECT IdProducto, Nombre, IdCategoria, Precio, Stock FROM Productos WHERE IdProducto = @Id";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (SQLiteDataReader r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            return new Producto(
                                Convert.ToInt32(r.GetValue(0)),
                                r.GetString(1),
                                Convert.ToInt32(r.GetValue(2)),
                                Convert.ToDecimal(r.GetValue(3)),
                                Convert.ToInt32(r.GetValue(4))
                            );
                        }
                    }
                }
            }
            return null;
        }

        public int Insertar(Producto producto)
        {
            if (producto == null) throw new ArgumentNullException(nameof(producto));

            using (SQLiteConnection cn = _conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = @"INSERT INTO Productos (Nombre, IdCategoria, Precio, Stock) 
                               VALUES (@Nombre, @IdCategoria, @Precio, @Stock); SELECT last_insert_rowid();";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", producto.Nombre.Trim());
                    cmd.Parameters.AddWithValue("@IdCategoria", producto.IdCategoria);
                    cmd.Parameters.AddWithValue("@Precio", producto.Precio);
                    cmd.Parameters.AddWithValue("@Stock", producto.Stock);
                    int id = Convert.ToInt32(cmd.ExecuteScalar());
                    producto.IdProducto = id;
                    return id;
                }
            }
        }

        public bool Actualizar(Producto producto)
        {
            if (producto == null) throw new ArgumentNullException(nameof(producto));

            using (SQLiteConnection cn = _conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = @"UPDATE Productos 
                               SET Nombre = @Nombre, IdCategoria = @IdCategoria, Precio = @Precio, Stock = @Stock 
                               WHERE IdProducto = @IdProducto";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", producto.Nombre.Trim());
                    cmd.Parameters.AddWithValue("@IdCategoria", producto.IdCategoria);
                    cmd.Parameters.AddWithValue("@Precio", producto.Precio);
                    cmd.Parameters.AddWithValue("@Stock", producto.Stock);
                    cmd.Parameters.AddWithValue("@IdProducto", producto.IdProducto);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Eliminar(int id)
        {
            using (SQLiteConnection cn = _conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = "DELETE FROM Productos WHERE IdProducto = @Id";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    try
                    {
                        return cmd.ExecuteNonQuery() > 0;
                    }
                    catch (SQLiteException ex) when (ex.ResultCode == SQLiteErrorCode.Constraint)
                    {
                        throw new InvalidOperationException("No se puede eliminar el producto porque tiene pedidos, ventas o recetas asociadas.", ex);
                    }
                }
            }
        }
    }
}
