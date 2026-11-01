using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace RestaurantePOS.Modelos
{
    /// <summary>
    /// Cabecera de un pedido (mesa, fecha, estado, cliente/empleado/método de pago) con sus renglones.
    /// NombreCliente, NombreMesero, NumeroMesa y NombreMetodoPago son solo para mostrar (grilla y factura).
    /// </summary>
    public class clsPedido
    {
        private int _idPedido;
        private int _idMesa;
        private DateTime _fecha;
        private string _estado = "Pendiente";
        private decimal? _totalPersistido;
        private int? _idCliente;
        private int? _idEmpleado;
        private int? _idMetodoPago;
        private string _nombreCliente = "Consumidor Final";
        private string _nombreMesero = "Sin asignar";
        private string _numeroMesa = string.Empty;
        private string _nombreMetodoPago = string.Empty;
        private BindingList<DetallePedido> _detalles = new BindingList<DetallePedido>();

        public int IdPedido
        {
            get { return _idPedido; }
            set { _idPedido = value; }
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

        public DateTime Fecha
        {
            get { return _fecha; }
            set { _fecha = value; }
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

        public int? IdCliente
        {
            get { return _idCliente; }
            set { _idCliente = value; }
        }

        public int? IdEmpleado
        {
            get { return _idEmpleado; }
            set { _idEmpleado = value; }
        }

        public int? IdMetodoPago
        {
            get { return _idMetodoPago; }
            set { _idMetodoPago = value; }
        }

        /// <summary>Solo para mostrar. "Consumidor Final" si el pedido no tiene cliente.</summary>
        public string NombreCliente
        {
            get { return _nombreCliente; }
            set { _nombreCliente = value; }
        }

        /// <summary>Solo para mostrar. "Sin asignar" si el pedido no tiene empleado.</summary>
        public string NombreMesero
        {
            get { return _nombreMesero; }
            set { _nombreMesero = value; }
        }

        /// <summary>Solo para mostrar (ej. "Mesa 3").</summary>
        public string NumeroMesa
        {
            get { return _numeroMesa; }
            set { _numeroMesa = value; }
        }

        /// <summary>Solo para mostrar. Vacío si aún no se cobra.</summary>
        public string NombreMetodoPago
        {
            get { return _nombreMetodoPago; }
            set { _nombreMetodoPago = value; }
        }

        public BindingList<DetallePedido> Detalles
        {
            get { return _detalles; }
        }

        /// <summary>
        /// Suma de subtotales si hay detalles en memoria; si no, el total guardado en la BD.
        /// </summary>
        public decimal Total
        {
            get
            {
                if (_detalles != null && _detalles.Count > 0)
                    return _detalles.Sum(d => d.Subtotal);
                return _totalPersistido ?? 0m;
            }
            set { _totalPersistido = value; }
        }

        public clsPedido()
        {
            _fecha = DateTime.Now;
        }

        public clsPedido(int idMesa) : this()
        {
            this.IdMesa = idMesa;
        }

        public clsPedido(int idPedido, int idMesa, DateTime fecha, string estado, decimal total,
                         int? idCliente, int? idEmpleado, int? idMetodoPago) : this()
        {
            this.IdPedido = idPedido;
            this.IdMesa = idMesa;
            this.Fecha = fecha;
            this.Estado = estado;
            this.Total = total;
            this.IdCliente = idCliente;
            this.IdEmpleado = idEmpleado;
            this.IdMetodoPago = idMetodoPago;
        }

        public void AgregarDetalle(DetallePedido detalle)
        {
            if (detalle == null)
                throw new ArgumentException("El detalle no puede ser nulo.");
            _detalles.Add(detalle);
        }

        public void QuitarDetalle(DetallePedido detalle)
        {
            _detalles.Remove(detalle);
        }

        public List<string> Validar()
        {
            var errores = new List<string>();

            if (IdMesa <= 0)
                errores.Add("Debe asignar una mesa válida al pedido.");

            if (Detalles == null || Detalles.Count == 0)
                errores.Add("El pedido debe contener al menos un producto.");

            if (Total <= 0)
                errores.Add("El total del pedido debe ser mayor a C$ 0.00.");

            return errores;
        }

        public List<string> ValidarParaCobro(decimal? montoRecibido = null, bool esEfectivo = false)
        {
            var errores = new List<string>();

            if (IdPedido <= 0)
                errores.Add("Debe seleccionar un pedido registrado válido.");

            if (Estado == "Pagado")
                errores.Add("El pedido ya se encuentra pagado.");

            if (IdMesa <= 0)
                errores.Add("El pedido no tiene una mesa asociada válida.");

            if (Total <= 0)
                errores.Add("El total a cobrar debe ser mayor a C$ 0.00.");

            if (!IdMetodoPago.HasValue || IdMetodoPago.Value <= 0)
                errores.Add("Debe seleccionar un método de pago válido.");

            if (esEfectivo)
            {
                if (!montoRecibido.HasValue || montoRecibido.Value < Total)
                    errores.Add(string.Format("El monto recibido ({0:C2}) es insuficiente para cubrir el total de {1:C2}.", montoRecibido ?? 0, Total));
            }

            return errores;
        }

        public override string ToString()
        {
            return "Pedido #" + IdPedido + " - " + Estado + " - " + Total.ToString("N2");
        }
    }

}
