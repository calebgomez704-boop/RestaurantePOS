using System;
using System.Data.SQLite;

namespace RestaurantePOS.DAL
{
    /// <summary>
    /// Crea la base de datos SQLite local (si no existe) con todas las tablas y,
    /// SOLO la primera vez, los datos iniciales. Nunca borra ni recrea una base existente
    /// y nunca restablece contrasenas de usuarios existentes.
    /// </summary>
    public static class DatabaseInitializer
    {
        // Version del esquema/siembra guardada en PRAGMA user_version.
        private const int VersionSiembra = 1;

        public static void Inicializar()
        {
            Conexion conexion = new Conexion();
            using (SQLiteConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();

                using (SQLiteTransaction tr = cn.BeginTransaction())
                {
                    EjecutarNonQuery(cn, tr, SqlEsquema);

                    bool sembrar = ObtenerUserVersion(cn, tr) < VersionSiembra
                                   && ContarFilas(cn, tr, "Empleados") == 0;

                    if (sembrar)
                    {
                        SembrarDatosIniciales(cn, tr);
                    }

                    if (ObtenerUserVersion(cn, tr) < VersionSiembra)
                    {
                        EjecutarNonQuery(cn, tr, "PRAGMA user_version = " + VersionSiembra + ";");
                    }

                    tr.Commit();
                }
            }
        }

        // =====================================================================
        // ESQUEMA
        // =====================================================================
        private const string SqlEsquema = @"
CREATE TABLE IF NOT EXISTS Categorias (
    IdCategoria INTEGER PRIMARY KEY AUTOINCREMENT,
    Nombre      TEXT NOT NULL,
    Descripcion TEXT NULL
);

CREATE TABLE IF NOT EXISTS Productos (
    IdProducto  INTEGER PRIMARY KEY AUTOINCREMENT,
    Nombre      TEXT NOT NULL,
    IdCategoria INTEGER NOT NULL,
    Precio      DECIMAL(18,2) NOT NULL DEFAULT 0,
    Stock       INTEGER NOT NULL DEFAULT 0,
    FOREIGN KEY (IdCategoria) REFERENCES Categorias(IdCategoria)
);

CREATE TABLE IF NOT EXISTS Clientes (
    IdCliente INTEGER PRIMARY KEY AUTOINCREMENT,
    Nombre    TEXT NOT NULL,
    Telefono  TEXT NULL,
    Email     TEXT NULL,
    Activo    INTEGER NOT NULL DEFAULT 1
);

CREATE TABLE IF NOT EXISTS Proveedores (
    IdProveedor INTEGER PRIMARY KEY AUTOINCREMENT,
    Nombre      TEXT NOT NULL,
    Telefono    TEXT NULL,
    Email       TEXT NULL,
    Activo      INTEGER NOT NULL DEFAULT 1
);

CREATE TABLE IF NOT EXISTS Empleados (
    IdEmpleado       INTEGER PRIMARY KEY AUTOINCREMENT,
    Nombre           TEXT NOT NULL,
    Puesto           TEXT NOT NULL,
    Telefono         TEXT NULL,
    Activo           INTEGER NOT NULL DEFAULT 1,
    Usuario          TEXT NULL COLLATE NOCASE UNIQUE,
    ContrasenaHash   TEXT NULL,
    CambiarClave     INTEGER NOT NULL DEFAULT 0,
    IntentosFallidos INTEGER NOT NULL DEFAULT 0,
    BloqueadoHasta   TEXT NULL
);

CREATE TABLE IF NOT EXISTS Mesas (
    IdMesa    INTEGER PRIMARY KEY AUTOINCREMENT,
    Numero    INTEGER NOT NULL,
    Capacidad INTEGER NOT NULL DEFAULT 4,
    Estado    TEXT NOT NULL DEFAULT 'Disponible'
);

CREATE TABLE IF NOT EXISTS MetodosPago (
    IdMetodoPago INTEGER PRIMARY KEY AUTOINCREMENT,
    Nombre       TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS Promociones (
    IdPromocion INTEGER PRIMARY KEY AUTOINCREMENT,
    Nombre      TEXT NOT NULL,
    Descripcion TEXT NULL,
    Descuento   DECIMAL(5,2) NOT NULL DEFAULT 0,
    FechaInicio TEXT NOT NULL,
    FechaFin    TEXT NOT NULL,
    IdProducto  INTEGER NULL,
    Activa      INTEGER NOT NULL DEFAULT 1,
    FOREIGN KEY (IdProducto) REFERENCES Productos(IdProducto)
);

CREATE TABLE IF NOT EXISTS Pedidos (
    IdPedido     INTEGER PRIMARY KEY AUTOINCREMENT,
    IdMesa       INTEGER NOT NULL,
    Fecha        TEXT NOT NULL DEFAULT (datetime('now','localtime')),
    Total        DECIMAL(18,2) NOT NULL DEFAULT 0,
    Estado       TEXT NOT NULL DEFAULT 'Pendiente',
    IdCliente    INTEGER NULL,
    IdEmpleado   INTEGER NULL,
    IdMetodoPago INTEGER NULL,
    FOREIGN KEY (IdMesa)       REFERENCES Mesas(IdMesa),
    FOREIGN KEY (IdCliente)    REFERENCES Clientes(IdCliente),
    FOREIGN KEY (IdEmpleado)   REFERENCES Empleados(IdEmpleado),
    FOREIGN KEY (IdMetodoPago) REFERENCES MetodosPago(IdMetodoPago)
);

CREATE TABLE IF NOT EXISTS DetallePedidos (
    IdDetallePedido     INTEGER PRIMARY KEY AUTOINCREMENT,
    IdPedido            INTEGER NOT NULL,
    IdProducto          INTEGER NOT NULL,
    Cantidad            INTEGER NOT NULL,
    PrecioUnitario      DECIMAL(18,2) NOT NULL,
    IdPromocion         INTEGER NULL,
    PorcentajeDescuento DECIMAL(5,2) NOT NULL DEFAULT 0,
    MontoDescuento      DECIMAL(18,2) NOT NULL DEFAULT 0,
    FOREIGN KEY (IdPedido)    REFERENCES Pedidos(IdPedido),
    FOREIGN KEY (IdProducto)  REFERENCES Productos(IdProducto),
    FOREIGN KEY (IdPromocion) REFERENCES Promociones(IdPromocion) ON DELETE SET NULL
);

CREATE TABLE IF NOT EXISTS Insumos (
    IdInsumo    INTEGER PRIMARY KEY AUTOINCREMENT,
    Nombre      TEXT NOT NULL,
    Unidad      TEXT NOT NULL,
    Stock       DECIMAL(18,2) NOT NULL DEFAULT 0,
    IdProveedor INTEGER NOT NULL,
    FOREIGN KEY (IdProveedor) REFERENCES Proveedores(IdProveedor)
);

CREATE TABLE IF NOT EXISTS RecetaProductos (
    IdRecetaProducto  INTEGER PRIMARY KEY AUTOINCREMENT,
    IdProducto        INTEGER NOT NULL,
    IdInsumo          INTEGER NOT NULL,
    CantidadNecesaria DECIMAL(18,2) NOT NULL,
    FOREIGN KEY (IdProducto) REFERENCES Productos(IdProducto),
    FOREIGN KEY (IdInsumo)   REFERENCES Insumos(IdInsumo)
);

CREATE TABLE IF NOT EXISTS Reservaciones (
    IdReservacion  INTEGER PRIMARY KEY AUTOINCREMENT,
    IdCliente      INTEGER NOT NULL,
    IdMesa         INTEGER NOT NULL,
    FechaHora      TEXT NOT NULL,
    NumeroPersonas INTEGER NOT NULL DEFAULT 1,
    Estado         TEXT NOT NULL DEFAULT 'Pendiente',
    FOREIGN KEY (IdCliente) REFERENCES Clientes(IdCliente),
    FOREIGN KEY (IdMesa)    REFERENCES Mesas(IdMesa)
);

CREATE TABLE IF NOT EXISTS Turnos (
    IdTurno     INTEGER PRIMARY KEY AUTOINCREMENT,
    IdEmpleado  INTEGER NOT NULL,
    FechaInicio TEXT NOT NULL,
    FechaFin    TEXT NULL,
    FOREIGN KEY (IdEmpleado) REFERENCES Empleados(IdEmpleado)
);

CREATE INDEX IF NOT EXISTS IX_Pedidos_Fecha          ON Pedidos(Fecha);
CREATE INDEX IF NOT EXISTS IX_Pedidos_Estado         ON Pedidos(Estado);
CREATE INDEX IF NOT EXISTS IX_Pedidos_IdMesa         ON Pedidos(IdMesa);
CREATE INDEX IF NOT EXISTS IX_Detalle_IdPedido       ON DetallePedidos(IdPedido);
CREATE INDEX IF NOT EXISTS IX_Productos_IdCategoria  ON Productos(IdCategoria);
CREATE INDEX IF NOT EXISTS IX_Turnos_IdEmpleado      ON Turnos(IdEmpleado);
";

        // =====================================================================
        // DATOS INICIALES (solo primera vez)
        // =====================================================================
        private static void SembrarDatosIniciales(SQLiteConnection cn, SQLiteTransaction tr)
        {
            // Categorias
            EjecutarNonQuery(cn, tr, @"
INSERT INTO Categorias (Nombre, Descripcion) VALUES
('Entradas',       'Sopas, ensaladas y aperitivos'),
('Platos Fuertes', 'Platos principales del menu'),
('Bebidas',        'Jugos, refrescos y aguas'),
('Postres',        'Dulces y helados');");

            // Productos de ejemplo (precios en C$, IVA incluido)
            EjecutarNonQuery(cn, tr, @"
INSERT INTO Productos (Nombre, IdCategoria, Precio, Stock)
SELECT 'Sopa del dia',        IdCategoria,  85, 50 FROM Categorias WHERE Nombre = 'Entradas'
UNION ALL SELECT 'Nachos con queso',    IdCategoria, 120, 50 FROM Categorias WHERE Nombre = 'Entradas'
UNION ALL SELECT 'Ensalada Cesar',      IdCategoria, 130, 50 FROM Categorias WHERE Nombre = 'Entradas'
UNION ALL SELECT 'Pollo a la plancha',  IdCategoria, 220, 50 FROM Categorias WHERE Nombre = 'Platos Fuertes'
UNION ALL SELECT 'Filete de res',       IdCategoria, 320, 50 FROM Categorias WHERE Nombre = 'Platos Fuertes'
UNION ALL SELECT 'Hamburguesa clasica', IdCategoria, 190, 50 FROM Categorias WHERE Nombre = 'Platos Fuertes'
UNION ALL SELECT 'Pasta bolognesa',     IdCategoria, 180, 50 FROM Categorias WHERE Nombre = 'Platos Fuertes'
UNION ALL SELECT 'Agua natural',        IdCategoria,  25, 100 FROM Categorias WHERE Nombre = 'Bebidas'
UNION ALL SELECT 'Refresco',            IdCategoria,  35, 100 FROM Categorias WHERE Nombre = 'Bebidas'
UNION ALL SELECT 'Jugo natural',        IdCategoria,  60, 100 FROM Categorias WHERE Nombre = 'Bebidas'
UNION ALL SELECT 'Cafe',                IdCategoria,  40, 100 FROM Categorias WHERE Nombre = 'Bebidas'
UNION ALL SELECT 'Pastel de chocolate', IdCategoria,  95, 50 FROM Categorias WHERE Nombre = 'Postres'
UNION ALL SELECT 'Helado 2 bolas',      IdCategoria,  70, 50 FROM Categorias WHERE Nombre = 'Postres';");

            // Metodos de pago
            EjecutarNonQuery(cn, tr, @"
INSERT INTO MetodosPago (Nombre) VALUES ('Efectivo'), ('Tarjeta'), ('Transferencia');");

            // 8 mesas numeradas del 1 al 8
            for (int numero = 1; numero <= 8; numero++)
            {
                using (SQLiteCommand cmd = new SQLiteCommand(
                    "INSERT INTO Mesas (Numero, Capacidad, Estado) VALUES (@Numero, 4, 'Disponible')", cn, tr))
                {
                    cmd.Parameters.AddWithValue("@Numero", numero);
                    cmd.ExecuteNonQuery();
                }
            }

            // Unico usuario inicial: admin / Admin123! (debe cambiar la clave al primer ingreso)
            string hash = BCrypt.Net.BCrypt.HashPassword("Admin123!");
            using (SQLiteCommand cmd = new SQLiteCommand(@"
INSERT INTO Empleados (Nombre, Puesto, Telefono, Activo, Usuario, ContrasenaHash, CambiarClave, IntentosFallidos)
VALUES ('Administrador', 'Administrador', '', 1, 'admin', @Hash, 1, 0)", cn, tr))
            {
                cmd.Parameters.AddWithValue("@Hash", hash);
                cmd.ExecuteNonQuery();
            }
        }

        // =====================================================================
        // UTILIDADES
        // =====================================================================
        private static void EjecutarNonQuery(SQLiteConnection cn, SQLiteTransaction tr, string sql)
        {
            using (SQLiteCommand cmd = new SQLiteCommand(sql, cn, tr))
            {
                cmd.ExecuteNonQuery();
            }
        }

        private static int ObtenerUserVersion(SQLiteConnection cn, SQLiteTransaction tr)
        {
            using (SQLiteCommand cmd = new SQLiteCommand("PRAGMA user_version;", cn, tr))
            {
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        private static long ContarFilas(SQLiteConnection cn, SQLiteTransaction tr, string tabla)
        {
            // 'tabla' es siempre una constante interna (no proviene del usuario).
            using (SQLiteCommand cmd = new SQLiteCommand("SELECT COUNT(*) FROM " + tabla, cn, tr))
            {
                return Convert.ToInt64(cmd.ExecuteScalar());
            }
        }
    }
}
