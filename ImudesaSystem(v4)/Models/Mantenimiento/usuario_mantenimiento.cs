using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Diagnostics;
using ImudesaSystem_v4_.Models.Clases;

namespace ImudesaSystem_v4_.Models
{
    public class usuario_mantenimiento
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Modelo"].ConnectionString);

        public DataSet get_sucursales()
        {
            SqlCommand com = new SqlCommand("sp_get_Sucursales", con);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }


        public DataTable get_usuarios()
        {
            SqlCommand com = new SqlCommand("sp_get_Usuarios", con);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

       
        public DataTable get_usuarioxID(int id)
        {
            SqlCommand com = new SqlCommand("sp_get_UsuarioxID", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@idusuario", id);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public int insupd_usuario(usuario u)
        {
           
            SqlCommand com;
            if (u.idusuario == 0)
            {

                com = new SqlCommand("sp_ins_usuario", con);
                com.CommandType = CommandType.StoredProcedure;
            }
            else
            {
                com = new SqlCommand("sp_upd_usuario", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@idusuario", u.idusuario);
            }

            com.Parameters.AddWithValue("@usuario", u.nombreusuario);
            com.Parameters.AddWithValue("@clave", u.clave);
            com.Parameters.AddWithValue("@idtrabajador", u.idtrabajador);
            //////////////////////////
            SqlParameter varOutput = new SqlParameter("@nResultado", SqlDbType.Int);
            varOutput.Direction = ParameterDirection.Output;
            com.Parameters.Add(varOutput);
            //////////////////
            con.Open();
            com.ExecuteNonQuery();
            int resultado = (int)com.Parameters["@nResultado"].Value;
            con.Close();
            Debug.WriteLine("valor retornado:" + resultado);
            return resultado;//si es -1 no realiza la accion, si es 1 si registra
        }

        public int del_Usuario(int id)
        {
            int resultado;
            SqlCommand com = new SqlCommand("sp_del_usuario", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@idusuario", id);
            //////////////////////////
            SqlParameter varOutput = new SqlParameter("@nResultado", SqlDbType.Int);
            varOutput.Direction = ParameterDirection.Output;
            com.Parameters.Add(varOutput);
            //////////////////
            con.Open();
            com.ExecuteNonQuery();
            resultado = (int)com.Parameters["@nResultado"].Value;
            con.Close();
            Debug.WriteLine("usuario anulado:" + resultado);
            return resultado;/*si es -1 no realiza la accion, si es 1 si registra*/
        }




        public DataSet iniciarSesion(usuario u)
        {
            cleanNulls(u);
            SqlCommand com = new SqlCommand("sp_iniciarSesion", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@cuenta",u.cuenta);
            com.Parameters.AddWithValue("@clave", u.clave);
            com.Parameters.AddWithValue("@codsuc", u.codsuc);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);
            Debug.WriteLine("sucursal:" + varGlobales.g_codsuc);

            return ds;
        }

        public DataSet get_AccesosMenuxUsuario(usuario u)
        {
            SqlCommand com = new SqlCommand("sp_get_AccesosMenuxUsuario", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@idusuario", u.idusuario);
            com.Parameters.AddWithValue("@codMenu", u.codMenu);
            com.Parameters.AddWithValue("@codOpcion", u.codOpcion);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }


        public String isvalidusuario(usuario u)
        {
            String mensaje = "SI";
           

            if (string.IsNullOrEmpty(u.cuenta ) || u.cuenta=="")
            {
                mensaje = "cuenta del usuario obligatoria!! ";
                return mensaje;
            }

            if (string.IsNullOrEmpty(u.clave) || u.clave == "")
            {
                mensaje = "contraseña del usuario obligatoria!! ";
                return mensaje;
            }
            return mensaje;
        }

        public void cleanNulls(usuario u)
        {
            if (string.IsNullOrEmpty(u.cuenta))
            {
                u.cuenta = "";
            }
            if(string.IsNullOrEmpty(u.clave) )
            {
                u.clave = "";
            }
            
           
        }


    }
}