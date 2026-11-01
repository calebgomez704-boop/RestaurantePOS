using System;
using System.Collections.Generic;

namespace RestaurantePOS.Modelos
{
    /// <summary>Descuento o campaña promocional aplicable en el restaurante.</summary>
    public class Promocion
    {
        private int _idPromocion;
        private string _nombre = string.Empty;
        private string _descripcion = string.Empty;
        private decimal _descuento;
        private DateTime _fechaInicio = DateTime.Today;
        private DateTime _fechaFin = DateTime.Today.AddDays(7);
        private int? _idProducto;
        private bool _activa = true;

        public int IdPromocion
        {
            get { return _idPromocion; }
            set { _idPromocion = value; }
        }

        public string Nombre
        {
            get { return _nombre; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El nombre de la promoción no puede estar vacío.");
                if (value.Length > 100)
                    throw new ArgumentException("El nombre no puede superar los 100 caracteres.");
                _nombre = value.Trim();
            }
        }

        public string Descripcion
        {
            get { return _descripcion; }
            set { _descripcion = value?.Trim() ?? string.Empty; }
        }

        public decimal Descuento
        {
            get { return _descuento; }
            set
            {
                if (value < 1 || value > 100)
                    throw new ArgumentException("El descuento debe estar entre 1% y 100%.");
                _descuento = value;
            }
        }

        public DateTime FechaInicio
        {
            get { return _fechaInicio; }
            set { _fechaInicio = value; }
        }

        public DateTime FechaFin
        {
            get { return _fechaFin; }
            set { _fechaFin = value; }
        }

        public int? IdProducto
        {
            get { return _idProducto; }
            set { _idProducto = value; }
        }

        public bool Activa
        {
            get { return _activa; }
            set { _activa = value; }
        }

        public Promocion() { }

        public Promocion(int idPromocion, string nombre, string descripcion, decimal descuento, DateTime fechaInicio, DateTime fechaFin, int? idProducto = null, bool activa = true)
        {
            this.IdPromocion = idPromocion;
            this.Nombre = nombre;
            this.Descripcion = descripcion;
            this.Descuento = descuento;
            this.FechaInicio = fechaInicio;
            this.FechaFin = fechaFin;
            this.IdProducto = idProducto;
            this.Activa = activa;
        }

        public List<string> Validar()
        {
            var errores = new List<string>();
            if (string.IsNullOrWhiteSpace(Nombre))
                errores.Add("El nombre de la promoción no puede estar vacío.");
            if (Descuento < 1 || Descuento > 100)
                errores.Add("El descuento debe ser un porcentaje entre 1 y 100.");
            if (FechaFin < FechaInicio)
                errores.Add("La fecha fin no puede ser menor que la fecha de inicio.");
            return errores;
        }

        public override string ToString()
        {
            return $"{Nombre} ({Descuento}% desc.)";
        }
    }
}
