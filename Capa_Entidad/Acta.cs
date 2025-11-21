using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Acta
    {
        public int IdActa { get; set; }
        public Usuario oUsuario { get; set; }
        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }

        public int IdFarmacia { get; set; } 

        public string Nombre { get; set; }
        public string Codigo { get; set; }

        public List<Detalle_Acta> oDetalle_Acta { get; set; }
        public string FechaRegistro { get; set; }
        public string EstadoAutorizacion { get; set; } // ✅ Nueva propiedad
        public bool EsAnulado { get; set; }
    }
}
