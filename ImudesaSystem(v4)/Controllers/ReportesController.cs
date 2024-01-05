using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ImudesaSystem_v4_.Models;
using ImudesaSystem_v4_.Models.Clases;
using Microsoft.Reporting.WebForms;

/*******Se instala el rdcl en extensiones y actualizaciones*************/
/*******Se instala el reportviewercontrol web y form en paquete nugets*********/

namespace ImudesaSystem_v3_.Controllers
{
    public class ReportesController : Controller
    {
        String fechaactual = System.DateTime.Now.ToString("dd-MM-yyyy");

        //public ActionResult ReporteVisitantesxFiltro(String fechai, String fechaf, String nombre, String dni, String destino, String empresa_proveniente)
        //{
        //    visitante_mantenimiento ma = new visitante_mantenimiento();
        //    LocalReport localreport = new LocalReport();
        //    localreport.ReportPath = Server.MapPath("~/Plantillas/FormatoReportes/ReportListarVisitasxFiltro.rdlc");

        //    ReportParameter[] parameters = new ReportParameter[6];
        //    parameters[0] = new ReportParameter("fechai",fechai);
        //    parameters[1] = new ReportParameter("fechaf", fechaf);
        //    parameters[2] = new ReportParameter("nombre",nombre);
        //    parameters[3] = new ReportParameter("dni",dni);
        //    parameters[4] = new ReportParameter("proveedor", empresa_proveniente);
        //    parameters[5] = new ReportParameter("destino", destino);
        //    localreport.SetParameters(parameters);

        //    ReportDataSource reportDataSource = new ReportDataSource();
        //    reportDataSource.Name = "DataSetVisitasxFiltrosN";
        //    reportDataSource.Value =ma.getVisitantesFiltradasForExcel(fechai,fechaf,nombre,dni,destino,empresa_proveniente);
        //    localreport.DataSources.Add(reportDataSource);

        //    String reportType = "Excel";
        //    String mimeType;
        //    String encoding;
        //    String fileNameExtension = "xlsx";
        //    String[] streams;
        //    // String[] streams={nombre,encargada,destino,fechai,fechaf,dni};
        //    Warning[] warnings;
        //    byte[] renderedByte;
        //    renderedByte = localreport.Render(reportType, "", out mimeType, out encoding, out fileNameExtension,
        //        out streams, out warnings);

        //    Response.AddHeader("content-disposition", "attachment;filename= VisitasFiltradas_report("+fechaactual+")." + fileNameExtension);
        //    return File(renderedByte, fileNameExtension);
        //}
        /*******************************************************************************************************/
        /**********************************VISITAS**************************************************************/
        public ActionResult ReporteVisitasHoy()
        {
            try
            {
                movimiento_mantenimiento mm = new movimiento_mantenimiento();
                LocalReport localreport = new LocalReport();
                localreport.ReportPath = Server.MapPath("~/Plantillas/DiseñoReportes/Report_VisitasDelDia.rdlc");

                ReportDataSource reportDataSource = new ReportDataSource();
                reportDataSource.Name = "movimientosHoy";
                DataTable dt = mm.get_movimientosHoy("v"); ;

                reportDataSource.Value = dt;

                localreport.DataSources.Add(reportDataSource);

                String reportType = "Excel";
                //String reportType = "PDF";

                String mimeType;
                String encoding;
                String fileNameExtension = "xlsx";
                //String fileNameExtension = "pdf";

                String[] streams;
                // String[] streams={nombre,encargada,destino,fechai,fechaf,dni};
                Warning[] warnings;
                byte[] renderedByte;
                renderedByte = localreport.Render(reportType, "", out mimeType, out encoding, out fileNameExtension,
                    out streams, out warnings);

                Response.AddHeader("content-disposition", "attachment;filename= VisitasDelDia_report(" + fechaactual + ")." + fileNameExtension);
                return File(renderedByte, fileNameExtension);
            }catch(Exception e){
               return Content("Error:"+e.Message);
            }
            
        }



        public ActionResult ReporteVisitasFiltros(string fechadesde, string fechahasta,string nombre,
            string dni,string destino,string proveedor)
        {
            EntidadBusqueda entidad = new EntidadBusqueda
            {
                fechadesde = fechadesde,
                fechahasta = fechahasta,
                nombre = nombre,
                dni = dni,
                destino = destino,
                proveedor = proveedor
            };



            movimiento_mantenimiento mm = new movimiento_mantenimiento();
            LocalReport localreport = new LocalReport();
            localreport.ReportPath = Server.MapPath("~/Plantillas/DiseñoReportes/Report_VisitasFiltradas.rdlc");


            ReportParameter[] parameters = new ReportParameter[6];
            parameters[0] = new ReportParameter("fechai", entidad.fechadesde);
            parameters[1] = new ReportParameter("fechaf", entidad.fechahasta);
            parameters[2] = new ReportParameter("nombre", entidad.nombre);
            parameters[3] = new ReportParameter("dni", entidad.dni);
            parameters[4] = new ReportParameter("destino", entidad.destino);
            parameters[5] = new ReportParameter("proveedor", entidad.proveedor);


            localreport.SetParameters(parameters);

            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "visitasxFiltros";
            DataTable dt = mm.getVisitantesFiltradas(entidad);
            reportDataSource.Value = dt;
            localreport.DataSources.Add(reportDataSource);

            String reportType = "Excel";
            //String reportType = "PDF";

            String mimeType;
            String encoding;
            String fileNameExtension = "xlsx";
            //String fileNameExtension = "pdf";
            String[] streams;
            //String[] streams={entidad.fechadesde,entidad.fechahasta,entidad.nombre,
            //                     entidad.dni,entidad.destino,entidad.proveedor};
            Warning[] warnings;
            byte[] renderedByte;
            renderedByte = localreport.Render(reportType, "", out mimeType, out encoding, out fileNameExtension,
                out streams, out warnings);

            Response.AddHeader("content-disposition", "attachment;filename= visitasFiltradas_report(" + fechaactual + ")." + fileNameExtension);
            return File(renderedByte, fileNameExtension);
        }
        /**************************************************************************/
        /**********************************INDESEADOS*************************************************/
        public ActionResult ReporteIndeseados()
        {
            indeseado_mantenimiento ma = new indeseado_mantenimiento();
            LocalReport localreport = new LocalReport();
            localreport.ReportPath = Server.MapPath("~/Plantillas/DiseñoReportes/Report_Indeseados.rdlc");

            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "ListaNegra";
            DataTable dt = ma.getIndeseados();
            reportDataSource.Value = dt;
            localreport.DataSources.Add(reportDataSource);

            String reportType = "Excel";
            //String reportType = "PDF";

            String mimeType;
            String encoding;
            String fileNameExtension = "xlsx";
            //String fileNameExtension = "pdf";

            String[] streams;
            // String[] streams={nombre,encargada,destino,fechai,fechaf,dni};
            Warning[] warnings;
            byte[] renderedByte;
            renderedByte = localreport.Render(reportType, "", out mimeType, out encoding, out fileNameExtension,
                out streams, out warnings);

            Response.AddHeader("content-disposition", "attachment;filename= Indeseados_report(" + fechaactual + ")." + fileNameExtension);
            return File(renderedByte, fileNameExtension);
        }
        /*********************************************************/
        /*********************TRABAJADORES******************************/
        public ActionResult reporteTrabajadores()
        {
            trabajador_mantenimiento ma = new trabajador_mantenimiento();
            LocalReport localreport = new LocalReport();
            localreport.ReportPath = Server.MapPath("~/Plantillas/DiseñoReportes/Report_TrabajadoresGarita.rdlc");
            /***********************************************/

            /**************************************************/
            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "DataSetTrabajadoresGarita";
            DataTable dt = ma.get_trabajadores();
            reportDataSource.Value =dt;
            localreport.DataSources.Add(reportDataSource);

            String reportType = "Excel";
            //String reportType = "PDF";

            String mimeType;
            String encoding;
            String fileNameExtension = "xlsx";
            //String fileNameExtension = "pdf";

            String[] streams;
            // String[] streams={nombre,encargada,destino,fechai,fechaf,dni};
            Warning[] warnings;
            byte[] renderedByte;
            renderedByte = localreport.Render(reportType, "", out mimeType, out encoding, out fileNameExtension,
                out streams, out warnings);

            Response.AddHeader("content-disposition", "attachment;filename= Trabajadores_report(" + fechaactual + ")." + fileNameExtension);
            return File(renderedByte, fileNameExtension);
        }

        /*********************************************************/
        /*********************SEGUROS******************************/
        public ActionResult reporteSCTR()
        {
            seguro_mantenimiento ma = new seguro_mantenimiento();
            LocalReport localreport = new LocalReport();
            localreport.ReportPath = Server.MapPath("~/Plantillas/DiseñoReportes/Report_Seguros.rdlc");

            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "DataSetSeguros";
            DataTable dt = ma.get_seguros();
            reportDataSource.Value = dt;
            localreport.DataSources.Add(reportDataSource);

            String reportType = "Excel";
            //String reportType = "PDF";

            String mimeType;
            String encoding;
            String fileNameExtension = "xlsx";
            //String fileNameExtension = "pdf";

            String[] streams;
            // String[] streams={nombre,encargada,destino,fechai,fechaf,dni};
            Warning[] warnings;
            byte[] renderedByte;
            renderedByte = localreport.Render(reportType, "", out mimeType, out encoding, out fileNameExtension,
                out streams, out warnings);

            Response.AddHeader("content-disposition", "attachment;filename= SCTR_report(" + fechaactual + ")." + fileNameExtension);
            return File(renderedByte, fileNameExtension);
        }

        /*********************************************************/
        /*********************EMPRESAS******************************/
        public ActionResult ReporteEmpresas()
        {
            empresa_mantenimiento ma = new empresa_mantenimiento();
            LocalReport localreport = new LocalReport();
            localreport.ReportPath = Server.MapPath("~/Plantillas/DiseñoReportes/Report_Empresas.rdlc");

            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "DataSetEmpresas";
            DataTable dt = ma.get_empresas();
            reportDataSource.Value = dt;
            localreport.DataSources.Add(reportDataSource);

            String reportType = "Excel";
            //String reportType = "PDF";

            String mimeType;
            String encoding;
            String fileNameExtension = "xlsx";
            //String fileNameExtension = "pdf";

            String[] streams;
            // String[] streams={nombre,encargada,destino,fechai,fechaf,dni};
            Warning[] warnings;
            byte[] renderedByte;
            renderedByte = localreport.Render(reportType, "", out mimeType, out encoding, out fileNameExtension,
                out streams, out warnings);

            Response.AddHeader("content-disposition", "attachment;filename= Empresas_report(" + fechaactual + ")." + fileNameExtension);
            return File(renderedByte, fileNameExtension);
        }









        /*******************************************************************************************/
        /**********************************ASISTENCIAS TRABAJADORES*************************************************/

        public ActionResult ReporteAsistenciasHoy()
        {
            asistencia_mantenimiento ma = new asistencia_mantenimiento();
            LocalReport localreport = new LocalReport();
            localreport.ReportPath = Server.MapPath("~/Plantillas/DiseñoReportes/Report_AsistenciasDelDia.rdlc");

            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "DataSetAsistenciasHoy";
            DataTable dt = ma.getAsistenciasHoy();
            reportDataSource.Value = dt;
            localreport.DataSources.Add(reportDataSource);

            String reportType = "Excel";
            String mimeType;
            String encoding;
            String fileNameExtension = "xlsx";
            String[] streams;
            // String[] streams={nombre,encargada,destino,fechai,fechaf,dni};
            Warning[] warnings;
            byte[] renderedByte;
            renderedByte = localreport.Render(reportType, "", out mimeType, out encoding, out fileNameExtension,
                out streams, out warnings);

            Response.AddHeader("content-disposition", "attachment;filename= AsistenciaDelDia_report(" + fechaactual + ")." + fileNameExtension);
            return File(renderedByte, fileNameExtension);
        }






        public ActionResult ReporteAsistenciasFiltros(string fechai,string fechaf,
            string nombre,string dni,string empresa)
        {
            EntidadBusqueda t=new EntidadBusqueda {
                fechadesde=fechai,
                fechahasta=fechaf,
                nombre=nombre,
                dni=dni,
                empresa=empresa
            };
            Debug.WriteLine("fecha****:" + fechaf);


            asistencia_mantenimiento ma = new asistencia_mantenimiento();
            ma.cleanNulls(t);
            LocalReport localreport = new LocalReport();
            localreport.ReportPath = Server.MapPath("~/Plantillas/DiseñoReportes/Report_AsistenciasFiltradas.rdlc");


            ReportParameter[] parameters = new ReportParameter[5];
            parameters[0] = new ReportParameter("fechai", t.fechadesde);
            parameters[1] = new ReportParameter("fechaf", t.fechahasta);
            parameters[2] = new ReportParameter("nombre", t.nombre);
            parameters[3] = new ReportParameter("dni", t.dni);
            parameters[4] = new ReportParameter("empresa", t.empresa);
            
            
            localreport.SetParameters(parameters);

            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "DataSetAsistenciasFiltros";
            DataTable dt = ma.getAsistenciasFiltradas(t);
            reportDataSource.Value = dt;
            localreport.DataSources.Add(reportDataSource);

            String reportType = "Excel";
            String mimeType;
            String encoding;
            String fileNameExtension = "xlsx";
            String[] streams;
            //String[] streams={t.fechai,t.fechaf,t.nombre,t.dni,t.empresa};
            Warning[] warnings;
            byte[] renderedByte;
            renderedByte = localreport.Render(reportType, "", out mimeType, out encoding, out fileNameExtension,
                out streams, out warnings);

            Response.AddHeader("content-disposition", "attachment;filename= AsistenciaConFiltros_report(" + fechaactual + ")." + fileNameExtension);
            return File(renderedByte, fileNameExtension);
        }


        //public ActionResult ReporteGetCarga(int id)
        //{
        //    vehiculo_mantenimiento ma = new vehiculo_mantenimiento();
        //    List<cargaVehicular> listrs = new List<cargaVehicular>();
        //    DataSet ds = ma.get_vehiculobyid(id);
        //    listrs = ma.DataSeT_to_List(ds);
        //    cargaVehicular c = listrs[0];
        //    LocalReport localreport = new LocalReport();
        //    localreport.ReportPath = Server.MapPath("~/Plantillas/DiseñoReportes/Report_CargaVehicular.rdlc");
        //    /**************************************************************/
        //    ReportParameter[] parameters = new ReportParameter[17];
        //    parameters[0] = new ReportParameter("fecha", c.fecha);
        //    if (c.bunidad == 1)
        //    {
        //        parameters[1] = new ReportParameter("isunidad", "X");
        //    }
        //    else
        //    {
        //        parameters[1] = new ReportParameter("isunidad", "");
        //    }
        //    parameters[2] = new ReportParameter("horaingreso",c.horaentrada);
        //    parameters[3] = new ReportParameter("horasalida",c.horasalida);
        //    parameters[4] = new ReportParameter("empresa", c.empresa_proveniente);
        //    parameters[5] = new ReportParameter("cliente", c.cliente);
        //    parameters[6] = new ReportParameter("nombre", c.nomConductor);
        //    parameters[7] = new ReportParameter("dni", c.docCondutor);
        //    parameters[8] = new ReportParameter("licencia", c.licConductor);
        //    parameters[9] = new ReportParameter("placa", c.placaVehiculo);
        //    if (c.bcarreta == 1)
        //    {
        //        parameters[10] = new ReportParameter("iscarreta", "X");
        //    }
        //    else
        //    {
        //        parameters[10] = new ReportParameter("iscarreta", "");

        //    }
        //    parameters[11] = new ReportParameter("contenedor", c.numContenedor);
        //    parameters[12] = new ReportParameter("carreta", c.codcarreta);

        //    parameters[13] = new ReportParameter("tipo", c.tipoContenedor);
        //    parameters[14] = new ReportParameter("estado", c.estadContenedor);
        //    parameters[15] = new ReportParameter("guiainfo", c.guiainf);
        //    parameters[16] = new ReportParameter("idcarga",c.idcargavehiculo+"");



        //    localreport.SetParameters(parameters);


        //    /***************************************************************/
        //    ReportDataSource reportDataSource = new ReportDataSource();
        //    reportDataSource.Name = "DataCargaVehicular";
        //    //DataSet ds = ma.get_vehiculobyid(id);
        //    reportDataSource.Value = listrs;
        //    localreport.DataSources.Add(reportDataSource);

        //    String reportType = "Excel";
        //    String mimeType;
        //    String encoding;
        //    String fileNameExtension = "xlsx";
        //    String[] streams;
        //    // String[] streams={nombre,encargada,destino,fechai,fechaf,dni};
        //    Warning[] warnings;
        //    byte[] renderedByte;
        //    renderedByte = localreport.Render(reportType, "", out mimeType, out encoding, out fileNameExtension,
        //        out streams, out warnings);

        //    Response.AddHeader("content-disposition", "attachment;filename= AsistenciaDelDia_report(" + fechaactual + ")." + fileNameExtension);
        //    return File(renderedByte, fileNameExtension);
        //}
        /********************************************************************/
        public ActionResult ReporteCitasHoy()
        {
            cita_mantenimiento ma = new cita_mantenimiento();
            LocalReport localreport = new LocalReport();
            localreport.ReportPath = Server.MapPath("~/Plantillas/DiseñoReportes/Report_CitasProgramadasHoy.rdlc");

            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "DataCitasHoy";
            DataTable dt = ma.get_CitasHoy();
            reportDataSource.Value = dt;
            localreport.DataSources.Add(reportDataSource);

            String reportType = "Excel";
            //String reportType = "PDF";

            String mimeType;
            String encoding;
            String fileNameExtension = "xlsx";
            //String fileNameExtension = "pdf";

            String[] streams;
            // String[] streams={nombre,encargada,destino,fechai,fechaf,dni};
            Warning[] warnings;
            byte[] renderedByte;
            renderedByte = localreport.Render(reportType, "", out mimeType, out encoding, out fileNameExtension,
                out streams, out warnings);

            Response.AddHeader("content-disposition", "attachment;filename= CitasProgramadasHoy_report(" + fechaactual + ")." + fileNameExtension);
            return File(renderedByte, fileNameExtension);
        }

        public ActionResult ReporteCitasFiltros(string fechadesde, string fechahasta, string nombre,
            string dni, string encargado, string destino)
        {
            EntidadBusqueda entidad = new EntidadBusqueda
            {
                fechadesde = fechadesde,
                fechahasta = fechahasta,
                nombre = nombre,
                dni = dni,
                destino = destino,
                encargado = encargado
            };


            cita_mantenimiento ma = new cita_mantenimiento();
            LocalReport localreport = new LocalReport();
            localreport.ReportPath = Server.MapPath("~/Plantillas/DiseñoReportes/Report_CitasxFiltros.rdlc");


            ReportParameter[] parameters = new ReportParameter[6];
            parameters[0] = new ReportParameter("fechai", entidad.fechadesde);
            parameters[1] = new ReportParameter("fechaf", entidad.fechahasta);
            parameters[2] = new ReportParameter("nombre", entidad.nombre);
            parameters[3] = new ReportParameter("dni", entidad.dni);
            parameters[4] = new ReportParameter("destino", entidad.destino);
            parameters[5] = new ReportParameter("encargado", entidad.proveedor);


            localreport.SetParameters(parameters);

            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "DataCitasConFiltros";
            DataTable dt = ma.getCitasFiltradas(entidad);
            reportDataSource.Value = dt;
            localreport.DataSources.Add(reportDataSource);

            String reportType = "Excel";
            //String reportType = "PDF";

            String mimeType;
            String encoding;
            String fileNameExtension = "xlsx";
            //String fileNameExtension = "pdf";
            String[] streams;
            //String[] streams={entidad.fechadesde,entidad.fechahasta,entidad.nombre,
            //                     entidad.dni,entidad.destino,entidad.proveedor};
            Warning[] warnings;
            byte[] renderedByte;
            renderedByte = localreport.Render(reportType, "", out mimeType, out encoding, out fileNameExtension,
                out streams, out warnings);

            Response.AddHeader("content-disposition", "attachment;filename= citasFiltradas_report(" + fechaactual + ")." + fileNameExtension);
            return File(renderedByte, fileNameExtension);
        }
        /*******************************************************************************************/


        //public ActionResult ReporteCargasHoy()
        //{
        //    vehiculo_mantenimiento ma = new vehiculo_mantenimiento();
        //    LocalReport localreport = new LocalReport();
        //    localreport.ReportPath = Server.MapPath("~/Plantillas/DiseñoReportes/Report_CargasHoy.rdlc");

        //    ReportDataSource reportDataSource = new ReportDataSource();
        //    reportDataSource.Name = "DataCargasHoy";
        //    DataSet ds = ma.get_CargasVehiculosHoy();
        //    reportDataSource.Value = ma.DataSeT_to_List(ds);
        //    localreport.DataSources.Add(reportDataSource);

        //    String reportType = "Excel";
        //    String mimeType;
        //    String encoding;
        //    String fileNameExtension = "xlsx";
        //    String[] streams;
        //    // String[] streams={nombre,encargada,destino,fechai,fechaf,dni};
        //    Warning[] warnings;
        //    byte[] renderedByte;
        //    renderedByte = localreport.Render(reportType, "", out mimeType, out encoding, out fileNameExtension,
        //        out streams, out warnings);

        //    Response.AddHeader("content-disposition", "attachment;filename= CargasVehiculosDelDia_report(" + fechaactual + ")." + fileNameExtension);
        //    return File(renderedByte, fileNameExtension);
        //}

        /***************************************************************************/
        //public ActionResult ReporteCargasFiltros(string fechadesde, string fechahasta, string nombre,
        //   string dni, string licencia, string placa, string proveedor, string cliente)
        //{
        //    EntidadBusqueda entidad = new EntidadBusqueda
        //    {
        //        fechadesde = fechadesde,
        //        fechahasta = fechahasta,
        //        nombre = nombre,
        //        dni = dni,
        //        licencia = licencia,
        //        placa = placa,
        //        proveedor=proveedor,
        //        proviene=proveedor,
        //        cliente=cliente
        //    };


        //    vehiculo_mantenimiento ma = new vehiculo_mantenimiento();
        //    LocalReport localreport = new LocalReport();
        //    localreport.ReportPath = Server.MapPath("~/Plantillas/DiseñoReportes/Report_CargasFiltradas.rdlc");


        //    ReportParameter[] parameters = new ReportParameter[8];
        //    parameters[0] = new ReportParameter("fechai", entidad.fechadesde);
        //    parameters[1] = new ReportParameter("fechaf", entidad.fechahasta);
        //    parameters[2] = new ReportParameter("nombre", entidad.nombre);
        //    parameters[3] = new ReportParameter("dni", entidad.dni);
        //    parameters[4] = new ReportParameter("licencia", entidad.destino);
        //    parameters[5] = new ReportParameter("placa", entidad.placa);
        //    parameters[6] = new ReportParameter("proveedor", entidad.proveedor);
        //    parameters[7] = new ReportParameter("cliente", entidad.cliente);

        //    localreport.SetParameters(parameters);

        //    ReportDataSource reportDataSource = new ReportDataSource();
        //    reportDataSource.Name = "DataCargasFiltradas";
        //    DataSet ds = ma.getCargasFiltradas(entidad);
        //    reportDataSource.Value = ma.DataSeT_to_List(ds);
        //    localreport.DataSources.Add(reportDataSource);

        //    String reportType = "Excel";
        //    //String reportType = "PDF";

        //    String mimeType;
        //    String encoding;
        //    String fileNameExtension = "xlsx";
        //    //String fileNameExtension = "pdf";
        //    String[] streams;
        //    //String[] streams={entidad.fechadesde,entidad.fechahasta,entidad.nombre,
        //    //                     entidad.dni,entidad.destino,entidad.proveedor};
        //    Warning[] warnings;
        //    byte[] renderedByte;
        //    renderedByte = localreport.Render(reportType, "", out mimeType, out encoding, out fileNameExtension,
        //        out streams, out warnings);

        //    Response.AddHeader("content-disposition", "attachment;filename= VehiculosFiltrados_report(" + fechaactual + ")." + fileNameExtension);
        //    return File(renderedByte, fileNameExtension);
        //}
        /*******************************************************************************************/


    }
}
