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

            formSubasta.Show();
        }

        private void Menu_Load(object sender, EventArgs e)
        {

            FormLogin formLogin = new FormLogin();
            formLogin.MdiParent = this;
            formLogin.FormBorderStyle = FormBorderStyle.None;
            formLogin.Dock = DockStyle.Fill;

            formLogin.Show();
            ValidarPermisos();

        }
        public void ValidarPermisos()
        {
            Usuario usuario = SessionManager.GetInstance.usuario;

            if (usuario == null)
            {
                aBMArticulosToolStripMenuItem.Visible = false;
                return;
            }

            aBMArticulosToolStripMenuItem.Visible =
                usuario.Permisos.Contains(Permiso.ABM_ARTICULOS);
        }
        private void cerrarSesionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormLogin formLogin = new FormLogin();
            formLogin.MdiParent = this;
            formLogin.FormBorderStyle = FormBorderStyle.None;
            formLogin.Dock = DockStyle.Fill;

            formLogin.Show();
        }
    }
}
