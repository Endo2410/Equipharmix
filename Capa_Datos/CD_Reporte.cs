using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Reporte
    {
        public List<ActaReporte> ReporteActas(string fechainicio, string fechafin)
        {
            List<ActaReporte> lista = new List<ActaReporte>();

            using (SqlConnection oConexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("usp_Obtenerreporteacta", oConexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FechaInicio", fechainicio);
                    cmd.Parameters.AddWithValue("@FechaFin", fechafin);

                    oConexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new ActaReporte
                            {
                                NumeroDocumento = dr["NumeroDocumento"].ToString(),
                                FechaRegistro = Convert.ToDateTime(dr["FechaRegistro"]).ToString("dd/MM/yyyy"),
                                NombreFarmacia = dr["NombreFarmacia"].ToString(),
                                CodigoEquipo = dr["CodigoEquipo"].ToString(),
                                NombreEquipo = dr["NombreEquipo"].ToString(),
                                Estado = dr["Estado"].ToString(),
                                Cantidad = Convert.ToInt32(dr["Cantidad"]),
                                NumeroSerial = dr["NumeroSerial"].ToString(),
                                Caja = dr["Caja"].ToString(),
                                EstadoAutorizacion = dr["EstadoAutorizacion"].ToString()
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    lista = new List<ActaReporte>();
                }
            }

            return lista;
        }

        public List<ReporteRegistrar> Registrar(string fechainicio, string fechafin)
        {
            List<ReporteRegistrar> lista = new List<ReporteRegistrar>();

            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_ReporteRegistrar", oconexion);
                    cmd.Parameters.AddWithValue("@fechainicio", fechainicio);
                    cmd.Parameters.AddWithValue("@fechafin", fechafin);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new ReporteRegistrar()
                            {
                                FechaRegistro = dr["FechaRegistro"].ToString(),
                                TipoDocumento = dr["TipoDocumento"].ToString(),
                                NumeroDocumento = dr["NumeroDocumento"].ToString(),
                                UsuarioRegistro = dr["UsuarioRegistro"].ToString(),
                                Codigo = dr["CodigoEquipo"].ToString(),
                                Nombre = dr["Equipo"].ToString(),
                                Marca = dr["Marca"].ToString(),
                                Cantidad = dr["Cantidad"].ToString(),
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Opcional: registrar el error, ej:
                    // Console.WriteLine(ex.Message);
                    lista = new List<ReporteRegistrar>();
                }
            }
            return lista;
        }

    }
}
