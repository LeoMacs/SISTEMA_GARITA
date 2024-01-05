using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using ImudesaSystem_v4_.Models.Clases;


namespace ImudesaSystem_v4_.Models
{
    public class cita_mantenimiento
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Modelo"].ConnectionString);

      
        public DataTable get_CitasHoy()
        {
            SqlCommand com = new SqlCommand("sp_get_citasHoy", con);
            com.Parameters.AddWithValue("@idusuario", varGlobales.g_idusuario);
            com.Parameters.AddWithValue("@nSucursal", varGlobales.g_codsuc);
            com.Parameters.AddWithValue("@codPais", "PER");
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
        /********************************************************/
        public DataTable getCitasFiltradas(EntidadBusqueda e)
        {
            SqlCommand com = new SqlCommand("sp_get_citas", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@fechai", e.fechadesde);
            com.Parameters.AddWithValue("@fechaf", e.fechahasta);
            com.Parameters.AddWithValue("@nombre", e.nombre);
            com.Parameters.AddWithValue("@dni", e.dni);
            com.Parameters.AddWithValue("@destino", e.destino);
            com.Parameters.AddWithValue("@encargado", e.encargado);
            com.Parameters.AddWithValue("@nSucursal", varGlobales.g_codsuc);
            com.Parameters.AddWithValue("@idusuario", varGlobales.g_idusuario);

            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        /*******************************************************/
        public int insupd_cita(cita cita)
        {
            cita.cleanNulls();
            int resultado;
            //////////////////////////
            SqlCommand com;
            if (cita.idcita == 0)
            {
                com = new SqlCommand("sp_ins_cita", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@sucursal", varGlobales.g_codsuc);
             
            }
            else
            {
                com = new SqlCommand("sp_upd_cita", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@idcita", cita.idcita);
            }

            com.Parameters.AddWithValue("@fecha", cita.fecha);
            com.Parameters.AddWithValue("@hora", cita.hora);
            com.Parameters.AddWithValue("@dni", cita.dni);
            com.Parameters.AddWithValue("@nombres", cita.nombres);
            com.Parameters.AddWithValue("@apPaterno", cita.apPaterno);
            com.Parameters.AddWithValue("@apMaterno", cita.apMaterno);
            com.Parameters.AddWithValue("@idDestino", cita.idDestino);
            com.Parameters.AddWithValue("@encargado", cita.encargado);
            com.Parameters.AddWithValue("@empresapvnte", cita.empresa_proveniente);
            com.Parameters.AddWithValue("@observacion", cita.observacion);
            com.Parameters.AddWithValue("@idcitador", varGlobales.g_idusuario);
            com.Parameters.AddWithValue("@codPais", "PER");
            com.Parameters.AddWithValue("@user", varGlobales.g_nombreusuario);
            //////////////////////////
            SqlParameter varOutput = new SqlParameter("@nResultado", SqlDbType.Int);
            varOutput.Direction = ParameterDirection.Output;
            com.Parameters.Add(varOutput);
            //////////////////
            con.Open();
            com.ExecuteNonQuery();
            resultado = (int)com.Parameters["@nResultado"].Value;
            con.Close();
            Debug.WriteLine("valor retornado:" + resultado);
            return resultado;//si es -1 no realiza la accion, si es 1 si registra
           
        }

        /******************************************************/
        public DataTable get_citabyid(int id)
        {
            SqlCommand com = new SqlCommand("sp_get_citabyid", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@id", id);
            com.Parameters.AddWithValue("@codPais", "PER");
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        /**************************************************************************/
        public int del_cita(cita cita)
        {
            cita.cleanNulls();
            int resultado;
            SqlCommand com = new SqlCommand("sp_del_cita", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@idcita", cita.idcita);
            com.Parameters.AddWithValue("@codPais", "PER");
            com.Parameters.AddWithValue("@user", varGlobales.g_nombreusuario);
            //////////////////////////
            SqlParameter varOutput = new SqlParameter("@nResultado", SqlDbType.Int);
            varOutput.Direction = ParameterDirection.Output;
            com.Parameters.Add(varOutput);
            //////////////////
            con.Open();
            com.ExecuteNonQuery();
            resultado = (int)com.Parameters["@nResultado"].Value;
            con.Close();
            Debug.WriteLine("cita anulada:" + resultado);
            return resultado;/*si es -1 no realiza la accion, si es 1 si registra*/
        }



       
        public int confirm_citaAsistencia(cita c)
        {
            c.cleanNulls();
            int resultado;
            SqlCommand com = new SqlCommand("sp_ins_citaAsistencia", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@sucursal", varGlobales.g_codsuc);
            com.Parameters.AddWithValue("@idcita", c.idcita);
            com.Parameters.AddWithValue("@dni", c.dni);
            com.Parameters.AddWithValue("@numAsignado", c.nroasignado);
            com.Parameters.AddWithValue("@destino", c.destino);
            com.Parameters.AddWithValue("@encargado", c.encargado);
            com.Parameters.AddWithValue("@empresapvnte", c.empresa_proveniente);
            com.Parameters.AddWithValue("@codPais", "PER");
            com.Parameters.AddWithValue("@user", varGlobales.g_nombreusuario);
            //////////////////////////
            SqlParameter varOutput = new SqlParameter("@nResultado", SqlDbType.Int);
            varOutput.Direction = ParameterDirection.Output;
            com.Parameters.Add(varOutput);
            //////////////////
            con.Open();
            com.ExecuteNonQuery();
            resultado = (int)com.Parameters["@nResultado"].Value;
            con.Close();
            return resultado;//si es -1 no realiza la accion, si es 1 si registra
        }



        public String isvalidfiltros(EntidadBusqueda entidad)
        {
            String Mensaje = "SI";

            if (string.IsNullOrEmpty(entidad.fechadesde) || entidad.fechahasta == "" || string.IsNullOrEmpty(entidad.fechahasta) || entidad.fechahasta == "")
            {
                Mensaje = "Ingrese el rango de fechas!! ";
                return Mensaje;
            }
            else
            {
                string f1, f2;
                f1 = Reversefecha(entidad.fechadesde);
                f2 = Reversefecha(entidad.fechahasta);
                Debug.WriteLine("fechas:" + f1 + "--" + f2);
                if (string.Compare(f1, f2) == 1)
                {
                    Mensaje = "Fecha inicial no debe ser mayor a la fecha final!!";
                    return Mensaje;
                }


            }
            return Mensaje;
        }
        /**************************************************************/
       


       
        /*****************************************************************/
       

        public string Reversefecha(string s)
        {
            char[] charArr1 = s.ToCharArray();
            char[] temp = new Char[10];
            for (int i = 0; i < 4; i++)
            {
                temp[i] = charArr1[6 + i];//hasta i=3
            }
            temp[4] = '-';

            for (int i = 0; i < 2; i++)
            {
                temp[i + 5] = charArr1[3 + i];//hasta i=6
            }

            temp[7] = '-';
            for (int i = 0; i < 2; i++)
            {
                temp[i + 8] = charArr1[i];//hasta i=9
            }

            String fecha = new String(temp);

            return fecha;
        }


        /****************************************************/
        //public string DeshabilitarArea(int idarea)
        //{

        //    String disabled = "";

        //    if (idarea == 2)//Area
        //    {
        //        disabled = "true";
        //    }

        //    return disabled;

        //}
        ///****************************************************/
        //public string DeshabilitarSeguridad(int idperfil)
        //{
        //    String disabled = "";

        //    if (idperfil > 2)//Area
        //    {
        //        disabled = "true";
        //    }

        //    return disabled;
        //}


        ///*********************************************************/
        //public int registrarcitaAtendida(visitante visita)
        //{
        //    int resultado;
        //    SqlCommand com = new SqlCommand("Sp_updateasistenciavisita", con);
        //    com.CommandType = CommandType.StoredProcedure;
        //    com.Parameters.AddWithValue("@idvisitante", visita.idvisitante);

        //    con.Open();
        //    resultado = com.ExecuteNonQuery();
        //    con.Close();
        //    Debug.WriteLine("registros ingresados:" + resultado);
        //    return resultado;//si es -1 no realiza la accion, si es 1 si registra
        //}


        /************************************************************/


        ///**************************************************************************/
        //public int ocultarcita(cita cita)
        //{
        //    cita.cleanNulls();
        //    int resultado;
        //    SqlCommand com = new SqlCommand("[sp_updhidecita]", con);
        //    com.CommandType = CommandType.StoredProcedure;
        //    com.Parameters.AddWithValue("@idcita", cita.idcita);
        //    com.Parameters.AddWithValue("@user", varGlobales.g_nombreusuario);

        //    con.Open();
        //    resultado = com.ExecuteNonQuery();
        //    con.Close();
        //    Debug.WriteLine("cita actualizada:" + resultado);
        //    return resultado;/*si es -1 no realiza la accion, si es 1 si registra*/
        //}



    }
}