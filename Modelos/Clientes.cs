using System.Collections.Generic;

namespace RestaurantePOS.Modelos
{
    /// <summary>Cliente del restaurante (datos de contacto).</summary>
    public class Cliente
    {
        private int _idCliente;
        private string _nombre = string.Empty;
        private string _telefono = string.Empty;
        private string _email = string.Empty;

        public int IdCliente
        {
            get { return _idCliente; }
            set { _idCliente = value; }
        }

        public string Nombre
        {
            get { return _nombre; }
            set { _nombre = value; }
        }

        public string Telefono
        {
            get { return _telefono; }
            set { _telefono = value; }
        }

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        public Cliente() { }

        public Cliente(int idCliente, string nombre, string telefono, string email)
        {
            this.IdCliente = idCliente;
            this.Nombre = nombre;
            this.Telefono = telefono;
            this.Email = email;
        }

        /// <summary>Devuelve la lista de errores; vacía si los datos son válidos.</summary>
        public List<string> Validar()
        {
            var errores = new List<string>();

            if (string.IsNullOrWhiteSpace(Nombre))
                errores.Add("El nombre del cliente no puede estar vacío.");
            else if (Nombre.Length > 100)
                errores.Add("El nombre no puede superar los 100 caracteres.");

            if (string.IsNullOrWhiteSpace(Telefono))
                errores.Add("El teléfono es obligatorio.");

            if (!string.IsNullOrWhiteSpace(Email) && !Email.Contains("@"))
                errores.Add("El email no tiene un formato válido.");

            return errores;
        }

        public override string ToString()
        {
            return Nombre;
        }
    }
}
