using Capa_Datos;
using Capa_Entidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Negocio
{
    public class CN_Producto
    {

        private CD_Producto objcd_Productos = new CD_Producto();

        public List<Producto> Listar()
        {
            return objcd_Productos.Listar();
        }

        public int Registrar(Producto obj, out string Mensaje)
        {
            //Reglas que se deben cumplir en la capa presentacion
            Mensaje = string.Empty;
            if (obj.Codigo == "")
            {
                Mensaje += "Es necesario el codigo del Productos\n";
            }

            if (obj.Nombre == "")
            {
                Mensaje += "Es necesario el nombre del Productos\n";
            }

            if (obj.Descripcion == "")
            {
                Mensaje += "Es necesario ingresar la descripcion del Productos\n";
            }

            if (Mensaje != string.Empty)
            {
                return 0;
            }
            else
            {
                return objcd_Productos.Registrar(obj, out Mensaje);
            }
        }

        public bool Editar(Producto obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            if (obj.Codigo == "")
            {
                Mensaje += "Es necesario el codigo del Productos\n";
            }

            if (obj.Nombre == "")
            {
                Mensaje += "Es necesario el nombre del Productos\n";
            }

            if (obj.Descripcion == "")
            {
                Mensaje += "Es necesario ingresar la descripcion del Productos\n";
            }

            if (Mensaje != string.Empty)
            {
                return false;
            }
            else
            {
                return objcd_Productos.Editar(obj, out Mensaje);
            }
        }

        public bool Eliminar(Producto obj, out string Mensaje)
        {
            return objcd_Productos.Eliminar(obj, out Mensaje);
        }
    }
}
