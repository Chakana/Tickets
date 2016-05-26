using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TNT.Controllers
{
    
    public class simple_req
    {
        public string echo { get; set; }
    }
    public class respuesta_usuario
    {
        public bool resultado { get; set; }
        public int id_usuario { get; set; }
        public string mensaje { get; set; }

    }
    public class request_registra_usuario
    {
        public string email {get;set;}
        public string password {get;set;}
        public string cedula_identidad {get;set;} 
        public string numero_celular {get;set;}
    }
    public class request_actualizacion_usuario
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string apellidos { get; set; }
        public string cedula_identidad { get; set; }
        public string fecha_nacimiento { get; set; }
        public string numero_celular { get; set; }
        public string direccion { get; set; }
    }
    public class respuesta_actualizacion_usuario
    {
        public bool resultado;
    }
    public class respuesta_sesion
    {
        public bool resultado { get; set; }
        public int id_usuario { get; set; }
        public string mensaje { get; set; }
        public string auth_token { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string nombre { get; set; }
        public string apellidos { get; set; }
        public string cedula_identidad { get; set; }
        public string fecha_nacimiento { get; set; }
        public string numero_celular { get; set; }
        public string direccion { get; set; } 
    }
    public class request_inicia_sesion
    {
        public string email { get; set; }
        public string password { get; set; }
    }
    public class request_compra_ticket
    {
        public int id_usuario { get; set; }
        public int id_evento { get; set; }
        public int id_sector { get; set; }
        public int cantidad { get; set; }
    }
    public class response_compra_ticket
    {
        public string codigo_recaudacion { get; set; }
        public decimal costo_total { get; set; }
    }
    public class request_compra_rapida_ticket
    {
        public string nombre_usuario { get; set; }
        public string email_usuario { get; set; }
        public string nit_usuario { get; set; }
        public int id_evento { get; set; }
        public int id_sector { get; set; }
        public int cantidad { get; set; }

    }
    public class response_compra_rapida_ticket
    {
        public string codigo_recaudacion { get; set; }
        public decimal costo_total { get; set; }
    }
    public class request_verifica_ticket
    {
        public int id_usuario { get; set; }
        public string codigo_qr { get; set; }
        public string codigo_recaudacion { get; set; }
    }
    public class response_verifica_ticket
    {
        public bool ticket_valido { get; set; }
        public string mensaje { get; set; }
    }
    public class request_registra_ingreso
    {
        public int id_usuario { get; set; }
        public string codigo_qr { get; set; }
        public string codigo_recaudacion { get; set; }
    }
    public class response_registra_ingreso
    {
        public bool resultado { get; set; }
        public string mensaje { get; set; }
    }
    public class request_listar_eventos
    {
        public string fecha_evento { get; set; }
        public int id_tipo_evento { get; set; }
        public int id_lugar { get; set; }
    }
    public class request_obtener_evento
    {
        public int id_evento { get; set; }
    }
    public class response_listar_eventos
    {
        public List<Eventos_app> eventos { get; set; }
    }
    public class Eventos_app
    {
        public int id_evento { get; set; }
        public int id_empresa { get; set; }
        public string nombre_evento { get; set; }
        public string nombre_lugar {get;set;}
        public string direccion_lugar {get;set;}
        public string fecha_evento { get; set; }
        public string hora_evento { get; set; }
        public List<Sectores_app> sectores { get; set; }
        public decimal latitud { get; set; }
        public decimal longitud { get; set; }
        public string img_url { get; set; }
        public string tipo_evento { get; set; }
        public string descripcion { get; set; }
        public string email { get; set; }
        public string telefono { get; set; }
        public string empresa_organizadora { get; set; }

    }
    public class Empresas_app
    {
        public int id_empresa { get; set; }
        public string nombre_empresa { get; set; }
        public string nit_empresa { get; set; }
        public string direccion { get; set; }
        public string telefono { get; set; }
        public string representante_legal { get; set; }
        public string dosificacion_codigo_autorizacion { get; set; }
        public string dosificacion_actividad_comercial { get; set; }
        public string departamento { get; set; }
    }
    public class Sectores_app
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public decimal precio_unitario { get; set; }
        public int asientos_disponibles { get; set; }
    }
    public class request_listar_tipos_evento
    {

    }
    public class response_listar_tipos_evento
    {
        public List<Tipo_Evento> tipos_evento { get; set; }
    }
    public class Tipo_Evento
    {
        public int id { get; set; }
        public string descripcion { get; set; }
    }

    public class response_consulta_comision
    {
        public int id_sector { get; set; }
        public decimal monto_comision { get; set; }
    }

    public class request_consulta_comision
    {
        public int id_sector { get; set; }
        public int cantidad_tickets { get; set; }
    }
    public class Puntos_Pago_app
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string direccion { get; set; }
        public string departamento { get; set; }
        public decimal latitud { get; set; }
        public decimal longitud { get; set; }
    }
    public class request_obtiene_puntos_pago
    {

    }
    public class response_obtiene_puntos_pago
    {
        public List<Puntos_Pago_app> puntos_pago { get; set; }
    }
    public class request_obtener_historial_compra
    {
        public int id_usuario { get; set; }
    }
    public class Compra_app_historial
    {
        public int id_compra { get; set; }
        public string nombre_evento { get; set; }
        public string fecha_reserva { get; set; }
        public decimal monto_total { get; set; }
        public string fecha_pago { get; set; }
        public string estado { get; set; }
        public string codigo_recaudacion { get; set; }

    }
    public class response_obtener_historial_compra
    {
        public List<Compra_app_historial> historial_compras { get; set; }
    }
    public class request_recuperar_contrasena
    {
        public string email { get; set; }
    }
    public class response_recuperar_contrasena
    {
        public string message { get; set; }
    }
    public class request_enviar_factura_ticket_correo
    {
        public string codigo_recaudacion { get; set; }
    }
    public class response_enviar_factura_ticket_correo
    {
        public bool resultado { get; set; }
    }
}