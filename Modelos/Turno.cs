using System;
using System.Collections.Generic;

namespace RestaurantePOS.Modelos
{
    /// <summary>Turno o jornada de trabajo asignada a un empleado.</summary>
    public class Turno
    {
        private int _idTurno;
        private int _idEmpleado;
        private DateTime _fechaInicio = DateTime.Now;
        private DateTime? _fechaFin;
        private string _nombreEmpleado = string.Empty;

        public int IdTurno
        {
            get { return _idTurno; }
            set { _idTurno = value; }
        }

        public int IdEmpleado
        {
            get { return _idEmpleado; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Debe seleccionar un empleado válido.");
                _idEmpleado = value;
            }
        }

        public DateTime FechaInicio
        {
            get { return _fechaInicio; }
            set { _fechaInicio = value; }
        }

        public DateTime? FechaFin
        {
            get { return _fechaFin; }
            set
            {
                if (value.HasValue && value.Value < _fechaInicio)
                    throw new ArgumentException("La fecha de fin no puede ser anterior a la fecha de inicio.");
                _fechaFin = value;
            }
        }

        public string NombreEmpleado
        {
            get { return _nombreEmpleado; }
            set { _nombreEmpleado = value?.Trim() ?? string.Empty; }
        }

        public Turno() { }

        public Turno(int idTurno, int idEmpleado, DateTime fechaInicio, DateTime? fechaFin)
        {
            this.IdTurno = idTurno;
            this.IdEmpleado = idEmpleado;
            this.FechaInicio = fechaInicio;
            this.FechaFin = fechaFin;
        }

        public List<string> Validar()
        {
            var errores = new List<string>();
            if (IdEmpleado <= 0)
                errores.Add("Debe seleccionar un empleado válido.");
            if (FechaFin.HasValue && FechaFin.Value < FechaInicio)
                errores.Add("La fecha de fin no puede ser anterior a la de inicio.");
            return errores;
        }

        public override string ToString()
        {
            string fin = FechaFin.HasValue ? FechaFin.Value.ToString("g") : "En curso";
            return $"Turno #{IdTurno} - {NombreEmpleado} ({FechaInicio:g} - {fin})";
        }
    }
}
