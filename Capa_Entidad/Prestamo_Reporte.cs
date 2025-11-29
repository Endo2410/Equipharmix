using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Prestamo_Reporte
    {
        public string NumeroDocumento { get; set; }
        public string FechaRegistro { get; set; }
        public string NombreFarmacia { get; set; }
        public string CodigoEquipo { get; set; }
        public string NombreEquipo { get; set; }
        public int Cantidad { get; set; }
        public string NumeroSerial { get; set; }
        public string EstadoPrestamo { get; set; }
        public string NombreCompleto { get; set; }
    }

}
