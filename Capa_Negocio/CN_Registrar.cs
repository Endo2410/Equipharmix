using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Registrar
    {

        private CD_Registrar objcd_Registrar = new CD_Registrar();


        public int ObtenerCorrelativo()
        {
            return objcd_Registrar.ObtenerCorrelativo();
        }

        public bool Registrar(Registrar obj,DataTable DetalleRegistrar, out string Mensaje)
        {
            return objcd_Registrar.Registrar(obj,DetalleRegistrar, out Mensaje);
        }

        public Registrar ObtenerRegistrar(string numero) {

            Registrar oRegistrar = objcd_Registrar.ObtenerRegistro(numero);

            if (oRegistrar.IdRegistrar != 0) {
                List<Detalle_Registrar> oDetalleRegistrar = objcd_Registrar.ObtenerDetalleRegistrar(oRegistrar.IdRegistrar);

                oRegistrar.oDetalleRegistrar = oDetalleRegistrar;
            }
            return oRegistrar;
        }


    }
}
