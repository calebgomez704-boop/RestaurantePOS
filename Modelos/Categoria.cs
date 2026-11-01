using System;

namespace RestaurantePOS.Modelos
{
    /// <summary>Categoría o grupo de productos del menú (Bebidas, Postres, etc.).</summary>
    public class Categoria
    {
        private int _idCategoria;
        private string _nombre = string.Empty;
        private string _descripcion = string.Empty;

        public int IdCategoria
        {
            get { return _idCategoria; }
            set { _idCategoria = value; }
        }

        /// <summary>No puede estar vacío ni superar 50 caracteres.</summary>
        public string Nombre
        {
            get { return _nombre; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El nombre de la categoría no puede estar vacío.");
                if (value.Length > 50)
                    throw new ArgumentException("El nombre no puede superar los 50 caracteres.");
                _nombre = value.Trim();
            }
        }

        public string Descripcion
        {
            get { return _descripcion; }
            set { _descripcion = value?.Trim() ?? string.Empty; }
        }

        public Categoria() { }

        public Categoria(int idCategoria, string nombre, string descripcion)
        {
            this.IdCategoria = idCategoria;
            this.Nombre = nombre;
            this.Descripcion = descripcion;
        }

        public override string ToString()
        {
            return Nombre;
        }
    }
}
