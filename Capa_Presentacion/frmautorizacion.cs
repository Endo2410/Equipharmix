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
    public partial class frmautorizacion : Form
    {

        private static Usuario usuarioActual;
        private CN_Acta objCN_Acta = new CN_Acta();
        private CN_Prestamo objCN_Prestamo = new CN_Prestamo();


        public frmautorizacion(Usuario objusuario)
        {
            usuarioActual = objusuario;
            InitializeComponent();
        }

       

        private void frmautorizacion_Load(object sender, EventArgs e)
        {
            // Obtiene los permisos del usuario logueado
            List<Permiso> listaPermisos = new CN_Permiso().Listar(Inicio.usuarioActual.IdUsuario);

            // Controla visibilidad de los botones según permisos
            btnguardar.Visible = UtilPermisos.TienePermisoAccion(listaPermisos, "submenuautorizacionbaja", "btnguardar");
            btnlimpiar.Visible = UtilPermisos.TienePermisoAccion(listaPermisos, "submenuautorizacionbaja", "btnlimpiar");
            btneliminar.Visible = UtilPermisos.TienePermisoAccion(listaPermisos, "submenuautorizacionbaja", "btneliminar");

            try
            {
                dgvdata.AutoGenerateColumns = false;
                dgvdata.Rows.Clear();


                CargarEquiposEnEspera();


                // Llenar ComboBox búsqueda
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
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar:\n" + ex.Message);
            }

        }

        private void CargarEquiposEnEspera()
        {
            dgvdata.Rows.Clear();

            // 1️⃣ Equipos en espera de ACTA
            var listaActa = objCN_Acta.ObtenerEquiposEnEspera();
            foreach (var item in listaActa)
            {
                dgvdata.Rows.Add(
                    "", // Imagen seleccionar
                    item.NumeroDocumento,
                    item.FechaRegistro.ToString("dd/MM/yyyy"),
                    item.oFarmacia?.Nombre ?? "",
                    item.oEquipo.Codigo,
                    item.oEquipo.Nombre,
                    item.oEquipo.oMarca?.Descripcion ?? "",
                    item.Cantidad,
                    item.NumeroSerial,
                    item.Caja,                     // Siempre tiene caja en acta
                    item.MotivoBaja,
                    item.EstadoBaja,
                    item.oUsuarioSolicitante?.NombreCompleto ?? "",
                    "ACTA"                         // Identificador
                );

                var fila = dgvdata.Rows[dgvdata.Rows.Count - 1];
                if (item.EstadoBaja == "En espera")
                    fila.DefaultCellStyle.BackColor = Color.White;
            }

            // 2️⃣ Equipos en espera de PRÉSTAMO
            var listaPrestamo = objCN_Prestamo.ObtenerEquiposPrestamoEnEspera();
            foreach (var item in listaPrestamo)
            {
                dgvdata.Rows.Add(
                    "", // Imagen seleccionar
                    item.NumeroDocumento,
                    item.FechaPrestamo.ToString("dd/MM/yyyy"), // Fecha de préstamo
                    item.oFarmacia?.Nombre ?? "",
                    item.oEquipo.Codigo,
                    item.oEquipo.Nombre,
                    item.oEquipo.oMarca?.Descripcion ?? "",
                    item.Cantidad,
                    item.NumeroSerial,
                    "", // NO hay caja en préstamo
                    item.MotivoBaja,
                    item.EstadoBaja,
                    item.oUsuarioSolicitante?.NombreCompleto ?? "",
                    "PRESTAMO"                     // Identificador
                );

                var fila = dgvdata.Rows[dgvdata.Rows.Count - 1];
                if (item.EstadoBaja == "En espera")
                    fila.DefaultCellStyle.BackColor = Color.LightYellow; 
            }
        }


        private void btnguardar_Click(object sender, EventArgs e)
        {
            try
            {
                string numeroDocumento = txtdocumento.Text.Trim();
                string codigoEquipo = txtcodigoequipo.Text.Trim();
                string numeroSerial = txtserial.Text.Trim();
                string tipo = dgvdata.Rows[int.Parse(txtindice.Text)].Cells["Tipo"].Value?.ToString() ?? "ACTA"; // Tipo: ACTA o PRESTAMO

                if (string.IsNullOrEmpty(numeroDocumento) || string.IsNullOrEmpty(codigoEquipo) || string.IsNullOrEmpty(numeroSerial))
                {
                    MessageBox.Show("Debe seleccionar un equipo válido antes de autorizar.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult result = MessageBox.Show("¿Está seguro que desea autorizar este equipo?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    int idUsuarioActual = usuarioActual.IdUsuario;
                    bool exito = false;

                    if (tipo == "ACTA")
                        exito = objCN_Acta.AutorizarBaja(numeroDocumento, codigoEquipo, numeroSerial, idUsuarioActual);
                    else if (tipo == "PRESTAMO")
                        exito = objCN_Prestamo.AutorizarBaja(numeroDocumento, codigoEquipo, numeroSerial, idUsuarioActual);

                    if (exito)
                    {
                        MessageBox.Show("Equipo autorizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarEquiposEnEspera();
                        LimpiarCampos();
                    }
                    else
                    {
                        MessageBox.Show("Error al autorizar equipo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error inesperado:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarCampos()
        {
            txtindice.Text = "";
            txtdocumento.Text = "";
            txtfecha.Text = "";
            txtfarmacia.Text = "";
            txtcodigoequipo.Text = "";
            txtequipo.Text = "";
            txtmarca.Text = "";
            txtcantidad.Text = "";
            txtserial.Text = "";
            txtcaja.Text = "";
            txtsolicitante.Text = "";
            txtmotivo.Text = "";
            txtestadobaja.Text = "";
        }

        private void btneliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvdata.CurrentRow == null)
                {
                    MessageBox.Show("Debe seleccionar un equipo para eliminar.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DataGridViewRow fila = dgvdata.CurrentRow;

                string numeroDocumento = fila.Cells["NumeroDocumento"].Value?.ToString() ?? "";
                string codigoEquipo = fila.Cells["CodigoEquipo"].Value?.ToString() ?? "";
                string numeroSerial = fila.Cells["NumeroSerial"].Value?.ToString() ?? "";
                string tipo = fila.Cells["Tipo"].Value?.ToString() ?? ""; // Columna que identifica ACTA o PRESTAMO

                if (string.IsNullOrEmpty(numeroDocumento) || string.IsNullOrEmpty(codigoEquipo) || string.IsNullOrEmpty(numeroSerial))
                {
                    MessageBox.Show("No se pudo obtener la información del equipo seleccionado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DialogResult result = MessageBox.Show("¿Está seguro que deseas rechazar la baja?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    string mensaje = ""; // Inicializar para evitar error de variable no asignada
                    bool exito = false;

                    if (tipo == "ACTA")
                    {
                        exito = objCN_Acta.LimpiarMotivoYEstado(numeroDocumento, codigoEquipo, numeroSerial, out mensaje);
                    }
                    else if (tipo == "PRESTAMO")
                    {
                        exito = objCN_Prestamo.LimpiarMotivoYEstado(numeroDocumento, codigoEquipo, numeroSerial, out mensaje);
                    }

                    if (exito)
                    {
                        MessageBox.Show("Baja rechazada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarEquiposEnEspera(); // Recargar la lista
                        LimpiarCampos();
                    }
                    else
                    {
                        MessageBox.Show("No se pudo rechazar la baja:\n" + mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error inesperado:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnlimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
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
                        if (row.Cells[columnaFiltro].Value != null &&
                            row.Cells[columnaFiltro].Value.ToString().Trim().ToUpper().Contains(txtbusqueda.Text.Trim().ToUpper()))
                        {
                            row.Visible = true;
                        }
                        else
                        {
                            row.Visible = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en la búsqueda:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void dgvdata_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;

                if (e.ColumnIndex == dgvdata.Columns["btnseleccionar"].Index)
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
            catch { }
        }

        private void dgvdata_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && dgvdata.Columns[e.ColumnIndex].Name == "btnseleccionar")
                {
                    DataGridViewRow fila = dgvdata.Rows[e.RowIndex];

                    txtindice.Text = e.RowIndex.ToString();
                    txtdocumento.Text = fila.Cells["NumeroDocumento"].Value?.ToString() ?? "";
                    txtfecha.Text = fila.Cells["FechaRegistro"].Value?.ToString() ?? "";
                    txtfarmacia.Text = fila.Cells["NombreFarmacia"].Value?.ToString() ?? "";
                    txtcodigoequipo.Text = fila.Cells["CodigoEquipo"].Value?.ToString() ?? "";
                    txtequipo.Text = fila.Cells["NombreEquipo"].Value?.ToString() ?? "";
                    txtmarca.Text = fila.Cells["Marca"].Value?.ToString() ?? "";
                    txtcantidad.Text = fila.Cells["Cantidad"].Value?.ToString() ?? "";
                    txtserial.Text = fila.Cells["NumeroSerial"].Value?.ToString() ?? "";
                    txtcaja.Text = fila.Cells["Caja"].Value?.ToString() ?? "";
                    txtmotivo.Text = fila.Cells["MotivoBaja"].Value?.ToString() ?? "";
                    txtestadobaja.Text = fila.Cells["EstadoBaja"].Value?.ToString() ?? "";
                    txtsolicitante.Text = fila.Cells["UsuarioSolicitante"].Value?.ToString() ?? "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al seleccionar el equipo:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
