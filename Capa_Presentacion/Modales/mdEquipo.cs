using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.Utilidades;

namespace CapaPresentacion.Modales
{
    public partial class mdEquipo : Form
    {
        public Equipo _Equipo { get; set; }
        public mdEquipo()
        {
            InitializeComponent();
        }

        private void mdEquipo_Load(object sender, EventArgs e)
        {
            foreach (DataGridViewColumn columna in dgvdata.Columns)
            {
                if (columna.Visible)
                {
                    cbobusqueda.Items.Add(new OpcionCombo() { Valor = columna.Name, Texto = columna.HeaderText });
                }
            }

            cbobusqueda.DisplayMember = "Texto";
            cbobusqueda.ValueMember = "Valor";
            cbobusqueda.SelectedIndex = 0;

            List<Equipo> lista = new CN_Equipo().Listar();

            foreach (Equipo item in lista)
            {
                dgvdata.Rows.Add(new object[] {
                    item.IdEquipo,
                    item.Codigo,
                    item.Nombre,
                    item.oMarca.Descripcion,
                    item.Stock
                });
            }

            dgvdata.CellDoubleClick += dgvdata_CellDoubleClick;
        }


        private void dgvdata_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int iRow = e.RowIndex;
            if (iRow >= 0)
            {
                _Equipo = new Equipo()
                {
                    IdEquipo = Convert.ToInt32(dgvdata.Rows[iRow].Cells[0].Value),
                    Codigo = dgvdata.Rows[iRow].Cells[1].Value.ToString(),
                    Nombre = dgvdata.Rows[iRow].Cells[2].Value.ToString(),
                    Stock = Convert.ToInt32(dgvdata.Rows[iRow].Cells[4].Value)
                };
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnbuscar_Click(object sender, EventArgs e)
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

        private void btnlimpiarbuscador_Click(object sender, EventArgs e)
        {
            txtbusqueda.Text = "";
            foreach (DataGridViewRow row in dgvdata.Rows)
            {
                row.Visible = true;
            }
        }

    }
}
