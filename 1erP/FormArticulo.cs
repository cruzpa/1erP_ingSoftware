using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace _1erP
{
    public partial class FormArticulo : Form
    {
        List<Articulo> articulos;

        public FormArticulo()
        {
            InitializeComponent();
        }

        private void volver_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormArticulo_Load(object sender, EventArgs e)
        {
            ConfigurarGrillaArticulos();
            CargarArticulos();
            CargarArticulosLote();

        }
        private void CargarArticulosLote()
        {
            clbArticulosLote.Items.Clear();

            foreach (Articulo articulo in articulos)
            {
                clbArticulosLote.Items.Add(articulo);
            }
        }
        private void ConfigurarGrillaArticulos()
        {
            dgvArticulos.AutoGenerateColumns = true;
            dgvArticulos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvArticulos.MultiSelect = false;
            dgvArticulos.ReadOnly = true;
            dgvArticulos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        private void CargarArticulos()
        {
            articulos = ArticuloService.Listar();

            dgvArticulos.DataSource = null;
            dgvArticulos.DataSource = articulos;

            if (dgvArticulos.Columns["Articulos"] != null)
            {
                dgvArticulos.Columns["Articulos"].Visible = false;
            }

        }
        private Articulo ObtenerArticuloSeleccionado()
        {
            if (dgvArticulos.CurrentRow == null)
            {
                return null;
            }

            return dgvArticulos.CurrentRow.DataBoundItem as Articulo;
        }
        private void dgvArticulos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            Articulo articulo = ObtenerArticuloSeleccionado();

            if (articulo == null) return;

            txtNombre.Text = articulo.Nombre;
            txtDescripcion.Text = articulo.Descripcion;
            txtPrecio.Text = articulo.Precio.ToString();

            rbArticulo.Checked = articulo is Articulo && !(articulo is Lote);
            rbLote.Checked = articulo is Lote;

            txtPrecio.Enabled = !(articulo is Lote);

            //para seleccionar los elementos del lote en el CheckedListBox
            clbArticulosLote.ClearSelected();

            for (int i = 0; i < clbArticulosLote.Items.Count; i++)
            {
                clbArticulosLote.SetItemChecked(i, false);
            }
            if (articulo is Lote lote)
            {
                foreach (Articulo articuloLote in lote.Articulos)
                {
                    for (int i = 0; i < clbArticulosLote.Items.Count; i++)
                    {
                        Articulo item = (Articulo)clbArticulosLote.Items[i];

                        if (item.Id == articuloLote.Id)
                        {
                            clbArticulosLote.SetItemChecked(i, true);
                        }
                    }
                }
            }
        }
        private void crear_Click(object sender, EventArgs e)
        {
            try
            {
                Articulo articulo;

                if (rbArticulo.Checked)
                {
                    articulo = new Articulo();
                    articulo.Precio = decimal.Parse(txtPrecio.Text);
                }
                else
                {
                    Lote lote = new Lote();
                    lote.Articulos = new List<Articulo>();

                    foreach (object item in clbArticulosLote.CheckedItems)
                    {
                        lote.Articulos.Add((Articulo)item);
                    }

                    articulo = lote;
                }

                articulo.Nombre = txtNombre.Text;
                articulo.Descripcion = txtDescripcion.Text;

                ArticuloService.Crear(articulo);

                MessageBox.Show("Artículo creado correctamente");

                LimpiarFormulario();
                CargarArticulos();
                CargarArticulosLote();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void LimpiarFormulario()
        {
            txtNombre.Clear();
            txtDescripcion.Clear();
            txtPrecio.Clear();

            rbArticulo.Checked = true;
            rbLote.Checked = false;

            for (int i = 0; i < clbArticulosLote.Items.Count; i++)
            {
                clbArticulosLote.SetItemChecked(i, false);
            }
        }

        private void editar_Click(object sender, EventArgs e)
        {
            try
            {
                Articulo articulo = ObtenerArticuloSeleccionado();

                if (articulo == null)
                {
                    MessageBox.Show("Seleccione un artículo");
                    return;
                }

                articulo.Nombre = txtNombre.Text;
                articulo.Descripcion = txtDescripcion.Text;

                if (articulo is Lote lote)
                {
                    lote.Articulos.Clear();

                    foreach (object item in clbArticulosLote.CheckedItems)
                    {
                        Articulo itemArticulo = (Articulo)item;

                        if (itemArticulo.Id != lote.Id)
                        {
                            lote.Articulos.Add(itemArticulo);
                        }
                    }
                }
                else
                {
                    articulo.Precio = decimal.Parse(txtPrecio.Text);
                }

                ArticuloService.Editar(articulo);

                MessageBox.Show("Artículo editado correctamente");

                LimpiarFormulario();
                CargarArticulos();
                CargarArticulosLote();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void borrar_Click(object sender, EventArgs e)
        {
            try
            {
                Articulo articulo = ObtenerArticuloSeleccionado();

                if (articulo == null)
                {
                    MessageBox.Show("Seleccione un artículo");
                    return;
                }

                DialogResult respuesta = MessageBox.Show(
                    "¿Seguro que desea borrar el artículo seleccionado?",
                    "Confirmar borrado",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (respuesta != DialogResult.Yes)
                {
                    return;
                }

                ArticuloService.Borrar(articulo);

                MessageBox.Show("Artículo borrado correctamente");

                LimpiarFormulario();
                CargarArticulos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void rblote_CheckedChanged(object sender, EventArgs e)
        {
            bool esLote = rbLote.Checked;

            if(esLote)
            {
                clbArticulosLote.Visible = true;
                labelContenidoLote.Visible = true;
                txtPrecio.Enabled = false;
                //txtPrecio.Text = "0";
            } else
            {
                clbArticulosLote.Visible = false;
                labelContenidoLote.Visible = false;
                txtPrecio.Enabled = true;
            }

            //btnAgregar.Visible = esLote;
            //btnQuitar.Visible = esLote;
        }
    }


}
