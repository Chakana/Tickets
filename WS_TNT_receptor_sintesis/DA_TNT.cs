using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace WS_TNT_receptor_sintesis
{
    public class DA_TNT
    {
        public static bool ws_actualiza_pago(string codigo_recaudacion,string numero_factura,string fecha,string hora)
        {
            bool exito = true;
            List<SP_parameters> lista_parametros = new List<SP_parameters>();
            lista_parametros.Add(new SP_parameters("codigo_recaudacion", codigo_recaudacion, System.Data.SqlDbType.VarChar));
            lista_parametros.Add(new SP_parameters("numero_factura", numero_factura, System.Data.SqlDbType.VarChar));
            lista_parametros.Add(new SP_parameters("fecha", fecha, System.Data.SqlDbType.VarChar));
            lista_parametros.Add(new SP_parameters("hora", hora, System.Data.SqlDbType.VarChar));
            try
            {
                SP_Helper.ejecutar_SP_void("ws_actualizar_pago", lista_parametros);
                StreamWriter sw = new StreamWriter(HttpContext.Current.Server.MapPath("~") + "log_1", true);
                sw.WriteLine(DateTime.Now.ToString() + "request exitoso,COD_REC:"+codigo_recaudacion);
                sw.Flush();
                sw.Close();
            }
            catch (Exception ex)
            {
                StreamWriter sw = new StreamWriter(HttpContext.Current.Server.MapPath("~") + "log_1", true);
                sw.WriteLine(DateTime.Now.ToString() + "request error,COD_REC:" + codigo_recaudacion + " error:"+ex.Message);
                sw.Flush();
                sw.Close();

                exito = false;
            }
            return exito;
        }
    }

    public class SP_parameters
    {
        public string nombre_parametro { get; set; }
        public string valor_parametro { get; set; }
        public SqlDbType tipo_parametro { get; set; }
        public SP_parameters(string nombre, string valor, SqlDbType tipo)
        {
            nombre_parametro = nombre;
            valor_parametro = valor;
            tipo_parametro = tipo;
        }
    }

    public class SP_Helper
    {
        public static string Cadena_Conexion
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["TNT_DB"].ConnectionString;
            }
        }
        public static void ejecutar_SP_void(string nombre_sp, List<SP_parameters> parametros)
        {
            using (SqlConnection con = new SqlConnection(Cadena_Conexion))
            {
                using (SqlCommand cmd = new SqlCommand(nombre_sp, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    foreach (var parametro in parametros)
                    {
                        cmd.Parameters.Add("@" + parametro.nombre_parametro, parametro.tipo_parametro).Value = parametro.valor_parametro;

                    }
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static string ejecutar_SP_scalar(string nombre_sp, List<SP_parameters> parametros)
        {
            string respuesta = String.Empty;
            using (SqlConnection con = new SqlConnection(Cadena_Conexion))
            {
                using (SqlCommand cmd = new SqlCommand(nombre_sp, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    foreach (var parametro in parametros)
                    {
                        cmd.Parameters.Add("@" + parametro.nombre_parametro, parametro.tipo_parametro).Value = parametro.valor_parametro;

                    }
                    con.Open();
                    respuesta = (string)cmd.ExecuteScalar();
                }
            }
            return respuesta;
        }
        public static void ejecutar_SP_parametros_salida(string nombre_sp, List<SP_parameters> parametros, ref List<SP_parameters> parametros_salida)
        {
            using (SqlConnection con = new SqlConnection(Cadena_Conexion))
            {
                using (SqlCommand cmd = new SqlCommand(nombre_sp, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    foreach (var parametro in parametros)
                    {
                        cmd.Parameters.Add("@" + parametro.nombre_parametro, parametro.tipo_parametro).Value = parametro.valor_parametro;

                    }
                    foreach (var parametro in parametros_salida)
                    {
                        SqlParameter param = new SqlParameter();
                        param.ParameterName = "@" + parametro.nombre_parametro;
                        param.SqlDbType = parametro.tipo_parametro;
                        param.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(param);
                    }
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static DataTable ejecutar_SP_consulta_datatable(string nombre_sp, List<SP_parameters> parametros)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(Cadena_Conexion))
            {
                using (SqlCommand cmd = new SqlCommand(nombre_sp, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    foreach (var parametro in parametros)
                    {
                        cmd.Parameters.Add("@" + parametro.nombre_parametro, parametro.tipo_parametro).Value = parametro.valor_parametro;

                    }
                    con.Open();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }

                }
            }
            return dt;
        }



    }

}