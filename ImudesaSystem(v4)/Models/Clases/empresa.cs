using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImudesaSystem_v4_.Models.Clases
{
    public class empresa
    {
        public int idempresa { get; set; }
        public string codsuc { get; set; }
        public string ruc { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public string estado { get; set; }

        public String isvalidEmpresa()
        {
            String Mensaje = "SI";

            if (string.IsNullOrEmpty(nombre) || nombre == "")
            {
                Mensaje = "Nombre de la empresa obligatorio!! ";
                return Mensaje;
            }
            return Mensaje;
        }

        public void cleanNulls( )
        {
            if (string.IsNullOrEmpty(descripcion))
            {
                descripcion = "";
            }
        }

    

    }


     
}