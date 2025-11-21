using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Modulo
    {
        private CD_Modulo objCD = new CD_Modulo();
        public List<Modulo> Listar() => objCD.Listar();
    }
}
