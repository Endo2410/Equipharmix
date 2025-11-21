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
    public class CD_Marca
    {
        //Mostrar lista de Marcas
        public List<Marca> Listar()
        {
            List<Marca> lista = new List<Marca>();

            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_LISTAR_MARCA", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Marca()
                            {
                                IdMarca = Convert.ToInt32(dr["IdMarca"]),
                                Descripcion = dr["Descripcion"].ToString(),
                                Estado = Convert.ToBoolean(dr["Estado"])
                            });
                        }
                    }
                }
                catch
                {
                    lista = new List<Marca>();
                }
            }

            return lista;
        }


        //Registrar Marca
        public int Registrar(Marca obj, out string Mensaje)
        {
            int idMarcagenerado = 0;
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


                    SqlCommand cmd = new SqlCommand("SP_REGISTRAR_MARCA", oconexion);
                    cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
                    cmd.Parameters.AddWithValue("Estado", obj.Estado);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.ExecuteNonQuery();

                    idMarcagenerado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();

                }

            }
            catch (Exception ex)
            {
                idMarcagenerado = 0;
                Mensaje = ex.Message;
            }

            return idMarcagenerado;
        }


        //Editar Marca
        public bool Editar(Marca obj, out string Mensaje)
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


                    SqlCommand cmd = new SqlCommand("SP_EDITAR_MARCA", oconexion);
                    cmd.Parameters.AddWithValue("IdMarca", obj.IdMarca);
                    cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
                    cmd.Parameters.AddWithValue("Estado", obj.Estado);
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
