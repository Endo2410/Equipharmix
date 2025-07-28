using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Estado
    {
        private CD_Estado objcd_Estado = new CD_Estado();

        public List<Estado> Listar()
        {
            return objcd_Estado.Listar();
        }
    }
}
