using RestaurantePOS.DAL;
using RestaurantePOS.Modelos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace RestaurantePOS
{
    /// <summary>
    /// Formulario 'FormPedidos': Módulo del Punto de Venta (POS) para toma de órdenes.
    /// Cumple con las especificaciones de PARTE 1 y PARTE 4:
    /// - Almacena las listas tipadas (listaPedidos, listaClientes, listaEmpleados, listaProductos, listaMesas, listaMetodosPago, listaDetalle)
    /// - Asigna IdEmpleado con la sesión del mesero que toma la comanda
    /// - Permite seleccionar Cliente opcional (o NULL para Consumidor Final)
    /// - Muestra el nombre del mesero que atiende la mesa
    /// </summary>
    public partial class FormPedidos : Form
    {
        // ==========================================
        // PARTE 4 (Punto 7): Listas declaradas como campos de la clase
        // ==========================================
        private List<clsPedido> listaPedidos;
        private List<Cliente> listaClientes;
        private List<Empleado> listaEmpleados;
        private List<Producto> listaProductos;
        private List<Mesa> listaMesas;
        private List<MetodoPago> listaMetodosPago;
        private List<DetallePedido> listaDetalle = new List<DetallePedido>();

        // Variables de control de pedido
        private clsPedido pedidoActual;
        private int mesaSeleccionadaInicial = 0;
        private BindingSource bsDetalles = new BindingSource();

        public FormPedidos()
        {
            InitializeComponent();
        }

        public FormPedidos(int mesaIdInicial) : this()
        {
            this.mesaSeleccionadaInicial = mesaIdInicial;
        }

        private void FormPedidos_Load(object sender, EventArgs e)
        {
            // PARTE 5 (Punto 20): Validar permisos
            if (!FormPrincipal.TienePermiso(Sesion.Puesto, "Pedidos"))
            {
                MessageBox.Show("No tiene autorización para tomar pedidos.", "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.BeginInvoke(new Action(() => this.Close()));
                return;
            }

            // ==========================================
            // PARTE 4 (Punto 7): Cargar las listas en el Load
            // ==========================================
            CargarListas();

            // Estilos visuales
            UITheme.AplicarEstiloDataGridView(dgvDetalle);
            UITheme.ConfigurarBotonAccion(btnAgregarDetalle, UITheme.TipoBotonAccion.Agregar, "  Agregar a la Orden");
            UITheme.ConfigurarBotonAccion(btnGuardarPedido, UITheme.TipoBotonAccion.Guardar, "  Guardar y Enviar Pedido");

            lblTotal.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTotal.ForeColor = UITheme.ColorVinoOscuro;

            // PARTE 1 (Punto 4): Mostrar el nombre del mesero que atiende
            string nombreMesero = !string.IsNullOrWhiteSpace(Sesion.Nombre) ? Sesion.Nombre : "Mesero en Turno";
            lblMeseroAtiende.Text = $"Atendido por: {nombreMesero} ({Sesion.Puesto})";

            // Iniciar nuevo pedido
            IniciarNuevoPedido();
        }

        private void CargarListas()
        {
            try
            {
                listaPedidos = Repositorio.ObtenerPedidos();
                listaClientes = Repositorio.ObtenerClientes();
                listaEmpleados = Repositorio.ObtenerEmpleados(false);
                listaProductos = Repositorio.ObtenerProductos();
                listaMesas = Repositorio.ObtenerMesas();
                listaMetodosPago = Repositorio.ObtenerMetodosPago();
                listaDetalle = new List<DetallePedido>();

                // 1. Configurar ComboBox de Mesas
                var mesasDisponibles = listaMesas
                    .Where(m => m.Estado == "Disponible" || m.IdMesa == mesaSeleccionadaInicial)
                    .ToList();

                cmbMesa.DataSource = mesasDisponibles;
                cmbMesa.DisplayMember = "Numero";
                cmbMesa.ValueMember = "IdMesa";

                if (mesaSeleccionadaInicial > 0)
                {
                    cmbMesa.SelectedValue = mesaSeleccionadaInicial;
                }

                // 2. Configurar ComboBox de Productos
                cmbProducto.DataSource = listaProductos;
                cmbProducto.DisplayMember = "Nombre";
                cmbProducto.ValueMember = "IdProducto";

                // 3. PARTE 1 (Punto 2): ComboBox opcional de Clientes (con Consumidor Final)
                var comboClientes = new List<Cliente>();
                comboClientes.Add(new Cliente(0, "(Consumidor Final)", "", ""));
                comboClientes.AddRange(listaClientes);

                cmbCliente.DataSource = comboClientes;
                cmbCliente.DisplayMember = "Nombre";
                cmbCliente.ValueMember = "IdCliente";
                cmbCliente.SelectedIndex = 0; // Consumidor final por defecto
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos del formulario de pedidos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void IniciarNuevoPedido()
        {
            pedidoActual = new clsPedido();
            // PARTE 1 (Punto 1): Asignar IdEmpleado con el empleado que inició sesión
            pedidoActual.IdEmpleado = Sesion.IdEmpleado > 0 ? (int?)Sesion.IdEmpleado : (int?)null;
            listaDetalle = new List<DetallePedido>();

            ActualizarGrilla();
        }

        private void ActualizarGrilla()
        {
            bsDetalles.DataSource = pedidoActual.Detalles;
            dgvDetalle.DataSource = bsDetalles;
            bsDetalles.ResetBindings(false);

            if (dgvDetalle.Columns["IdDetallePedido"] != null) dgvDetalle.Columns["IdDetallePedido"].Visible = false;
            if (dgvDetalle.Columns["IdPedido"] != null) dgvDetalle.Columns["IdPedido"].Visible = false;

            if (dgvDetalle.Columns["IdProducto"] != null)
            {
                dgvDetalle.Columns["IdProducto"].HeaderText = "Código";
                dgvDetalle.Columns["IdProducto"].DisplayIndex = 0;
            }
            if (dgvDetalle.Columns["NombreProducto"] != null)
            {
                dgvDetalle.Columns["NombreProducto"].HeaderText = "Producto";
                dgvDetalle.Columns["NombreProducto"].DisplayIndex = 1;
            }
            if (dgvDetalle.Columns["Cantidad"] != null)
            {
                dgvDetalle.Columns["Cantidad"].HeaderText = "Cantidad";
                dgvDetalle.Columns["Cantidad"].DisplayIndex = 2;
            }
            if (dgvDetalle.Columns["PrecioUnitario"] != null)
            {
                dgvDetalle.Columns["PrecioUnitario"].HeaderText = "Precio Unitario";
                dgvDetalle.Columns["PrecioUnitario"].DefaultCellStyle.Format = "C2";
                dgvDetalle.Columns["PrecioUnitario"].DisplayIndex = 3;
            }
            if (dgvDetalle.Columns["IdPromocion"] != null) dgvDetalle.Columns["IdPromocion"].Visible = false;

            if (dgvDetalle.Columns["PorcentajeDescuento"] != null)
            {
                dgvDetalle.Columns["PorcentajeDescuento"].HeaderText = "Desc (%)";
                dgvDetalle.Columns["PorcentajeDescuento"].DefaultCellStyle.Format = "0.##'%'";
                dgvDetalle.Columns["PorcentajeDescuento"].DisplayIndex = 4;
            }
            if (dgvDetalle.Columns["MontoDescuento"] != null)
            {
                dgvDetalle.Columns["MontoDescuento"].HeaderText = "Ahorro";
                dgvDetalle.Columns["MontoDescuento"].DefaultCellStyle.Format = "C2";
                dgvDetalle.Columns["MontoDescuento"].DisplayIndex = 5;
            }
            if (dgvDetalle.Columns["Subtotal"] != null)
            {
                dgvDetalle.Columns["Subtotal"].HeaderText = "Subtotal";
                dgvDetalle.Columns["Subtotal"].DefaultCellStyle.Format = "C2";
                dgvDetalle.Columns["Subtotal"].DisplayIndex = 6;
            }

            lblTotal.Text = $"Total a Pagar: {pedidoActual.Total:C2}";
        }

        private void btnAgregarDetalle_Click(object sender, EventArgs e)
        {
            if (cmbProducto.SelectedItem == null)
            {
                MessageBox.Show("Seleccione un producto del catálogo.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var producto = (Producto)cmbProducto.SelectedItem;
                int cantidad = (int)numCantidad.Value;

                // Validación de stock
                if (!producto.HayStockSuficiente(cantidad))
                {
                    MessageBox.Show($"Stock insuficiente. Solo quedan {producto.Stock} unidades de '{producto.Nombre}'.", "Stock", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Buscar promoción activa vigente hoy para este producto
                int? idPromo = null;
                decimal pctDesc = 0m;
                decimal montoDesc = 0m;

                try
                {
                    var promoDal = new PromocionDAL();
                    var promo = promoDal.BuscarPromocionVigentePorProducto(producto.IdProducto, DateTime.Today);
                    if (promo != null)
                    {
                        var respuesta = MessageBox.Show(
                            $"¡{producto.Nombre} tiene {promo.Descuento:0.##}% de descuento ({promo.Nombre})!\n¿Desea aplicar la promoción a este pedido?",
                            "Promoción Disponible",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        if (respuesta == DialogResult.Yes)
                        {
                            idPromo = promo.IdPromocion;
                            pctDesc = promo.Descuento;
                            decimal subtotalBase = cantidad * producto.Precio;
                            montoDesc = Math.Round(subtotalBase * (pctDesc / 100m), 2);
                        }
                    }
                }
                catch { }

                // Crear detalle con descuento si aplica
                var detalle = new DetallePedido(producto.IdProducto, cantidad, producto.Precio, producto.Nombre, idPromo, pctDesc, montoDesc);

                // Agregar al pedido y al carrito en memoria (listaDetalle)
                pedidoActual.AgregarDetalle(detalle);
                listaDetalle.Add(detalle);

                ActualizarGrilla();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Dato inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnGuardarPedido_Click(object sender, EventArgs e)
        {
            if (cmbMesa.SelectedValue == null)
            {
                MessageBox.Show("Seleccioná una mesa para el pedido.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (pedidoActual.Detalles.Count == 0)
            {
                MessageBox.Show("Agregá al menos un producto a la orden antes de guardar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                pedidoActual.IdMesa = (int)cmbMesa.SelectedValue;

                // PARTE 1 (Punto 1): Asegurar IdEmpleado con el empleado logueado
                if (Sesion.IdEmpleado > 0)
                {
                    pedidoActual.IdEmpleado = Sesion.IdEmpleado;
                }

                // PARTE 1 (Punto 2): Guardar IdCliente seleccionado o NULL si es consumidor final
                int idClienteSeleccionado = cmbCliente.SelectedValue != null ? Convert.ToInt32(cmbCliente.SelectedValue) : 0;
                pedidoActual.IdCliente = idClienteSeleccionado > 0 ? (int?)idClienteSeleccionado : (int?)null;

                pedidoActual.Fecha = DateTime.Now;
                pedidoActual.Estado = "Pendiente";

                // Validaciones de negocio del modelo
                var errores = pedidoActual.Validar();
                if (errores.Count > 0)
                {
                    MessageBox.Show(string.Join("\n", errores), "Validación de Pedido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Guardar en base de datos mediante ADO.NET parametrizado
                int idGenerado = Repositorio.CrearPedido(pedidoActual);

                MessageBox.Show($"Pedido #{idGenerado} guardado y enviado a cocina con éxito.", "Pedido Registrado", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Recargar listas y reiniciar orden
                CargarListas();
                IniciarNuevoPedido();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar el pedido: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
