using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImudesaSystem_v4_.Models.Clases
{
    public class usuario
    {
        public int idusuario { get; set; }
        public string cuenta { get; set; }
        public string clave { get; set; }
        public string tipocuenta { get; set; }
        public int activo { get; set; }
        public string estado { get; set; }
        public Nullable<System.DateTime> fecregistro { get; set; }
        public string nombreusuario { get; set; }
        public string trabajador { get; set; }
        public int idtrabajador { get; set; }
        public int idperfil { get; set; }
        public int codsuc { get; set; }
        public int idArea { get; set; }
        public string area { get; set; }
        public int idmenu { get; set; }
        public string codMenu { get; set; }
        public string codOpcion { get; set; }
        public string noabrir { get; set; }
        public string noguardar { get; set; }
        public string nocerrar { get; set; }
        public string noexportar { get; set; }
        public string noeliminar { get; set; }

        public String isvalidUsuario()
        {
            String Mensaje = "SI";

            if (string.IsNullOrEmpty(nombreusuario) || nombreusuario == "")
            {
                Mensaje = "Nombre del usuario obligatorio!! ";
                return Mensaje;
            }

            if (string.IsNullOrEmpty(clave) || clave == "")
            {
                Mensaje = "contraseña del usuario obligatoria!! ";
                return Mensaje;
            }

            return Mensaje;
        }

       

    }
}