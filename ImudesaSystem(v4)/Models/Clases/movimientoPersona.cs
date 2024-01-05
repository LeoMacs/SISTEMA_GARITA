using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
namespace ImudesaSystem_v4_.Models.Clases
{
    public class movimientoPersona
    {

        public int idMovimiento { get; set; }
        public string codsuc { get; set; }
        public string fecha { get; set; }
        public string horaentrada { get; set; }
        public string horasalida { get; set; }
        public int idPersona { get; set; }
        public string persona { get; set; }
        public string nombres { get; set; }
        public string apPaterno { get; set; }
        public string apMaterno { get; set; }
        public string dni { get; set; }
        public string destino { get; set; }
        public string encargado { get; set; }
        public string empresa_proveniente { get; set; }
        public string observacion { get; set; }
        public string nroasignado { get; set; }
        public string userRegistro { get; set; }
        public string modo { get; set; }
        /////////////////////////////////////
        public string noedit { get; set; }
        public string nodelete { get; set; }
        public string sisalida { get; set; }
        /////////////////////////////////////
        public string habilitado { get; set; }
        public string haSalido { get; set; }

        // validar movimiento
        public String isvalidMovimiento()
        {
            String Mensaje = "SI";
            Debug.WriteLine("visitante:" + idMovimiento);
            Debug.WriteLine("dni:" + dni);

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

            nombres = nombres.Trim();
            apPaterno = apPaterno.Trim();
            apMaterno = apMaterno.Trim();
            dni = dni.Trim();

            return Mensaje;
        }


        public void cleanNulls()
        {

            if (string.IsNullOrEmpty(destino) || destino.Trim() == "")
            {
                destino = "";
            }

            if (string.IsNullOrEmpty(encargado) || encargado.Trim() == "")
            {
                encargado = "";
            }


            if (string.IsNullOrEmpty(empresa_proveniente) || empresa_proveniente.Trim() == "")
            {
                empresa_proveniente = "";
            }

            if (string.IsNullOrEmpty(observacion) || observacion.Trim() == "")
            {
                observacion = "";
            }

        }



    }
}