using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.Utilidades;
using ClosedXML.Excel; 
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaPresentacion.Modales;

namespace CapaPresentacion
{
    public partial class frmactaautorizacion : Form
    {

        private CN_Acta objCN_Acta = new CN_Acta();
        private static Usuario usuarioActual;

        public frmactaautorizacion(Usuario objusuario)
        {
            usuarioActual = objusuario;
            InitializeComponent();
        }

        private void frmactaautorizacion_Load(object sender, EventArgs e)
        {
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

                    // Columnas datos
                    dgvdata.Columns.Add("NumeroDocumento", "Documento");
                    dgvdata.Columns.Add("FechaRegistro", "Fecha");
                    dgvdata.Columns.Add("NombreFarmacia", "Farmacia");
                    dgvdata.Columns.Add("EstadoAutorizacion", "Autorización");
                    dgvdata.Columns.Add("CreadorActa", "Usuario Solicitante");

                    // Columna margen
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

                CargarEquiposPendientes();

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
                if (cbobusqueda.Items.Count > 0) cbobusqueda.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarEquiposPendientes()
        {
            try
            {
                dgvdata.Rows.Clear();
                var lista = objCN_Acta.ObtenerEquiposPendientes();

                // Agrupar por NumeroDocumento
                var documentosAgrupados = lista
                    .GroupBy(x => x.NumeroDocumento)
                    .Select(g => g.First()) // solo una fila por documento
                    .ToList();

                foreach (var item in documentosAgrupados)
                {
                    dgvdata.Rows.Add(
                        Properties.Resources.check20,
                        item.NumeroDocumento,
                        item.FechaRegistro.ToShortDateString(),
                        item.oFarmacia.Nombre,
                        item.EstadoAutorizacion,
                        item.oUsuarioSolicitante.NombreCompleto
                        
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar equipos pendientes:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void dgvdata_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && dgvdata.Columns[e.ColumnIndex].Name == "btnseleccionar")
                {
                    string numeroDocumento = dgvdata.Rows[e.RowIndex].Cells["NumeroDocumento"].Value.ToString();
                    mdActa detalle = new mdActa(numeroDocumento, usuarioActual.IdUsuario);
                    if (detalle.ShowDialog() == DialogResult.OK)
                    {
                        frmactaautorizacion_Load(null, null); // recargar lista
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abrir detalle:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("Error al buscar:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("Error al limpiar búsqueda:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
