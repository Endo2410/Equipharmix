using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using CapaPresentacion.Utilidades;

using CapaEntidad;
using CapaNegocio;
using System.Security.Cryptography;

namespace CapaPresentacion
{
    public partial class frmUsuarios : Form
    {

        private bool contraseñaEditada = false;
        public frmUsuarios()
        {
            InitializeComponent();           
        }


        // Función para validar la contraseña
        private void Txtclave_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string contraseña = txtclave.Text;

            if (!Validaciones(contraseña))
            {
                MessageBox.Show("La contraseña debe tener al menos 8 caracteres, incluir minúsculas, mayúsculas, números y símbolos.", "Contraseña no válida", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtclave.SelectAll();
                e.Cancel = true; // Evita que el foco salga del TextBox si la contraseña no es válida
            }
        }

        // Función para validar si una contraseña es robusta
        private bool Validaciones(string contraseña)
        {
            // Requisitos mínimos para una contraseña robusta
            int longitudMinima = 8;
            bool contieneMayusculas = contraseña.Any(char.IsUpper);
            bool contieneMinusculas = contraseña.Any(char.IsLower);
            bool contieneNumeros = contraseña.Any(char.IsDigit);
            bool contieneSimbolos = contraseña.Any(c => !char.IsLetterOrDigit(c));

            // Verifica si cumple con los requisitos mínimos
            if (contraseña.Length < longitudMinima ||
                !contieneMayusculas ||
                !contieneMinusculas ||
                !contieneNumeros ||
                !contieneSimbolos)
            {
                return false;
            }

            return true;
        }


        //Detecta si modifico el campo
        private void txtclave_TextChanged(object sender, EventArgs e)
        {
            // Activa la bandera solo si el texto realmente fue modificado manualmente
            if (txtclave.Focused)
                contraseñaEditada = true;
        }


        private void frmUsuarios_Load(object sender, EventArgs e)
        {
            // Obtiene los permisos del usuario logueado
            List<Permiso> listaPermisos = new CN_Permiso().Listar(Inicio.usuarioActual.IdUsuario);

            // Controla visibilidad de los botones según permisos
            btnguardar.Visible = UtilPermisos.TienePermisoAccion(listaPermisos, "submenuusuarios", "btnguardar");
            btnlimpiar.Visible = UtilPermisos.TienePermisoAccion(listaPermisos, "submenuusuarios", "btnlimpiar");

            txtdocumento.MaxLength = 40;
            txtnombrecompleto.MaxLength = 40;
            txtnombreusuario.MaxLength = 30;
            txtcorreo.MaxLength = 40;
            txtclave.MaxLength = 30;
            txtconfirmarclave.MaxLength = 30;
            txtclave.TextChanged += txtclave_TextChanged;

            cboestado.Items.Add(new OpcionCombo() { Valor = 1 , Texto = "Activo" });
            cboestado.Items.Add(new OpcionCombo() { Valor = 0 , Texto = "No Activo" });
            cboestado.DisplayMember = "Texto";
            cboestado.ValueMember = "Valor";
            cboestado.SelectedIndex = 0;


            List<Rol> listaRol = new CN_Rol().Listar();

            foreach (Rol item in listaRol) {
                cborol.Items.Add(new OpcionCombo() { Valor = item.IdRol, Texto = item.Descripcion });
            }
            cborol.DisplayMember = "Texto";
            cborol.ValueMember = "Valor";
            cborol.SelectedIndex = 0;


            foreach (DataGridViewColumn columna in dgvdata.Columns) {

                if (columna.Visible == true && columna.Name != "btnseleccionar") {
                    cbobusqueda.Items.Add(new OpcionCombo() { Valor = columna.Name, Texto = columna.HeaderText});
                }
            }
            cbobusqueda.DisplayMember = "Texto";
            cbobusqueda.ValueMember = "Valor";
            cbobusqueda.SelectedIndex = 0;



            //MOSTRAR TODOS LOS USUARIOS
            List<Usuario> listaUsuario = new CN_Usuario().Listar();

            foreach (Usuario item in listaUsuario)
            {

                dgvdata.Rows.Add(new object[] {"",item.IdUsuario,item.Documento,item.NombreCompleto,item.NombreUsuario,item.Correo,item.Clave,
                    item.oRol.IdRol,
                    item.oRol.Descripcion,
                    item.Estado == true ? 1 : 0 ,
                    item.Estado == true ? "Activo" : "No Activo"
                });
            }

        }

        private void btnguardar_Click(object sender, EventArgs e)
        {
            try
            {
                string mensaje = string.Empty;

                int idEditado = Convert.ToInt32(txtid.Text);
                int idLogueado = Inicio.usuarioActual.IdUsuario;
                int estadoSeleccionado = Convert.ToInt32(((OpcionCombo)cboestado.SelectedItem).Valor);

                // No permitir desactivarse a sí mismo
                if (idEditado == idLogueado && estadoSeleccionado == 0)
                {
                    MessageBox.Show("No puedes desactivar tu propia cuenta mientras estás usando el sistema.",
                        "Acción no permitida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }


                Usuario objusuario = new Usuario()
                {
                    IdUsuario = Convert.ToInt32(txtid.Text),
                    Documento = txtdocumento.Text,
                    NombreCompleto = txtnombrecompleto.Text,
                    NombreUsuario = txtnombreusuario.Text,
                    Correo = txtcorreo.Text,
                    oRol = new Rol() { IdRol = Convert.ToInt32(((OpcionCombo)cborol.SelectedItem).Valor) },
                    Estado = Convert.ToInt32(((OpcionCombo)cboestado.SelectedItem).Valor) == 1 ? true : false
                };

                // Validar y asignar contraseña
                if (contraseñaEditada)
                {
                    string contraseña = txtclave.Text;
                    string confirmarContraseña = txtconfirmarclave.Text;

                    if (!Validaciones(contraseña))
                    {
                        MessageBox.Show("La contraseña debe tener al menos 8 caracteres, incluir minúsculas, mayúsculas, números y símbolos.",
                            "Contraseña no válida", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtclave.Select();
                        return;
                    }

                    if (contraseña != confirmarContraseña)
                    {
                        MessageBox.Show("Las contraseñas no coinciden. Por favor, asegúrate de ingresar la misma contraseña en ambos campos.",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // ✅ Encriptar con salt
                    objusuario.Clave = Encriptacion.EncriptarContraseña(contraseña);
                }
                else
                {
                    objusuario.Clave = txtclave.Text;
                }


                // Registrar o editar
                if (objusuario.IdUsuario == 0)
                {
                    int idusuariogenerado = new CN_Usuario().Registrar(objusuario, out mensaje);

                    if (idusuariogenerado != 0)
                    {
                        dgvdata.Rows.Add(new object[] {
                            "", idusuariogenerado, txtdocumento.Text, txtnombrecompleto.Text,txtnombreusuario.Text, txtcorreo.Text, objusuario.Clave,
                            ((OpcionCombo)cborol.SelectedItem).Valor.ToString(),
                            ((OpcionCombo)cborol.SelectedItem).Texto.ToString(),
                            ((OpcionCombo)cboestado.SelectedItem).Valor.ToString(),
                            ((OpcionCombo)cboestado.SelectedItem).Texto.ToString()
                        });

                        Limpiar();
                    }
                    else
                    {
                        MessageBox.Show(mensaje);
                    }
                }

                else
                {
                    bool resultado = new CN_Usuario().Editar(objusuario, out mensaje);

                    if (resultado)
                    {
                        DataGridViewRow row = dgvdata.Rows[Convert.ToInt32(txtindice.Text)];
                        row.Cells["Id"].Value = txtid.Text;
                        row.Cells["Documento"].Value = txtdocumento.Text;
                        row.Cells["NombreCompleto"].Value = txtnombrecompleto.Text;
                        row.Cells["NombreUsuario"].Value = txtnombreusuario.Text;
                        row.Cells["Correo"].Value = txtcorreo.Text;
                        row.Cells["Clave"].Value = objusuario.Clave;
                        row.Cells["IdRol"].Value = ((OpcionCombo)cborol.SelectedItem).Valor.ToString();
                        row.Cells["Rol"].Value = ((OpcionCombo)cborol.SelectedItem).Texto.ToString();
                        row.Cells["EstadoValor"].Value = ((OpcionCombo)cboestado.SelectedItem).Valor.ToString();
                        row.Cells["Estado"].Value = ((OpcionCombo)cboestado.SelectedItem).Texto.ToString();

                        Limpiar();
                    }
                    else
                    {
                        MessageBox.Show(mensaje);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ha ocurrido un error inesperado:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void Limpiar() {

            txtindice.Text = "-1";
            txtid.Text = "0";
            txtdocumento.Text = "";
            txtnombrecompleto.Text = "";
            txtnombreusuario.Text = "";
            txtcorreo.Text = "";
            txtclave.Text = "";
            txtconfirmarclave.Text = "";
            cborol.SelectedIndex = 0;
            cboestado.SelectedIndex = 0;

            txtdocumento.Select();
        }

        private void dgvdata_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (e.ColumnIndex == 0) {

                e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                var w = Properties.Resources.check20.Width;
                var h = Properties.Resources.check20.Height;
                var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
                var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;

                e.Graphics.DrawImage(Properties.Resources.check20, new Rectangle(x,y,w,h));
                e.Handled = true;
            }

        }

        //Maneja la selección de una celda en el DataGridView 
        private void dgvdata_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            contraseñaEditada = false;

            if (dgvdata.Columns[e.ColumnIndex].Name == "btnseleccionar")
            {
                int indice = e.RowIndex;

                if (indice >= 0)
                {
                    txtindice.Text = indice.ToString();
                    txtid.Text = dgvdata.Rows[indice].Cells["Id"].Value.ToString();
                    txtdocumento.Text = dgvdata.Rows[indice].Cells["Documento"].Value.ToString();
                    txtnombrecompleto.Text = dgvdata.Rows[indice].Cells["NombreCompleto"].Value.ToString();
                    txtnombreusuario.Text = dgvdata.Rows[indice].Cells["NombreUsuario"].Value.ToString();
                    txtcorreo.Text = dgvdata.Rows[indice].Cells["Correo"].Value.ToString();
                    txtclave.Text = dgvdata.Rows[indice].Cells["Clave"].Value.ToString();
                    txtconfirmarclave.Text = dgvdata.Rows[indice].Cells["Clave"].Value.ToString();

                    foreach (OpcionCombo oc in cborol.Items)
                    {
                        if (Convert.ToInt32(oc.Valor) == Convert.ToInt32(dgvdata.Rows[indice].Cells["IdRol"].Value))
                        {
                            cborol.SelectedIndex = cborol.Items.IndexOf(oc);
                            break;
                        }
                    }

                    foreach (OpcionCombo oc in cboestado.Items)
                    {
                        if (Convert.ToInt32(oc.Valor) == Convert.ToInt32(dgvdata.Rows[indice].Cells["EstadoValor"].Value))
                        {
                            cboestado.SelectedIndex = cboestado.Items.IndexOf(oc);
                            break;
                        }
                    }

                    // ✅ AHORA que ya txtid está asignado, recién validamos
                    if (Convert.ToInt32(txtid.Text) == Inicio.usuarioActual.IdUsuario)
                    {
                        cboestado.Enabled = false;
                    }
                    else
                    {
                        cboestado.Enabled = true;
                    }
                }
            }
        }

        private void btnbuscar_Click(object sender, EventArgs e)
        {
            string columnaFiltro = ((OpcionCombo)cbobusqueda.SelectedItem).Valor.ToString();

            if (dgvdata.Rows.Count > 0) {
                foreach (DataGridViewRow row in dgvdata.Rows) {

                    if (row.Cells[columnaFiltro].Value.ToString().Trim().ToUpper().Contains(txtbusqueda.Text.Trim().ToUpper()))
                        row.Visible = true;
                    else
                        row.Visible = false;
                }
            }
        }

        private void btnlimpiarbuscador_Click(object sender, EventArgs e)
        {
            txtbusqueda.Text = "";
            foreach (DataGridViewRow row in dgvdata.Rows)
            {
                row.Visible = true;
            }
        }

        private void btnlimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

    }
}
