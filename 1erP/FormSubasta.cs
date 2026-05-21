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
        private CasaSubastaService casaSubastaService;

        public FormSubasta()
        {
            InitializeComponent();
            this.Load += FormSubasta_Load;
        }

        private void FormSubasta_Load(object sender, EventArgs e)
        {
            casaSubastaService = CasaSubastaService.GetInstance;
            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            ConfigurarFechas();
            ConfigurarGrilla();
            CargarJornadaActiva();
            LimpiarDetalle();
            ActualizarEstadoJornada();
            ActualizarBotonesJornada();
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

        private void CargarJornadaActiva()
        {
            if (!casaSubastaService.CargarJornadaActiva())
            {
                return;
            }

            dateTimePicker1.Value = casaSubastaService.CasaSubasta.FechaInicio;
            dateTimePicker2.Value = casaSubastaService.CasaSubasta.FechaFin;
            CargarGrillaSubastas();
            AgregarNotificacion("Jornada activa recuperada.");
        }

        private void ConfigurarGrilla()
        {
            dgvSubastas.AutoGenerateColumns = true;
            dgvSubastas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSubastas.MultiSelect = false;
            dgvSubastas.ReadOnly = true;
            dgvSubastas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
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
            ActualizarBotonesJornada();
            AgregarNotificacion("Jornada configurada y subastas cargadas correctamente.");
        }

        public void FinalizarJornada_Click(object sender, EventArgs e)
        {
            try
            {
                List<Subasta> subastasActivas = casaSubastaService.ListarSubastas()
                    .Where(s => s.Estado == EstadoSubasta.Activa)
                    .ToList();

                casaSubastaService.FinalizarSubastas(true);

                foreach (Subasta subasta in subastasActivas)
                {
                    AgregarNotificacion(CrearMensajeFinSubasta(subasta));
                }

                CargarGrillaSubastas();
                CargarDetalleSubasta(ObtenerSubastaSeleccionada());
                ActualizarEstadoJornada();
                ActualizarBotonesJornada();
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
                AgregarNotificacion("Dejaste de seguir " + subasta.Articulo.Nombre + ".");
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

                decimal monto;

                if (!decimal.TryParse(txtMontoOferta.Text, out monto))
                {
                    MessageBox.Show("Ingrese un monto valido.");
                    return;
                }

                casaSubastaService.Ofertar(subasta, cliente, monto);

                txtMontoOferta.Clear();
                CargarGrillaSubastas();
                SeleccionarSubasta(subasta);
                CargarDetalleSubasta(subasta);
                AgregarNotificacion("Nueva oferta de " + cliente.Username + " en " + subasta.Articulo.Nombre + ". Precio actual: " + subasta.PrecioFinal);
                AgregarNotificacion("Usuarios notificados: " + ObtenerUsuariosNotificados(subasta) + ".");
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

            AjustarColumnasSubastas();
        }

        private void AjustarColumnasSubastas()
        {
            foreach (DataGridViewColumn columna in dgvSubastas.Columns)
            {
                columna.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            if (dgvSubastas.Columns["Nombre"] != null)
            {
                dgvSubastas.Columns["Nombre"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvSubastas.Columns["Nombre"].MinimumWidth = 120;
            }

            if (dgvSubastas.Columns["Id"] != null)
            {
                dgvSubastas.Columns["Id"].MinimumWidth = 40;
            }
        }

        private Subasta ObtenerSubastaSeleccionada()
        {
            if (dgvSubastas.CurrentRow == null)
            {
                return null;
            }

            SubastaView subastaView = dgvSubastas.CurrentRow.DataBoundItem as SubastaView;

            if (subastaView == null)
            {
                return null;
            }

            return subastaView.Subasta;
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

            lblNombreSubasta.Text = "Nombre: " + subasta.Articulo.Nombre;
            lblTipoSubasta.Text = "Tipo: " + subasta.Articulo.Tipo;
            lblDescripcionSubasta.Text = "Descripcion: " + subasta.Articulo.Descripcion;
            lblPrecioInicial.Text = "Precio inicial: " + subasta.PrecioInicial;
            lblPrecioActual.Text = "Precio actual: " + subasta.PrecioFinal;
            lblMejorPostor.Text = "Mejor postor: " + ObtenerNombreMejorPostor(subasta);
            lblEstadoSubasta.Text = "Estado: " + subasta.Estado;
            lblCantidadInteresados.Text = "Interesados: " + subasta.Interesados.Count;

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
                lblEstado.Text = "Estado jornada: \nSin configurar";
                return;
            }

            if (ahora < inicio)
            {
                lblEstado.Text = "Estado jornada: \nNo iniciada";
            }
            else if (ahora > fin)
            {
                lblEstado.Text = "Estado jornada: \nFinalizada";
            }
            else
            {
                lblEstado.Text = "Estado jornada: \nActiva";
            }
        }

        private void ActualizarBotonesJornada()
        {
            bool hayJornadaActiva = casaSubastaService.ListarSubastas()
                .Any(s => s.Estado == EstadoSubasta.Activa);

            button1.Enabled = !hayJornadaActiva;
            button2.Enabled = hayJornadaActiva;
        }

        private string CrearMensajeFinSubasta(Subasta subasta)
        {
            return "Finalizo " + subasta.Articulo.Nombre + ". Ganador: " + ObtenerNombreMejorPostor(subasta) + ". Precio final: " + subasta.PrecioFinal;
        }

        private string ObtenerNombreMejorPostor(Subasta subasta)
        {
            return subasta.MejorPostor != null
                ? subasta.MejorPostor.Username
                : "Sin ofertas";
        }

        private string ObtenerUsuariosNotificados(Subasta subasta)
        {
            List<string> usuarios = subasta.Interesados
                .OfType<Cliente>()
                .Select(c => c.Username)
                .Distinct()
                .ToList();

            if (usuarios.Count == 0)
            {
                return "Sin usuarios";
            }

            return string.Join(", ", usuarios);
        }

        private void AgregarNotificacion(string mensaje)
        {
            listBox2.Items.Add(DateTime.Now.ToString("HH:mm:ss") + " - " + mensaje);
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

            public int Id
            {
                get { return Subasta.Id; }
            }

            public string Tipo
            {
                get { return Subasta.Articulo.Tipo; }
            }

            public string Nombre
            {
                get { return Subasta.Articulo.Nombre; }
            }

            public decimal PrecioInicial
            {
                get { return Subasta.PrecioInicial; }
            }

            public decimal PrecioActual
            {
                get { return Subasta.PrecioFinal; }
            }

            public string MejorPostor
            {
                get
                {
                    return Subasta.MejorPostor != null
                        ? Subasta.MejorPostor.Username
                        : "Sin ofertas";
                }
            }

            public int Interesados
            {
                get { return Subasta.Interesados.Count; }
            }

            public EstadoSubasta Estado
            {
                get { return Subasta.Estado; }
            }

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
                AgregarNotificacion("Dejaste de seguir " + subasta.Articulo.Nombre + ".");
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }
    }
}
