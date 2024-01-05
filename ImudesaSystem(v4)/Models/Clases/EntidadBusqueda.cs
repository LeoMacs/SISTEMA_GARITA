using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImudesaSystem_v4_.Models
{
    public class EntidadBusqueda
    {
        public String fechadesde { get; set; }
        public String fechahasta { get; set; }
        /************************************/
        public String proveedor { get; set; }
        public String destino { get; set; }
        public String nombre { get; set; }
        public String dni { get; set; }
        public String encargado { get; set; }
        public String empresa { get; set; }
        /************************************/
        public String modo { get; set; }
        /************************************/
        public String licencia { get; set; }
        public String proviene { get; set; }
        public String placa { get; set; }
        public String cliente { get; set; }

        public void inicializadorBuscador()
        {
            
            if (nombre == null)
            {
                nombre = "";
            }
            if (dni == null)
            {
                dni = "";
            }
            if (proveedor == null)
            {
                proveedor = "";
            }
            if (destino == null)
            {
                destino = "";
            }

            if (encargado == null)
            {
                encargado = "";
            }

            nombre = nombre.Trim();
            dni = dni.Trim();
            proveedor = proveedor.Trim();
            destino = destino.Trim();
            encargado = encargado.Trim();
            
        }







    }
}