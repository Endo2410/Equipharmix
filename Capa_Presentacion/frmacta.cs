using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.Utilidades;
using CapaPresentacion.Modales;

namespace CapaPresentacion
{
    public partial class frmacta : Form
    {
        private Usuario _Usuario;
        private static Usuario usuarioActual;

        public frmacta(Usuario oUsuario)
        {
            _Usuario = oUsuario;
            usuarioActual = oUsuario;
            InitializeComponent();
        }

        private void frmacta_Load(object sender, EventArgs e)
        {
            // Obtiene los permisos del usuario logueado
            List<Permiso> listaPermisos = new CN_Permiso().Listar(Inicio.usuarioActual.IdUsuario);

            // Controla visibilidad de los botones según permisos
            btncrearventa.Visible = UtilPermisos.TienePermisoAccion(listaPermisos, "submenuregistraracta", "btncrearventa");

            cbotipodocumento.Items.Add(new OpcionCombo() { Valor = "Word", Texto = "Word" });
            cbotipodocumento.Items.Add(new OpcionCombo() { Valor = "Word", Texto = "Word" });
            cbotipodocumento.DisplayMember = "Texto";
            cbotipodocumento.ValueMember = "Valor";
            cbotipodocumento.SelectedIndex = 0;

            txtfecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtidequipo.Text = "0";

        }

        private void btnbuscarframacia_Click(object sender, EventArgs e)
        {
            using (var modal = new mdFarmacia())
            {
                var result = modal.ShowDialog();

                if (result == DialogResult.OK)
                {
                    txtdocumentocliente.Text = modal._Farmacia.Codigo;
                    txtnombrecliente.Text = modal._Farmacia.Nombre;
                    txtcodEquipo.Select();
                }
                else
                {
                    txtdocumentocliente.Select();
                }
            }
        }

        private void btnbuscarequipo_Click(object sender, EventArgs e)
        {
            using (var modal = new mdEquipo())
            {
                var result = modal.ShowDialog();

                if (result == DialogResult.OK)
                {
                    txtidequipo.Text = modal._Equipo.IdEquipo.ToString();
                    txtcodEquipo.Text = modal._Equipo.Codigo;
                    txtequipo.Text = modal._Equipo.Nombre;
                    txtstock.Text = modal._Equipo.Stock.ToString();
                    txtcantidad.Select();
                }
                else
                {
                    txtcodEquipo.Select();
                }
            }
        }

        private void txtcodEquipo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                try
                {
                    string codigo = txtcodEquipo.Text.Trim();
                    Equipo oEquipo = new CN_Equipo().ObtenerPorCodigo(codigo);

                    if (oEquipo != null)
                    {
                        txtcodEquipo.BackColor = Color.Honeydew;
                        txtidequipo.Text = oEquipo.IdEquipo.ToString();
                        txtequipo.Text = oEquipo.Nombre;
                        txtstock.Text = oEquipo.Stock.ToString();
                    }
                    else
                    {
                        txtcodEquipo.BackColor = Color.MistyRose;
                        txtidequipo.Text = "0";
                        txtequipo.Text = "";
                        txtstock.Text = "";
                        txtcantidad.Value = 1;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al buscar el equipo: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnagregarequipo_Click(object sender, EventArgs e)
        {
            try
            {
                if (int.Parse(txtidequipo.Text) == 0)
                {
                    MessageBox.Show("Debe seleccionar un equipo", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtcaja.Text))
                {
                    MessageBox.Show("Debe ingresar la caja", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                int stockDisponible = Convert.ToInt32(txtstock.Text);
                int cantidadSolicitada = Convert.ToInt32(txtcantidad.Value);

                // Calcular cuánto ya se ha agregado de ese mismo equipo en el DataGridView
                int cantidadAcumulada = 0;
                foreach (DataGridViewRow fila in dgvdata.Rows)
                {
                    if (fila.Cells["IdEquipo"].Value.ToString() == txtidequipo.Text)
                        cantidadAcumulada += Convert.ToInt32(fila.Cells["Cantidad"].Value);
                }

                if (cantidadAcumulada + cantidadSolicitada > stockDisponible)
                {
                    MessageBox.Show("No hay suficiente stock disponible para agregar esta cantidad.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Agregar fila al DataGridView con número de serie automático
                for (int i = 0; i < cantidadSolicitada; i++)
                {
                    string numeroSerie = GenerarCodigoSerie(); // ← Aquí se genera automáticamente

                    dgvdata.Rows.Add(new object[]
                    {
                txtidequipo.Text,
                txtequipo.Text,
                1,               // Cada fila 1 unidad
                numeroSerie,     // Serie automática
                txtcaja.Text
                    });
                }

                limpiarProducto();
                txtcodEquipo.Select();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar equipo: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void limpiarProducto()
        {
            txtidequipo.Text = "0";
            txtcodEquipo.Text = "";
            txtequipo.Text = "";
            txtstock.Text = "";
            txtcantidad.Value = 1;
            txtcaja.Text = "";
        }

        private void dgvdata_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (e.ColumnIndex == 5)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                var w = Properties.Resources.delete25.Width;
                var h = Properties.Resources.delete25.Height;
                var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
                var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;

                e.Graphics.DrawImage(Properties.Resources.delete25, new Rectangle(x, y, w, h));
                e.Handled = true;
            }
        }

        private void dgvdata_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvdata.Columns[e.ColumnIndex].Name == "btneliminar")
            {
                int index = e.RowIndex;
                if (index >= 0)
                {
                    dgvdata.Rows.RemoveAt(index);
                }
            }
        }

        private void AgregarOActualizarEquipo(Equipo oEquipo, int cantidad)
        {
            bool productoExiste = false;

            foreach (DataGridViewRow fila in dgvdata.Rows)
            {
                if (fila.Cells["IdEquipo"].Value.ToString() == oEquipo.IdEquipo.ToString())
                {
                    // Si ya existe, aumentar la cantidad (verificando stock)
                    int cantidadActual = Convert.ToInt32(fila.Cells["Cantidad"].Value);
                    int stock = oEquipo.Stock;

                    if (cantidadActual + cantidad <= stock)
                    {
                        fila.Cells["Cantidad"].Value = cantidadActual + cantidad;
                        productoExiste = true;
                        break;
                    }
                    else
                    {
                        MessageBox.Show("No hay suficiente stock para agregar más unidades.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        productoExiste = true;
                        break;
                    }
                }
            }

            if (!productoExiste)
            {
                // Agregar fila nueva solo si hay stock
                if (cantidad <= oEquipo.Stock)
                {
                     dgvdata.Rows.Add(new object[] {
                        oEquipo.IdEquipo,
                        oEquipo.Nombre,
                        cantidad.ToString(),
                        txtcaja.Text // <- El número de serie visible en la tabla
                     });
                }
                else
                {
                    MessageBox.Show("La cantidad supera el stock disponible.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

        }

       
        private string _numeroDocumentoGenerado;

        private void btncrearventa_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtdocumentocliente.Text))
                {
                    MessageBox.Show("Debe ingresar código de la farmacia", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtnombrecliente.Text))
                {
                    MessageBox.Show("Debe ingresar nombre de la farmacia", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                if (dgvdata.Rows.Count < 1)
                {
                    MessageBox.Show("Debe ingresar equipos en la Acta", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                DataTable detalle_acta = new DataTable();
                detalle_acta.Columns.Add("IdEquipo", typeof(int));
                detalle_acta.Columns.Add("Cantidad", typeof(int));
                detalle_acta.Columns.Add("NumeroSerial", typeof(string));
                detalle_acta.Columns.Add("Caja", typeof(string));

                foreach (DataGridViewRow row in dgvdata.Rows)
                {
                    detalle_acta.Rows.Add(new object[]
                    {
                        Convert.ToInt32(row.Cells["IdEquipo"].Value),
                        Convert.ToInt32(row.Cells["Cantidad"].Value),
                        row.Cells["NumeroSerial"].Value?.ToString() ?? "",
                        row.Cells["Caja"].Value?.ToString() ?? ""
                    });
                }

                CN_Farmacia objcn_farmacia = new CN_Farmacia();
                int idFarmacia = objcn_farmacia.ObtenerIdFarmaciaPorCodigo(txtdocumentocliente.Text.Trim());

                if (idFarmacia == 0)
                {
                    MessageBox.Show("Código de farmacia no válido.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                CN_Acta objCN_Acta = new CN_Acta();
                string codigoFarmacia = txtdocumentocliente.Text.Trim();
                string numeroDocumento = objCN_Acta.GenerarNumeroDocumento(idFarmacia, codigoFarmacia);

                Acta oActa = new Acta()
                {
                    oUsuario = new Usuario() { IdUsuario = _Usuario.IdUsuario },
                    TipoDocumento = ((OpcionCombo)cbotipodocumento.SelectedItem).Texto,
                    NumeroDocumento = numeroDocumento,
                    Codigo = codigoFarmacia,
                    Nombre = txtnombrecliente.Text.Trim()
                };

                string mensaje = string.Empty;
                bool respuesta = objCN_Acta.Registrar(oActa, detalle_acta, out mensaje);

                if (respuesta)
                {
                    MessageBox.Show($"Acta generada correctamente y se encuentra en espera.\n\nNúmero de documento: {numeroDocumento}", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    txtdocumentocliente.Text = "";
                    txtnombrecliente.Text = "";
                    dgvdata.Rows.Clear();
                }
                else
                {
                    MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

                _numeroDocumentoGenerado = numeroDocumento;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar acta: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtnumeroserial_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;      // Marcar el evento como manejado
                e.SuppressKeyPress = true; // Evitar que se procese más (no "salta")
            }
        }

        public string GenerarCodigoSerie()
        {
            // FS + año+mes+día+hora+minuto+segundo+milisegundos
            return "FS" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}
