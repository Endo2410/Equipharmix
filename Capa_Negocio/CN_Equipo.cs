
using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Equipo
    {


        private CD_Equipo objcd_Producto = new CD_Equipo();


        public List<Equipo> Listar()
        {
            return objcd_Producto.Listar();
        }

        public Equipo ObtenerPorCodigo(string codigo)
        {
            // Por ejemplo, si el IdEstado 1 representa "Activo"
            return Listar().FirstOrDefault(p => p.Codigo == codigo && p.oEstado.IdEstado == 1);
        }

        public int Registrar(Equipo obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.Codigo == "")
            {
                Mensaje += "Es necesario el codigo del equipo\n";
            }

            if (obj.Nombre == "")
            {
                Mensaje += "Es necesario el nombre del equipo\n";
            }

            if (obj.Descripcion == "")
            {
                Mensaje += "Es necesario la Descripcion del equipo\n";
            }

            if (Mensaje != string.Empty)
            {
                return 0;
            }
            else
            {
                return objcd_Producto.Registrar(obj, out Mensaje);
            }


        }


        public bool Editar(Equipo obj, out string Mensaje)
        {

            Mensaje = string.Empty;
            

            if (obj.Codigo == "")
            {
                Mensaje += "Es necesario el codigo del equipo\n";
            }

            if (obj.Nombre == "")
            {
                Mensaje += "Es necesario el nombre del equipo\n";
            }

            if (obj.Descripcion == "")
            {
                Mensaje += "Es necesario la Descripcion del equipo\n";
            }

            if (Mensaje != string.Empty)
            {
                return false;
            }
            else
            {
                return objcd_Producto.Editar(obj, out Mensaje);
            }
        }
    }
}
