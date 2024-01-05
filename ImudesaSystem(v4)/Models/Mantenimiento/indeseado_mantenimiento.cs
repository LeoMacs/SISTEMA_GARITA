using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Net;
using System.Net.Mail;
using System.Net.Configuration;
using ImudesaSystem_v4_.Models.Clases;

namespace ImudesaSystem_v4_.Models
{
    public class indeseado_mantenimiento
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Modelo"].ConnectionString);

        public DataTable getIndeseados()
        {
            SqlCommand com = new SqlCommand("sp_get_indeseados", con);
            com.Parameters.AddWithValue("@idusuario", varGlobales.g_idusuario);
            com.Parameters.AddWithValue("@nSucursal", varGlobales.g_codsuc);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public DataTable get_indeseadobyIdDni(int id, string dni, string modo)
        {
            SqlCommand com = new SqlCommand("sp_get_indeseadobyIdDni", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@id", id);
            com.Parameters.AddWithValue("@dni", dni);
            com.Parameters.AddWithValue("@modo", modo);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }


        public Boolean verificarIndeseado(string dni)
        {
            if (dni == null)
            {
                dni = "";
            }
            Boolean isIndeseado = false;
            SqlCommand com = new SqlCommand("SELECT dbo.fn_get_verificarIndeseado(@dni)", con);
            com.Parameters.AddWithValue("@dni", dni);
            con.Open();
            int resultado = Convert.ToInt32(com.ExecuteScalar());
            con.Close();
            if (resultado != 0)
            {
                isIndeseado = true;
                Debug.WriteLine("indeseado detectado en los registros!!" + resultado);
            }
            return isIndeseado;//si es true se encontró un visitante en la lista negra
        }



        public int insupd_indeseado(indeseado i)
        {
            i.cleanNulls();
            int resultado;
            SqlCommand com;
            if (i.idindeseado == 0)
            {
                com = new SqlCommand("sp_ins_indeseado", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@sucursal",varGlobales.g_codsuc );
                com.Parameters.AddWithValue("@dni", i.dni);
                com.Parameters.AddWithValue("@nombres", i.nombres);
                com.Parameters.AddWithValue("@apPaterno", i.apPaterno);
                com.Parameters.AddWithValue("@apMaterno", i.apMaterno);
            }
            else
            {
                com = new SqlCommand("sp_upd_indeseado", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@idindeseado", i.idindeseado);
            }

            com.Parameters.AddWithValue("@cargo", i.cargo);
            com.Parameters.AddWithValue("@empresapvnte", i.empresaPvnte);
            com.Parameters.AddWithValue("@observacion", i.observacion);
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
            Debug.WriteLine("registros ingresados:" + resultado);
            return resultado;//si es -1 no realiza la accion, si es 1 si registra
        }

        /**************************************************************************/
        public int del_indeseado(indeseado i)
        {
            i.cleanNulls();
            int resultado;
            SqlCommand com = new SqlCommand("sp_del_indeseado", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@idindeseado", i.idindeseado);
            com.Parameters.AddWithValue("@motAnular", i.motAnular);
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
            Debug.WriteLine("indeseado anulado:" + resultado);
            return resultado;/*si es -1 no realiza la accion, si es 1 si registra*/
        }
        

       


        //public List<indeseado> DataSetToList(DataSet ds)
        //{
        //    List<indeseado> indeseados = new List<indeseado>();

        //    if (ds != null)
        //    {
        //        indeseados = ds.Tables[0].AsEnumerable().Select(dataRow => new indeseado
        //        {
        //            nombre = dataRow.Field<string>("nombre"),
        //            apellido = dataRow.Field<string>("apellido"),
        //            dni = dataRow.Field<string>("dni"),
        //            cargo = dataRow.Field<String>("cargo"),
        //            empresa = dataRow.Field<string>("empresa"),
        //            observacion = dataRow.Field<String>("observacion"),
        //            fechaRegistro = dataRow.Field<string>("fechaRegistro")

        //        }).ToList();
        //    }


        //    return indeseados;
        //}



        //        public void reportarIndeseado(indeseado indeseado)
        //        {
        //            var smtpSection = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
        //            string usuario = smtpSection.Network.UserName ;
        //            string password = smtpSection.Network.Password;
        //            string servidor = smtpSection.Network.Host ;
        //            int puerto = smtpSection.Network.Port ;
        //            string rec1 = "leomacs1009@gmail.com";
        //            string rec2 = "makito_1008@hotmail.com";
        //            string rec3 = "marco.cueva@unmsm.edu.pe";


        //            String contenido = "<html>" +
        //                "<head >" +
        //    "<title></title>" +
        //"</head>" +
        //"<body>" +

        //    "<div>" +
        //    "<p>Un visitante registrado en la lista negra intentó ingresar.</p>" +
        //    "</div>" +
        //    "<div >" +
        //        "<h3 style='font-weight:bold;color:blue'>" +
        //            "Datos del Visitante" +
        //        "</h3>" +
        //        "<p>Dni:<label style='border: 2px solid;font-weight:bold;'> " + indeseado.dni + " </label></p>" +
        //        "<p>Nombres y Apellidos: <label style='border: 2px solid;font-weight:bold;'> " + indeseado.nombre +" "+indeseado.apellido+ " </label></p>" +
        //        "<p>Empresa proveniente: <label style='border: 2px solid;font-weight:bold;'>  " + indeseado.empresa + " </label></p>" +
        //        "<p>Fecha de registro en la lista negra: <label style='border: 2px solid;font-weight:bold;'> " + indeseado.fechaRegistro + " </label></p>" +
        //        "<p>razón del registro: <label style='border: 2px solid;font-weight:bold;'> " + indeseado.observacion + " </label></p>" +
        //    "</div>" +
        //     "<div>" +
        //        "<img style='width:160px;height:60px' src='http://190.8.140.44/sil/imagenes/logo2_int.gif' />" +
        //    "</div>" +
        //"</body>" +
        //"</html>";

        //            //Puerto 25 funciona
        //            using (SmtpClient cliente = new SmtpClient(servidor, puerto))
        //            {

        //                //khuamani@agunsa.com.pe
        //                cliente.EnableSsl = true;
        //                cliente.Credentials = new NetworkCredential(usuario,password);
        //                MailMessage mensaje = new MailMessage(usuario, rec1)
        //                {
        //                    Subject = "ALERTA DE APLICACIÓN WEB GARITA",
        //                    Body = contenido,
        //                    IsBodyHtml = true

        //                };
        //                mensaje.CC.Add(rec2);
        //                mensaje.CC.Add(rec3);


        //                //,"Alerta de Aplicación Web Garita",contenido);
        //                try
        //                {
        //                    cliente.Send(mensaje);

        //                }
        //                catch (Exception e)
        //                {
        //                    Debug.WriteLine("error:" + e.Message);

        //                }
        //            }
        //        }


        //public indeseado getIndeseado(String dni)
        //{
        //    SqlCommand com = new SqlCommand("Sp_indeseadobyDni", con);
        //    com.CommandType = CommandType.StoredProcedure;
        //    com.Parameters.AddWithValue("@dni", dni);
        //    con.Open();
        //    SqlDataReader registros = com.ExecuteReader();
        //    indeseado indeseado = new indeseado();
        //    if (registros.Read())
        //    {
        //        indeseado.idindeseado = int.Parse(registros["idindeseado"].ToString());
        //        indeseado.persona = registros["persona"].ToString();
        //        indeseado.dni = registros["dni"].ToString();
        //        indeseado.empresaPvnte = registros["empresaPvnte"].ToString();
        //        indeseado.fechaRegistro = registros["fechaRegistro"].ToString();
        //        indeseado.observacion = registros["observacion"].ToString();
        //        indeseado.cargo = registros["cargo"].ToString();
        //    }
        //    con.Close();
        //    return indeseado;
        //}

    }
}