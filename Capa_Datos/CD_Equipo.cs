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
    public class CD_Equipo
    {


        public List<Equipo> Listar()
        {
            List<Equipo> lista = new List<Equipo>();

            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_LISTAR_EQUIPO", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Equipo()
                            {
                                IdEquipo = Convert.ToInt32(dr["IdEquipo"]),
                                Codigo = dr["Codigo"].ToString(),
                                Nombre = dr["Nombre"].ToString(),
                                Descripcion = dr["Descripcion"].ToString(),
                                oMarca = new Marca()
                                {
                                    IdMarca = Convert.ToInt32(dr["IdMarca"]),
                                    Descripcion = dr["DescripcionMarca"].ToString()
                                },
                                Stock = Convert.ToInt32(dr["Stock"]),
                                oEstado = new Estado()
                                {
                                    IdEstado = Convert.ToInt32(dr["IdEstado"]),
                                    Descripcion = dr["DescripcionEstado"].ToString()
                                }
                            });
                        }
                    }
                }
                catch
                {
                    lista = new List<Equipo>();
                }
            }

            return lista;
        }

        public int Registrar(Equipo obj, out string Mensaje)
        {
            int idEquipogenerado = 0;
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

                    SqlCommand cmd = new SqlCommand("SP_REGISTRAR_EQUIPO", oconexion);
                    cmd.Parameters.AddWithValue("Codigo", obj.Codigo);
                    cmd.Parameters.AddWithValue("Nombre", obj.Nombre);
                    cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
                    cmd.Parameters.AddWithValue("IdMarca", obj.oMarca.IdMarca);
                    cmd.Parameters.AddWithValue("IdEstado", obj.oEstado.IdEstado);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;


                    cmd.ExecuteNonQuery();

                    idEquipogenerado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                idEquipogenerado = 0;
                Mensaje = ex.Message;
            }
            return idEquipogenerado;
        }

        public bool Editar(Equipo obj, out string Mensaje)
        {
            bool respuesta = false;
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


                    SqlCommand cmd = new SqlCommand("SP_MODIFICAR_EQUIPO", oconexion);
                    cmd.Parameters.AddWithValue("IdEquipo", obj.IdEquipo);
                    cmd.Parameters.AddWithValue("Codigo", obj.Codigo);
                    cmd.Parameters.AddWithValue("Nombre", obj.Nombre);
                    cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
                    cmd.Parameters.AddWithValue("IdMarca", obj.oMarca.IdMarca);
                    cmd.Parameters.AddWithValue("IdEstado", obj.oEstado.IdEstado);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }

            }
            catch (Exception ex)
            {
                respuesta = false;
                Mensaje = ex.Message;
            }
            return respuesta;
        }
    }
}
