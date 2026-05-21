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
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        private void ClaveModificada(object o, EventArgs e)
        {
            this.Visible = true;
            this.Close();
        }

        private void login_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtUsuario.Text == string.Empty || txtPassword.Text == string.Empty) { MessageBox.Show("Debe ingresar el Usuario y la Contraseña para continuar."); }
                else
                {
                    int resultado = LoginService.Login(txtUsuario.Text, txtPassword.Text);
                    switch (resultado)
                    {
                        case 1:
                            this.Close();
                            break;
                        case 2:
                            MessageBox.Show("Usuario inexistente");
                            break;
                        case 3:
                            MessageBox.Show("Password incorrecto");
                            break;
                        case 4:
                            MessageBox.Show("Usuario bloqueado");
                            break;
                        case 5:
                            MessageBox.Show("Usuario eliminado");
                            break;
                        case 6:
                            MessageBox.Show("Cambio de password requerido");
                            FormCambioPassword f = new FormCambioPassword();
                            f.FormClosed += ClaveModificada;
                            this.Visible = false;
                            f.Show();
                            break;
                        default:
                            MessageBox.Show("Error");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                try
                {
                    BE_Bitacora bitacora = new BE_Bitacora(SessionManager.GetInstance.usuario.Username, DateTime.UtcNow, "CRITICAL", ex.Message);
                    BitacoraService.Crear(bitacora);
                    MessageBox.Show("Error crítico, contacte al administrador.");
                }
                catch { 
                    throw new Exception("HAY QUE HACER EL LOG DE BITACORA LOCAL EN TXT"); 
                }
            }
            this.Close();
        }
    }
}
