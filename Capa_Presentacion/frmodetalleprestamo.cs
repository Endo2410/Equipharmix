using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.Utilidades;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;


namespace CapaPresentacion
{
    public partial class frmodetalleprestamo : Form
    {
        public frmodetalleprestamo()
        {
            InitializeComponent();
        }

        private void btnbuscar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            Prestamo oPrestamo = null;

            try
            {
                CN_Prestamo cnPrestamo = new CN_Prestamo();
                oPrestamo = cnPrestamo.ObtenerPrestamo(txtbusqueda.Text.Trim());

                if (oPrestamo != null && oPrestamo.IdPrestamo != 0)
                {
                    CargarPrestamoEnFormulario(oPrestamo);

                    //Mostrar u ocultar botón Descargar según estado
                    btndescargar.Visible = oPrestamo.EstadoPrestamo == "AUTORIZADO";
                }
                else
                {
                    MessageBox.Show("No se encontró ningún préstamo con ese número de documento.",
                        "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    btndescargar.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar préstamo: " + ex.Message,
                    "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                btndescargar.Visible = false;
            }
        }

        private void CargarPrestamoEnFormulario(Prestamo oPrestamo)
        {
            // Cabecera
            txtnumerodocumento.Text = oPrestamo.NumeroDocumento ?? "";
            txtfecha.Text = oPrestamo.FechaPrestamo.ToString("dd/MM/yyyy") ?? "";
            txttipodocumento.Text = oPrestamo.TipoDocumento ?? "";
            txtusuario.Text = oPrestamo.oUsuarioSolicita?.NombreCompleto ?? "";

            txtcodigo.Text = oPrestamo.oFarmacia?.Codigo ?? "";
            txtfarmacia.Text = oPrestamo.oFarmacia?.Nombre ?? "";

            // Detalles
            dgvdata.Rows.Clear();
            if (oPrestamo.oDetalle != null && oPrestamo.oDetalle.Count > 0)
            {
                foreach (Detalle_Prestamo dv in oPrestamo.oDetalle)
                {
                    dgvdata.Rows.Add(new object[]
                    {
                        dv.oEquipo?.Nombre ?? "",
                        dv.oEquipo?.oMarca?.Descripcion ?? "",
                        dv.Cantidad,
                        dv.NumeroSerial ?? ""
                    });
                }
                
                // Primer serial como referencia
                txtnumeroserial.Text = oPrestamo.oDetalle[0].NumeroSerial ?? "";
            }
            else
            {
                txtnumeroserial.Text = "";
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
            txtnumeroserial.Text = "";
            dgvdata.Rows.Clear();
        }

        private void frmodetalleprestamo_Load(object sender, EventArgs e)
        {
            // Obtiene los permisos del usuario logueado
            List<Permiso> listaPermisos = new CN_Permiso().Listar(Inicio.usuarioActual.IdUsuario);

            // Controla visibilidad de los botones según permisos
            btndescargar.Visible = UtilPermisos.TienePermisoAccion(listaPermisos, "submenuverdetalleacta", "btndescargar");

            txtbusqueda.Select();
        }

        private void btnborrar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void btndescargar_Click(object sender, EventArgs e)
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
                        </tr>";
                    }
                }

                // Ruta base a plantilla e imágenes
                string basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Utilidades", "piloto");

                // Leer HTML y reemplazar etiquetas (sin tocar rutas imágenes)
                string htmlContent = File.ReadAllText(Path.Combine(basePath, "prestamo.html"))
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
                string nombreArchivo = $"PRESTAMO_{txtnumerodocumento.Text.Trim().Replace(" ", "_")}.pdf";
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
