using System;
using System.Collections.Generic;

namespace RestaurantePOS.Modelos
{
    /// <summary>Miembro del personal (Administrador, Supervisor, Mesero, Cajero, Cocinero).</summary>
    public class Empleado
    {
        private int _idEmpleado;
        private string _nombre = string.Empty;
        private string _puesto = string.Empty;
        private string _telefono = string.Empty;
        private string _usuario = string.Empty;
        private string _contrasenaHash = string.Empty;
        private bool _cambiarClave = true;
        private bool _activo = true;
        private int _intentosFallidos = 0;
        private DateTime? _bloqueadoHasta;

        public int IdEmpleado
        {
            get { return _idEmpleado; }
            set { _idEmpleado = value; }
        }

        public string Nombre
        {
            get { return _nombre; }
            set { _nombre = value; }
        }

        public string Puesto
        {
            get { return _puesto; }
            set { _puesto = value; }
        }

        public string Telefono
        {
            get { return _telefono; }
            set { _telefono = value; }
        }

        /// <summary>Nombre de usuario único para iniciar sesión.</summary>
        public string Usuario
        {
            get { return _usuario; }
            set { _usuario = value; }
        }

        /// <summary>Hash BCrypt de la contraseña. Nunca texto plano.</summary>
        public string ContrasenaHash
        {
            get { return _contrasenaHash; }
            set { _contrasenaHash = value; }
        }

        public bool CambiarClave
        {
            get { return _cambiarClave; }
            set { _cambiarClave = value; }
        }

        public bool Activo
        {
            get { return _activo; }
            set { _activo = value; }
        }

        public int IntentosFallidos
        {
            get { return _intentosFallidos; }
            set { _intentosFallidos = value; }
        }

        public DateTime? BloqueadoHasta
        {
            get { return _bloqueadoHasta; }
            set { _bloqueadoHasta = value; }
        }

        public Empleado() { }

        public Empleado(int idEmpleado, string nombre, string puesto, string telefono, string usuario = "", bool activo = true)
        {
            this.IdEmpleado = idEmpleado;
            this.Nombre = nombre;
            this.Puesto = puesto;
            this.Telefono = telefono;
            this.Usuario = usuario;
            this.Activo = activo;
        }

        public List<string> Validar()
        {
            var errores = new List<string>();

            if (string.IsNullOrWhiteSpace(Nombre))
                errores.Add("El nombre del empleado no puede estar vacío.");
            else if (Nombre.Length > 100)
                errores.Add("El nombre no puede superar los 100 caracteres.");

            var puestosValidos = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "Administrador", "Supervisor", "Mesero", "Cajero", "Cocinero"
            };

            if (string.IsNullOrWhiteSpace(Puesto) || !puestosValidos.Contains(Puesto.Trim()))
                errores.Add("El puesto debe ser uno de los permitidos: Administrador, Supervisor, Mesero, Cajero, Cocinero.");

            if (string.IsNullOrWhiteSpace(Usuario))
                errores.Add("El nombre de usuario es obligatorio.");
            else if (Usuario.Length < 3 || Usuario.Length > 50)
                errores.Add("El usuario debe tener entre 3 y 50 caracteres.");

            return errores;
        }

        public override string ToString()
        {
            return Nombre;
        }
    }
}
