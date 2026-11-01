using RestaurantePOS.DAL;
using RestaurantePOS.Modelos;
using RestaurantePOS.Servicios;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace RestaurantePOS.Formularios
{
    /// <summary>
    /// Formulario 'FormCaja': Módulo Operativo de Cobro, Facturación y Liberación de Mesas.
    /// También actúa como monitor de comandas pendientes para el rol Cocinero.
    /// </summary>
    public partial class FormCaja : Form
    {
        private readonly bool modoCocinero = false;
        private clsPedido pedidoSeleccionado = null;

        private List<clsPedido> listaPedidosPendientes = new List<clsPedido>();
        private List<Cliente> listaClientes = new List<Cliente>();
        private List<Empleado> listaEmpleados = new List<Empleado>();
        private List<Mesa> listaMesas = new List<Mesa>();
        private List<MetodoPago> listaMetodosPago = new List<MetodoPago>();

        public FormCaja()
        {
            InitializeComponent();
        }

        public FormCaja(bool modoCocinero) : this()
        {
            this.modoCocinero = modoCocinero;
        }

        private void FormCaja_Load(object sender, EventArgs e)
        {
            // Validar permiso
            if (modoCocinero)
            {
                if (!FormPrincipal.TienePermiso(Sesion.Puesto, "PedidosCocina"))
                {
                    MessageBox.Show("No tiene autorización para ver el módulo de cocina.", "Acceso Restringido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.BeginInvoke(new Action(() => this.Close()));
                    return;
                }

                // Modo Cocinero: ocultar controles de cobro y ajustar títulos
                lblTitulo.Text = "Monitor de Cocina (Comandas Pendientes)";
                lblSubtitulo.Text = "Visualización de platillos y pedidos en preparación";
                pnlCobro.Visible = false;
                splMain.SplitterDistance = this.Width / 2;
            }
            else
            {
                if (!FormPrincipal.TienePermiso(Sesion.Puesto, "Caja"))
                {
                    MessageBox.Show("No tiene autorización para acceder a la Caja.", "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.BeginInvoke(new Action(() => this.Close()));
                    return;
                }
            }

            // Estilos visuales
            UITheme.AplicarEstiloDataGridView(dgvPedidosPendientes);
            UITheme.AplicarEstiloDataGridView(dgvDetalles);

            EstilarBotonCaja(btnCobrar, "Confirmar Cobro y Facturar", UITheme.ColorVinoPrincipal, UITheme.ColorDorado, 11F);
            EstilarBotonCaja(btnDescargarPDF, "Descargar Factura PDF", UITheme.ColorVerdeExito, Color.White, 10F);
            EstilarBotonCaja(btnRefrescar, "Refrescar Lista", UITheme.ColorVinoPrincipal, UITheme.ColorDorado, 9.5F);

            lblTotalPagar.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTotalPagar.ForeColor = UITheme.ColorVinoOscuro;

            lblCambio.Font = new Font("Segoe UI", 11.5F, FontStyle.Bold);
            lblCambio.ForeColor = UITheme.ColorVerdeExito;

            ConfigurarColumnasGridPedidos();
            CargarCatalogos();
            CargarPedidosPendientes();
        }

        private void EstilarBotonCaja(Button btn, string texto, Color backColor, Color foreColor, float fontSize)
        {
            btn.Image = null;
            btn.Text = texto;
            btn.BackColor = backColor;
            btn.ForeColor = foreColor;
            btn.Font = new Font("Segoe UI", fontSize, FontStyle.Bold);
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Cursor = Cursors.Hand;
            btn.TextAlign = ContentAlignment.MiddleCenter;
            btn.TextImageRelation = TextImageRelation.Overlay;
            btn.Padding = new Padding(0);
        }

        private void ConfigurarColumnasGridPedidos()
        {
            dgvPedidosPendientes.Columns.Clear();
            dgvPedidosPendientes.AutoGenerateColumns = false;

            // PARTE 3 (Punto 6): Mostrar nombre de cliente y mesero en lugar de IDs
            dgvPedidosPendientes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "IdPedido",
                HeaderText = "Nº Pedido",
                Width = 70
            });
            dgvPedidosPendientes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "NumeroMesa",
                HeaderText = "Mesa",
                Width = 70
            });
            dgvPedidosPendientes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "NombreCliente",
                HeaderText = "Cliente",
                Width = 110
            });
            dgvPedidosPendientes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "NombreMesero",
                HeaderText = "Mesero",
                Width = 100
            });
            dgvPedidosPendientes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Fecha",
                HeaderText = "Fecha / Hora",
                Width = 110,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "HH:mm:ss" }
            });
            dgvPedidosPendientes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Total",
                HeaderText = "Total",
                Width = 85,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" }
            });
            dgvPedidosPendientes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Estado",
                HeaderText = "Estado",
                Width = 75
            });

            // Columnas de IDs explícitamente ocultas
            var colIdCliente = new DataGridViewTextBoxColumn { DataPropertyName = "IdCliente", Visible = false };
            var colIdEmpleado = new DataGridViewTextBoxColumn { DataPropertyName = "IdEmpleado", Visible = false };
            var colIdMetodo = new DataGridViewTextBoxColumn { DataPropertyName = "IdMetodoPago", Visible = false };
            dgvPedidosPendientes.Columns.Add(colIdCliente);
            dgvPedidosPendientes.Columns.Add(colIdEmpleado);
            dgvPedidosPendientes.Columns.Add(colIdMetodo);
        }

        private void CargarCatalogos()
        {
            try
            {
                listaMetodosPago = Repositorio.ObtenerMetodosPago();
                cmbMetodoPago.DataSource = listaMetodosPago;
                cmbMetodoPago.DisplayMember = "Nombre";
                cmbMetodoPago.ValueMember = "IdMetodoPago";

                listaClientes = Repositorio.ObtenerClientes();
                listaEmpleados = Repositorio.ObtenerEmpleados(false);
                listaMesas = Repositorio.ObtenerMesas();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar catálogos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void CargarPedidosPendientes()
        {
            try
            {
                listaPedidosPendientes = Repositorio.ObtenerPedidos("Pendiente");

                // ==========================================
                // Los nombres (cliente, mesero, mesa, método de pago) ya vienen del Repositorio (LEFT JOIN).

                dgvPedidosPendientes.DataSource = null;
                dgvPedidosPendientes.DataSource = listaPedidosPendientes;

                pedidoSeleccionado = null;
                LimpiarDetalleSeleccionado();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar comandas pendientes: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarDetalleSeleccionado()
        {
            dgvDetalles.DataSource = null;
            lblPedidoInfo.Text = "Ningún pedido seleccionado";
            lblTotalPagar.Text = "Total a Pagar: C$ 0.00";
            txtMontoRecibido.Text = "0.00";
            lblCambio.Text = "Cambio: C$ 0.00";
            btnCobrar.Enabled = false;
        }

        private void dgvPedidosPendientes_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvPedidosPendientes.CurrentRow == null || dgvPedidosPendientes.CurrentRow.DataBoundItem == null)
            {
                LimpiarDetalleSeleccionado();
                return;
            }

            pedidoSeleccionado = (clsPedido)dgvPedidosPendientes.CurrentRow.DataBoundItem;

            lblPedidoInfo.Text = $"Pedido #{pedidoSeleccionado.IdPedido} - {pedidoSeleccionado.NumeroMesa} - Mesero: {pedidoSeleccionado.NombreMesero} - Cliente: {pedidoSeleccionado.NombreCliente}";
            lblTotalPagar.Text = $"Total a Pagar: {pedidoSeleccionado.Total:C2}";
            btnCobrar.Enabled = true;

            // Cargar renglones del pedido
            try
            {
                var detalles = Repositorio.ObtenerDetallesPedido(pedidoSeleccionado.IdPedido);
                dgvDetalles.DataSource = null;
                dgvDetalles.DataSource = detalles;

                if (dgvDetalles.Columns["IdDetallePedido"] != null) dgvDetalles.Columns["IdDetallePedido"].Visible = false;
                if (dgvDetalles.Columns["IdPedido"] != null) dgvDetalles.Columns["IdPedido"].Visible = false;
                if (dgvDetalles.Columns["IdProducto"] != null) dgvDetalles.Columns["IdProducto"].HeaderText = "Cod.";
                if (dgvDetalles.Columns["NombreProducto"] != null) dgvDetalles.Columns["NombreProducto"].HeaderText = "Producto";
                if (dgvDetalles.Columns["Cantidad"] != null) dgvDetalles.Columns["Cantidad"].HeaderText = "Cant.";
                if (dgvDetalles.Columns["PrecioUnitario"] != null)
                {
                    dgvDetalles.Columns["PrecioUnitario"].HeaderText = "Precio Unit.";
                    dgvDetalles.Columns["PrecioUnitario"].DefaultCellStyle.Format = "C2";
                }
                if (dgvDetalles.Columns["Subtotal"] != null)
                {
                    dgvDetalles.Columns["Subtotal"].HeaderText = "Subtotal";
                    dgvDetalles.Columns["Subtotal"].DefaultCellStyle.Format = "C2";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al cargar detalles: " + ex.Message);
            }

            txtMontoRecibido.Text = pedidoSeleccionado.Total.ToString("N2");
            CalcularCambio();
        }

        private void cmbMetodoPago_SelectedIndexChanged(object sender, EventArgs e)
        {
            string nombreMetodo = cmbMetodoPago.Text.ToLower();
            bool esEfectivo = nombreMetodo.Contains("efectivo");

            pnlEfectivo.Visible = esEfectivo;
            CalcularCambio();
        }

        private void txtMontoRecibido_TextChanged(object sender, EventArgs e)
        {
            CalcularCambio();
        }

        private void CalcularCambio()
        {
            if (pedidoSeleccionado == null) return;

            if (decimal.TryParse(txtMontoRecibido.Text, out decimal recibido))
            {
                decimal cambio = recibido - pedidoSeleccionado.Total;
                if (cambio >= 0)
                {
                    lblCambio.Text = $"Cambio: {cambio:C2}";
                    lblCambio.ForeColor = UITheme.ColorVerdeExito;
                }
                else
                {
                    lblCambio.Text = $"Faltante: {Math.Abs(cambio):C2}";
                    lblCambio.ForeColor = UITheme.ColorRojoPeligro;
                }
            }
            else
            {
                lblCambio.Text = "Monto inválido";
                lblCambio.ForeColor = UITheme.ColorRojoPeligro;
            }
        }

        private void btnCobrar_Click(object sender, EventArgs e)
        {
            if (pedidoSeleccionado == null)
            {
                MessageBox.Show("Seleccione un pedido pendiente de la lista.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbMetodoPago.SelectedValue == null)
            {
                MessageBox.Show("Seleccione un método de pago.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idMetodoPago = (int)cmbMetodoPago.SelectedValue;
            string nombreMetodo = cmbMetodoPago.Text;
            bool esEfectivo = nombreMetodo.ToLower().Contains("efectivo");
            decimal montoRecibido = 0;

            if (esEfectivo)
            {
                if (!decimal.TryParse(txtMontoRecibido.Text, out montoRecibido) || montoRecibido < pedidoSeleccionado.Total)
                {
                    MessageBox.Show("El monto recibido no es suficiente para cubrir el total del pedido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            else
            {
                montoRecibido = pedidoSeleccionado.Total;
            }

            // Validar negocio
            pedidoSeleccionado.IdMetodoPago = idMetodoPago;
            var errores = pedidoSeleccionado.ValidarParaCobro(montoRecibido, esEfectivo);
            if (errores.Count > 0)
            {
                MessageBox.Show(string.Join("\n", errores), "Validación de Cobro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show($"¿Confirmar el cobro de {pedidoSeleccionado.Total:C2} con {nombreMetodo} y liberar la mesa?",
                                          "Confirmar Cobro", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                // ==========================================
                // PARTE 2 (Punto 5): La factura debe leer Mesero, Cliente y Método de Pago
                // DESDE EL PEDIDO (con joins/relaciones), NO desde el usuario logueado ni texto fijo.
                // Si IdCliente es NULL, mostrar "Consumidor Final". Pedidos con NULL no deben romperse.
                // ==========================================
                var datosFac = new TicketService.DatosFactura
                {
                    IdPedido = pedidoSeleccionado.IdPedido,
                    FechaHora = DateTime.Now,
                    MetodoPago = nombreMetodo,
                    Total = pedidoSeleccionado.Total,
                    MontoRecibido = esEfectivo ? montoRecibido : (decimal?)null,
                    Cambio = esEfectivo ? (montoRecibido - pedidoSeleccionado.Total) : (decimal?)null,
                    Cliente = !string.IsNullOrWhiteSpace(pedidoSeleccionado.NombreCliente) ? pedidoSeleccionado.NombreCliente : "Consumidor Final",
                    Mesero = !string.IsNullOrWhiteSpace(pedidoSeleccionado.NombreMesero) ? pedidoSeleccionado.NombreMesero : "Sin asignar"
                };

                // Mesa
                var mesaObj = listaMesas.FirstOrDefault(m => m.IdMesa == pedidoSeleccionado.IdMesa);
                datosFac.NumeroMesa = mesaObj != null ? mesaObj.Numero : pedidoSeleccionado.IdMesa;

                // Renglones de la factura
                var renglones = Repositorio.ObtenerDetallesPedido(pedidoSeleccionado.IdPedido);
                foreach (var d in renglones)
                {
                    datosFac.Items.Add(new TicketService.ItemFactura
                    {
                        Producto = !string.IsNullOrWhiteSpace(d.NombreProducto) ? d.NombreProducto : $"Producto #{d.IdProducto}",
                        Cantidad = d.Cantidad,
                        PrecioUnitario = d.PrecioUnitario,
                        Descuento = d.MontoDescuento
                    });
                }

                // ==========================================
                // PARTE 1 (Punto 3): Al pagar, guardar IdMetodoPago junto con el cambio de Estado a 'Pagado'
                // ==========================================
                Repositorio.CobrarPedido(pedidoSeleccionado.IdPedido, idMetodoPago, pedidoSeleccionado.IdMesa);

                MessageBox.Show("Pago registrado exitosamente. La mesa ha sido liberada a estado 'Disponible'.", "Cobro Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Mostrar vista previa / ticket
                TicketService.MostrarVistaPrevia(datosFac, this);
                ExportadorService.GenerarFacturaPDF(datosFac, this);

                // Refrescar lista
                CargarPedidosPendientes();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al procesar el cobro: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefrescar_Click(object sender, EventArgs e)
        {
            CargarCatalogos();
            CargarPedidosPendientes();
        }

        private void btnDescargarPDF_Click(object sender, EventArgs e)
        {
            if (pedidoSeleccionado == null)
            {
                MessageBox.Show("Seleccione un pedido de la lista para generar el PDF.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var datosFac = ConstruirDatosFacturaActual();
            ExportadorService.GenerarFacturaPDF(datosFac, this);
        }

        private TicketService.DatosFactura ConstruirDatosFacturaActual()
        {
            if (pedidoSeleccionado == null) return null;

            int idMetodoPago = cmbMetodoPago.SelectedValue != null ? Convert.ToInt32(cmbMetodoPago.SelectedValue) : 1;
            string nombreMetodo = cmbMetodoPago.Text;
            bool esEfectivo = nombreMetodo.IndexOf("Efectivo", StringComparison.OrdinalIgnoreCase) >= 0;

            decimal? montoRecibido = null;
            if (decimal.TryParse(txtMontoRecibido.Text.Trim(), out decimal mr))
            {
                montoRecibido = mr;
            }

            var datosFac = new TicketService.DatosFactura
            {
                IdPedido = pedidoSeleccionado.IdPedido,
                FechaHora = pedidoSeleccionado.Fecha != default(DateTime) ? pedidoSeleccionado.Fecha : DateTime.Now,
                MetodoPago = !string.IsNullOrWhiteSpace(pedidoSeleccionado.NombreMetodoPago) ? pedidoSeleccionado.NombreMetodoPago : nombreMetodo,
                Total = pedidoSeleccionado.Total,
                MontoRecibido = esEfectivo ? montoRecibido : (decimal?)null,
                Cambio = esEfectivo && montoRecibido.HasValue ? (montoRecibido - pedidoSeleccionado.Total) : (decimal?)null,
                Cliente = !string.IsNullOrWhiteSpace(pedidoSeleccionado.NombreCliente) ? pedidoSeleccionado.NombreCliente : "Consumidor Final",
                Mesero = !string.IsNullOrWhiteSpace(pedidoSeleccionado.NombreMesero) ? pedidoSeleccionado.NombreMesero : "Sin asignar"
            };

            var mesaObj = listaMesas.FirstOrDefault(m => m.IdMesa == pedidoSeleccionado.IdMesa);
            datosFac.NumeroMesa = mesaObj != null ? mesaObj.Numero : pedidoSeleccionado.IdMesa;

            var renglones = Repositorio.ObtenerDetallesPedido(pedidoSeleccionado.IdPedido);
            foreach (var d in renglones)
            {
                datosFac.Items.Add(new TicketService.ItemFactura
                {
                    Producto = !string.IsNullOrWhiteSpace(d.NombreProducto) ? d.NombreProducto : $"Producto #{d.IdProducto}",
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    Descuento = d.MontoDescuento
                });
            }

            return datosFac;
        }

        private void lblTitulo_Click(object sender, EventArgs e)
        {

        }
    }
}