using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentacion.Modales
{
    public partial class mdActa : Form
    {
        private string numeroDocumento;
        private int idUsuario;
        private CN_Acta objCN_Acta = new CN_Acta();

        public mdActa(string numeroDocumento, int idUsuario)
        {
            this.numeroDocumento = numeroDocumento;
            this.idUsuario = idUsuario;

            InitializeComponent();
            
        }

        private void mdActa_Load(object sender, EventArgs e)
        {

            //  alinear el CheckBox al centro
            dgvEquipo.Columns["chk"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


            CargarEquiposDelDocumento();
        }

        private void CargarEquiposDelDocumento()
        {
            dgvEquipo.Rows.Clear();

            // Lista combinada de Acta y Prestamo
            List<dynamic> lista = new List<dynamic>();

            try
            {
                // Traer equipos de acta si existen
                List<Detalle_Acta> listaActa = objCN_Acta.ObtenerEquiposPorDocumento(numeroDocumento);
                if (listaActa != null)
                {
                    foreach (var item in listaActa)
                        lista.Add(new { Movimiento = "Acta", Item = item });
                }

                // Traer equipos de préstamo si existen
                CN_Prestamo objCN_Prestamo = new CN_Prestamo();
                List<Detalle_Prestamo> listaPrestamo = objCN_Prestamo.ObtenerEquiposPorDocumento(numeroDocumento);
                if (listaPrestamo != null)
                {
                    foreach (var item in listaPrestamo)
                        lista.Add(new { Movimiento = "Prestamo", Item = item });
                }

                // Si todos los registros son préstamos, ocultar columna Caja
                bool tieneActa = lista.Any(r => r.Movimiento == "Acta");
                dgvEquipo.Columns["Caja"].Visible = tieneActa;

                // Cargar filas en la grilla
                foreach (var registro in lista)
                {
                    dynamic item = registro.Item;
                    string tipo = registro.Movimiento;

                    var equipo = item.oEquipo;
                    var marca = equipo?.oMarca;
                    var estado = equipo?.oEstado;

                    if (equipo == null || marca == null || estado == null)
                        continue;

                    dgvEquipo.Rows.Add(
                        false,
                        equipo.Codigo ?? "",
                        equipo.Nombre ?? "",
                        marca.Descripcion ?? "",
                        estado.Descripcion ?? "",
                        item.Cantidad,
                        item.NumeroSerial ?? "",
                        tipo == "Acta" ? item.Caja ?? "" : "", // Solo asigna Caja si es Acta
                        tipo  // <-- ESTA ES LA CLAVE
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar detalles: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
 
        private void btnCancela_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnRechaza_Click(object sender, EventArgs e)
        {
            bool todosSeleccionados = true;

            foreach (DataGridViewRow row in dgvEquipo.Rows)
            {
                bool estaSeleccionado = Convert.ToBoolean(row.Cells["chk"].Value);
                if (!estaSeleccionado)
                {
                    todosSeleccionados = false;
                    break;
                }
            }

            if (!todosSeleccionados)
            {
                MessageBox.Show("Debe seleccionar todos los equipos para poder rechazar el documento.",
                    "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirmacion = MessageBox.Show(
                "¿Está seguro que desea rechazar? Se eliminarán todos los equipos.",
                "Confirmar Rechazo",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (confirmacion != DialogResult.Yes)
                return;

            string mensaje = "";

            CN_Prestamo objCN_Prestamo = new CN_Prestamo();

            foreach (DataGridViewRow row in dgvEquipo.Rows)
            {
                string codigoEquipo = row.Cells["CodigoEquipo"].Value.ToString();
                string numeroSerial = row.Cells["NumeroSerial"].Value.ToString();
                string movimiento = row.Cells["Movimiento"].Value.ToString(); // <-- Acta o Prestamo

                if (movimiento == "Acta")
                {
                    objCN_Acta.EliminarEquipoDeActa(numeroDocumento, codigoEquipo, numeroSerial, out mensaje);
                }
                else // Prestamo
                {
                    objCN_Prestamo.EliminarEquipoPrestamo(numeroDocumento, codigoEquipo, numeroSerial, out mensaje);
                }
            }

            MessageBox.Show("Documento rechazado correctamente.",
                "Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.DialogResult = DialogResult.OK;
        }

        private void btnAutoriza_Click(object sender, EventArgs e)
        {
            DialogResult confirmacion = MessageBox.Show(
        "¿Está seguro que desea autorizar los equipos seleccionados y eliminar los no seleccionados?",
        "Confirmar Autorización",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Question
    );

            if (confirmacion != DialogResult.Yes)
                return;

            bool huboCambios = false;
            string mensaje = "";

            CN_Prestamo objCN_Prestamo = new CN_Prestamo();

            foreach (DataGridViewRow row in dgvEquipo.Rows)
            {
                bool estaSeleccionado = Convert.ToBoolean(row.Cells["chk"].Value);
                string codigoEquipo = row.Cells["CodigoEquipo"].Value.ToString();
                string numeroSerial = row.Cells["NumeroSerial"].Value.ToString();
                string movimiento = row.Cells["Movimiento"].Value.ToString(); // Acta o Prestamo

                if (estaSeleccionado)
                {
                    // AUTORIZAR
                    if (movimiento == "Acta")
                    {
                        bool resultado = objCN_Acta.AutorizarEquipoPendiente(numeroDocumento, codigoEquipo, numeroSerial, idUsuario, out mensaje);
                        if (!resultado)
                        {
                            MessageBox.Show($"Error al autorizar equipo {codigoEquipo} ({numeroSerial}): {mensaje}",
                                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else // Prestamo
                    {
                        bool resultado;
                        objCN_Prestamo.AutorizarPrestamo(numeroDocumento, codigoEquipo, numeroSerial, idUsuario, out resultado, out mensaje);
                        if (!resultado)
                        {
                            MessageBox.Show($"Error al autorizar equipo {codigoEquipo} ({numeroSerial}): {mensaje}",
                                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    // ELIMINAR
                    if (movimiento == "Acta")
                    {
                        objCN_Acta.EliminarEquipoDeActa(numeroDocumento, codigoEquipo, numeroSerial, out mensaje);
                    }
                    else // Prestamo
                    {
                        objCN_Prestamo.EliminarEquipoPrestamo(numeroDocumento, codigoEquipo, numeroSerial, out mensaje);
                    }
                }

                huboCambios = true;
            }

            if (huboCambios)
            {
                MessageBox.Show("Se procesaron los equipos correctamente.",
                    "Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("No seleccionó ningún equipo.",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void chkSeleccionarTod_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvEquipo.Rows)
            {
                row.Cells["chk"].Value = chkSeleccionarTod.Checked;
            }

            dgvEquipo.RefreshEdit();
        }
    }
}
