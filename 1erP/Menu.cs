using BE;
using Servicios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _1erP
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void aBMArticulosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormArticulo formArticulo = new FormArticulo();
            formArticulo.MdiParent = this;
            formArticulo.FormBorderStyle = FormBorderStyle.None;
            formArticulo.Dock = DockStyle.Fill;

            formArticulo.Show();
        }

        private void verSubastasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSubasta formSubasta = new FormSubasta();
            formSubasta.MdiParent = this;
            formSubasta.FormBorderStyle = FormBorderStyle.None;
            formSubasta.Dock = DockStyle.Fill;
            formSubasta.FormClosed += FormSubasta_FormClosed;

            MostrarSoloCerrarSesion();
            formSubasta.Show();
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            MostrarLogin();
        }

        public void ValidarPermisos()
        {
            Usuario usuario = SessionManager.GetInstance.usuario;

            if (usuario == null)
            {
                menuStrip1.Visible = false;
                aBMArticulosToolStripMenuItem.Visible = false;
                verSubastasToolStripMenuItem.Visible = false;
                cerrarSesionToolStripMenuItem.Visible = false;
                return;
            }

            menuStrip1.Visible = true;
            aBMArticulosToolStripMenuItem.Visible =
                usuario.Permisos.Contains(Permiso.ABM_ARTICULOS);
            verSubastasToolStripMenuItem.Visible =
                usuario.Permisos.Contains(Permiso.PARTICIPAR_SUBASTA);
            cerrarSesionToolStripMenuItem.Visible = true;
        }

        private void MostrarSoloCerrarSesion()
        {
            menuStrip1.Visible = true;
            aBMArticulosToolStripMenuItem.Visible = false;
            verSubastasToolStripMenuItem.Visible = false;
            cerrarSesionToolStripMenuItem.Visible = true;
        }

        private void MostrarLogin()
        {
            ValidarPermisos();

            FormLogin formLogin = new FormLogin();
            formLogin.MdiParent = this;
            formLogin.FormBorderStyle = FormBorderStyle.None;
            formLogin.Dock = DockStyle.Fill;

            formLogin.Show();
        }

        private void FormSubasta_FormClosed(object sender, FormClosedEventArgs e)
        {
            ValidarPermisos();
        }

        private void cerrarSesionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (SessionManager.GetInstance.usuario != null)
                {
                    SessionManager.Logout();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            MostrarLogin();
        }
    }
}
