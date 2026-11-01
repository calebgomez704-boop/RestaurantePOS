using System;
using System.Collections.Generic;

namespace RestaurantePOS.Modelos
{
    /// <summary>Reservación de mesa programada para un cliente.</summary>
    public class Reservacion
    {
        private int _idReservacion;
        private int _idCliente;
        private int _idMesa;
        private DateTime _fechaHora = DateTime.Now;
        private int _numeroPersonas = 2;
        private string _estado = "Pendiente";
        private string _nombreCliente = string.Empty;
        private string _numeroMesa = string.Empty;

        public int IdReservacion
        {
            get { return _idReservacion; }
            set { _idReservacion = value; }
        }

        public int IdCliente
        {
            get { return _idCliente; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Debe seleccionar un cliente válido.");
                _idCliente = value;
            }
        }

        public int IdMesa
        {
            get { return _idMesa; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Debe seleccionar una mesa válida.");
                _idMesa = value;
            }
        }

        public DateTime FechaHora
        {
            get { return _fechaHora; }
            set { _fechaHora = value; }
        }

        public int NumeroPersonas
        {
            get { return _numeroPersonas; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("El número de personas debe ser al menos 1.");
                _numeroPersonas = value;
            }
        }

        public string Estado
        {
            get { return _estado; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El estado no puede estar vacío.");
                _estado = value.Trim();
            }
        }

        public string NombreCliente
        {
            get { return _nombreCliente; }
            set { _nombreCliente = value?.Trim() ?? string.Empty; }
        }

        public string NumeroMesa
        {
            get { return _numeroMesa; }
            set { _numeroMesa = value?.Trim() ?? string.Empty; }
        }

        public Reservacion() { }

        public Reservacion(int idReservacion, int idCliente, int idMesa, DateTime fechaHora, int numeroPersonas, string estado)
        {
            this.IdReservacion = idReservacion;
            this.IdCliente = idCliente;
            this.IdMesa = idMesa;
            this.FechaHora = fechaHora;
            this.NumeroPersonas = numeroPersonas;
            this.Estado = estado;
        }

        public List<string> Validar()
        {
            var errores = new List<string>();
            if (IdCliente <= 0)
                errores.Add("Debe seleccionar un cliente válido.");
            if (IdMesa <= 0)
                errores.Add("Debe seleccionar una mesa válida.");
            if (NumeroPersonas <= 0)
                errores.Add("El número de personas debe ser al menos 1.");
            if (string.IsNullOrWhiteSpace(Estado))
                errores.Add("El estado es obligatorio.");
            return errores;
        }

        public override string ToString()
        {
            return $"Reservación #{IdReservacion} - {NombreCliente} (Mesa {NumeroMesa}) {FechaHora:g}";
        }
    }
}
