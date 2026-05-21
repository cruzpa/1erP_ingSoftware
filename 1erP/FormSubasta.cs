using BE;
using BLL;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace _1erP
{
    public partial class FormSubasta : Form
    {
        private readonly CasaSubastaService casaSubastaService = CasaSubastaService.GetInstance;

        public FormSubasta()
        {
            InitializeComponent();
            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            ConfigurarFechas();
            ConfigurarGrilla();
            LimpiarDetalle();
            ActualizarEstadoJornada();
        }

        private void ConfigurarFechas()
        {
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "dd/MM/yyyy HH:mm";
            dateTimePicker1.ShowUpDown = true;
            dateTimePicker1.Value = DateTime.Now;

            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = "dd/MM/yyyy HH:mm";
            dateTimePicker2.ShowUpDown = true;
            dateTimePicker2.Value = DateTime.Now.AddHours(1);
        }

        private void ConfigurarGrilla()
        {
            dgvSubastas.AutoGenerateColumns = true;
            dgvSubastas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSubastas.MultiSelect = false;
            dgvSubastas.ReadOnly = true;
            dgvSubastas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        public void ConfigurarJornada_Click(object sender, EventArgs e)
        {
            try
            {
                ConfigurarJornadaYCargarSubastas();
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }
        private void ConfigurarJornadaYCargarSubastas()
        {
            List<Articulo> articulos = ArticuloService.Listar();

            casaSubastaService.ConfigurarJornada(dateTimePicker1.Value, dateTimePicker2.Value);
            casaSubastaService.CrearSubastasDesdeCatalogo(articulos);

            CargarGrillaSubastas();
            LimpiarDetalle();
            ActualizarEstadoJornada();
            AgregarNotificacion("Jornada configurada y subastas cargadas correctamente.");
        }

        public void FinalizarJornada_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (Subasta subasta in casaSubastaService.ListarSubastas())
                {
                    if (subasta.Estado == EstadoSubasta.Activa)
                    {
                        subasta.Finalizar();
                        AgregarNotificacion(CrearMensajeFinSubasta(subasta));
                    }
                }

                CargarGrillaSubastas();
                CargarDetalleSubasta(ObtenerSubastaSeleccionada());
                ActualizarEstadoJornada();
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        public void dgvSubastas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            CargarDetalleSubasta(ObtenerSubastaSeleccionada());
        }

        public void DejarDeSeguirSubasta_Click(object sender, EventArgs e)
        {
            try
            {
                Subasta subasta = ObtenerSubastaSeleccionada();
                Cliente cliente = ObtenerClienteActual();

                if (subasta == null)
                {
                    MessageBox.Show("Seleccione una subasta.");
                    return;
                }

                casaSubastaService.Desuscribir(subasta, cliente);

                CargarGrillaSubastas();
                CargarDetalleSubasta(subasta);
                AgregarNotificacion($"Dejaste de seguir {subasta.Articulo.Nombre}.");
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void btnOfertar_Click(object sender, EventArgs e)
        {
            try
            {
                Subasta subasta = ObtenerSubastaSeleccionada();
                Cliente cliente = ObtenerClienteActual();

                if (subasta == null)
                {
                    MessageBox.Show("Seleccione una subasta.");
                    return;
                }

                if (!decimal.TryParse(txtMontoOferta.Text, out decimal monto))
                {
                    MessageBox.Show("Ingrese un monto valido.");
                    return;
                }

                casaSubastaService.Ofertar(subasta, cliente, monto);

                txtMontoOferta.Clear();
                CargarGrillaSubastas();
                SeleccionarSubasta(subasta);
                CargarDetalleSubasta(subasta);
                AgregarNotificacion($"Nueva oferta en {subasta.Articulo.Nombre}. Precio actual: {subasta.PrecioFinal}");
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }
        private void CargarGrillaSubastas()
        {
            List<SubastaView> subastas = casaSubastaService.ListarSubastas()
                .Select(s => new SubastaView(s))
                .ToList();

            dgvSubastas.DataSource = null;
            dgvSubastas.DataSource = subastas;

            if (dgvSubastas.Columns["Subasta"] != null)
            {
                dgvSubastas.Columns["Subasta"].Visible = false;
            }
        }

        private Subasta ObtenerSubastaSeleccionada()
        {
            if (dgvSubastas.CurrentRow == null)
            {
                return null;
            }

            SubastaView subastaView = dgvSubastas.CurrentRow.DataBoundItem as SubastaView;
            return subastaView?.Subasta;
        }

        private void SeleccionarSubasta(Subasta subasta)
        {
            if (subasta == null)
            {
                return;
            }

            foreach (DataGridViewRow row in dgvSubastas.Rows)
            {
                SubastaView subastaView = row.DataBoundItem as SubastaView;

                if (subastaView != null && subastaView.Subasta == subasta)
                {
                    row.Selected = true;
                    dgvSubastas.CurrentCell = row.Cells[0];
                    return;
                }
            }
        }
        private void CargarDetalleSubasta(Subasta subasta)
        {
            if (subasta == null)
            {
                LimpiarDetalle();
                return;
            }

            lblNombreSubasta.Text = $"Nombre: {subasta.Articulo.Nombre}";
            lblTipoSubasta.Text = $"Tipo: {subasta.Articulo.Tipo}";
            lblDescripcionSubasta.Text = $"Descripcion: {subasta.Articulo.Descripcion}";
            lblPrecioInicial.Text = $"Precio inicial: {subasta.PrecioInicial}";
            lblPrecioActual.Text = $"Precio actual: {subasta.PrecioFinal}";
            lblMejorPostor.Text = $"Mejor postor: {ObtenerNombreMejorPostor(subasta)}";
            lblEstadoSubasta.Text = $"Estado: {subasta.Estado}";
            lblCantidadInteresados.Text = $"Interesados: {subasta.Interesados.Count}";

            CargarArticulosLote(subasta);
        }

        private void CargarArticulosLote(Subasta subasta)
        {
            listBox1.Items.Clear();

            Lote lote = subasta.Articulo as Lote;
            bool esLote = lote != null;

            lstArticulosLote.Visible = esLote;
            listBox1.Visible = esLote;

            if (!esLote)
            {
                return;
            }

            foreach (Articulo articulo in lote.Articulos)
            {
                listBox1.Items.Add(articulo);
            }
        }

        private void LimpiarDetalle()
        {
            lblNombreSubasta.Text = "Nombre:";
            lblTipoSubasta.Text = "Tipo:";
            lblDescripcionSubasta.Text = "Descripcion:";
            lblPrecioInicial.Text = "Precio inicial:";
            lblPrecioActual.Text = "Precio actual:";
            lblMejorPostor.Text = "Mejor postor:";
            lblEstadoSubasta.Text = "Estado:";
            lblCantidadInteresados.Text = "Interesados:";

            listBox1.Items.Clear();
            lstArticulosLote.Visible = false;
            listBox1.Visible = false;
        }

        private Cliente ObtenerClienteActual()
        {
            Cliente cliente = SessionManager.GetInstance.usuario as Cliente;

            if (cliente == null)
            {
                throw new InvalidOperationException("Debe iniciar sesion como cliente para participar en una subasta.");
            }

            return cliente;
        }

        private void ActualizarEstadoJornada()
        {
            DateTime ahora = DateTime.Now;
            DateTime inicio = casaSubastaService.CasaSubasta.FechaInicio;
            DateTime fin = casaSubastaService.CasaSubasta.FechaFin;

            if (inicio == DateTime.MinValue || fin == DateTime.MinValue)
            {
                lblEstado.Text = "Estado jornada: Sin configurar";
                return;
            }

            if (ahora < inicio)
            {
                lblEstado.Text = "Estado jornada: No iniciada";
            }
            else if (ahora > fin)
            {
                lblEstado.Text = "Estado jornada: Finalizada";
            }
            else
            {
                lblEstado.Text = "Estado jornada: Activa";
            }
        }

        private string CrearMensajeFinSubasta(Subasta subasta)
        {
            return $"Finalizo {subasta.Articulo.Nombre}. Ganador: {ObtenerNombreMejorPostor(subasta)}. Precio final: {subasta.PrecioFinal}";
        }

        private string ObtenerNombreMejorPostor(Subasta subasta)
        {
            return subasta.MejorPostor != null
                ? subasta.MejorPostor.Username
                : "Sin ofertas";
        }

        private void AgregarNotificacion(string mensaje)
        {
            listBox2.Items.Add($"{DateTime.Now:HH:mm:ss} - {mensaje}");
            listBox2.TopIndex = listBox2.Items.Count - 1;
        }

        private void MostrarError(Exception ex)
        {
            MessageBox.Show(ex.Message);
            AgregarNotificacion(ex.Message);
        }

        private class SubastaView
        {
            public SubastaView(Subasta subasta)
            {
                Subasta = subasta;
            }

            public int Id => Subasta.Id;
            public string Tipo => Subasta.Articulo.Tipo;
            public string Nombre => Subasta.Articulo.Nombre;
            public decimal PrecioInicial => Subasta.PrecioInicial;
            public decimal PrecioActual => Subasta.PrecioFinal;
            public string MejorPostor => Subasta.MejorPostor != null ? Subasta.MejorPostor.Username : "Sin ofertas";
            public int Interesados => Subasta.Interesados.Count;
            public EstadoSubasta Estado => Subasta.Estado;
            public Subasta Subasta { get; private set; }
        }

        private void btnDejarSeguir_Click(object sender, EventArgs e)
        {
            try
            {
                Subasta subasta = ObtenerSubastaSeleccionada();
                Cliente cliente = ObtenerClienteActual();

                if (subasta == null)
                {
                    MessageBox.Show("Seleccione una subasta.");
                    return;
                }

                casaSubastaService.Desuscribir(subasta, cliente);

                CargarGrillaSubastas();
                CargarDetalleSubasta(subasta);
                AgregarNotificacion($"Dejaste de seguir {subasta.Articulo.Nombre}.");
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }
    }
}
