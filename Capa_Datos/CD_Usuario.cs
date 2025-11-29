using CapaDatos;
using CapaEntidad;
using CapaPresentacion.Utilidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Usuario
    {

        // Listar usuarios usando SP_ListarUsuarios
        public List<Usuario> Listar()
        {
            List<Usuario> lista = new List<Usuario>();

            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_LISTAR_USUARIOS", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Usuario()
                            {
                                IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                                Documento = dr["Documento"].ToString(),
                                NombreCompleto = dr["NombreCompleto"].ToString(),
                                NombreUsuario = dr["NombreUsuario"].ToString(),
                                Correo = dr["Correo"].ToString(),
                                Clave = dr["Clave"].ToString(),
                                Estado = Convert.ToBoolean(dr["Estado"]),
                                oRol = new Rol()
                                {
                                    IdRol = Convert.ToInt32(dr["IdRol"]),
                                    Descripcion = dr["RolDescripcion"].ToString()
                                }
                            });
                        }
                    }
                }
                catch (Exception)
                {
                    lista = new List<Usuario>();
                }
            }

            return lista;
        }

        // Registrar usuario
        public int Registrar(Usuario obj, out string Mensaje)
        {
            int idusuariogenerado = 0;
            Mensaje = string.Empty;


            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
                {

                    oconexion.Open();

                    // Guardar usuario logueado en SESSION_CONTEXT
                    using (SqlCommand cmdt = new SqlCommand("EXEC sp_set_session_context @key, @value", oconexion))
                    {
                        cmdt.Parameters.AddWithValue("@key", "Usuario");
                        cmdt.Parameters.AddWithValue("@value", UsuarioSesion.NombreCompleto);
                        cmdt.ExecuteNonQuery();
                    }

                    SqlCommand cmd = new SqlCommand("SP_REGISTRAR_USUARIO", oconexion);
                    cmd.Parameters.AddWithValue("Documento", obj.Documento);
                    cmd.Parameters.AddWithValue("NombreCompleto", obj.NombreCompleto);
                    cmd.Parameters.AddWithValue("NombreUsuario", obj.NombreUsuario);
                    cmd.Parameters.AddWithValue("Correo", obj.Correo);
                    cmd.Parameters.AddWithValue("Clave", obj.Clave);
                    cmd.Parameters.AddWithValue("IdRol", obj.oRol.IdRol);
                    cmd.Parameters.AddWithValue("Estado", obj.Estado);
                    cmd.Parameters.Add("IdUsuarioResultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.ExecuteNonQuery();

                    idusuariogenerado = Convert.ToInt32(cmd.Parameters["IdUsuarioResultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                idusuariogenerado = 0;
                Mensaje = ex.Message;
            }

            return idusuariogenerado;
        }

        // Editar usuario
        public bool Editar(Usuario obj, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
                {
                    oconexion.Open();

                    // Guardar usuario logueado en SESSION_CONTEXT para auditoría
                    using (SqlCommand cmdt = new SqlCommand("EXEC sp_set_session_context @key, @value", oconexion))
                    {
                        cmdt.Parameters.AddWithValue("@key", "Usuario");
                        cmdt.Parameters.AddWithValue("@value", UsuarioSesion.NombreCompleto);
                        cmdt.ExecuteNonQuery();
                    }

                    using (SqlCommand cmd = new SqlCommand("SP_EDITAR_USUARIO", oconexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Parámetros de entrada
                        cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = obj.IdUsuario;
                        cmd.Parameters.Add("@Documento", SqlDbType.VarChar, 50).Value = obj.Documento;
                        cmd.Parameters.Add("@NombreCompleto", SqlDbType.VarChar, 100).Value = obj.NombreCompleto;
                        cmd.Parameters.Add("@NombreUsuario", SqlDbType.VarChar, 50).Value = obj.NombreUsuario;
                        cmd.Parameters.Add("@Correo", SqlDbType.VarChar, 100).Value = obj.Correo;
                        cmd.Parameters.Add("@Clave", SqlDbType.VarChar, 100).Value = obj.Clave;
                        cmd.Parameters.Add("@IdRol", SqlDbType.Int).Value = obj.oRol.IdRol;
                        cmd.Parameters.Add("@Estado", SqlDbType.Bit).Value = obj.Estado;

                        // Parámetros de salida
                        SqlParameter pRespuesta = new SqlParameter("@Respuesta", SqlDbType.Bit)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(pRespuesta);

                        SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.VarChar, 500)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(pMensaje);

                        // Ejecutar SP
                        cmd.ExecuteNonQuery();

                        // Leer resultados
                        respuesta = Convert.ToBoolean(pRespuesta.Value);
                        Mensaje = pMensaje.Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                respuesta = false;
                Mensaje = ex.Message;
            }

            return respuesta;
        }


        // Actualizar clave
        public bool ActualizarClave(int idUsuario, string nuevaClave)
        {
            bool resultado = false;

            using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
            {
                try
                {

                    conexion.Open();

                    // Guardar usuario logueado en SESSION_CONTEXT para auditoría
                    using (SqlCommand cmdt = new SqlCommand("EXEC sp_set_session_context @key, @value", conexion))
                    {
                        cmdt.Parameters.AddWithValue("@key", "Usuario");
                        cmdt.Parameters.AddWithValue("@value", UsuarioSesion.NombreCompleto);
                        cmdt.ExecuteNonQuery();
                    }

                    SqlCommand cmd = new SqlCommand("SP_ACTUALIZAR_CLAVE", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                    cmd.Parameters.AddWithValue("@NuevaClave", nuevaClave);

                    cmd.ExecuteNonQuery();
                    resultado = true;
                }
                catch (Exception)
                {
                    resultado = false;
                }
            }

            return resultado;
        }
    }
}