using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaPresentacion.Utilidades
{
    public static class UtilPermisos
    {
        /// <summary>
        /// Verifica si un usuario tiene permiso para un botón/acción dentro de un submenú o módulo.
        /// </summary>
        /// <param name="permisos">Lista de permisos del usuario</param>
        /// <param name="nombreMenu">Nombre del submenú o módulo</param>
        /// <param name="nombreAccion">Nombre de la acción/botón</param>
        /// <returns></returns>
        public static bool TienePermisoAccion(List<Permiso> permisos, string nombreMenu, string nombreAccion)
        {
            return permisos.Any(p =>
                p.oAccion != null && p.oAccion.NombreAccion == nombreAccion &&
                (
                    // Si la acción pertenece a un SUBMENÚ
                    (p.oSubMenu != null && p.oSubMenu.NombreSubMenu == nombreMenu) ||

                    // Si la acción pertenece directamente a un MÓDULO
                    (p.oSubMenu == null && p.oModulo != null && p.oModulo.NombreModulo == nombreMenu)
                )
            );
        }
    }
}
