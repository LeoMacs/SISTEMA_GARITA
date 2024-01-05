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
    public class TrabajadorController : Controller
    {
        trabajador_mantenimiento ma = new trabajador_mantenimiento();
        indeseado_mantenimiento mi = new indeseado_mantenimiento();
        empresa_mantenimiento me = new empresa_mantenimiento();

        // GET: Trabajador
        public ActionResult PanelTrabajadores()
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



        public JsonResult Get_trabajadorbyid(int id)
        {
            DataTable dt = ma.get_trabajadorbyid(id);
            List<trabajador> listrs = new List<trabajador>();
            foreach (DataRow dr in dt.Rows)
            {
                listrs.Add(new trabajador
                {

                    idtrabajador = Convert.ToInt32(dr["idtrabajador"]),
                    nombre = dr["nombre"].ToString(),
                    nombres = dr["nombres"].ToString(),
                    apPaterno = dr["apPaterno"].ToString(),
                    apMaterno = dr["apMaterno"].ToString(),
                    dni = dr["dni"].ToString(),
                    empresa = dr["empresa"].ToString(),
                    idempresa = Convert.ToInt32(dr["idempresa"]),
                    cargo = dr["cargo"].ToString(),
                    idarea = Convert.ToInt32(dr["idarea"]),
                    area = dr["area"].ToString(),
                    fecingreso = dr["fecingreso"].ToString(),
                    fecretiro = dr["fecretiro"].ToString(),
                    habilitado = Convert.ToInt32(dr["bActivo"]),
                    estado = dr["estado"].ToString(),
                    colorEstado = dr["colorEstado"].ToString(),
                    opcionsi = dr["opcionsi"].ToString(),
                    opcionno = dr["opcionno"].ToString()
                });
                //Response.Write("<script language='JavaScript'>alert('fecha i: " + dr["fechainicio"].ToString() + "');</script>");

            }
            return Json(listrs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Get_trabajadores()
        {
            DataTable dt = ma.get_trabajadores();
            List<trabajador> listrs = new List<trabajador>();
            foreach (DataRow dr in dt.Rows)
            {

                listrs.Add(new trabajador
                {

                    idtrabajador = Convert.ToInt32(dr["idtrabajador"]),
                    nombre = dr["nombre"].ToString(),
                    nombres = dr["nombres"].ToString(),
                    apPaterno = dr["apPaterno"].ToString(),
                    apMaterno = dr["apMaterno"].ToString(),
                    empresa = dr["empresa"].ToString(),
                    dni = dr["dni"].ToString(),
                    idempresa = Convert.ToInt32(dr["idempresa"]),
                    cargo = dr["cargo"].ToString(),
                    idarea = Convert.ToInt32(dr["idarea"]),
                    area = dr["area"].ToString(),
                    fecingreso = dr["fecingreso"].ToString(),
                    fecretiro =  dr["fecretiro"].ToString(),
                    habilitado = Convert.ToInt32(dr["bActivo"]),
                    estado = dr["estado"].ToString(),
                    colorEstado = dr["colorEstado"].ToString(),
                    opcionsi = dr["opcionsi"].ToString(),
                    opcionno = dr["opcionno"].ToString(),
                    creador = dr["creador"].ToString(),
                    modificador = dr["modificador"].ToString(),
                    noedit = dr["noedit"].ToString(),
                    nodelete = dr["nodelete"].ToString()
                });

            }
            return Json(listrs, JsonRequestBehavior.AllowGet);
        }


        /**************************************************************/
        public JsonResult insupd_trabajador(trabajador t)
        {
            string res = string.Empty;
            try
            {
                /******************************************/
                String isvalido = t.isvalidtrabajador();
                if (isvalido == "SI")
                {
                    /*******************************************************/
                    Boolean isindesdeado = mi.verificarIndeseado(t.dni);
                    if (isindesdeado)
                    {

                        res = "Trabajador está registrado en la Lista Negra, regular este trabajador!!!!";
                    }
                    else
                    {
                        /*******************************************************/

                        int flaginsert = ma.insupd_trabajador(t);
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

        /****************************************************************/
        /*****************************************************************/
        public JsonResult delete_trabajador(trabajador t)
        {
            string res = string.Empty;

            try
            {

                int flaganular = ma.del_trabajador(t);
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


        /***********************************************************************/
        public JsonResult Get_areas()
        {
            DataTable dt = ma.get_areas();
            List<area> listrs = new List<area>();
            foreach (DataRow dr in dt.Rows)
            {
                listrs.Add(new area
                {

                    idarea = Convert.ToInt32(dr["idArea"]),
                    nombre = dr["nombre"].ToString(),
                    fecReg = dr["fecReg"].ToString(),
                     estado= dr["estado"].ToString()
                });

            }
            return Json(listrs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Get_areasconSeleccion(int idarea)
        {
            DataTable dt = ma.get_areasconSeleccion(idarea);
            List<area> listrs = new List<area>();
            foreach (DataRow dr in dt.Rows)
            {
                listrs.Add(new area
                {

                    idarea = Convert.ToInt32(dr["idArea"]),
                    nombre = dr["nombre"].ToString(),
                    fecReg = dr["fecReg"].ToString(),
                     estado= dr["estado"].ToString()
                });

            }
            return Json(listrs, JsonRequestBehavior.AllowGet);
        }


        public JsonResult Get_segurobyid(int id)
        {
            DataTable dt = ma.get_seguro(id);
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

        //public JsonResult Get_empresasconSeleccion(int idempresa)
        //{
        //    DataTable dt = me.get_empresasconSeleccion(idempresa);
        //    List<empresa> listrs = new List<empresa>();
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        listrs.Add(new empresa
        //        {
        //            idempresa = Convert.ToInt32(dr["idempresa"]),
        //            ruc = dr["ruc"].ToString(),
        //            nombre = dr["nombre"].ToString(),
        //            descripcion = dr["descripcion"].ToString(),
        //            estado= dr["estado"].ToString()
        //        });

        //    }
        //    return Json(listrs, JsonRequestBehavior.AllowGet);
        //}




        //public JsonResult Add_trabajador(trabajador t)
        //{
        //    string res = string.Empty;
        //    string isvalido = "";
        //    int flaginsert;

        //    try

        //    {
        //        isvalido = ma.isvalidtrabajador(t);
        //        if (isvalido.Equals("SI"))
        //        {

        //            flaginsert = ma.Add_trabajador(t);
        //            if (flaginsert > 0)
        //            {
        //                res = "S";
        //            }
        //            else
        //            {
        //                res = "N";
        //            }



        //        }
        //        else
        //        {
        //            res = isvalido;
        //        }

        //    }
        //    catch (Exception)

        //    {
        //        res = "";
        //    }

        //    return Json(new { success = true, responseText = res }, JsonRequestBehavior.AllowGet);
        //}


        ///***************************************************************************/
        //// Display usuarios by id

        //public JsonResult update_trabajador(trabajador t)
        //{
        //    string res = string.Empty;
        //    string isvalido = "";
        //    int flagupdate;
        //    try
        //    {
        //        /***************************************/

        //        isvalido = ma.isvalidtrabajador(t);
        //        if (isvalido.Equals("SI"))
        //        {
        //            /********************************/
        //            flagupdate = ma.update_trabajador(t);
        //            if (flagupdate >= 1)
        //            {
        //                res = "S";
        //            }
        //            else
        //            {
        //                res = "N";
        //            }
        //            /*********************************/



        //        }
        //        else
        //        {
        //            res = isvalido;
        //        }
        //        /***************************************/

        //    }
        //    catch (Exception)
        //    {
        //        res = "";
        //    }

        //    //return Json(res, JsonRequestBehavior.AllowGet);
        //    return Json(new { success = true, responseText = res }, JsonRequestBehavior.AllowGet);

        //}

    }
}
