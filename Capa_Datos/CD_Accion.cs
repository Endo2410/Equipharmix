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
    public class CD_Accion
    {
        public List<Accion> Listar()
        {
            List<Accion> lista = new List<Accion>();

            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_LISTAR_ACCION", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Accion()
                            {
                                IdAccion = Convert.ToInt32(dr["IdAccion"]),
                                IdSubMenu = dr["IdSubMenu"] != DBNull.Value ? Convert.ToInt32(dr["IdSubMenu"]) : (int?)null,
                                IdModulo = dr["IdModulo"] != DBNull.Value ? Convert.ToInt32(dr["IdModulo"]) : (int?)null,
                                NombreAccion = dr["NombreAccion"].ToString()
                            });
                        }
                    }
                }
                catch
                {
                    lista = new List<Accion>();
                }
            }

            return lista;
        }
    }
}
