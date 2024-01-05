using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImudesaSystem_v4_.Models.Clases
{
    public class cita
    {
        public int idcita { get; set; }
        public string fecha { get; set; }
        public string hora { get; set; }
        public string nombres { get; set; }
        public string apPaterno { get; set; }
        public string apMaterno { get; set; }
        public string persona { get; set; }
        public string dni { get; set; }
        public int idDestino { get; set; }
        public string destino { get; set; }
        public string encargado { get; set; }
        public string empresa_proveniente { get; set; }
        public string observacion { get; set; }
        public int idcitador { get; set; }
        public int estado { get; set; }
        public string codsuc { get; set; }
        public string fechaRegistro { get; set; }
        public string userRegistro { get; set; }
        public int mostrar { get; set; }
        public int atendida { get; set; }
        public int nroasignado { get; set; }
        public string estadocita { get; set; }
        public string colorestado { get; set; }
        public string user { get; set; }
        public string creador { get; set; }
        public string modificador { get; set; }
        public int idarea { get; set; }
       
        public string noedit { get; set; }
        public string nodelete { get; set; }
        public string noconfirm { get; set; }
        public string isdisabledarea { get; set; }
        public string isdisabledseg { get; set; }


        public void cleanNulls()
        {
            if (string.IsNullOrEmpty(observacion))
            {
                observacion = "";
            }

            if (string.IsNullOrEmpty(encargado))
            {
                encargado = "";
            }

            if (string.IsNullOrEmpty(empresa_proveniente))
            {
                empresa_proveniente = "";
            }
        }


        public String isvalidcita()
        {
            String Mensaje = "SI";
            

            if (string.IsNullOrEmpty(dni) || dni == "")
            {
                Mensaje = "Dni del citado obligatorio!! ";
                return Mensaje;
            }

            if (dni.Length != 8)
            {
                Mensaje = "Dni debe contener 8 dígitos!! ";
                return Mensaje;
            }


            if (string.IsNullOrEmpty(nombres) || nombres == "")
            {
                Mensaje = "Nombre del citado obligatorio!! ";
                return Mensaje;
            }


            if (string.IsNullOrEmpty(apPaterno) || apPaterno == "")
            {
                Mensaje = "Nombre del citado obligatorio!! ";
                return Mensaje;
            }


            if (string.IsNullOrEmpty(apMaterno) || apMaterno == "")
            {
                Mensaje = "Nombre del citado obligatorio!! ";
                return Mensaje;
            }

            if (idDestino == 0)
            {
                Mensaje = "Destino del citado obligatorio!! ";
                return Mensaje;
            }

            return Mensaje;
        }

        public String isValidNumeroAsignado()
        {
            String Mensaje = "SI";
            if (nroasignado <= 0)
            {
                Mensaje = "No se puede asignar ese número!! ";
            }
            return Mensaje;
        }


    }
}