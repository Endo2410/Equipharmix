using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class CD_Prestamo
    {
        public List<Prestamo> ListarPrestamosAutorizados()
        {
            List<Prestamo> lista = new List<Prestamo>();

            try
            {
                using (SqlConnection cn = new SqlConnection(Conexion.cadena))
                using (SqlCommand cmd = new SqlCommand("SP_LISTAR_PRESTAMOS_AUTORIZADOS", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            string numeroDoc = dr["NumeroDocumento"].ToString();
                            Prestamo prestamo = lista.Find(p => p.NumeroDocumento == numeroDoc);

                            if (prestamo == null)
                            {
                                prestamo = new Prestamo
                                {
                                    NumeroDocumento = numeroDoc,
                                    FechaPrestamo = Convert.ToDateTime(dr["FechaPrestamo"]), // asegúrate que el alias coincida
                                    oFarmacia = new Farmacia { Nombre = dr["NombreFarmacia"].ToString() },
                                    oUsuario = new Usuario { NombreCompleto = dr["NombreCompleto"].ToString() },
                                    oDetalle = new List<Detalle_Prestamo>()
                                };
                                lista.Add(prestamo);
                            }

                            // Agregar detalle
                            Detalle_Prestamo detalle = new Detalle_Prestamo
                            {
                                oEquipo = new Equipo
                                {
                                    Codigo = dr["CodigoEquipo"].ToString(),
                                    Nombre = dr["NombreEquipo"].ToString(),
                                    oMarca = new Marca { Descripcion = dr["MarcaEquipo"].ToString() }
                                },
                                Cantidad = Convert.ToInt32(dr["Cantidad"]),
                                NumeroSerial = dr["NumeroSerial"].ToString(),
                                MotivoBaja = dr["MotivoBaja"] != DBNull.Value ? dr["MotivoBaja"].ToString() : "",
                                EstadoBaja = dr["EstadoBaja"] != DBNull.Value ? dr["EstadoBaja"].ToString() : ""
                            };

                            prestamo.oDetalle.Add(detalle);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar préstamos: " + ex.Message);
            }

            return lista;
        }


        public bool RegistrarPrestamo(Prestamo obj, DataTable detallePrestamo, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
                {
                    SqlCommand cmd = new SqlCommand("SP_REGISTRAR_PRESTAMO", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdUsuarioSolicita", obj.oUsuarioSolicita.IdUsuario);
                    cmd.Parameters.AddWithValue("@IdFarmacia", obj.IdFarmacia);
                    cmd.Parameters.AddWithValue("@TipoDocumento", obj.TipoDocumento);
                    cmd.Parameters.AddWithValue("@NumeroDocumento", obj.NumeroDocumento);
                    cmd.Parameters.AddWithValue("@DetallePrestamo", detallePrestamo);

                    SqlParameter resultadoParam = new SqlParameter("@Resultado", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                    SqlParameter mensajeParam = new SqlParameter("@Mensaje", SqlDbType.VarChar, 500) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(resultadoParam);
                    cmd.Parameters.Add(mensajeParam);

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(resultadoParam.Value);
                    mensaje = mensajeParam.Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;
            }

            return resultado;
        }

        // Obtener préstamos por número de documento
        public Prestamo ObtenerPrestamo(string numeroDocumento)
        {
            Prestamo obj = null;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
                {
                    SqlCommand cmd = new SqlCommand("SP_OBTENER_PRESTAMO_POR_NUMERO_DOCUMENTO", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NumeroDocumento", numeroDocumento);

                    oconexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            obj = new Prestamo()
                            {
                                IdPrestamo = Convert.ToInt32(dr["IdPrestamo"]),
                                NumeroDocumento = dr["NumeroDocumento"].ToString(),
                                TipoDocumento = dr["TipoDocumento"].ToString(),
                                FechaPrestamo = Convert.ToDateTime(dr["FechaPrestamo"]),
                                FechaDevolucion = dr["FechaDevolucion"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["FechaDevolucion"]),
                                EstadoPrestamo = dr["EstadoPrestamo"].ToString(),
                                oUsuarioSolicita = new Usuario() { NombreCompleto = dr["UsuarioSolicita"].ToString() },
                                oUsuarioAutoriza = new Usuario() { NombreCompleto = dr["UsuarioAutoriza"].ToString() },
                                oFarmacia = new Farmacia()
                                {
                                    Codigo = dr["CodigoFarmacia"].ToString(),
                                    Nombre = dr["NombreFarmacia"].ToString()
                                },
                                oDetalle = new List<Detalle_Prestamo>()
                            };
                        }

                        if (obj != null && dr.NextResult())
                        {
                            while (dr.Read())
                            {
                                obj.oDetalle.Add(new Detalle_Prestamo()
                                {
                                    IdDetallePrestamo = Convert.ToInt32(dr["IdDetallePrestamo"]),
                                    IdEquipo = Convert.ToInt32(dr["IdEquipo"]),
                                    Cantidad = Convert.ToInt32(dr["Cantidad"]),
                                    NumeroSerial = dr["NumeroSerial"]?.ToString() ?? "",
                                    oEquipo = new Equipo()
                                    {
                                        IdEquipo = Convert.ToInt32(dr["IdEquipo"]),
                                        Nombre = dr["NombreEquipo"]?.ToString() ?? "",
                                        oMarca = new Marca()
                                        {
                                            IdMarca = Convert.ToInt32(dr["IdMarca"]),
                                            Descripcion = dr["NombreMarca"]?.ToString() ?? ""
                                        }
                                    },
                                    EstadoDevolucion = dr["EstadoDevolucion"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch
            {
                obj = null;
            }

            return obj;
        }

        public string GenerarNumeroPrestamo(int idFarmacia, string codigoFarmacia)
        {
            int correlativo = 0;

            using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("SP_OBTENER_CORRELATIVO_PRESTAMO_POR_FARMACIA", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdFarmacia", idFarmacia);

                SqlParameter outputParam = new SqlParameter("@Correlativo", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputParam);

                cmd.ExecuteNonQuery();
                correlativo = Convert.ToInt32(outputParam.Value);
            }

            return $"{codigoFarmacia}-{correlativo.ToString("D5")}";
        }

        public List<Detalle_Prestamo> ObtenerPrestamosPendientes(string codigoFarmacia = null)
        {
            List<Detalle_Prestamo> lista = new List<Detalle_Prestamo>();

            using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("SP_OBTENER_PRESTAMOS_PENDIENTES_POR_FARMACIA", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodigoFarmacia", (object)codigoFarmacia ?? DBNull.Value);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Detalle_Prestamo
                            {
                                NumeroDocumento = dr["NumeroDocumento"].ToString(),
                                FechaRegistro = Convert.ToDateTime(dr["FechaRegistro"]),
                                Cantidad = Convert.ToInt32(dr["Cantidad"]),
                                NumeroSerial = dr["NumeroSerial"].ToString(),

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
                                        Descripcion = dr["MarcaEquipo"]?.ToString() ?? ""
                                    },
                                    oEstado = new Estado
                                    {
                                        Descripcion = dr["EstadoEquipo"].ToString()
                                    }
                                },

                                oFarmacia = new Farmacia
                                {
                                    Nombre = dr["NombreFarmacia"]?.ToString() ?? ""
                                },

                                EstadoAutorizacion = dr["EstadoAutorizacion"].ToString()
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener préstamos pendientes: " + ex.Message);
                }
            }

            return lista;
        }

        public void AutorizarPrestamo(string numeroDocumento, string codigoEquipo, string numeroSerial, int idUsuario, out bool resultado, out string mensaje)
        {
            resultado = false;
            mensaje = "";

            using (SqlConnection cn = new SqlConnection(Conexion.cadena))
            using (SqlCommand cmd = new SqlCommand("SP_AUTORIZAR_PRESTAMO_PENDIENTE", cn))
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
                    mensaje = "Error al autorizar préstamo: " + ex.Message;
                }
            }
        }

        public bool EliminarEquipoPrestamo(string numeroDocumento, string codigoEquipo, string numeroSerial, out string mensaje)
        {
            bool resultado = false;
            mensaje = "";

            try
            {
                using (SqlConnection cn = new SqlConnection(Conexion.cadena))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_ELIMINAR_EQUIPO_DE_PRESTAMO", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@NumeroDocumento", numeroDocumento);
                        cmd.Parameters.AddWithValue("@CodigoEquipo", codigoEquipo);
                        cmd.Parameters.AddWithValue("@NumeroSerial", numeroSerial);

                        cmd.Parameters.Add("@Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                        cn.Open();
                        cmd.ExecuteNonQuery();

                        resultado = Convert.ToBoolean(cmd.Parameters["@Resultado"].Value);
                        mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;
            }

            return resultado;
        }

        public bool DevolverEquipoPrestamo(string numeroDocumento, string codigoEquipo, string numeroSerial, int idUsuarioDevuelve,out string mensaje)
        {
            mensaje = string.Empty;
            bool resultado = false;

            try
            {
                using (SqlConnection cn = new SqlConnection(Conexion.cadena))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_DEVOLVER_EQUIPO_PRESTAMO", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@NumeroDocumento", numeroDocumento);
                        cmd.Parameters.AddWithValue("@CodigoEquipo", codigoEquipo);
                        cmd.Parameters.AddWithValue("@NumeroSerial", numeroSerial);
                        cmd.Parameters.AddWithValue("@IdUsuarioDevuelve", idUsuarioDevuelve);

                        SqlParameter parResultado = new SqlParameter("@Resultado", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                        SqlParameter parMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 500) { Direction = ParameterDirection.Output };

                        cmd.Parameters.Add(parResultado);
                        cmd.Parameters.Add(parMensaje);

                        cmd.ExecuteNonQuery();

                        resultado = Convert.ToBoolean(parResultado.Value);
                        mensaje = parMensaje.Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;
            }

            return resultado;
        }

        public bool MarcarPrestamoEquipoComoEnEspera(string numeroDocumento, string codigoEquipo, string numeroSerial, string motivo, int idUsuario)
        {
            using (SqlConnection con = new SqlConnection(Conexion.cadena))
            {
                SqlCommand cmd = new SqlCommand("SP_MARCAR_PRESTAMO_EN_ESPERA", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@NumeroDocumento", numeroDocumento);
                cmd.Parameters.AddWithValue("@CodigoEquipo", codigoEquipo);
                cmd.Parameters.AddWithValue("@NumeroSerial", numeroSerial);
                cmd.Parameters.AddWithValue("@MotivoBaja", motivo);
                cmd.Parameters.AddWithValue("@IdUsuarioSolicita", idUsuario);

                con.Open();
                int filas = Convert.ToInt32(cmd.ExecuteScalar());
                return filas > 0;
            }
        }

        public List<Detalle_Prestamo> ObtenerEquiposPrestamoEnEspera()
        {
            List<Detalle_Prestamo> lista = new List<Detalle_Prestamo>();

            using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("SP_OBTENER_EQUIPOS_PRESTAMO_EN_ESPERA", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Detalle_Prestamo
                            {
                                NumeroDocumento = dr["NumeroDocumento"].ToString(),

                                FechaPrestamo = Convert.ToDateTime(dr["FechaPrestamo"]),
                                FechaRegistro = Convert.ToDateTime(dr["FechaRegistro"]),

                                NumeroSerial = dr["NumeroSerial"].ToString(),
                                Cantidad = Convert.ToInt32(dr["Cantidad"]),
                                MotivoBaja = dr["MotivoBaja"].ToString(),
                                EstadoBaja = dr["EstadoBaja"].ToString(),

                                // Usuario solicitante
                                oUsuarioSolicitante = new Usuario()
                                {
                                    NombreCompleto = dr["UsuarioSolicitante"].ToString()
                                },

                                // Equipo
                                oEquipo = new Equipo()
                                {
                                    Codigo = dr["CodigoEquipo"].ToString(),
                                    Nombre = dr["NombreEquipo"].ToString(),

                                    oMarca = new Marca()
                                    {
                                        Descripcion = dr["Marca"].ToString()
                                    }
                                },

                                // Farmacia
                                oFarmacia = new Farmacia()
                                {
                                    Nombre = dr["NombreFarmacia"].ToString()
                                }
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener equipos de préstamo en espera: " + ex.Message);
                }
            }

            return lista;
        }

        public bool LimpiarMotivoYEstado(string numeroDocumento, string codigoEquipo, string numeroSerial, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;

            using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_LIMPIAR_ESTADO_Y_MOTIVO_PRESTAMO", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NumeroDocumento", numeroDocumento);
                    cmd.Parameters.AddWithValue("@CodigoEquipo", codigoEquipo);
                    cmd.Parameters.AddWithValue("@NumeroSerial", numeroSerial);

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

        public bool AutorizarBaja(string numeroDocumento, string codigoEquipo, string numeroSerial, int idUsuarioAutoriza)
        {
            using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_AUTORIZAR_EQUIPO_PRESTAMO", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NumeroDocumento", numeroDocumento);
                    cmd.Parameters.AddWithValue("@CodigoEquipo", codigoEquipo);
                    cmd.Parameters.AddWithValue("@NumeroSerial", numeroSerial);
                    cmd.Parameters.AddWithValue("@IdUsuarioAutoriza", idUsuarioAutoriza);

                    conexion.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }


        public List<Detalle_Prestamo> ObtenerEquiposPrestamoAutorizados()
        {
            List<Detalle_Prestamo> lista = new List<Detalle_Prestamo>();

            using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_OBTENER_EQUIPOS_PRESTAMO_AUTORIZADOS", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    conexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Detalle_Prestamo()
                            {
                                NumeroDocumento = dr["NumeroDocumento"].ToString(),
                                FechaRegistro = dr["FechaRegistro"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dr["FechaRegistro"]),
                                oFarmacia = new Farmacia() { Nombre = dr["NombreFarmacia"]?.ToString() ?? "" },
                                oEquipo = new Equipo()
                                {
                                    Codigo = dr["CodigoEquipo"].ToString(),
                                    Nombre = dr["NombreEquipo"].ToString(),
                                    oMarca = new Marca { Descripcion = dr["MarcaEquipo"]?.ToString() ?? "" },
                                },
                                Cantidad = dr["Cantidad"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Cantidad"]),
                                NumeroSerial = dr["NumeroSerial"]?.ToString() ?? "",
                                MotivoBaja = dr["MotivoBaja"]?.ToString() ?? "",
                                EstadoBaja = dr["EstadoBaja"]?.ToString() ?? "",
                                oUsuarioSolicitante = new Usuario { NombreCompleto = dr["UsuarioSolicitante"]?.ToString() ?? "" },
                                oUsuarioAutorizador = new Usuario { NombreCompleto = dr["UsuarioAutorizador"] == DBNull.Value ? "" : dr["UsuarioAutorizador"].ToString() }
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener equipos de préstamo autorizados", ex);
                }
            }

            return lista;
        }

    }
}
