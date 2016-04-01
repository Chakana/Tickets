using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TNT.helpers;
namespace TNT.DA_clases
{
    public class DA_Eventos
    {
        public static List<TNT.Controllers.Eventos_app> lista_eventos(String fecha_evento, int id_tipo, int id_lugar)
        {
            DateTime dt_fecha_evento = new DateTime();
            if (fecha_evento != String.Empty)
            {
                dt_fecha_evento = DateTime.ParseExact(fecha_evento, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            }
            
            List<TNT.Controllers.Eventos_app> respuestaEventos = new List<Controllers.Eventos_app>();
            Controllers.Eventos_app evento = new Controllers.Eventos_app();
            List<SP_parameters> lista_parametros = new List<SP_parameters>();
            DataTable dt_consulta=null;
            if (fecha_evento == "" && id_tipo == 0 && id_lugar == 0) //retorna todos los eventos
            {
                try
                {
                    dt_consulta = SP_Helper.ejecutar_SP_consulta_datatable("app_listar_eventos_todos", lista_parametros);
                    
                }
                catch (Exception ex)
                {
                    return null;
                }

            }
            else if (fecha_evento != "" && id_tipo == 0 && id_lugar == 0)
            {
                lista_parametros.Add(new SP_parameters("fecha_evento", dt_fecha_evento.ToString("yyyy-MM-dd"), System.Data.SqlDbType.Date));
                try
                {
                    dt_consulta = SP_Helper.ejecutar_SP_consulta_datatable("app_listar_eventos_fecha_evento", lista_parametros);

                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            else if (fecha_evento == "" && id_tipo > 0 && id_lugar == 0)
            {
                lista_parametros.Add(new SP_parameters("id_tipo", id_tipo.ToString(), System.Data.SqlDbType.Int));
                try
                {
                    dt_consulta = SP_Helper.ejecutar_SP_consulta_datatable("app_listar_eventos_tipo", lista_parametros);

                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            else if (fecha_evento == "" && id_tipo == 0 && id_lugar > 0)
            {
                lista_parametros.Add(new SP_parameters("id_lugar", id_lugar.ToString(), System.Data.SqlDbType.Int));
                try
                {
                    dt_consulta = SP_Helper.ejecutar_SP_consulta_datatable("app_listar_eventos_lugar", lista_parametros);

                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            //obtener sectores de evento
            foreach (DataRow row in dt_consulta.Rows)
            {
                evento = new Controllers.Eventos_app();
                Controllers.Sectores_app sector = new Controllers.Sectores_app();
                List<Controllers.Sectores_app> sectores_evento = new List<Controllers.Sectores_app>();
                DataTable dt_consulta_sectores=null;
                evento.direccion_lugar = row.Field<string>("direccion");
                DateTime fecha = row.Field<DateTime>("fecha_evento");
                TimeSpan hora = row.Field<TimeSpan>("hora_evento");
                evento.fecha_evento = fecha.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                evento.hora_evento = hora.ToString();
                evento.id_evento = row.Field<int>("id");
                evento.nombre_evento = row.Field<string>("nombre_evento");
                evento.nombre_lugar = row.Field<string>("nombre_lugar");
                evento.latitud = row.Field<decimal>("latitud");
                evento.longitud = row.Field<decimal>("longitud");
                evento.img_url = row.Field<string>("img_url");
                evento.tipo_evento = row.Field<string>("Tipo_Evento_Descripcion");
                evento.descripcion = row.Field<string>("descripcion");
                evento.email = row.Field<string>("email");
                evento.telefono = row.Field<string>("telefono");
                evento.empresa_organizadora = row.Field<string>("nombre_empresa");
                List<SP_parameters> lista_parametros_sectores = new List<SP_parameters>();
                lista_parametros_sectores.Add(new SP_parameters("id_evento", evento.id_evento.ToString(), System.Data.SqlDbType.Int));
                try
                {
                    dt_consulta_sectores = SP_Helper.ejecutar_SP_consulta_datatable("app_obtener_sectores_evento", lista_parametros_sectores);
                }
                catch (Exception ex)
                {
                    return null;
                }
                foreach (DataRow row_sectores in dt_consulta_sectores.Rows)
                {
                    sector = new Controllers.Sectores_app();
                    sector.id = row_sectores.Field<int>("id");
                    sector.asientos_disponibles = row_sectores.Field<int>("asientos_disponibles");
                    sector.descripcion = row_sectores.Field<string>("descripcion");
                    sector.precio_unitario = row_sectores.Field<decimal>("precio_unitario");
                    sectores_evento.Add(sector);
                }
                evento.sectores = sectores_evento;
                respuestaEventos.Add(evento);
            }
           
            return respuestaEventos;

        }
        public static TNT.Controllers.Eventos_app obtener_evento(int id_evento)
        {
            
            TNT.Controllers.Eventos_app respuestaEventos = new Controllers.Eventos_app();
            Controllers.Eventos_app evento = new Controllers.Eventos_app();
            List<SP_parameters> lista_parametros = new List<SP_parameters>();
            DataTable dt_consulta = null;
            
            lista_parametros.Add(new SP_parameters("id", id_evento.ToString(), System.Data.SqlDbType.Int));
            try
            {
                dt_consulta = SP_Helper.ejecutar_SP_consulta_datatable("app_obtener_evento", lista_parametros);

            }
            catch (Exception ex)
            {
                return null;
            }
            
            //obtener sectores de evento
            foreach (DataRow row in dt_consulta.Rows)
            {
                evento = new Controllers.Eventos_app();
                Controllers.Sectores_app sector = new Controllers.Sectores_app();
                List<Controllers.Sectores_app> sectores_evento = new List<Controllers.Sectores_app>();
                DataTable dt_consulta_sectores = null;
                evento.direccion_lugar = row.Field<string>("direccion");
                DateTime fecha = row.Field<DateTime>("fecha_evento");
                TimeSpan hora = row.Field<TimeSpan>("hora_evento");
                evento.fecha_evento = fecha.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                evento.hora_evento = hora.ToString();
                evento.id_evento = row.Field<int>("id");
                evento.id_empresa = row.Field<int>("id_empresa");
                evento.nombre_evento = row.Field<string>("nombre_evento");
                evento.nombre_lugar = row.Field<string>("nombre_lugar");
                evento.latitud = row.Field<decimal>("latitud");
                evento.longitud = row.Field<decimal>("longitud");
                evento.img_url = row.Field<string>("img_url");
                List<SP_parameters> lista_parametros_sectores = new List<SP_parameters>();
                lista_parametros_sectores.Add(new SP_parameters("id_evento", evento.id_evento.ToString(), System.Data.SqlDbType.Int));
                try
                {
                    dt_consulta_sectores = SP_Helper.ejecutar_SP_consulta_datatable("app_obtener_sectores_evento", lista_parametros_sectores);
                }
                catch (Exception ex)
                {
                    return null;
                }
                foreach (DataRow row_sectores in dt_consulta_sectores.Rows)
                {
                    sector = new Controllers.Sectores_app();
                    sector.id = row_sectores.Field<int>("id");
                    sector.asientos_disponibles = row_sectores.Field<int>("asientos_disponibles");
                    sector.descripcion = row_sectores.Field<string>("descripcion");
                    sector.precio_unitario = row_sectores.Field<decimal>("precio_unitario");
                    sectores_evento.Add(sector);
                }
                evento.sectores = sectores_evento;
                respuestaEventos=evento;
            }

            return respuestaEventos;
        }
        internal static List<Controllers.Tipo_Evento> lista_tipos_evento()
        {
            List<Controllers.Tipo_Evento> tipos_evento = new List<Controllers.Tipo_Evento>();
            Controllers.Tipo_Evento tipo_evento = new Controllers.Tipo_Evento();
            List<SP_parameters> lista_parametros = new List<SP_parameters>();
            DataTable dt_consulta = null;
            try
            {
                dt_consulta = SP_Helper.ejecutar_SP_consulta_datatable("app_obtiene_tipos_evento", lista_parametros);

            }
            catch (Exception ex)
            {
                return null;
            }
            foreach (DataRow row in dt_consulta.Rows)
            {
                tipo_evento = new Controllers.Tipo_Evento();
                tipo_evento.id = row.Field<int>("id");
                tipo_evento.descripcion = row.Field<string>("descripcion");
                tipos_evento.Add(tipo_evento);
            }
            return tipos_evento;

        }
    }
}