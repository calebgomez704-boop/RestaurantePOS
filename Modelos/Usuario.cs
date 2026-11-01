using System;
using System.Security.Cryptography;
using System.Text;

namespace RestaurantePOS.Modelos
{
    /// <summary>
    /// Clase 'Usuario': Representa la entidad de un usuario del sistema (como un Administrador o Mesero).
    /// Aplica el principio de Programación Orientada a Objetos (POO) llamado Encapsulamiento,
    /// protegiendo los datos mediante variables privadas y exponiéndolas a través de propiedades con validación.
    /// También incluye la lógica de seguridad para el manejo de contraseñas mediante hashing SHA-256.
    /// </summary>
    public class Usuario
    {
        // ==========================================
        // ATRIBUTOS PRIVADOS (Campos de respaldo)
        // ==========================================
        // Nadie fuera de esta clase puede modificarlos directamente.
        private int idUsuario;
        private string nombreUsuario = string.Empty;
        private string contrasenaHash = string.Empty;

        // ==========================================
        // PROPIEDADES PÚBLICAS
        // ==========================================

        /// <summary>
        /// Identificador único del usuario en la base de datos (Clave Primaria).
        /// </summary>
        public int IdUsuario
        {
            get { return idUsuario; }
            set { idUsuario = value; }
        }

        /// <summary>
        /// Nombre con el que el usuario inicia sesión.
        /// Valida que el texto no esté vacío antes de asignarlo y elimina espacios sobrantes (.Trim()).
        /// </summary>
        public string NombreUsuario
        {
            get { return nombreUsuario; }
            set
            {
                // Validación: No permite valores nulos, vacíos ni con solo espacios en blanco
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El nombre de usuario no puede estar vacío.");
                nombreUsuario = value.Trim();
            }
        }

        /// <summary>
        /// Contraseña encriptada (Hash SHA-256).
        /// Se utiliza para asignar el hash que ya viene guardado desde la base de datos.
        /// </summary>
        public string ContrasenaHash
        {
            get { return contrasenaHash; }
            set { contrasenaHash = value; }
        }

        /// <summary>
        /// Rol o perfil del usuario en el sistema: "Mesero" o "Administrador".
        /// Es fundamental para la seguridad por roles: en FormPrincipal se evalúa esta propiedad
        /// para determinar qué botones y módulos se muestran o se ocultan.
        /// Por defecto se inicializa en "Mesero".
        /// </summary>
        public string Rol { get; set; } = "Mesero";

        // ==========================================
        // CONSTRUCTOR
        // ==========================================
        /// <summary>
        /// Constructor vacío: Permite instanciar un usuario y llenar sus propiedades según sea necesario.
        /// </summary>
        public Usuario() { }

        // ==========================================
        // MÉTODOS DE SEGURIDAD Y NEGOCIO
        // ==========================================

       
        /// Recibe una contraseña escrita en texto plano (ej. "1234"),
        /// valida su longitud mínima y la convierte en un hash SHA-256 antes de guardarla.
        /// NOTA PARA EXPOSICIÓN: Por seguridad nunca se guarda la contraseña real en la base de datos,
        /// solo una huella digital matemática (hash) que no se puede revertir.
       
        /// <param name="contrasenaTextoPlano">Contraseña en texto claro escrita por el usuario.</param>
        public void EstablecerContrasena(string contrasenaTextoPlano)
        {
            if (string.IsNullOrWhiteSpace(contrasenaTextoPlano))
                throw new ArgumentException("La contraseña no puede estar vacía.");
            if (contrasenaTextoPlano.Length < 4)
                throw new ArgumentException("La contraseña debe tener al menos 4 caracteres.");

            // Calcula el hash y lo almacena
            contrasenaHash = CalcularHash(contrasenaTextoPlano);
        }

        /// <summary>
        /// Valida si una contraseña ingresada coincide con la almacenada.
        /// ¿Cómo funciona? Toma el texto plano, le calcula su hash SHA-256 y compara
        /// si ese resultado es idéntico al hash que tenemos guardado en la propiedad ContrasenaHash.
        /// </summary>
        /// <param name="contrasenaTextoPlano">La contraseña que el usuario escribió en el login.</param>
        /// <returns>True si coincide; False si es incorrecta.</returns>
        public bool VerificarContrasena(string contrasenaTextoPlano)
        {
            string hashIngresado = CalcularHash(contrasenaTextoPlano);
            return hashIngresado == contrasenaHash;
        }

        /// <summary>
        /// Método privado (Encapsulado): Solo la clase Usuario lo usa internamente.
        /// Convierte un texto plano en su representación hexadecimal SHA-256 de 64 caracteres.
        /// </summary>
        /// <param name="texto">Cadena de texto a procesar.</param>
        /// <returns>Cadena en formato hexadecimal con el hash SHA-256.</returns>
        private string CalcularHash(string texto)
        {
            // Crea el algoritmo criptográfico SHA-256 dentro de un bloque using para liberar memoria al terminar
            using (SHA256 sha256 = SHA256.Create())
            {
                // Convierte el texto en un arreglo de bytes utilizando codificación UTF-8
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(texto));
                
                // Construye el texto hexadecimal recorriendo cada byte
                StringBuilder sb = new StringBuilder();
                foreach (byte b in bytes)
                    sb.Append(b.ToString("x2")); // "x2" formatea cada byte como 2 dígitos hexadecimales en minúscula
                
                return sb.ToString();
            }
        }
    }
}