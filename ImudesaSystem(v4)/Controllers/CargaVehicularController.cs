using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Web.Mvc;
using System.Linq;
using ImudesaSystem_v4_.Models;
using ImudesaSystem_v4_.Models.Clases;

namespace ImudesaSystem_v4_.Controllers
{
    public class CargaVehicularController : Controller
    {
        //
        // GET: /Vehiculo/

        vehiculo_mantenimiento ma = new vehiculo_mantenimiento();
        indeseado_mantenimiento mi = new indeseado_mantenimiento();
        movimiento_mantenimiento mm = new movimiento_mantenimiento();

        public ActionResult PanelVehiculos()
        {
            Session["modulo"] = "vehiculos";
            return View();
        }

        //
        // GET: /Vehiculo/Details/5

        public ActionResult HistorialVehiculos()
        {
            return View();
        }


        public JsonResult refresh()
        {
            string res = string.Empty;
            res = "SI";
            return Json(res, JsonRequestBehavior.AllowGet);
        }


        /********************************************/
        public JsonResult Get_cargabyid(int id)
        {
            DataTable  dt = ma.get_vehiculobyid(id);
            List<cargaVehicular> listrs = new List<cargaVehicular>();
            foreach (DataRow dr in dt.Rows)
            {
                listrs.Add(new cargaVehicular
                {
                    idcargavehiculo = Convert.ToInt32(dr["idCargaVehicular"]),
                    fecha = dr["fecha"].ToString(),
                    horaentrada = dr["horaentrada"].ToString(),
                    horasalida = dr["horasalida"].ToString(),
                    empresa_proveniente = dr["empresaPvnte"].ToString(),
                    idempresa_dtno = Convert.ToInt32(dr["idDestino"]),
                    empresa_dtno = "",
                    persona = "",
                    nombres = dr["nombres"].ToString(),
                    apPaterno = dr["apPaterno"].ToString(),
                    apMaterno = dr["apMaterno"].ToString(),
                    dni = dr["dni"].ToString(),
                    licencia = dr["licencia"].ToString(),
                    placa = dr["placa"].ToString(),
                    bunidad = Convert.ToInt32(dr["bunidad"]),
                    bcarreta = Convert.ToInt32(dr["bcarreta"]),
                    codcarreta = dr["codcarreta"].ToString(),
                    numContenedor = dr["numContenedor"].ToString(),
                    tipoContenedor = dr["tipo"].ToString(),
                    estadContenedor = dr["estcarga"].ToString(),
                    precintoGuia = dr["precinto"].ToString()
                });

            }
            return Json(listrs, JsonRequestBehavior.AllowGet);
        }

        /*****************************************************************/
        public JsonResult Get_cargaVehicularHoy()
        {
            DataTable  dt = ma.get_CargasVehiculosHoy();
            List<cargaVehicular> listrs = new List<cargaVehicular>();

            foreach (DataRow dr in dt.Rows)
            {
                listrs.Add(new cargaVehicular
                {
                    idcargavehiculo = Convert.ToInt32(dr["ID"]),
                    fecha = dr["fecha"].ToString(),
                    horaentrada = dr["horaentrada"].ToString(),
                    horasalida = dr["horasalida"].ToString(),
                    hasalido = dr["hasalido"].ToString(),
                    noedit = dr["noedit"].ToString(),
                    nodelete = dr["nodelete"].ToString(),
                    empresa_proveniente = dr["origen"].ToString(),
                    idempresa_dtno = Convert.ToInt32(dr["iddestino"]),
                    empresa_dtno = dr["destino"].ToString(),
                    persona = dr["persona"].ToString(),
                    dni = dr["dni"].ToString(),
                    licencia = dr["licencia"].ToString(),
                    placa = dr["placa"].ToString(),
                    bunidad = Convert.ToInt32(dr["bunidad"]),
                    bcarreta = Convert.ToInt32(dr["bcarreta"]),
                    codcarreta = dr["codcarreta"].ToString(),
                    numContenedor = dr["numContenedor"].ToString(),
                    tipoContenedor = dr["tipo"].ToString(),
                    estadContenedor = dr["estcarga"].ToString(),
                    precintoGuia = dr["precinto"].ToString()

                }
                );
            }
            return Json(listrs, JsonRequestBehavior.AllowGet);
        }
        /******************************************************************/
        /******************************************************/





        /**************************************************************/
        public JsonResult insUpd_cargaVehicular(cargaVehicular vehiculo)
        {
            string res = string.Empty;
            try
            {
                /******************************************/
                String isvalido = ma.isvalidcarga(vehiculo);
                if (isvalido == "SI")
                {

                    /*******************************************************/
                    Boolean isindesdeado = mi.verificarIndeseado(vehiculo.dni);
                    if (isindesdeado)
                    {
                        //indeseado ind = mi.getIndeseado(vehiculo.docCondutor);
                        //mi.reportarIndeseado(ind); 
                        movimientoPersona m = new movimientoPersona();
                        m.modo = "i";
                        m.idMovimiento = 0;
                        m.idPersona = vehiculo.idPersona;
                        m.dni = vehiculo.dni;
                        m.nombres = vehiculo.nombres;
                        m.apPaterno = vehiculo.apPaterno;
                        m.apMaterno = vehiculo.apMaterno;
                        m.fecha = vehiculo.fecha;
                        m.horaentrada = vehiculo.fecha;
                        m.empresa_proveniente = vehiculo.empresa_proveniente;
                        int flag = mm.insupd_movimiento(m);
                        res = "Conductor registrado en la Lista Negra, no está permitido darle ingreso!!!!";
                    }
                    else
                    {
                        /*******************************************************/
                        int flaginsert = ma.InsUpd_cargavehicular(vehiculo);
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
                res = "ERROR: " + e.Message;
            }
            return Json(new { success = true, responseText = res }, JsonRequestBehavior.AllowGet);
        }


        /****************************************************************/
        /***********************************************************/
        public JsonResult registrar_salida(cargaVehicular v)
        {
            string res = string.Empty;

            int flagupdate;
            try
            {
                flagupdate = ma.registrarSalida(v);
                if (flagupdate > 0)
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
                res = "ERROR: " + e.Message;
            }

            //return Json(res, JsonRequestBehavior.AllowGet);
            return Json(new { success = true, responseText = res }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult Get_VehiculosxFiltros(EntidadBusqueda e)
        {
            ma.inicializadorBuscador(e);
            DataTable  dt = ma.getCargasFiltradas(e);
            List<cargaVehicular> listrs = new List<cargaVehicular>();
            string res = string.Empty;

            try
            {
                foreach (DataRow dr in dt.Rows)
                {

                    listrs.Add(new cargaVehicular
                    {
                        idcargavehiculo = Convert.ToInt32(dr["ID"]),
                        fecha = dr["fecha"].ToString(),
                        horaentrada = dr["horaentrada"].ToString(),
                        horasalida = dr["horasalida"].ToString(),
                        empresa_proveniente = dr["origen"].ToString(),
                        empresa_dtno = dr["destino"].ToString(),
                        persona = dr["persona"].ToString(),
                        dni = dr["dni"].ToString(),
                        licencia = dr["licencia"].ToString(),
                        placa = dr["placa"].ToString(),
                        codcarreta = dr["codCarreta"].ToString(),
                        numContenedor = dr["numContenedor"].ToString(),
                        tipoContenedor = dr["tipo"].ToString(),
                        estadContenedor = dr["estcarga"].ToString(),
                        precintoGuia = dr["precinto"].ToString()

                    });

                }
                res = "Registro Filtrado!!";
            }
            catch (Exception ex)
            {
                res = "ERROR:"+ex.Message;
            }

            return Json(listrs, JsonRequestBehavior.AllowGet);
        }


    }
}
