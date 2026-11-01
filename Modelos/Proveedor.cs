using System.Collections.Generic;

namespace RestaurantePOS.Modelos
{
    /// <summary>Empresa o persona que suministra insumos al restaurante.</summary>
    public class Proveedor
    {
        private int _idProveedor;
        private string _nombre = string.Empty;
        private string _telefono = string.Empty;
        private string _email = string.Empty;

        public int IdProveedor
        {
            get { return _idProveedor; }
            set { _idProveedor = value; }
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

        public Proveedor() { }

        public Proveedor(int idProveedor, string nombre, string telefono, string email)
        {
            this.IdProveedor = idProveedor;
            this.Nombre = nombre;
            this.Telefono = telefono;
            this.Email = email;
        }

        public List<string> Validar()
        {
            var errores = new List<string>();

            if (string.IsNullOrWhiteSpace(Nombre))
                errores.Add("El nombre del proveedor no puede estar vacío.");
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
