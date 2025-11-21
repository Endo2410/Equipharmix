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
using CapaPresentacion.Modales;
using CapaPresentacion.Utilidades;

namespace CapaPresentacion
{
    public partial class frmregistrar : Form
    {

        private Usuario _Usuario;

        public frmregistrar(Usuario oUsuario = null)
        {
            _Usuario = oUsuario;
            InitializeComponent();
        }

        private void frmregistrar_Load(object sender, EventArgs e)
        {
            // Obtiene los permisos del usuario logueado
            List<Permiso> listaPermisos = new CN_Permiso().Listar(Inicio.usuarioActual.IdUsuario);

            // Controla visibilidad de los botones según permisos
            btnagregarequipo.Visible = UtilPermisos.TienePermisoAccion(listaPermisos, "submenuregistrarcompra", "btnagregarequipo");
            btnregistrar.Visible = UtilPermisos.TienePermisoAccion(listaPermisos, "submenuregistrarcompra", "btnregistrar");



            // Agrega columna imagen si no existe
            if (!dgvdata.Columns.Contains("btneliminar"))
            {
                DataGridViewImageColumn btnEliminar = new DataGridViewImageColumn();
                btnEliminar.Name = "btneliminar";
                btnEliminar.HeaderText = "";
                btnEliminar.Image = Properties.Resources.delete25; // asegúrate que esta imagen exista
                btnEliminar.Width = 30;
                dgvdata.Columns.Add(btnEliminar);
            }

            cbotipodocumento.Items.Add(new OpcionCombo() { Valor = "Boleta", Texto = "Boleta" });
            cbotipodocumento.Items.Add(new OpcionCombo() { Valor = "Word", Texto = "Word" });
            cbotipodocumento.DisplayMember = "Texto";
            cbotipodocumento.ValueMember = "Valor";
            cbotipodocumento.SelectedIndex = 0;

            txtfecha.Text = DateTime.Now.ToString("dd/MM/yyyy");

            txtidEquipo.Text = "0";
        }


        private void txtcodEquipo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                try
                {
                    string codigo = txtcodEquipo.Text.Trim();

                    Equipo oEquipo = new CN_Equipo().ObtenerPorCodigo(codigo);

                    if (oEquipo != null)
                    {
                        txtcodEquipo.BackColor = Color.Honeydew;
                        txtidEquipo.Text = oEquipo.IdEquipo.ToString();
                        txtEquipo.Text = oEquipo.Nombre;
                        txtcantidad.Value = 1;

                        // Solo enfocar el botón o el campo cantidad, no agregar nada al dgvdata
                        txtcantidad.Select();
                    }
                    else
                    {
                        txtcodEquipo.BackColor = Color.MistyRose;
                        txtidEquipo.Text = "0";
                        txtEquipo.Text = "";
                        txtcantidad.Value = 1;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al obtener el equipo: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        

        private void btnagregarequipo_Click(object sender, EventArgs e)
        {
            try
            {
                bool Equipo_existe = false;

                if (int.Parse(txtidEquipo.Text) == 0)
                {
                    MessageBox.Show("Debe seleccionar un Equipo", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                foreach (DataGridViewRow fila in dgvdata.Rows)
                {
                    if (fila.Cells["IdEquipo"].Value.ToString() == txtidEquipo.Text)
                    {
                        Equipo_existe = true;
                        break;
                    }
                }

                if (!Equipo_existe)
                {
                    dgvdata.Rows.Add(new object[] {
                txtidEquipo.Text,
                txtEquipo.Text,
                txtcantidad.Value.ToString(),

            });

                    limpiarEquipo();
                    txtcodEquipo.Select();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar equipo: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void limpiarEquipo()
        {
            txtidEquipo.Text = "0";
            txtcodEquipo.Text = "";
            txtcodEquipo.BackColor = Color.White;
            txtEquipo.Text = "";
            txtcantidad.Value = 1;
            
        }

        private void limpiarTodo()
        {
            dgvdata.Rows.Clear(); // Esto limpia las filas del DataGridView

            // Limpiar campos individuales
            limpiarEquipo();
        }

        private void dgvdata_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (e.ColumnIndex == 3)
            {

                e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                var w = Properties.Resources.delete25.Width;
                var h = Properties.Resources.delete25.Height;
                var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
                var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;

                e.Graphics.DrawImage(Properties.Resources.delete25, new Rectangle(x, y, w, h));
                e.Handled = true;
            }
        }

        private void dgvdata_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (dgvdata.Columns[e.ColumnIndex].Name == "btneliminar")
            {
                int indice = e.RowIndex;

                if (indice >= 0)
                {
                    dgvdata.Rows.RemoveAt(indice);
                }
            }
        }

        private void btnregistrar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvdata.Rows.Count < 1)
                {
                    MessageBox.Show("Debe ingresar Equipos en la Registrar", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                DataTable detalle_Registrar = new DataTable();

                detalle_Registrar.Columns.Add("IdEquipo", typeof(int));
                detalle_Registrar.Columns.Add("Cantidad", typeof(int));

                foreach (DataGridViewRow row in dgvdata.Rows)
                {
                    if (row.IsNewRow) continue; // evita error por fila vacía

                    int idEquipo = Convert.ToInt32(row.Cells["IdEquipo"].Value);
                    int cantidad = Convert.ToInt32(Convert.ToDecimal(row.Cells["Cantidad"].Value)); // conversión segura

                    detalle_Registrar.Rows.Add(
                        new object[] {
                    idEquipo,
                    cantidad
                        });
                }

                int idcorrelativo = new CN_Registrar().ObtenerCorrelativo();
                string numerodocumento = string.Format("{0:00000}", idcorrelativo);

                if (_Usuario == null)
                {
                    MessageBox.Show("El usuario no ha sido cargado.");
                    return;
                }

                if (cbotipodocumento.SelectedItem == null)
                {
                    MessageBox.Show("Seleccione un tipo de documento.");
                    return;
                }

                Registrar oRegistrar = new Registrar()
                {
                    oUsuario = new Usuario() { IdUsuario = _Usuario.IdUsuario },
                    TipoDocumento = ((OpcionCombo)cbotipodocumento.SelectedItem).Texto,
                    NumeroDocumento = numerodocumento
                };

                string mensaje = string.Empty;
                bool respuesta = new CN_Registrar().Registrar(oRegistrar, detalle_Registrar, out mensaje);

                if (respuesta)
                {
                    var result = MessageBox.Show("Número de Registrar generada:\n" + numerodocumento + "\n\n¿Desea copiar al portapapeles?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                    if (result == DialogResult.Yes)
                        Clipboard.SetText(numerodocumento);

                    limpiarTodo();
                }
                else
                {
                    MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnbuscarequipo_Click(object sender, EventArgs e)
        {
            using (var modal = new mdEquipo())
            {
                var result = modal.ShowDialog();

                if (result == DialogResult.OK)
                {
                    txtidEquipo.Text = modal._Equipo.IdEquipo.ToString();
                    txtcodEquipo.Text = modal._Equipo.Codigo;
                    txtEquipo.Text = modal._Equipo.Nombre;
                }
                else
                {
                    txtcodEquipo.Select();
                }
            }
        }
    }
}

