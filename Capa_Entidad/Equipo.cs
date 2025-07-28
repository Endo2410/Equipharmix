using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Equipo
    {
        public int IdEquipo { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public Marca oMarca { get; set; }
        public int Stock { get; set; }
        
        public Estado oEstado { get; set; }
        public string FechaRegistro { get; set; }
    }
}
