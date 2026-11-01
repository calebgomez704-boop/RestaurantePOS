using System;
using System.Drawing;
using System.Windows.Forms;
using RestaurantePOS.DAL;
using RestaurantePOS.Modelos;

namespace RestaurantePOS
{
    /// <summary>
    /// Formulario 'FormProductos': Catálogo y administración de platillos y bebidas (CRUD).
    /// Maneja el enlace a categorías mediante ComboBox usando CategoriaDAL, formato condicional
    /// en el DataGridView (semáforo de stock) y persistencia mediante ProductoDAL.
    /// </summary>
    public partial class FormProductos : Form
    {
        private readonly ProductoDAL _productoDAL = new ProductoDAL();
        private readonly CategoriaDAL _categoriaDAL = new CategoriaDAL();
        private int _idSeleccionado = 0;

        public FormProductos()
        {
            InitializeComponent();
        }

        private void FormProductos_Load(object sender, EventArgs e)
        {
            CargarCategorias();
            CargarProductos();

            // Aplicar estilo visual moderno al DataGridView de productos
            UITheme.AplicarEstiloDataGridView(productosDataGridView);

            // Estilizar los botones CRUD con iconos y colores consistentes
            UITheme.ConfigurarBotonAccion(btnAgregar, UITheme.TipoBotonAccion.Agregar, "  Agregar");
            UITheme.ConfigurarBotonAccion(btnEditar, UITheme.TipoBotonAccion.Editar, "  Editar");
            UITheme.ConfigurarBotonAccion(btnEliminar, UITheme.TipoBotonAccion.Eliminar, "  Eliminar");
        }

        private void CargarCategorias()
        {
            try
            {
                var categorias = _categoriaDAL.Listar();
                cmbCategoria.DataSource = categorias;
                cmbCategoria.DisplayMember = "Nombre";
                cmbCategoria.ValueMember = "IdCategoria";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar categorías: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarProductos()
        {
            try
            {
                productosDataGridView.DataSource = null;
                productosDataGridView.DataSource = _productoDAL.Listar();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los productos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void productosDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (productosDataGridView.CurrentRow == null || productosDataGridView.CurrentRow.DataBoundItem == null)
            {
                _idSeleccionado = 0;
                return;
            }

            var prod = productosDataGridView.CurrentRow.DataBoundItem as Producto;
            if (prod != null)
            {
                _idSeleccionado = prod.IdProducto;
                txtNombre.Text = prod.Nombre;
                if (cmbCategoria.Items.Count > 0)
                {
                    cmbCategoria.SelectedValue = prod.IdCategoria;
                }
                txtPrecio.Text = prod.Precio.ToString("0.00");
                txtStock.Text = prod.Stock.ToString();
            }
        }

        private void productosDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var fila = productosDataGridView.Rows[e.RowIndex];
            if (fila.DataBoundItem is Producto prod)
            {
                if (prod.Stock < 5)
                    fila.DefaultCellStyle.BackColor = UITheme.ColorStockAgotado;
                else if (prod.Stock < 15)
                    fila.DefaultCellStyle.BackColor = UITheme.ColorStockBajo;
                else
                    fila.DefaultCellStyle.BackColor = UITheme.ColorBlanco;
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbCategoria.SelectedValue == null)
                {
                    MessageBox.Show("Debe seleccionar una categoría.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!decimal.TryParse(txtPrecio.Text.Trim(), out decimal precio))
                {
                    MessageBox.Show("El precio debe ser un número válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!int.TryParse(txtStock.Text.Trim(), out int stock))
                {
                    MessageBox.Show("El stock debe ser un número entero válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Producto producto = new Producto
                {
                    Nombre = txtNombre.Text,
                    IdCategoria = Convert.ToInt32(cmbCategoria.SelectedValue),
                    Precio = precio,
                    Stock = stock
                };

                _productoDAL.Insertar(producto);
                MessageBox.Show("Producto agregado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarProductos();
            }
            catch (FormatException)
            {
                MessageBox.Show("Precio y Stock deben ser números válidos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Dato inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar el producto: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (_idSeleccionado <= 0)
            {
                MessageBox.Show("Seleccioná un producto de la lista primero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (cmbCategoria.SelectedValue == null)
                {
                    MessageBox.Show("Debe seleccionar una categoría.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!decimal.TryParse(txtPrecio.Text.Trim(), out decimal precio))
                {
                    MessageBox.Show("El precio debe ser un número válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!int.TryParse(txtStock.Text.Trim(), out int stock))
                {
                    MessageBox.Show("El stock debe ser un número entero válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Producto producto = new Producto
                {
                    IdProducto = _idSeleccionado,
                    Nombre = txtNombre.Text,
                    IdCategoria = Convert.ToInt32(cmbCategoria.SelectedValue),
                    Precio = precio,
                    Stock = stock
                };

                _productoDAL.Actualizar(producto);
                MessageBox.Show("Producto actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarProductos();
            }
            catch (FormatException)
            {
                MessageBox.Show("Precio y Stock deben ser números válidos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Dato inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar el producto: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (_idSeleccionado <= 0)
            {
                MessageBox.Show("Seleccioná un producto de la lista primero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmacion = MessageBox.Show("¿Seguro que deseas eliminar este producto?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmacion == DialogResult.Yes)
            {
                try
                {
                    _productoDAL.Eliminar(_idSeleccionado);
                    MessageBox.Show("Producto eliminado.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarProductos();
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar el producto: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LimpiarCampos()
        {
            _idSeleccionado = 0;
            txtNombre.Clear();
            txtPrecio.Clear();
            txtStock.Clear();
        }
    }
}
