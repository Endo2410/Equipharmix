using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CapaNegocio
{
    public class CN_Prestamo
    {
        private CD_Prestamo objcd = new CD_Prestamo();


        public bool RegistrarPrestamo(Prestamo obj, DataTable detallePrestamo, out string mensaje)
        {
            return objcd.RegistrarPrestamo(obj, detallePrestamo, out mensaje);
        }

        public Prestamo ObtenerPrestamo(string numeroDocumento)
        {
            return objcd.ObtenerPrestamo(numeroDocumento);
        }

        public string GenerarNumeroPrestamo(int idFarmacia, string codigoFarmacia)
        {
            return objcd.GenerarNumeroPrestamo(idFarmacia, codigoFarmacia);
        }

        public List<Detalle_Prestamo> ObtenerPrestamosPendientes(string codigoFarmacia = null)
        {
            return objcd.ObtenerPrestamosPendientes(codigoFarmacia);
        }

        public List<Detalle_Prestamo> ObtenerEquiposPorDocumento(string numeroDocumento)
        {
            try
            {
                List<Detalle_Prestamo> todos = ObtenerPrestamosPendientes(); // Llama a tu método ya implementado
                return todos.Where(x => x.NumeroDocumento == numeroDocumento).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener equipos por documento: " + ex.Message);
            }
        }

        public void AutorizarPrestamo(string numeroDocumento, string codigoEquipo, string numeroSerial, int idUsuario, out bool resultado, out string mensaje)
        {

           objcd.AutorizarPrestamo(numeroDocumento, codigoEquipo, numeroSerial, idUsuario, out resultado, out mensaje);
      
        }

        public bool EliminarEquipoPrestamo(string numeroDocumento, string codigoEquipo, string numeroSerial, out string mensaje)
        {
            return objcd.EliminarEquipoPrestamo(numeroDocumento, codigoEquipo, numeroSerial, out mensaje);
        }

        public List<Prestamo> ListarPrestamosAutorizados()
        {
            return objcd.ListarPrestamosAutorizados();
        }

        public bool DevolverEquipoPrestamo(string numeroDocumento, string codigoEquipo,string numeroSerial, int idUsuarioDevuelve,out string mensaje)
        {
           
            return objcd.DevolverEquipoPrestamo(numeroDocumento, codigoEquipo, numeroSerial, idUsuarioDevuelve,  out mensaje);
           
        }
    }
}
