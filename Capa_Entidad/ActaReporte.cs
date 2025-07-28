using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class ActaReporte
    {
        public string NumeroDocumento { get; set; }
        public string FechaRegistro { get; set; }
        public string NombreFarmacia { get; set; }
        public string CodigoEquipo { get; set; }
        public string NombreEquipo { get; set; }
        public string Estado { get; set; }
        public int Cantidad { get; set; }
        public string NumeroSerial { get; set; }
        public string Caja { get; set; }
        public string MotivoBaja { get; set; }
        public string EstadoBaja { get; set; }
        public string EstadoAutorizacion { get; set; }
    }
}
