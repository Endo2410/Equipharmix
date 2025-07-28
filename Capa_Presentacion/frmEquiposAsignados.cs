using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.Utilidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClosedXML.Excel;

namespace CapaPresentacion
{
    public partial class frmEquiposAsignados : Form
    {

        private static Usuario usuarioActual;
        private CN_Acta objCN_Acta = new CN_Acta();

        public frmEquiposAsignados(Usuario objusuario)
        {
            usuarioActual = objusuario;
            InitializeComponent();
        }

        private void frmEquiposAsignados_Load(object sender, EventArgs e)
        {
            try
            {
                if (dgvdata.Columns.Count == 0)
                {
                    // Botón seleccionar
                    DataGridViewImageColumn btnSeleccionar = new DataGridViewImageColumn();
                    btnSeleccionar.Name = "btnseleccionar";
                    btnSeleccionar.HeaderText = "";
                    btnSeleccionar.Image = Properties.Resources.check20;
                    btnSeleccionar.ImageLayout = DataGridViewImageCellLayout.Zoom;
                    btnSeleccionar.Width = 30;
                    btnSeleccionar.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dgvdata.Columns.Add(btnSeleccionar);

                    // Columnas de datos visibles
                    dgvdata.Columns.Add("NumeroDocumento", "Documento");
                    dgvdata.Columns.Add("CodigoEquipo", "Código");
                    dgvdata.Columns.Add("NombreEquipo", "Nombre");
                    dgvdata.Columns.Add("Marca", "Marca");
                    dgvdata.Columns.Add("Estado", "Estado");
                    dgvdata.Columns.Add("NumeroSerial", "Serial");
                    dgvdata.Columns.Add("Caja", "Caja");
                    dgvdata.Columns.Add("Cantidad", "Cantidad");
                    dgvdata.Columns.Add("FechaRegistro", "Fecha");

                    // Columnas ocultas técnicas
                    dgvdata.Columns.Add("IdEstado", "IdEstado");
                    dgvdata.Columns["IdEstado"].Visible = false;

                    dgvdata.Columns.Add("MotivoBaja", "Motivo Baja");
                    dgvdata.Columns["MotivoBaja"].Visible = false;

                    dgvdata.Columns.Add("EstadoBaja", "Estado Baja");
                    dgvdata.Columns["EstadoBaja"].Visible = false;

                    // Columna margen decorativa
                    DataGridViewTextBoxColumn colMargen = new DataGridViewTextBoxColumn();
                    colMargen.Name = "colMargen";
                    colMargen.HeaderText = "";
                    colMargen.ReadOnly = true;
                    colMargen.Width = 30;
                    colMargen.SortMode = DataGridViewColumnSortMode.NotSortable;
                    colMargen.DefaultCellStyle.BackColor = dgvdata.BackgroundColor;
                    colMargen.DefaultCellStyle.SelectionBackColor = dgvdata.BackgroundColor;
                    colMargen.DefaultCellStyle.ForeColor = dgvdata.BackgroundColor;
                    colMargen.DefaultCellStyle.SelectionForeColor = dgvdata.BackgroundColor;
                    dgvdata.Columns.Add(colMargen);

                    // Ajustar modos de tamaño por columna
                    foreach (DataGridViewColumn col in dgvdata.Columns)
                    {
                        if (col.Name == "NombreEquipo")
                        {
                            col.Width = 150;
                            col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                        }
                        else if (col.Name != "btnseleccionar" && col.Name != "colMargen")
                        {
                            col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        }
                        else
                        {
                            col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                        }
                    }
                }

                dgvdata.CellContentClick += dgvdata_CellContentClick;

                // Ocultar controles de motivo
                lblmotivo.Visible = false;
                txtmotivo.Visible = false;
                btnguardarmotivo.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnbuscar_Click(object sender, EventArgs e)
        {
            string codigo = txtCodigoFarmacia.Text.Trim();

            if (string.IsNullOrEmpty(codigo))
            {
                MessageBox.Show("Por favor ingrese un código de farmacia.");
                return;
            }

            List<Detalle_Acta> lista = objCN_Acta.ObtenerEquiposPorFarmacia(codigo);

            if (lista == null || lista.Count == 0)
            {
                MessageBox.Show("No se encontraron registros para esa farmacia.");
                dgvdata.Rows.Clear();
                return;
            }

            dgvdata.Rows.Clear();

            foreach (var detalle in lista)
            {
                // Ya el filtro en BD no trae Estado 'PENDIENTE' ni EstadoBaja 'Autorizado',
                // pero por seguridad aquí podrías evitar mostrarlos si quieres (opcional):
                if (!string.IsNullOrEmpty(detalle.EstadoBaja) && detalle.EstadoBaja.Equals("Autorizado", StringComparison.OrdinalIgnoreCase))
                    continue;

                if (!string.IsNullOrEmpty(detalle.oEquipo.oEstado.Descripcion) && detalle.oEquipo.oEstado.Descripcion.Equals("PENDIENTE", StringComparison.OrdinalIgnoreCase))
                    continue;

                dgvdata.Rows.Add(
                    Properties.Resources.check20,
                    detalle.NumeroDocumento,
                    detalle.oEquipo.Codigo,
                    detalle.oEquipo.Nombre,
                    detalle.oEquipo.oMarca.Descripcion,
                    detalle.oEquipo.oEstado.Descripcion,
                    detalle.NumeroSerial,
                    detalle.Caja,
                    detalle.Cantidad,
                    detalle.FechaRegistro.ToShortDateString(),
                    detalle.oEquipo.oEstado.IdEstado,
                    detalle.MotivoBaja,
                    detalle.EstadoBaja,
                    "" // ← colMargen
                );
            }
            

        }

        private void btnlimpiarbuscador_Click(object sender, EventArgs e)
        {
            txtCodigoFarmacia.Text = "";
            foreach (DataGridViewRow row in dgvdata.Rows)
            {
                row.Visible = true;
            }
            dgvdata.Refresh();
        }

     

        private void dgvdata_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvdata.Columns[e.ColumnIndex].Name == "btnseleccionar")
            {
                DataGridViewRow fila = dgvdata.Rows[e.RowIndex];

                txtindice.Text = e.RowIndex.ToString();
                txtdocumento.Text = fila.Cells["NumeroDocumento"].Value?.ToString() ?? "";
                txtcodigo.Text = fila.Cells["CodigoEquipo"].Value?.ToString() ?? "";
                txtnombre.Text = fila.Cells["NombreEquipo"].Value?.ToString() ?? "";
                txtmarca.Text = fila.Cells["Marca"].Value?.ToString() ?? "";
                txtserie.Text = fila.Cells["NumeroSerial"].Value?.ToString() ?? "";
                txtcaja.Text = fila.Cells["Caja"].Value?.ToString() ?? "";
                txtcantidad.Text = fila.Cells["Cantidad"].Value?.ToString() ?? "";
                txtfecha.Text = fila.Cells["FechaRegistro"].Value?.ToString() ?? "";
                txtestado.Text = fila.Cells["Estado"].Value?.ToString() ?? "";

                // Obtener Estado de Baja
                string estadoBaja = fila.Cells["EstadoBaja"].Value?.ToString();
                string motivoBaja = fila.Cells["MotivoBaja"].Value?.ToString();

                if (!string.IsNullOrEmpty(estadoBaja) && estadoBaja.Equals("En espera", StringComparison.OrdinalIgnoreCase))
                {
                    // Mostrar controles y cargar motivo
                    lblmotivo.Visible = true;
                    txtmotivo.Visible = true;
                    txtmotivo.Text = motivoBaja;
                }
                else
                {
                    // Ocultar controles si no está en espera
                    lblmotivo.Visible = false;
                    txtmotivo.Visible = false;
                    btnguardarmotivo.Visible = false;
                    txtmotivo.Clear();
                }
            }
        }

        private void VerificarVisibilidadMotivo()
        {
            bool tieneTexto = !string.IsNullOrWhiteSpace(txtmotivo.Text);
            lblmotivo.Visible = tieneTexto;
            txtmotivo.Visible = true; // Siempre visible si quieres permitir editar
            btnguardarmotivo.Visible = tieneTexto;
        } 

        private void btneliminar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtcodigo.Text))
            {
                MessageBox.Show("Seleccione un equipo para dar de baja.");
                return;
            }

            lblmotivo.Visible = true;
            txtmotivo.Visible = true;
            btnguardarmotivo.Visible = true;
        }

        private void btnguardarmotivo_Click(object sender, EventArgs e)
        {
            string motivo = txtmotivo.Text.Trim();

            if (string.IsNullOrEmpty(motivo))
            {
                MessageBox.Show("Debe ingresar el motivo de baja.");
                return;
            }

            string documento = txtdocumento.Text;
            string codigoEquipo = txtcodigo.Text;
            string numeroSerial = txtserie.Text;

            // Obtén el Id del usuario activo. 
            // Debes tener una variable global o sesión que almacene el usuario logueado
            int idUsuarioSolicita = usuarioActual.IdUsuario;  // Ejemplo


            //// Obtener el ID del usuario logueado, por ejemplo:
            //int idUsuarioActual = usuarioActual.IdUsuario; // <-- Ajusta esta línea según cómo guardes el usuario

            bool resultado = objCN_Acta.MarcarEquipoComoEnEspera(documento, codigoEquipo, numeroSerial, motivo, idUsuarioSolicita);

            if (resultado)
            {
                MessageBox.Show("El motivo fue guardado correctamente y el equipo está marcado como 'En espera'.");

                // Actualizar la fila visualmente
                int indice = int.Parse(txtindice.Text);
                DataGridViewRow fila = dgvdata.Rows[indice];
                fila.Cells["MotivoBaja"].Value = motivo;
                fila.Cells["EstadoBaja"].Value = "En espera";
                fila.DefaultCellStyle.BackColor = Color.Yellow;

                // Ocultar controles
                txtmotivo.Clear();
                txtmotivo.Visible = false;
                lblmotivo.Visible = false;
                btnguardarmotivo.Visible = false;
            }
            else
            {
                MessageBox.Show("Ocurrió un error al guardar el motivo.");
            }
        }

        private void dgvdata_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            var fila = dgvdata.Rows[e.RowIndex];
            string estadoBaja = fila.Cells["EstadoBaja"].Value?.ToString();

            if (!string.IsNullOrEmpty(estadoBaja) && estadoBaja.Equals("En Espera", StringComparison.OrdinalIgnoreCase))
            {
                fila.DefaultCellStyle.BackColor = Color.Yellow;
            }
        }

        private void LimpiarCamposEquipo()
        {
            txtindice.Clear();
            txtdocumento.Clear();
            txtcodigo.Clear();
            txtnombre.Clear();
            txtserie.Clear();
            txtcaja.Clear();
            txtcantidad.Clear();
            txtfecha.Clear();
            txtestado.Clear();
            txtmotivo.Clear();

            lblmotivo.Visible = false;
            txtmotivo.Visible = false;
            btnguardarmotivo.Visible = false;
        }


        private void btnlimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCamposEquipo();
        }

        private void btnexportar_Click(object sender, EventArgs e)
        {
            if (dgvdata.Rows.Count < 1)
            {
                MessageBox.Show("No hay datos para exportar", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            DataTable dt = new DataTable();

            // Agregar columnas visibles (excepto botón de selección)
            foreach (DataGridViewColumn columna in dgvdata.Columns)
            {
                if (columna.Visible && columna.Name != "btnseleccionar")
                {
                    dt.Columns.Add(columna.HeaderText, typeof(string));
                }
            }

            // Agregar filas visibles
            foreach (DataGridViewRow row in dgvdata.Rows)
            {
                if (row.Visible && !row.IsNewRow)
                {
                    List<string> fila = new List<string>();

                    foreach (DataGridViewColumn columna in dgvdata.Columns)
                    {
                        if (columna.Visible && columna.Name != "btnseleccionar")
                        {
                            string valor = row.Cells[columna.Index].Value?.ToString() ?? "";
                            fila.Add(valor);
                        }
                    }

                    dt.Rows.Add(fila.ToArray());
                }
            }

            SaveFileDialog savefile = new SaveFileDialog();
            savefile.FileName = $"Equipos_Asignados_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
            savefile.Filter = "Excel Files|*.xlsx";

            if (savefile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        var hoja = wb.Worksheets.Add(dt, "Equipos Asignados");
                        hoja.ColumnsUsed().AdjustToContents();
                        wb.SaveAs(savefile.FileName);
                    }

                    MessageBox.Show("Reporte exportado correctamente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Abrir archivo
                    if (MessageBox.Show("¿Desea abrir el archivo ahora?", "Abrir archivo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(savefile.FileName);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al exportar:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btncaja_Click(object sender, EventArgs e)
        {
            try
            {
                string filtroCaja = txtcajabuscar.Text.Trim().ToLower();

                foreach (DataGridViewRow row in dgvdata.Rows)
                {
                    if (row.IsNewRow) continue;

                    string valorCaja = row.Cells["Caja"]?.Value?.ToString().ToLower() ?? "";

                    row.Visible = valorCaja.Contains(filtroCaja);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al filtrar por caja: " + ex.Message);
            }
        }

        private void limpiarcaja_Click(object sender, EventArgs e)
        {
            txtcajabuscar.Text = "";

            foreach (DataGridViewRow row in dgvdata.Rows)
            {
                row.Visible = true;
            }
        }
    }
}

