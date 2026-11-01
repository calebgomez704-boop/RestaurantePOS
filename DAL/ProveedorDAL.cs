using System;
using System.Collections.Generic;
using System.Data.SQLite;
using RestaurantePOS.Modelos;

namespace RestaurantePOS.DAL
{
    public class ProveedorDAL
    {
        private readonly Conexion _conexion = new Conexion();

        public List<Proveedor> Listar()
        {
            var lista = new List<Proveedor>();
            using (SQLiteConnection cn = _conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = "SELECT IdProveedor, Nombre, Telefono, Email FROM Proveedores ORDER BY Nombre";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    using (SQLiteDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            var p = new Proveedor
                            {
                                IdProveedor = Convert.ToInt32(r.GetValue(0)),
                                Nombre = r.GetString(1),
                                Telefono = r.IsDBNull(2) ? string.Empty : r.GetString(2),
                                Email = r.IsDBNull(3) ? string.Empty : r.GetString(3)
                            };
                            lista.Add(p);
                        }
                    }
                }
            }
            return lista;
        }

        public Proveedor BuscarPorId(int id)
        {
            using (SQLiteConnection cn = _conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = "SELECT IdProveedor, Nombre, Telefono, Email FROM Proveedores WHERE IdProveedor = @Id";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (SQLiteDataReader r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            return new Proveedor
                            {
                                IdProveedor = Convert.ToInt32(r.GetValue(0)),
                                Nombre = r.GetString(1),
                                Telefono = r.IsDBNull(2) ? string.Empty : r.GetString(2),
                                Email = r.IsDBNull(3) ? string.Empty : r.GetString(3)
                            };
                        }
                    }
                }
            }
            return null;
        }

        public int Insertar(Proveedor proveedor)
        {
            if (proveedor == null) throw new ArgumentNullException(nameof(proveedor));

            using (SQLiteConnection cn = _conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = @"INSERT INTO Proveedores (Nombre, Telefono, Email) 
                               VALUES (@Nombre, @Telefono, @Email); SELECT last_insert_rowid();";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", proveedor.Nombre.Trim());
                    cmd.Parameters.AddWithValue("@Telefono", string.IsNullOrWhiteSpace(proveedor.Telefono) ? (object)DBNull.Value : proveedor.Telefono.Trim());
                    cmd.Parameters.AddWithValue("@Email", string.IsNullOrWhiteSpace(proveedor.Email) ? (object)DBNull.Value : proveedor.Email.Trim());
                    int id = Convert.ToInt32(cmd.ExecuteScalar());
                    proveedor.IdProveedor = id;
                    return id;
                }
            }
        }

        public bool Actualizar(Proveedor proveedor)
        {
            if (proveedor == null) throw new ArgumentNullException(nameof(proveedor));

            using (SQLiteConnection cn = _conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = @"UPDATE Proveedores 
                               SET Nombre = @Nombre, Telefono = @Telefono, Email = @Email 
                               WHERE IdProveedor = @IdProveedor";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", proveedor.Nombre.Trim());
                    cmd.Parameters.AddWithValue("@Telefono", string.IsNullOrWhiteSpace(proveedor.Telefono) ? (object)DBNull.Value : proveedor.Telefono.Trim());
                    cmd.Parameters.AddWithValue("@Email", string.IsNullOrWhiteSpace(proveedor.Email) ? (object)DBNull.Value : proveedor.Email.Trim());
                    cmd.Parameters.AddWithValue("@IdProveedor", proveedor.IdProveedor);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Eliminar(int id)
        {
            using (SQLiteConnection cn = _conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = "DELETE FROM Proveedores WHERE IdProveedor = @Id";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    try
                    {
                        return cmd.ExecuteNonQuery() > 0;
                    }
                    catch (SQLiteException ex) when (ex.ResultCode == SQLiteErrorCode.Constraint)
                    {
                        throw new InvalidOperationException("No se puede eliminar el proveedor porque tiene insumos u otros registros asociados.", ex);
                    }
                }
            }
        }
    }
}
