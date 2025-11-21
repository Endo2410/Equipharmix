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
                    // 🛑 Validar si el usuario está inactivo
                    if (!usuario.Estado)
                    {
                        lbl_error.Text = "Su usuario está inactivo." + Environment.NewLine +
                        "Contacte al administrador.";
                        lbl_error.Visible = true;
                        txtusuario.Clear();
                        txtpassword.Clear();
                        txtusuario.Focus();
                        return; // detener el flujo de login
                    }

                    string claveAlmacenada = usuario.Clave;

                    // 🧠 Verifica si la contraseña almacenada tiene salt (nuevo formato)
                    bool accesoPermitido = false;
                    if (claveAlmacenada.Contains(":"))
                    {
                        accesoPermitido = Encriptacion.VerificarContraseña(txtpassword.Text, claveAlmacenada);
                    }
                    else
                    {
                        // 🕰 Compatibilidad con contraseñas antiguas (sin salt)
                        string hashAntiguo = Encriptacion.EncriptarContraseñaAntigua(txtpassword.Text);
                        accesoPermitido = (txtpassword.Text == claveAlmacenada || hashAntiguo == claveAlmacenada);

                        // ✅ Si entra con formato viejo, lo actualizamos al nuevo formato (salt)
                        if (accesoPermitido)
                        {
                            string nuevaClave = Encriptacion.EncriptarContraseña(txtpassword.Text);
                            usuario.Clave = nuevaClave;
                            new CN_Usuario().ActualizarClave(usuario.IdUsuario, nuevaClave);
                        }
                    }

                    if (accesoPermitido)
                    {
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
            frmbaja frmrecuperar = new frmbaja();
            frmrecuperar.Show();
            this.Hide();
        }
    }
}
