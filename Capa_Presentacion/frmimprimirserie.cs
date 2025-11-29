using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;

namespace CapaPresentacion
{
    public partial class frmimprimirserie : Form
    {
        // Clase para almacenar número de serie y su código de barra
        // Clase para guardar número de serie
        public class SerieParaImprimir
        {
            public string NumeroSerie { get; set; }
            public string Equipo { get; set; }
        }

        private List<SerieParaImprimir> seriesAImprimir = new List<SerieParaImprimir>();
        private int indiceImpresion = 0;

        public frmimprimirserie()
        {
            InitializeComponent();
        }

        private void frmimprimirserie_Load(object sender, EventArgs e)
        {
            dgvSeries.AutoGenerateColumns = false;
            // Hacer editable la columna CheckBox
            dgvSeries.EditMode = DataGridViewEditMode.EditOnEnter;
        }

        private void btnimprimirselecionadas_Click(object sender, EventArgs e)
        {
            List<SerieParaImprimir> seleccionadas = new List<SerieParaImprimir>();

            for (int i = 0; i < dgvSeries.Rows.Count; i++)
            {
                if (Convert.ToBoolean(dgvSeries.Rows[i].Cells["chkSeleccionar"].Value))
                {
                    string numeroSerie = dgvSeries.Rows[i].Cells["NumeroSerie"].Value.ToString();
                    var item = seriesAImprimir.Find(x => x.NumeroSerie == numeroSerie);
                    if (item != null)
                        seleccionadas.Add(item);
                }
            }

            if (seleccionadas.Count == 0)
            {
                MessageBox.Show("Seleccione al menos una serie para imprimir.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ImprimirConDialogo(seleccionadas);
        }


        // Método para enviar a Bixolon
        private void ImprimirConDialogo(List<SerieParaImprimir> lista)
        {
            try
            {
                indiceImpresion = 0;
                PrintDocument pd = new PrintDocument();

                pd.DefaultPageSettings.PaperSize = new PaperSize("Etiqueta", 240, 150); // 60x38mm
                pd.DefaultPageSettings.Margins = new Margins(12, 12, 8, 8);

                pd.PrintPage += (s, e) =>
                {
                    // SI YA SE IMPRIMIERON TODAS
                    if (indiceImpresion >= lista.Count)
                    {
                        e.HasMorePages = false;
                        return;
                    }

                    float x = e.MarginBounds.Left - 5;
                    float y = e.MarginBounds.Top;

                    int ancho = e.MarginBounds.Width;
                    int altoCodigo = 50;

                    // ===== IMPRIMIR SOLO EL CÓDIGO DE BARRAS =====
                    var item = lista[indiceImpresion];

                    BarcodeWriter writer = new BarcodeWriter
                    {
                        Format = BarcodeFormat.CODE_128,
                        Options = new ZXing.Common.EncodingOptions
                        {
                            Height = altoCodigo,
                            Width = (int)(ancho * 1.4),
                            Margin = 0
                        }
                    };

                    using (Bitmap bmp = writer.Write(item.NumeroSerie))
                    {
                        e.Graphics.DrawImage(bmp, new Rectangle((int)x, (int)y, ancho, altoCodigo));
                    }

                    // SIGUIENTE SERIE
                    indiceImpresion++;

                    // SI AÚN QUEDAN, SE PIDE OTRA PÁGINA
                    e.HasMorePages = (indiceImpresion < lista.Count);
                };

                PrintDialog dlg = new PrintDialog { Document = pd };
                if (dlg.ShowDialog() == DialogResult.OK)
                    pd.Print();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnborrar_Click(object sender, EventArgs e)
        {
            txtnumerodocumento.Text = "";
            dgvSeries.Rows.Clear();
            seriesAImprimir.Clear();
        }

        private void btnbuscar_Click(object sender, EventArgs e)
        {
            dgvSeries.Rows.Clear();
            seriesAImprimir.Clear();

            string numeroDocumento = txtnumerodocumento.Text.Trim();
            if (string.IsNullOrEmpty(numeroDocumento))
            {
                MessageBox.Show("Ingrese un número de documento", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                CN_Acta cnActa = new CN_Acta();
                Acta oActa = cnActa.ObtenerActa(numeroDocumento);

                if (oActa == null || oActa.IdActa == 0)
                {
                    MessageBox.Show("No se encontró el documento.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (oActa.EstadoAutorizacion?.Trim().ToUpper() != "AUTORIZADO")
                {
                    MessageBox.Show("El documento no está autorizado para imprimir.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                foreach (var detalle in oActa.oDetalle_Acta)
                {
                    int rowIndex = dgvSeries.Rows.Add();
                    DataGridViewRow row = dgvSeries.Rows[rowIndex];
                    row.Cells["chkSeleccionar"].Value = false;
                    row.Cells["NumeroSerie"].Value = detalle.NumeroSerial;
                    row.Cells["Equipo"].Value = detalle.oEquipo.Nombre;

                    seriesAImprimir.Add(new SerieParaImprimir
                    {
                        NumeroSerie = detalle.NumeroSerial,
                        Equipo = detalle.oEquipo.Nombre
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar las series: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnimprimirtodo_Click(object sender, EventArgs e)
        {
            if (seriesAImprimir.Count == 0)
            {
                MessageBox.Show("No hay series para imprimir.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ImprimirConDialogo(seriesAImprimir);
        }
    }
}
