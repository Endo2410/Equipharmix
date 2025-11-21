using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Accion
    {
        private CD_Accion objCD = new CD_Accion();
        public List<Accion> Listar() => objCD.Listar();
    }
}
