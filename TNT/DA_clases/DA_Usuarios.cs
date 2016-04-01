using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TNT.helpers;

namespace TNT.DA_clases
{
    public class DA_Usuarios
    {
        public static bool registrar_usuario(string email,string password,string cedula_identidad,string numero_celular,ref int id_usuario)
        {
            bool respuesta = false;
            List<SP_parameters> lista_parametros = new List<SP_parameters>();
            lista_parametros.Add(new SP_parameters("email",email,System.Data.SqlDbType.VarChar));
            lista_parametros.Add(new SP_parameters("password",password,System.Data.SqlDbType.VarChar));
            lista_parametros.Add(new SP_parameters("cedula_identidad",cedula_identidad,System.Data.SqlDbType.VarChar));
            lista_parametros.Add(new SP_parameters("numero_celular",numero_celular,System.Data.SqlDbType.VarChar));
            try
            {
                respuesta = true;
                id_usuario=Int32.Parse(SP_Helper.ejecutar_SP_scalar("app_registrar_usuario", lista_parametros));
            }
            catch (Exception )
            {
                respuesta = false;                
            }
            
            return respuesta;

        }
        public static bool actualizar_usuario(int id_usuario, string nombre, string apellidos, string cedula_identidad, DateTime fecha_nacimiento, string numero_celular, string direccion)
        {
            bool respuesta = false;
            List<SP_parameters> lista_parametros = new List<SP_parameters>();
            lista_parametros.Add(new SP_parameters("id", id_usuario.ToString(), System.Data.SqlDbType.Int));
            lista_parametros.Add(new SP_parameters("nombre", nombre, System.Data.SqlDbType.VarChar));
            lista_parametros.Add(new SP_parameters("apellidos", apellidos, System.Data.SqlDbType.VarChar));
            lista_parametros.Add(new SP_parameters("cedula_identidad", cedula_identidad, System.Data.SqlDbType.VarChar));
            lista_parametros.Add(new SP_parameters("fecha_nacimiento", fecha_nacimiento.ToString("yyyy-MM-dd"), System.Data.SqlDbType.Date));
            lista_parametros.Add(new SP_parameters("numero_celular", numero_celular, System.Data.SqlDbType.VarChar));
            lista_parametros.Add(new SP_parameters("direccion", direccion, System.Data.SqlDbType.VarChar));
            try
            {
                respuesta = true;
                SP_Helper.ejecutar_SP_void("app_actualizar_usuario", lista_parametros);
            }
            catch (Exception)
            {
                respuesta = false;
            }

            return respuesta;
        }
        public static int iniciar_sesion(string email, string password,string token)
        {
            int id_usuario = 0;
            List<SP_parameters> lista_parametros = new List<SP_parameters>();
            lista_parametros.Add(new SP_parameters("email", email, System.Data.SqlDbType.VarChar));
            lista_parametros.Add(new SP_parameters("password", password, System.Data.SqlDbType.VarChar));
            lista_parametros.Add(new SP_parameters("token", token, System.Data.SqlDbType.VarChar));
            try
            {
                id_usuario = Int32.Parse(SP_Helper.ejecutar_SP_scalar("app_login", lista_parametros));
            }
            catch (Exception )
            {
                id_usuario = 0;
            }
            return id_usuario;
        }
    }
}