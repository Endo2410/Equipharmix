using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.Utilidades;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class frmreporteacta : Form
    {
        public frmreporteacta()
        {
            InitializeComponent();
        }

        private void btnbuscarreporte_Click(object sender, EventArgs e)
        {
            try
            {
                List<ActaReporte> lista = new CN_Reporte().ReporteActas(
                     txtfechainicio.Value.ToString("yyyy-MM-dd"),
                     txtfechafin.Value.ToString("yyyy-MM-dd")
                );

                dgvdata.DataSource = lista;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar el reporte: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmreporteacta_Load(object sender, EventArgs e)
        {
            try
            {
                dgvdata.AutoGenerateColumns = false;
                dgvdata.Columns.Clear();

                var columnas = new (string Propiedad, string Titulo)[]
                {
            ("NumeroDocumento", "N° Documento"),
            ("FechaRegistro", "Fecha"),
            ("NombreFarmacia", "Farmacia"),
            ("CodigoEquipo", "Cod. Equipo"),
            ("NombreEquipo", "Nombre Equipo"),
            ("Estado", "Estado"),
            ("Cantidad", "Cantidad"),
            ("NumeroSerial", "Serial"),
            ("Caja", "Caja"),
            ("EstadoAutorizacion", "Autorización")
                };

                foreach (var col in columnas)
                {
                    dgvdata.Columns.Add(new DataGridViewTextBoxColumn()
                    {
                        Name = col.Propiedad,
                        DataPropertyName = col.Propiedad,
                        HeaderText = col.Titulo,
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                    });
                }

                dgvdata.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                cbobusqueda.Items.Clear();
                foreach (var col in columnas)
                {
                    cbobusqueda.Items.Add(new OpcionCombo() { Valor = col.Propiedad, Texto = col.Titulo });
                }

                cbobusqueda.DisplayMember = "Texto";
                cbobusqueda.ValueMember = "Valor";
                cbobusqueda.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar el formulario: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnexportar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvdata.Rows.Count < 1)
                {
                    MessageBox.Show("No hay registros para exportar", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                DataTable dt = new DataTable();

                foreach (DataGridViewColumn columna in dgvdata.Columns)
                {
                    dt.Columns.Add(columna.HeaderText, typeof(string));
                }

                foreach (DataGridViewRow row in dgvdata.Rows)
                {
                    if (row.Visible)
                    {
                        object[] fila = new object[dgvdata.Columns.Count];
                        for (int i = 0; i < dgvdata.Columns.Count; i++)
                        {
                            fila[i] = row.Cells[i].Value?.ToString() ?? "";
                        }
                        dt.Rows.Add(fila);
                    }
                }

                SaveFileDialog savefile = new SaveFileDialog();
                savefile.FileName = $"ReporteActas_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                savefile.Filter = "Excel Files | *.xlsx";

                if (savefile.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        XLWorkbook wb = new XLWorkbook();
                        var hoja = wb.Worksheets.Add(dt, "Reporte Actas");
                        hoja.ColumnsUsed().AdjustToContents();
                        wb.SaveAs(savefile.FileName);

                        MessageBox.Show("Reporte exportado correctamente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al generar el archivo Excel:\n" + ex.Message, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al procesar la exportación: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnbuscar_Click(object sender, EventArgs e)
        {
            try
            {
                string columnaFiltro = ((OpcionCombo)cbobusqueda.SelectedItem).Valor.ToString();

                if (!dgvdata.Columns.Contains(columnaFiltro))
                {
                    MessageBox.Show($"La columna '{columnaFiltro}' no existe en el DataGridView.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

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
                MessageBox.Show("Error al filtrar los datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("Error al limpiar el buscador: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
