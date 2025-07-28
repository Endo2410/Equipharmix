using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.Utilidades;

namespace CapaPresentacion
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            lbl_error.Visible = false;
        }

        private void CloseForm(object sender, FormClosedEventArgs e)
        {
            this.Show();
            lbl_error.Visible = false;
            txtusuario.Focus();
        }

        private void btnentrar_Click(object sender, EventArgs e)
        {
            if (txtusuario.Text != "" && txtpassword.Text != "")
            {
                List<Usuario> usuarios = new CN_Usuario().Listar();
                Usuario usuario = usuarios.FirstOrDefault(u => u.NombreUsuario == txtusuario.Text);

                if (usuario != null)
                {
                    // Desencripta la contraseña almacenada en la base de datos
                    string claveAlmacenadaDesencriptada = usuario.Clave;

                    // Encripta la contraseña ingresada por el usuario para compararla con la contraseña almacenada
                    string claveIngresadaEncriptada = Encriptacion.EncriptarContraseña(txtpassword.Text);

                    // Compara las contraseñas encriptadas
                    if (claveIngresadaEncriptada == claveAlmacenadaDesencriptada)
                    {
                        // Autenticación exitosa
                        this.Hide();
                        Bienvenido bienvenido = new Bienvenido(usuario);
                        bienvenido.ShowDialog();
                        Inicio form = new Inicio(usuario);
                        form.Show();
                        form.FormClosed -= CloseForm;
                    }
                    else
                    {
                        lbl_error.Text = "Contraseña incorrecta";
                        lbl_error.Visible = true;
                        txtpassword.Clear();
                        txtpassword.Focus();
                        txtpassword.SelectAll();
                    }
                }
                else
                {
                    lbl_error.Text = "Credenciales incorrectas";
                    lbl_error.Visible = true;
                    txtusuario.Clear();
                    txtpassword.Clear();
                    txtusuario.Focus();
                    txtusuario.SelectAll();
                }
            }
            else if (txtusuario.Text == "")
            {
                lbl_error.Text = "Usuario incorrecto!";
                lbl_error.Visible = true;
                txtusuario.Focus();
                txtusuario.SelectAll();
            }
            else
            {
                lbl_error.Text = "Contraseña incorrecta";
                lbl_error.Visible = true;
                txtpassword.Focus();
                txtpassword.SelectAll();
            }
        }

        private void pbclose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void linrecuperacion_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmrecuperar frmrecuperar = new frmrecuperar();
            frmrecuperar.Show();
            this.Hide();
        }
    }
}
