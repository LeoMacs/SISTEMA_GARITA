using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImudesaSystem_v4_.Models.Clases
{
    public class cargaVehicular
    {
        public int idcargavehiculo { get; set; }
        public string fecha { get; set; }
        public string horaentrada { get; set; }
        public string horasalida { get; set; }
        public string empresa_proveniente { get; set; }
        public int  idempresa_dtno { get; set; }
        public string empresa_dtno { get; set; }
        public string nombres { get; set; }
        public string apPaterno { get; set; }
        public string apMaterno { get; set; }
        public string dni { get; set; }
        public int idPersona { get; set; }
        public string persona { get; set; }
        public string licencia { get; set; }
        public string placa { get; set; }
        public int bunidad { get; set; }
        public int bcarreta { get; set; }
        public string isunidad { get; set; }
        public string iscarreta { get; set; }
        public string codcarreta { get; set; }
        public string numContenedor { get; set; }
        public string tipoContenedor { get; set; }
        public string estadContenedor { get; set; }
        public string precintoGuia { get; set; }
        public int estado { get; set; }
       
        
        public string hasalido { get; set; }
        public string noedit { get; set; }
        public string nodelete { get; set; }
    }
}