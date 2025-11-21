using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.Utilidades;
using ClosedXML.Excel;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using iTextSharp.text;
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
    public partial class frmreporteauditoria : Form
    {
        public frmreporteauditoria()
        {
            InitializeComponent();
        }

        private void btnbuscarreporte_Click(object sender, EventArgs e)
        {
            dgvdata.Rows.Clear();

            string tablaSeleccionada = cbobusqueda.SelectedItem?.ToString() ?? "";
            string fechaInicio = txtfechainicio.Value.ToString("yyyy-MM-dd");
            string fechaFin = txtfechafin.Value.ToString("yyyy-MM-dd");

            List<Auditoria> lista = new CN_Reporte().ObtenerAuditoria(tablaSeleccionada, fechaInicio, fechaFin);

            foreach (Auditoria aud in lista)
            {
                dgvdata.Rows.Add(new object[]
                {
                    aud.IdAuditoria,
                    aud.Tabla,
                    aud.Operacion,
                    aud.Usuario,
                    aud.Fecha,
                    aud.Datos
                });
            }

            if (lista.Count == 0)
                MessageBox.Show("No se encontraron registros en ese rango de fechas.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void frmreporteauditoria_Load(object sender, EventArgs e)
        {
            try
            {
                // Llenar combo de búsqueda usando las columnas del DGV
                cbobusqueda.Items.Clear();

                foreach (DataGridViewColumn col in dgvdata.Columns)
                {
                    cbobusqueda.Items.Add(new OpcionCombo()
                    {
                        Valor = col.Name,
                        Texto = col.HeaderText
                    });
                }

                cbobusqueda.DisplayMember = "Texto";
                cbobusqueda.ValueMember = "Valor";

                if (cbobusqueda.Items.Count > 0)
                    cbobusqueda.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar el formulario: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnbuscar_Click(object sender, EventArgs e)
        {
            string columnaFiltro = ((OpcionCombo)cbobusqueda.SelectedItem).Valor.ToString();
            string textoFiltro = txtbusqueda.Text.Trim().ToUpper();

            foreach (DataGridViewRow row in dgvdata.Rows)
            {
                var valorCelda = row.Cells[columnaFiltro].Value?.ToString().Trim().ToUpper() ?? "";

                if (columnaFiltro == "Estado") // comparación exacta para Estado
                    row.Visible = valorCelda == textoFiltro;
                else // búsqueda parcial para el resto
                    row.Visible = valorCelda.Contains(textoFiltro);
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

                // Solo agregar columnas visibles
                var columnasVisibles = dgvdata.Columns.Cast<DataGridViewColumn>()
                                        .Where(c => c.Visible)
                                        .ToList();

                foreach (DataGridViewColumn columna in columnasVisibles)
                {
                    dt.Columns.Add(columna.HeaderText, typeof(string));
                }

                // Agregar filas visibles
                foreach (DataGridViewRow row in dgvdata.Rows)
                {
                    if (row.Visible)
                    {
                        object[] fila = new object[columnasVisibles.Count];
                        for (int i = 0; i < columnasVisibles.Count; i++)
                        {
                            fila[i] = row.Cells[columnasVisibles[i].Index].Value?.ToString() ?? "";
                        }
                        dt.Rows.Add(fila);
                    }
                }

                SaveFileDialog savefile = new SaveFileDialog();
                savefile.FileName = $"ReporteAuditoria_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                savefile.Filter = "Excel Files | *.xlsx";

                if (savefile.ShowDialog() == DialogResult.OK)
                {
                    XLWorkbook wb = new XLWorkbook();
                    var hoja = wb.Worksheets.Add(dt, "Reporte Auditoría");
                    hoja.ColumnsUsed().AdjustToContents();
                    wb.SaveAs(savefile.FileName);

                    MessageBox.Show("Reporte exportado correctamente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al exportar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btndescargar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvdata.Rows.Count < 1)
                {
                    MessageBox.Show("No hay registros para exportar", "Mensaje",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                string html = Properties.Resources.PlantillaAuditoria.ToString();

                // Datos del negocio
                Negocio datos = new CN_Negocio().ObtenerDatos();
                html = html.Replace("@nombrenegocio", datos.Nombre.ToUpper());
                html = html.Replace("@docnegocio", datos.RUC);
                html = html.Replace("@direcnegocio", datos.Direccion);

                // Construcción de filas
                string filas = "";
                foreach (DataGridViewRow row in dgvdata.Rows)
                {
                    if (!row.Visible) continue;

                    filas += "<tr>";
                    filas += $"<td>{row.Cells["Tabla"].Value}</td>";
                    filas += $"<td>{row.Cells["Operacion"].Value}</td>";
                    filas += $"<td>{row.Cells["Usuario"].Value}</td>";
                    filas += $"<td>{row.Cells["Fecha"].Value}</td>";
                    filas += $"<td>{row.Cells["Datos"].Value}</td>";
                    filas += "</tr>";
                }

                html = html.Replace("@filas", filas);

                SaveFileDialog savefile = new SaveFileDialog();
                savefile.FileName = $"Reporte_Auditoria_{DateTime.Now:yyyyMMddHHmmss}.pdf";
                savefile.Filter = "PDF Files|*.pdf";

                if (savefile.ShowDialog() == DialogResult.OK)
                {
                    using (FileStream stream = new FileStream(savefile.FileName, FileMode.Create))
                    {
                        Document pdfDoc = new Document(PageSize.A4, 25, 25, 90, 25);
                        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                        pdfDoc.Open();

                        // Logo negocio (opcional)
                        bool obtenido;
                        byte[] byteImage = new CN_Negocio().ObtenerLogo(out obtenido);
                        if (obtenido)
                        {
                            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(byteImage);
                            img.ScaleToFit(70, 70);
                            img.SetAbsolutePosition(pdfDoc.Left + 5, pdfDoc.Top - 55);
                            pdfDoc.Add(img);
                        }

                        using (StringReader sr = new StringReader(html))
                        {
                            XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                        }

                        pdfDoc.Close();
                        stream.Close();
                    }

                    MessageBox.Show("PDF generado correctamente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar PDF: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
