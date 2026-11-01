using System;
using System.Collections.Generic;

namespace RestaurantePOS.Modelos
{
    /// <summary>Insumo o materia prima utilizada en recetas y cocina.</summary>
    public class Insumo
    {
        private int _idInsumo;
        private string _nombre = string.Empty;
        private string _unidad = string.Empty;
        private decimal _stock;
        private int _idProveedor;
        private string _nombreProveedor = string.Empty;

        public int IdInsumo
        {
            get { return _idInsumo; }
            set { _idInsumo = value; }
        }

        public string Nombre
        {
            get { return _nombre; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El nombre del insumo no puede estar vacío.");
                if (value.Length > 100)
                    throw new ArgumentException("El nombre no puede superar los 100 caracteres.");
                _nombre = value.Trim();
            }
        }

        public string Unidad
        {
            get { return _unidad; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("La unidad de medida es obligatoria.");
                _unidad = value.Trim();
            }
        }

        public decimal Stock
        {
            get { return _stock; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("El stock no puede ser negativo.");
                _stock = value;
            }
        }

        public int IdProveedor
        {
            get { return _idProveedor; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Debe seleccionar un proveedor válido.");
                _idProveedor = value;
            }
        }

        public string NombreProveedor
        {
            get { return _nombreProveedor; }
            set { _nombreProveedor = value?.Trim() ?? string.Empty; }
        }

        public Insumo() { }

        public Insumo(int idInsumo, string nombre, string unidad, decimal stock, int idProveedor)
        {
            this.IdInsumo = idInsumo;
            this.Nombre = nombre;
            this.Unidad = unidad;
            this.Stock = stock;
            this.IdProveedor = idProveedor;
        }

        public List<string> Validar()
        {
            var errores = new List<string>();
            if (string.IsNullOrWhiteSpace(Nombre))
                errores.Add("El nombre del insumo no puede estar vacío.");
            if (string.IsNullOrWhiteSpace(Unidad))
                errores.Add("La unidad de medida es obligatoria.");
            if (Stock < 0)
                errores.Add("El stock no puede ser negativo.");
            if (IdProveedor <= 0)
                errores.Add("Debe seleccionar un proveedor válido.");
            return errores;
        }

        public override string ToString()
        {
            return $"{Nombre} ({Stock} {Unidad})";
        }
    }
}
