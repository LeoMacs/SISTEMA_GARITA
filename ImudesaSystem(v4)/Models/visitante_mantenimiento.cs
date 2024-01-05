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
    public class visitante_mantenimiento
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Modelo"].ConnectionString);


        public DataSet get_visitantesHoy()
        {
            SqlCommand com = new SqlCommand("sp_getVisitasHoy", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@codsuc", varGlobales.g_codsuc);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;

        }

        public int Add_visitante(visitante visita)
        {
            cleanNulls(visita);
            int resultado;
            SqlCommand com = new SqlCommand("sp_addvisitante", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@nombre", visita.nombre);
            com.Parameters.AddWithValue("@dni", visita.dni);
            com.Parameters.AddWithValue("@fecha", visita.fecha);
            com.Parameters.AddWithValue("@horaentrada", visita.horaentrada);
            com.Parameters.AddWithValue("@destino", visita.destino);
            com.Parameters.AddWithValue("@encargado", visita.encargado);
            com.Parameters.AddWithValue("@empresa_proveniente", visita.empresa_proveniente);
            com.Parameters.AddWithValue("@nroasignado", visita.nroasignado);
            com.Parameters.AddWithValue("@codsuc", varGlobales.g_codsuc);
            com.Parameters.AddWithValue("@userRegistro", varGlobales.g_nombreusuario);
            com.Parameters.AddWithValue("@idcita", 0);


            con.Open();
            resultado=com.ExecuteNonQuery();
            con.Close();
            Debug.WriteLine("visitas ingresadas al sistema:" + resultado);
            return resultado;//si es -1 no realiza la accion, si es 1 si registra
        }



        public int registrarSalida(visitante visita)
        {
            int resultado;
            SqlCommand com = new SqlCommand("Sp_updateasistenciavisita", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@idvisitante", visita.idvisitante);

            con.Open();
            resultado = com.ExecuteNonQuery();
            con.Close();
            Debug.WriteLine("registros ingresados:" + resultado);
            return resultado;//si es -1 no realiza la accion, si es 1 si registra
        }


        public int update_visita(visitante visita)
        {
            cleanNulls(visita);
            int resultado;
            SqlCommand com = new SqlCommand("[sp_updatevisitante]", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@idvisitante", visita.idvisitante);
            com.Parameters.AddWithValue("@nombre", visita.nombre);
            com.Parameters.AddWithValue("@dni", visita.dni);
            com.Parameters.AddWithValue("@destino", visita.destino);
            com.Parameters.AddWithValue("@encargado", visita.encargado);
            com.Parameters.AddWithValue("@empresa_proveniente", visita.empresa_proveniente);

            con.Open();
            resultado = com.ExecuteNonQuery();
            con.Close();
            Debug.WriteLine("visitas ingresadas al sistema:" + resultado);
            return resultado;/*si es -1 no realiza la accion, si es 1 si registra*/
        }

        public DataSet getVisitantesFiltradas(EntidadBusqueda e)
        {
            SqlCommand com = new SqlCommand("Sp_getVisitasConFiltros", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@fechai", e.fechadesde);
            com.Parameters.AddWithValue("@fechaf", e.fechahasta);
            com.Parameters.AddWithValue("@nombre", e.nombre);
            com.Parameters.AddWithValue("@dni", e.dni);
            com.Parameters.AddWithValue("@destino", e.destino);
            com.Parameters.AddWithValue("@empresa_proveniente", e.proveedor);
            com.Parameters.AddWithValue("@codsuc", varGlobales.g_codsuc);

            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }


        public List<visitante> getVisitantesFiltradasForExcel(String fechai, String fechaf, String nombre, String dni, String destino, String empresa_proveniente)
        {
            List<visitante> visitantes = new List<visitante>();
            con.Open();
            SqlCommand com = new SqlCommand("sp_getVisitasxFiltrosGarita", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@fechai",fechai);
            com.Parameters.AddWithValue("@fechaf",fechaf);
            com.Parameters.AddWithValue("@nombre",nombre);
            com.Parameters.AddWithValue("@dni", dni);
            com.Parameters.AddWithValue("@destino", destino);
            com.Parameters.AddWithValue("@empresa_proveniente", empresa_proveniente);
            
            SqlDataReader registros = com.ExecuteReader();
            while (registros.Read())
            {
                visitante visitante = new visitante
                {
                    
                    fecha = registros["fecha"].ToString(),
                    horaentrada = registros["horaentrada"].ToString(),
                    horasalida = registros["horasalida"].ToString(),
                    nombre = registros["nombre"].ToString(),
                    dni = registros["dni"].ToString(),
                    destino = registros["destino"].ToString(),
                    encargado = registros["encargado"].ToString(),
                    empresa_proveniente = registros["empresa_proveniente"].ToString()
                };
                visitantes.Add(visitante);
            }
            
            return visitantes;
        }


        // Get visita by id
        public DataSet get_visitabyid(int id)
        {
            SqlCommand com = new SqlCommand("Sp_visitantebyid", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@idvisitante", id);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        // Delete record

        public void deletevisita(int id)
        {
            SqlCommand com = new SqlCommand("Sp_deletevisita", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@idvisitante", id);
            con.Open();
            com.ExecuteNonQuery();
            con.Close();
        }



        public String HabilitarMarcarSalida(String salida)
        {
            String cadena = "";
            if (salida != "")
            {
                cadena = "true";
            }
            return cadena;
        }

        public void inicializadorBuscador(EntidadBusqueda e)
        {
            if (e.nombre==null)
            {
                e.nombre = "";
            }
            if (e.dni == null)
            {
                e.dni = "";
            }
            if (e.proveedor == null)
            {
                e.proveedor = "";
            }
            if (e.destino == null)
            {
                e.destino = "";

            }
        }

        public List<visitante> DataSetToList(DataSet ds)
        {
            List<visitante> visitantes = new List<visitante>();

            if (ds != null)
            {
                visitantes = ds.Tables[0].AsEnumerable().Select(dataRow => new visitante
                {
                    fecha = dataRow.Field<string>("fecha"),
                    horaentrada = dataRow.Field<string>("horaentrada"),
                    horasalida = dataRow.Field<String>("horasalida"),
                    nombre = dataRow.Field<string>("nombre"),
                    dni = dataRow.Field<String>("dni"),
                    destino = dataRow.Field<String>("destino"),
                    encargado = dataRow.Field<string>("encargado"),
                    empresa_proveniente = dataRow.Field<String>("empresa_proveniente")

                }).ToList();
            }
            
           
            return visitantes;
        }

        /*
       
        public Visitante getVisitante(int idvisitante)
        {
            Conectar();
            SqlCommand comando = new SqlCommand("select idvisitante,FORMAT(fecha,'yyyy-MM-dd') as fecha,horaentrada,horasalida,nombre,dni,destino,encargado,empresa_proveniente from visitante where idvisitante=@idvisitante", con);
            comando.Parameters.Add("@idvisitante", SqlDbType.Int);
            comando.Parameters["@idvisitante"].Value = idvisitante;
            con.Open();
            SqlDataReader registros = comando.ExecuteReader();
            Visitante visitante = new Visitante();
            if (registros.Read())
            {
                visitante.idvisitante = int.Parse(registros["idvisitante"].ToString());
                visitante.fecha = registros["fecha"].ToString();
                visitante.horaentrada = registros["horaentrada"].ToString();
                visitante.horasalida = registros["horasalida"].ToString();
                visitante.nombre = registros["nombre"].ToString();
                visitante.dni = registros["dni"].ToString();
                visitante.destino = registros["destino"].ToString();
                visitante.encargado = registros["encargado"].ToString();
                visitante.empresa_proveniente = registros["empresa_proveniente"].ToString();
            }
            con.Close();
            return visitante;
        }



        */


        public String isvalidvisita(visitante v)
        {
            String Mensaje = "SI";
            Debug.WriteLine("visitante:" + v.idvisitante);
            Debug.WriteLine("dni:" + v.dni);

            
            if (string.IsNullOrEmpty(v.nombre) || v.nombre == "")
            {
                Mensaje = "Nombre del visitante obligatorio!! ";
                return Mensaje;
            }

            if (string.IsNullOrEmpty(v.dni) || v.dni == "")
            {
                Mensaje = "Dni del visitante obligatorio!! ";
                return Mensaje;
            }

            if (v.dni.Length != 8)
            {
                Mensaje = "Dni debe contener 8 dígitos!! ";
                return Mensaje;
            }

            if (string.IsNullOrEmpty(v.destino) || v.destino == "")
            {
                Mensaje = "Destino del visitante obligatorio!! ";
                return Mensaje;
            }

            if (string.IsNullOrEmpty(v.encargado) || v.encargado == "")
            {
                Mensaje = "Encargado del visitante obligatorio!! ";
                return Mensaje;
            }



            return Mensaje;
        }

        public void cleanNulls(visitante v)
        {
            if (string.IsNullOrEmpty(v.empresa_proveniente) || v.empresa_proveniente == "")
            {
                v.empresa_proveniente = " ";
            }

        }

        public List<visitante> DataSeT_to_List(DataSet ds)
        {

            List<visitante> listrs = new List<visitante>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                listrs.Add(new visitante
                {
                    fecha = dr["fecha"].ToString(),
                    horaentrada = dr["horaentrada"].ToString(),
                    horasalida = dr["horasalida"].ToString(),
                    nombre = dr["nombre"].ToString(),
                    dni = dr["dni"].ToString(),
                    destino = dr["destino"].ToString(),
                    encargado = dr["encargado"].ToString(),

                });

            }
            return listrs;

        }



    }
}