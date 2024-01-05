using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ImudesaSystem_v4_.Models;
using ImudesaSystem_v4_.Models.Clases;

namespace ImudesaSystem_v3_.Controllers
{
    public class IndeseadosController : Controller
    {
        // GET: Indeseado
        indeseado_mantenimiento ma = new indeseado_mantenimiento();

        public ActionResult panelListaNegra()
        {
            return View();
        }

        // GET: Indeseado/Details/5

        public JsonResult refresh()
        {
            string res = string.Empty;
            res = "SI";
            return Json(res, JsonRequestBehavior.AllowGet);
        }


        public JsonResult Get_indeseados()
        {
            DataTable  dt = ma.getIndeseados ();
            List<indeseado> listrs = new List<indeseado>();
            foreach (DataRow dr in dt.Rows)
            {
                listrs.Add(new indeseado
                {
                    idindeseado = Convert.ToInt32(dr["idindeseado"]),
                    persona = dr["persona"].ToString(),
                    dni = dr["dni"].ToString(),
                    empresaPvnte = dr["empresaPvnte"].ToString(),
                    cargo = dr["cargo"].ToString(),
                    observacion = dr["observacion"].ToString(),
                    motAnular = dr["motAnular"].ToString(),
                    fechaRegistro = dr["fechaRegistro"].ToString(),
                    noedit = dr["noedit"].ToString(),
                    nodelete = dr["nodelete"].ToString()
                });

            }
            return Json(listrs, JsonRequestBehavior.AllowGet);
        }


        //--------------------------------
        public JsonResult Get_indeseadobyid(int id)
        {
            DataTable dt = ma.get_indeseadobyIdDni(id,"","I");//búsqueda por ID
            List<indeseado> listrs = new List<indeseado>();
            foreach (DataRow dr in dt.Rows)
            {

                listrs.Add(new indeseado
                {
                    idindeseado = Convert.ToInt32(dr["idindeseado"]),
                    nombres = dr["nombre"].ToString(),
                    apPaterno = dr["apPaterno"].ToString(),
                    apMaterno = dr["apMaterno"].ToString(),
                    dni = dr["dni"].ToString(),
                    empresaPvnte = dr["empresaPvnte"].ToString(),
                    cargo = dr["cargo"].ToString(),
                    observacion = dr["observacion"].ToString(),
                    motAnular = dr["motAnular"].ToString(),
                    fechaRegistro = dr["fechaRegistro"].ToString()
                });

            }
            return Json(listrs, JsonRequestBehavior.AllowGet);

        }


        //*****************************
        public JsonResult insupd_indeseado(indeseado i)
        {
            string res = string.Empty;
            string isvalido = "";
            int flaginsUpd;
            try
            {
                isvalido = i.isvalidIndeseado();
                if (isvalido=="SI")
                {
                    flaginsUpd = ma.insupd_indeseado(i);
                    if (flaginsUpd > 0)
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
                    res = isvalido;
                }
            }
            catch (Exception e)
            {
                res = "Registro Fallido: " + e.Message;
            }

            return Json(new { success = true, responseText = res }, JsonRequestBehavior.AllowGet);
        }
       
        //*************************************************************/
        public JsonResult delete_indeseado(indeseado i)
        {
            string res = string.Empty;

            try
            {

                int flaganular = ma.del_indeseado(i);
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

    }
}
