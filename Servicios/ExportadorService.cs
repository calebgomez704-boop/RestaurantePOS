using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace RestaurantePOS.Servicios
{
    public static class ExportadorService
    {
        public static void GenerarFacturaPDF(TicketService.DatosFactura fac, Form parent = null)
        {
            if (fac == null) return;

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Documento PDF (*.pdf)|*.pdf";
                sfd.FileName = $"Factura_Pedido_{fac.IdPedido}_{DateTime.Now:yyyyMMdd_HHmm}.pdf";
                sfd.Title = "Guardar Factura en PDF";

                if (sfd.ShowDialog(parent) != DialogResult.OK)
                    return;

                string ruta = sfd.FileName;

                try
                {
                    PdfDocument document = new PdfDocument();
                    document.Info.Title = $"Comprobante #{fac.IdPedido} - Sabores del Valle";

                    PdfPage page = document.AddPage();
                    page.Width = XUnit.FromPoint(400);
                    page.Height = XUnit.FromPoint(620);

                    using (XGraphics gfx = XGraphics.FromPdfPage(page))
                    {
                        XFont fontTitulo = new XFont("Arial", 16, XFontStyle.Bold);
                        XFont fontSub = new XFont("Arial", 10, XFontStyle.Regular);
                        XFont fontNegrita = new XFont("Arial", 9.5, XFontStyle.Bold);
                        XFont fontNormal = new XFont("Arial", 9, XFontStyle.Regular);
                        XFont fontSmall = new XFont("Arial", 8, XFontStyle.Italic);

                        XBrush brushVino = new XSolidBrush(XColor.FromArgb(107, 62, 62));
                        XBrush brushOscuro = new XSolidBrush(XColor.FromArgb(50, 50, 50));
                        XBrush brushDorado = new XSolidBrush(XColor.FromArgb(180, 140, 30));
                        XPen penLinea = new XPen(XColor.FromArgb(200, 190, 180), 0.8);
                        XPen penVino = new XPen(XColor.FromArgb(107, 62, 62), 1.5);

                        double y = 30;
                        double left = 25;
                        double right = page.Width.Point - 25;
                        double contentWidth = right - left;

                        // Encabezado
                        gfx.DrawString("SABORES DEL VALLE", fontTitulo, brushVino, new XRect(left, y, contentWidth, 22), XStringFormats.TopCenter);
                        y += 24;
                        gfx.DrawString("Comprobante de Consumo del Sistema", fontSub, brushOscuro, new XRect(left, y, contentWidth, 16), XStringFormats.TopCenter);
                        y += 16;
                        gfx.DrawString("Documento informativo no fiscal", fontSmall, XBrushes.Gray, new XRect(left, y, contentWidth, 14), XStringFormats.TopCenter);
                        y += 22;

                        gfx.DrawLine(penVino, left, y, right, y);
                        y += 10;

                        // Datos generales
                        string mesero = !string.IsNullOrWhiteSpace(fac.Mesero) ? fac.Mesero : "Sin asignar";
                        string cliente = !string.IsNullOrWhiteSpace(fac.Cliente) ? fac.Cliente : "Consumidor Final";

                        gfx.DrawString($"Factura / Pedido: #{fac.IdPedido}", fontNegrita, brushOscuro, left, y);
                        gfx.DrawString($"Fecha: {fac.FechaHora:dd/MM/yyyy HH:mm}", fontNormal, brushOscuro, right, y, XStringFormats.TopRight);
                        y += 16;
                        gfx.DrawString($"Mesa: {fac.NumeroMesa}", fontNormal, brushOscuro, left, y);
                        gfx.DrawString($"Mesero: {mesero}", fontNormal, brushOscuro, left + 140, y);
                        y += 16;
                        gfx.DrawString($"Cliente: {cliente}", fontNormal, brushOscuro, left, y);
                        y += 22;

                        gfx.DrawLine(penLinea, left, y, right, y);
                        y += 8;

                        // Cabecera de tabla
                        gfx.DrawRectangle(new XSolidBrush(XColor.FromArgb(245, 235, 225)), left, y, contentWidth, 20);
                        gfx.DrawString("Cant.", fontNegrita, brushVino, left + 5, y + 4);
                        gfx.DrawString("Producto", fontNegrita, brushVino, left + 45, y + 4);
                        gfx.DrawString("P. Unit", fontNegrita, brushVino, right - 135, y + 4, XStringFormats.TopRight);
                        gfx.DrawString("Desc.", fontNegrita, brushVino, right - 75, y + 4, XStringFormats.TopRight);
                        gfx.DrawString("Subtotal", fontNegrita, brushVino, right - 5, y + 4, XStringFormats.TopRight);
                        y += 24;

                        // Filas
                        if (fac.Items != null)
                        {
                            foreach (var it in fac.Items)
                            {
                                string nom = it.Producto ?? "Item";
                                if (nom.Length > 24) nom = nom.Substring(0, 24) + "..";

                                gfx.DrawString(it.Cantidad.ToString(), fontNormal, brushOscuro, left + 5, y);
                                gfx.DrawString(nom, fontNormal, brushOscuro, left + 45, y);
                                gfx.DrawString(it.PrecioUnitario.ToString("N2"), fontNormal, brushOscuro, right - 135, y, XStringFormats.TopRight);
                                
                                decimal subtotalOriginal = it.Cantidad * it.PrecioUnitario;
                                decimal descuento = subtotalOriginal > it.Subtotal ? (subtotalOriginal - it.Subtotal) : 0m;
                                string strDesc = descuento > 0 ? $"-{descuento:N2}" : "-";

                                gfx.DrawString(strDesc, fontNormal, brushOscuro, right - 75, y, XStringFormats.TopRight);
                                gfx.DrawString($"C$ {it.Subtotal:N2}", fontNormal, brushOscuro, right - 5, y, XStringFormats.TopRight);
                                y += 18;
                            }
                        }

                        y += 6;
                        gfx.DrawLine(penLinea, left, y, right, y);
                        y += 12;

                        // Totales con IVA
                        decimal totalPagar = fac.Total;
                        decimal subtotalNeto = Math.Round(totalPagar / (1m + ConfiguracionPOS.TasaIVA), 2);
                        decimal montoIva = totalPagar - subtotalNeto;

                        gfx.DrawString("Subtotal:", fontNormal, brushOscuro, right - 160, y);
                        gfx.DrawString($"C$ {subtotalNeto:N2}", fontNormal, brushOscuro, right - 5, y, XStringFormats.TopRight);
                        y += 16;

                        gfx.DrawString($"IVA ({ConfiguracionPOS.TasaIVA * 100:0}%):", fontNormal, brushOscuro, right - 160, y);
                        gfx.DrawString($"C$ {montoIva:N2}", fontNormal, brushOscuro, right - 5, y, XStringFormats.TopRight);
                        y += 18;

                        gfx.DrawLine(penLinea, right - 170, y, right, y);
                        y += 6;

                        gfx.DrawString("TOTAL A PAGAR:", fontNegrita, brushVino, right - 170, y);
                        gfx.DrawString($"C$ {totalPagar:N2}", fontTitulo, brushVino, right - 5, y - 2, XStringFormats.TopRight);
                        y += 26;

                        // Método de pago y vuelto
                        gfx.DrawLine(penLinea, left, y, right, y);
                        y += 10;

                        gfx.DrawString($"Método de Pago: {fac.MetodoPago}", fontNormal, brushOscuro, left, y);
                        y += 16;

                        if (fac.MontoRecibido.HasValue && fac.MontoRecibido.Value > 0)
                        {
                            gfx.DrawString($"Monto Recibido: C$ {fac.MontoRecibido.Value:N2}", fontNormal, brushOscuro, left, y);
                            gfx.DrawString($"Cambio: C$ {fac.Cambio ?? 0:N2}", fontNegrita, brushDorado, left + 180, y);
                            y += 18;
                        }

                        y += 12;
                        gfx.DrawLine(penVino, left, y, right, y);
                        y += 16;

                        gfx.DrawString("¡Gracias por su visita a Sabores del Valle!", fontNegrita, brushVino, new XRect(left, y, contentWidth, 18), XStringFormats.TopCenter);
                        y += 16;
                        gfx.DrawString("Esperamos servirle nuevamente muy pronto.", fontSmall, brushOscuro, new XRect(left, y, contentWidth, 14), XStringFormats.TopCenter);
                    }

                    document.Save(ruta);
                    document.Close();

                    AbrirArchivo(ruta);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al generar PDF: " + ex.Message, "Error PDF", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public static void ExportarReportePDF(DataGridView dgv, string tituloReporte, string rangoFechas, string resumenTotales, Form parent = null)
        {
            if (dgv == null || dgv.Rows.Count == 0)
            {
                MessageBox.Show("No hay datos en el reporte para exportar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Documento PDF (*.pdf)|*.pdf";
                sfd.FileName = $"Reporte_{DateTime.Now:yyyyMMdd_HHmm}.pdf";
                sfd.Title = "Exportar Reporte a PDF";

                if (sfd.ShowDialog(parent) != DialogResult.OK)
                    return;

                string ruta = sfd.FileName;

                try
                {
                    PdfDocument document = new PdfDocument();
                    document.Info.Title = tituloReporte;

                    // Hoja horizontal (Landscape)
                    XFont fontTitulo = new XFont("Arial", 14, XFontStyle.Bold);
                    XFont fontSub = new XFont("Arial", 9.5, XFontStyle.Regular);
                    XFont fontHeader = new XFont("Arial", 8, XFontStyle.Bold);
                    XFont fontFila = new XFont("Arial", 7.5, XFontStyle.Regular);
                    XFont fontFilaBold = new XFont("Arial", 7.5, XFontStyle.Bold);
                    XFont fontFooter = new XFont("Arial", 8, XFontStyle.Italic);

                    XBrush brushVino = new XSolidBrush(XColor.FromArgb(107, 62, 62));
                    XBrush brushOscuro = new XSolidBrush(XColor.FromArgb(40, 40, 40));
                    XPen penBorde = new XPen(XColor.FromArgb(220, 215, 210), 0.5);

                    // Filtrar columnas visibles
                    var columnasVisibles = new List<DataGridViewColumn>();
                    foreach (DataGridViewColumn col in dgv.Columns)
                    {
                        if (col.Visible) columnasVisibles.Add(col);
                    }

                    double margin = 25;
                    double pageWidth = XUnit.FromPoint(842).Point;   // A4 Landscape width
                    double pageHeight = XUnit.FromPoint(595).Point;  // A4 Landscape height
                    double contentWidth = pageWidth - (margin * 2);

                    // Calcular anchos proporcionales
                    double[] colWidths = new double[columnasVisibles.Count];
                    double totalDgvWidth = 0;
                    foreach (var col in columnasVisibles) totalDgvWidth += Math.Max(col.Width, 30);
                    for (int i = 0; i < columnasVisibles.Count; i++)
                    {
                        colWidths[i] = (Math.Max(columnasVisibles[i].Width, 30) / totalDgvWidth) * contentWidth;
                    }

                    int filaIdx = 0;
                    int pageNumber = 1;
                    int totalFilas = dgv.Rows.Count;

                    while (filaIdx < totalFilas)
                    {
                        PdfPage page = document.AddPage();
                        page.Orientation = PdfSharp.PageOrientation.Landscape;
                        page.Width = XUnit.FromPoint(pageWidth);
                        page.Height = XUnit.FromPoint(pageHeight);

                        using (XGraphics gfx = XGraphics.FromPdfPage(page))
                        {
                            double y = margin;

                            // Encabezado
                            gfx.DrawString("SABORES DEL VALLE - REPORTE DE VENTAS", fontTitulo, brushVino, margin, y);
                            y += 18;
                            gfx.DrawString($"{tituloReporte} | Período: {rangoFechas} | Generado: {DateTime.Now:dd/MM/yyyy HH:mm}", fontSub, brushOscuro, margin, y);
                            y += 20;

                            // Encabezados de tabla
                            double x = margin;
                            double hHeader = 20;
                            gfx.DrawRectangle(new XSolidBrush(XColor.FromArgb(107, 62, 62)), margin, y, contentWidth, hHeader);

                            for (int c = 0; c < columnasVisibles.Count; c++)
                            {
                                gfx.DrawString(columnasVisibles[c].HeaderText, fontHeader, XBrushes.White, new XRect(x + 3, y + 4, colWidths[c] - 6, hHeader), XStringFormats.TopLeft);
                                x += colWidths[c];
                            }
                            y += hHeader;

                            // Filas
                            double hRow = 16;
                            double maxY = pageHeight - margin - 35; // Espacio para totales y número de página

                            while (filaIdx < totalFilas && y + hRow <= maxY)
                            {
                                var row = dgv.Rows[filaIdx];
                                bool esTotales = row.Cells[0].Value?.ToString() == "TOTALES" ||
                                                 row.Cells[1].Value?.ToString() == "TOTALES";

                                XBrush bgBrush = esTotales ? new XSolidBrush(XColor.FromArgb(245, 230, 211)) :
                                                 (filaIdx % 2 == 0 ? XBrushes.White : new XSolidBrush(XColor.FromArgb(250, 248, 245)));

                                gfx.DrawRectangle(bgBrush, margin, y, contentWidth, hRow);

                                x = margin;
                                for (int c = 0; c < columnasVisibles.Count; c++)
                                {
                                    string val = row.Cells[columnasVisibles[c].Index].FormattedValue?.ToString() ?? string.Empty;
                                    XFont f = esTotales ? fontFilaBold : fontFila;
                                    XBrush b = esTotales ? brushVino : brushOscuro;

                                    XStringFormat fmt = XStringFormats.TopLeft;
                                    if (columnasVisibles[c].DefaultCellStyle.Alignment == DataGridViewContentAlignment.MiddleRight ||
                                        columnasVisibles[c].DefaultCellStyle.Alignment == DataGridViewContentAlignment.TopRight)
                                    {
                                        fmt = XStringFormats.TopRight;
                                    }
                                    else if (columnasVisibles[c].DefaultCellStyle.Alignment == DataGridViewContentAlignment.MiddleCenter)
                                    {
                                        fmt = XStringFormats.TopCenter;
                                    }

                                    gfx.DrawString(val, f, b, new XRect(x + 2, y + 2, colWidths[c] - 4, hRow), fmt);
                                    gfx.DrawRectangle(penBorde, x, y, colWidths[c], hRow);
                                    x += colWidths[c];
                                }

                                y += hRow;
                                filaIdx++;
                            }

                            // Si es la última página, dibujar resumen de totales
                            if (filaIdx >= totalFilas && !string.IsNullOrWhiteSpace(resumenTotales))
                            {
                                y += 6;
                                gfx.DrawString(resumenTotales, fontFilaBold, brushVino, margin, y);
                            }

                            // Pie de página con número de página
                            gfx.DrawString($"Página {pageNumber}", fontFooter, brushOscuro, pageWidth - margin, pageHeight - margin + 5, XStringFormats.TopRight);
                            gfx.DrawString("Restaurante Sabores del Valle POS", fontFooter, XBrushes.Gray, margin, pageHeight - margin + 5, XStringFormats.TopLeft);
                            pageNumber++;
                        }
                    }

                    document.Save(ruta);
                    document.Close();

                    AbrirArchivo(ruta);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al exportar reporte PDF: " + ex.Message, "Error PDF", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public static void ExportarReporteCSV(DataGridView dgv, Form parent = null)
        {
            if (dgv == null || dgv.Rows.Count == 0)
            {
                MessageBox.Show("No hay datos en el reporte para exportar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Archivo CSV (Excel) (*.csv)|*.csv";
                sfd.FileName = $"Reporte_Ventas_{DateTime.Now:yyyyMMdd_HHmm}.csv";
                sfd.Title = "Exportar Reporte a CSV";

                if (sfd.ShowDialog(parent) != DialogResult.OK)
                    return;

                string ruta = sfd.FileName;

                try
                {
                    var sb = new StringBuilder();

                    // Encabezados
                    var headers = new List<string>();
                    var indicesVisibles = new List<int>();
                    foreach (DataGridViewColumn col in dgv.Columns)
                    {
                        if (col.Visible)
                        {
                            headers.Add(EscapearCSV(col.HeaderText));
                            indicesVisibles.Add(col.Index);
                        }
                    }
                    sb.AppendLine(string.Join(";", headers));

                    // Filas
                    foreach (DataGridViewRow row in dgv.Rows)
                    {
                        if (row.IsNewRow) continue;
                        var filaValores = new List<string>();
                        foreach (int idx in indicesVisibles)
                        {
                            string val = row.Cells[idx].FormattedValue?.ToString() ?? string.Empty;
                            filaValores.Add(EscapearCSV(val));
                        }
                        sb.AppendLine(string.Join(";", filaValores));
                    }

                    File.WriteAllText(ruta, sb.ToString(), Encoding.UTF8);

                    MessageBox.Show($"Archivo CSV exportado exitosamente:\n{ruta}", "Exportación Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    AbrirArchivo(ruta);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al exportar CSV: " + ex.Message, "Error CSV", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private static string EscapearCSV(string texto)
        {
            if (string.IsNullOrEmpty(texto)) return "";
            if (texto.Contains(";") || texto.Contains("\"") || texto.Contains("\n") || texto.Contains("\r"))
            {
                return "\"" + texto.Replace("\"", "\"\"") + "\"";
            }
            return texto;
        }

        private static void AbrirArchivo(string ruta)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = ruta,
                    UseShellExecute = true
                });
            }
            catch { }
        }
    }
}
