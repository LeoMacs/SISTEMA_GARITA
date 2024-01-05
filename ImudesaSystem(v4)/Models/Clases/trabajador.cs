using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImudesaSystem_v4_.Models.Clases
{
    public class trabajador
    {


        public int idtrabajador { get; set; }
        public int idpersona { get; set; }
        public string nombres { get; set; }
        public string apPaterno { get; set; }
        public string apMaterno { get; set; }
        public string nombre { get; set; }
        public string dni { get; set; }
        public string cargo { get; set; }
        public int  idarea { get; set; }
        public string area { get; set; }
        public int habilitado { get; set; }
        public int idempresa { get; set; }
        public string empresa { get; set; }
        public string fecingreso { get; set; }
        public string fecretiro { get; set; }
        public string rutaimage { get; set; }
        public string codsuc { get; set; }
        ////////////////////
        public int idsegtrab { get; set; }
        public int idseguro { get; set; }
        ////////////////////
        public string estado { get; set; }
        public string colorEstado { get; set; }
        public string opcionsi { get; set; }
        public string opcionno { get; set; }
        public string opcionsiSeguro { get; set; }
        public string opcionnoSeguro { get; set; }
        public string colorEstadoSeguro { get; set; }
        //////////////////////
        public string fecha { get; set; }
        public string hora { get; set; }
        public string nombreSeguro { get; set; }
        public string fechafinal { get; set; }
        public string colorSctr { get; set; }
        public string estadoBoton { get; set; }
        //////////////////////
        public string noedit { get; set; }
        public string nodelete { get; set; }
        //////////////////////
        public string creador { get; set; }
        public string modificador { get; set; }

        public String isvalidtrabajador()
        {

            String Mensaje = "SI";
            if (idempresa < 1)
            {
                Mensaje = "empresa del trabajador obligatoria!! ";
                return Mensaje;
            }

            if (string.IsNullOrEmpty(nombres) || nombres == "")
            {
                Mensaje = "Nombres del trabajador obligatorio!! ";
                return Mensaje;
            }

            if (string.IsNullOrEmpty(apPaterno) || apPaterno == "")
            {
                Mensaje = "Apellido paterno del trabajador obligatorio!! ";
                return Mensaje;
            }

            if (string.IsNullOrEmpty(apMaterno) || apMaterno == "")
            {
                Mensaje = "Apellido materno del trabajador obligatorio!! ";
                return Mensaje;
            }

            if (string.IsNullOrEmpty(dni) || dni == "")
            {
                Mensaje = "dni del trabajador obligatorio!! ";
                return Mensaje;
            }

            if ( dni.Length < 8)
            {
                Mensaje = "documento debe contener al menos 8 dígitos!! ";
                return Mensaje;
            }

            return Mensaje;
        }


        public void cleanNulls()
        {
            if (string.IsNullOrEmpty(area))
            {
                area = "";
            }

            if (string.IsNullOrEmpty(cargo))
            {
                cargo = "";
            }
          
        }
        
    }
}