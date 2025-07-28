using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Registrar
    {
        public int IdRegistrar { get; set; }
        public Usuario oUsuario { get; set; }
        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public List<Detalle_Registrar> oDetalleRegistrar { get; set; }
        public string FechaRegistro { get; set; }
    }
}
