using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.Modales;
using FontAwesome.Sharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class Inicio : Form
    {

        public static Usuario usuarioActual { get; private set; }
        private static IconMenuItem MenuActivo = null;
        private static Form FormularioActivo = null;

        public Inicio(Usuario objusuario)
        {
            usuarioActual = objusuario;
            InitializeComponent();
        }


        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

      
        private void MDIParent1_Load(object sender, EventArgs e)
        {
            this.UseWaitCursor = false;
            this.Cursor = Cursors.Hand;
            pb_salir.Cursor = Cursors.Hand;


            List<Permiso> ListaPermisos = new CN_Permiso().Listar(usuarioActual.IdUsuario);

            foreach (IconMenuItem iconmenu in menu.Items)
            {

                bool encontrado = ListaPermisos.Any(m => m.NombreMenu == iconmenu.Name);

                if (encontrado == false)
                {
                    iconmenu.Visible = false;
                }

            }
            lblusuario.Text = usuarioActual.NombreCompleto;
        }

        private void AbrirFormulario(IconMenuItem menu, Form formulario)
        {

            if (MenuActivo != null)
            {
                MenuActivo.BackColor = Color.White;
            }
            menu.BackColor = Color.PaleGreen;
            MenuActivo = menu;

            if (FormularioActivo != null)
            {
                FormularioActivo.Close();
            }

            FormularioActivo = formulario;
            formulario.TopLevel = false;
            formulario.FormBorderStyle = FormBorderStyle.None;
            formulario.Dock = DockStyle.Fill;
            formulario.BackColor = Color.MintCream;

            contenedor.Controls.Add(formulario);
            formulario.Show();
        }

        ///
        private void menuusuarios_Click(object sender, EventArgs e)
        {
            AbrirFormulario((IconMenuItem)sender, new frmUsuarios());
        }

        private void submenumarca_Click_1(object sender, EventArgs e)
        {
            AbrirFormulario(menumantenedor, new frmmarca());
        }

        private void submenuequipo_Click_1(object sender, EventArgs e)
        {
            AbrirFormulario(menumantenedor, new frmEquipo());
        }

        private void pb_salir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void submenuregistraracta_Click_1(object sender, EventArgs e)
        {
            AbrirFormulario(menuacta, new frmacta(usuarioActual));
        }

        private void menuacercade_Click(object sender, EventArgs e)
        {
            mdAcercade md = new mdAcercade();
            md.ShowDialog();
        }

        private void submenuverdetalleacta_Click_1(object sender, EventArgs e)
        {
            AbrirFormulario(menuacta, new frmdetalleacta());
        }

        private void submenuregistrarcompra_Click_1(object sender, EventArgs e)
        {
            AbrirFormulario(menuregistrar, new frmregistrar(usuarioActual));
        }

        private void submenuverdatallecompra_Click_1(object sender, EventArgs e)
        {
            AbrirFormulario(menuregistrar, new frmdetalleregistrar());
        }

        private void menufarmacia_Click(object sender, EventArgs e)
        {
            AbrirFormulario((IconMenuItem)sender, new frmfarmacia());
        }

        private void autorizacionActaToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            AbrirFormulario(menuautorizacion, new frmactaautorizacion(usuarioActual));
        }

        private void autorizacionBajaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menuautorizacion, new frmautorizacion(usuarioActual));
        }

        private void equiposDeBajaToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            AbrirFormulario(menuautorizacion, new frmrecuperar());
        }

        private void menuasiganados_Click_1(object sender, EventArgs e)
        {
            AbrirFormulario((IconMenuItem)sender, new frmEquiposAsignados(usuarioActual));
        }

        private void submenunegocio_Click_1(object sender, EventArgs e)
        {
            AbrirFormulario(menumantenedor, new frmNegocio());
        }

        private void pb_cerrarsecion_Click_1(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Desea Cerrar sesión?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Cierra el formulario actual
                this.Close();

                // Abre una nueva instancia del formulario de inicio de sesión
                Login login = new Login();
                login.Show();
            }
        }

        private void pb_minimizar_Click_1(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void pb_maximizar_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal) this.WindowState = FormWindowState.Maximized;
            else this.WindowState = FormWindowState.Normal;
        }

        private void submenureporteacta_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menureportes, new frmreporteacta());
        }

        private void submenureporteregistrar_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menureportes, new frmreporteregistrar());
        }
    }
}
