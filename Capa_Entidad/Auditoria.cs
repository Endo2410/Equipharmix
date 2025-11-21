using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Auditoria
    {
        public int IdAuditoria { get; set; }
        public string Tabla { get; set; }
        public string Operacion { get; set; }
        public string Usuario { get; set; }
        public string Fecha { get; set; }
        public string Datos { get; set; }
    }
}
