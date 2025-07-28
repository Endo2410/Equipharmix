using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Detalle_Registrar
    {
        public int IdDetalleRegistrar { get; set; }
        public Equipo oEquipo { get; set; }

        public int Cantidad { get; set; }
        public string FechaRegistro { get; set; }
    }
}
