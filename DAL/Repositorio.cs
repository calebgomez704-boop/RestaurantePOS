using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using RestaurantePOS.Modelos;

namespace RestaurantePOS.DAL
{
    /// <summary>
    /// Repositorio de datos: Provee métodos ADO.NET con consultas 100% parametrizadas
    /// y retorno en listas fuertemente tipadas de modelos (clsPedido, Cliente, Empleado, etc.)
    /// para operaciones con LINQ en la capa de interfaz.
    /// </summary>
    public static class Repositorio
    {
        private static readonly Conexion conexion = new Conexion();

        // =========================================================================
        // PEDIDOS
        // =========================================================================

        public static List<clsPedido> ObtenerPedidos(string filtroEstado = null)
        {
            var lista = new List<clsPedido>();
            using (SQLiteConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = @"
SELECT p.IdPedido, p.IdMesa, p.Fecha, p.Total, p.Estado, p.IdCliente, p.IdEmpleado, p.IdMetodoPago,
       c.Nombre  AS NombreCliente,
       e.Nombre  AS NombreMesero,
       m.Numero  AS NumeroMesa,
       mp.Nombre AS NombreMetodoPago
FROM Pedidos p
LEFT JOIN Clientes    c  ON c.IdCliente    = p.IdCliente
LEFT JOIN Empleados   e  ON e.IdEmpleado   = p.IdEmpleado
LEFT JOIN Mesas       m  ON m.IdMesa       = p.IdMesa
LEFT JOIN MetodosPago mp ON mp.IdMetodoPago = p.IdMetodoPago";
                if (!string.IsNullOrWhiteSpace(filtroEstado))
                {
                    sql += " WHERE p.Estado = @Estado";
                }
                sql += " ORDER BY p.IdPedido DESC";

                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    if (!string.IsNullOrWhiteSpace(filtroEstado))
                        cmd.Parameters.AddWithValue("@Estado", filtroEstado);

                    using (SQLiteDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            var p = new clsPedido
                            {
                                IdPedido = Convert.ToInt32(r.GetValue(0)),
                                IdMesa = Convert.ToInt32(r.GetValue(1)),
                                Fecha = Convert.ToDateTime(r.GetValue(2)),
                                Total = Convert.ToDecimal(r.GetValue(3)),
                                Estado = r.GetString(4),
                                IdCliente = r.IsDBNull(5) ? (int?)null : Convert.ToInt32(r.GetValue(5)),
                                IdEmpleado = r.IsDBNull(6) ? (int?)null : Convert.ToInt32(r.GetValue(6)),
                                IdMetodoPago = r.IsDBNull(7) ? (int?)null : Convert.ToInt32(r.GetValue(7)),
                                // Nombres para mostrar (LEFT JOIN: pueden ser NULL en pedidos viejos)
                                NombreCliente = (r.IsDBNull(8) || string.IsNullOrWhiteSpace(r.GetString(8))) ? "Consumidor Final" : r.GetString(8),
                                NombreMesero = (r.IsDBNull(9) || string.IsNullOrWhiteSpace(r.GetString(9))) ? "Sin asignar" : r.GetString(9),
                                NumeroMesa = r.IsDBNull(10) ? "Mesa #" + Convert.ToInt32(r.GetValue(1)) : "Mesa " + Convert.ToInt32(r.GetValue(10)),
                                NombreMetodoPago = r.IsDBNull(11) ? string.Empty : r.GetString(11)
                            };
                            lista.Add(p);
                        }
                    }
                }
            }
            return lista;
        }

        public static int CrearPedido(clsPedido pedido)
        {
            using (SQLiteConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();
                using (SQLiteTransaction tr = cn.BeginTransaction())
                {
                    try
                    {
                        // 1. Insertar Cabecera de Pedido
                        string sqlPedido = @"
INSERT INTO Pedidos (IdMesa, Fecha, Total, Estado, IdCliente, IdEmpleado, IdMetodoPago)
VALUES (@IdMesa, @Fecha, @Total, @Estado, @IdCliente, @IdEmpleado, @IdMetodoPago); SELECT last_insert_rowid();";

                        int nuevoIdPedido = 0;
                        using (SQLiteCommand cmd = new SQLiteCommand(sqlPedido, cn, tr))
                        {
                            cmd.Parameters.AddWithValue("@IdMesa", pedido.IdMesa);
                            cmd.Parameters.AddWithValue("@Fecha", pedido.Fecha);
                            cmd.Parameters.AddWithValue("@Total", pedido.Total);
                            cmd.Parameters.AddWithValue("@Estado", string.IsNullOrWhiteSpace(pedido.Estado) ? "Pendiente" : pedido.Estado);
                            cmd.Parameters.AddWithValue("@IdCliente", pedido.IdCliente.HasValue ? (object)pedido.IdCliente.Value : DBNull.Value);
                            cmd.Parameters.AddWithValue("@IdEmpleado", pedido.IdEmpleado.HasValue ? (object)pedido.IdEmpleado.Value : DBNull.Value);
                            cmd.Parameters.AddWithValue("@IdMetodoPago", pedido.IdMetodoPago.HasValue ? (object)pedido.IdMetodoPago.Value : DBNull.Value);

                            nuevoIdPedido = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        pedido.IdPedido = nuevoIdPedido;

                        // 2. Insertar DetallePedidos
                        string sqlDetalle = @"
INSERT INTO DetallePedidos (IdPedido, IdProducto, Cantidad, PrecioUnitario, IdPromocion, PorcentajeDescuento, MontoDescuento)
VALUES (@IdPedido, @IdProducto, @Cantidad, @PrecioUnitario, @IdPromocion, @PorcentajeDescuento, @MontoDescuento)";

                        foreach (var det in pedido.Detalles)
                        {
                            using (SQLiteCommand cmdDet = new SQLiteCommand(sqlDetalle, cn, tr))
                            {
                                cmdDet.Parameters.AddWithValue("@IdPedido", nuevoIdPedido);
                                cmdDet.Parameters.AddWithValue("@IdProducto", det.IdProducto);
                                cmdDet.Parameters.AddWithValue("@Cantidad", det.Cantidad);
                                cmdDet.Parameters.AddWithValue("@PrecioUnitario", det.PrecioUnitario);
                                cmdDet.Parameters.AddWithValue("@IdPromocion", det.IdPromocion.HasValue ? (object)det.IdPromocion.Value : DBNull.Value);
                                cmdDet.Parameters.AddWithValue("@PorcentajeDescuento", det.PorcentajeDescuento);
                                cmdDet.Parameters.AddWithValue("@MontoDescuento", det.MontoDescuento);
                                cmdDet.ExecuteNonQuery();
                            }
                        }

                        // 3. Actualizar estado de la mesa a 'Ocupada'
                        string sqlMesa = "UPDATE Mesas SET Estado = 'Ocupada' WHERE IdMesa = @IdMesa";
                        using (SQLiteCommand cmdMesa = new SQLiteCommand(sqlMesa, cn, tr))
                        {
                            cmdMesa.Parameters.AddWithValue("@IdMesa", pedido.IdMesa);
                            cmdMesa.ExecuteNonQuery();
                        }

                        tr.Commit();
                        return nuevoIdPedido;
                    }
                    catch
                    {
                        tr.Rollback();
                        throw;
                    }
                }
            }
        }

        public static void CobrarPedido(int idPedido, int idMetodoPago, int idMesa)
        {
            using (SQLiteConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();
                using (SQLiteTransaction tr = cn.BeginTransaction())
                {
                    try
                    {
                        // 1. Actualizar Pedido a 'Pagado' guardando IdMetodoPago (SIN alterar IdEmpleado)
                        string sqlPedido = "UPDATE Pedidos SET Estado = 'Pagado', IdMetodoPago = @IdMetodoPago WHERE IdPedido = @IdPedido";
                        using (SQLiteCommand cmd = new SQLiteCommand(sqlPedido, cn, tr))
                        {
                            cmd.Parameters.AddWithValue("@IdMetodoPago", idMetodoPago);
                            cmd.Parameters.AddWithValue("@IdPedido", idPedido);
                            cmd.ExecuteNonQuery();
                        }

                        // 2. Liberar la mesa a 'Disponible'
                        string sqlMesa = "UPDATE Mesas SET Estado = 'Disponible' WHERE IdMesa = @IdMesa";
                        using (SQLiteCommand cmdMesa = new SQLiteCommand(sqlMesa, cn, tr))
                        {
                            cmdMesa.Parameters.AddWithValue("@IdMesa", idMesa);
                            cmdMesa.ExecuteNonQuery();
                        }

                        tr.Commit();
                    }
                    catch
                    {
                        tr.Rollback();
                        throw;
                    }
                }
            }
        }

        public static List<DetallePedido> ObtenerDetallesPedido(int idPedido)
        {
            var lista = new List<DetallePedido>();
            using (SQLiteConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = @"
SELECT d.IdDetallePedido, d.IdPedido, d.IdProducto, d.Cantidad, d.PrecioUnitario, IFNULL(p.Nombre, '') AS NombreProducto, d.IdPromocion, IFNULL(d.PorcentajeDescuento, 0), IFNULL(d.MontoDescuento, 0)
FROM DetallePedidos d
LEFT JOIN Productos p ON d.IdProducto = p.IdProducto
WHERE d.IdPedido = @IdPedido";

                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@IdPedido", idPedido);
                    using (SQLiteDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            int idProducto = Convert.ToInt32(r.GetValue(2));
                            int cantidad = Convert.ToInt32(r.GetValue(3));
                            decimal precioUnitario = Convert.ToDecimal(r.GetValue(4));
                            string nombreProducto = r.GetString(5);

                            int? idPromocion = r.IsDBNull(6) ? (int?)null : Convert.ToInt32(r.GetValue(6));
                            decimal pctDesc = Convert.ToDecimal(r.GetValue(7));
                            decimal montoDesc = Convert.ToDecimal(r.GetValue(8));

                            var det = new DetallePedido(idProducto, cantidad, precioUnitario, nombreProducto, idPromocion, pctDesc, montoDesc)
                            {
                                IdDetallePedido = Convert.ToInt32(r.GetValue(0)),
                                IdPedido = Convert.ToInt32(r.GetValue(1))
                            };
                            lista.Add(det);
                        }
                    }
                }
            }
            return lista;
        }

        // =========================================================================
        // CLIENTES
        // =========================================================================

        public static List<Cliente> ObtenerClientes()
        {
            var lista = new List<Cliente>();
            using (SQLiteConnection cn = conexion.ObtenerConexion())
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
                                Telefono = r.IsDBNull(2) ? "" : r.GetString(2),
                                Email = r.IsDBNull(3) ? "" : r.GetString(3)
                            };
                            lista.Add(c);
                        }
                    }
                }
            }
            return lista;
        }

        // =========================================================================
        // EMPLEADOS
        // =========================================================================

        public static List<Empleado> ObtenerEmpleados(bool soloActivos = false)
        {
            var lista = new List<Empleado>();
            using (SQLiteConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = "SELECT IdEmpleado, Nombre, Puesto, Telefono, Activo, Usuario, ContrasenaHash, CambiarClave, IntentosFallidos, BloqueadoHasta FROM Empleados";
                if (soloActivos)
                    sql += " WHERE Activo = 1";
                sql += " ORDER BY Nombre";

                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    using (SQLiteDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            var emp = new Empleado
                            {
                                IdEmpleado = Convert.ToInt32(r.GetValue(0)),
                                Nombre = r.GetString(1),
                                Puesto = r.GetString(2),
                                Telefono = r.IsDBNull(3) ? "" : r.GetString(3),
                                Activo = Convert.ToBoolean(r.GetValue(4)),
                                Usuario = r.IsDBNull(5) ? "" : r.GetString(5),
                                ContrasenaHash = r.IsDBNull(6) ? "" : r.GetString(6),
                                CambiarClave = r.IsDBNull(7) ? false : Convert.ToBoolean(r.GetValue(7)),
                                IntentosFallidos = r.IsDBNull(8) ? 0 : Convert.ToInt32(r.GetValue(8)),
                                BloqueadoHasta = r.IsDBNull(9) ? (DateTime?)null : Convert.ToDateTime(r.GetValue(9))
                            };
                            lista.Add(emp);
                        }
                    }
                }
            }
            return lista;
        }

        public static Empleado ObtenerEmpleadoPorUsuario(string usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario)) return null;

            using (SQLiteConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = "SELECT IdEmpleado, Nombre, Puesto, Telefono, Activo, Usuario, ContrasenaHash, CambiarClave, IntentosFallidos, BloqueadoHasta FROM Empleados WHERE Usuario = @Usuario LIMIT 1";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@Usuario", usuario.Trim());
                    using (SQLiteDataReader r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            return new Empleado
                            {
                                IdEmpleado = Convert.ToInt32(r.GetValue(0)),
                                Nombre = r.GetString(1),
                                Puesto = r.GetString(2),
                                Telefono = r.IsDBNull(3) ? "" : r.GetString(3),
                                Activo = Convert.ToBoolean(r.GetValue(4)),
                                Usuario = r.IsDBNull(5) ? "" : r.GetString(5),
                                ContrasenaHash = r.IsDBNull(6) ? "" : r.GetString(6),
                                CambiarClave = r.IsDBNull(7) ? false : Convert.ToBoolean(r.GetValue(7)),
                                IntentosFallidos = r.IsDBNull(8) ? 0 : Convert.ToInt32(r.GetValue(8)),
                                BloqueadoHasta = r.IsDBNull(9) ? (DateTime?)null : Convert.ToDateTime(r.GetValue(9))
                            };
                        }
                    }
                }
            }
            return null;
        }

        public static bool ExisteUsuario(string usuario, int idEmpleadoExcluir = 0)
        {
            using (SQLiteConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = "SELECT COUNT(*) FROM Empleados WHERE Usuario = @Usuario AND IdEmpleado <> @IdExcluir";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@Usuario", usuario.Trim());
                    cmd.Parameters.AddWithValue("@IdExcluir", idEmpleadoExcluir);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        public static int ContarAdministradoresActivos()
        {
            using (SQLiteConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = "SELECT COUNT(*) FROM Empleados WHERE Puesto = 'Administrador' AND Activo = 1";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public static void GuardarEmpleado(Empleado emp, string nuevaClavePlana = null)
        {
            using (SQLiteConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();
                if (emp.IdEmpleado <= 0)
                {
                    // INSERT
                    string hash = !string.IsNullOrWhiteSpace(nuevaClavePlana)
                        ? BCrypt.Net.BCrypt.HashPassword(nuevaClavePlana)
                        : (string.IsNullOrWhiteSpace(emp.ContrasenaHash) ? BCrypt.Net.BCrypt.HashPassword("123456") : emp.ContrasenaHash);

                    string sql = @"
INSERT INTO Empleados (Nombre, Puesto, Telefono, Activo, Usuario, ContrasenaHash, CambiarClave, IntentosFallidos)
VALUES (@Nombre, @Puesto, @Telefono, @Activo, @Usuario, @Hash, @CambiarClave, 0)";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                    {
                        cmd.Parameters.AddWithValue("@Nombre", emp.Nombre.Trim());
                        cmd.Parameters.AddWithValue("@Puesto", emp.Puesto.Trim());
                        cmd.Parameters.AddWithValue("@Telefono", emp.Telefono ?? "");
                        cmd.Parameters.AddWithValue("@Activo", emp.Activo);
                        cmd.Parameters.AddWithValue("@Usuario", emp.Usuario.Trim());
                        cmd.Parameters.AddWithValue("@Hash", hash);
                        cmd.Parameters.AddWithValue("@CambiarClave", emp.CambiarClave ? 1 : 0);
                        cmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    // UPDATE
                    string sql;
                    if (!string.IsNullOrWhiteSpace(nuevaClavePlana))
                    {
                        string hash = BCrypt.Net.BCrypt.HashPassword(nuevaClavePlana);
                        sql = @"
UPDATE Empleados 
SET Nombre = @Nombre, Puesto = @Puesto, Telefono = @Telefono, Activo = @Activo, 
    Usuario = @Usuario, ContrasenaHash = @Hash, CambiarClave = @CambiarClave
WHERE IdEmpleado = @Id";
                        using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                        {
                            cmd.Parameters.AddWithValue("@Nombre", emp.Nombre.Trim());
                            cmd.Parameters.AddWithValue("@Puesto", emp.Puesto.Trim());
                            cmd.Parameters.AddWithValue("@Telefono", emp.Telefono ?? "");
                            cmd.Parameters.AddWithValue("@Activo", emp.Activo);
                            cmd.Parameters.AddWithValue("@Usuario", emp.Usuario.Trim());
                            cmd.Parameters.AddWithValue("@Hash", hash);
                            cmd.Parameters.AddWithValue("@CambiarClave", emp.CambiarClave ? 1 : 0);
                            cmd.Parameters.AddWithValue("@Id", emp.IdEmpleado);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        sql = @"
UPDATE Empleados 
SET Nombre = @Nombre, Puesto = @Puesto, Telefono = @Telefono, Activo = @Activo, Usuario = @Usuario
WHERE IdEmpleado = @Id";
                        using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                        {
                            cmd.Parameters.AddWithValue("@Nombre", emp.Nombre.Trim());
                            cmd.Parameters.AddWithValue("@Puesto", emp.Puesto.Trim());
                            cmd.Parameters.AddWithValue("@Telefono", emp.Telefono ?? "");
                            cmd.Parameters.AddWithValue("@Activo", emp.Activo);
                            cmd.Parameters.AddWithValue("@Usuario", emp.Usuario.Trim());
                            cmd.Parameters.AddWithValue("@Id", emp.IdEmpleado);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        public static void DesactivarEmpleado(int idEmpleado)
        {
            using (SQLiteConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = "UPDATE Empleados SET Activo = 0 WHERE IdEmpleado = @Id";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@Id", idEmpleado);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void ActualizarContrasena(int idEmpleado, string nuevoHash, bool cambiarClave = false)
        {
            using (SQLiteConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = "UPDATE Empleados SET ContrasenaHash = @Hash, CambiarClave = @CambiarClave, IntentosFallidos = 0, BloqueadoHasta = NULL WHERE IdEmpleado = @Id";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@Hash", nuevoHash);
                    cmd.Parameters.AddWithValue("@CambiarClave", cambiarClave ? 1 : 0);
                    cmd.Parameters.AddWithValue("@Id", idEmpleado);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void RegistrarIntentoFallido(int idEmpleado, int intentos, DateTime? bloqueadoHasta)
        {
            using (SQLiteConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = "UPDATE Empleados SET IntentosFallidos = @Intentos, BloqueadoHasta = @Bloqueado WHERE IdEmpleado = @Id";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@Intentos", intentos);
                    cmd.Parameters.AddWithValue("@Bloqueado", bloqueadoHasta.HasValue ? (object)bloqueadoHasta.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@Id", idEmpleado);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void ResetearIntentos(int idEmpleado)
        {
            using (SQLiteConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = "UPDATE Empleados SET IntentosFallidos = 0, BloqueadoHasta = NULL WHERE IdEmpleado = @Id";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@Id", idEmpleado);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // =========================================================================
        // PRODUCTOS, MESAS Y MÉTODOS DE PAGO
        // =========================================================================

        public static List<Producto> ObtenerProductos()
        {
            var lista = new List<Producto>();
            using (SQLiteConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = "SELECT IdProducto, Nombre, IdCategoria, Precio, Stock FROM Productos ORDER BY Nombre";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    using (SQLiteDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            var p = new Producto(Convert.ToInt32(r.GetValue(0)), r.GetString(1), Convert.ToInt32(r.GetValue(2)), Convert.ToDecimal(r.GetValue(3)), Convert.ToInt32(r.GetValue(4)));
                            lista.Add(p);
                        }
                    }
                }
            }
            return lista;
        }

        public static List<Mesa> ObtenerMesas()
        {
            var lista = new List<Mesa>();
            using (SQLiteConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = "SELECT IdMesa, Numero, Capacidad, Estado FROM Mesas ORDER BY Numero";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    using (SQLiteDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            var m = new Mesa(Convert.ToInt32(r.GetValue(0)), Convert.ToInt32(r.GetValue(1)), Convert.ToInt32(r.GetValue(2)), r.GetString(3));
                            lista.Add(m);
                        }
                    }
                }
            }
            return lista;
        }

        public static List<MetodoPago> ObtenerMetodosPago()
        {
            var lista = new List<MetodoPago>();
            using (SQLiteConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = "SELECT IdMetodoPago, Nombre FROM MetodosPago ORDER BY IdMetodoPago";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    using (SQLiteDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            var m = new MetodoPago(Convert.ToInt32(r.GetValue(0)), r.GetString(1));
                            lista.Add(m);
                        }
                    }
                }
            }
            return lista;
        }

        // =========================================================================
        // REPORTES DETALLADOS CON IVA (15%)
        // =========================================================================

        public static List<ItemReporteVenta> ObtenerVentasDetalladasConIVA(DateTime fechaInicio, DateTime fechaFin)
        {
            var lista = new List<ItemReporteVenta>();
            using (SQLiteConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();
                string sql = @"
SELECT p.Fecha, p.IdPedido,
       IFNULL(c.Nombre, 'Consumidor Final') AS NombreCliente,
       IFNULL(e.Nombre, 'Sin asignar') AS NombreMesero,
       IFNULL(mp.Nombre, 'Efectivo') AS NombreMetodoPago,
       IFNULL(pr.Nombre, 'Producto') AS NombreProducto,
       d.PrecioUnitario,
       d.Cantidad,
       IFNULL(d.MontoDescuento, 0) AS MontoDescuento, ((d.PrecioUnitario * d.Cantidad) - IFNULL(d.MontoDescuento, 0)) AS TotalLinea
FROM DetallePedidos d
INNER JOIN Pedidos p ON p.IdPedido = d.IdPedido
LEFT JOIN Clientes c ON c.IdCliente = p.IdCliente
LEFT JOIN Empleados e ON e.IdEmpleado = p.IdEmpleado
LEFT JOIN MetodosPago mp ON mp.IdMetodoPago = p.IdMetodoPago
LEFT JOIN Productos pr ON pr.IdProducto = d.IdProducto
WHERE p.Estado = 'Pagado' AND p.Fecha >= @FechaInicio AND p.Fecha <= @FechaFin
ORDER BY p.Fecha DESC, p.IdPedido DESC";

                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                    cmd.Parameters.AddWithValue("@FechaFin", fechaFin);

                    using (SQLiteDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            decimal precioUnitario = Convert.ToDecimal(r.GetValue(6));
                            int cantidad = Convert.ToInt32(r.GetValue(7));
                            decimal descuento = Convert.ToDecimal(r.GetValue(8));
                            decimal ventaConIva = Convert.ToDecimal(r.GetValue(9));
                            if (ventaConIva < 0) ventaConIva = 0;
                            decimal neto = Math.Round(ventaConIva / (1m + ConfiguracionPOS.TasaIVA), 2);
                            decimal iva = ventaConIva - neto;

                            var item = new ItemReporteVenta
                            {
                                Fecha = Convert.ToDateTime(r.GetValue(0)),
                                NumeroPedido = "#" + Convert.ToInt32(r.GetValue(1)).ToString(),
                                Cliente = r.GetString(2),
                                Vendedor = r.GetString(3),
                                MetodoPago = r.GetString(4),
                                Producto = r.GetString(5),
                                PrecioUnitario = precioUnitario,
                                Cantidad = cantidad,
                                TotalNeto = neto,
                                IVA = iva,
                                VentaConIVA = ventaConIva
                            };
                            lista.Add(item);
                        }
                    }
                }
            }
            return lista;
        }
    }
}
