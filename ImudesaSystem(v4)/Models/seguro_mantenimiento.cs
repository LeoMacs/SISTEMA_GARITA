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
    public class seguro_mantenimiento
    {
        /*******************************************************/
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Modelo"].ConnectionString);

        public DataTable get_segurobyid(int id)
        {
            SqlCommand com = new SqlCommand("sp_get_segurobyid", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@idseguro", id);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }


        public DataTable get_seguros()
        {
            SqlCommand com = new SqlCommand("sp_get_seguros", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@idusuario", varGlobales.g_idusuario);
            com.Parameters.AddWithValue("@codsuc", varGlobales.g_codsuc);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }



        public int insupd_seguro(seguro s)
        {

            s.cleanNulls();
            int resultado;

            SqlCommand com;
            if (s.idseguro == 0)
            {

                com = new SqlCommand("sp_ins_seguro", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@codsuc", varGlobales.g_codsuc);

            }
            else
            {
                com = new SqlCommand("sp_upd_seguro", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@idseguro", s.idseguro);

            }



            com.Parameters.AddWithValue("@nombre", s.nombre);
            com.Parameters.AddWithValue("@nrosalud", s.nrosalud);
            com.Parameters.AddWithValue("@nropoliza", s.nropoliza);
            com.Parameters.AddWithValue("@idempresa", s.idempresa);
            com.Parameters.AddWithValue("@fechainicio", s.fechainicio);
            com.Parameters.AddWithValue("@fechafin", s.fechafin);
            com.Parameters.AddWithValue("@user", varGlobales.g_nombreusuario);
            //////////////////////////
            SqlParameter varOutput = new SqlParameter("@nResultado", SqlDbType.Int);
            varOutput.Direction = ParameterDirection.Output;
            com.Parameters.Add(varOutput);
            //////////////////
            con.Open();
            resultado = com.ExecuteNonQuery();
            con.Close();
            Debug.WriteLine("columnas afectadas:" + resultado);
            return resultado;//si es -1 no realiza la accion, si es 1 si registra


        }


        public int del_seguro(int idseguro)
        {
            int resultado;

            SqlCommand com = new SqlCommand("sp_del_seguro", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@idseguro", idseguro);
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
            Debug.WriteLine("seguro anulado");
            return resultado;//si es -1 no realiza la accion, si es 1 si actualiza
        }






        public DataTable get_empDispForSeguro()
        {
            SqlCommand com = new SqlCommand("Sp_getEmpDispForSeguro", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@codsuc", varGlobales.g_codsuc);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }



        public DataTable get_PersonalxSeguro(int id)
        {
            SqlCommand com = new SqlCommand("sp_get_PersonalxSeguro", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@idseguro", id);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public DataTable get_PersonalSinSeguro(int id)
        {
            SqlCommand com = new SqlCommand("sp_get_PersonalSinSeguro", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@idseguro", id);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        ////////////////////////////////////////////////
        public int add_afiliado(trabajador t)
        {
            t.cleanNulls();
            int resultado;

            SqlCommand com = new SqlCommand("sp_ins_seguroTrabajador", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@idseguro", t.idseguro);
            com.Parameters.AddWithValue("@idtrabajador", t.idtrabajador);
            com.Parameters.AddWithValue("@user", varGlobales.g_nombreusuario);
            //////////////////////////
            SqlParameter varOutput = new SqlParameter("@nResultado", SqlDbType.Int);
            varOutput.Direction = ParameterDirection.Output;
            com.Parameters.Add(varOutput);
            //////////////////
            con.Open();
            resultado = com.ExecuteNonQuery();
            con.Close();
            Debug.WriteLine("columnas afectadas:" + resultado);
            return resultado;//si es -1 no realiza la accion, si es 1 si registra


        }


        public int del_afiliado(trabajador t)
        {
            int resultado;

            SqlCommand com = new SqlCommand("sp_del_seguroTrabajador", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@idsegtrab", t.idsegtrab);
            //////////////////////////
            SqlParameter varOutput = new SqlParameter("@nResultado", SqlDbType.Int);
            varOutput.Direction = ParameterDirection.Output;
            com.Parameters.Add(varOutput);
            //////////////////
            con.Open();
            com.ExecuteNonQuery();
            resultado = (int)com.Parameters["@nResultado"].Value;
            con.Close();
            Debug.WriteLine("afiliado anulado");
            return resultado;//si es -1 no realiza la accion, si es 1 si actualiza
        }






    }
}