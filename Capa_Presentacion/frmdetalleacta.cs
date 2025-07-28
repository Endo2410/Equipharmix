using CapaEntidad;
using CapaNegocio;
using System;
using System.IO;
using System.Windows.Forms;



namespace CapaPresentacion
{
    public partial class frmdetalleacta : Form
    {
       

        public frmdetalleacta()
        {
            InitializeComponent();
        }

        private void frmdetalleacta_Load(object sender, EventArgs e)
        {

            txtbusqueda.Select();
        }

        private void btnbuscar_Click(object sender, EventArgs e)
        {
            Acta oActa = null; // Declarar aquí para que tenga alcance en todo el método

            try
            {
                CN_Acta cnActa = new CN_Acta();
                oActa = cnActa.ObtenerActa(txtbusqueda.Text.Trim());

                if (oActa != null && oActa.IdActa != 0)
                {
                    txtnumerodocumento.Text = oActa.NumeroDocumento;
                    txtfecha.Text = oActa.FechaRegistro;
                    txttipodocumento.Text = oActa.TipoDocumento;
                    txtusuario.Text = oActa.oUsuario?.NombreCompleto ?? "";

                    txtcodigo.Text = oActa.Codigo;
                    txtfarmacia.Text = oActa.Nombre;

                    // Mostrar el primer serial, caja y marca (si existen)
                    if (oActa.oDetalle_Acta != null && oActa.oDetalle_Acta.Count > 0)
                    {
                        txtcaja.Text = oActa.oDetalle_Acta[0].Caja;
                        txtnumeroserial.Text = oActa.oDetalle_Acta[0].NumeroSerial;
                    }
                    else
                    {
                        txtcaja.Text = "";
                        txtnumeroserial.Text = "";                       
                    }

                    dgvdata.Rows.Clear();

                    foreach (Detalle_Acta dv in oActa.oDetalle_Acta)
                    {
                        dgvdata.Rows.Add(new object[]
                        {
                            dv.oEquipo.Nombre,
                            dv.oEquipo.oMarca?.Descripcion ?? "",
                            dv.Cantidad,
                            dv.NumeroSerial,
                            dv.Caja
                        });
                    }

                    // Mostrar u ocultar botones según EstadoAutorizacion
                    if (oActa.EstadoAutorizacion?.Trim().ToUpper() == "AUTORIZADO")
                    {
                        btndescargar.Visible = true;
                    }
                    else
                    {
                        btndescargar.Visible = false;
                    }
                }
                else
                {
                    // Si no encontró acta, limpiar campos y ocultar botones
                    LimpiarCampos();
                    btndescargar.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                btndescargar.Visible = false;
            }
        }

        private void LimpiarCampos()
        {
            txtnumerodocumento.Text = "";
            txtfecha.Text = "";
            txttipodocumento.Text = "";
            txtusuario.Text = "";
            txtcodigo.Text = "";
            txtfarmacia.Text = "";
            txtcaja.Text = "";
            txtnumeroserial.Text = "";
            dgvdata.Rows.Clear();
        }

        private void btnborrar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

      

        private void btndescargar_Click(object sender, EventArgs e)
        {
            GenerarPdf();
        }

        private void GenerarPdf()
        {
            try
            {
                // Construir las filas HTML desde el DataGridView
                string filasHtml = "";
                foreach (DataGridViewRow row in dgvdata.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        filasHtml += $@"
                        <tr>
                            <td>{row.Cells[0].Value}</td>
                            <td>{row.Cells[1].Value}</td>
                            <td>{row.Cells[2].Value}</td>
                            <td>{row.Cells[3].Value}</td>
                            <td>{row.Cells[4].Value}</td>
                        </tr>";
                    }
                }

                // Ruta base a plantilla e imágenes
                string basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Utilidades", "piloto");

                // Leer HTML y reemplazar etiquetas (sin tocar rutas imágenes)
                string htmlContent = File.ReadAllText(Path.Combine(basePath, "plantilla.html"))
                    .Replace("@docframacia", txtcodigo.Text)
                    .Replace("@nombre", txtfarmacia.Text)
                    .Replace("@fecharegistro", txtfecha.Text)
                    .Replace("@usuarioregistro", txtusuario.Text)
                    .Replace("@numerodocumento", txtnumerodocumento.Text)
                    .Replace("@filas", filasHtml);

                // Guardar HTML temporal en la carpeta "piloto"
                string htmlTempPath = Path.Combine(basePath, "acta_temp.html");
                File.WriteAllText(htmlTempPath, htmlContent);

                // Ruta al ejecutable wkhtmltopdf
                string wkhtmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Utilidades", "wkhtmltopdf.exe");

                // Ruta salida PDF
                string nombreArchivo = $"ACTA_{txtnumerodocumento.Text.Trim().Replace(" ", "_")}.pdf";
                string outputPdfPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), nombreArchivo);


                // Configurar proceso para wkhtmltopdf
                var psi = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = wkhtmlPath,
                    Arguments = $"--enable-local-file-access \"{htmlTempPath}\" \"{outputPdfPath}\"",
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = System.Diagnostics.Process.Start(psi))
                {
                    process.WaitForExit();

                    if (File.Exists(outputPdfPath))
                    {
                        MessageBox.Show("PDF generado correctamente en el Escritorio.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("No se generó el archivo PDF. Verifica la ruta del ejecutable.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar PDF: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
