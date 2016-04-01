using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;


namespace TNT.helpers
{
    public class SP_parameters
    {
        public string nombre_parametro {get;set;}
        public string valor_parametro {get;set;}
        public SqlDbType tipo_parametro {get;set;}
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
        public static void ejecutar_SP_void(string nombre_sp,List<SP_parameters> parametros )
        {
            using (SqlConnection con = new SqlConnection(Cadena_Conexion))
            {
                using (SqlCommand cmd = new SqlCommand(nombre_sp, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    foreach (var parametro in parametros)
	                {
                        cmd.Parameters.Add("@"+parametro.nombre_parametro, parametro.tipo_parametro).Value = parametro.valor_parametro;
		 
	                }                        
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static string ejecutar_SP_scalar(string nombre_sp, List<SP_parameters> parametros)
        {
            string respuesta=String.Empty;
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
                    respuesta =(string) cmd.ExecuteScalar();                    
                }
            }
            return respuesta;
        }
        public static void ejecutar_SP_parametros_salida(string nombre_sp, List<SP_parameters> parametros, ref SP_parameters parametros_salida)
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
                    
                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@" + parametros_salida.nombre_parametro;
                    param.SqlDbType = parametros_salida.tipo_parametro;
                    param.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(param);
                    
                    con.Open();
                    cmd.ExecuteNonQuery();
                    parametros_salida.valor_parametro = param.Value.ToString();
                    
                }
            }
        }
        public static DataTable ejecutar_SP_consulta_datatable(string nombre_sp, List<SP_parameters> parametros)
        {
            DataTable dt=new DataTable();
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