using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Accion
    {
        public int IdAccion { get; set; }
        public int? IdSubMenu { get; set; }
        public int? IdModulo { get; set; }
        public string NombreAccion { get; set; }
        public string FechaRegistro { get; set; }
    }
}
