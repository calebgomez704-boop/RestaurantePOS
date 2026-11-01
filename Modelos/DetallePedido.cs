using System;
using System.Collections.Generic;

namespace RestaurantePOS.Modelos
{
    /// <summary>Renglón de un pedido: producto, cantidad, precio unitario, descuentos y subtotal.</summary>
    public class DetallePedido
    {
        private int _idDetallePedido;
        private int _idPedido;
        private int _idProducto;
        private string _nombreProducto = string.Empty;
        private int _cantidad;
        private decimal _precioUnitario;
        private int? _idPromocion;
        private decimal _porcentajeDescuento;
        private decimal _montoDescuento;

        public int IdDetallePedido
        {
            get { return _idDetallePedido; }
            set { _idDetallePedido = value; }
        }

        public int IdPedido
        {
            get { return _idPedido; }
            set { _idPedido = value; }
        }

        public int IdProducto
        {
            get { return _idProducto; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Debe seleccionar un producto válido.");
                _idProducto = value;
            }
        }

        public string NombreProducto
        {
            get { return _nombreProducto; }
            set { _nombreProducto = value ?? string.Empty; }
        }

        public int Cantidad
        {
            get { return _cantidad; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("La cantidad debe ser mayor a cero.");
                _cantidad = value;
            }
        }

        public decimal PrecioUnitario
        {
            get { return _precioUnitario; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("El precio unitario no puede ser negativo.");
                _precioUnitario = value;
            }
        }

        public int? IdPromocion
        {
            get { return _idPromocion; }
            set { _idPromocion = value; }
        }

        public decimal PorcentajeDescuento
        {
            get { return _porcentajeDescuento; }
            set { _porcentajeDescuento = value; }
        }

        public decimal MontoDescuento
        {
            get { return _montoDescuento; }
            set { _montoDescuento = value; }
        }

        /// <summary>Calculado: (Cantidad x PrecioUnitario) - MontoDescuento.</summary>
        public decimal Subtotal
        {
            get
            {
                decimal sub = (_cantidad * _precioUnitario) - _montoDescuento;
                return sub > 0 ? sub : 0m;
            }
        }

        public DetallePedido() { }

        public DetallePedido(int idProducto, int cantidad, decimal precioUnitario, string nombreProducto = "", int? idPromocion = null, decimal porcentajeDescuento = 0m, decimal montoDescuento = 0m)
        {
            this.IdProducto = idProducto;
            this.Cantidad = cantidad;
            this.PrecioUnitario = precioUnitario;
            this.NombreProducto = nombreProducto;
            this.IdPromocion = idPromocion;
            this.PorcentajeDescuento = porcentajeDescuento;
            this.MontoDescuento = montoDescuento;
        }

        public List<string> Validar()
        {
            var errores = new List<string>();

            if (IdProducto <= 0)
                errores.Add("Debe seleccionar un producto válido.");
            if (Cantidad <= 0)
                errores.Add("La cantidad debe ser mayor a cero.");
            if (PrecioUnitario < 0)
                errores.Add("El precio unitario no puede ser negativo.");

            return errores;
        }

        public override string ToString()
        {
            return Cantidad + " x " + NombreProducto;
        }
    }
}
