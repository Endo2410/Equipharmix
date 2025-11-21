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
        private CN_Prestamo objCN_Prestamo = new CN_Prestamo();
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
                // Crear columna de selección si no existe
                if (!dgvdata.Columns.Contains("btnseleccionar"))
                {
                    DataGridViewImageColumn colBtn = new DataGridViewImageColumn();
                    colBtn.Name = "btnseleccionar";
                    colBtn.HeaderText = "";
                    colBtn.ImageLayout = DataGridViewImageCellLayout.Normal;
                    colBtn.Width = 30;
                    dgvdata.Columns.Insert(0, colBtn);
                }

                // Llenar combo de búsqueda
                cbobusqueda.Items.Clear();
                foreach (DataGridViewColumn columna in dgvdata.Columns)
                {
                    if (columna.Visible && columna.Name != "btnseleccionar")
                    {
                        cbobusqueda.Items.Add(new OpcionCombo()
                        {
                            Valor = columna.Name,
                            Texto = columna.HeaderText
                        });
                    }
                }
                cbobusqueda.DisplayMember = "Texto";
                cbobusqueda.ValueMember = "Valor";
                if (cbobusqueda.Items.Count > 0)
                    cbobusqueda.SelectedIndex = 0;

                CargarPendientes();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos:\n" + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarPendientes()
        {
            dgvdata.Rows.Clear();

            // Obtener asignaciones
            var listaAsignaciones = objCN_Acta.ObtenerEquiposPendientes();
            foreach (var item in listaAsignaciones)
            {
                dgvdata.Rows.Add(
                    "", // columna de selección
                    "ASIGNACIÓN",
                    item.NumeroDocumento,
                    item.FechaRegistro.ToShortDateString(),
                    item.oFarmacia.Nombre,
                    item.EstadoAutorizacion,
                    item.oUsuarioSolicitante.NombreCompleto                 
                );
            }

            // Obtener préstamos
            var listaPrestamos = objCN_Prestamo.ObtenerPrestamosPendientes();
            foreach (var item in listaPrestamos)
            {
                dgvdata.Rows.Add(
                    "",
                    "PRÉSTAMO",
                    item.NumeroDocumento,
                    item.FechaRegistro.ToShortDateString(),
                    item.oFarmacia?.Nombre ?? "",
                    item.EstadoAutorizacion,
                    item.oUsuarioSolicitante.NombreCompleto
                    
                );
            }
        }

        private void btnbuscar_Click(object sender, EventArgs e)
        {
            try
            {
                string columnaFiltro = ((OpcionCombo)cbobusqueda.SelectedItem).Valor.ToString();

                foreach (DataGridViewRow row in dgvdata.Rows)
                {
                    bool coincide =
                        row.Cells[columnaFiltro].Value != null &&
                        row.Cells[columnaFiltro].Value.ToString().Trim()
                        .ToUpper()
                        .Contains(txtbusqueda.Text.Trim().ToUpper());

                    row.Visible = coincide;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar:\n" + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("Error al limpiar búsqueda:\n" + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvdata_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) return;

                if (e.ColumnIndex == dgvdata.Columns["btnseleccionar"].Index)
                {
                    e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                    var img = Properties.Resources.check20;
                    int x = e.CellBounds.Left + (e.CellBounds.Width - img.Width) / 2;
                    int y = e.CellBounds.Top + (e.CellBounds.Height - img.Height) / 2;

                    e.Graphics.DrawImage(img, new Rectangle(x, y, img.Width, img.Height));
                    e.Handled = true;
                }
            }
            catch { }
        }

        private void dgvdata_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvdata.Columns[e.ColumnIndex].Name == "btnseleccionar")
            {
                string numeroDocumento = dgvdata.Rows[e.RowIndex].Cells["NumeroDocumento"].Value.ToString();
                string tipo = dgvdata.Rows[e.RowIndex].Cells["TipoMovimiento"].Value.ToString();

                if (tipo == "ASIGNACIÓN")
                {
                    mdActa detalle = new mdActa(numeroDocumento, usuarioActual.IdUsuario);
                    if (detalle.ShowDialog() == DialogResult.OK)
                        CargarPendientes();
                }
                else if (tipo == "PRÉSTAMO")
                {
                    mdActa detalle = new mdActa(numeroDocumento, usuarioActual.IdUsuario);
                    if (detalle.ShowDialog() == DialogResult.OK)
                        CargarPendientes();
                }
            }
        }
    }
}
