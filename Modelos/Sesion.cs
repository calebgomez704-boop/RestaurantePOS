using System;

namespace RestaurantePOS.Modelos
{
    /// <summary>
    /// Clase estática 'Sesion': Almacena los datos del empleado actualmente autenticado en el sistema.
    /// Requerido en la PARTE 5 (Punto 18):
    /// "Al iniciar sesión, guardar en una clase estática Sesion: IdEmpleado, Nombre y Puesto. Usar Sesion.IdEmpleado al crear pedidos."
    /// </summary>
    public static class Sesion
    {
        public static int IdEmpleado { get; set; } = 0;
        public static string Nombre { get; set; } = string.Empty;
        public static string Puesto { get; set; } = string.Empty;
        public static string Usuario { get; set; } = string.Empty;

        public static bool EstaAutenticado => IdEmpleado > 0;
        public static bool EsAdministrador => string.Equals(Puesto, "Administrador", StringComparison.OrdinalIgnoreCase);

        public static void Iniciar(int idEmpleado, string nombre, string puesto, string usuario)
        {
            IdEmpleado = idEmpleado;
            Nombre = nombre ?? string.Empty;
            Puesto = puesto ?? string.Empty;
            Usuario = usuario ?? string.Empty;

            // Mantener sincronizado UsuarioActual para compatibilidad
            UsuarioActual.Usuario = new Usuario
            {
                IdUsuario = idEmpleado,
                NombreUsuario = usuario ?? string.Empty,
                Rol = puesto ?? string.Empty
            };
        }

        public static void CerrarSesion()
        {
            IdEmpleado = 0;
            Nombre = string.Empty;
            Puesto = string.Empty;
            Usuario = string.Empty;
            UsuarioActual.Usuario = null;
        }
    }
}
