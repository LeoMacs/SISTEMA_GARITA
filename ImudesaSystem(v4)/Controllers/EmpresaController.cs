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
    public class EmpresaController : Controller
    {
        empresa_mantenimiento ma = new empresa_mantenimiento();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult panelEmpresas()
        {
            Session["modulo"] = "trabajadores";
            return View();
        }
        /******************************************************/
        /******************************************************/
        public JsonResult insupd_empresa(empresa e)
        {
            string res = string.Empty;
            try
            {
                /******************************************/

                String isvalido = e.isvalidEmpresa();
                if (isvalido == "SI")
                {
                    /*******************************************************/
                    
                        int flaginsert = ma.insupd_empresa(e);
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
                    res = isvalido;
                }
            }
            catch (Exception ex)
            {
                res = "Registro Fallido: " + ex.Message;
            }
            return Json(new { success = true, responseText = res }, JsonRequestBehavior.AllowGet);
        }


        ///***********************************************************************/
        //public JsonResult Add_empresa(empresa emp)
        //{
        //    string res = string.Empty;
        //    string isvalido="";
        //    int flaginsert;

        //    try

        //    {
        //        isvalido = ma.isvalidEmpresa(emp);
        //        if (isvalido.Equals("SI"))
        //        {
                   
        //            flaginsert= ma.Add_empresa(emp);
        //            if (flaginsert == 1)
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
        //    catch (Exception e)

        //    {
        //        res =e.Message;
        //    }

        //    return Json(new { success = true, responseText = res }, JsonRequestBehavior.AllowGet);
        //}


        ///***************************************************************************/
        //// Display usuarios by id

        //public JsonResult update_empresa(empresa emp)
        //{
        //    string res = string.Empty;
        //    string isvalido = "";
        //    int flagupdate;
        //    try
        //    {
        //        /***************************************/

        //        isvalido = ma.isvalidEmpresa(emp);
        //        if (isvalido.Equals("SI"))
        //        {
        //            /********************************/
        //            flagupdate = ma.update_empresa(emp);
        //            if (flagupdate == 1)
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
        //    catch (Exception e)
        //    {
        //        res = e.Message;
        //    }

        //    //return Json(res, JsonRequestBehavior.AllowGet);
        //    return Json(new { success = true, responseText = res }, JsonRequestBehavior.AllowGet);

        //}
        /*----------------------------------------------------------*/
        public JsonResult Get_empresabyid(int id)

        {

            DataSet ds = ma.get_empresabyid(id);
            List<empresa> listrs = new List<empresa>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                listrs.Add(new empresa
                {
                    idempresa = Convert.ToInt32(dr["idempresa"]),
                    ruc = dr["ruc"].ToString(),
                    nombre = dr["nombre"].ToString(),
                    descripcion = dr["descripcion"].ToString()
                });

            }
            return Json(listrs, JsonRequestBehavior.AllowGet);

        }
        /***********************************************************************/
        public JsonResult Get_empresas()
        {
            DataTable dt = ma.get_empresas();
            List<empresa> listrs = new List<empresa>();
            foreach (DataRow dr in dt.Rows)
            {

                listrs.Add(new empresa
                {

                    idempresa = Convert.ToInt32(dr["idempresa"]),
                    ruc = dr["ruc"].ToString(),
                    nombre = dr["nombre"].ToString(),
                    descripcion = dr["descripcion"].ToString(),
                    estado=""
                   
                });

            }
            return Json(listrs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Get_empresasconSeleccion(int idempresa)
        {
            DataTable dt = ma.get_empresasconSeleccion(idempresa);
            List<empresa> listrs = new List<empresa>();
            foreach (DataRow dr in dt.Rows)
            {
                listrs.Add(new empresa
                {
                    idempresa = Convert.ToInt32(dr["idempresa"]),
                    ruc = dr["ruc"].ToString(),
                    nombre = dr["nombre"].ToString(),
                    descripcion = dr["descripcion"].ToString(),
                    estado = dr["estado"].ToString()
                });

            }
            return Json(listrs, JsonRequestBehavior.AllowGet);
        }


        public JsonResult Get_Personalxempresa(int id)
        {
            DataSet ds = ma.get_Personalxempresa(id);
            List<trabajador> listrs = new List<trabajador>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                listrs.Add(new trabajador
                {
                    idtrabajador = Convert.ToInt32(dr["idtrabajador"]),
                    nombre = dr["nombre"].ToString(),
                    dni = dr["dni"].ToString(),
                    cargo = dr["cargo"].ToString(),
                    area = dr["area"].ToString(),
                    empresa= dr["empresa"].ToString()
                });
            }
            return Json(listrs, JsonRequestBehavior.AllowGet);
        }


        public JsonResult Get_SCTRxempresa(int id)
        {
            DataSet ds = ma.get_SCTRxEmpresa(id);
            List<seguro> listrs = new List<seguro>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                listrs.Add(new seguro
                {
                    idseguro = Convert.ToInt32(dr["idseguro"]),
                    nombre = dr["nombre"].ToString(),
                    nrosalud = dr["nrosalud"].ToString(),
                    nropoliza = dr["nropoliza"].ToString(),
                    fechainicio = dr["fechainicio"].ToString(),
                    fechafin = dr["fechafin"].ToString(),
                    empresa= dr["empresa"].ToString(),
                    idempresa = Convert.ToInt32(dr["idempresa"])
                });
            }
            return Json(listrs, JsonRequestBehavior.AllowGet);
        }



    }
}