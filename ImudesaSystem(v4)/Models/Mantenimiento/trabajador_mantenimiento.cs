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
    public class trabajador_mantenimiento
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Modelo"].ConnectionString);


        public DataTable get_trabajadores()
        {
            SqlCommand com = new SqlCommand("sp_get_Trabajadores", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@idusuario", varGlobales.g_idusuario);
            com.Parameters.AddWithValue("@codsuc", varGlobales.g_codsuc);

            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public DataTable get_trabajadorbyid(int id)
        {

            SqlCommand com = new SqlCommand("sp_get_trabajadorbyid", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@idtrabajador", id);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public int insupd_trabajador(trabajador t)
        {
            t.cleanNulls();
            int resultado;
            SqlCommand com;
            if (t.idtrabajador == 0)
            {
                com = new SqlCommand("sp_ins_trabajador", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@codsuc", varGlobales.g_codsuc);
                com.Parameters.AddWithValue("@dni", t.dni);
                com.Parameters.AddWithValue("@nombres", t.nombres);
                com.Parameters.AddWithValue("@apPaterno", t.apPaterno);
                com.Parameters.AddWithValue("@apMaterno", t.apMaterno);
            }
            else
            {
                com = new SqlCommand("sp_upd_trabajador", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@idtrabajador", t.idtrabajador);
                com.Parameters.AddWithValue("@bactivo", t.habilitado);
                com.Parameters.AddWithValue("@fecRetiro", t.fecretiro);
            }

            com.Parameters.AddWithValue("@cargo", t.cargo);
            com.Parameters.AddWithValue("@idarea", t.idarea);
            com.Parameters.AddWithValue("@idempresa", t.idempresa);
            com.Parameters.AddWithValue("@fecIngreso", t.fecingreso);
            com.Parameters.AddWithValue("@rutaImage", t.rutaimage);
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
        public int del_trabajador(trabajador t)
        {
            t.cleanNulls();
            int resultado;
            SqlCommand com = new SqlCommand("sp_del_trabajador", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@idtrabajador", t.idtrabajador);
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
            Debug.WriteLine("trabajador anulado:" + resultado);
            return resultado;/*si es -1 no realiza la accion, si es 1 si registra*/
        }
        


        //public int Add_trabajador(trabajador t)
        //{
        //    t.cleanNulls();
        //    int resultado;

        //    SqlCommand com = new SqlCommand("sp_addtrabajador", con);

        //    com.CommandType = CommandType.StoredProcedure;

        //    com.Parameters.AddWithValue("@nombre", t.nombre);
        //    com.Parameters.AddWithValue("@dni", t.dni);
        //    com.Parameters.AddWithValue("@cargo", t.cargo);
        //    com.Parameters.AddWithValue("@area", t.area);
        //    com.Parameters.AddWithValue("@idempresa", t.idempresa);
        //    com.Parameters.AddWithValue("@userRegistro", varGlobales.g_nombreusuario);
        //    com.Parameters.AddWithValue("@codsuc", varGlobales.g_codsuc);


        //    con.Open();
        //    resultado = com.ExecuteNonQuery();
        //    con.Close();
        //    Debug.WriteLine("columnas afectadas:" + resultado);
        //    return resultado;//si es -1 no realiza la accion, si es 1 si registra

        //}

        //public int update_trabajador(trabajador t)
        //{
        //    cleanNulls(t);
        //    int resultado;

        //    SqlCommand com = new SqlCommand("sp_updatetrabajador", con);

        //    com.CommandType = CommandType.StoredProcedure;
        //    com.Parameters.AddWithValue("@idtrabajador", t.idtrabajador);
        //    com.Parameters.AddWithValue("@nombre", t.nombre);
        //    com.Parameters.AddWithValue("@dni", t.dni);
        //    com.Parameters.AddWithValue("@cargo", t.cargo);
        //    com.Parameters.AddWithValue("@area", t.area);
        //    com.Parameters.AddWithValue("@habilitado", t.habilitado);
        //    com.Parameters.AddWithValue("@conSeguro", t.conSeguro);

        //    com.Parameters.AddWithValue("@idempresa", t.idempresa);
        //    com.Parameters.AddWithValue("@userRegistro", varGlobales.g_nombreusuario);

        //    con.Open();
        //    resultado = com.ExecuteNonQuery();
        //    con.Close();
        //    Debug.WriteLine("Número:" + resultado);
        //    return resultado;//si es -1 no realiza la accion, si es 1 si actualiza
        //}

      

        public DataTable get_seguro(int idseguro)
        {
            SqlCommand com = new SqlCommand("sp_get_segurobyid", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@idseguro", idseguro);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public DataTable get_areas()
        {
            SqlCommand com = new SqlCommand("sp_get_Areas", con);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public DataTable get_areasconSeleccion(int idarea)
        {
            SqlCommand com = new SqlCommand("sp_get_AreasconSeleccion", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@idarea", idarea);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }



        //public List<trabajador> DataSeT_to_List(DataSet ds)
        //{

        //    List<trabajador> listrs = new List<trabajador>();
        //    foreach (DataRow dr in ds.Tables[0].Rows)
        //    {

        //        listrs.Add(new trabajador
        //        {
        //            idtrabajador=Convert.ToInt32(dr["idtrabajador"]),
        //            nombre = dr["nombre"].ToString(),
        //            dni = dr["dni"].ToString(),
        //            cargo = dr["cargo"].ToString(),
        //            area = dr["area"].ToString(),
        //            empresa = dr["empresa"].ToString(),
        //            estado = dr["estado"].ToString(),
        //            conSeguro = dr["conSeguro"].ToString(),
        //        });

        //    }
        //    return listrs;

        //}


    }
}