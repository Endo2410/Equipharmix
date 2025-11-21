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
    public class CD_SubMenu
    {
        public List<SubMenu> Listar()
        {
            List<SubMenu> lista = new List<SubMenu>();

            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_LISTAR_SUB_MENU", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new SubMenu
                            {
                                IdSubMenu = Convert.ToInt32(dr["IdSubMenu"]),
                                IdModulo = Convert.ToInt32(dr["IdModulo"]),
                                NombreSubMenu = dr["NombreSubMenu"].ToString()
                            });
                        }
                    }
                }
                catch (Exception)
                {
                    lista = new List<SubMenu>();
                }
            }

            return lista;
        }

    }
}
