using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.Modales;
using CapaPresentacion.Utilidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class frmprestamo : Form
    {
        private CN_Prestamo objcd = new CN_Prestamo();

        private Usuario _Usuario;
        private static Usuario usuarioActual;
        
        public frmprestamo(Usuario oUsuario)
        {
            _Usuario = oUsuario;
            usuarioActual = oUsuario;
            InitializeComponent();
        }

        private void frmprestamo_Load(object sender, EventArgs e)
        {
            // Obtiene los permisos del usuario logueado
            List<Permiso> listaPermisos = new CN_Permiso().Listar(Inicio.usuarioActual.IdUsuario);

            //// Controla visibilidad de los botones según permisos
            //btncrearventa.Visible = UtilPermisos.TienePermisoAccion(listaPermisos, "submenuregistraracta", "btncrearventa");

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
                        txtnumeroserial.Select(); // → Prepararse para que ingrese cantidad
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
                bool producto_existe = false;

                if (int.Parse(txtidequipo.Text) == 0)
                {
                    MessageBox.Show("Debe seleccionar un producto", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtnumeroserial.Text))
                {
                    MessageBox.Show("Debe ingresar el número de serie", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                int stockDisponible = Convert.ToInt32(txtstock.Text);
                int cantidadSolicitada = Convert.ToInt32(txtcantidad.Value);

                // Calcular cuánto ya se ha agregado de ese mismo equipo en el DataGridView
                int cantidadAcumulada = 0;
                foreach (DataGridViewRow fila in dgvdata.Rows)
                {
                    if (fila.Cells["IdEquipo"].Value.ToString() == txtidequipo.Text)
                    {
                        cantidadAcumulada += Convert.ToInt32(fila.Cells["Cantidad"].Value);
                    }
                }

                if (cantidadAcumulada + cantidadSolicitada > stockDisponible)
                {
                    MessageBox.Show("No hay suficiente stock disponible para agregar esta cantidad.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Verificar que no se repita la combinación IdEquipo + Serial + Caja
                foreach (DataGridViewRow fila in dgvdata.Rows)
                {
                    if (
                        fila.Cells["IdEquipo"].Value.ToString() == txtidequipo.Text &&
                        fila.Cells["NumeroSerial"].Value.ToString().Trim().ToUpper() == txtnumeroserial.Text.Trim().ToUpper() 
                    )
                    {
                        producto_existe = true;
                        break;
                    }
                }

                if (producto_existe)
                {
                    MessageBox.Show("Este equipo con el mismo número de serie y caja ya fue agregado.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Agregar al DataGridView sin afectar el stock real aún
                dgvdata.Rows.Add(new object[] {
                    txtidequipo.Text,
                    txtequipo.Text,
                    cantidadSolicitada.ToString(),
                    txtnumeroserial.Text.Trim()
                });

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
            txtnumeroserial.Text = "";
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
                    bool respuesta = new CN_Acta().SumarStock(
                    Convert.ToInt32(dgvdata.Rows[index].Cells["IdEquipo"].Value.ToString()),
                    Convert.ToInt32(dgvdata.Rows[index].Cells["Cantidad"].Value.ToString()));


                    if (respuesta)
                    {
                        dgvdata.Rows.RemoveAt(index);

                    }
                }
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

        private void btncrearventa_Click(object sender, EventArgs e)
        {
            try
            {
                // Validaciones básicas
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
                    MessageBox.Show("Debe ingresar equipos en el préstamo", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                // Construir detalle del préstamo
                DataTable detallePrestamo = new DataTable();
                detallePrestamo.Columns.Add("IdEquipo", typeof(int));
                detallePrestamo.Columns.Add("Cantidad", typeof(int));
                detallePrestamo.Columns.Add("NumeroSerial", typeof(string));
                detallePrestamo.Columns.Add("EstadoDevolucion", typeof(string)); // default "PENDIENTE"

                foreach (DataGridViewRow row in dgvdata.Rows)
                {
                    detallePrestamo.Rows.Add(new object[]
                    {
                        Convert.ToInt32(row.Cells["IdEquipo"].Value),
                        Convert.ToInt32(row.Cells["Cantidad"].Value),
                        row.Cells["NumeroSerial"].Value?.ToString() ?? "",
                        "PENDIENTE"
                    });
                }

                // Obtener IdFarmacia
                CN_Farmacia objCN_Farmacia = new CN_Farmacia();
                int idFarmacia = objCN_Farmacia.ObtenerIdFarmaciaPorCodigo(txtdocumentocliente.Text.Trim());

                if (idFarmacia == 0)
                {
                    MessageBox.Show("Código de farmacia no válido.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Generar número de documento del préstamo
                CN_Prestamo objcn = new CN_Prestamo();
                string numeroDocumento = objcn.GenerarNumeroPrestamo(idFarmacia, txtdocumentocliente.Text.Trim());

                // Crear objeto Prestamo
                Prestamo oPrestamo = new Prestamo()
                {
                    oUsuarioSolicita = new Usuario() { IdUsuario = _Usuario.IdUsuario },
                    IdFarmacia = idFarmacia,
                    TipoDocumento = ((OpcionCombo)cbotipodocumento.SelectedItem).Texto,
                    NumeroDocumento = numeroDocumento
                    //Comentario = txtComentario.Text.Trim()
                };

                
                string mensaje = string.Empty;
                bool respuesta = objcd.RegistrarPrestamo(oPrestamo, detallePrestamo, out mensaje);

                if (respuesta)
                {
                    MessageBox.Show($"Préstamo registrado correctamente.\n\nNúmero de documento: {numeroDocumento}", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    txtdocumentocliente.Text = "";
                    txtnombrecliente.Text = "";
                    //txtComentario.Text = "";
                    dgvdata.Rows.Clear();
                }
                else
                {
                    MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar préstamo: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
