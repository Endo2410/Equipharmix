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
                    SqlCommand cmd = new SqlCommand("SP_OBTENER_REPORTE_ACTA", oConexion);
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
                                Cantidad = Convert.ToInt32(dr["Cantidad"]),
                                NumeroSerial = dr["NumeroSerial"].ToString(),
                                Caja = dr["Caja"].ToString(),
                                EstadoAutorizacion = dr["EstadoAutorizacion"].ToString(),
                                NombreCompleto = dr["NombreCompleto"].ToString()
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
                    SqlCommand cmd = new SqlCommand("SP_REPORTE_REGISTRAR", oconexion);
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

        public List<Auditoria> ObtenerAuditoria(string tabla, string fechainicio, string fechafin)
        {
            List<Auditoria> lista = new List<Auditoria>();

            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_Reporte_Auditoria", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FechaInicio", fechainicio);
                    cmd.Parameters.AddWithValue("@FechaFin", fechafin);

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Auditoria
                            {
                                IdAuditoria = Convert.ToInt32(dr["IdAuditoria"]),
                                Tabla = dr["Tabla"].ToString(),
                                Operacion = dr["Operacion"].ToString(),
                                Usuario = dr["Usuario"].ToString(),
                                Fecha = Convert.ToDateTime(dr["Fecha"]).ToString("dd/MM/yyyy HH:mm"),
                                Datos = dr["Datos"].ToString()
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    lista = new List<Auditoria>();
                }
            }

            return lista;
        }

        public List<Prestamo_Reporte> ReportePrestamo(string fechainicio, string fechafin)
        {
            List<Prestamo_Reporte> lista = new List<Prestamo_Reporte>();

            using (SqlConnection oConexion = new SqlConnection(Conexion.cadena))
            {
                SqlCommand cmd = new SqlCommand("SP_OBTENER_REPORTE_PRESTAMO", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FechaInicio", fechainicio);
                cmd.Parameters.AddWithValue("@FechaFin", fechafin);

                oConexion.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Prestamo_Reporte
                        {
                            NumeroDocumento = dr["NumeroDocumento"].ToString(),
                            FechaRegistro = Convert.ToDateTime(dr["FechaRegistro"]).ToString("dd/MM/yyyy"),
                            NombreFarmacia = dr["NombreFarmacia"].ToString(),
                            CodigoEquipo = dr["CodigoEquipo"].ToString(),
                            NombreEquipo = dr["NombreEquipo"].ToString(),
                            Cantidad = Convert.ToInt32(dr["Cantidad"]),
                            NumeroSerial = dr["NumeroSerial"].ToString(),
                            EstadoPrestamo = dr["EstadoPrestamo"].ToString(),
                            NombreCompleto = dr["NombreCompleto"].ToString()
                        });
                    }
                }
            }

            return lista;
        }

        public ActaReporte BuscarSerie(string numeroSerie)
        {
            ActaReporte obj = null;

            using (SqlConnection con = new SqlConnection(Conexion.cadena))
            {
                SqlCommand cmd = new SqlCommand("SP_BUSCAR_SERIE", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NumeroSerie", numeroSerie);

                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        obj = new ActaReporte
                        {
                            NumeroDocumento = dr["NumeroDocumento"].ToString(),
                            FechaRegistro = Convert.ToDateTime(dr["FechaRegistro"]).ToString("dd/MM/yyyy"),
                            NombreFarmacia = dr["NombreFarmacia"].ToString(),
                            CodigoEquipo = dr["CodigoEquipo"].ToString(),
                            NombreEquipo = dr["NombreEquipo"].ToString(),
                            Cantidad = Convert.ToInt32(dr["Cantidad"]),
                            NumeroSerial = dr["NumeroSerial"].ToString(),
                            Caja = dr["Caja"].ToString(),
                            Marca = dr["Marca"].ToString(),
                            NombreCompleto = dr["NombreCompleto"].ToString()
                        };
                    }
                }
            }
            return obj;
        }
    }
}
