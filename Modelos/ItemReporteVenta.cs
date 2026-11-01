using System;

namespace RestaurantePOS.Modelos
{
    /// <summary>
    /// Modelo para el reporte de ventas detallado por producto con desglose de Descuentos e IVA (15%).
    /// </summary>
    public class ItemReporteVenta
    {
        public DateTime? Fecha { get; set; }
        public string NumeroPedido { get; set; } = string.Empty;
        public string Cliente { get; set; } = string.Empty;
        public string Vendedor { get; set; } = string.Empty;
        public string MetodoPago { get; set; } = string.Empty;
        public string Producto { get; set; } = string.Empty;
        public decimal? PrecioUnitario { get; set; }
        public int? Cantidad { get; set; }
        public decimal Descuento { get; set; }
        public decimal TotalNeto { get; set; }
        public decimal IVA { get; set; }
        public decimal VentaConIVA { get; set; }
    }
}
