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

namespace CapaPresentacion
{
    public partial class frmscanear : Form
    {
        public frmscanear()
        {
            InitializeComponent();
        }

        private void txtSerie_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BuscarSerie();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void BuscarSerie()
        {
            string serie = txtSerie.Text.Trim();

            if (string.IsNullOrEmpty(serie))
            {
                MessageBox.Show("Ingrese o escanee un número de serie.", "Mensaje",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            CN_Reporte obj = new CN_Reporte();
            var detalle = obj.BuscarSerie(serie);

            if (detalle == null)
            {
                MessageBox.Show("No se encontró información para este número de serie.",
                                "Sin resultados", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LimpiarCampos(false);
                return;
            }

            // Asignar datos al formulario
            txtDocumento.Text = detalle.NumeroDocumento;
            txtFecha.Text = detalle.FechaRegistro;
            txtFarmacia.Text = detalle.NombreFarmacia;
            txtCodigoEquipo.Text = detalle.CodigoEquipo;
            txtNombreEquipo.Text = detalle.NombreEquipo;
            txtCantidad.Text = detalle.Cantidad.ToString();
            txtCaja.Text = detalle.Caja;
            txtMarca.Text = detalle.Marca;
            txtUsuario.Text = detalle.NombreCompleto;
        }

        private void LimpiarCampos(bool limpiarSerie)
        {
            if (limpiarSerie)
                txtSerie.Clear();

            txtDocumento.Clear();
            txtFecha.Clear();
            txtFarmacia.Clear();
            txtCodigoEquipo.Clear();
            txtNombreEquipo.Clear();
            txtCantidad.Clear();
            txtCaja.Clear();
            txtMarca.Clear();
            txtUsuario.Clear();

            txtSerie.Focus();
        }

        private void btnborrar_Click(object sender, EventArgs e)
        {
            LimpiarCampos(true);
        }

        private void btnbuscar_Click(object sender, EventArgs e)
        {
            BuscarSerie();
        }
    }
}
