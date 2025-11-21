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
    public partial class frmreportefarmacia : Form
    {
        public frmreportefarmacia()
        {
            InitializeComponent();
        }

        private void btnbuscarreporte_Click(object sender, EventArgs e)
        {
            dgvdata.Rows.Clear(); // Limpiar filas previas

            List<Farmacia> lista = new CN_Farmacia().ReporteFarmacia(
                txtfechainicio.Value.ToString("yyyy-MM-dd"),
                txtfechafin.Value.ToString("yyyy-MM-dd")
            );

            foreach (var f in lista)
            {
                dgvdata.Rows.Add(new object[]
                {
                    f.IdFarmacia,
                    f.Codigo,
                    f.Nombre,
                    f.Correo,
                    f.Telefono,
                    f.Estado ? "ACTIVO" : "INACTIVO",
                    f.FechaRegistro.ToString("dd/MM/yyyy")
                });
            }

            if (lista.Count == 0)
                MessageBox.Show("No se encontraron farmacias en ese rango de fechas.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void frmreportefarmacia_Load(object sender, EventArgs e)
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
                savefile.FileName = $"Reporte_Farmacias_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                savefile.Filter = "Excel Files | *.xlsx";

                if (savefile.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        XLWorkbook wb = new XLWorkbook();
                        var hoja = wb.Worksheets.Add(dt, "Reporte Farmacias");
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

       
        private void btndescargar_Click(object sender, EventArgs e)
        {
            if (dgvdata.Rows.Count < 1)
            {
                MessageBox.Show("No hay registros para generar PDF", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            string Texto_Html = Properties.Resources.PlantillaFarmacia.ToString();

            // Datos del negocio
            Negocio odatos = new CN_Negocio().ObtenerDatos();
            Texto_Html = Texto_Html.Replace("@nombrenegocio", odatos.Nombre.ToUpper());
            Texto_Html = Texto_Html.Replace("@docnegocio", odatos.RUC);
            Texto_Html = Texto_Html.Replace("@direcnegocio", odatos.Direccion);

            // Generar filas solo de las visibles
            string filas = "";
            foreach (DataGridViewRow row in dgvdata.Rows)
            {
                if (row.IsNewRow || !row.Visible) continue;

                filas += "<tr>";
                filas += $"<td>{row.Cells["Codigo"].Value}</td>";
                filas += $"<td>{row.Cells["Nombre"].Value}</td>";
                filas += $"<td>{row.Cells["Correo"].Value}</td>";
                filas += $"<td>{row.Cells["Telefono"].Value}</td>";
                filas += $"<td>{row.Cells["Estado"].Value}</td>";
                filas += $"<td>{row.Cells["FechaRegistro"].Value}</td>";
                filas += "</tr>";
            }
            Texto_Html = Texto_Html.Replace("@filas", filas);

            SaveFileDialog savefile = new SaveFileDialog();
            savefile.FileName = $"Reporte_Farmacias_{DateTime.Now:yyyyMMddHHmmss}.pdf";
            savefile.Filter = "Pdf Files|*.pdf";

            if (savefile.ShowDialog() == DialogResult.OK)
            {
                using (FileStream stream = new FileStream(savefile.FileName, FileMode.Create))
                {
                    Document pdfDoc = new Document(PageSize.A4, 25, 25, 25, 25);
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();

                    // Logo arriba, antes de la línea horizontal
                    bool obtenido = true;
                    byte[] byteImage = new CN_Negocio().ObtenerLogo(out obtenido);
                    if (obtenido)
                    {
                        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(byteImage);
                        img.ScaleToFit(80, 80);
                        img.Alignment = iTextSharp.text.Image.ALIGN_LEFT;
                        img.SetAbsolutePosition(pdfDoc.Left, pdfDoc.Top - 80); // Ajusta la posición
                        pdfDoc.Add(img);

                        // Agregar un espacio extra entre logo y la línea horizontal
                        pdfDoc.Add(new Paragraph("\n\n"));
                    }

                    using (StringReader sr = new StringReader(Texto_Html))
                    {
                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                    }

                    pdfDoc.Close();
                    stream.Close();
                    MessageBox.Show("PDF generado correctamente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}
