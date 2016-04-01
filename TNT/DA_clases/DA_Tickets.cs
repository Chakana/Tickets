using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TNT.helpers;


namespace TNT.DA_clases
{
    public class DA_Tickets
    {
        public static decimal obtiene_costo_sector(int id_evento,int id_sector,int cantidad){
            decimal costo = 0M;
            List<SP_parameters> lista_parametros = new List<SP_parameters>();
            lista_parametros.Add(new SP_parameters("id_evento", id_evento.ToString(), System.Data.SqlDbType.Int));
            lista_parametros.Add(new SP_parameters("id_sector", id_sector.ToString(), System.Data.SqlDbType.Int));
            lista_parametros.Add(new SP_parameters("cantidad", cantidad.ToString(), System.Data.SqlDbType.Int));
            SP_parameters lista_parametros_salida = new SP_parameters("costo", "0", System.Data.SqlDbType.Decimal);
            
            try
            {
                SP_Helper.ejecutar_SP_parametros_salida("app_obtiene_costo_sector", lista_parametros, ref lista_parametros_salida);
                //costo = Decimal.Parse(lista_parametros_salida.FirstOrDefault(param => param.nombre_parametro == "costo").valor_parametro);
                costo = Decimal.Parse(lista_parametros_salida.valor_parametro);

            }
            catch (Exception ex)
            {
                
            }
            return costo;
        }
        public static bool registra_compra(int id_usuario,string codigo_recaudacion,decimal monto_cobrar)
        {
            bool exito = true;
            List<SP_parameters> lista_parametros = new List<SP_parameters>();
            lista_parametros.Add(new SP_parameters("id_usuario", id_usuario.ToString(), System.Data.SqlDbType.Int));
            lista_parametros.Add(new SP_parameters("codigo_recaudacion", codigo_recaudacion.ToString(), System.Data.SqlDbType.VarChar));
            lista_parametros.Add(new SP_parameters("monto_cobrar", monto_cobrar.ToString(), System.Data.SqlDbType.Decimal));
            try
            {
                SP_Helper.ejecutar_SP_void("app_registra_compra", lista_parametros);
            }
            catch (Exception ex)
            {
                exito = false;
            }
            return exito;
        }
        public static bool registra_ticket(string codigo_ticket, string butaca, int id_evento, int id_sector, string codigo_recaudacion,string nombre_usuario,string nit_usuario)
        {
            bool exito = true;
            List<SP_parameters> lista_parametros = new List<SP_parameters>();
            lista_parametros.Add(new SP_parameters("codigo_ticket", codigo_ticket.ToString(), System.Data.SqlDbType.VarChar));
            lista_parametros.Add(new SP_parameters("butaca", butaca.ToString(), System.Data.SqlDbType.VarChar));
            lista_parametros.Add(new SP_parameters("id_evento", id_evento.ToString(), System.Data.SqlDbType.Int));
            lista_parametros.Add(new SP_parameters("id_sector", butaca.ToString(), System.Data.SqlDbType.Int));
            lista_parametros.Add(new SP_parameters("codigo_recaudacion", id_evento.ToString(), System.Data.SqlDbType.VarChar));
            lista_parametros.Add(new SP_parameters("nombre_usuario", nombre_usuario, System.Data.SqlDbType.VarChar));
            lista_parametros.Add(new SP_parameters("nit_usuario", nit_usuario, System.Data.SqlDbType.VarChar));
            try
            {
                SP_Helper.ejecutar_SP_void("app_registra_ticket", lista_parametros);
            }
            catch (Exception ex)
            {
                exito = false;
            }
            return exito;
        }
        public static void verifica_ticket(string codigo_ticket,string codigo_recaudacion,ref bool valido){
            List<SP_parameters> lista_parametros = new List<SP_parameters>();
            lista_parametros.Add(new SP_parameters("codigo_ticket", codigo_ticket.ToString(), System.Data.SqlDbType.VarChar));
            lista_parametros.Add(new SP_parameters("codigo_recaudacion", codigo_recaudacion.ToString(), System.Data.SqlDbType.VarChar));
            SP_parameters lista_parametros_salida = new SP_parameters("valido", "0", System.Data.SqlDbType.Bit);            
            try
            {
                SP_Helper.ejecutar_SP_parametros_salida("app_verifica_ticket", lista_parametros, ref lista_parametros_salida);
                //valido = Boolean.Parse(lista_parametros_salida.FirstOrDefault(param => param.nombre_parametro == "valido").valor_parametro);
                valido = Boolean.Parse(lista_parametros_salida.valor_parametro);
            }
            catch (Exception ex)
            {
                valido = false;
            }
        }
        public static void registra_ingreso(string codigo_ticket, string codigo_recaudacion, ref bool valido)
        {
            valido = true;
            List<SP_parameters> lista_parametros = new List<SP_parameters>();
            lista_parametros.Add(new SP_parameters("codigo_ticket", codigo_ticket.ToString(), System.Data.SqlDbType.VarChar));
            lista_parametros.Add(new SP_parameters("codigo_recaudacion", codigo_recaudacion.ToString(), System.Data.SqlDbType.VarChar));           
            try
            {
                SP_Helper.ejecutar_SP_void("app_registra_ingreso", lista_parametros);                
            }
            catch (Exception ex)
            {
                valido = false;
            }
        }
        public static decimal consulta_comision(int id_sector)
        {
            decimal monto_comision = 0m;
            DataTable dt_consulta = null;
            List<SP_parameters> lista_parametros = new List<SP_parameters>();
            lista_parametros.Add(new SP_parameters("id_sector", id_sector.ToString(), System.Data.SqlDbType.Int));
            try
            {
                dt_consulta=SP_Helper.ejecutar_SP_consulta_datatable("app_obtener_comisiones", lista_parametros);

            }
            catch (Exception ex)
            {
                
            }
            foreach (DataRow row in dt_consulta.Rows)
            {
                monto_comision = row.Field<decimal>("monto_comision");
            }
                 
            return monto_comision;


        }
            
    }
}