using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TNT.Controllers;
using TNT.helpers;

namespace TNT.DA_clases
{
    public class DA_Puntos_Pago
    {
        public static List<Puntos_Pago_app> Obtiene_Puntos_Pago()
        {
            List<TNT.Controllers.Puntos_Pago_app> respuestaPuntos = new List<Controllers.Puntos_Pago_app>();
            Controllers.Puntos_Pago_app punto = new Puntos_Pago_app(); 
            List<SP_parameters> lista_parametros = new List<SP_parameters>();
            DataTable dt_consulta = null;
            
            try
            {
                dt_consulta = SP_Helper.ejecutar_SP_consulta_datatable("app_obtiene_puntos_pago", lista_parametros);

            }
            catch (Exception ex)
            {
                return null;
            }
            foreach (DataRow row in dt_consulta.Rows)
            {
                punto = new Puntos_Pago_app();
                punto.departamento = row.Field<string>("departamento");
                punto.direccion = row.Field<string>("direccion");
                punto.id = row.Field<int>("id");
                punto.latitud = row.Field<decimal>("latitud");
                punto.longitud = row.Field<decimal>("longitud");
                punto.nombre = row.Field<string>("nombre");
                respuestaPuntos.Add(punto);                
            }
            return respuestaPuntos;
        }
    }
}