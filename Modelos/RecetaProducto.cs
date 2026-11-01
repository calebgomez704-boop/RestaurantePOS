using System;
using System.Collections.Generic;

namespace RestaurantePOS.Modelos
{
    /// <summary>Composición de insumos requeridos para elaborar un producto del menú.</summary>
    public class RecetaProducto
    {
        private int _idRecetaProducto;
        private int _idProducto;
        private int _idInsumo;
        private decimal _cantidadNecesaria;
        private string _nombreProducto = string.Empty;
        private string _nombreInsumo = string.Empty;
        private string _unidadInsumo = string.Empty;

        public int IdRecetaProducto
        {
            get { return _idRecetaProducto; }
            set { _idRecetaProducto = value; }
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

        public int IdInsumo
        {
            get { return _idInsumo; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Debe seleccionar un insumo válido.");
                _idInsumo = value;
            }
        }

        public decimal CantidadNecesaria
        {
            get { return _cantidadNecesaria; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("La cantidad necesaria debe ser mayor a 0.");
                _cantidadNecesaria = value;
            }
        }

        public string NombreProducto
        {
            get { return _nombreProducto; }
            set { _nombreProducto = value?.Trim() ?? string.Empty; }
        }

        public string NombreInsumo
        {
            get { return _nombreInsumo; }
            set { _nombreInsumo = value?.Trim() ?? string.Empty; }
        }

        public string UnidadInsumo
        {
            get { return _unidadInsumo; }
            set { _unidadInsumo = value?.Trim() ?? string.Empty; }
        }

        public RecetaProducto() { }

        public RecetaProducto(int idRecetaProducto, int idProducto, int idInsumo, decimal cantidadNecesaria)
        {
            this.IdRecetaProducto = idRecetaProducto;
            this.IdProducto = idProducto;
            this.IdInsumo = idInsumo;
            this.CantidadNecesaria = cantidadNecesaria;
        }

        public List<string> Validar()
        {
            var errores = new List<string>();
            if (IdProducto <= 0)
                errores.Add("Debe seleccionar un producto válido.");
            if (IdInsumo <= 0)
                errores.Add("Debe seleccionar un insumo válido.");
            if (CantidadNecesaria <= 0)
                errores.Add("La cantidad requerida debe ser mayor a 0.");
            return errores;
        }

        public override string ToString()
        {
            return $"{NombreProducto} - {CantidadNecesaria} {UnidadInsumo} de {NombreInsumo}";
        }
    }
}
