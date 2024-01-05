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
    public class vehiculo_mantenimiento
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Modelo"].ConnectionString);



        public int InsUpd_cargavehicular(cargaVehicular v)
        {
            cleanNulls(v);
            SqlCommand com;
            if (v.idcargavehiculo == 0)
            {
                com = new SqlCommand("sp_ins_cargaVehicular", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@sucursal", varGlobales.g_codsuc);
                com.Parameters.AddWithValue("@fecha", v.fecha);
                com.Parameters.AddWithValue("@horaentrada", v.horaentrada);
                com.Parameters.AddWithValue("@horasalida", "");
                com.Parameters.AddWithValue("@dni", v.dni);
                com.Parameters.AddWithValue("@nombres", v.nombres);
                com.Parameters.AddWithValue("@apPaterno", v.apPaterno);
                com.Parameters.AddWithValue("@apMaterno", v.apMaterno);
            }
            else
            {
                com = new SqlCommand("sp_upd_cargaVehicular", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@idcarga", v.idcargavehiculo);
            }

            com.Parameters.AddWithValue("@licencia", v.licencia);
            com.Parameters.AddWithValue("@empresa_pvnte", v.empresa_proveniente);
            com.Parameters.AddWithValue("@iddestino", v.idempresa_dtno);
            com.Parameters.AddWithValue("@placa", v.placa);
            com.Parameters.AddWithValue("@numContenedor", v.numContenedor);
            com.Parameters.AddWithValue("@bunidad", v.bunidad);
            com.Parameters.AddWithValue("@bcarreta", v.bcarreta);
            com.Parameters.AddWithValue("@codcarreta", v.codcarreta);
            com.Parameters.AddWithValue("@tipo", v.tipoContenedor);
            com.Parameters.AddWithValue("@estado", v.estadContenedor);
            com.Parameters.AddWithValue("@precinto", v.precintoGuia);
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
            Debug.WriteLine("valor retornado:" + resultado);
            return resultado;//si es -1 no realiza la accion, si es 1 si registra
        }
        /******************************************************/


        public String isvalidcarga(cargaVehicular v)
        {
            String Mensaje = "SI";
           
            if (string.IsNullOrEmpty(v.nombres) || v.nombres.Trim() == "")
            {
                Mensaje = "Nombre del conductor obligatorio!! ";
                return Mensaje;
            }

            if (string.IsNullOrEmpty(v.apPaterno) || v.apPaterno.Trim() == "")
            {
                Mensaje = "apellido Paterno obligatorio!! ";
                return Mensaje;
            }

            if (string.IsNullOrEmpty(v.apMaterno) || v.apMaterno.Trim() == "")
            {
                Mensaje = "apellido Materno obligatorio!! ";
                return Mensaje;
            }

            if (string.IsNullOrEmpty(v.dni) || v.dni.Trim() == "")
            {
                Mensaje = "Dni obligatorio!! ";
                return Mensaje;
            }

            if (v.dni.Length < 8)
            {
                Mensaje = "Dni debe contener 8 dígitos como mínimo!! ";
                return Mensaje;
            }

            if (string.IsNullOrEmpty(v.licencia) || v.licencia.Trim() == "")
            {
                Mensaje = "Licencia del conductor obligatoria!! ";
                return Mensaje;
            }

            if (string.IsNullOrEmpty(v.placa) || v.placa.Trim() == "")
            {
                Mensaje = "Placa del vehiculo obligatoria!! ";
                return Mensaje;
            }
            v.nombres = v.nombres.Trim();
            v.apPaterno = v.apPaterno.Trim();
            v.apMaterno = v.apMaterno.Trim();
            v.dni = v.dni.Trim();
            v.licencia = v.licencia.Trim();
            v.placa = v.placa.Trim();
           

            return Mensaje;
        }

        /*****************************************************************/
        public void cleanNulls(cargaVehicular v)
        {
            if (string.IsNullOrEmpty(v.empresa_proveniente))
            {
                v.empresa_proveniente   = "";
            }

      
            if (string.IsNullOrEmpty(v.codcarreta))
            {
                v.codcarreta = "";
            }

            if (string.IsNullOrEmpty(v.numContenedor))
            {
                v.numContenedor = "";
            }

            if (string.IsNullOrEmpty(v.precintoGuia))
            {
                v.precintoGuia = "";
            }
        }




        public int registrarSalida(cargaVehicular v)
        {
            int resultado;
            SqlCommand com = new SqlCommand("sp_upd_cargaVehicularSalida", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@idcarga", v.idcargavehiculo);
            com.Parameters.AddWithValue("@codPais", "PER");
            com.Parameters.AddWithValue("@user", varGlobales.g_nombreusuario);
            con.Open();
            resultado = com.ExecuteNonQuery();
            con.Close();
            Debug.WriteLine("registros ingresados:" + resultado);
            return resultado;//si es -1 no realiza la accion, si es 1 si registra
        }

        /******************************************************/
        public DataTable get_vehiculobyid(int id)
        {
            SqlCommand com = new SqlCommand("sp_get_cargaVehicularbyid", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@id", id);
            com.Parameters.AddWithValue("@codPais","PER");
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        /************************************************************/

        public DataTable  get_CargasVehiculosHoy()
        {
            SqlCommand com = new SqlCommand("sp_get_cargaVehicularHoy", con);
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

        public void inicializadorBuscador(EntidadBusqueda e)
        {
            if (e.nombre == null)
            {
                e.nombre = "";
            }
            if (e.dni == null)
            {
                e.dni = "";
            }
            if (e.proviene == null)
            {
                e.proviene = "";
            }
            if (e.licencia == null)
            {
                e.licencia = "";

            }
            if (e.placa == null)
            {
                e.placa = "";

            }
            if (e.cliente == null)
            {
                e.cliente = "";

            }
        }

       
        public DataTable getCargasFiltradas(EntidadBusqueda e)
        {
            SqlCommand com = new SqlCommand("sp_get_cargaVehicular", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@fechai", e.fechadesde);
            com.Parameters.AddWithValue("@fechaf", e.fechahasta);
            com.Parameters.AddWithValue("@dni", e.dni);
            com.Parameters.AddWithValue("@nombre", e.nombre);
            com.Parameters.AddWithValue("@licencia", e.licencia);
            com.Parameters.AddWithValue("@placa", e.placa);
            com.Parameters.AddWithValue("@proveniente", e.proviene);
            com.Parameters.AddWithValue("@destino", e.destino);
            com.Parameters.AddWithValue("@nSucursal", varGlobales.g_codsuc);

            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable  dt = new DataTable ();
            da.Fill(dt);
            return dt;
        }

        /*******************************************************/




        //public int update_cargaVehicular(cargaVehicular v)
        //{
        //    cleanNulls(v);
        //    int resultado;
        //    SqlCommand com = new SqlCommand("[sp_updateCargaVehicular]", con);
        //    com.CommandType = CommandType.StoredProcedure;
        //    com.Parameters.AddWithValue("@idcargavehicular",v.idcargavehiculo);
        //    com.Parameters.AddWithValue("@empresa_proveniente", v.empresa_proveniente);
        //    com.Parameters.AddWithValue("@idcliente", v.idcliente);
        //    com.Parameters.AddWithValue("@nomConductor", v.nomConductor);
        //    com.Parameters.AddWithValue("@docCondutor", v.docCondutor);
        //    com.Parameters.AddWithValue("@licConductor", v.licConductor);
        //    com.Parameters.AddWithValue("@placaVehiculo", v.placaVehiculo);
        //    com.Parameters.AddWithValue("@bunidad", v.bunidad);
        //    com.Parameters.AddWithValue("@bcarreta", v.bcarreta);
        //    com.Parameters.AddWithValue("@codcarreta", v.codcarreta);
        //    com.Parameters.AddWithValue("@numContenedor", v.numContenedor);
        //    com.Parameters.AddWithValue("@tipoContenedor", v.tipoContenedor);
        //    com.Parameters.AddWithValue("@estadContenedor",v.estadContenedor);
        //    com.Parameters.AddWithValue("@guiainf", v.guiainf);


        //    com.Parameters.AddWithValue("@user", varGlobales.g_nombreusuario);

        //    con.Open();
        //    resultado = com.ExecuteNonQuery();
        //    con.Close();
        //    Debug.WriteLine("actualizado en el sistema:" + resultado);
        //    return resultado;/*si es -1 no realiza la accion, si es 1 si registra*/
        //}


        /********************************************************/
        

       


        //public String isvalidfiltros(EntidadBusqueda entidad)
        //{
        //    String Mensaje = "SI";

        //    if (string.IsNullOrEmpty(entidad.fechadesde) || entidad.fechahasta == "" || string.IsNullOrEmpty(entidad.fechahasta) || entidad.fechahasta == "")
        //    {
        //        Mensaje = "Ingrese el rango de fechas!! ";
        //        return Mensaje;
        //    }
        //    else
        //    {
        //        string f1, f2;
        //        f1 = Reversefecha(entidad.fechadesde);
        //        f2 = Reversefecha(entidad.fechahasta);
        //        Debug.WriteLine("fechas:" + f1 + "--" + f2);
        //        if (string.Compare(f1, f2) == 1)
        //        {
        //            Mensaje = "Fecha inicial no debe ser mayor a la fecha final!!";
        //            return Mensaje;
        //        }


        //    }
        //    return Mensaje;
        //}
        /**************************************************************/
        
        /***********************************************/

        //public string Reversefecha(string s)
        //{
        //    char[] charArr1 = s.ToCharArray();
        //    char[] temp = new Char[10];
        //    for (int i = 0; i < 4; i++)
        //    {
        //        temp[i] = charArr1[6 + i];//hasta i=3
        //    }
        //    temp[4] = '-';

        //    for (int i = 0; i < 2; i++)
        //    {
        //        temp[i + 5] = charArr1[3 + i];//hasta i=6
        //    }

        //    temp[7] = '-';
        //    for (int i = 0; i < 2; i++)
        //    {
        //        temp[i + 8] = charArr1[i];//hasta i=9
        //    }

        //    String fecha = new String(temp);

        //    return fecha;
        //}
       
        



        //public List<cargaVehicular> DataSeT_to_List(DataSet ds)
        //{

        //    List<cargaVehicular> listrs = new List<cargaVehicular>();
        //    foreach (DataRow dr in ds.Tables[0].Rows)
        //    {

        //        listrs.Add(new cargaVehicular
        //        {

        //            idcargavehiculo = Convert.ToInt32(dr["idcargavehiculo"]),
        //            fecha = dr["fecha"].ToString(),
        //            horaentrada = dr["horaentrada"].ToString(),
        //            horasalida = dr["horasalida"].ToString(),
        //            empresa_proveniente = dr["empresa_proveniente"].ToString(),
        //            idcliente = Convert.ToInt32(dr["idcliente"]),
        //            cliente = dr["cliente"].ToString(),
        //            nomConductor = dr["nomConductor"].ToString(),
        //            docCondutor = dr["docCondutor"].ToString(),
        //            licConductor = dr["licConductor"].ToString(),
        //            placaVehiculo = dr["placaVehiculo"].ToString(),
        //            bunidad = Convert.ToInt32(dr["bunidad"]),
        //            bcarreta = Convert.ToInt32(dr["bcarreta"]),
        //            codcarreta = dr["codcarreta"].ToString(),
        //            numContenedor = dr["numContenedor"].ToString(),
        //            tipoContenedor = dr["tipoContenedor"].ToString(),
        //            estadContenedor = dr["estadContenedor"].ToString(),
        //            guiainf = dr["guiainf"].ToString(),
        //            userRegistro = dr["userRegistro"].ToString()
        //        });

        //    }
        //    return listrs;

        //}



    }
}