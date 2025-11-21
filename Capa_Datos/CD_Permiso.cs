using CapaEntidad;
using CapaPresentacion.Utilidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class CD_Permiso
    {
        // Listar permisos por usuario usando SP
        public List<Permiso> Listar(int idUsuario)
        {
            List<Permiso> lista = new List<Permiso>();

            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_LISTAR_PERMISOS_POR_USUARIO", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Permiso
                            {
                                IdPermiso = Convert.ToInt32(dr["IdPermiso"]),
                                oRol = new Rol { IdRol = Convert.ToInt32(dr["IdRol"]), Descripcion = dr["Rol"].ToString() },
                                oModulo = dr["IdModulo"] != DBNull.Value ? new Modulo
                                {
                                    IdModulo = Convert.ToInt32(dr["IdModulo"]),
                                    NombreModulo = dr["NombreModulo"].ToString()
                                } : null,
                                oSubMenu = dr["IdSubMenu"] != DBNull.Value ? new SubMenu
                                {
                                    IdSubMenu = Convert.ToInt32(dr["IdSubMenu"]),
                                    NombreSubMenu = dr["NombreSubMenu"].ToString()
                                } : null,
                                oAccion = dr["IdAccion"] != DBNull.Value ? new Accion
                                {
                                    IdAccion = Convert.ToInt32(dr["IdAccion"]),
                                    NombreAccion = dr["NombreAccion"].ToString()
                                } : null
                            });
                        }
                    }
                }
                catch
                {
                    lista = new List<Permiso>();
                }
            }

            return lista;
        }

        // Listar permisos por rol usando SP
        public List<Permiso> ListarPorRol(int idRol)
        {
            List<Permiso> lista = new List<Permiso>();

            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_LISTAR_PERMISOS_POR_ROL", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdRol", idRol);

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Permiso
                            {
                                IdPermiso = Convert.ToInt32(dr["IdPermiso"]),
                                oRol = new Rol { IdRol = Convert.ToInt32(dr["IdRol"]) },
                                oModulo = dr["IdModulo"] != DBNull.Value ? new Modulo
                                {
                                    IdModulo = Convert.ToInt32(dr["IdModulo"]),
                                    NombreModulo = dr["NombreModulo"].ToString()
                                } : null,
                                oSubMenu = dr["IdSubMenu"] != DBNull.Value ? new SubMenu
                                {
                                    IdSubMenu = Convert.ToInt32(dr["IdSubMenu"]),
                                    NombreSubMenu = dr["NombreSubMenu"].ToString()
                                } : null,
                                oAccion = dr["IdAccion"] != DBNull.Value ? new Accion
                                {
                                    IdAccion = Convert.ToInt32(dr["IdAccion"]),
                                    NombreAccion = dr["NombreAccion"].ToString()
                                } : null
                            });
                        }
                    }
                }
                catch
                {
                    lista = new List<Permiso>();
                }
            }

            return lista;
        }

        // Guardar permisos usando SP y tipo tabla
        public bool GuardarPermisos(int idRol, List<Permiso> listaPermisos)
        {
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {

                    oconexion.Open();

                    // Guardar usuario logueado en SESSION_CONTEXT
                    using (SqlCommand cmdt = new SqlCommand("EXEC sp_set_session_context @key, @value", oconexion))
                    {
                        cmdt.Parameters.AddWithValue("@key", "Usuario");
                        cmdt.Parameters.AddWithValue("@value", UsuarioSesion.NombreCompleto);
                        cmdt.ExecuteNonQuery();
                    }

                    SqlCommand cmd = new SqlCommand("SP_GUARDAR_PERMISOS_COMPLETO", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdRol", idRol);

                    // Crear DataTable para pasar como TVP
                    DataTable dtPermisos = new DataTable();
                    dtPermisos.Columns.Add("IdModulo", typeof(int));
                    dtPermisos.Columns.Add("IdSubMenu", typeof(int));
                    dtPermisos.Columns.Add("IdAccion", typeof(int));

                    foreach (var p in listaPermisos)
                    {
                        dtPermisos.Rows.Add(
                            p.oModulo?.IdModulo ?? 0,
                            (object)p.oSubMenu?.IdSubMenu ?? DBNull.Value,
                            (object)p.oAccion?.IdAccion ?? DBNull.Value
                        );
                    }

                    SqlParameter tvpParam = cmd.Parameters.AddWithValue("@Permisos", dtPermisos);
                    tvpParam.SqlDbType = SqlDbType.Structured;
                    tvpParam.TypeName = "TipoPermiso"; 

                    cmd.ExecuteNonQuery();

                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}
