using System;
using System.Collections.Generic;

namespace RestaurantePOS.Modelos
{
    /// <summary>Mesa del restaurante y su estado operativo.</summary>
    public class Mesa
    {
        private int _idMesa;
        private int _numero;
        private int _capacidad;
        private string _estado = "Disponible";

        public int IdMesa
        {
            get { return _idMesa; }
            set { _idMesa = value; }
        }

        public int Numero
        {
            get { return _numero; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("El número de mesa debe ser mayor a 0.");
                _numero = value;
            }
        }

        public int Capacidad
        {
            get { return _capacidad; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("La capacidad de la mesa debe ser al menos de 1 persona.");
                _capacidad = value;
            }
        }

        public string Estado
        {
            get { return _estado; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El estado de la mesa no puede estar vacío.");
                _estado = value.Trim();
            }
        }

        public Mesa() { }

        public Mesa(int idMesa, int numero, int capacidad, string estado)
        {
            this.IdMesa = idMesa;
            this.Numero = numero;
            this.Capacidad = capacidad;
            this.Estado = estado;
        }

        public List<string> Validar()
        {
            var errores = new List<string>();

            if (Numero <= 0)
                errores.Add("El número de mesa debe ser un valor positivo.");
            if (Capacidad <= 0)
                errores.Add("La capacidad debe ser de al menos 1 comensal.");
            if (string.IsNullOrWhiteSpace(Estado))
                errores.Add("El estado de la mesa es requerido.");

            return errores;
        }

        public override string ToString()
        {
            return $"Mesa {Numero} ({Estado} - {Capacidad} pers.)";
        }
    }
}
