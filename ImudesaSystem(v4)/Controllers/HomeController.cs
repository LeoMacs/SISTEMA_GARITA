using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ImudesaSystem_v4_.Models;
using System.Data;
using System.Diagnostics;
using System.Web.Security;
using ImudesaSystem_v4_.Models.Clases;

namespace ImudesaSystem_v4_.Controllers
{
    public class HomeController : Controller
    {

        usuario_mantenimiento ma = new usuario_mantenimiento();
        // GET: Home
        public ActionResult Login()
        {
            return View();
        }


        public ActionResult Inicio()
        {
            return View();
        }

        public JsonResult Get_sucursales()
        {
            DataSet ds = ma.get_sucursales();
            List<sucursal> listrs = new List<sucursal>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                listrs.Add(new sucursal
                {
                    codsuc = dr["idSucursal"].ToString(),
                    descripcion = dr["cNombre"].ToString()

                });

            }
            return Json(listrs, JsonRequestBehavior.AllowGet);
        }



        public JsonResult iniciarSesion(usuario u)
        {
            varGlobales.g_codsuc = u.codsuc;
            string valido = string.Empty;
            DataSet ds = ma.iniciarSesion(u);
            List<usuario> listrs = new List<usuario>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                listrs.Add(new usuario
                {
                    idusuario = Convert.ToInt32(dr["idUsuario"]),
                    cuenta = dr["cUsuNombre"].ToString(),
                    idArea = Convert.ToInt32(dr["idArea"].ToString()),
                    area = dr["area"].ToString(),
                });
            }

            usuario usu = listrs[0];

            if (usu.idusuario > 0)
            {
                Session["id"] = usu.idusuario;
                varGlobales.g_idusuario = usu.idusuario;
                Session["nombre"] = usu.cuenta;
                varGlobales.g_nombreusuario = usu.cuenta;
                Session["area"] = usu.area;
                varGlobales.g_area = usu.area;
                Session["idarea"] = usu.idArea;
                varGlobales.g_idarea = usu.idArea;
                Debug.WriteLine("AREA:" + varGlobales.g_idarea);

                valido = "SI";
            }
            else
            {
                valido = "NO";
            }

            return Json(new { success = true, responseText = valido, usuario = usu }, JsonRequestBehavior.AllowGet);
        }



        public JsonResult Get_usuarioEnSesión()
        {
            List<usuario> listrs = new List<usuario>();
            listrs.Add(new usuario
            {
                idusuario = varGlobales.g_idusuario,
                area = varGlobales.g_area,
                nombreusuario = varGlobales.g_nombreusuario

            });

            return Json(listrs, JsonRequestBehavior.AllowGet);
        }


        public JsonResult Get_AccesosMenuxUsuario(usuario u)
        {
            string valido = string.Empty;
            usuario usu = null;
            u.idusuario = varGlobales.g_idusuario;
            DataSet ds = ma.get_AccesosMenuxUsuario(u);
            List<usuario> listrs = new List<usuario>();

            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    listrs.Add(new usuario
                    {
                        idusuario = Convert.ToInt32(dr["idUsuario"]),
                        idmenu = Convert.ToInt32(dr["idMenu"]),
                        codMenu = dr["cCodMenu"].ToString(),
                        codOpcion = dr["cCodOpcion"].ToString(),
                        noabrir = dr["noabrir"].ToString(),
                        noguardar = dr["noguardar"].ToString(),
                        nocerrar = dr["nocerrar"].ToString(),
                        noexportar = dr["noexportar"].ToString(),
                        noeliminar = dr["noeliminar"].ToString()
                    });
                }

                usu = listrs[0];
                valido = "SI";

            }
            else
            {
                valido = "NO";
            }


            return Json(new { success = true, responseText = valido, usuario = usu }, JsonRequestBehavior.AllowGet);

        }


        public ActionResult cerrarSesion()
        {
            Session.Abandon();
            string url = string.Format("/Home");
            return Redirect(url);
        }


    }
}
