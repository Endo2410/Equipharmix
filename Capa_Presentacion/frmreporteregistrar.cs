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
    public partial class frmreporteregistrar : Form
    {
        public frmreporteregistrar()
        {
            InitializeComponent();
        }

        private void frmreporteregistrar_Load(object sender, EventArgs e)
        {
            // Obtiene los permisos del usuario logueado
            List<Permiso> listaPermisos = new CN_Permiso().Listar(Inicio.usuarioActual.IdUsuario);

            // Controla visibilidad de los botones según permisos
            btnexportar.Visible = UtilPermisos.TienePermisoAccion(listaPermisos, "submenureporteregistrar", "btnexportar");

            foreach (DataGridViewColumn columna in dgvdata.Columns)
            {
                cbobusqueda.Items.Add(new OpcionCombo() { Valor = columna.Name, Texto = columna.HeaderText });
            }
            cbobusqueda.DisplayMember = "Texto";
            cbobusqueda.ValueMember = "Valor";
            cbobusqueda.SelectedIndex = 0;
        }

        private void btnbuscar_Click(object sender, EventArgs e)
        {
            string columnaFiltro = ((OpcionCombo)cbobusqueda.SelectedItem).Valor.ToString();

            if (dgvdata.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvdata.Rows)
                {

                    if (row.Cells[columnaFiltro].Value.ToString().Trim().ToUpper().Contains(txtbusqueda.Text.Trim().ToUpper()))
                        row.Visible = true;
                    else
                        row.Visible = false;
                }
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

        private void btnbuscarresultado_Click(object sender, EventArgs e)
        {
            // Obtener lista desde la capa de negocio
            List<ReporteRegistrar> lista = new CN_Reporte().Registrar(
                txtfechainicio.Value.ToString("yyyy-MM-dd"),
                txtfechafin.Value.ToString("yyyy-MM-dd")
            );


            // Limpiar el DataGridView antes de llenarlo
            dgvdata.Rows.Clear();

            // Llenar el DataGridView con los resultados
            foreach (ReporteRegistrar rr in lista)
            {
                dgvdata.Rows.Add(new object[]
                {
                    rr.FechaRegistro,
                    rr.TipoDocumento,
                    rr.NumeroDocumento,
                    rr.UsuarioRegistro,
                    rr.Codigo,
                    rr.Nombre,
                    rr.Marca,
                    rr.Cantidad
                });
            }

            if (lista.Count == 0)
            {
                MessageBox.Show("No se encontraron registros en ese rango de fechas.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnexportar_Click(object sender, EventArgs e)
        {
            if (dgvdata.Rows.Count < 1)
            {
                MessageBox.Show("No hay registros para exportar", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            // Crear DataTable para exportar
            DataTable dt = new DataTable();

            // Agregar columnas al DataTable
            foreach (DataGridViewColumn columna in dgvdata.Columns)
            {
                dt.Columns.Add(columna.HeaderText, typeof(string));
            }

            // Agregar filas visibles
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

            // Guardar archivo
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.FileName = $"Reporte_Registrar_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
            savefile.Filter = "Excel Files | *.xlsx";

            if (savefile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    XLWorkbook wb = new XLWorkbook();
                    var hoja = wb.Worksheets.Add(dt, "Registro Equipos");
                    hoja.ColumnsUsed().AdjustToContents(); // Ajusta columnas automáticamente

                    wb.SaveAs(savefile.FileName);

                    MessageBox.Show("Reporte exportado correctamente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al exportar:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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

                string html = Properties.Resources.PlantillaRegistrar.ToString();

                // Datos del negocio
                Negocio datos = new CN_Negocio().ObtenerDatos();
                html = html.Replace("@nombrenegocio", datos.Nombre.ToUpper());
                html = html.Replace("@docnegocio", datos.RUC);
                html = html.Replace("@direcnegocio", datos.Direccion);

                // Construcción de filas para la tabla del PDF
                string filas = "";

                foreach (DataGridViewRow row in dgvdata.Rows)
                {
                    if (!row.Visible) continue;

                    filas += "<tr>";
                    filas += $"<td>{row.Cells["FechaRegistro"].Value}</td>";
                    filas += $"<td>{row.Cells["TipoDocumento"].Value}</td>";
                    filas += $"<td>{row.Cells["NumeroDocumento"].Value}</td>";
                    filas += $"<td>{row.Cells["UsuarioRegistro"].Value}</td>";
                    filas += $"<td>{row.Cells["Codigo"].Value}</td>";
                    filas += $"<td>{row.Cells["Nombre"].Value}</td>";
                    filas += $"<td>{row.Cells["Marca"].Value}</td>";
                    filas += $"<td>{row.Cells["Cantidad"].Value}</td>";
                    filas += "</tr>";
                }

                html = html.Replace("@filas", filas);

                SaveFileDialog savefile = new SaveFileDialog();
                savefile.FileName = $"Reporte_Registrar_{DateTime.Now:yyyyMMddHHmmss}.pdf";
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

                            // Ajusta la posición vertical del logo
                            img.SetAbsolutePosition(pdfDoc.Left + 5, pdfDoc.Top - 50);

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
