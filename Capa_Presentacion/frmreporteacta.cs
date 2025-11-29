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
    

    public partial class frmreporteacta : Form
    {
        private BindingSource _bindingSource = new BindingSource();
        private List<ActaReporte> listaOriginal = new List<ActaReporte>();

        public frmreporteacta()
        {
            InitializeComponent();

        }

        private void btnbuscarreporte_Click(object sender, EventArgs e)
        {
            dgvdata.Rows.Clear();

            List<ActaReporte> lista = new CN_Reporte().ReporteActas(
                txtfechainicio.Value.ToString("yyyy-MM-dd"),
                txtfechafin.Value.ToString("yyyy-MM-dd")
            );

            foreach (ActaReporte acta in lista)
            {
                dgvdata.Rows.Add(new object[]
                {
                    acta.NumeroDocumento,
                    acta.FechaRegistro,
                    acta.NombreFarmacia,
                    acta.CodigoEquipo,
                    acta.NombreEquipo,
                    acta.Cantidad,
                    acta.NumeroSerial,
                    acta.Caja,
                    acta.EstadoAutorizacion,
                    acta.NombreCompleto
                });
            }

            if (lista.Count == 0)
                MessageBox.Show("No se encontraron registros en ese rango de fechas.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void frmreporteacta_Load(object sender, EventArgs e)
        {
            // 🔥 FIJA el color del texto de las filas (el diseñador NO lo pone)
            dgvdata.RowsDefaultCellStyle.ForeColor = Color.Black;
            dgvdata.RowsDefaultCellStyle.BackColor = Color.White;

            //// Permisos
            //List<Permiso> listaPermisos = new CN_Permiso().Listar(Inicio.usuarioActual.IdUsuario);
            //btnexportar.Visible = UtilPermisos.TienePermisoAccion(listaPermisos, "submenureporteacta", "btnexportar");

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
            try
            {
                if (dgvdata.Rows.Count < 1)
                {
                    MessageBox.Show("No hay registros para exportar", "Mensaje",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                string html = Properties.Resources.PlantillaActas.ToString();

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
                    filas += $"<td>{row.Cells["NumeroDocumento"].Value}</td>";
                    filas += $"<td>{row.Cells["FechaRegistro"].Value}</td>";
                    filas += $"<td>{row.Cells["NombreFarmacia"].Value}</td>";
                    filas += $"<td>{row.Cells["CodigoEquipo"].Value}</td>";
                    filas += $"<td>{row.Cells["NombreEquipo"].Value}</td>";
                    filas += $"<td>{row.Cells["Cantidad"].Value}</td>";
                    filas += $"<td>{row.Cells["NumeroSerial"].Value}</td>";
                    filas += $"<td>{row.Cells["Caja"].Value}</td>";
                    filas += $"<td>{row.Cells["EstadoAutorizacion"].Value}</td>";
                    filas += $"<td>{row.Cells["NombreCompleto"].Value}</td>";
                    filas += "</tr>";
                }

                html = html.Replace("@filas", filas);

                SaveFileDialog savefile = new SaveFileDialog();
                savefile.FileName = $"Reporte_Actas_{DateTime.Now:yyyyMMddHHmmss}.pdf";
                savefile.Filter = "PDF Files|*.pdf";

                if (savefile.ShowDialog() == DialogResult.OK)
                {
                    using (FileStream stream = new FileStream(savefile.FileName, FileMode.Create))
                    {
                        Document pdfDoc = new Document(PageSize.A4, 25, 25, 90, 25);
                        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                        pdfDoc.Open();

                        // LOGO ENCIMA DE LA LÍNEA
                        bool obtenido;
                        byte[] byteImage = new CN_Negocio().ObtenerLogo(out obtenido);

                        if (obtenido)
                        {
                            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(byteImage);
                            img.ScaleToFit(70, 70);
                            img.SetAbsolutePosition(pdfDoc.Left + 5, pdfDoc.Top - 55); //  aquí ajustás el margen
                            pdfDoc.Add(img);
                        }

                        using (StringReader sr = new StringReader(html))
                        {
                            XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                        }

                        pdfDoc.Close();
                        stream.Close();
                    }

                    MessageBox.Show("PDF generado correctamente", "Mensaje",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar el PDF: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
