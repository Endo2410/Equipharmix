using CapaEntidad;
using CapaNegocio;
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
    public partial class frmlistarprestamo : Form
    {
        private CN_Prestamo objcn_Prestamo = new CN_Prestamo();
        private Usuario _UsuarioActual;

        public frmlistarprestamo()
        {
            InitializeComponent();
        }

        private void frmlistarprestamo_Load(object sender, EventArgs e)
        {
            try
            {
                _UsuarioActual = Inicio.usuarioActual;

                dgvdata.Rows.Clear();

                // Configurar ComboBox de búsqueda
                cbobusqueda.Items.Clear();
                foreach (DataGridViewColumn columna in dgvdata.Columns)
                {
                    if (columna.Visible && columna.Name != "btnseleccionar")
                    {
                        cbobusqueda.Items.Add(new OpcionCombo() { Valor = columna.Name, Texto = columna.HeaderText });
                    }
                }
                cbobusqueda.DisplayMember = "Texto";
                cbobusqueda.ValueMember = "Valor";
                cbobusqueda.SelectedIndex = 0;

                // Listar préstamos autorizados
                List<Prestamo> listaPrestamos = objcn_Prestamo.ListarPrestamosAutorizados();

                foreach (Prestamo p in listaPrestamos)
                {
                    foreach (Detalle_Prestamo dp in p.oDetalle)
                    {
                        // Filtrar: no mostrar los que ya están autorizados
                        if (!string.IsNullOrEmpty(dp.EstadoBaja) &&
                            dp.EstadoBaja.Equals("AUTORIZADO", StringComparison.OrdinalIgnoreCase))
                        {
                            continue; 
                        }


                        dgvdata.Rows.Add(new object[] {
                            "",                 // btnseleccionar
                            p.IdPrestamo,       // Id
                            p.NumeroDocumento,
                            p.FechaPrestamo.ToString("dd/MM/yyyy"),
                            p.oFarmacia?.Nombre ?? "N/A",
                            dp.oEquipo.IdEquipo,
                            dp.oEquipo.Codigo,
                            dp.oEquipo.Nombre,
                            dp.oEquipo.oMarca.Descripcion,
                            dp.Cantidad,
                            dp.NumeroSerial,
                            p.oUsuario.NombreCompleto,
                            dp.MotivoBaja,
                            dp.EstadoBaja
                        });

                        // Pintar amarillo si está en espera
                        int rowIndex = dgvdata.Rows.Count - 1;
                        if (!string.IsNullOrEmpty(dp.EstadoBaja) &&
                            dp.EstadoBaja.Equals("EN ESPERA", StringComparison.OrdinalIgnoreCase))
                        {
                            dgvdata.Rows[rowIndex].DefaultCellStyle.BackColor = Color.Yellow;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar préstamos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvdata_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;

                if (e.ColumnIndex == 0)
                {
                    e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                    var w = Properties.Resources.check20.Width;
                    var h = Properties.Resources.check20.Height;
                    var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
                    var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;

                    e.Graphics.DrawImage(Properties.Resources.check20, new Rectangle(x, y, w, h));
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en la pintura de celda: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void dgvdata_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvdata.Columns[e.ColumnIndex].Name == "btnseleccionar" && e.RowIndex >= 0)
                {
                    DataGridViewRow row = dgvdata.Rows[e.RowIndex];

                    txtindice.Text = e.RowIndex.ToString();
                    txtid.Text = row.Cells["Id"].Value?.ToString() ?? "0";
                    txtdocumento.Text = row.Cells["NumeroDocumento"].Value?.ToString();
                    txtfecha.Text = row.Cells["FechaPrestamo"].Value?.ToString();
                    txtfarmacia.Text = row.Cells["NombreFarmacia"].Value?.ToString();
                    txtcodigo.Text = row.Cells["CodigoEquipo"].Value?.ToString();
                    txtequipo.Text = row.Cells["NombreEquipo"].Value?.ToString();
                    txtmarca.Text = row.Cells["MarcaEquipo"].Value?.ToString();
                    txtcantidad.Text = row.Cells["Cantidad"].Value?.ToString();
                    txtserial.Text = row.Cells["NumeroSerial"].Value?.ToString();
                    txtusuario.Text = row.Cells["NombreCompleto"].Value?.ToString();

                    string motivoBaja = row.Cells["MotivoBaja"].Value?.ToString();
                    string estadoBaja = row.Cells["EstadoBaja"].Value?.ToString();
                    

                    if (!string.IsNullOrEmpty(estadoBaja) &&
                        estadoBaja.Equals("EN ESPERA", StringComparison.OrdinalIgnoreCase))
                    {
                        lblmotivo.Visible = true;
                        txtmotivo.Visible = true;
                        txtmotivo.Text = motivoBaja;
                        btnguardarmotivo.Visible = true;
                    }
                    else
                    {
                        lblmotivo.Visible = false;
                        txtmotivo.Visible = false;
                        txtmotivo.Text = "";
                        btnguardarmotivo.Visible = false;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al seleccionar fila: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Limpiar()
        {
            try
            {
                txtindice.Text = "-1";
                txtid.Text = "0";
                txtdocumento.Text = "";
                txtfecha.Text = "";
                txtfarmacia.Text = "";
                txtcodigo.Text = "";
                txtequipo.Text = "";
                txtmarca.Text = "";
                txtcantidad.Text = "";
                txtserial.Text = "";
                txtusuario.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al limpiar campos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnbuscar_Click(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnlimpiarbuscador_Click(object sender, EventArgs e)
        {
            try
            {
                txtbusqueda.Text = "";
                foreach (DataGridViewRow row in dgvdata.Rows)
                {
                    row.Visible = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al limpiar búsqueda: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnlimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void btndevolver_Click(object sender, EventArgs e)
        {
            string numeroDocumento = txtdocumento.Text.Trim();
            string codigoEquipo = txtcodigo.Text.Trim();
            string numeroSerial = txtserial.Text.Trim();

            if (string.IsNullOrEmpty(numeroDocumento) ||
                string.IsNullOrEmpty(codigoEquipo) ||
                string.IsNullOrEmpty(numeroSerial))
            {
                MessageBox.Show("Debe seleccionar un préstamo válido antes de devolver.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idUsuario = _UsuarioActual.IdUsuario;

            string mensaje;
            bool resultado = objcn_Prestamo.DevolverEquipoPrestamo(
                                numeroDocumento,
                                codigoEquipo,
                                numeroSerial,
                                idUsuario,
                                out mensaje);

            if (resultado)
            {
                MessageBox.Show(mensaje, "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Limpiar();
                frmlistarprestamo_Load(null, null);
            }
            else
            {
                MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btneliminar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtcodigo.Text))
            {
                MessageBox.Show("Seleccione un equipo del préstamo para dar de baja.");
                return;
            }

            lblmotivo.Visible = true;
            txtmotivo.Visible = true;
            btnguardarmotivo.Visible = true;
        }

        private void btnguardarmotivo_Click(object sender, EventArgs e)
        {
            try
            {
                string motivo = txtmotivo.Text.Trim();

                if (string.IsNullOrEmpty(motivo))
                {
                    MessageBox.Show("Debe ingresar el motivo de baja.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string documento = txtdocumento.Text;
                string codigoEquipo = txtcodigo.Text;
                string numeroSerial = txtserial.Text;

                int idUsuarioSolicita = _UsuarioActual.IdUsuario;

                // Declarar variable para recibir mensaje del método
                string mensaje;

                // Llamar al método de negocio con out string mensaje
                bool resultado = objcn_Prestamo.MarcarPrestamoEquipoComoEnEspera(documento, codigoEquipo, numeroSerial, motivo, idUsuarioSolicita, out mensaje);

                if (resultado)
                {
                    MessageBox.Show(mensaje, "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Actualizar la fila visualmente
                    if (int.TryParse(txtindice.Text, out int indice) && indice >= 0 && indice < dgvdata.Rows.Count)
                    {
                        DataGridViewRow fila = dgvdata.Rows[indice];
                        fila.Cells["MotivoBaja"].Value = motivo;
                        fila.Cells["EstadoBaja"].Value = "EN ESPERA";

                        // Pintar fila de amarillo y forzar refresco
                        fila.DefaultCellStyle.BackColor = Color.Yellow;
                        dgvdata.InvalidateRow(indice);
                    }

                    // Ocultar y limpiar controles
                    txtmotivo.Clear();
                    txtmotivo.Visible = false;
                    lblmotivo.Visible = false;
                    btnguardarmotivo.Visible = false;
                }
                else
                {
                    // Mostrar mensaje de error
                    MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al guardar el motivo:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvdata_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            var row = dgvdata.Rows[e.RowIndex];
            string estadoBaja = row.Cells["EstadoBaja"].Value?.ToString().Trim().ToUpper();

            if (estadoBaja == "EN ESPERA")
            {
                row.DefaultCellStyle.BackColor = Color.Yellow;
            }
            else
            {
                row.DefaultCellStyle.BackColor = Color.White; // Color normal de la fila
            }
        }
    }
}

