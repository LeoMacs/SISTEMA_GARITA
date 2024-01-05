using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImudesaSystem_v4_.Models.Clases
{
    public class indeseado
    {
        public int idindeseado { get; set; }
        public int idpersona { get; set; }
        public string persona { get; set; }
        public string nombres { get; set; }
        public string apPaterno { get; set; }
        public string apMaterno { get; set; }
        public string dni { get; set; }
        public string cargo { get; set; }
        public string empresaPvnte { get; set; }
        public string fechaRegistro { get; set; }
        public string observacion { get; set; }
        public string motAnular { get; set; }
        public string userRegistro { get; set; }
        /////////////////////////////////////
        public string noedit { get; set; }
        public string nodelete { get; set; }


        public String isvalidIndeseado()
        {
            String Mensaje = "SI";

            if (string.IsNullOrEmpty(nombres) || nombres.Trim() == "")
            {
                Mensaje = "Nombre obligatorio!! ";
                return Mensaje;
            }

            if (string.IsNullOrEmpty(apPaterno) || apPaterno.Trim() == "")
            {
                Mensaje = "Apellido paterno obligatorio!! ";
                return Mensaje;
            }

            if (string.IsNullOrEmpty(apMaterno) || apMaterno.Trim() == "")
            {
                Mensaje = "Apellido materno obligatorio!! ";
                return Mensaje;
            }

            if (string.IsNullOrEmpty(dni) || dni.Trim() == "")
            {
                Mensaje = "Dni obligatorio!! ";
                return Mensaje;
            }

            if (dni.Length < 8)
            {
                Mensaje = "Documento debe contener como mínimo 8 dígitos!! ";
                return Mensaje;
            }


            if (string.IsNullOrEmpty(observacion) || observacion.Trim() == "")
            {
                Mensaje = "Observación obligatoria!! ";
                return Mensaje;
            }

            nombres = nombres.Trim();
            apPaterno = apPaterno.Trim();
            apMaterno = apMaterno.Trim();
            dni =dni.Trim();

            return Mensaje;
        }

        public void cleanNulls()
        {
            if (string.IsNullOrEmpty(cargo) || cargo.Trim() == "")
            {
                cargo = "";
            }

            if (string.IsNullOrEmpty(empresaPvnte) || empresaPvnte.Trim() == "")
            {
                empresaPvnte = "";
            }
        }

    }
}