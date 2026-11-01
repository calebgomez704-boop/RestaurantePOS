using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace RestaurantePOS
{
    /// <summary>
    /// Clase 'UITheme': Gestiona la identidad visual, paleta de colores oficial,
    /// iconos vectoriales/PNG, estilos de DataGridView y efectos hover interactivos del sistema POS.
    /// </summary>
    public static class UITheme
    {
        // ==========================================
        // PALETA DE COLORES OFICIAL DEL PROYECTO
        // ==========================================
        public static readonly Color ColorVinoPrincipal = Color.FromArgb(107, 62, 62);      // #6B3E3E - Color primario
        public static readonly Color ColorVinoOscuro    = Color.FromArgb(80, 45, 45);       // #502D2D - Activo / Hover oscuro
        public static readonly Color ColorVinoClaro     = Color.FromArgb(138, 80, 80);      // #8A5050 - Hover suave
        public static readonly Color ColorDorado        = Color.FromArgb(212, 175, 55);     // #D4AF37 - Acento dorado
        public static readonly Color ColorDoradoHover   = Color.FromArgb(240, 205, 95);     // #F0CD5F - Dorado brillante
        public static readonly Color ColorFondoCrema    = Color.FromArgb(245, 230, 211);    // #F5E6D3 - Fondo de formularios
        public static readonly Color ColorFondoTarjeta  = Color.FromArgb(255, 252, 248);    // Blanco cálido para tarjetas
        public static readonly Color ColorTextoOscuro   = Color.FromArgb(62, 62, 62);       // #3E3E3E - Texto principal
        public static readonly Color ColorTextoGris     = Color.FromArgb(110, 110, 110);    // Texto secundario
        public static readonly Color ColorVerdeExito    = Color.FromArgb(46, 139, 87);      // #2E8B57 - Acciones de agregar/guardar
        public static readonly Color ColorRojoPeligro   = Color.FromArgb(178, 34, 34);      // #B22222 - Acciones de eliminar
        public static readonly Color ColorAzulInfo      = Color.FromArgb(70, 130, 180);     // #4682B4 - Acciones de editar
        public static readonly Color ColorStockAgotado  = Color.LightCoral;                 // Alerta agotado
        public static readonly Color ColorStockBajo     = Color.LightGoldenrodYellow;       // Alerta stock bajo
        public static readonly Color ColorBlanco        = Color.White;                      // Blanco puro
        public static readonly Color ColorBotonDeshabilitadoFondo = Color.FromArgb(230, 230, 230);
        public static readonly Color ColorBotonDeshabilitadoTexto = Color.FromArgb(120, 120, 120);

        // ==========================================
        // TIPOGRAFÍAS DEL SISTEMA
        // ==========================================
        public static readonly Font FuenteTituloGrande = new Font("Segoe UI", 16F, FontStyle.Bold);
        public static readonly Font FuenteSubtitulo    = new Font("Segoe UI", 12F, FontStyle.Bold);
        public static readonly Font FuenteBotonesMenu  = new Font("Segoe UI", 10.5F, FontStyle.Bold);
        public static readonly Font FuenteBotones      = new Font("Segoe UI", 10F, FontStyle.Bold);
        public static readonly Font FuenteCampos       = new Font("Segoe UI", 10.5F, FontStyle.Regular);
        public static readonly Font FuenteEtiquetas    = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        public static readonly Font FuenteGridHeader   = new Font("Segoe UI", 10F, FontStyle.Bold);
        public static readonly Font FuenteGridFilas    = new Font("Segoe UI", 9.5F, FontStyle.Regular);

        // ==========================================
        // CARGA DE ICONOS CON RESPALDO
        // ==========================================
        private static string ObtenerRutaAssets()
        {
            // Instalacion: la carpeta Assets va junto al ejecutable.
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string rutaLocal = Path.Combine(baseDir, "Assets");
            if (Directory.Exists(rutaLocal)) return rutaLocal;

            // Respaldo solo para desarrollo en Visual Studio (bin\Debug o bin\Release).
            try
            {
                string rutaProyecto = Path.GetFullPath(Path.Combine(baseDir, @"..\..\Assets"));
                if (Directory.Exists(rutaProyecto)) return rutaProyecto;
            }
            catch { }

            return baseDir;
        }

        public static Image ObtenerIcono(string nombreIcono)
        {
            try
            {
                string ruta = Path.Combine(ObtenerRutaAssets(), nombreIcono + ".png");
                if (File.Exists(ruta))
                {
                    using (var stream = new MemoryStream(File.ReadAllBytes(ruta)))
                    {
                        return Image.FromStream(stream);
                    }
                }
            }
            catch { }

            // Generador de respaldo si no existe el archivo físico
            return GenerarIconoRespaldo(nombreIcono);
        }

        private static Image GenerarIconoRespaldo(string tipo)
        {
            Bitmap bmp = new Bitmap(28, 28);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (Pen pen = new Pen(ColorDorado, 2))
                using (SolidBrush brush = new SolidBrush(ColorDorado))
                using (Pen whitePen = new Pen(Color.White, 2))
                using (SolidBrush whiteBrush = new SolidBrush(Color.White))
                {
                    switch (tipo.ToLower())
                    {
                        case "icon_pedidos":
                            g.DrawRectangle(pen, 5, 5, 18, 20);
                            g.FillRectangle(brush, 9, 2, 10, 4);
                            g.DrawLine(whitePen, 9, 11, 19, 11);
                            g.DrawLine(whitePen, 9, 16, 19, 16);
                            break;
                        case "icon_productos":
                            g.DrawArc(pen, 3, 7, 22, 16, 180, 180);
                            g.DrawLine(pen, 2, 15, 26, 15);
                            g.FillEllipse(brush, 12, 4, 4, 4);
                            break;
                        case "icon_clientes":
                            g.DrawEllipse(pen, 10, 3, 8, 8);
                            g.DrawArc(pen, 4, 13, 20, 14, 180, 180);
                            break;
                        case "icon_empleados":
                            g.DrawEllipse(pen, 6, 5, 7, 7);
                            g.DrawEllipse(pen, 15, 5, 7, 7);
                            g.DrawArc(pen, 5, 15, 18, 12, 180, 180);
                            break;
                        case "icon_proveedores":
                            g.DrawRectangle(pen, 3, 7, 14, 11);
                            g.DrawRectangle(pen, 17, 10, 8, 8);
                            g.FillEllipse(brush, 6, 16, 5, 5);
                            g.FillEllipse(brush, 19, 16, 5, 5);
                            break;
                        case "icon_pagos":
                            g.DrawRectangle(pen, 3, 6, 22, 15);
                            g.FillRectangle(brush, 3, 10, 22, 3);
                            break;
                        case "icon_reportes":
                            g.DrawLine(pen, 4, 4, 4, 24);
                            g.DrawLine(pen, 4, 24, 24, 24);
                            g.FillRectangle(brush, 7, 16, 4, 8);
                            g.FillRectangle(whiteBrush, 13, 11, 4, 13);
                            g.FillRectangle(brush, 19, 6, 4, 18);
                            break;
                        case "icon_mesas":
                            g.DrawEllipse(pen, 7, 7, 14, 14);
                            g.DrawLine(whitePen, 14, 7, 14, 21);
                            g.DrawLine(whitePen, 7, 14, 21, 14);
                            break;
                        case "icon_caja":
                            g.DrawRectangle(pen, 4, 7, 20, 14);
                            g.DrawLine(whitePen, 4, 12, 24, 12);
                            g.FillRectangle(brush, 11, 15, 6, 3);
                            break;
                        case "icon_reservaciones":
                            g.DrawRectangle(pen, 4, 6, 20, 18);
                            g.DrawLine(whitePen, 4, 11, 24, 11);
                            g.FillEllipse(brush, 8, 3, 3, 5);
                            g.FillEllipse(brush, 17, 3, 3, 5);
                            break;
                        case "icon_insumos":
                            g.DrawRectangle(pen, 5, 8, 18, 16);
                            g.DrawLine(whitePen, 5, 14, 23, 14);
                            g.DrawLine(pen, 14, 8, 14, 24);
                            break;
                        case "icon_recetas":
                            g.DrawRectangle(pen, 5, 4, 18, 20);
                            g.DrawLine(whitePen, 8, 8, 18, 8);
                            g.DrawLine(whitePen, 8, 13, 18, 13);
                            g.DrawLine(whitePen, 8, 18, 15, 18);
                            break;
                        case "icon_promociones":
                            g.DrawRectangle(pen, 4, 7, 20, 14);
                            g.DrawLine(whitePen, 18, 9, 10, 19);
                            g.FillEllipse(brush, 8, 9, 3, 3);
                            g.FillEllipse(brush, 17, 16, 3, 3);
                            break;
                        case "icon_turnos":
                            g.DrawEllipse(pen, 4, 4, 20, 20);
                            g.DrawLine(whitePen, 14, 14, 14, 8);
                            g.DrawLine(whitePen, 14, 14, 19, 14);
                            break;
                        case "icon_logout":
                            g.DrawRectangle(pen, 5, 4, 11, 20);
                            g.DrawLine(whitePen, 13, 14, 23, 14);
                            g.DrawLine(whitePen, 19, 9, 23, 14);
                            g.DrawLine(whitePen, 19, 19, 23, 14);
                            break;
                        case "icon_add":
                            g.DrawEllipse(pen, 3, 3, 22, 22);
                            g.DrawLine(whitePen, 14, 8, 14, 20);
                            g.DrawLine(whitePen, 8, 14, 20, 14);
                            break;
                        case "icon_edit":
                            g.DrawLine(pen, 6, 22, 22, 6);
                            g.DrawRectangle(whitePen, 17, 4, 6, 6);
                            break;
                        case "icon_delete":
                            using (Pen rp = new Pen(Color.FromArgb(235, 87, 87), 2))
                            {
                                g.DrawLine(rp, 5, 7, 23, 7);
                                g.DrawRectangle(rp, 7, 7, 14, 16);
                            }
                            break;
                        case "icon_save":
                            using (Pen gp = new Pen(ColorVerdeExito, 2.5f))
                            {
                                g.DrawLine(gp, 6, 14, 12, 20);
                                g.DrawLine(gp, 12, 20, 23, 8);
                            }
                            break;
                        default:
                            g.FillEllipse(brush, 10, 10, 8, 8);
                            break;
                    }
                }
            }
            return bmp;
        }

        // ==========================================
        // ESTILIZACIÓN DE LA BARRA SUPERIOR
        // ==========================================
        public static void ConfigurarBotonMenuSuperior(Button btn, string nombreIcono, bool esLogout = false)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = esLogout ? Color.FromArgb(120, 40, 40) : ColorVinoClaro;
            btn.FlatAppearance.MouseDownBackColor = esLogout ? Color.FromArgb(60, 20, 20) : ColorVinoOscuro;
            btn.BackColor = esLogout ? ColorVinoOscuro : ColorVinoPrincipal;
            btn.ForeColor = ColorDorado;
            btn.Font = FuenteBotonesMenu;
            btn.Cursor = Cursors.Hand;
            btn.ImageAlign = ContentAlignment.MiddleLeft;
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.TextImageRelation = TextImageRelation.ImageBeforeText;
            btn.Padding = new Padding(12, 0, 10, 0);

            Image ico = ObtenerIcono(nombreIcono);
            if (ico != null)
            {
                btn.Image = ico;
            }

            // Animación y respuesta visual elegante al pasar el mouse
            btn.MouseEnter += (s, e) =>
            {
                if ((string)btn.Tag != "activo")
                {
                    btn.BackColor = esLogout ? Color.FromArgb(110, 40, 40) : ColorVinoClaro;
                    btn.ForeColor = ColorDoradoHover;
                }
            };

            btn.MouseLeave += (s, e) =>
            {
                if ((string)btn.Tag != "activo")
                {
                    btn.BackColor = esLogout ? ColorVinoOscuro : ColorVinoPrincipal;
                    btn.ForeColor = ColorDorado;
                }
            };
        }

        public static void MarcarBotonMenuActivo(Button btnActivo, Control contenedorMenu)
        {
            foreach (Control ctrl in contenedorMenu.Controls)
            {
                if (ctrl is Button btn && btn.Name != "btnCerrarSesion")
                {
                    btn.Tag = null;
                    btn.BackColor = ColorVinoPrincipal;
                    btn.ForeColor = ColorDorado;
                    btn.Font = FuenteBotonesMenu;
                }
            }

            if (btnActivo != null && btnActivo.Name != "btnCerrarSesion")
            {
                btnActivo.Tag = "activo";
                btnActivo.BackColor = ColorVinoOscuro;
                btnActivo.ForeColor = Color.White;
            }
        }

        // ==========================================
        // ESTILIZACIÓN MODERNA DE DATAGRIDVIEW
        // ==========================================
        public static void AplicarEstiloDataGridView(DataGridView dgv)
        {
            if (dgv == null) return;

            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.GridColor = Color.FromArgb(235, 225, 215);
            dgv.EnableHeadersVisualStyles = false;
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.AllowUserToResizeRows = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Encabezados
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = ColorVinoPrincipal;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = ColorDorado;
            dgv.ColumnHeadersDefaultCellStyle.Font = FuenteGridHeader;
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 0, 0, 0);
            dgv.ColumnHeadersHeight = 40;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            // Filas
            dgv.RowTemplate.Height = 34;
            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.ForeColor = ColorTextoOscuro;
            dgv.DefaultCellStyle.Font = FuenteGridFilas;
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(138, 80, 80);
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;
            dgv.DefaultCellStyle.Padding = new Padding(8, 2, 8, 2);

            // Filas alternadas suaves
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(253, 248, 242);
            dgv.AlternatingRowsDefaultCellStyle.ForeColor = ColorTextoOscuro;
            dgv.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(138, 80, 80);
            dgv.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.White;
        }

        // ==========================================
        // ESTILIZACIÓN DE BOTONES CRUD
        // ==========================================
        public enum TipoBotonAccion
        {
            Agregar,
            Editar,
            Eliminar,
            Guardar,
            Secundario
        }

        public static void ConfigurarBotonAccion(Button btn, TipoBotonAccion tipo, string textoConIcono = null)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = FuenteBotones;
            btn.Cursor = Cursors.Hand;
            btn.TextImageRelation = TextImageRelation.ImageBeforeText;
            btn.ImageAlign = ContentAlignment.MiddleLeft;
            btn.TextAlign = ContentAlignment.MiddleCenter;
            btn.Padding = new Padding(8, 0, 8, 0);

            switch (tipo)
            {
                case TipoBotonAccion.Agregar:
                    btn.BackColor = ColorVerdeExito;
                    btn.ForeColor = Color.White;
                    btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(38, 115, 71);
                    btn.Image = ObtenerIcono("icon_add");
                    break;

                case TipoBotonAccion.Editar:
                    btn.BackColor = ColorAzulInfo;
                    btn.ForeColor = Color.White;
                    btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(55, 105, 145);
                    btn.Image = ObtenerIcono("icon_edit");
                    break;

                case TipoBotonAccion.Eliminar:
                    btn.BackColor = ColorRojoPeligro;
                    btn.ForeColor = Color.White;
                    btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(145, 25, 25);
                    btn.Image = ObtenerIcono("icon_delete");
                    break;

                case TipoBotonAccion.Guardar:
                    btn.BackColor = ColorVinoPrincipal;
                    btn.ForeColor = ColorDorado;
                    btn.FlatAppearance.MouseOverBackColor = ColorVinoClaro;
                    btn.Image = ObtenerIcono("icon_save");
                    break;

                case TipoBotonAccion.Secundario:
                    btn.BackColor = Color.FromArgb(150, 140, 130);
                    btn.ForeColor = Color.White;
                    btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(130, 120, 110);
                    break;
            }

            if (!string.IsNullOrEmpty(textoConIcono))
            {
                btn.Text = textoConIcono;
            }
        }
    }
}
