using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_SubMenu
    {
        private CD_SubMenu objCD = new CD_SubMenu();
        public List<SubMenu> Listar() => objCD.Listar();
    }
}
