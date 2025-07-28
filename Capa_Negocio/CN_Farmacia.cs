using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Farmacia
    {

        private CD_Farmacia objcd_Farmacia = new CD_Farmacia();


        public List<Farmacia> Listar()
        {
            return objcd_Farmacia.Listar();
        }

        public int Registrar(Farmacia obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.Codigo == "")
            {
                Mensaje += "Es necesario el Codigo de la Farmacia\n";
            }

            if (obj.Nombre == "")
            {
                Mensaje += "Es necesario el nombre completo de la Farmacia\n";
            }

            if (Mensaje != string.Empty)
            {
                return 0;
            }
            else
            {
                return objcd_Farmacia.Registrar(obj, out Mensaje);
            }


        }


        public bool Editar(Farmacia obj, out string Mensaje)
        {

            Mensaje = string.Empty;

            if (obj.Codigo == "")
            {
                Mensaje += "Es necesario el Codigo de la Farmacia\n";
            }

            if (obj.Nombre == "")
            {
                Mensaje += "Es necesario el nombre completo de la Farmacia\n";
            }

            if (Mensaje != string.Empty)
            {
                return false;
            }
            else
            {
                return objcd_Farmacia.Editar(obj, out Mensaje);
            }
        }
    }
}