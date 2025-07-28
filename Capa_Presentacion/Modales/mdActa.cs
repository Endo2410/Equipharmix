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

        private DataGridView dgvEquipos;
        private Button btnAutorizar;
        private Button btnCancelar;
        private Button btnRechazar;
        private CheckBox chkSeleccionarTodo;

        public mdActa(string numeroDocumento, int idUsuario)
        {
            this.numeroDocumento = numeroDocumento;
            this.idUsuario = idUsuario;

            InitializeComponent();
            
        }

        private void mdActa_Load(object sender, EventArgs e)
        {
            this.Text = "Autorizar Equipos";
            this.Size = new Size(1000, 500);
            this.StartPosition = FormStartPosition.CenterParent;

            dgvEquipos = new DataGridView
            {
                Dock = DockStyle.Top,
                Height = 360,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            dgvEquipos.Columns.Add(new DataGridViewCheckBoxColumn() { Name = "chk", HeaderText = "" });
            dgvEquipos.Columns.Add("CodigoEquipo", "Código");
            dgvEquipos.Columns.Add("NombreEquipo", "Equipo");
            dgvEquipos.Columns.Add("Marca", "Marca");
            dgvEquipos.Columns.Add("Estado", "Estado");
            dgvEquipos.Columns.Add("Cantidad", "Cantidad");
            dgvEquipos.Columns.Add("NumeroSerial", "N° Serial");
            dgvEquipos.Columns.Add("Caja", "Caja");

            chkSeleccionarTodo = new CheckBox
            {
                Text = "Seleccionar todos",
                Left = 20,
                Top = dgvEquipos.Bottom + 10,
                AutoSize = true
            };
            chkSeleccionarTodo.CheckedChanged += ChkSeleccionarTodo_CheckedChanged;

            // Botón Autorizar (primero a la izquierda)
            btnAutorizar = new Button
            {
                Text = "✔ Autorizar",
                Width = 160,
                Height = 45,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Left = 20,
                Top = chkSeleccionarTodo.Bottom + 15,
                BackColor = Color.FromArgb(0, 153, 76),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnAutorizar.FlatAppearance.BorderSize = 0;
            btnAutorizar.Click += BtnAutorizar_Click;

            // Botón Rechazar (a la derecha de Autorizar)
            btnRechazar = new Button
            {
                Text = "⛔ Rechazar Acta",
                Width = 170,
                Height = 45,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Left = btnAutorizar.Right + 15,
                Top = btnAutorizar.Top,
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnRechazar.FlatAppearance.BorderSize = 0;
            btnRechazar.Click += BtnRechazar_Click;

            // Botón Cancelar (a la derecha de Rechazar)
            btnCancelar = new Button
            {
                Text = "Cancelar",
                Width = 120,
                Height = 45,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Left = btnRechazar.Right + 15,
                Top = btnAutorizar.Top,
                BackColor = Color.LightGray,
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat
            };
            btnCancelar.FlatAppearance.BorderSize = 0;
            btnCancelar.Click += BtnCancelar_Click;

            this.Controls.Add(dgvEquipos);
            this.Controls.Add(chkSeleccionarTodo);
            this.Controls.Add(btnAutorizar);
            this.Controls.Add(btnRechazar);
            this.Controls.Add(btnCancelar);

            CargarEquiposDelDocumento();
        }

        private void CargarEquiposDelDocumento()
        {
            dgvEquipos.Rows.Clear();

            chkSeleccionarTodo.CheckedChanged -= ChkSeleccionarTodo_CheckedChanged;
            chkSeleccionarTodo.Checked = false;
            chkSeleccionarTodo.CheckedChanged += ChkSeleccionarTodo_CheckedChanged;

            List<Detalle_Acta> lista = objCN_Acta.ObtenerEquiposPorDocumento(numeroDocumento);
            if (lista == null) lista = new List<Detalle_Acta>();

            foreach (var item in lista)
            {
                if (item.oEquipo == null || item.oEquipo.oMarca == null || item.oEquipo.oEstado == null)
                    continue;

                dgvEquipos.Rows.Add(
                    false,
                    item.oEquipo.Codigo ?? "",
                    item.oEquipo.Nombre ?? "",
                    item.oEquipo.oMarca.Descripcion ?? "",
                    item.oEquipo.oEstado.Descripcion ?? "",
                    item.Cantidad,
                    item.NumeroSerial ?? "",
                    item.Caja ?? ""
                );
            }
        }

        private void ChkSeleccionarTodo_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvEquipos.Rows)
            {
                row.Cells["chk"].Value = chkSeleccionarTodo.Checked;
            }

            dgvEquipos.RefreshEdit();
        }

        private void BtnAutorizar_Click(object sender, EventArgs e)
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

            foreach (DataGridViewRow row in dgvEquipos.Rows)
            {
                bool estaSeleccionado = Convert.ToBoolean(row.Cells["chk"].Value);
                string codigoEquipo = row.Cells["CodigoEquipo"].Value.ToString();
                string numeroSerial = row.Cells["NumeroSerial"].Value.ToString();

                if (estaSeleccionado)
                {
                    objCN_Acta.AutorizarEquipoPendiente(numeroDocumento, codigoEquipo, numeroSerial, idUsuario, out mensaje);
                    huboCambios = true;
                }
                else
                {
                    objCN_Acta.EliminarEquipoDeActa(numeroDocumento, codigoEquipo, numeroSerial, out mensaje);
                    huboCambios = true;
                }
            }

            if (huboCambios)
            {
                MessageBox.Show("Se procesaron los equipos correctamente.", "Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("No seleccionó ningún equipo.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnRechazar_Click(object sender, EventArgs e)
        {
            bool todosSeleccionados = true;

            foreach (DataGridViewRow row in dgvEquipos.Rows)
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
                MessageBox.Show("Debe seleccionar todos los equipos para poder rechazar el acta.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirmacion = MessageBox.Show(
                "¿Está seguro que desea rechazar el acta? Se eliminarán todos los equipos.",
                "Confirmar Rechazo",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (confirmacion != DialogResult.Yes)
                return;

            string mensaje = "";

            foreach (DataGridViewRow row in dgvEquipos.Rows)
            {
                string codigoEquipo = row.Cells["CodigoEquipo"].Value.ToString();
                string numeroSerial = row.Cells["NumeroSerial"].Value.ToString();
                objCN_Acta.EliminarEquipoDeActa(numeroDocumento, codigoEquipo, numeroSerial, out mensaje);
            }

            MessageBox.Show("El acta fue rechazada correctamente.", "Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
