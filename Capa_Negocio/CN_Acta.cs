using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CapaNegocio
{
    public class CN_Acta
    {
        private CD_Acta objcd_Acta = new CD_Acta();


        public bool RestarStock(int idEquipo, int cantidad) {
            return objcd_Acta.RestarStock(idEquipo, cantidad);
        }

        public bool SumarStock(int idEquipo, int cantidad) {
            return objcd_Acta.SumarStock(idEquipo, cantidad);
        }

        public int ObtenerCorrelativo(int idFarmacia)
        {
            return objcd_Acta.ObtenerCorrelativo(idFarmacia);
        }



        public string GenerarNumeroDocumento(int idFarmacia, string codigoFarmacia)
        {
            int correlativo = ObtenerCorrelativo(idFarmacia);
            return $"{codigoFarmacia}-{correlativo.ToString("D5")}";
        }

       
        public bool AutorizarBaja(string numeroDocumento, string codigoEquipo, string numeroSerial, int idUsuarioAutorizo)
        {
            return new CD_Acta().AutorizarBaja(numeroDocumento, codigoEquipo, numeroSerial, idUsuarioAutorizo);
        }

        public List<Detalle_Acta> ObtenerEquiposAutorizados()
        {
            return objcd_Acta.ObtenerEquiposAutorizados();
        }

        public bool EliminarEquipoAutorizado(string numeroDocumento, string codigoEquipo, string numeroSerial)
        {
            return objcd_Acta.EliminarEquipoAutorizado(numeroDocumento, codigoEquipo, numeroSerial);
        }


        public List<Detalle_Acta> ObtenerEquiposEnEspera()
        {
            return objcd_Acta.ObtenerEquiposEnEspera();
        }

        public bool Registrar(Acta obj, DataTable DetalleActa, out string Mensaje)
        {
            return objcd_Acta.Registrar(obj, DetalleActa, out Mensaje);
        }




        public Acta ObtenerActa(string numero)
        {
            Acta oActa = objcd_Acta.ObtenerActa(numero);

            if (oActa == null || oActa.IdActa == 0)
                throw new Exception("No se encontró ningún acta con ese número.");

            return oActa;
        }



        public bool MarcarEquipoComoEnEspera(string numeroDocumento, string codigoEquipo, string numeroSerial, string motivoBaja, int idUsuarioSolicita)
        {
            try
            {
                return objcd_Acta.MarcarEquipoComoEnEspera(numeroDocumento, codigoEquipo, numeroSerial, motivoBaja, idUsuarioSolicita);
            }
            catch (Exception ex)
            {
                // Manejo de error o log
                throw new Exception("Error en CN_Acta: " + ex.Message);
            }
        }
        public bool CambiarEstadoEquipo(string numeroDocumento, string codigoEquipo, string numeroSerial, string nuevoEstado, string motivo = "")
        {
            return objcd_Acta.CambiarEstadoEquipo(numeroDocumento, codigoEquipo, numeroSerial, nuevoEstado, motivo);
        }

        public bool LimpiarMotivoYEstado(string numeroDocumento, string codigoEquipo, string numeroSerial, out string mensaje)
        {
            return objcd_Acta.LimpiarMotivoYEstado(numeroDocumento, codigoEquipo, numeroSerial, out mensaje);
        }


        public List<Detalle_Acta> ObtenerEquiposPendientes()
        {
            return objcd_Acta.ObtenerEquiposPendientes();
        }

        public List<Detalle_Acta> ObtenerEquiposPorFarmacia(string codigoFarmacia)
        {
            return new CD_Acta().ObtenerEquiposPorFarmacia(codigoFarmacia);
        }

       


        public List<Detalle_Acta> ObtenerEquiposPorDocumento(string numeroDocumento)
        {
            try
            {
                List<Detalle_Acta> todos = ObtenerEquiposPendientes(); // Llama a tu método ya implementado
                return todos.Where(x => x.NumeroDocumento == numeroDocumento).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener equipos por documento: " + ex.Message);
            }
        }
        // Método público que la capa presentación puede llamar
        public bool AutorizarEquipoPendiente(string numeroDocumento, string codigoEquipo, string numeroSerial, int idUsuario, out string mensaje)
        {
            return new CD_Acta().AutorizarEquipoPendiente(numeroDocumento, codigoEquipo, numeroSerial, idUsuario, out mensaje);
        }

        // Método para autorizar todos los equipos de un documento
        public bool AutorizarTodosLosEquiposDelDocumento(string numeroDocumento, int idUsuario, out string mensaje)
        {
            mensaje = "";
            bool resultadoFinal = true;

            try
            {
                // Obtener todos los equipos del documento
                List<Detalle_Acta> equipos = new CN_Acta().ObtenerEquiposPorDocumento(numeroDocumento);

                foreach (var equipo in equipos)
                {
                    bool resultado = AutorizarEquipoPendiente(
                        equipo.NumeroDocumento,
                        equipo.oEquipo.Codigo,
                        equipo.NumeroSerial,
                        idUsuario,
                        out string msg
                    );

                    if (!resultado)
                    {
                        mensaje = $"Error al autorizar el equipo '{equipo.oEquipo.Codigo}' ({equipo.NumeroSerial}): {msg}";
                        resultadoFinal = false;
                        break;
                    }
                }

                if (resultadoFinal)
                {
                    mensaje = "Todos los equipos del documento fueron autorizados correctamente.";
                }
            }
            catch (Exception ex)
            {
                mensaje = "Error general al autorizar equipos: " + ex.Message;
                resultadoFinal = false;
            }

            return resultadoFinal;
        }




        public bool EliminarEquipoDeActa(string numeroDocumento, string codigoEquipo, string numeroSerial, out string mensaje)
        {
            return new CD_Acta().EliminarEquipoDeActa(numeroDocumento, codigoEquipo, numeroSerial, out mensaje);
        }

    }
}
