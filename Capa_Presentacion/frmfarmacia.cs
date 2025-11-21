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
    public partial class frmfarmacia : Form
    {
        public frmfarmacia()
        {
            InitializeComponent();
        }

        private void frmfarmacia_Load(object sender, EventArgs e)
        {
            // Obtiene los permisos del usuario logueado
            List<Permiso> listaPermisos = new CN_Permiso().Listar(Inicio.usuarioActual.IdUsuario);

            // Controla visibilidad de los botones según permisos
            btnguardar.Visible = UtilPermisos.TienePermisoAccion(listaPermisos, "menufarmacia", "btnguardar");
            btnlimpiar.Visible = UtilPermisos.TienePermisoAccion(listaPermisos, "menufarmacia", "btnlimpiar");

            try
            {
                txtcodigo.MaxLength = 50;
                txtnombre.MaxLength = 50;
                txttelefono.MaxLength = 10;

                cboestado.Items.Add(new OpcionCombo() { Valor = 1, Texto = "Activo" });
                cboestado.Items.Add(new OpcionCombo() { Valor = 0, Texto = "No Activo" });
                cboestado.DisplayMember = "Texto";
                cboestado.ValueMember = "Valor";
                cboestado.SelectedIndex = 0;

                cbobusqueda.Items.Clear();
                foreach (DataGridViewColumn columna in dgvdata.Columns)
                {
                    if (columna.Visible == true && columna.Name != "btnseleccionar")
                    {
                        cbobusqueda.Items.Add(new OpcionCombo() { Valor = columna.Name, Texto = columna.HeaderText });
                    }
                }
                cbobusqueda.DisplayMember = "Texto";
                cbobusqueda.ValueMember = "Valor";
                cbobusqueda.SelectedIndex = 0;

                //MOSTRAR TODAS LAS FARMACIAS
                List<Farmacia> lista = new CN_Farmacia().Listar();

                foreach (Farmacia item in lista)
                {
                    dgvdata.Rows.Add(new object[] {
                        "",
                        item.IdFarmacia,
                        item.Codigo,
                        item.Nombre,
                        item.Correo,
                        item.Telefono,
                        item.Estado == true ? 1 : 0,
                        item.Estado == true ? "Activo" : "No Activo"
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void Limpiar()
        {
            try
            {
                txtindice.Text = "-1";
                txtid.Text = "0";
                txtcodigo.Text = "";
                txtnombre.Text = "";
                txtcorreo.Text = "";
                txttelefono.Text = "";
                cboestado.SelectedIndex = 0;
                txtcodigo.Select();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al limpiar campos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnguardar_Click(object sender, EventArgs e)
        {
            try
            {
                string mensaje = string.Empty;

                Farmacia obj = new Farmacia()
                {
                    IdFarmacia = Convert.ToInt32(txtid.Text),
                    Codigo = txtcodigo.Text,
                    Nombre = txtnombre.Text,
                    Correo = txtcorreo.Text,
                    Telefono = txttelefono.Text,
                    Estado = Convert.ToInt32(((OpcionCombo)cboestado.SelectedItem).Valor) == 1 ? true : false
                };

                if (obj.IdFarmacia == 0)
                {
                    int idgenerado = new CN_Farmacia().Registrar(obj, out mensaje);

                    if (idgenerado != 0)
                    {
                        dgvdata.Rows.Add(new object[] {
                            "",
                            idgenerado,
                            txtcodigo.Text,
                            txtnombre.Text,
                            txtcorreo.Text,
                            txttelefono.Text,
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
                    bool resultado = new CN_Farmacia().Editar(obj, out mensaje);

                    if (resultado)
                    {
                        DataGridViewRow row = dgvdata.Rows[Convert.ToInt32(txtindice.Text)];
                        row.Cells["Id"].Value = txtid.Text;
                        row.Cells["Codigo"].Value = txtcodigo.Text;
                        row.Cells["Nombre"].Value = txtnombre.Text;
                        row.Cells["Correo"].Value = txtcorreo.Text;
                        row.Cells["Telefono"].Value = txttelefono.Text;
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
                MessageBox.Show("Error al guardar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnbuscar_Click(object sender, EventArgs e)
        {
            try
            {
                string columnaFiltro = ((OpcionCombo)cbobusqueda.SelectedItem).Valor.ToString();

                if (dgvdata.Rows.Count > 0)
                {
                    foreach (DataGridViewRow row in dgvdata.Rows)
                    {
                        if (row.Cells[columnaFiltro].Value.ToString().Trim().ToUpper().Contains(txtbusqueda.Text.Trim().ToUpper()))
                            row.Visible = true;
                        else
                            row.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnlimpiarbuscador_Click(object sender, EventArgs e)
        {
            try
            {
                txtbusqueda.Text = "";
                foreach (DataGridViewRow row in dgvdata.Rows)
                {
                    row.Visible = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al limpiar búsqueda: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnlimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void dgvdata_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;

                if (e.ColumnIndex == 0)
                {
                    e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                    var w = Properties.Resources.check20.Width;
                    var h = Properties.Resources.check20.Height;
                    var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
                    var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;

                    e.Graphics.DrawImage(Properties.Resources.check20, new Rectangle(x, y, w, h));
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en la pintura de celda: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvdata_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvdata.Columns[e.ColumnIndex].Name == "btnseleccionar")
                {
                    int indice = e.RowIndex;

                    if (indice >= 0)
                    {
                        txtindice.Text = indice.ToString();
                        txtid.Text = dgvdata.Rows[indice].Cells["Id"].Value.ToString();
                        txtcodigo.Text = dgvdata.Rows[indice].Cells["Codigo"].Value.ToString();
                        txtnombre.Text = dgvdata.Rows[indice].Cells["Nombre"].Value.ToString();
                        txtcorreo.Text = dgvdata.Rows[indice].Cells["Correo"].Value.ToString();
                        txttelefono.Text = dgvdata.Rows[indice].Cells["Telefono"].Value.ToString();

                        foreach (OpcionCombo oc in cboestado.Items)
                        {
                            if (Convert.ToInt32(oc.Valor) == Convert.ToInt32(dgvdata.Rows[indice].Cells["EstadoValor"].Value))
                            {
                                int indice_combo = cboestado.Items.IndexOf(oc);
                                cboestado.SelectedIndex = indice_combo;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al seleccionar fila: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
