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
using ClosedXML.Excel;

namespace CapaPresentacion
{
    public partial class frmbaja : Form
    {

        private CN_Acta objCN_Acta = new CN_Acta();
        public frmbaja()
        {
            InitializeComponent();
        }

        private void frmrecuperar_Load(object sender, EventArgs e)
        {
            // Obtiene los permisos del usuario logueado
            List<Permiso> listaPermisos = new CN_Permiso().Listar(Inicio.usuarioActual.IdUsuario);

            // Controla visibilidad de los botones según permisos
            btneliminar.Visible = UtilPermisos.TienePermisoAccion(listaPermisos, "submenuequiposbaja", "btneliminar");
            btnlimpiar.Visible = UtilPermisos.TienePermisoAccion(listaPermisos, "submenuequiposbaja", "btnlimpiar");
            btnexportar.Visible = UtilPermisos.TienePermisoAccion(listaPermisos, "submenuequiposbaja", "btnexportar");

            try
            {
                CargarEquiposAutorizados();

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
                MessageBox.Show("Error al cargar formulario: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarEquiposAutorizados()
        {
            try
            {
                if (dgvdata.Columns.Count == 0)
                {
                    DataGridViewImageColumn btnSeleccionar = new DataGridViewImageColumn();
                    btnSeleccionar.Name = "btnseleccionar";
                    btnSeleccionar.HeaderText = "";
                    btnSeleccionar.Image = Properties.Resources.check20;
                    btnSeleccionar.ImageLayout = DataGridViewImageCellLayout.Zoom;
                    btnSeleccionar.Width = 30;
                    btnSeleccionar.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dgvdata.Columns.Add(btnSeleccionar);

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
                    dgvdata.Columns.Add("UsuarioAutorizador", "Usuario Autorizador");

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

                    foreach (DataGridViewColumn col in dgvdata.Columns)
                    {
                        if (col.Name == "NombreFarmacia")
                        {
                            col.Width = 150;
                            col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                        }
                        else if (col.Name != "btnseleccionar" && col.Name != "colMargen")
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

                var lista = objCN_Acta.ObtenerEquiposAutorizados();

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
                        item.oUsuarioAutorizador.NombreCompleto
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar equipos autorizados: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                    txtautorizador.Text = fila.Cells["UsuarioAutorizador"].Value?.ToString() ?? "";
                    txtmotivo.Text = fila.Cells["MotivoBaja"].Value?.ToString() ?? "";
                    txtestadobaja.Text = fila.Cells["EstadoBaja"].Value?.ToString() ?? "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al seleccionar el equipo: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            txtestado.Text = "";
            txtcantidad.Text = "";
            txtserial.Text = "";
            txtcaja.Text = "";
            txtsolicitante.Text = "";
            txtautorizador.Text = "";
            txtmotivo.Text = "";
            txtestadobaja.Text = "";
        }

        private void btnlimpiar_Click(object sender, EventArgs e)
        {
            try
            {
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al limpiar campos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btneliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvdata.CurrentRow != null)
                {
                    DataGridViewRow fila = dgvdata.CurrentRow;

                    string numeroDocumento = fila.Cells["NumeroDocumento"].Value.ToString().Trim();
                    string codigoEquipo = fila.Cells["CodigoEquipo"].Value.ToString().Trim();
                    string numeroSerial = fila.Cells["NumeroSerial"].Value.ToString().Trim();

                    DialogResult confirmacion = MessageBox.Show(
                        "¿Está seguro de eliminar este equipo autorizado?",
                        "Confirmar Eliminación",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (confirmacion == DialogResult.Yes)
                    {
                        bool eliminado = objCN_Acta.EliminarEquipoAutorizado(numeroDocumento, codigoEquipo, numeroSerial);

                        if (eliminado)
                        {
                            MessageBox.Show("Equipo eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            CargarEquiposAutorizados();
                            LimpiarCampos();
                        }
                        else
                        {
                            MessageBox.Show("No se pudo eliminar el equipo. Verifique que esté autorizado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Seleccione una fila para eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar equipo: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("Error al buscar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnlimpiarbuscador_Click(object sender, EventArgs e)
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
                MessageBox.Show("Error al buscar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnexportar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvdata.Rows.Count < 1)
                {
                    MessageBox.Show("No hay datos para exportar", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                DataTable dt = new DataTable();

                foreach (DataGridViewColumn columna in dgvdata.Columns)
                {
                    if (columna.Visible && columna.Name != "btnseleccionar")
                    {
                        dt.Columns.Add(columna.HeaderText, typeof(string));
                    }
                }

                foreach (DataGridViewRow row in dgvdata.Rows)
                {
                    if (row.Visible && !row.IsNewRow)
                    {
                        List<string> fila = new List<string>();

                        foreach (DataGridViewColumn columna in dgvdata.Columns)
                        {
                            if (columna.Visible && columna.Name != "btnseleccionar")
                            {
                                fila.Add(row.Cells[columna.Index].Value?.ToString() ?? "");
                            }
                        }

                        dt.Rows.Add(fila.ToArray());
                    }
                }

                SaveFileDialog savefile = new SaveFileDialog();
                savefile.FileName = $"EquiposBaja_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                savefile.Filter = "Excel Files|*.xlsx";

                if (savefile.ShowDialog() == DialogResult.OK)
                {
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        var hoja = wb.Worksheets.Add(dt, "Equipos Baja");
                        hoja.ColumnsUsed().AdjustToContents();
                        wb.SaveAs(savefile.FileName);
                    }

                    MessageBox.Show("Reporte exportado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (MessageBox.Show("¿Desea abrir el archivo ahora?", "Abrir archivo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(savefile.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al exportar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
