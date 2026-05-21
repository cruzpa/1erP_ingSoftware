using BE;
using BLL;
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
    public partial class FormRegistro : Form
    {
        public FormRegistro()
        {
            InitializeComponent();
            txtPassword.PasswordChar = '*';
        }

        private void btnCrearUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                Cliente cliente = new Cliente();

                cliente.Username = txtUsername.Text;
                cliente.Password = SeguridadService.Encriptar(txtPassword.Text);
                cliente.Nombre = txtNombre.Text;
                cliente.Apellido = txtApellido.Text;

                cliente.Email = txtEmail.Text;
                cliente.Telefono = txtTelefono.Text;
                cliente.Direccion = txtDireccion.Text;

                UsuarioService.Crear(cliente);

                MessageBox.Show("Usuario creado correctamente");

                LimpiarFormulario();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void LimpiarFormulario()
        {
            txtUsername.Clear();
            txtPassword.Clear();

            txtNombre.Clear();
            txtApellido.Clear();

            txtEmail.Clear();
            txtTelefono.Clear();
            txtDireccion.Clear();
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            FormLogin formLogin = new FormLogin();

            formLogin.MdiParent = this.MdiParent;

            formLogin.FormBorderStyle = FormBorderStyle.None;

            formLogin.Dock = DockStyle.Fill;

            formLogin.Show();

            this.Close();
        }
    }
}
