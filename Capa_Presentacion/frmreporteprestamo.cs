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
    public partial class frmreporteprestamo : Form
    {
        public frmreporteprestamo()
        {
            InitializeComponent();
        }

        private void frmreporteprestamo_Load(object sender, EventArgs e)
        {
            dgvdata.RowsDefaultCellStyle.ForeColor = Color.Black;
            dgvdata.RowsDefaultCellStyle.BackColor = Color.White;


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

        private void btnbuscarreporte_Click(object sender, EventArgs e)
        {
            dgvdata.Rows.Clear();

            List<Prestamo_Reporte> lista = new CN_Reporte().ReportePrestamo(
                txtfechainicio.Value.ToString("yyyy-MM-dd"),
                txtfechafin.Value.ToString("yyyy-MM-dd")
            );

            foreach (Prestamo_Reporte prestamo in lista)
            {
                dgvdata.Rows.Add(new object[]
                {
                    prestamo.NumeroDocumento,
                    prestamo.FechaRegistro,
                    prestamo.NombreFarmacia,
                    prestamo.CodigoEquipo,
                    prestamo.NombreEquipo,
                    prestamo.Cantidad,
                    prestamo.NumeroSerial,
                    prestamo.EstadoPrestamo,
                    prestamo.NombreCompleto
                });
            }

            if (lista.Count == 0)
                MessageBox.Show("No se encontraron registros en ese rango de fechas.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnbuscar_Click(object sender, EventArgs e)
        {
            string columnaFiltro = ((OpcionCombo)cbobusqueda.SelectedItem).Valor.ToString();
            string textoFiltro = txtbusqueda.Text.Trim().ToUpper();

            foreach (DataGridViewRow row in dgvdata.Rows)
            {
                string valorCelda = row.Cells[columnaFiltro].Value?.ToString().Trim().ToUpper() ?? "";

                if (columnaFiltro == "Estado")
                    row.Visible = valorCelda == textoFiltro;
                else
                    row.Visible = valorCelda.Contains(textoFiltro);
            }
        }

        private void btnlimpiarbuscador_Click(object sender, EventArgs e)
        {
            txtbusqueda.Clear();

            foreach (DataGridViewRow row in dgvdata.Rows)
                row.Visible = true;
        }

        private void btnexportar_Click(object sender, EventArgs e)
        {
            if (dgvdata.Rows.Count < 1)
            {
                MessageBox.Show("No hay registros para exportar", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            DataTable dt = new DataTable();
            foreach (DataGridViewColumn col in dgvdata.Columns)
                dt.Columns.Add(col.HeaderText, typeof(string));

            foreach (DataGridViewRow row in dgvdata.Rows)
            {
                if (!row.Visible) continue;

                object[] fila = new object[dgvdata.Columns.Count];
                for (int i = 0; i < dgvdata.Columns.Count; i++)
                    fila[i] = row.Cells[i].Value?.ToString() ?? "";

                dt.Rows.Add(fila);
            }

            SaveFileDialog savefile = new SaveFileDialog();
            savefile.FileName = $"ReportePrestamos_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
            savefile.Filter = "Excel Files | *.xlsx";

            if (savefile.ShowDialog() == DialogResult.OK)
            {
                XLWorkbook wb = new XLWorkbook();
                var hoja = wb.Worksheets.Add(dt, "Reporte Préstamos");
                hoja.ColumnsUsed().AdjustToContents();
                wb.SaveAs(savefile.FileName);
                MessageBox.Show("Reporte exportado correctamente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btndescargar_Click(object sender, EventArgs e)
        {
            if (dgvdata.Rows.Count < 1)
            {
                MessageBox.Show("No hay registros para exportar", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            string html = Properties.Resources.PlantillaPrestamos.ToString();

            Negocio datos = new CN_Negocio().ObtenerDatos();
            html = html.Replace("@nombrenegocio", datos.Nombre.ToUpper());
            html = html.Replace("@docnegocio", datos.RUC);
            html = html.Replace("@direcnegocio", datos.Direccion);

            string filas = "";
            foreach (DataGridViewRow row in dgvdata.Rows)
            {
                if (!row.Visible) continue;

                filas += "<tr>";
                filas += $"<td>{row.Cells["NumeroDocumento"].Value}</td>";
                filas += $"<td>{row.Cells["FechaRegistro"].Value}</td>";
                filas += $"<td>{row.Cells["NombreFarmacia"].Value}</td>";
                filas += $"<td>{row.Cells["CodigoEquipo"].Value}</td>";
                filas += $"<td>{row.Cells["NombreEquipo"].Value}</td>";
                filas += $"<td>{row.Cells["Cantidad"].Value}</td>";
                filas += $"<td>{row.Cells["NumeroSerial"].Value}</td>";
                filas += $"<td>{row.Cells["EstadoPrestamo"].Value}</td>";
                filas += $"<td>{row.Cells["NombreCompleto"].Value}</td>";
                filas += "</tr>";
            }

            html = html.Replace("@filas", filas);

            SaveFileDialog savefile = new SaveFileDialog();
            savefile.FileName = $"Reporte_Prestamos_{DateTime.Now:yyyyMMddHHmmss}.pdf";
            savefile.Filter = "PDF Files|*.pdf";

            if (savefile.ShowDialog() == DialogResult.OK)
            {
                using (FileStream stream = new FileStream(savefile.FileName, FileMode.Create))
                {
                    Document pdfDoc = new Document(PageSize.A4, 25, 25, 90, 25);
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();

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
                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);

                    pdfDoc.Close();
                    stream.Close();
                }

                MessageBox.Show("PDF generado correctamente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

    }
}
