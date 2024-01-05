using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ImudesaSystem_v4_.Models;
using ImudesaSystem_v4_.Models.Clases;

namespace ImudesaSystem_v4_.Controllers
{
    public class SeguroController : Controller
    {
        seguro_mantenimiento ma = new seguro_mantenimiento();

        // GET: Home
        public ActionResult Index()
        {
            Session["modulo"] = "trabajadores";
            return View();
        }

        public ActionResult PanelSeguros()
        {
            return View();
        }
        /**************************************************/
        public JsonResult refresh()
        {
            string res = string.Empty;
            res = "SI";

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        /*----------------------------------------------------------*/
        public JsonResult Get_segurobyid(int id)
        {

            DataTable dt = ma.get_segurobyid(id);
            List<seguro> listrs = new List<seguro>();
            foreach (DataRow dr in dt.Rows)
            {

                listrs.Add(new seguro
                {
                    idseguro = Convert.ToInt32(dr["idseguro"]),
                    nombre = dr["nombre"].ToString(),
                    nrosalud = dr["nrosalud"].ToString(),
                    nropoliza = dr["nropoliza"].ToString(),
                    fechainicio = dr["fechainicio"].ToString(),
                    fechafin = dr["fechafin"].ToString(),
                    empresa = dr["empresa"].ToString(),
                    idempresa = Convert.ToInt32(dr["idempresa"])

                });
                //Response.Write("<script language='JavaScript'>alert('fecha i: " + dr["fechainicio"].ToString() + "');</script>");

            }
            return Json(listrs, JsonRequestBehavior.AllowGet);

        }
        /***********************************************************************/
        public JsonResult Get_seguros()
        {
            DataTable dt = ma.get_seguros();
            List<seguro> listrs = new List<seguro>();
            foreach (DataRow dr in dt.Rows)
            {

                listrs.Add(new seguro
                {

                    idseguro = Convert.ToInt32(dr["idseguro"]),
                    nombre = dr["nombre"].ToString(),
                    empresa = dr["empresa"].ToString(),
                    idempresa = Convert.ToInt32(dr["idempresa"]),
                    nrosalud = dr["nrosalud"].ToString(),
                    nropoliza = dr["nropoliza"].ToString(),
                    fechainicio = dr["fechainicio"].ToString(),
                    fechafin = dr["fechafin"].ToString(),
                    estado = dr["estado"].ToString(),
                    colorEstado = dr["colorEstado"].ToString(),
                    creador = dr["creador"].ToString(),
                    modificador = dr["modificador"].ToString(),
                    noedit = dr["noedit"].ToString(),
                    nodelete = dr["nodelete"].ToString()
                });

            }
            return Json(listrs, JsonRequestBehavior.AllowGet);
        }


        /**************************************************/

        public JsonResult insupd_seguro(seguro s)
        {
            string res = string.Empty;
            try
            {
                /******************************************/
                String isvalido = s.isvalidseguro();
                if (isvalido == "SI")
                {

                    /*******************************************************/

                    int flaginsert = ma.insupd_seguro(s);
                    if (flaginsert > 0)
                    {
                        res = "SI";
                    }
                    else
                    {
                        res = "NO";
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


       
        /*************************************************************/
        public JsonResult delete_Seguro(seguro s)
        {
            string res = string.Empty;
            int flagdelete;
            try
            {
                /***************************************/

                flagdelete = ma.del_seguro(s.idseguro);
                if (flagdelete > 0)
                {
                    res = "SI";
                }
                else
                {
                    res = "NO";
                }
                /*********************************/

            }
            catch (Exception e)
            {
                res = e.Message;
            }

            //return Json(res, JsonRequestBehavior.AllowGet);
            return Json(new { success = true, responseText = res }, JsonRequestBehavior.AllowGet);

        }




        public JsonResult Get_empDispSeguro()
        {
            DataTable dt = ma.get_empDispForSeguro();
            List<empresa> listrs = new List<empresa>();
            foreach (DataRow dr in dt.Rows)
            {
                listrs.Add(new empresa
                {
                    idempresa = Convert.ToInt32(dr["idempresa"]),
                    nombre = dr["nombre"].ToString(),
                    descripcion = dr["descripcion"].ToString()

                });

            }
            return Json(listrs, JsonRequestBehavior.AllowGet);
        }



        public JsonResult get_PersonalxSeguro(int id)
        {
            DataTable dt = ma.get_PersonalxSeguro(id);
            List<trabajador> listrs = new List<trabajador>();
            foreach (DataRow dr in dt.Rows)
            {
                listrs.Add(new trabajador
                {
                    idsegtrab = Convert.ToInt32(dr["idsegtrab"]),
                    idseguro = Convert.ToInt32(dr["idseguro"]),
                    idtrabajador = Convert.ToInt32(dr["idtrabajador"]),
                    nombres = dr["nombre"].ToString(),
                    dni = dr["dni"].ToString(),
                    cargo = dr["cargo"].ToString(),
                    area = dr["area"].ToString(),
                    empresa = dr["empresa"].ToString()
                });
            }
            return Json(listrs, JsonRequestBehavior.AllowGet);
        }


        public JsonResult get_PersonalSinSeguro(int id)
        {
            DataTable dt = ma.get_PersonalSinSeguro(id);
            List<trabajador> listrs = new List<trabajador>();
            foreach (DataRow dr in dt.Rows)
            {
                listrs.Add(new trabajador
                {
                    idtrabajador = Convert.ToInt32(dr["idtrabajador"]),
                    nombres = dr["nombre"].ToString(),
                    dni = dr["dni"].ToString(),
                    cargo = dr["cargo"].ToString(),
                    area = dr["area"].ToString(),
                    empresa = dr["empresa"].ToString()
                });
            }
            return Json(listrs, JsonRequestBehavior.AllowGet);
        }

        /////////////////////////////////////
        public JsonResult add_afiliado(trabajador t)
        {
            string res = string.Empty;
            try
            {
                /******************************************/
                    int flaginsert = ma.add_afiliado(t);
                    if (flaginsert > 0)
                    {
                        res = "SI";
                    }
                    else
                    {
                        res = "NO";
                    }
            }
            catch (Exception e)
            {
                res = "Registro Fallido: " + e.Message;
            }
            return Json(new { success = true, responseText = res }, JsonRequestBehavior.AllowGet);

        }



        /*************************************************************/
        public JsonResult delete_afiliado(trabajador t)
        {
            string res = string.Empty;
            int flagdelete;
            try
            {
                flagdelete = ma.del_afiliado(t);
                if (flagdelete > 0)
                {
                    res = "SI";
                }
                else
                {
                    res = "NO";
                }
            }
            catch (Exception e)
            {
                res = e.Message;
            }
            return Json(new { success = true, responseText = res }, JsonRequestBehavior.AllowGet);
        }




        //public JsonResult Get_empDispSeguro()
        //{
        //    DataTable dt = ma.get_empDispForSeguro();
        //    List<empresa> listrs = new List<empresa>();
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        listrs.Add(new empresa
        //        {
        //            idempresa = Convert.ToInt32(dr["idempresa"]),
        //            nombre = dr["nombre"].ToString(),
        //            descripcion = dr["descripcion"].ToString()

        //        });

        //    }
        //    return Json(listrs, JsonRequestBehavior.AllowGet);
        //}



    }
}
