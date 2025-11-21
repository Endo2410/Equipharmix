using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaPresentacion.Utilidades
{
    public static class UsuarioSesion
    {
        public static string NombreCompleto { get; private set; }
        public static int IdUsuario { get; private set; }

        public static void IniciarSesion(int idUsuario, string nombreCompleto)
        {
            IdUsuario = idUsuario;
            NombreCompleto = nombreCompleto;
        }

        public static void CerrarSesion()
        {
            IdUsuario = 0;
            NombreCompleto = null;
        }
    }
}
