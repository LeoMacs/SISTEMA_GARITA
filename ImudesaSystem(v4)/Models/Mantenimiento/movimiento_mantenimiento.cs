using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Configuration;

namespace ImudesaSystem_v4_.Models.Clases
{
    public class movimiento_mantenimiento
    {

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Modelo"].ConnectionString);


        public DataTable get_movimientosHoy(String modoMov)
        {
            SqlCommand com = new SqlCommand("sp_get_movimientosHoy", con);
            com.Parameters.AddWithValue("@modoMov", modoMov);
            com.Parameters.AddWithValue("@idusuario", varGlobales.g_idusuario);
            com.Parameters.AddWithValue("@nSucursal", varGlobales.g_codsuc);
            com.Parameters.AddWithValue("@codPais", "PER");
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

       

        // Get movimiento by id
        public DataTable get_movimientobyid(int id)
        {
            SqlCommand com = new SqlCommand("sp_get_movimientobyid", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@idMovimiento", id);
            com.Parameters.AddWithValue("@codPais", "PER");
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }


        public int insupd_movimiento(movimientoPersona  m)
        {
            m.cleanNulls();
            SqlCommand com;
            if (m.idMovimiento == 0)
            {
                com = new SqlCommand("sp_ins_movimiento", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@sucursal", varGlobales.g_codsuc);
                com.Parameters.AddWithValue("@fecha", m.fecha);
                com.Parameters.AddWithValue("@horaentrada", m.horaentrada);
                com.Parameters.AddWithValue("@horaSalida", "");
                com.Parameters.AddWithValue("@dni", m.dni);
                com.Parameters.AddWithValue("@nombres", m.nombres);
                com.Parameters.AddWithValue("@apPaterno", m.apPaterno);
                com.Parameters.AddWithValue("@apMaterno", m.apMaterno);
                com.Parameters.AddWithValue("@modo", m.modo);
            }
            else
            {
                com = new SqlCommand("sp_upd_movimiento", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@idmovimiento", m.idMovimiento);
            }

            com.Parameters.AddWithValue("@numAsignado", m.nroasignado);
            com.Parameters.AddWithValue("@destino", m.destino);
            com.Parameters.AddWithValue("@encargado", m.encargado);
            com.Parameters.AddWithValue("@empresapvnte", m.empresa_proveniente);
            com.Parameters.AddWithValue("@observacion", m.observacion);
            com.Parameters.AddWithValue("@codPais", "PER");
            com.Parameters.AddWithValue("@user", varGlobales.g_nombreusuario);
            //////////////////////////
            SqlParameter varOutput = new SqlParameter("@nResultado", SqlDbType.Int);
            varOutput.Direction = ParameterDirection.Output;
            com.Parameters.Add(varOutput);
            //////////////////
            con.Open();
            com.ExecuteNonQuery();
            int resultado =(int)com.Parameters["@nResultado"].Value;
            con.Close();
            Debug.WriteLine("valor retornado:" + resultado);
            return resultado;//si es -1 no realiza la accion, si es 1 si registra
        }


        // Delete record--sp_deletevisita
        public int del_movimiento(movimientoPersona m)
        {
            SqlCommand com = new SqlCommand("sp_del_movimiento", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@idmovimiento", m.idMovimiento);
            com.Parameters.AddWithValue("@codPais", "PER");
            com.Parameters.AddWithValue("@user", varGlobales.g_nombreusuario);
            //////////////////////////
            SqlParameter varOutput = new SqlParameter("@nResultado", SqlDbType.Int);
            varOutput.Direction = ParameterDirection.Output;
            com.Parameters.Add(varOutput);
            //////////////////
            con.Open();
            com.ExecuteNonQuery();
            int resultado = (int)com.Parameters["@nResultado"].Value;
            con.Close();
            Debug.WriteLine("registros ingresados:" + resultado);
            return resultado;//si es -1 no realiza la accion, si es 1 si registra
        }



        public int registrarSalida(movimientoPersona  m)
        {
       
            SqlCommand com = new SqlCommand("sp_upd_movimientoSalida", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@idmovimiento", m.idMovimiento);
            com.Parameters.AddWithValue("@codPais", "PER");
            com.Parameters.AddWithValue("@user", varGlobales.g_nombreusuario);
          
            con.Open();
            int resultado = com.ExecuteNonQuery();
            con.Close();
            Debug.WriteLine("registros ingresados:" + resultado);
            return resultado;//si es -1 no realiza la accion, si es 1 si registra
        }


       



        public DataTable getVisitantesFiltradas(EntidadBusqueda e)
        {
            SqlCommand com = new SqlCommand("sp_get_movimientos", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@fechai", e.fechadesde);
            com.Parameters.AddWithValue("@fechaf", e.fechahasta);
            com.Parameters.AddWithValue("@nombre", e.nombre);
            com.Parameters.AddWithValue("@dni", e.dni);
            com.Parameters.AddWithValue("@destino", e.destino);
            com.Parameters.AddWithValue("@encargado", e.encargado);
            com.Parameters.AddWithValue("@empresa_proveniente", e.proveedor);
            com.Parameters.AddWithValue("@modoMov", e.modo);
            com.Parameters.AddWithValue("@nSucursal", varGlobales.g_codsuc);
            //Debug.WriteLine("Datos:" + e.fechadesde + "/" + e.fechahasta);
            //Console.WriteLine("Datos:" + e.fechadesde + "/" + e.fechahasta);

            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }


        // get_persona
        public DataTable  get_persona(string dni, int id, int modo)
        {
            SqlCommand com = new SqlCommand("sp_get_persona", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@dni", dni);
            com.Parameters.AddWithValue("@idPersona", id);
            com.Parameters.AddWithValue("@modo", modo);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        //public List<visitante> getVisitantesFiltradasForExcel(String fechai, String fechaf, String nombre, String dni, String destino, String empresa_proveniente)
        //{
        //    List<visitante> visitantes = new List<visitante>();
        //    con.Open();
        //    SqlCommand com = new SqlCommand("sp_getVisitasxFiltrosGarita", con);
        //    com.CommandType = CommandType.StoredProcedure;
        //    com.Parameters.AddWithValue("@fechai", fechai);
        //    com.Parameters.AddWithValue("@fechaf", fechaf);
        //    com.Parameters.AddWithValue("@nombre", nombre);
        //    com.Parameters.AddWithValue("@dni", dni);
        //    com.Parameters.AddWithValue("@destino", destino);
        //    com.Parameters.AddWithValue("@empresa_proveniente", empresa_proveniente);

        //    SqlDataReader registros = com.ExecuteReader();
        //    while (registros.Read())
        //    {
        //        visitante visitante = new visitante
        //        {

        //            fecha = registros["fecha"].ToString(),
        //            horaentrada = registros["horaentrada"].ToString(),
        //            horasalida = registros["horasalida"].ToString(),
        //            nombre = registros["nombre"].ToString(),
        //            dni = registros["dni"].ToString(),
        //            destino = registros["destino"].ToString(),
        //            encargado = registros["encargado"].ToString(),
        //            empresa_proveniente = registros["empresa_proveniente"].ToString()
        //        };
        //        visitantes.Add(visitante);
        //    }

        //    return visitantes;
        //}


       

        


        //public String HabilitarMarcarSalida(String salida)
        //{
        //    String cadena = "";
        //    if (salida != "")
        //    {
        //        cadena = "true";
        //    }
        //    return cadena;
        //}

        //public void inicializadorBuscador(EntidadBusqueda e)
        //{
        //    if (e.nombre == null)
        //    {
        //        e.nombre = "";
        //    }
        //    if (e.dni == null)
        //    {
        //        e.dni = "";
        //    }
        //    if (e.proveedor == null)
        //    {
        //        e.proveedor = "";
        //    }
        //    if (e.destino == null)
        //    {
        //        e.destino = "";

        //    }
        //}

        //public List<visitante> DataSetToList(DataSet ds)
        //{
        //    List<visitante> visitantes = new List<visitante>();

        //    if (ds != null)
        //    {
        //        visitantes = ds.Tables[0].AsEnumerable().Select(dataRow => new visitante
        //        {
        //            fecha = dataRow.Field<string>("fecha"),
        //            horaentrada = dataRow.Field<string>("horaentrada"),
        //            horasalida = dataRow.Field<String>("horasalida"),
        //            nombre = dataRow.Field<string>("nombre"),
        //            dni = dataRow.Field<String>("dni"),
        //            destino = dataRow.Field<String>("destino"),
        //            encargado = dataRow.Field<string>("encargado"),
        //            empresa_proveniente = dataRow.Field<String>("empresa_proveniente")

        //        }).ToList();
        //    }


        //    return visitantes;
        //}

        ///*
       
        //public Visitante getVisitante(int idvisitante)
        //{
        //    Conectar();
        //    SqlCommand comando = new SqlCommand("select idvisitante,FORMAT(fecha,'yyyy-MM-dd') as fecha,horaentrada,horasalida,nombre,dni,destino,encargado,empresa_proveniente from visitante where idvisitante=@idvisitante", con);
        //    comando.Parameters.Add("@idvisitante", SqlDbType.Int);
        //    comando.Parameters["@idvisitante"].Value = idvisitante;
        //    con.Open();
        //    SqlDataReader registros = comando.ExecuteReader();
        //    Visitante visitante = new Visitante();
        //    if (registros.Read())
        //    {
        //        visitante.idvisitante = int.Parse(registros["idvisitante"].ToString());
        //        visitante.fecha = registros["fecha"].ToString();
        //        visitante.horaentrada = registros["horaentrada"].ToString();
        //        visitante.horasalida = registros["horasalida"].ToString();
        //        visitante.nombre = registros["nombre"].ToString();
        //        visitante.dni = registros["dni"].ToString();
        //        visitante.destino = registros["destino"].ToString();
        //        visitante.encargado = registros["encargado"].ToString();
        //        visitante.empresa_proveniente = registros["empresa_proveniente"].ToString();
        //    }
        //    con.Close();
        //    return visitante;
        //}



        //*/



      

        //public List<visitante> DataSeT_to_List(DataSet ds)
        //{

        //    List<visitante> listrs = new List<visitante>();
        //    foreach (DataRow dr in ds.Tables[0].Rows)
        //    {

        //        listrs.Add(new visitante
        //        {
        //            fecha = dr["fecha"].ToString(),
        //            horaentrada = dr["horaentrada"].ToString(),
        //            horasalida = dr["horasalida"].ToString(),
        //            nombre = dr["nombre"].ToString(),
        //            dni = dr["dni"].ToString(),
        //            destino = dr["destino"].ToString(),
        //            encargado = dr["encargado"].ToString(),

        //        });

        //    }
        //    return listrs;

        //}


    }
}