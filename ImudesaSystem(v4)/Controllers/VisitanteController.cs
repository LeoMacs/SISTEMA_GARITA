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
    public class VisitanteController : Controller
    {

        movimiento_mantenimiento mm = new movimiento_mantenimiento();
        //visitante_mantenimiento ma = new visitante_mantenimiento();
        indeseado_mantenimiento mi = new indeseado_mantenimiento();

        // GET: Visitante
        public ActionResult PanelVisitantes()
        {
            Session["modulo"] = "visitantes";
            return View();
        }

        public ActionResult RegistroVisitas()
        {
            return View();
        }

        public JsonResult refresh()
        {
            string res = string.Empty;
            res = "SI";

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        /***************************************************/
        /***************************************************/
        public JsonResult Get_visitantes_DiaActual()
        {
            DataTable dt = mm.get_movimientosHoy("v");
            List<movimientoPersona> listrs = new List<movimientoPersona>();
            foreach (DataRow dr in dt.Rows)
            {
                
                listrs.Add(new movimientoPersona
                {
                    idMovimiento = Convert.ToInt32(dr["idMovimiento"]),
                    fecha = dr["fecha"].ToString(),
                    horaentrada = dr["horaentrada"].ToString(),
                    horasalida = dr["horasalida"].ToString(),
                    persona = dr["persona"].ToString(),
                    dni = dr["dni"].ToString(),
                    destino = dr["destino"].ToString(),
                    encargado = dr["encargado"].ToString(),
                    empresa_proveniente = dr["empresaPvnte"].ToString(),
                    nroasignado = dr["numAsignado"].ToString(),
                    haSalido = dr["hasalido"].ToString(),
                    noedit = dr["noedit"].ToString(),
                    nodelete = dr["nodelete"].ToString(),
                    sisalida = dr["sisalida"].ToString()
                });

            }
            return Json(listrs, JsonRequestBehavior.AllowGet);
        }


        
        /********************************************/
        /********************************************/
        public JsonResult Get_visitabyid(int id)
        {
            DataTable  dt = mm.get_movimientobyid(id);
            List<movimientoPersona> listrs = new List<movimientoPersona>();
            foreach (DataRow dr in dt.Rows)
            {
                listrs.Add(new movimientoPersona
                {

                    idMovimiento = Convert.ToInt32(dr["idMovimiento"]),
                    idPersona = Convert.ToInt32(dr["idPersona"]),
                    nombres = dr["nombres"].ToString(),
                    apPaterno = dr["apPaterno"].ToString(),
                    apMaterno = dr["apMaterno"].ToString(),
                    dni = dr["dni"].ToString(),
                    fecha = dr["fecha"].ToString(),
                    horaentrada = dr["horaentrada"].ToString(),
                    destino = dr["destino"].ToString(),
                    encargado = dr["encargado"].ToString(),
                    empresa_proveniente = dr["empresaPvnte"].ToString(),
                    nroasignado = dr["numAsignado"].ToString(),
                    observacion = dr["observacion"].ToString()
                });

            }
            return Json(listrs, JsonRequestBehavior.AllowGet);

        }

        /******************************************************/
        /******************************************************/
        public JsonResult insupd_visita(movimientoPersona m)
        {
            string res = string.Empty;
            try
            {
                /******************************************/
                m.modo = "v";
                String isvalido = m.isvalidMovimiento();
                if (isvalido == "SI")
                {
                    /*******************************************************/
                    Boolean isindesdeado = mi.verificarIndeseado(m.dni);
                    if (isindesdeado)
                    {
                        //indeseado ind = mi.getIndeseado(visita.dni);
                        //mi.reportarIndeseado(ind);
                        m.modo = "i";
                        int flag=mm.insupd_movimiento(m);
                        res = "La visita está registrada en la Lista Negra, no está permitido darle ingreso!!!!";
                    }
                    else
                    {
                        int flaginsert = mm.insupd_movimiento(m);
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

        /***********************************************************/
        /***********************************************************/
        public JsonResult delete_visitante(movimientoPersona m)
        {
            string res = string.Empty;
            int flagdelete;
            try
            {
                flagdelete = mm.del_movimiento(m);
                if (flagdelete > 0)
                {
                    res = "S";
                }
                else
                {
                    res = "N";
                }
            }
            catch (Exception)
            {
                res = "";
            }

            //return Json(res, JsonRequestBehavior.AllowGet);
            return Json(new { success = true, responseText = res }, JsonRequestBehavior.AllowGet);

        }


        /***********************************************************/
        /***********************************************************/
        public JsonResult registrar_salida(movimientoPersona m)
        {
            string res = string.Empty;
            int flagupdate;
            try
            {
                flagupdate = mm.registrarSalida(m);
                if (flagupdate > 0)
                {
                    res = "S";
                }
                else
                {
                    res = "N";
                }
            }
            catch (Exception)
            {
                res = "";
            }

            //return Json(res, JsonRequestBehavior.AllowGet);
            return Json(new { success = true, responseText = res }, JsonRequestBehavior.AllowGet);

        }



        /***********************************************************/
        /***********************************************/
        public JsonResult Get_visitantesxFiltros(EntidadBusqueda e)
        {
           e.modo = "v";
           e.inicializadorBuscador();
            DataTable dt = mm.getVisitantesFiltradas(e);
            List<movimientoPersona> listrs = new List<movimientoPersona>();
            string res = string.Empty;

            try
            {
                foreach (DataRow dr in dt.Rows)
                {

                    listrs.Add(new movimientoPersona 
                    {
                        idMovimiento = Convert.ToInt32(dr["idMovimiento"]),
                        fecha = dr["fecha"].ToString(),
                        horaentrada = dr["horaentrada"].ToString(),
                        horasalida = dr["horasalida"].ToString(),
                        persona = dr["persona"].ToString(),
                        dni = dr["dni"].ToString(),
                        destino = dr["destino"].ToString(),
                        encargado = dr["encargado"].ToString(),
                        empresa_proveniente = dr["empresaPvnte"].ToString(),
                    });

                }
                res = "Búsqueda realizada!!";
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }

            //return Json(listrs, JsonRequestBehavior.AllowGet);
            return Json(new { visitas = listrs, responseText = res }, JsonRequestBehavior.AllowGet);
        }

        /********************************************/
        /********************************************/
        public JsonResult Get_persona(string dni)
        {
            DataTable dt = mm.get_persona(dni,0,1);
            List<movimientoPersona> listrs = new List<movimientoPersona>();
            foreach (DataRow dr in dt.Rows)
            {

                listrs.Add(new movimientoPersona
                {

                    idPersona = Convert.ToInt32(dr["idPersona"]),
                    nombres = dr["nombres"].ToString(),
                    apPaterno = dr["apPaterno"].ToString(),
                    apMaterno = dr["apMaterno"].ToString(),
                    dni = dr["dni"].ToString()
                  
                });

            }
            return Json(listrs, JsonRequestBehavior.AllowGet);

        }


        //public JsonResult actualizar_visita(visitante visita)
        //{
        //    string res = string.Empty;

        //    try
        //    {
        //        /******************************************/
        //        String isvalido = ma.isvalidvisita(visita);
        //        if (isvalido == "SI")
        //        {
        //            /*******************************************************/
        //            Boolean isindesdeado = mi.verificarIndeseado(visita.dni);
        //            if (isindesdeado)
        //            {
        //                res = "La visita está registrada en la Lista Negra, no está permitido darle ingreso!!!!";
        //            }
        //            else
        //            {

        //                int flagupdate = ma.update_visita(visita);
        //                if (flagupdate > 0)
        //                {
        //                    res = "S";
        //                }
        //                else
        //                {
        //                    res = "N";
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
