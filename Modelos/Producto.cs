using System;

namespace RestaurantePOS.Modelos
{
    /// <summary>Platillo, bebida o artículo del menú, con control básico de inventario.</summary>
    public class Producto
    {
        private int _idProducto;
        private string _nombre = string.Empty;
        private int _idCategoria;
        private decimal _precio;
        private int _stock;

        public int IdProducto
        {
            get { return _idProducto; }
            set { _idProducto = value; }
        }

        public string Nombre
        {
            get { return _nombre; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El nombre del producto no puede estar vacío.");
                if (value.Length > 100)
                    throw new ArgumentException("El nombre no puede superar los 100 caracteres.");
                _nombre = value.Trim();
            }
        }

        public int IdCategoria
        {
            get { return _idCategoria; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Debe seleccionar una categoría válida.");
                _idCategoria = value;
            }
        }

        public decimal Precio
        {
            get { return _precio; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("El precio no puede ser negativo.");
                _precio = value;
            }
        }

        public int Stock
        {
            get { return _stock; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("El stock no puede ser negativo.");
                _stock = value;
            }
        }

        public Producto() { }

        public Producto(int idProducto, string nombre, int idCategoria, decimal precio, int stock)
        {
            this.IdProducto = idProducto;
            this.Nombre = nombre;
            this.IdCategoria = idCategoria;
            this.Precio = precio;
            this.Stock = stock;
        }

        public bool HayStockSuficiente(int cantidadSolicitada)
        {
            return cantidadSolicitada <= _stock;
        }

        public void DescontarStock(int cantidad)
        {
            if (cantidad <= 0)
                throw new ArgumentException("La cantidad a descontar debe ser mayor a cero.");
            if (cantidad > _stock)
                throw new InvalidOperationException("No hay suficiente stock para descontar esa cantidad.");
            _stock -= cantidad;
        }

        public override string ToString()
        {
            return $"{Nombre} - {Precio:C2}";
        }
    }
}
