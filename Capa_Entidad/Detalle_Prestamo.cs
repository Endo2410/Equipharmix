using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Detalle_Prestamo
    {
        public int IdDetallePrestamo { get; set; }
        public int IdPrestamo { get; set; }
        public string NumeroDocumento { get; set; }
        public int IdEquipo { get; set; }
        public int Cantidad { get; set; }
        public string NumeroSerial { get; set; }
        public string EstadoDevolucion { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string EstadoBaja { get; set; }
        public string MotivoBaja { get; set; }
        public Equipo oEquipo { get; set; }
        public Prestamo oPrestamo { get; set; }
        public Farmacia oFarmacia { get; set; }
        public Usuario oUsuarioSolicitante { get; set; }
        public Usuario oUsuarioAutorizador { get; set; }
        public string EstadoAutorizacion { get; set; }
        public DateTime FechaPrestamo { get; set; }
    }
}
