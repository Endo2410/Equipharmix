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
        private CN_Prestamo objCN_Prestamo = new CN_Prestamo();
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
                    if (columna.Visible && columna.Name != "btnseleccionar")
                    {
                        cbobusqueda.Items.Add(new OpcionCombo() { Valor = columna.Name, Texto = columna.HeaderText });
                    }
                }
                cbobusqueda.DisplayMember = "Texto";
                cbobusqueda.ValueMember = "Valor";
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
                dgvdata.Rows.Clear();

                // Equipos autorizados de ACTA
                var listaActa = objCN_Acta.ObtenerEquiposAutorizados();
                foreach (var item in listaActa)
                {
                    dgvdata.Rows.Add(
                        "", // botón seleccionar
                        item.NumeroDocumento,
                        item.FechaRegistro.ToString("dd/MM/yyyy"),
                        item.oFarmacia?.Nombre ?? "",
                        item.oEquipo.Codigo,
                        item.oEquipo.Nombre,
                        item.oEquipo.oMarca?.Descripcion ?? "",
                        item.Cantidad,
                        item.NumeroSerial,
                        item.Caja,                     // Caja siempre existe en ACTA
                        item.MotivoBaja,
                        item.EstadoBaja,
                        item.oUsuarioSolicitante?.NombreCompleto ?? "",
                        item.oUsuarioAutorizador?.NombreCompleto ?? "",
                        "ACTA"                         // Identificador
                    );
                }

                //  Equipos autorizados de PRÉSTAMO
                var listaPrestamo = objCN_Prestamo.ObtenerEquiposPrestamoAutorizados();
                foreach (var item in listaPrestamo)
                {
                    dgvdata.Rows.Add(
                        "",
                        item.NumeroDocumento,
                        item.FechaPrestamo.ToString("dd/MM/yyyy"),
                        item.oFarmacia?.Nombre ?? "",
                        item.oEquipo.Codigo,
                        item.oEquipo.Nombre,
                        item.oEquipo.oMarca?.Descripcion ?? "",
                        item.Cantidad,
                        item.NumeroSerial,
                        "", // Caja vacío
                        item.MotivoBaja,
                        item.EstadoBaja,
                        item.oUsuarioSolicitante?.NombreCompleto ?? "",
                        item.oUsuarioAutorizador?.NombreCompleto ?? "",
                        "PRESTAMO"                     // Identificador
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar equipos autorizados: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                if (dgvdata.Rows.Count < 1) return;

                DataTable dt = new DataTable();
                foreach (DataGridViewColumn col in dgvdata.Columns)
                {
                    if (col.Visible && col.Name != "btnseleccionar")
                        dt.Columns.Add(col.HeaderText, typeof(string));
                }

                foreach (DataGridViewRow row in dgvdata.Rows)
                {
                    if (row.Visible && !row.IsNewRow)
                    {
                        List<string> fila = new List<string>();
                        foreach (DataGridViewColumn col in dgvdata.Columns)
                        {
                            if (col.Visible && col.Name != "btnseleccionar")
                                fila.Add(row.Cells[col.Index].Value?.ToString() ?? "");
                        }
                        dt.Rows.Add(fila.ToArray());
                    }
                }

                SaveFileDialog savefile = new SaveFileDialog();
                savefile.FileName = $"EquiposBaja_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                savefile.Filter = "Excel Files|*.xlsx";

                if (savefile.ShowDialog() != DialogResult.OK) return;

                using (XLWorkbook wb = new XLWorkbook())
                {
                    var hoja = wb.Worksheets.Add(dt, "Equipos Baja");
                    hoja.ColumnsUsed().AdjustToContents();
                    wb.SaveAs(savefile.FileName);
                }

                MessageBox.Show("Reporte exportado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al exportar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvdata_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;

                if (e.ColumnIndex == dgvdata.Columns["btnseleccionar"].Index)
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
            catch { }
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
    }
}

