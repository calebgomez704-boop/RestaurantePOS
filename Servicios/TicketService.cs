using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;

namespace RestaurantePOS.Servicios
{
    /// <summary>
    /// Servicio de impresión de Facturas / Tickets del Restaurante POS 'Sabores del Valle'.
    /// Utiliza componentes nativos de .NET Framework (PrintDocument, PrintPreviewDialog).
    /// PARTE 2 (Punto 5): Lee Mesero, Cliente y Método de Pago directamente desde los datos
    /// asociados al pedido. Si Cliente es nulo o vacío, muestra 'Consumidor Final'.
    /// Los pedidos antiguos con valores nulos no rompen la factura.
    /// </summary>
    public static class TicketService
    {
        public class ItemFactura
        {
            public string Producto { get; set; } = string.Empty;
            public int Cantidad { get; set; }
            public decimal PrecioUnitario { get; set; }
            public decimal Descuento { get; set; } = 0;
            public decimal Subtotal => (Cantidad * PrecioUnitario) - Descuento;
        }

        public class DatosFactura
        {
            public int IdPedido { get; set; }
            public int NumeroMesa { get; set; }
            public string Mesero { get; set; } = "Sin asignar";
            public string Cliente { get; set; } = "Consumidor Final";
            public DateTime FechaHora { get; set; } = DateTime.Now;
            public string MetodoPago { get; set; } = "Efectivo";
            public decimal Total { get; set; }
            public decimal? MontoRecibido { get; set; }
            public decimal? Cambio { get; set; }
            public List<ItemFactura> Items { get; set; } = new List<ItemFactura>();
        }

        public static string GenerarTextoFactura(DatosFactura fac)
        {
            if (fac == null) return string.Empty;

            string mesero = !string.IsNullOrWhiteSpace(fac.Mesero) ? fac.Mesero : "Sin asignar";
            string cliente = !string.IsNullOrWhiteSpace(fac.Cliente) ? fac.Cliente : "Consumidor Final";
            string metodo = !string.IsNullOrWhiteSpace(fac.MetodoPago) ? fac.MetodoPago : "Efectivo";

            var sb = new StringBuilder();
            sb.AppendLine("========================================");
            sb.AppendLine("          SABORES DEL VALLE             ");
            sb.AppendLine("      Servicio de Restaurante POS       ");
            sb.AppendLine("========================================");
            sb.AppendLine($" Factura / Pedido: #{fac.IdPedido}");
            sb.AppendLine($" Mesa: {fac.NumeroMesa}    Mesero: {mesero}");
            sb.AppendLine($" Cliente: {cliente}");
            sb.AppendLine($" Fecha: {fac.FechaHora:dd/MM/yyyy}  Hora: {fac.FechaHora:HH:mm:ss}");
            sb.AppendLine("----------------------------------------");
            sb.AppendLine(string.Format("{0,-18} {1,4} {2,8} {3,8}", "Producto", "Cant", "Precio", "Total"));
            sb.AppendLine("----------------------------------------");

            if (fac.Items != null)
            {
                foreach (var it in fac.Items)
                {
                    string nomProd = it.Producto ?? "Item";
                    string nombre = nomProd.Length > 17 ? nomProd.Substring(0, 17) : nomProd;
                    sb.AppendLine(string.Format("{0,-18} {1,4} {2,8:N2} {3,8:N2}", nombre, it.Cantidad, it.PrecioUnitario, it.Subtotal));
                }
            }

            sb.AppendLine("----------------------------------------");
            decimal totalPagar = fac.Total;
            decimal subtotalNeto = Math.Round(totalPagar / (1m + ConfiguracionPOS.TasaIVA), 2);
            decimal montoIva = totalPagar - subtotalNeto;

            sb.AppendLine(string.Format("{0,-28} {1,10:N2}", "Subtotal:", subtotalNeto));
            sb.AppendLine(string.Format("{0,-28} {1,10:N2}", $"IVA ({ConfiguracionPOS.TasaIVA * 100:0}%):", montoIva));
            sb.AppendLine(string.Format("{0,-28} {1,10:C2}", "TOTAL A PAGAR:", totalPagar));
            sb.AppendLine("----------------------------------------");
            sb.AppendLine($" Método de Pago: {metodo}");

            if (fac.MontoRecibido.HasValue && fac.MontoRecibido.Value > 0)
            {
                sb.AppendLine(string.Format("{0,-28} {1,10:C2}", "Monto Recibido:", fac.MontoRecibido.Value));
                sb.AppendLine(string.Format("{0,-28} {1,10:C2}", "Cambio / Vuelto:", fac.Cambio ?? 0));
            }

            sb.AppendLine("========================================");
            sb.AppendLine("        ¡Gracias por su visita!         ");
            sb.AppendLine("========================================");

            return sb.ToString();
        }

        public static void MostrarVistaPrevia(DatosFactura fac, Form parent = null)
        {
            try
            {
                string textoTicket = GenerarTextoFactura(fac);
                PrintDocument pd = new PrintDocument();
                pd.DocumentName = $"Factura_Pedido_{fac.IdPedido}";
                pd.PrintPage += (sender, e) =>
                {
                    using (Font font = new Font("Courier New", 9F, FontStyle.Regular))
                    using (SolidBrush brush = new SolidBrush(Color.Black))
                    {
                        e.Graphics.DrawString(textoTicket, font, brush, new PointF(10, 10));
                    }
                };

                PrintPreviewDialog dlg = new PrintPreviewDialog();
                dlg.Document = pd;
                dlg.Width = 500;
                dlg.Height = 650;
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.ShowDialog(parent);
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo abrir la vista de impresión: " + ex.Message, "Impresión", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
