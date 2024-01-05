using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
//using Microsoft.Reporting.Map.WebForms.BingMaps;
using System.Diagnostics;
using ImudesaSystem_v4_.Models.Clases;

namespace ImudesaSystem_v4_.Models
{
    public class empresa_mantenimiento
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Modelo"].ConnectionString);

        public int insupd_empresa(empresa e)
        {
            e.cleanNulls();
            SqlCommand com;
            if (e.idempresa == 0)
            {
                
                com = new SqlCommand("sp_ins_empresa", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@ruc", e.ruc);
                com.Parameters.AddWithValue("@codsuc", varGlobales.g_codsuc);
            }
            else
            {
                com = new SqlCommand("sp_upd_empresa", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@idempresa", e.idempresa);
            }

            com.Parameters.AddWithValue("@nombre", e.nombre);
            com.Parameters.AddWithValue("@descripcion", e.descripcion);
            com.Parameters.AddWithValue("@user", e.descripcion);
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



        //public int Add_empresa(empresa emp)
        //{

        //    cleanNulls(emp);
        //    int resultado ;
        //    SqlCommand com = new SqlCommand("sp_addempresa", con);
        //    com.CommandType = CommandType.StoredProcedure;
        //    com.Parameters.AddWithValue("@nombre", emp.nombre);
        //    com.Parameters.AddWithValue("@descripcion", emp.descripcion);
        //    com.Parameters.AddWithValue("@userRegistro", emp.descripcion);
        //    com.Parameters.AddWithValue("@codsuc", varGlobales.g_codsuc);


        //    con.Open();
        //    resultado=com.ExecuteNonQuery();
        //    con.Close();
        //    Debug.WriteLine("Número:" + resultado);
        //    return resultado;


        //}


        //public int update_empresa(empresa emp)
        //{
        //    cleanNulls(emp);
        //    int resultado;
        //    SqlCommand com = new SqlCommand("Sp_updatempresa", con);
        //    com.CommandType = CommandType.StoredProcedure;
        //    com.Parameters.AddWithValue("@idempresa",emp.idempresa);
        //    com.Parameters.AddWithValue("@nombre", emp.nombre);
        //    com.Parameters.AddWithValue("@descripcion", emp.descripcion);
        //    com.Parameters.AddWithValue("@userRegistro", varGlobales.g_nombreusuario);

        //    con.Open();
        //    resultado = com.ExecuteNonQuery();
        //    con.Close();
        //    Debug.WriteLine("Número:" + resultado);
        //    return resultado;
        //}



        public DataSet get_empresabyid(int id)
        {
            SqlCommand com = new SqlCommand("sp_get_Empresabyid", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@idempresa", id);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }


        public DataTable get_empresas()
        {
            SqlCommand com = new SqlCommand("sp_get_Empresas", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@codsuc", varGlobales.g_codsuc);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;

        }

        public DataTable get_empresasconSeleccion(int idempresa)
        {
            SqlCommand com = new SqlCommand("sp_get_EmpresasconSeleccion", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@idempresa", idempresa);
            com.Parameters.AddWithValue("@codsuc", varGlobales.g_codsuc);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public DataSet get_SCTRxEmpresa(int id)
        {
            SqlCommand com = new SqlCommand("sp_get_SCTRxEmpresa", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@idempresa", id);
            com.Parameters.AddWithValue("@nSucursal", varGlobales.g_codsuc);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        public DataSet get_Personalxempresa(int id)
        {
            SqlCommand com = new SqlCommand("sp_get_PersonalxEmpresa", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@idempresa", id);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }





        public void deleteempresa(int id)
        {

            SqlCommand com = new SqlCommand("Sp_deleteusuario", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@idempresa", id);
            con.Open();
            com.ExecuteNonQuery();
            con.Close();
        }

       

        /*********************************************************/

        public List<empresa> DataSetToList(DataSet ds)
        {
            List<empresa> empresas = new List<empresa>();

            if (ds != null)
            {
                empresas = ds.Tables[0].AsEnumerable().Select(dataRow => new empresa
                {
                    nombre = dataRow.Field<string>("nombre"),
                    descripcion = dataRow.Field<string>("descripcion")
                    
                }).ToList();
            }

            return empresas;
        }


       
    }


}