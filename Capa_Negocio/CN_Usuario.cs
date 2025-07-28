using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions; // Necesario para expresiones regulares
using System.Threading.Tasks;

using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Usuario
    {
        private CD_Usuario objcd_usuario = new CD_Usuario();

        public List<Usuario> Listar()
        {
            return objcd_usuario.Listar();
        }

        public int Registrar(Usuario obj, out string Mensaje)
        {
            Mensaje = ValidarUsuario(obj);

            if (Mensaje != string.Empty)
            {
                return 0;
            }
            else
            {
                return objcd_usuario.Registrar(obj, out Mensaje);
            }
        }

        public bool Editar(Usuario obj, out string Mensaje)
        {
            Mensaje = ValidarUsuario(obj);

            if (Mensaje != string.Empty)
            {
                return false;
            }
            else
            {
                return objcd_usuario.Editar(obj, out Mensaje);
            }
        }

        public bool ActualizarClave(int idUsuario, string nuevaClave)
        {
            return objcd_usuario.ActualizarClave(idUsuario, nuevaClave);
        }

        // Método de validación reutilizable para registrar y editar
        private string ValidarUsuario(Usuario obj)
        {
            StringBuilder mensaje = new StringBuilder();

            if (string.IsNullOrWhiteSpace(obj.Documento))
            {
                mensaje.AppendLine("Es necesario el documento del usuario.");
            }

            // Validación de nombre completo: solo letras y espacios
            if (string.IsNullOrWhiteSpace(obj.NombreCompleto))
            {
                mensaje.AppendLine("Es necesario el nombre completo del usuario.");
            }
            else if (!Regex.IsMatch(obj.NombreCompleto, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$"))
            {
                mensaje.AppendLine("El nombre completo solo debe contener letras y espacios.");
            }

            // Validación de correo electrónico
            if (string.IsNullOrWhiteSpace(obj.Correo))
            {
                mensaje.AppendLine("Es necesario el correo del usuario.");
            }
            else if (!Regex.IsMatch(obj.Correo, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                mensaje.AppendLine("El formato del correo no es válido.");
            }

            // Validación de clave
            if (string.IsNullOrWhiteSpace(obj.Clave))
            {
                mensaje.AppendLine("Es necesario la clave del usuario.");
            }

            return mensaje.ToString();
        }
    }
}
