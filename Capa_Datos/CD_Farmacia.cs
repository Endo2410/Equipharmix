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
    public class CD_Farmacia
    {
        //Mostar lista de Farmacias
        public List<Farmacia> Listar()
        {
            List<Farmacia> lista = new List<Farmacia>();

            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_LISTAR_FARMACIA", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Farmacia()
                            {
                                IdFarmacia = Convert.ToInt32(dr["IdFarmacia"]),
                                Codigo = dr["Codigo"].ToString(),
                                Nombre = dr["Nombre"].ToString(),
                                Correo = dr["Correo"].ToString(),
                                Telefono = dr["Telefono"].ToString(),
                                Estado = Convert.ToBoolean(dr["Estado"])
                            });
                        }
                    }
                }
                catch
                {
                    lista = new List<Farmacia>();
                }
            }

            return lista;
        }

        public int ObtenerIdFarmaciaPorCodigo(string codigo)
        {
            int idFarmacia = 0;

            using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
            {
                SqlCommand cmd = new SqlCommand("SP_OBTENER_ID_FARMACIA", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Codigo", codigo);

                conexion.Open();
                object result = cmd.ExecuteScalar();
                if (result != null)
                    idFarmacia = Convert.ToInt32(result);
            }

            return idFarmacia;
        }


        //Registrar Farmacia

        public int Registrar(Farmacia obj, out string Mensaje)
        {
            int idFarmaciagenerado = 0;
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

                    SqlCommand cmd = new SqlCommand("SP_REGISTRAR_FARMACIA", oconexion);
                    cmd.Parameters.AddWithValue("Codigo", obj.Codigo);
                    cmd.Parameters.AddWithValue("Nombre", obj.Nombre);
                    cmd.Parameters.AddWithValue("Correo", obj.Correo);
                    cmd.Parameters.AddWithValue("Telefono", obj.Telefono);
                    cmd.Parameters.AddWithValue("Estado", obj.Estado);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.ExecuteNonQuery();

                    idFarmaciagenerado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                idFarmaciagenerado = 0;
                Mensaje = ex.Message;
            }
            return idFarmaciagenerado;
        }

        //Editar Farmacias
        public bool Editar(Farmacia obj, out string Mensaje)
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


                    SqlCommand cmd = new SqlCommand("SP_MODIFICAR_FARMACIA", oconexion);
                    cmd.Parameters.AddWithValue("IdFarmacia", obj.IdFarmacia);
                    cmd.Parameters.AddWithValue("Codigo", obj.Codigo);
                    cmd.Parameters.AddWithValue("Nombre", obj.Nombre);
                    cmd.Parameters.AddWithValue("Correo", obj.Correo);
                    cmd.Parameters.AddWithValue("Telefono", obj.Telefono);
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

        public List<Farmacia> ReporteFarmacia(string fechainicio, string fechafin)
        {
            List<Farmacia> lista = new List<Farmacia>();

            using (SqlConnection oConexion = new SqlConnection(Conexion.cadena))
            {
                SqlCommand cmd = new SqlCommand("SP_OBTENER_REPORTE_FARMACIA", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FechaInicio", fechainicio);
                cmd.Parameters.AddWithValue("@FechaFin", fechafin);

                oConexion.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Farmacia
                        {
                            IdFarmacia = Convert.ToInt32(dr["IdFarmacia"]),
                            Codigo = dr["Codigo"].ToString(),
                            Nombre = dr["Nombre"].ToString(),
                            Telefono = dr["Telefono"].ToString(),
                            Correo = dr["Correo"].ToString(),
                            Estado = Convert.ToBoolean(dr["Estado"]),
                            FechaRegistro = Convert.ToDateTime(dr["FechaRegistro"])
                        });
                    }
                }
            }

            return lista;
        }
    }
}
