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
    public partial class mdAcercade : Form
    {
        public mdAcercade()
        {
            InitializeComponent();
        }

        private void mdAcercade_Load(object sender, EventArgs e)
        {
            // Ajustar propiedades visuales del RichTextBox desde código
            richTextBox1.Font = new Font("Segoe UI", 12);
            richTextBox1.ReadOnly = true;
            richTextBox1.BorderStyle = BorderStyle.None;
            richTextBox1.BackColor = SystemColors.Control;
            richTextBox1.Dock = DockStyle.Fill;
            richTextBox1.ScrollBars = RichTextBoxScrollBars.Vertical;
            richTextBox1.SelectionAlignment = HorizontalAlignment.Left; // No hay justificado nativo
            this.Size = new Size(580, 400);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // Contenido del mensaje
            richTextBox1.Text =
                "📦 SISTEMA DE CONTROL DE INVENTARIO PARA FARMACIAS\n\n" +
                "Este software ha sido desarrollado con el objetivo de optimizar la gestión de equipos tecnológicos en farmacias. " +
                "Permite registrar ingresos y salidas de equipos, generar actas de entrega, autorizar o rechazar bajas, y realizar un " +
                "seguimiento detallado del estado y número de serie de cada equipo.\n\n" +

                "FUNCIONALIDADES DESTACADAS:\n" +
                " • Registro y control de equipos por farmacia.\n" +
                " • Generación automática de actas de entrega.\n" +
                " • Control de autorizaciones de bajas y recuperación.\n" +
                " • Búsqueda, filtrado y exportación de reportes.\n\n" +

                "El sistema proporciona una administración segura, trazable y eficiente del inventario farmacéutico.\n\n" +

                "🛠️ Desarrollado por: José Luis López Blanco\n" +
                "© Farmacia Saba 2025. Todos los derechos reservados.";

        }
    }
    
}
