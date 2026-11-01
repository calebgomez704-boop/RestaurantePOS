using RestaurantePOS.Servicios;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using RestaurantePOS.DAL;
using RestaurantePOS.Modelos;

namespace RestaurantePOS.Modelos
{
    public partial class FormReportes : Form
    {
        private bool _modoDetallado = true;

        public FormReportes()
        {
            InitializeComponent();
        }

        private void FormReportes_Load(object sender, EventArgs e)
        {
            if (!FormPrincipal.TienePermiso(Sesion.Puesto, "Reportes"))
            {
                MessageBox.Show("No tiene autorización para ver los reportes del restaurante.", "Acceso Restringido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.BeginInvoke(new Action(() => this.Close()));
                return;
            }

            dtpDesde.Value = DateTime.Today.AddDays(-30);
            dtpHasta.Value = DateTime.Today;

            UITheme.AplicarEstiloDataGridView(dgvReportes);
            EstilarBotonReporte(btnFiltrar, "Filtrar", UITheme.ColorVinoPrincipal, UITheme.ColorDorado);
            EstilarBotonReporte(btnExportarPDF, "Exportar PDF", UITheme.ColorVinoPrincipal, UITheme.ColorDorado);
            EstilarBotonReporte(btnExportarExcel, "Exportar Excel", UITheme.ColorVerdeExito, Color.White);
            EstilarBotonReporte(btnFacturaPDF, "Factura PDF", Color.FromArgb(55, 105, 145), Color.White);

            CargarReporte();
        }

        private void EstilarBotonReporte(Button btn, string texto, Color backColor, Color foreColor)
        {
            btn.Image = null;
            btn.Text = texto;
            btn.BackColor = backColor;
            btn.ForeColor = foreColor;
            btn.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Cursor = Cursors.Hand;
            btn.TextAlign = ContentAlignment.MiddleCenter;
            btn.TextImageRelation = TextImageRelation.Overlay;
            btn.Padding = new Padding(0);
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            CargarReporte();
        }

        private void btnModoDetallado_Click(object sender, EventArgs e)
        {
            _modoDetallado = true;
            btnModoDetallado.BackColor = UITheme.ColorVinoPrincipal;
            btnModoDetallado.ForeColor = UITheme.ColorDorado;
            btnModoPedidos.BackColor = Color.FromArgb(150, 140, 130);
            btnModoPedidos.ForeColor = Color.White;
            CargarReporte();
        }

        private void btnModoPedidos_Click(object sender, EventArgs e)
        {
            _modoDetallado = false;
            btnModoPedidos.BackColor = UITheme.ColorVinoPrincipal;
            btnModoPedidos.ForeColor = UITheme.ColorDorado;
            btnModoDetallado.BackColor = Color.FromArgb(150, 140, 130);
            btnModoDetallado.ForeColor = Color.White;
            CargarReporte();
        }

        private void CargarReporte()
        {
            if (_modoDetallado)
            {
                CargarReporteDetalladoConIVA();
            }
            else
            {
                CargarReportePedidos();
            }
        }

        private void CargarReporteDetalladoConIVA()
        {
            try
            {
                DateTime inicio = dtpDesde.Value.Date;
                DateTime fin = dtpHasta.Value.Date.AddDays(1).AddTicks(-1);

                var lista = Repositorio.ObtenerVentasDetalladasConIVA(inicio, fin);

                decimal sumaNeto = lista.Sum(x => x.TotalNeto);
                decimal sumaIVA = lista.Sum(x => x.IVA);
                decimal sumaTotal = lista.Sum(x => x.VentaConIVA);

                lblTotales.Text = $"TOTALES DEL PERÍODO -> Total Neto: {sumaNeto:C2} | IVA (15%): {sumaIVA:C2} | Venta + IVA: {sumaTotal:C2}  (Pedidos Pagados: {lista.Select(x => x.NumeroPedido).Distinct().Count()})";

                // Agregar fila resumen al final de la grilla
                if (lista.Count > 0)
                {
                    lista.Add(new ItemReporteVenta
                    {
                        Fecha = null,
                        NumeroPedido = "TOTALES",
                        Cliente = "---",
                        Vendedor = "---",
                        MetodoPago = "---",
                        Producto = "---",
                        PrecioUnitario = null,
                        Cantidad = lista.Sum(x => x.Cantidad ?? 0),
                        TotalNeto = sumaNeto,
                        IVA = sumaIVA,
                        VentaConIVA = sumaTotal
                    });
                }

                ConfigurarColumnasDetalladas();
                dgvReportes.DataSource = null;
                dgvReportes.DataSource = lista;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar reporte detallado: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarColumnasDetalladas()
        {
            dgvReportes.Columns.Clear();
            dgvReportes.AutoGenerateColumns = false;

            dgvReportes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Fecha",
                HeaderText = "Fecha",
                Width = 125,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm" }
            });
            dgvReportes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "NumeroPedido",
                HeaderText = "Nº Pedido",
                Width = 90
            });
            dgvReportes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Cliente",
                HeaderText = "Cliente",
                Width = 130
            });
            dgvReportes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Vendedor",
                HeaderText = "Vendedor",
                Width = 110
            });
            dgvReportes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MetodoPago",
                HeaderText = "Método Pago",
                Width = 100
            });
            dgvReportes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Producto",
                HeaderText = "Producto",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            dgvReportes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "PrecioUnitario",
                HeaderText = "Precio Unit.",
                Width = 95,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });
            dgvReportes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Cantidad",
                HeaderText = "Cant.",
                Width = 60,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });
                        dgvReportes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Descuento",
                HeaderText = "Descuento",
                Width = 85,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });
            dgvReportes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TotalNeto",
                HeaderText = "Total Neto",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });
            dgvReportes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "IVA",
                HeaderText = "IVA (15%)",
                Width = 95,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });
            dgvReportes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "VentaConIVA",
                HeaderText = "Venta + IVA",
                Width = 110,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });
        }

        private void CargarReportePedidos()
        {
            try
            {
                DateTime inicio = dtpDesde.Value.Date;
                DateTime fin = dtpHasta.Value.Date.AddDays(1).AddTicks(-1);

                var todos = Repositorio.ObtenerPedidos();
                var filtrados = todos.Where(p => p.Fecha >= inicio && p.Fecha <= fin).ToList();

                var pagados = filtrados.Where(p => p.Estado == "Pagado").ToList();
                decimal total = pagados.Sum(p => p.Total);
                decimal neto = Math.Round(total / (1m + ConfiguracionPOS.TasaIVA), 2);
                decimal iva = total - neto;

                lblTotales.Text = $"TOTAL HISTÓRICO -> Total Neto: {neto:C2} | IVA (15%): {iva:C2} | Total Ventas: {total:C2}  (Pagados: {pagados.Count} de {filtrados.Count})";

                ConfigurarColumnasPedidos();
                dgvReportes.DataSource = null;
                dgvReportes.DataSource = filtrados;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar pedidos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarColumnasPedidos()
        {
            dgvReportes.Columns.Clear();
            dgvReportes.AutoGenerateColumns = false;

            dgvReportes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "IdPedido",
                HeaderText = "Nº Pedido",
                Width = 90
            });
            dgvReportes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "NumeroMesa",
                HeaderText = "Mesa",
                Width = 90
            });
            dgvReportes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "NombreCliente",
                HeaderText = "Cliente",
                Width = 180
            });
            dgvReportes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "NombreMesero",
                HeaderText = "Mesero",
                Width = 150
            });
            dgvReportes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Fecha",
                HeaderText = "Fecha y Hora",
                Width = 140,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm" }
            });
            dgvReportes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Total",
                HeaderText = "Monto Total",
                Width = 110,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });
            dgvReportes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "NombreMetodoPago",
                HeaderText = "Método Pago",
                Width = 120
            });
            dgvReportes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Estado",
                HeaderText = "Estado",
                Width = 90
            });
        }

        private void dgvReportes_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var fila = dgvReportes.Rows[e.RowIndex];
            if (fila.DataBoundItem is ItemReporteVenta item && item.NumeroPedido == "TOTALES")
            {
                fila.DefaultCellStyle.BackColor = Color.FromArgb(245, 230, 211);
                fila.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                fila.DefaultCellStyle.ForeColor = UITheme.ColorVinoOscuro;
            }
        }

        private void btnExportarPDF_Click(object sender, EventArgs e)
        {
            string titulo = _modoDetallado ? "Ventas Detalladas con Desglose de IVA (15%)" : "Histórico General de Pedidos";
            string rango = $"{dtpDesde.Value:dd/MM/yyyy} al {dtpHasta.Value:dd/MM/yyyy}";
            ExportadorService.ExportarReportePDF(dgvReportes, titulo, rango, lblTotales.Text, this);
        }

        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            ExportadorService.ExportarReporteCSV(dgvReportes, this);
        }

        private void btnFacturaPDF_Click(object sender, EventArgs e)
        {
            if (dgvReportes.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un registro en la grilla para generar su factura en PDF.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idPedido = 0;
            if (_modoDetallado)
            {
                var item = dgvReportes.CurrentRow.DataBoundItem as ItemReporteVenta;
                if (item != null && int.TryParse(item.NumeroPedido, out int id))
                {
                    idPedido = id;
                }
            }
            else
            {
                var ped = dgvReportes.CurrentRow.DataBoundItem as clsPedido;
                if (ped != null)
                {
                    idPedido = ped.IdPedido;
                }
            }

            if (idPedido <= 0)
            {
                MessageBox.Show("Seleccione un pedido válido (no la fila de totales).", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var todosPedidos = Repositorio.ObtenerPedidos();
            var pedido = todosPedidos.FirstOrDefault(p => p.IdPedido == idPedido);
            if (pedido == null)
            {
                MessageBox.Show("No se encontró la información del pedido seleccionado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var renglones = Repositorio.ObtenerDetallesPedido(idPedido);
            var datosFac = new TicketService.DatosFactura
            {
                IdPedido = pedido.IdPedido,
                FechaHora = pedido.Fecha,
                MetodoPago = !string.IsNullOrWhiteSpace(pedido.NombreMetodoPago) ? pedido.NombreMetodoPago : "Efectivo",
                Total = pedido.Total,
                Cliente = !string.IsNullOrWhiteSpace(pedido.NombreCliente) ? pedido.NombreCliente : "Consumidor Final",
                Mesero = !string.IsNullOrWhiteSpace(pedido.NombreMesero) ? pedido.NombreMesero : "Sin asignar"
            };

            int numMesa = 0;
            if (int.TryParse(pedido.NumeroMesa.Replace("Mesa", "").Trim(), out int nm))
                numMesa = nm;
            else
                numMesa = pedido.IdMesa;
            datosFac.NumeroMesa = numMesa;

            foreach (var r in renglones)
            {
                datosFac.Items.Add(new TicketService.ItemFactura
                {
                    Producto = !string.IsNullOrWhiteSpace(r.NombreProducto) ? r.NombreProducto : $"Producto #{r.IdProducto}",
                    Cantidad = r.Cantidad,
                    PrecioUnitario = r.PrecioUnitario,
                    Descuento = r.MontoDescuento
                });
            }

            ExportadorService.GenerarFacturaPDF(datosFac, this);
        }
}
}