using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ImudesaSystem_v4_.Models;
using ImudesaSystem_v4_.Models.Clases;

namespace ImudesaSystem_v4_.Controllers
{
    public class AsistenciaController : Controller
    {
        asistencia_mantenimiento ma = new asistencia_mantenimiento();
        indeseado_mantenimiento mi = new indeseado_mantenimiento();

        public ActionResult panelAsistenciaDelDia()
        {
            Session["modulo"] = "trabajadores";
            return View();
        }

        public ActionResult RegistroAsistencias()
        {
            Session["modulo"] = "trabajadores";
            return View();
        }

        public JsonResult refresh()
        {
            string res = string.Empty;
            res = "SI";

            return Json(res, JsonRequestBehavior.AllowGet);
        }



        public JsonResult Add_asistencia(trabajador t)
        {

            string res = string.Empty;
            //string isvalido = "";
            int flaginsert;
            Boolean isindeseado = mi.verificarIndeseado(t.dni);

            try

            {
                if (isindeseado)
                {
                    //indeseado ind = mi.getIndeseado(t.dni);
                    //mi.reportarIndeseado(ind);  
                    res = "El trabajador está registrado en la Lista Negra, no está permitido darle ingreso!!";
                }
                else
                {
                    if (t.estado == "VIGENTE")
                    {
                        flaginsert = ma.addAsistencia(t);
                        if (flaginsert > 0)
                        {
                            res = "S";
                        }
                        else
                        {
                            res = "N";
                        }
                    }
                    else
                    {
                        res = "No se puede ingresar un trabajador con SCTR 'NO VIGENTE'!!";
                    }

                }


            }
            catch (Exception e)

            {
                res = e.Message;
            }

            return Json(new { success = true, responseText = res }, JsonRequestBehavior.AllowGet);
        }


        /***************************************************************************/
        // Display usuarios by id

        public JsonResult Update_asistencia(asistencia a)
        {
            string res = string.Empty;
            
            int flagupdate;
            try
            {
                /***************************************/

                
                    /********************************/
                    flagupdate = ma.addSalida(a);
                    if (flagupdate > 0)
                    {
                        res = "S";
                    }
                    else
                    {
                        res = "N";
                    }
                    /*********************************/



               
                /***************************************/

            }
            catch (Exception e)
            {
                res = e.Message;
            }

            //return Json(res, JsonRequestBehavior.AllowGet);
            return Json(new { success = true, responseText = res }, JsonRequestBehavior.AllowGet);

        }
        /*----------------------------------------------------------*/
        public JsonResult Get_asistenciabyid(int id)
        {

            DataTable dt = ma.getAsistencia(id);
            List<asistencia> listrs = new List<asistencia>();
            foreach (DataRow dr in dt.Rows)
            {

                listrs.Add(new asistencia
                {
                    idasistencia = Convert.ToInt32(dr["idasistencia"]),
                    nombre = dr["nombre"].ToString(),
                    dni = dr["dni"].ToString(),
                    empresa = dr["empresa"].ToString(),
                    fecha = dr["fecha"].ToString(),
                    horainicio = dr["horainicio"].ToString(),
                    horasalida = dr["horasalida"].ToString()

                });

            }
            return Json(listrs, JsonRequestBehavior.AllowGet);

        }
        /***********************************************************************/
        public JsonResult Get_asistenciasHoy()
        {
            DataTable dt = ma.getAsistenciasHoy();
            List<asistencia> listrs = new List<asistencia>();
            int contadorFilas = 0;
            foreach (DataRow dr in dt.Rows)
            {
                contadorFilas++;
                listrs.Add(new asistencia
                {

                    idasistencia = Convert.ToInt32(dr["idasistencia"]),
                    nombre = dr["nombre"].ToString(),
                    dni = dr["dni"].ToString(),
                    empresa = dr["empresa"].ToString(),
                    fecha = dr["fecha"].ToString(),
                    horainicio = dr["horainicio"].ToString(),
                    horasalida = dr["horasalida"].ToString(),
                    deshabilitado = dr["disabled"].ToString()


                });

            }
            //return Json(new { numRows = contadorFilas, dataList = listrs }, JsonRequestBehavior.AllowGet);

            return Json(listrs, JsonRequestBehavior.AllowGet);
        }


        public JsonResult Get_asistenciasLimitado()
        {
            DataTable dt = ma.getAsistenciaslimite();
            List<asistencia> listrs = new List<asistencia>();
            foreach (DataRow dr in dt.Rows)
            {

                listrs.Add(new asistencia
                {

                    idasistencia = Convert.ToInt32(dr["idasistencia"]),
                    nombre = dr["nombre"].ToString(),
                    dni = dr["dni"].ToString(),
                    empresa = dr["empresa"].ToString(),
                    fecha = dr["fecha"].ToString(),
                    horainicio = dr["horainicio"].ToString(),
                    horasalida = dr["horasalida"].ToString(),
                    

                });

            }
            return Json(listrs, JsonRequestBehavior.AllowGet);
        }


        //public JsonResult Get_asistenciasxFiltros(TempAux entidad)
        //{
        //    DataSet ds = ma.getAsistenciasFiltradas(entidad);
        //    List<asistencia> listrs = new List<asistencia>();
        //    foreach (DataRow dr in ds.Tables[0].Rows)
        //    {

        //        listrs.Add(new asistencia
        //        {

        //            idasistencia = Convert.ToInt32(dr["idasistencia"]),
        //            nombre = dr["nombre"].ToString(),
        //            dni = dr["dni"].ToString(),
        //            empresa = dr["empresa"].ToString(),
        //            fecha = dr["fecha"].ToString(),
        //            horainicio = dr["horainicio"].ToString(),
        //            horasalida = dr["horasalida"].ToString(),


        //        });

        //    }
        //    return Json(listrs, JsonRequestBehavior.AllowGet);

        //}
        /************************************************************/
        public JsonResult Get_asistenciasxFiltros(EntidadBusqueda entidad)
        {
            string res = string.Empty;
            List<asistencia> listrs = new List<asistencia>();
            res = ma.isvalidfiltros(entidad);
            if (res == "SI")
            {
                DataTable dt = ma.getAsistenciasFiltradas(entidad);
                foreach (DataRow dr in dt.Rows)
                {
                    listrs.Add(new asistencia
                    {

                        idasistencia = Convert.ToInt32(dr["idasistencia"]),
                        nombre = dr["nombre"].ToString(),
                        dni = dr["dni"].ToString(),
                        empresa = dr["empresa"].ToString(),
                        fecha = dr["fecha"].ToString(),
                        horainicio = dr["horainicio"].ToString(),
                        horasalida = dr["horasalida"].ToString(),


                    });

                }
            }

            return Json(listrs, JsonRequestBehavior.AllowGet);
        }



        /**********************************************************/
        public JsonResult Get_trabajadores()
        {
            DataTable dt = ma.getTrabajadores();
            List<trabajador> listrs = new List<trabajador>();
            foreach (DataRow dr in dt.Rows)
            {
                listrs.Add(new trabajador
                {
                    fecha = dr["fecha"].ToString(),
                    hora = dr["hora"].ToString(),
                    idtrabajador = Convert.ToInt32(dr["idtrabajador"]),
                    nombre = dr["nombre"].ToString(),
                    dni = dr["dni"].ToString(),                    
                    empresa = dr["empresa"].ToString(),
                    nombreSeguro = dr["sctr"].ToString(),
                    fechafinal= dr["fechafin"].ToString(),
                    estado= dr["estadoSctr"].ToString(),
                    colorSctr = dr["color"].ToString(),
                    estadoBoton=dr["estadoBoton"].ToString(),

                });
            }
            return Json(listrs, JsonRequestBehavior.AllowGet);
        }




       


    }
}
