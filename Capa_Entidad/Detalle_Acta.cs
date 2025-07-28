using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Detalle_Acta
    {
        public int IdDetalleActa { get; set; }
        public Equipo oEquipo { get; set; }
        public int Cantidad { get; set; }
        public string NumeroSerial { get; set; }
        public string Caja { get; set; }

        public Acta oActa { get; set; }
        public Farmacia Farmacia { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string NumeroDocumento { get; set; }

        public string EstadoBaja { get; set; }
        public string MotivoBaja { get; set; }

        public Farmacia oFarmacia { get; set; }
        public Usuario oUsuarioSolicitante { get; set; }
        public Usuario oUsuarioAutorizador { get; set; }

        // Cambiado a int porque representa la clave foránea
        public int IdActa { get; set; }

        // Cambiado a int porque representa la clave foránea
        public int IdEquipo { get; set; }

        public string EstadoAutorizacion { get; set; }
    }

}
