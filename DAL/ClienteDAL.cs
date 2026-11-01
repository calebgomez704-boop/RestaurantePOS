using System;
using System.Collections.Generic;
using System.Data.SQLite;
using RestaurantePOS.Modelos;

namespace RestaurantePOS.DAL
{
    public class ClienteDAL
    {
        private readonly Conexion _conexion = new Conexion();

        public List<Cliente> Listar()
        {
            var lista = new List<Cliente>();
            using (SQLiteConnection cn = _conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = "SELECT IdCliente, Nombre, Telefono, Email FROM Clientes ORDER BY Nombre";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    using (SQLiteDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            var c = new Cliente
                            {
                                IdCliente = Convert.ToInt32(r.GetValue(0)),
                                Nombre = r.GetString(1),
                                Telefono = r.IsDBNull(2) ? string.Empty : r.GetString(2),
                                Email = r.IsDBNull(3) ? string.Empty : r.GetString(3)
                            };
                            lista.Add(c);
                        }
                    }
                }
            }
            return lista;
        }

        public Cliente BuscarPorId(int id)
        {
            using (SQLiteConnection cn = _conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = "SELECT IdCliente, Nombre, Telefono, Email FROM Clientes WHERE IdCliente = @Id";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (SQLiteDataReader r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            return new Cliente
                            {
                                IdCliente = Convert.ToInt32(r.GetValue(0)),
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

        public int Insertar(Cliente cliente)
        {
            if (cliente == null) throw new ArgumentNullException(nameof(cliente));

            using (SQLiteConnection cn = _conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = @"INSERT INTO Clientes (Nombre, Telefono, Email) 
                               VALUES (@Nombre, @Telefono, @Email); SELECT last_insert_rowid();";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", cliente.Nombre.Trim());
                    cmd.Parameters.AddWithValue("@Telefono", string.IsNullOrWhiteSpace(cliente.Telefono) ? (object)DBNull.Value : cliente.Telefono.Trim());
                    cmd.Parameters.AddWithValue("@Email", string.IsNullOrWhiteSpace(cliente.Email) ? (object)DBNull.Value : cliente.Email.Trim());
                    int id = Convert.ToInt32(cmd.ExecuteScalar());
                    cliente.IdCliente = id;
                    return id;
                }
            }
        }

        public bool Actualizar(Cliente cliente)
        {
            if (cliente == null) throw new ArgumentNullException(nameof(cliente));

            using (SQLiteConnection cn = _conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = @"UPDATE Clientes 
                               SET Nombre = @Nombre, Telefono = @Telefono, Email = @Email 
                               WHERE IdCliente = @IdCliente";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", cliente.Nombre.Trim());
                    cmd.Parameters.AddWithValue("@Telefono", string.IsNullOrWhiteSpace(cliente.Telefono) ? (object)DBNull.Value : cliente.Telefono.Trim());
                    cmd.Parameters.AddWithValue("@Email", string.IsNullOrWhiteSpace(cliente.Email) ? (object)DBNull.Value : cliente.Email.Trim());
                    cmd.Parameters.AddWithValue("@IdCliente", cliente.IdCliente);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Eliminar(int id)
        {
            using (SQLiteConnection cn = _conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = "DELETE FROM Clientes WHERE IdCliente = @Id";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    try
                    {
                        return cmd.ExecuteNonQuery() > 0;
                    }
                    catch (SQLiteException ex) when (ex.ResultCode == SQLiteErrorCode.Constraint)
                    {
                        throw new InvalidOperationException("No se puede eliminar el cliente porque tiene pedidos o reservaciones asociadas.", ex);
                    }
                }
            }
        }
    }
}
