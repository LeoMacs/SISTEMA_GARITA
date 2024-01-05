//------------------------------------------------------------------------------
// <auto-generated>
//    Este código se generó a partir de una plantilla.
//
//    Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//    Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ImudesaSystem_v4_.Models.Clases
{
    using System;
    using System.Collections.Generic;

    public partial class seguro
    {
        public int idseguro { get; set; }
        public string nombre { get; set; }
        public string nrosalud { get; set; }
        public string nropoliza { get; set; }
        public string fechainicio { get; set; }
        public string fechafin { get; set; }
        public int idempresa { get; set; }
        public int flagactivo { get; set; }
        public string usucreacion { get; set; }
        public string feccreacion { get; set; }
        public string usumodificacion { get; set; }
        public string fecmodificacion { get; set; }
        public string codsuc { get; set; }
        public string estado { get; set; }
        public string colorEstado { get; set; }


        public string empresa { get; set; }
        /////////////////////
        public string creador { get; set; }
        public string modificador { get; set; }
        public string noedit { get; set; }
        public string nodelete { get; set; }


        public String isvalidseguro()
        {
            String Mensaje = "SI";
           

            if (idempresa < 1)
            {
                Mensaje = "empresa del seguro obligatoria!! ";
                return Mensaje;
            }

            if (string.IsNullOrEmpty(nombre) || nombre == "")
            {
                Mensaje = "Nombre del seguro obligatorio!! ";
                return Mensaje;
            }

            if (string.IsNullOrEmpty(fechainicio) || fechainicio == "")
            {
                Mensaje = "fecha de inicio de vigencia obligatoria!! ";
                return Mensaje;
            }

            if (string.IsNullOrEmpty(fechafin) || fechafin == "")
            {
                Mensaje = "fecha de fin de vigencia obligatoria!! ";
                return Mensaje;
            }


            return Mensaje;
        }

        public void cleanNulls()
        {
            if (string.IsNullOrEmpty(nropoliza))
            {
                nropoliza = "";
            }

            if (string.IsNullOrEmpty(nrosalud))
            {
                nrosalud = "";
            }
        }


    }
}
