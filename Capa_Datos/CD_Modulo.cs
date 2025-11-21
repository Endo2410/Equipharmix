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
    public class CD_Modulo
    {
        public List<Modulo> Listar()
        {
            List<Modulo> lista = new List<Modulo>();

            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_LISTAR_MODULO", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Modulo
                            {
                                IdModulo = Convert.ToInt32(dr["IdModulo"]),
                                NombreModulo = dr["NombreModulo"].ToString()
                            });
                        }
                    }
                }
                catch
                {
                    lista = new List<Modulo>();
                }
            }

            return lista;
        }
    }
}
