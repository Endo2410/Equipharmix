using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Prestamo
    {

        public int IdPrestamo { get; set; }
        public int IdUsuarioSolicita { get; set; }
        public int? IdUsuarioAutoriza { get; set; }
        public int? IdFarmacia { get; set; }
        public string Nombre { get; set; }
        public string Codigo { get; set; }

        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public DateTime FechaPrestamo { get; set; }
        public DateTime? FechaDevolucion { get; set; }
        public string EstadoPrestamo { get; set; }
        public string Comentario { get; set; }
        public Usuario oUsuarioSolicita { get; set; }
        public Usuario oUsuarioAutoriza { get; set; }
        public List<Detalle_Prestamo> oDetalle { get; set; }
        public Farmacia oFarmacia{ get; set; }
        public Usuario oUsuario { get; set; }
        public object oDetalle_Prestamo { get; set; }
    }
}
