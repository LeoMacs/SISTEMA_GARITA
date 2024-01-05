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
    public class asistencia_mantenimiento
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Modelo"].ConnectionString);

        public DataTable getTrabajadores()
        {
            SqlCommand com = new SqlCommand("Sp_getTrabajadoresForAsistenciasHoy", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@codsuc", varGlobales.g_codsuc);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public DataTable getAsistenciasHoy()
        {
            SqlCommand com = new SqlCommand("sp_get_asistenciasTrabajadoresHoy", con);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }


        public int addAsistencia(trabajador t)
        {
            int resultado;
            SqlCommand com = new SqlCommand("sp_addasistencia", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@fecha", t.fecha);
            com.Parameters.AddWithValue("@horainicio", t.hora);
            com.Parameters.AddWithValue("@idtrabajador", t.idtrabajador);
            com.Parameters.AddWithValue("@empresa", t.empresa);
            com.Parameters.AddWithValue("@userRegistro", varGlobales.g_nombreusuario);
            com.Parameters.AddWithValue("@codsuc", varGlobales.g_codsuc);

           
            con.Open();
            resultado = com.ExecuteNonQuery();
            con.Close();
            Debug.WriteLine("registros ingresados:" + resultado);
            return resultado;//si es -1 no realiza la accion, si es 1 si registra
        }


        public int addSalida(asistencia a)
        {
            int resultado;
            SqlCommand com = new SqlCommand("Sp_updateasistencia", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@idasistencia", a.idasistencia);
            com.Parameters.AddWithValue("@user", varGlobales.g_nombreusuario);


            con.Open();
            resultado = com.ExecuteNonQuery();
            con.Close();
            Debug.WriteLine("registros ingresados:" + resultado);
            return resultado;//si es -1 no realiza la accion, si es 1 si registra
        }




        public DataTable getAsistenciaslimite()
        {
            SqlCommand com = new SqlCommand("Sp_getAsistenciasConLimite", con);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }


        public DataTable getAsistenciasFiltradas(EntidadBusqueda entidad)
        {
            cleanNulls(entidad);
            Debug.WriteLine("filtros:" + entidad.fechadesde+"--"+entidad.fechahasta+"--"+entidad.nombre+"--codsuc:"+varGlobales.g_codsuc);

            SqlCommand com = new SqlCommand("Sp_getAsistenciasConFiltros", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@fechadesde", entidad.fechadesde);
            com.Parameters.AddWithValue("@fechahasta", entidad.fechahasta);
            com.Parameters.AddWithValue("@nombre", entidad.nombre);
            com.Parameters.AddWithValue("@dni", entidad.dni);
            com.Parameters.AddWithValue("@empresa", entidad.empresa);
            com.Parameters.AddWithValue("@codsuc", varGlobales.g_codsuc);

            
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }



        public DataTable getAsistencia(int idasistencia)
        {
            SqlCommand com = new SqlCommand("Sp_asistenciabyid", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@idasistencia", idasistencia);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

       

        public int eliminarAsistencia(int idasistencia)
        {
           
            SqlCommand comando = new SqlCommand("delete from asistencia where idasistencia=@idasistencia", con);
            comando.Parameters.Add("@idasistencia", SqlDbType.Int);
            comando.Parameters["@idasistencia"].Value = idasistencia;
            con.Open();
            int i = comando.ExecuteNonQuery();
            con.Close();
            return i;
        }


        public String isvalidfiltros(EntidadBusqueda entidad)
        {
            String Mensaje = "SI";

            if (string.IsNullOrEmpty(entidad.fechadesde)  || entidad.fechahasta == "" ||string.IsNullOrEmpty(entidad.fechahasta) || entidad.fechahasta == "")
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
                if (string.Compare(f1, f2)==1)
                {
                    Mensaje = "Fecha inicial no debe ser mayor a la fecha final!!";
                    return Mensaje;
                }


            }
            return Mensaje;
        }


        public void cleanNulls(EntidadBusqueda entidad)
        {
            if (string.IsNullOrEmpty(entidad.nombre))
            {
                entidad.nombre = "";
            }

            if (string.IsNullOrEmpty(entidad.dni))
            {
                entidad.dni = "";
            }

            if (string.IsNullOrEmpty(entidad.empresa ) )
            {
                entidad.empresa = "";
            }
        }

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


        public List<asistencia> DataSeT_to_List(DataSet ds)
        {

            List<asistencia> listrs = new List<asistencia>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                listrs.Add(new asistencia
                {
                    
                    nombre = dr["nombre"].ToString(),
                    dni = dr["dni"].ToString(),
                    empresa = dr["empresa"].ToString(),
                    fecha = dr["fecha"].ToString(),
                    horainicio = dr["horainicio"].ToString(),
                    horasalida = dr["horasalida"].ToString()

                });

            }
            return listrs;

        }



    }
}