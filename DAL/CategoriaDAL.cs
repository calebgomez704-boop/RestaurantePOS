using System;
using System.Collections.Generic;
using System.Data.SQLite;
using RestaurantePOS.Modelos;

namespace RestaurantePOS.DAL
{
    public class CategoriaDAL
    {
        private readonly Conexion _conexion = new Conexion();

        public List<Categoria> Listar()
        {
            var lista = new List<Categoria>();
            using (SQLiteConnection cn = _conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = "SELECT IdCategoria, Nombre, Descripcion FROM Categorias ORDER BY Nombre";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    using (SQLiteDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            var c = new Categoria
                            {
                                IdCategoria = Convert.ToInt32(r.GetValue(0)),
                                Nombre = r.GetString(1),
                                Descripcion = r.IsDBNull(2) ? string.Empty : r.GetString(2)
                            };
                            lista.Add(c);
                        }
                    }
                }
            }
            return lista;
        }

        public Categoria BuscarPorId(int id)
        {
            using (SQLiteConnection cn = _conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = "SELECT IdCategoria, Nombre, Descripcion FROM Categorias WHERE IdCategoria = @Id";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (SQLiteDataReader r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            return new Categoria
                            {
                                IdCategoria = Convert.ToInt32(r.GetValue(0)),
                                Nombre = r.GetString(1),
                                Descripcion = r.IsDBNull(2) ? string.Empty : r.GetString(2)
                            };
                        }
                    }
                }
            }
            return null;
        }

        public int Insertar(Categoria categoria)
        {
            if (categoria == null) throw new ArgumentNullException(nameof(categoria));

            using (SQLiteConnection cn = _conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = @"INSERT INTO Categorias (Nombre, Descripcion) 
                               VALUES (@Nombre, @Descripcion); SELECT last_insert_rowid();";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", categoria.Nombre.Trim());
                    cmd.Parameters.AddWithValue("@Descripcion", string.IsNullOrWhiteSpace(categoria.Descripcion) ? (object)DBNull.Value : categoria.Descripcion.Trim());
                    int id = Convert.ToInt32(cmd.ExecuteScalar());
                    categoria.IdCategoria = id;
                    return id;
                }
            }
        }

        public bool Actualizar(Categoria categoria)
        {
            if (categoria == null) throw new ArgumentNullException(nameof(categoria));

            using (SQLiteConnection cn = _conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = @"UPDATE Categorias 
                               SET Nombre = @Nombre, Descripcion = @Descripcion 
                               WHERE IdCategoria = @IdCategoria";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", categoria.Nombre.Trim());
                    cmd.Parameters.AddWithValue("@Descripcion", string.IsNullOrWhiteSpace(categoria.Descripcion) ? (object)DBNull.Value : categoria.Descripcion.Trim());
                    cmd.Parameters.AddWithValue("@IdCategoria", categoria.IdCategoria);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Eliminar(int id)
        {
            using (SQLiteConnection cn = _conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = "DELETE FROM Categorias WHERE IdCategoria = @Id";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    try
                    {
                        return cmd.ExecuteNonQuery() > 0;
                    }
                    catch (SQLiteException ex) when (ex.ResultCode == SQLiteErrorCode.Constraint)
                    {
                        throw new InvalidOperationException("No se puede eliminar la categoría porque tiene productos u otros registros asociados.", ex);
                    }
                }
            }
        }
    }
}
