using Control_de_Inventario.Entindades;
using Control_de_Inventario.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Control_de_Inventario
{
    public partial class Form1 : Form
    {
        private InvenatarioRepository repo;
        private bool isEditing = false;
        private int editingId = 0;
        public Form1()
        {
            InitializeComponent();
            repo = new InvenatarioRepository();
            CargarProductos();
            
            ConfigurarControles();
        }

        private void ConfigurarControles()
        {
            // Configurar DataGridView
            dgvInventario.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvInventario.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvInventario.MultiSelect = false;

            // Configurar validación de cantidad y precio
            txtCantidad.KeyPress += ValidarNumeroEntero;
            txtPrecioCompra.KeyPress += ValidarNumeroDecimal;
        }
        private void CargarProductos()
        {
            dgvInventario.DataSource = null;
            dgvInventario.DataSource = repo.ObtenerTodos();

            // Agregar columna Valor Total si no existe
            if (!dgvInventario.Columns.Contains("ValorTotal"))
            {
                DataGridViewTextBoxColumn colValorTotal = new DataGridViewTextBoxColumn();
                colValorTotal.Name = "ValorTotal";
                colValorTotal.HeaderText = "Valor Total";
                colValorTotal.ReadOnly = true;
                dgvInventario.Columns.Add(colValorTotal);
            }
            foreach (DataGridViewRow row in dgvInventario.Rows)
            {
                if (row.Cells["Cantidad"].Value != null && row.Cells["PrecioCompra"].Value != null)
                {
                    int cantidad = Convert.ToInt32(row.Cells["Cantidad"].Value);
                    decimal precio = Convert.ToDecimal(row.Cells["PrecioCompra"].Value);
                    row.Cells["ValorTotal"].Value = cantidad * precio;
                }
            }
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos())
                return;

            Inventarioo producto = new Inventarioo
            {
                Producto = txtProducto.Text.Trim(),
                Categoria = txtCategoria.Text.Trim(),
                Cantidad = int.Parse(txtCantidad.Text),
                PrecioCompra = decimal.Parse(txtPrecioCompra.Text)
            };

            repo.Registrar(producto);
            CargarProductos();
            
            LimpiarCampos();

            MessageBox.Show("Producto registrado correctamente", "Éxito",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            if (!isEditing)
            {
                MessageBox.Show("Seleccione un producto para actualizar", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidarCampos())
                return;

            Inventarioo producto = new Inventarioo
            {
                Id = editingId,
                Producto = txtProducto.Text.Trim(),
                Categoria = txtCategoria.Text.Trim(),
                Cantidad = int.Parse(txtCantidad.Text),
                PrecioCompra = decimal.Parse(txtPrecioCompra.Text)
            };

            repo.Actualizar(producto);
            CargarProductos();
            
            LimpiarCampos();

            isEditing = false;
            editingId = 0;
            btnRegistrar.Enabled = true;

            MessageBox.Show("Producto actualizado correctamente", "Éxito",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvInventario.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un producto para eliminar", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = (int)dgvInventario.CurrentRow.Cells["Id"].Value;
            string producto = dgvInventario.CurrentRow.Cells["Producto"].Value.ToString();

            DialogResult result = MessageBox.Show($"¿Eliminar el producto '{producto}'?",
                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                repo.Eliminar(id);
                CargarProductos();
                
                LimpiarCampos();

                MessageBox.Show("Producto eliminado correctamente", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnRefrescar_Click(object sender, EventArgs e)
        {
            CargarProductos();
            LimpiarCampos();
        }

        private void dgvInventario_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvInventario.Rows[e.RowIndex];
                editingId = (int)row.Cells["Id"].Value;
                txtProducto.Text = row.Cells["Producto"].Value.ToString();
                txtCategoria.Text = row.Cells["Categoria"].Value.ToString();
                txtCantidad.Text = row.Cells["Cantidad"].Value.ToString();
                txtPrecioCompra.Text = row.Cells["PrecioCompra"].Value.ToString();

                isEditing = true;
                btnRegistrar.Enabled = false;
                btnActualizar.Enabled = true;

                MessageBox.Show("Modifique los datos y presione 'Actualizar'", "Modo Edición",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtProducto.Text))
            {
                MessageBox.Show("El nombre del producto es obligatorio", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtProducto.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtCategoria.Text))
            {
                MessageBox.Show("La categoría es obligatoria", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtCategoria.Focus();
                return false;
            }

            if (!int.TryParse(txtCantidad.Text, out int cantidad) || cantidad < 0)
            {
                MessageBox.Show("Ingrese una cantidad válida (0 o más)", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtCantidad.Focus();
                return false;
            }

            if (!decimal.TryParse(txtPrecioCompra.Text, out decimal precio) || precio <= 0)
            {
                MessageBox.Show("Ingrese un precio de compra válido (mayor a 0)", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPrecioCompra.Focus();
                return false;
            }

            return true;
        }
        private void LimpiarCampos()
        {
            txtProducto.Clear();
            txtCategoria.Clear();
            txtCantidad.Clear();
            txtPrecioCompra.Clear();
            txtProducto.Focus();
        }
        private void ValidarNumeroEntero(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void ValidarNumeroDecimal(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            if (e.KeyChar == '.' && (sender as TextBox).Text.Contains("."))
            {
                e.Handled = true;
            }
        }
    }
}

