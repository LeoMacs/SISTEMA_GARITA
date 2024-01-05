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
    public class CitaController : Controller
    {
        //
        // GET: /Cita/

        cita_mantenimiento ma = new cita_mantenimiento();
        indeseado_mantenimiento mi = new indeseado_mantenimiento();



        public ActionResult PanelCitas()
        {

            return View();
        }




        //
        // GET: /Cita/RegistroCitas/5

        public ActionResult RegistroCitas()
        {

            return View();
        }

        //
        /********************************************/
        public JsonResult refresh()
        {
            string res = string.Empty;
            res = "SI";

            return Json(res, JsonRequestBehavior.AllowGet);
        }


        /********************************************/
        public JsonResult Get_citabyid(int id)
        {
            DataTable dt = ma.get_citabyid(id);
            List<cita> listrs = new List<cita>();
            foreach (DataRow dr in dt.Rows)
            {
                listrs.Add(new cita
                {

                    idcita = Convert.ToInt32(dr["idcita"]),
                    nombres = dr["nombres"].ToString(),
                    apPaterno = dr["apPaterno"].ToString(),
                    apMaterno = dr["apMaterno"].ToString(),
                    persona = dr["persona"].ToString(),
                    fecha = dr["fecha"].ToString(),
                    hora = dr["hora"].ToString(),
                    dni = dr["dni"].ToString(),
                    destino = dr["destino"].ToString(),
                    idDestino =  Convert.ToInt32(dr["idDestino"]),
                    encargado = dr["encargado"].ToString(),
                    empresa_proveniente = dr["empresaPvnte"].ToString(),
                    observacion = dr["observacion"].ToString()
                });
                //Response.Write("<script language='JavaScript'>alert('fecha i: " + dr["fechainicio"].ToString() + "');</script>");
            }
            return Json(listrs, JsonRequestBehavior.AllowGet);
        }

        /*****************************************************************/
        public JsonResult Get_citasDiaActual()
        {
            DataTable dt = ma.get_CitasHoy();
            List<cita> listrs = new List<cita>();
            String disabledArea = "";
            String disabledSeguridad = "";

            foreach (DataRow dr in dt.Rows)
            {
                //disabledArea = ma.DeshabilitarArea(varGlobales.g_idarea);
                //disabledSeguridad = ma.DeshabilitarSeguridad(varGlobales.g_idarea);
                listrs.Add(new cita
                {
                    idcita = Convert.ToInt32(dr["idcita"]),
                    fecha = dr["fecha"].ToString(),
                    hora = dr["hora"].ToString(),
                    persona = dr["persona"].ToString(),
                    dni = dr["dni"].ToString(),
                    destino = dr["destino"].ToString(),
                    idDestino = Convert.ToInt32(dr["idDestino"]),
                    encargado = dr["encargado"].ToString(),
                    empresa_proveniente = dr["empresaPvnte"].ToString(),
                    observacion = dr["observacion"].ToString(),
                    creador = dr["creador"].ToString(),
                    modificador = dr["modificador"].ToString(),
                    noedit = dr["noedit"].ToString(),
                    nodelete = dr["nodelete"].ToString(),
                    noconfirm = dr["noconfirm"].ToString()
                }
                );
            }
            return Json(listrs, JsonRequestBehavior.AllowGet);
        }
        /********************************************/
        /******************************************************/

        public JsonResult Get_citasxFiltros(EntidadBusqueda e)
        {
            e.inicializadorBuscador();
            DataTable dt = ma.getCitasFiltradas(e);
            List<cita> listrs = new List<cita>();
            string res = string.Empty;

            try
            {
                foreach (DataRow dr in dt.Rows)
                {

                    listrs.Add(new cita
                    {
                        idcita = Convert.ToInt32(dr["idcita"]),
                        fecha = dr["fecha"].ToString(),
                        hora = dr["hora"].ToString(),
                        persona = dr["persona"].ToString(),
                        dni = dr["dni"].ToString(),
                        destino = dr["destino"].ToString(),
                        encargado = dr["encargado"].ToString(),
                        empresa_proveniente = dr["empresaPvnte"].ToString(),
                        observacion = dr["observacion"].ToString(),
                        user = dr["usuario"].ToString(),
                        estadocita = dr["estado"].ToString(),
                        colorestado = dr["colorEstado"].ToString()
                    });

                }
                res = "Citas Filtradas!!";
            }
            catch (Exception exc)
            {
                res = exc.Message;
            }

            return Json(listrs, JsonRequestBehavior.AllowGet);
        }


        /**************************************************************/
        /**************************************************************/
        public JsonResult insupd_cita(cita cita)
        {
            string res = string.Empty;
            try
            {
                /******************************************/
                String isvalido = cita.isvalidcita();
                if (isvalido == "SI")
                {

                    /*******************************************************/
                    Boolean isindesdeado = mi.verificarIndeseado(cita.dni);
                    if (isindesdeado)
                    {
                       
                        res = "Citad@ registrad@ en la Lista Negra, no está permitido darle ingreso!!!!";
                    }
                    else
                    {
                        /*******************************************************/

                        int flaginsert = ma.insupd_cita(cita);
                        if (flaginsert > 0)
                        {
                            res = "S";
                        }
                        else
                        {
                            res = "N";
                        }
                    }

                }
                else
                {
                    res = isvalido;
                }

            }
            catch (Exception e)
            {
                res = "Registro Fallido: " + e.Message;
            }
            return Json(new { success = true, responseText = res }, JsonRequestBehavior.AllowGet);
        }
        /****************************************************************/


        ///*****************************************************************/
        ///***************************************************************/
        //public JsonResult ocultar_cita(cita cita)
        //{
        //    string res = string.Empty;

        //    try
        //    {

        //        int flagupdate = ma.ocultarcita(cita);
        //        if (flagupdate > 0)
        //        {
        //            res = "SI";
        //        }
        //        else
        //        {
        //            res = "NO";
        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        res = "Registro Fallido: " + e.Message;
        //    }
        //    return Json(new { success = true, responseText = res }, JsonRequestBehavior.AllowGet);
        //}
        /****************************************************************/
        /*****************************************************************/
        public JsonResult delete_cita(cita cita)
        {
            string res = string.Empty;

            try
            {

                int flaganular = ma.del_cita(cita);
                if (flaganular > 0)
                {
                    res = "S";
                }
                else
                {
                    res = "N";
                }

            }
            catch (Exception e)
            {
                res = "Registro Fallido: " + e.Message;
            }
            return Json(new { success = true, responseText = res }, JsonRequestBehavior.AllowGet);
        }

        /*********************************************************/
        /*********************************************************/
        public JsonResult confirm_AsistenciaCita(cita cita)
        {
            string res = string.Empty;
            try
            {
                /******************************************/
                String isvalido = cita.isvalidcita();
               
                if (isvalido == "SI")
                {
                    /*******************************************************/
                    String isvalidoNum = cita.isValidNumeroAsignado();
                    if (isvalidoNum == "SI")
                    {
                        /*******************************************************/
                        int flaginsert = ma.confirm_citaAsistencia(cita);
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
                        res = isvalidoNum;
                    }

                }
                else
                {
                    res = isvalido;
                }

            }
            catch (Exception e)
            {
                res = "Registro Fallido: " + e.Message;
            }
            return Json(new { success = true, responseText = res }, JsonRequestBehavior.AllowGet);
        }


        ///***************************************************************/
        //public JsonResult registrar_asistencia(cita cita)
        //{
        //    string res = string.Empty;

        //    try
        //    {
        //        /******************************************/
        //        String isvalido = cita.isvalidcita();
        //        if (isvalido == "SI")
        //        {
        //            /*******************************************************/
        //            Boolean isindesdeado = mi.verificarIndeseado(cita.dni);
        //            if (isindesdeado)
        //            {
        //                res = "El citado está registrada en la Lista Negra, no está permitido agendarle una cita!!!!";
        //            }
        //            else
        //            {

        //                int flagupdate = ma.insupd_cita(cita);
        //                if (flagupdate > 0)
        //                {
        //                    res = "SI";
        //                }
        //                else
        //                {
        //                    res = "NO";
        //                }

        //            }


        //        }
        //        else
        //        {
        //            res = isvalido;
        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        res = "Registro Fallido: " + e.Message;
        //    }
        //    return Json(new { success = true, responseText = res }, JsonRequestBehavior.AllowGet);
        //}


    }
}
