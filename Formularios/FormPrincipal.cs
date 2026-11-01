using System;
using System.Drawing;
using System.Windows.Forms;
using RestaurantePOS.Formularios;
using RestaurantePOS.Modelos;

namespace RestaurantePOS
{
    public partial class FormPrincipal : Form
    {
        private Form formActivo = null;

        public FormPrincipal()
        {
            InitializeComponent();
            // Arranca maximizada: se adapta a cualquier pantalla (incluida 1366x768).
            this.WindowState = FormWindowState.Maximized;
        }

        /// <summary>
        /// Valida si el puesto del usuario tiene permiso para acceder a un módulo específico.
        /// - Administrador: todo.
        /// - Supervisor: todo menos Empleados.
        /// - Mesero: Mesas, Pedidos y Reservaciones.
        /// - Cajero: Caja, Clientes y Turnos.
        /// - Cocinero: Pedidos pendientes e Insumos.
        /// </summary>
        public static bool TienePermiso(string puesto, string modulo)
        {
            if (string.IsNullOrWhiteSpace(puesto)) return false;

            if (string.Equals(puesto, "Administrador", StringComparison.OrdinalIgnoreCase))
                return true;

            string p = puesto.Trim();

            if (string.Equals(p, "Supervisor", StringComparison.OrdinalIgnoreCase))
            {
                return !string.Equals(modulo, "Empleados", StringComparison.OrdinalIgnoreCase);
            }

            if (string.Equals(p, "Mesero", StringComparison.OrdinalIgnoreCase))
            {
                return modulo == "Mesas" || modulo == "Pedidos" || modulo == "Reservaciones";
            }

            if (string.Equals(p, "Cajero", StringComparison.OrdinalIgnoreCase))
            {
                return modulo == "Caja" || modulo == "Clientes" || modulo == "Turnos";
            }

            if (string.Equals(p, "Cocinero", StringComparison.OrdinalIgnoreCase))
            {
                return modulo == "PedidosCocina" || modulo == "PedidosPendientes" || modulo == "Insumos";
            }

            return false;
        }

        private void FormPrincipal_Load(object sender, EventArgs e)
        {
            // Título de ventana con usuario y puesto actual
            this.Text = $"RestaurantePOS - {Sesion.Nombre} [{Sesion.Puesto}]";

            // Configurar diseño visual e iconos de los botones superiores
            UITheme.ConfigurarBotonMenuSuperior(btnMesas, "icon_mesas");
            UITheme.ConfigurarBotonMenuSuperior(btnPedidos, "icon_pedidos");
            UITheme.ConfigurarBotonMenuSuperior(btnReservaciones, "icon_reservaciones");
            UITheme.ConfigurarBotonMenuSuperior(btnCaja, "icon_caja");
            UITheme.ConfigurarBotonMenuSuperior(btnTurnos, "icon_turnos");
            UITheme.ConfigurarBotonMenuSuperior(btnProductos, "icon_productos");
            UITheme.ConfigurarBotonMenuSuperior(btnInsumos, "icon_insumos");
            UITheme.ConfigurarBotonMenuSuperior(btnRecetas, "icon_recetas");
            UITheme.ConfigurarBotonMenuSuperior(btnPromociones, "icon_promociones");
            UITheme.ConfigurarBotonMenuSuperior(btnClientes, "icon_clientes");
            UITheme.ConfigurarBotonMenuSuperior(btnEmpleados, "icon_empleados");
            UITheme.ConfigurarBotonMenuSuperior(btnProveedores, "icon_proveedores");
            UITheme.ConfigurarBotonMenuSuperior(btnMetodosPago, "icon_pagos");
            UITheme.ConfigurarBotonMenuSuperior(btnReportes, "icon_reportes");
            UITheme.ConfigurarBotonMenuSuperior(btnCerrarSesion, "icon_logout", true);

            // Mostrar solo botones permitidos según el Puesto
            AplicarPermisosVisuales();

            // Iniciar en la pantalla correspondiente al puesto
            IniciarPantallaPorPuesto();
        }

        private void AplicarPermisosVisuales()
        {
            string puesto = Sesion.Puesto;

            btnMesas.Visible = TienePermiso(puesto, "Mesas");
            btnPedidos.Visible = TienePermiso(puesto, "Pedidos") || TienePermiso(puesto, "PedidosCocina");
            btnReservaciones.Visible = TienePermiso(puesto, "Reservaciones");
            btnCaja.Visible = TienePermiso(puesto, "Caja");
            btnTurnos.Visible = TienePermiso(puesto, "Turnos");
            btnProductos.Visible = TienePermiso(puesto, "Productos");
            btnInsumos.Visible = TienePermiso(puesto, "Insumos");
            btnRecetas.Visible = TienePermiso(puesto, "Recetas");
            btnPromociones.Visible = TienePermiso(puesto, "Promociones");
            btnClientes.Visible = TienePermiso(puesto, "Clientes");
            btnEmpleados.Visible = TienePermiso(puesto, "Empleados");
            btnProveedores.Visible = TienePermiso(puesto, "Proveedores");
            btnMetodosPago.Visible = TienePermiso(puesto, "MetodosPago");
            btnReportes.Visible = TienePermiso(puesto, "Reportes");
            btnCerrarSesion.Visible = true;

            lblCatOperacion.Visible = btnMesas.Visible || btnPedidos.Visible || btnReservaciones.Visible || btnCaja.Visible || btnTurnos.Visible;
            lblCatCatalogo.Visible = btnProductos.Visible || btnInsumos.Visible || btnRecetas.Visible || btnPromociones.Visible;
            lblCatPersonas.Visible = btnClientes.Visible || btnEmpleados.Visible || btnProveedores.Visible;
            lblCatAnalisis.Visible = btnMetodosPago.Visible || btnReportes.Visible;

            // Personalización para Cocinero
            if (string.Equals(puesto, "Cocinero", StringComparison.OrdinalIgnoreCase))
            {
                btnPedidos.Text = "  Cocina";
            }
        }

        private void IniciarPantallaPorPuesto()
        {
            string puesto = Sesion.Puesto;

            if (string.Equals(puesto, "Cocinero", StringComparison.OrdinalIgnoreCase))
            {
                AbrirPedidosCocina();
            }
            else if (string.Equals(puesto, "Cajero", StringComparison.OrdinalIgnoreCase))
            {
                AbrirFormularioEnPanel("Caja", new FormCaja(), btnCaja);
            }
            else
            {
                AbrirFormularioMesas();
            }
        }

        private void AbrirFormularioMesas()
        {
            if (!TienePermiso(Sesion.Puesto, "Mesas"))
            {
                MessageBox.Show("No tiene autorización para ver el módulo de Mesas.", "Acceso Restringido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var formMesas = new FormMesas((idMesa) =>
            {
                AbrirFormularioEnPanel("Pedidos", new FormPedidos(idMesa), btnPedidos);
            });
            AbrirFormularioEnPanel("Mesas", formMesas, btnMesas);
        }

        private void AbrirPedidosCocina()
        {
            var formCocina = new FormCaja(true);
            AbrirFormularioEnPanel("PedidosCocina", formCocina, btnPedidos);
        }

        private void AbrirFormularioEnPanel(string modulo, Form formHijo, Button botonSeleccionado)
        {
            if (!TienePermiso(Sesion.Puesto, modulo))
            {
                MessageBox.Show($"Su puesto ({Sesion.Puesto}) no tiene autorización para acceder al módulo de {modulo}.",
                                "Acceso Restringido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (formActivo != null)
            {
                formActivo.Close();
                pnlContent.Controls.Clear();
            }

            if (botonSeleccionado != null)
            {
                UITheme.MarcarBotonMenuActivo(botonSeleccionado, flpNav);
            }

            formActivo = formHijo;
            formHijo.TopLevel = false;
            formHijo.FormBorderStyle = FormBorderStyle.None;
            formHijo.Dock = DockStyle.Fill;
            pnlContent.Controls.Add(formHijo);
            pnlContent.Tag = formHijo;
            formHijo.BringToFront();
            formHijo.Show();
        }

        private void btnMesas_Click(object sender, EventArgs e)
        {
            AbrirFormularioMesas();
        }

        private void btnPedidos_Click(object sender, EventArgs e)
        {
            if (string.Equals(Sesion.Puesto, "Cocinero", StringComparison.OrdinalIgnoreCase))
            {
                AbrirPedidosCocina();
            }
            else
            {
                AbrirFormularioEnPanel("Pedidos", new FormPedidos(), btnPedidos);
            }
        }

        private void btnReservaciones_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel("Reservaciones", new FormReservaciones(), btnReservaciones);
        }

        private void btnCaja_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel("Caja", new FormCaja(), btnCaja);
        }

        private void btnTurnos_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel("Turnos", new FormTurnos(), btnTurnos);
        }

        private void btnProductos_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel("Productos", new FormProductos(), btnProductos);
        }

        private void btnInsumos_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel("Insumos", new FormInsumos(), btnInsumos);
        }

        private void btnRecetas_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel("Recetas", new FormRecetas(), btnRecetas);
        }

        private void btnPromociones_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel("Promociones", new FormPromociones(), btnPromociones);
        }

        private void btnClientes_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel("Clientes", new FormClientes(), btnClientes);
        }

        private void btnEmpleados_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel("Empleados", new FormEmpleados(), btnEmpleados);
        }

        private void btnProveedores_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel("Proveedores", new FormProveedores(), btnProveedores);
        }

        private void btnMetodosPago_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel("MetodosPago", new FormMetodosPago(), btnMetodosPago);
        }

        private void btnReportes_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel("Reportes", new FormReportes(), btnReportes);
        }

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            var confirmacion = MessageBox.Show("¿Estás seguro de que deseas cerrar sesión?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmacion == DialogResult.Yes)
            {
                Sesion.CerrarSesion();
                this.Close();
            }
        }
    }
}
