using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantePOS.Modelos
{
    /// <summary>
    /// Clase estática 'UsuarioActual': Funciona como una sesión global en memoria.
    /// Al ser 'static', no es necesario crear una instancia (new) de esta clase;
    /// su información está disponible en cualquier parte del sistema mientras la app esté abierta.
    /// </summary>
    public static class UsuarioActual
    {
        /// <summary>
        /// Almacena el objeto 'Usuario' que inició sesión exitosamente en el FormLogin.
        /// Permite que otros formularios (como FormPrincipal) consulten quién está conectado,
        /// su nombre y su rol ("Administrador" o "Mesero") para habilitar o restringir funciones.
        /// </summary>
        public static Usuario Usuario { get; set; }
    }
}


