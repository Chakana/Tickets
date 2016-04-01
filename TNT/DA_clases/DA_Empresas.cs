using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TNT.helpers;

namespace TNT.DA_clases
{
    public class DA_Empresas
    {
        public static TNT.Controllers.Empresas_app obtener_empresa(int id_empresa)
        {
            TNT.Controllers.Empresas_app respuestaEmpresa = new Controllers.Empresas_app();
           
            List<SP_parameters> lista_parametros = new List<SP_parameters>();
            DataTable dt_consulta = null;
            lista_parametros.Add(new SP_parameters("id_empresa", id_empresa.ToString(), System.Data.SqlDbType.Int));
            try
            {
                dt_consulta = SP_Helper.ejecutar_SP_consulta_datatable("app_obtiene_empresa", lista_parametros);

            }
            catch (Exception ex)
            {
                return null;
            }

            //obtener sectores de evento
            foreach (DataRow row in dt_consulta.Rows)
            {
                respuestaEmpresa = new Controllers.Empresas_app();
                respuestaEmpresa.departamento = row.Field<string>("departamento");
                respuestaEmpresa.direccion = row.Field<string>("direccion");
                respuestaEmpresa.dosificacion_actividad_comercial = row.Field<string>("dosificacion_actividad_comercial");
                respuestaEmpresa.dosificacion_codigo_autorizacion = row.Field<string>("dosificacion_codigo_autorizacion");
                respuestaEmpresa.id_empresa = row.Field<int>("id");
                respuestaEmpresa.nit_empresa = row.Field<string>("nit");
                respuestaEmpresa.nombre_empresa = row.Field<string>("nombre_empresa");
                respuestaEmpresa.representante_legal = row.Field<string>("representante_legal");
                respuestaEmpresa.telefono = row.Field<string>("telefono");
            }

            return respuestaEmpresa;
        }

    }
}