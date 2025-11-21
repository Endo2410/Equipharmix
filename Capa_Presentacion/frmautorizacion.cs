using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.Utilidades;
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
    public partial class frmautorizacion : Form
    {

        private static Usuario usuarioActual;
        private CN_Acta objCN_Acta = new CN_Acta();


        public frmautorizacion(Usuario objusuario)
        {
            usuarioActual = objusuario;
            InitializeComponent();
        }

        private void dgvdata_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && dgvdata.Columns[e.ColumnIndex].Name == "btnseleccionar")
                {
                    DataGridViewRow fila = dgvdata.Rows[e.RowIndex];

                    txtindice.Text = e.RowIndex.ToString();
                    txtdocumento.Text = fila.Cells["NumeroDocumento"].Value?.ToString() ?? "";
                    txtfecha.Text = fila.Cells["FechaRegistro"].Value?.ToString() ?? "";
                    txtfarmacia.Text = fila.Cells["NombreFarmacia"].Value?.ToString() ?? "";
                    txtcodigoequipo.Text = fila.Cells["CodigoEquipo"].Value?.ToString() ?? "";
                    txtequipo.Text = fila.Cells["NombreEquipo"].Value?.ToString() ?? "";
                    txtmarca.Text = fila.Cells["Marca"].Value?.ToString() ?? "";
                    txtestado.Text = fila.Cells["Estado"].Value?.ToString() ?? "";
                    txtcantidad.Text = fila.Cells["Cantidad"].Value?.ToString() ?? "";
                    txtserial.Text = fila.Cells["NumeroSerial"].Value?.ToString() ?? "";
                    txtcaja.Text = fila.Cells["Caja"].Value?.ToString() ?? "";
                    txtsolicitante.Text = fila.Cells["UsuarioSolicitante"].Value?.ToString() ?? "";
                    txtmotivo.Text = fila.Cells["MotivoBaja"].Value?.ToString() ?? "";
                    txtestadobaja.Text = fila.Cells["EstadoBaja"].Value?.ToString() ?? "";

                    dgvdata.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al seleccionar el equipo:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmautorizacion_Load(object sender, EventArgs e)
        {
            // Obtiene los permisos del usuario logueado
            List<Permiso> listaPermisos = new CN_Permiso().Listar(Inicio.usuarioActual.IdUsuario);

            // Controla visibilidad de los botones según permisos
            btnguardar.Visible = UtilPermisos.TienePermisoAccion(listaPermisos, "submenuautorizacionbaja", "btnguardar");
            btnlimpiar.Visible = UtilPermisos.TienePermisoAccion(listaPermisos, "submenuautorizacionbaja", "btnlimpiar");
            btneliminar.Visible = UtilPermisos.TienePermisoAccion(listaPermisos, "submenuautorizacionbaja", "btneliminar");


            try
            {
                if (dgvdata.Columns.Count == 0)
                {
                    // Columna botón seleccionar
                    DataGridViewImageColumn btnSeleccionar = new DataGridViewImageColumn();
                    btnSeleccionar.Name = "btnseleccionar";
                    btnSeleccionar.HeaderText = "";
                    btnSeleccionar.Image = Properties.Resources.check20;
                    btnSeleccionar.ImageLayout = DataGridViewImageCellLayout.Zoom;
                    btnSeleccionar.Width = 30;
                    btnSeleccionar.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dgvdata.Columns.Add(btnSeleccionar);

                    // Otras columnas de datos
                    dgvdata.Columns.Add("NumeroDocumento", "Documento");
                    dgvdata.Columns.Add("FechaRegistro", "Fecha");
                    dgvdata.Columns.Add("NombreFarmacia", "Farmacia");
                    dgvdata.Columns.Add("CodigoEquipo", "Código Equipo");
                    dgvdata.Columns.Add("NombreEquipo", "Nombre Equipo");
                    dgvdata.Columns.Add("Marca", "Marca");
                    dgvdata.Columns.Add("Estado", "Estado");
                    dgvdata.Columns.Add("Cantidad", "Cantidad");
                    dgvdata.Columns.Add("NumeroSerial", "Serial");
                    dgvdata.Columns.Add("Caja", "Caja");
                    dgvdata.Columns.Add("MotivoBaja", "Motivo Baja");
                    dgvdata.Columns.Add("EstadoBaja", "Estado Baja");
                    dgvdata.Columns.Add("UsuarioSolicitante", "Usuario Solicitante");

                    // Columna margen (espacio en blanco)
                    DataGridViewTextBoxColumn columnaMargen = new DataGridViewTextBoxColumn();
                    columnaMargen.Name = "colMargen";
                    columnaMargen.HeaderText = "";
                    columnaMargen.ReadOnly = true;
                    columnaMargen.Width = 30;
                    columnaMargen.SortMode = DataGridViewColumnSortMode.NotSortable;

                    columnaMargen.DefaultCellStyle.BackColor = dgvdata.BackgroundColor;
                    columnaMargen.DefaultCellStyle.SelectionBackColor = dgvdata.BackgroundColor;
                    columnaMargen.DefaultCellStyle.ForeColor = dgvdata.BackgroundColor;
                    columnaMargen.DefaultCellStyle.SelectionForeColor = dgvdata.BackgroundColor;
                    columnaMargen.DividerWidth = 0;

                    dgvdata.Columns.Add(columnaMargen);

                    // Ajustar AutoSizeColumnsMode y ancho de columnas
                    foreach (DataGridViewColumn col in dgvdata.Columns)
                    {
                        if (col.Name == "NombreFarmacia")
                        {
                            col.Width = 150;
                            col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                        }
                        else if (col.Name != "colMargen" && col.Name != "btnseleccionar")
                        {
                            col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        }
                        else
                        {
                            col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                        }
                    }
                }

                dgvdata.Rows.Clear();

                var lista = objCN_Acta.ObtenerEquiposEnEspera();

                foreach (var item in lista)
                {
                    dgvdata.Rows.Add(
                        Properties.Resources.check20,
                        item.NumeroDocumento,
                        item.FechaRegistro.ToShortDateString(),
                        item.oFarmacia.Nombre,
                        item.oEquipo.Codigo,
                        item.oEquipo.Nombre,
                        item.oEquipo.oMarca.Descripcion,
                        item.oEquipo.oEstado.Descripcion,
                        item.Cantidad,
                        item.NumeroSerial,
                        item.Caja,
                        item.MotivoBaja,
                        item.EstadoBaja,
                        item.oUsuarioSolicitante.NombreCompleto,
                        ""
                    );

                    var fila = dgvdata.Rows[dgvdata.Rows.Count - 1];
                    if (item.EstadoBaja?.Equals("En espera", StringComparison.OrdinalIgnoreCase) == true)
                        fila.DefaultCellStyle.BackColor = Color.White;
                }

                // Llenar ComboBox búsqueda excluyendo botón y margen
                cbobusqueda.Items.Clear();
                foreach (DataGridViewColumn columna in dgvdata.Columns)
                {
                    if (columna.Visible && columna.Name != "btnseleccionar" && columna.Name != "colMargen")
                    {
                        cbobusqueda.Items.Add(new OpcionCombo() { Valor = columna.Name, Texto = columna.HeaderText });
                    }
                }

                cbobusqueda.DisplayMember = "Texto";
                cbobusqueda.ValueMember = "Valor";
                if (cbobusqueda.Items.Count > 0)
                    cbobusqueda.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar el formulario:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnguardar_Click(object sender, EventArgs e)
        {
            try
            {
                string numeroDocumento = txtdocumento.Text.Trim();
                string codigoEquipo = txtcodigoequipo.Text.Trim();
                string numeroSerial = txtserial.Text.Trim();

                if (string.IsNullOrEmpty(numeroDocumento) || string.IsNullOrEmpty(codigoEquipo) || string.IsNullOrEmpty(numeroSerial))
                {
                    MessageBox.Show("Debe seleccionar un equipo válido antes de autorizar.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult result = MessageBox.Show("¿Está seguro que desea autorizar este equipo?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    int idUsuarioActual = usuarioActual.IdUsuario;

                    bool exito = objCN_Acta.AutorizarBaja(numeroDocumento, codigoEquipo, numeroSerial, idUsuarioActual);

                    if (exito)
                    {
                        MessageBox.Show("Equipo autorizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        frmautorizacion_Load(null, null);
                        LimpiarCampos();
                    }
                    else
                    {
                        MessageBox.Show("Error al autorizar equipo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error inesperado:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarCampos()
        {
            txtindice.Text = "";
            txtdocumento.Text = "";
            txtfecha.Text = "";
            txtfarmacia.Text = "";
            txtcodigoequipo.Text = "";
            txtequipo.Text = "";
            txtmarca.Text = "";
            txtestado.Text = "";
            txtcantidad.Text = "";
            txtserial.Text = "";
            txtcaja.Text = "";
            txtsolicitante.Text = "";
            txtmotivo.Text = "";
            txtestadobaja.Text = "";
        }

        private void btneliminar_Click(object sender, EventArgs e)
        {

            try
            {
                string numeroDocumento = txtdocumento.Text.Trim();
                string codigoEquipo = txtcodigoequipo.Text.Trim();
                string numeroSerial = txtserial.Text.Trim();

                if (string.IsNullOrEmpty(numeroDocumento) || string.IsNullOrEmpty(codigoEquipo) || string.IsNullOrEmpty(numeroSerial))
                {
                    MessageBox.Show("Debe seleccionar un equipo válido antes de eliminar el motivo.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult result = MessageBox.Show("¿Está seguro que deseas rechazar la baja?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    bool exito = objCN_Acta.LimpiarMotivoYEstado(numeroDocumento, codigoEquipo, numeroSerial);

                    if (exito)
                    {
                        MessageBox.Show("Baja rechazada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        frmautorizacion_Load(null, null);
                        LimpiarCampos();
                    }
                    else
                    {
                        MessageBox.Show("Error al rechazar baja.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error inesperado:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnlimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
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
                        if (row.Cells[columnaFiltro].Value != null &&
                            row.Cells[columnaFiltro].Value.ToString().Trim().ToUpper().Contains(txtbusqueda.Text.Trim().ToUpper()))
                        {
                            row.Visible = true;
                        }
                        else
                        {
                            row.Visible = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en la búsqueda:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
    }
}
