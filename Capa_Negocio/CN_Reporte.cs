using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Reporte
    {
        private CD_Reporte objcd_reporte = new CD_Reporte();

        public List<ActaReporte> ReporteActas(string fechainicio, string fechafin)
        {
            return objcd_reporte.ReporteActas(fechainicio, fechafin);
        }


        public List<ReporteRegistrar> Registrar(string fechainicio, string fechafin)
        {
            return objcd_reporte.Registrar(fechainicio, fechafin);
        }

        public List<Auditoria> ObtenerAuditoria(string tabla, string fechainicio, string fechafin)
        {
            return objcd_reporte.ObtenerAuditoria(tabla, fechainicio, fechafin);
        }
    }
}
