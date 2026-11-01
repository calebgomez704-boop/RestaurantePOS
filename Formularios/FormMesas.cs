using RestaurantePOS.DAL;
using RestaurantePOS.Modelos;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace RestaurantePOS.Formularios
{
    /// <summary>
    /// Formulario 'FormMesas': Mapa visual del salón con tarjetas dinámicas de estado por mesa.
    /// Muestra el nombre del mesero que atiende la mesa cuando está ocupada (PARTE 1, Punto 4).
    /// </summary>
    public partial class FormMesas : Form
    {
        private Action<int> callbackAbrirPedido;
        private List<Mesa> listaMesas = new List<Mesa>();
        private List<clsPedido> listaPedidosPendientes = new List<clsPedido>();
        private List<Empleado> listaEmpleados = new List<Empleado>();

        public FormMesas()
        {
            InitializeComponent();
        }

        public FormMesas(Action<int> abrirPedidoAction) : this()
        {
            this.callbackAbrirPedido = abrirPedidoAction;
        }

        private void FormMesas_Load(object sender, EventArgs e)
        {
            CargarMesas();
        }

        public void CargarMesas()
        {
            flpMesas.Controls.Clear();

            try
            {
                listaMesas = Repositorio.ObtenerMesas();
                listaPedidosPendientes = Repositorio.ObtenerPedidos("Pendiente");
                listaEmpleados = Repositorio.ObtenerEmpleados(false);

                int disponibles = 0;
                int ocupadas = 0;

                foreach (var mesa in listaMesas)
                {
                    bool libre = mesa.Estado.Equals("Disponible", StringComparison.OrdinalIgnoreCase);
                    if (libre) disponibles++;
                    else ocupadas++;

                    Panel pnlTarjeta = CrearTarjetaMesa(mesa);
                    flpMesas.Controls.Add(pnlTarjeta);
                }

                lblResumen.Text = $"Mesas Libres/Disponibles: {disponibles} | Mesas Ocupadas: {ocupadas} | Total: {listaMesas.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar mesas: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Panel CrearTarjetaMesa(Mesa mesa)
        {
            bool esDisponible = mesa.Estado.Equals("Disponible", StringComparison.OrdinalIgnoreCase);

            Panel card = new Panel
            {
                Size = new Size(205, 155),
                Margin = new Padding(10),
                BackColor = UITheme.ColorFondoTarjeta,
                BorderStyle = BorderStyle.FixedSingle,
                Cursor = esDisponible ? Cursors.Hand : Cursors.Default
            };

            // Franja superior de estado
            Panel headerEstado = new Panel
            {
                Dock = DockStyle.Top,
                Height = 26,
                BackColor = esDisponible ? UITheme.ColorVerdeExito : UITheme.ColorVinoOscuro
            };

            Label lblEstado = new Label
            {
                Dock = DockStyle.Fill,
                Text = mesa.Estado.ToUpper(),
                ForeColor = UITheme.ColorBlanco,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };
            headerEstado.Controls.Add(lblEstado);

            // Título de la mesa
            Label lblNumero = new Label
            {
                Text = $"MESA {mesa.Numero}",
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = UITheme.ColorTextoOscuro,
                Location = new Point(10, 32),
                Size = new Size(185, 25),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Capacidad
            Label lblCapacidad = new Label
            {
                Text = $"Capacidad: {mesa.Capacidad} personas",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Regular),
                ForeColor = UITheme.ColorTextoGris,
                Location = new Point(10, 58),
                Size = new Size(185, 18),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // PARTE 1 (Punto 4): Mostrar el nombre del mesero que atiende la mesa
            string meseroTexto = "Disponible";
            Color meseroColor = UITheme.ColorTextoGris;

            if (!esDisponible)
            {
                var pedidoActivo = listaPedidosPendientes.FirstOrDefault(p => p.IdMesa == mesa.IdMesa);
                if (pedidoActivo != null && pedidoActivo.IdEmpleado.HasValue)
                {
                    var emp = listaEmpleados.FirstOrDefault(e => e.IdEmpleado == pedidoActivo.IdEmpleado.Value);
                    meseroTexto = emp != null ? $"Mesero: {emp.Nombre}" : "Mesero: Sin asignar";
                }
                else
                {
                    meseroTexto = "Mesero: Sin asignar";
                }
                meseroColor = UITheme.ColorVinoOscuro;
            }

            Label lblMesero = new Label
            {
                Text = meseroTexto,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = meseroColor,
                Location = new Point(10, 80),
                Size = new Size(185, 20),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Botón de acción
            Button btnAccion = new Button
            {
                Text = esDisponible ? "Tomar Orden" : "Ocupada",
                Size = new Size(175, 30),
                Location = new Point(15, 110),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = esDisponible ? Cursors.Hand : Cursors.Default,
                Enabled = esDisponible
            };

            if (esDisponible)
            {
                btnAccion.BackColor = UITheme.ColorVinoPrincipal;
                btnAccion.ForeColor = UITheme.ColorDorado;
                btnAccion.FlatAppearance.BorderSize = 0;
                btnAccion.Click += (s, e) => IniciarPedido(mesa.IdMesa);
                card.Click += (s, e) => IniciarPedido(mesa.IdMesa);
            }
            else
            {
                btnAccion.BackColor = UITheme.ColorBotonDeshabilitadoFondo;
                btnAccion.ForeColor = UITheme.ColorBotonDeshabilitadoTexto;
                btnAccion.FlatAppearance.BorderSize = 0;
            }

            card.Controls.Add(headerEstado);
            card.Controls.Add(lblNumero);
            card.Controls.Add(lblCapacidad);
            card.Controls.Add(lblMesero);
            card.Controls.Add(btnAccion);

            return card;
        }

        private void IniciarPedido(int idMesa)
        {
            if (callbackAbrirPedido != null)
            {
                callbackAbrirPedido(idMesa);
            }
            else
            {
                var formPedido = new FormPedidos(idMesa);
                formPedido.ShowDialog(this);
                CargarMesas();
            }
        }

        private void btnRefrescar_Click(object sender, EventArgs e)
        {
            CargarMesas();
        }
    }
}
