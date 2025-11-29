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
    public class CD_Acta
    {

        public int ObtenerCorrelativo(int idFarmacia)
        {
            int correlativo = 1;

            using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
            {
                using (SqlCommand cmd = new SqlCommand("SP_OBTENER_CORRELATIVO_POR_FARMACIA", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdFarmacia", idFarmacia);

                    SqlParameter paramCorrelativo = new SqlParameter("@Correlativo", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(paramCorrelativo);

                    conexion.Open();
                    cmd.ExecuteNonQuery();

                    correlativo = (int)paramCorrelativo.Value;
                }
            }

            return correlativo;
        }

        public string GenerarNumeroDocumento(int idFarmacia, string codigoFarmacia)
        {
            int correlativo = ObtenerCorrelativo(idFarmacia);
            return $"{codigoFarmacia}-{correlativo.ToString("D5")}";
        }

        public bool RestarStock(int idEquipo, int cantidad)
        {
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                SqlCommand cmd = new SqlCommand("SP_RESTAR_STOCK", oconexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdEquipo", idEquipo);
                cmd.Parameters.AddWithValue("@Cantidad", cantidad);

                oconexion.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool SumarStock(int idEquipo, int cantidad)
        {
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                SqlCommand cmd = new SqlCommand("SP_SUMAR_STOCK", oconexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdEquipo", idEquipo);
                cmd.Parameters.AddWithValue("@Cantidad", cantidad);

                oconexion.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }


        public bool Registrar(Acta obj, DataTable DetalleActa, out string Mensaje)
        {
            bool Respuesta = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
                {
                    SqlCommand cmd = new SqlCommand("SP_REGISTRAR_ACTA", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parámetros normales
                    cmd.Parameters.AddWithValue("IdUsuario", obj.oUsuario.IdUsuario);
                    cmd.Parameters.AddWithValue("TipoDocumento", obj.TipoDocumento);
                    cmd.Parameters.AddWithValue("NumeroDocumento", obj.NumeroDocumento);
                    cmd.Parameters.AddWithValue("Codigo", obj.Codigo); // Código de la farmacia

                    // Parámetro tipo tabla
                    SqlParameter detalleParam = cmd.Parameters.AddWithValue("DetalleActa", DetalleActa);
                    detalleParam.SqlDbType = SqlDbType.Structured;
                    detalleParam.TypeName = "PDetalle_Acta"; // ← Nombre del tipo definido en SQL Server

                    // Parámetros de salida
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    // Recuperar resultados
                    Respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                Respuesta = false;
                Mensaje = ex.Message;
            }

            return Respuesta;
        }

        public Acta ObtenerActa(string numeroDocumento)
        {
            Acta obj = null;

            using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
            {
                conexion.Open();
                using (SqlCommand cmd = new SqlCommand("SP_OBTENER_ACTA_POR_NUMERO_DOCUMENTO", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NumeroDocumento", numeroDocumento);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        // Primer result set: datos generales del acta
                        if (dr.Read())
                        {
                            obj = new Acta()
                            {
                                IdActa = Convert.ToInt32(dr["IdActa"]),
                                oUsuario = new Usuario()
                                {
                                    IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                                    NombreCompleto = dr["Usuario"]?.ToString() ?? "" // Asegúrate que el alias sea "Usuario" como en el SP
                                },
                                TipoDocumento = dr["TipoDocumento"]?.ToString() ?? "",
                                NumeroDocumento = dr["NumeroDocumento"]?.ToString() ?? "",
                                IdFarmacia = Convert.ToInt32(dr["IdFarmacia"]),
                                Codigo = dr["CodigoFarmacia"]?.ToString() ?? "",
                                Nombre = dr["NombreFarmacia"]?.ToString() ?? "",
                                FechaRegistro = Convert.ToDateTime(dr["FechaRegistro"]).ToString("dd/MM/yyyy"),
                                EstadoAutorizacion = dr["EstadoAutorizacion"]?.ToString() ?? "",
                                oDetalle_Acta = new List<Detalle_Acta>()
                            };
                        }

                        // Segundo result set: detalles del acta
                        if (obj != null && dr.NextResult())
                        {
                            while (dr.Read())
                            {
                                Detalle_Acta detalle = new Detalle_Acta()
                                {
                                    IdDetalleActa = Convert.ToInt32(dr["IdDetalleActa"]),
                                    IdActa = Convert.ToInt32(dr["IdActa"]),
                                    Cantidad = Convert.ToInt32(dr["Cantidad"]),
                                    NumeroSerial = dr["NumeroSerial"]?.ToString() ?? "",
                                    Caja = dr["Caja"]?.ToString() ?? "",
                                    oEquipo = new Equipo()
                                    {
                                        IdEquipo = Convert.ToInt32(dr["IdEquipo"]),
                                        Nombre = dr["NombreEquipo"]?.ToString() ?? "",
                                        oMarca = new Marca()
                                        {
                                            IdMarca = Convert.ToInt32(dr["IdMarca"]),
                                            Descripcion = dr["NombreMarca"]?.ToString() ?? ""
                                        }
                                    }
                                };

                                obj.oDetalle_Acta.Add(detalle);
                            }
                        }
                    }
                }
            }

            return obj;
        }


        public List<Detalle_Acta> ObtenerEquiposPorFarmacia(string codigoFarmacia)
        {
            var lista = new List<Detalle_Acta>();

            using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    conexion.Open();

                    using (SqlCommand cmd = new SqlCommand("SP_OBTENER_EQUIPOS_POR_FARMACIA", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CodigoFarmacia", codigoFarmacia);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                string estadoBaja = dr["EstadoBaja"] == DBNull.Value ? "" : dr["EstadoBaja"].ToString();
                                string estadoAutorizacion = dr["EstadoAutorizacion"] == DBNull.Value ? "" : dr["EstadoAutorizacion"].ToString();

                                if (!string.Equals(estadoBaja, "Autorizado", StringComparison.OrdinalIgnoreCase) &&
                                    !string.Equals(estadoAutorizacion, "PENDIENTE", StringComparison.OrdinalIgnoreCase))
                                {
                                    var detalle = new Detalle_Acta
                                    {
                                        NumeroDocumento = dr["NumeroDocumento"]?.ToString() ?? "",
                                        FechaRegistro = dr["FechaRegistro"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dr["FechaRegistro"]),
                                        oEquipo = new Equipo
                                        {
                                            Codigo = dr["CodigoEquipo"]?.ToString() ?? "",
                                            Nombre = dr["NombreEquipo"]?.ToString() ?? "",
                                            oMarca = new Marca
                                            {
                                                Descripcion = dr["MarcaEquipo"]?.ToString() ?? "" // ← Aquí se asigna la marca
                                            },
                                            oEstado = new Estado
                                            {
                                                Descripcion = dr["EstadoEquipo"]?.ToString() ?? ""
                                            }
                                        },
                                        Cantidad = dr["Cantidad"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Cantidad"]),
                                        NumeroSerial = dr["NumeroSerial"]?.ToString() ?? "",
                                        Caja = dr["Caja"]?.ToString() ?? "",
                                        MotivoBaja = dr["MotivoBaja"]?.ToString() ?? "",
                                        EstadoBaja = estadoBaja
                                    };

                                    lista.Add(detalle);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al obtener equipos por farmacia: " + ex.Message);
                    lista.Clear();
                }
            }

            return lista;
        }


        public bool CambiarEstadoEquipo(string numeroDocumento, string codigoEquipo, string numeroSerial, string nuevoEstado, string motivo)
        {
            bool respuesta = false;

            using (SqlConnection con = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_CAMBIAR_ESTADO_EQUIPO", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NumeroDocumento", numeroDocumento);
                    cmd.Parameters.AddWithValue("@CodigoEquipo", codigoEquipo);
                    cmd.Parameters.AddWithValue("@NumeroSerial", numeroSerial);
                    cmd.Parameters.AddWithValue("@NuevoEstado", nuevoEstado);
                    cmd.Parameters.AddWithValue("@MotivoBaja", motivo);

                    con.Open();
                    int filasAfectadas = cmd.ExecuteNonQuery();
                    respuesta = true; // Si llega aquí, el SP se ejecutó bien, sin importar si afectó filas o no
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al cambiar el estado del equipo", ex);
                }
            }

            return respuesta;
        }

        public bool AutorizarBaja(string numeroDocumento, string codigoEquipo, string numeroSerial, int idUsuarioAutorizo)
        {
            using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("SP_AUTORIZAR_EQUIPO", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NumeroDocumento", numeroDocumento);
                    cmd.Parameters.AddWithValue("@CodigoEquipo", codigoEquipo);
                    cmd.Parameters.AddWithValue("@NumeroSerial", numeroSerial);
                    cmd.Parameters.AddWithValue("@IdUsuarioAutorizo", idUsuarioAutorizo);

                    int filasAfectadas = cmd.ExecuteNonQuery();

                    return true;
                }
                catch (Exception ex)
                {
                    // Puedes registrar el error
                    return false;
                }
            }
        }

        public List<Detalle_Acta> ObtenerEquiposPendientes()
        {
            List<Detalle_Acta> lista = new List<Detalle_Acta>();

            using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("SP_OBTENER_EQUIPOS_PENDIENTES_POR_FARMACIA", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Detalle_Acta
                            {
                                NumeroDocumento = dr["NumeroDocumento"].ToString(),
                                FechaRegistro = Convert.ToDateTime(dr["FechaRegistro"]),
                                Cantidad = Convert.ToInt32(dr["Cantidad"]),
                                NumeroSerial = dr["NumeroSerial"].ToString(),
                                Caja = dr["Caja"].ToString(),
                                EstadoAutorizacion = dr["EstadoAutorizacion"].ToString(),

                                oUsuarioSolicitante = new Usuario
                                {
                                    IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                                    NombreCompleto = dr["CreadorActa"].ToString()
                                },

                                oEquipo = new Equipo
                                {
                                    Codigo = dr["CodigoEquipo"].ToString(),
                                    Nombre = dr["NombreEquipo"].ToString(),
                                    oMarca = new Marca
                                    {
                                        Descripcion = dr["MarcaEquipo"]?.ToString() ?? "" // ← Aquí se asigna la marca
                                    },
                                    oEstado = new Estado
                                    {
                                        Descripcion = dr["EstadoEquipo"].ToString()
                                    }
                                },

                                oFarmacia = new Farmacia
                                {
                                    Nombre = dr["NombreFarmacia"].ToString()
                                }
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener equipos pendientes: " + ex.Message);
                }
            }

            return lista;
        }

        public List<Detalle_Acta> ObtenerEquiposEnEspera()
        {
            List<Detalle_Acta> lista = new List<Detalle_Acta>();

            using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("SP_OBTENER_EQUIPOS_EN_ESPERA", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Detalle_Acta
                            {
                                NumeroDocumento = dr["NumeroDocumento"].ToString(),
                                FechaRegistro = Convert.ToDateTime(dr["FechaRegistro"]),
                                NumeroSerial = dr["NumeroSerial"].ToString(),
                                Caja = dr["Caja"].ToString(),
                                Cantidad = Convert.ToInt32(dr["Cantidad"]),
                                MotivoBaja = dr["MotivoBaja"].ToString(),
                                EstadoBaja = dr["EstadoBaja"].ToString(),
                                oUsuarioSolicitante = new Usuario() { NombreCompleto = dr["UsuarioSolicitante"].ToString() },

                                oEquipo = new Equipo
                                {
                                    Codigo = dr["CodigoEquipo"].ToString(),
                                    Nombre = dr["NombreEquipo"].ToString(),
                                    oMarca = new Marca
                                    {
                                        Descripcion = dr["MarcaEquipo"]?.ToString() ?? "" // ← Aquí se asigna la marca
                                    }
                                },

                                oFarmacia = new Farmacia
                                {
                                    Nombre = dr["NombreFarmacia"].ToString()
                                }
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener equipos en espera: " + ex.Message);
                }
            }

            return lista;
        }
        
        public bool MarcarEquipoComoEnEspera(string numeroDocumento, string codigoEquipo, string numeroSerial, string motivoBaja, int idUsuarioSolicita)
        {
            bool resultado = false;

            using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_MARCAR_EQUIPO_EN_ESPERA", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NumeroDocumento", numeroDocumento);
                    cmd.Parameters.AddWithValue("@CodigoEquipo", codigoEquipo);
                    cmd.Parameters.AddWithValue("@NumeroSerial", numeroSerial);
                    cmd.Parameters.AddWithValue("@MotivoBaja", motivoBaja);
                    cmd.Parameters.AddWithValue("@IdUsuarioSolicita", idUsuarioSolicita);

                    conexion.Open();
                    int filasAfectadas = cmd.ExecuteNonQuery();

                    resultado = true;

                }
                catch (Exception ex)
                {
                    // Aquí puedes registrar el error o lanzar la excepción
                    throw new Exception("Error al marcar equipo en espera: " + ex.Message);
                }
            }

            return resultado;
        }

        public bool AutorizarEquipoPendiente(string numeroDocumento, string codigoEquipo, string numeroSerial, int idUsuario, out string mensaje)
        {
            bool resultado = false;
            mensaje = "";

            using (SqlConnection cn = new SqlConnection(Conexion.cadena))
            using (SqlCommand cmd = new SqlCommand("SP_AUTORIZAR_EQUIPO_PENDIENTE", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@NumeroDocumento", numeroDocumento);
                cmd.Parameters.AddWithValue("@CodigoEquipo", codigoEquipo);
                cmd.Parameters.AddWithValue("@NumeroSerial", numeroSerial);
                cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);

                SqlParameter paramResultado = new SqlParameter("@Resultado", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                SqlParameter paramMensaje = new SqlParameter("@Mensaje", SqlDbType.VarChar, 500) { Direction = ParameterDirection.Output };

                cmd.Parameters.Add(paramResultado);
                cmd.Parameters.Add(paramMensaje);

                try
                {
                    cn.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(paramResultado.Value);
                    mensaje = paramMensaje.Value.ToString();
                }
                catch (Exception ex)
                {
                    resultado = false;
                    mensaje = "Error al autorizar equipo: " + ex.Message;
                }
            }

            return resultado;
        }


        public bool EliminarEquipoDeActa(string numeroDocumento, string codigoEquipo, string numeroSerial, out string mensaje)
        {
            bool resultado = false;
            mensaje = "";

            using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_ELIMINAR_EQUIPO_DE_ACTA", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NumeroDocumento", numeroDocumento);
                    cmd.Parameters.AddWithValue("@CodigoEquipo", codigoEquipo);
                    cmd.Parameters.AddWithValue("@NumeroSerial", numeroSerial);

                    SqlParameter resultadoParam = new SqlParameter("@Resultado", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                    SqlParameter mensajeParam = new SqlParameter("@Mensaje", SqlDbType.VarChar, 500) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(resultadoParam);
                    cmd.Parameters.Add(mensajeParam);

                    conexion.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(resultadoParam.Value);
                    mensaje = mensajeParam.Value.ToString();
                }
                catch (Exception ex)
                {
                    resultado = false;
                    mensaje = "Error al eliminar equipo: " + ex.Message;
                }
            }

            return resultado;
        }




        public List<Detalle_Acta> ObtenerDetalleActa(int idActa)
        {
            List<Detalle_Acta> oLista = new List<Detalle_Acta>();

            using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_OBTENER_DETALLE_ACTA", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdActa", idActa);

                    conexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            oLista.Add(new Detalle_Acta()
                            {
                                oEquipo = new Equipo() { Nombre = dr["Nombre"].ToString() },
                                Cantidad = Convert.ToInt32(dr["Cantidad"]),
                                NumeroSerial = dr["NumeroSerial"].ToString(),
                                Caja = dr["Caja"].ToString(),
                            });
                        }
                    }
                }
                catch
                {
                    oLista = new List<Detalle_Acta>();
                }
            }

            return oLista;
        }



        public List<Detalle_Acta> ObtenerEquiposAutorizados()
        {
                List<Detalle_Acta> lista = new List<Detalle_Acta>();

                using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
                {
                    try
                    {
                        SqlCommand cmd = new SqlCommand("SP_OBTENER_EQUIPOS_AUTORIZADOS", conexion);
                        cmd.CommandType = CommandType.StoredProcedure;
                        conexion.Open();

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                lista.Add(new Detalle_Acta()
                                {
                                    NumeroDocumento = dr["NumeroDocumento"].ToString(),
                                    FechaRegistro = Convert.ToDateTime(dr["FechaRegistro"]),
                                    oFarmacia = new Farmacia() { Nombre = dr["NombreFarmacia"].ToString() },
                                    oEquipo = new Equipo()
                                    {
                                        Codigo = dr["CodigoEquipo"].ToString(),
                                        Nombre = dr["NombreEquipo"].ToString(),
                                        oMarca = new Marca
                                        {
                                            Descripcion = dr["MarcaEquipo"]?.ToString() ?? "" // ← Aquí se asigna la marca
                                        }
                                    },
                                    Cantidad = Convert.ToInt32(dr["Cantidad"]),
                                    NumeroSerial = dr["NumeroSerial"].ToString(),
                                    Caja = dr["Caja"].ToString(),
                                    MotivoBaja = dr["MotivoBaja"].ToString(),
                                    EstadoBaja = dr["EstadoBaja"].ToString(),
                                    oUsuarioSolicitante = new Usuario() { NombreCompleto = dr["UsuarioSolicitante"].ToString() },
                                    oUsuarioAutorizador = new Usuario() { NombreCompleto = dr["UsuarioAutorizador"] == DBNull.Value ? "" : dr["UsuarioAutorizador"].ToString() }
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Manejar excepción o relanzar
                        throw new Exception("Error al obtener equipos autorizados", ex);
                    }
                }
                return lista;
        }

        public bool EliminarEquipoAutorizado(string numeroDocumento, string codigoEquipo, string numeroSerial)
        {
            bool resultado = false;

            using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_ELIMINAR_EQUIPO_AUTORIZADO", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NumeroDocumento", numeroDocumento);
                    cmd.Parameters.AddWithValue("@CodigoEquipo", codigoEquipo);
                    cmd.Parameters.AddWithValue("@NumeroSerial", numeroSerial);

                    conexion.Open();
                    cmd.ExecuteNonQuery();
                    resultado = true;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error en CD_Acta -> EliminarEquipoAutorizado: " + ex.Message);
                }
            }

            return resultado;
        }

        public bool LimpiarMotivoYEstado(string numeroDocumento, string codigoEquipo, string numeroSerial, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;

            using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_LIMPIAR_ESTADO_Y_MOTIVO", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NumeroDocumento", numeroDocumento);
                    cmd.Parameters.AddWithValue("@CodigoEquipo", codigoEquipo);
                    cmd.Parameters.AddWithValue("@NumeroSerial", numeroSerial);

                    // Parámetros OUTPUT
                    SqlParameter paramResultado = new SqlParameter("@Resultado", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                    SqlParameter paramMensaje = new SqlParameter("@Mensaje", SqlDbType.VarChar, 500) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(paramResultado);
                    cmd.Parameters.Add(paramMensaje);

                    conexion.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(paramResultado.Value);
                    mensaje = paramMensaje.Value.ToString();
                }
                catch (Exception ex)
                {
                    resultado = false;
                    mensaje = "Error al ejecutar procedimiento: " + ex.Message;
                }
            }

            return resultado;
        }

    }
}
