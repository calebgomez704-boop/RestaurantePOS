using System.Collections.Generic;

namespace RestaurantePOS.Modelos
{
    /// <summary>Forma de cobro disponible (Efectivo, Tarjeta, Transferencia...).</summary>
    public class MetodoPago
    {
        private int _idMetodoPago;
        private string _nombre = string.Empty;

        public int IdMetodoPago
        {
            get { return _idMetodoPago; }
            set { _idMetodoPago = value; }
        }

        public string Nombre
        {
            get { return _nombre; }
            set { _nombre = value; }
        }

        public MetodoPago() { }

        public MetodoPago(int idMetodoPago, string nombre)
        {
            this.IdMetodoPago = idMetodoPago;
            this.Nombre = nombre;
        }

        public List<string> Validar()
        {
            var errores = new List<string>();

            if (string.IsNullOrWhiteSpace(Nombre))
                errores.Add("El nombre del método de pago no puede estar vacío.");
            else if (Nombre.Length > 50)
                errores.Add("El nombre no puede superar los 50 caracteres.");

            return errores;
        }

        public override string ToString()
        {
            return Nombre;
        }
    }
}
