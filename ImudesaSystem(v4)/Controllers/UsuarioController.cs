using ImudesaSystem_v4_.Models;
using ImudesaSystem_v4_.Models.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImudesaSystem_v4_.Controllers
{
    public class UsuarioController : Controller
    {
        usuario_mantenimiento ma = new usuario_mantenimiento();
        // GET: /Usuario/

        public ActionResult PanelUsuario()
        {
            Session["modulo"] = "administradores";
            return View();
        }

        /******************************************************/
        /******************************************************/
        public JsonResult insupd_usuario(usuario u)
        {
            string res = string.Empty;
            try
            {
                /******************************************/

                String isvalido = u.isvalidUsuario();
                if (isvalido == "SI")
                {
                    /*******************************************************/

                    int flaginsert = ma.insupd_usuario(u);
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
        /***********************************************************************/
        /***********************************************************************/
        public JsonResult Get_usuariobyid(int id)
        {

            DataTable dt = ma.get_usuarioxID(id);
            List<usuario> listrs = new List<usuario>();
            foreach (DataRow dr in dt.Rows)
            {

                listrs.Add(new usuario
                {
                    idusuario = Convert.ToInt32(dr["idUsuario"]),
                    nombreusuario = dr["nombreUsuario"].ToString(),
                    idtrabajador = Convert.ToInt32(dr["idTrabajador"]),
                    trabajador = dr["nombre"].ToString(),
                    idArea = Convert.ToInt32(dr["idArea"]),
                    area = dr["area"].ToString(),
                    activo = Convert.ToInt32(dr["bActivo"]),
                    estado = dr["estado"].ToString()
                });

            }
            return Json(listrs, JsonRequestBehavior.AllowGet);

        }
        /***********************************************************************/
        /***********************************************************************/
        public JsonResult Get_usuarios()
        {
            DataTable dt = ma.get_usuarios();
            List<usuario> listrs = new List<usuario>();
            foreach (DataRow dr in dt.Rows)
            {


                listrs.Add(new usuario
                {
                    idusuario = Convert.ToInt32(dr["idUsuario"]),
                    nombreusuario = dr["nombreUsuario"].ToString(),
                    idtrabajador = Convert.ToInt32(dr["idTrabajador"]),
                    trabajador = dr["nombre"].ToString(),
                    idArea = Convert.ToInt32(dr["idArea"]),
                    area = dr["area"].ToString(),
                    activo = Convert.ToInt32(dr["bActivo"]),
                    estado = dr["estado"].ToString()
                });


            }
            return Json(listrs, JsonRequestBehavior.AllowGet);
        }

        /****************************************************************/
        /*****************************************************************/
        public JsonResult delete_usuario(int id)
        {
            string res = string.Empty;

            try
            {

                int flaganular = ma.del_Usuario(id);
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
