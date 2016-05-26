using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TNT.helpers;
using System.Web.Http.Services;
using TNT.Models;
using System.Text;
using Microsoft.Reporting.WebForms;
using System.Web;


namespace TNT.Controllers
{

    public class RegistraUsuarioController : ApiController
    {
       
       public respuesta_usuario Post(request_registra_usuario request)
        {
            respuesta_usuario respuesta = new respuesta_usuario();
            int id_usuario = 0;
            respuesta.resultado = DA_clases.DA_Usuarios.registrar_usuario(request.email, request.password, request.cedula_identidad, request.numero_celular, ref id_usuario);
            if (respuesta.resultado)
            {
                respuesta.mensaje = "OK";
            }
            else
            {
                respuesta.mensaje = "ERROR";
            }
            respuesta.id_usuario = id_usuario;
            return respuesta;
        }

    }
    public class ActualizaUsuarioController : ApiController
    {
        public respuesta_actualizacion_usuario Post(request_actualizacion_usuario request)
        {
            respuesta_actualizacion_usuario respuesta = new respuesta_actualizacion_usuario();
            DateTime dt_fecha_nacimiento = DateTime.ParseExact(request.fecha_nacimiento, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            respuesta.resultado = DA_clases.DA_Usuarios.actualizar_usuario(request.id, request.nombre, request.apellidos, request.cedula_identidad, dt_fecha_nacimiento, request.numero_celular, request.direccion);
            return respuesta;
        }
    }
    public class IniciaSesionController : ApiController
    {
        private TNTEntities db = new TNTEntities();
        public respuesta_sesion Post(request_inicia_sesion request)
        {
            respuesta_sesion respuesta = new respuesta_sesion();
            //app_tnt:auth_token
            byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            byte[] key = Guid.NewGuid().ToByteArray();
            string token = CryptoClass.Encrypt(Convert.ToBase64String(time.Concat(key).ToArray()), System.Configuration.ConfigurationManager.AppSettings["passphrase"]); 
            respuesta.id_usuario = DA_clases.DA_Usuarios.iniciar_sesion(request.email, request.password,token);
            if (respuesta.id_usuario > 0)
            {
                var persona = db.Personas.FirstOrDefault(per => per.id_usuario == respuesta.id_usuario);
                respuesta.id_usuario = persona.id_usuario.Value;
                respuesta.apellidos = persona.apellidos;
                respuesta.cedula_identidad = persona.cedula_identidad.Trim();
                respuesta.direccion = persona.direccion;
                respuesta.email = request.email;
                respuesta.fecha_nacimiento = persona.fecha_nacimiento.Value.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                respuesta.nombre = persona.nombre;
                respuesta.numero_celular = persona.numero_celular;
                respuesta.resultado = true;
                respuesta.mensaje = "OK";
                respuesta.auth_token = token;
                
            }
            else
            {
                respuesta.id_usuario = 0;
                respuesta.resultado = false;
                respuesta.mensaje = "ERROR";
                respuesta.auth_token = "";
            }
            
            return respuesta;
        }
    }
    public class CompraTicketController : ApiController
    {
        public response_compra_ticket Post(request_compra_ticket request)
        {
            response_compra_ticket respuesta = new response_compra_ticket();
            decimal costo_total=DA_clases.DA_Tickets.obtiene_costo_sector(request.id_evento, request.id_sector, request.cantidad);
            string inicial = "L";
            string codigo_recaudacion = Helpers.Generar_codigo_recaudacion(inicial); //no podemos repetir el codigo de recaudacion (no debe existir en la tabla compra)
            //registrar en la tabla compra
            //loop de registro de cada ticket (cantidad)
            for (int i = 1; i <= request.cantidad; i++)
            {
                //registra ticket en la tabla tickets
                //butaca revisamos si lo tenemos en request como array, sino lo dejamos en blanco para que tome un valor generico(dependiendo del evento)ç
                string butaca="";
                //generamos un numero de ticket nuevo por cada registro
                string codigo_ticket=Helpers.Generar_codigo_ticket(50,new Random());
                DA_clases.DA_Tickets.registra_ticket(codigo_ticket, butaca, request.id_evento, request.id_sector, codigo_recaudacion,"S/N","0");
            }
            //aca debemos llamar a pagos net y ver si acepta e
            
            respuesta.codigo_recaudacion = codigo_recaudacion;
            respuesta.costo_total = costo_total;            
            return respuesta;
        }

    }
    public class CompraRapidaTicketController : ApiController
    {
        private TNTEntities db = new TNTEntities();
        public response_compra_rapida_ticket Post(List<request_compra_rapida_ticket> list_request)
        {
            response_compra_rapida_ticket respuesta = new response_compra_rapida_ticket();
            try
            {
                string inicial = list_request[0].nombre_usuario.Substring(1, 1);
                string codigo_recaudacion = Helpers.Generar_codigo_recaudacion(inicial); //no podemos repetir el codigo de recaudacion (no debe existir en la tabla compra)
                decimal costo_total = 0m;
                decimal total_comision = 0m;
                Eventos_app datos_evento = DA_clases.DA_Eventos.obtener_evento(list_request[0].id_evento);
                Empresas_app datos_empresa = DA_clases.DA_Empresas.obtener_empresa(datos_evento.id_empresa);
                /*if (sector.asientos_disponibles - request.cantidad <= 0)
                   {
                       respuesta.codigo_recaudacion = "";
                       respuesta.costo_total = 0;
                       list_respuesta.Add(respuesta);
                       continue;
                   }*/
                foreach (request_compra_rapida_ticket request in list_request)
                {
                    sectores sector = db.sectores.Find(request.id_sector);
                    decimal costo_total_sector = DA_clases.DA_Tickets.obtiene_costo_sector(request.id_evento, request.id_sector, request.cantidad);
                    var comisiones = db.comisiones.Where(com => com.id_empresa == datos_empresa.id_empresa);
                    if (comisiones.Count() == 0)
                    {
                        comisiones = db.comisiones.Where(com => com.id_empresa == 1);
                    }

                    var comision = comisiones.FirstOrDefault(com => com.rango_inferior >= costo_total_sector);
                    if (comision == null)//si no se encuentra comision inferior quiere decir que supera el limite inferior asi q tomamos el maximo
                    {
                        comision = comisiones.OrderByDescending(com => com.rango_superior).First();

                    }
                    decimal monto_comision = comision.monto_comision * request.cantidad; //Implementar   
                    total_comision += monto_comision;

                    string butaca = "";
                    //generamos un numero de ticket nuevo por cada registro

                    for (int i = 1; i <= request.cantidad; i++)
                    {
                        string codigo_ticket = Helpers.Generar_codigo_ticket(30, new Random());
                        Ticket ticket_nuevo = new Ticket();
                        ticket_nuevo.butaca = butaca;
                        ticket_nuevo.codigo = codigo_ticket;
                        ticket_nuevo.codigo_recaudacion = codigo_recaudacion;
                        ticket_nuevo.id_evento = request.id_evento;
                        ticket_nuevo.id_sector = request.id_sector;
                        ticket_nuevo.nit_usuario = request.nit_usuario;
                        ticket_nuevo.nombre_usuario = request.nombre_usuario;
                        ticket_nuevo.utilizada = false;
                        ticket_nuevo.valida = 0;
                        ticket_nuevo.costo_sin_comision = costo_total_sector / request.cantidad;
                        ticket_nuevo.fecha_modificacion = DateTime.Now;
                        db.Ticket.Add(ticket_nuevo);
                        db.SaveChanges();
                    }
                    costo_total += costo_total_sector;
                    sector.asientos_disponibles = sector.asientos_disponibles - request.cantidad;
                    db.Entry(sector).State = System.Data.EntityState.Modified;
                    db.SaveChanges();
                    //DA_clases.DA_Tickets.registra_ticket(codigo_ticket, butaca, request.id_evento, request.id_sector, codigo_recaudacion, request.nombre_usuario, request.nit_usuario);
                    //DA_clases.DA_Tickets.registra_compra(usuario.id, codigo_recaudacion, costo_total + monto_comision);

                }
                bool envio = Helpers.Envio_sintesis_compra_simple(
                       codigo_recaudacion, list_request[0].email_usuario, list_request[0].nombre_usuario, list_request[0].nit_usuario, datos_evento.nombre_evento,
                       datos_empresa.nombre_empresa, datos_empresa.nit_empresa, (double)costo_total, (double)total_comision);
                if (!envio)
                {
                    //guardar en SINTESIS relegados
                    Models.EnviosRelegadosSINTESIS relegado = new EnviosRelegadosSINTESIS();
                    relegado.codigo_recaudacion = codigo_recaudacion;
                    relegado.costo_total = costo_total;
                    relegado.email_usuario = list_request[0].email_usuario;
                    relegado.fecha_hora = DateTime.Now;
                    relegado.nit_empresa = datos_empresa.nit_empresa;
                    relegado.nit_usuario = list_request[0].nit_usuario;
                    relegado.nombre_empresa = datos_empresa.nombre_empresa;
                    relegado.nombre_evento = datos_evento.nombre_evento;
                    relegado.nombre_usuario = list_request[0].nombre_usuario;
                    relegado.pendiente = true;
                    relegado.total_comision = total_comision;
                    db.EnviosRelegadosSINTESIS.Add(relegado);
                    db.SaveChanges();
                    helpers.Helpers.EnviarMail("admin@tnt.com", "admin", "eisenob@gmail.com", "admin", "CODIGO RECAUDACION NO ENVIADO SINTESIS - PENDIENTE", "<h1>Codigo de recaudacion no enviado y pendiente de envio</h1><h2>" + codigo_recaudacion + "</h2>", null);

                }
                string email_usuario = list_request[0].email_usuario;
                Usuarios usuario = db.Usuarios.FirstOrDefault(us => us.email == email_usuario);
                Compra compra_nueva = new Compra();
                compra_nueva.codigo_recaudacion = codigo_recaudacion;
                compra_nueva.fecha_compra = DateTime.Now;
                compra_nueva.id_usuario_compra = usuario.id;
                compra_nueva.monto_cobrar = costo_total + total_comision;
                compra_nueva.monto_comision = total_comision;
                compra_nueva.monto_parcial = costo_total;
                compra_nueva.pagado = 0;
                db.Compra.Add(compra_nueva);
                db.SaveChanges();
                respuesta.codigo_recaudacion = codigo_recaudacion;
                respuesta.costo_total = costo_total + total_comision;
                //enviar correo a usuario con codigo de recaudacion
                string template = "<h2>Se efectuo una compra del evento:" + datos_evento.nombre_evento + "</h2>Este es el codigo de recaudacion:" + codigo_recaudacion + " con esto puede pasar a cancelar el monto de:" + respuesta.costo_total + " a el punto de pago mas cercano";
                Helpers.EnviarMail("admin@tnt.com", "admin", usuario.email, usuario.Personas.First().nombre, "codigo de recaudacion", template, null);

            }
            catch (Exception ex)
            {
                
                throw;
                
            }


                return respuesta;
        }
    }
    public class VerificaTicketController : ApiController
    {
        public response_verifica_ticket Post(request_verifica_ticket request)
        {
            response_verifica_ticket respuesta = new response_verifica_ticket();
            //codigo QR desencriptado seria el numero de ticket
            bool ticket_valido = false;
            respuesta.mensaje = "NO VERIFICADO";
            DA_clases.DA_Tickets.verifica_ticket(request.codigo_qr, request.codigo_recaudacion, ref ticket_valido);
            if (ticket_valido)
            {
                respuesta.mensaje = "OK";
            }
            return respuesta;
        }
    }
    public class RegistraIngresoController : ApiController
    {
        public response_registra_ingreso Post(request_verifica_ticket request)
        {
            response_registra_ingreso respuesta = new response_registra_ingreso();
            respuesta.mensaje = "ERROR";
            bool exito = false;
            respuesta.resultado = false;
            DA_clases.DA_Tickets.registra_ingreso(request.codigo_qr, request.codigo_recaudacion, ref exito);
            if (exito)
            {
                respuesta.mensaje = "OK";
            }
            return respuesta;
        }
    }
    public class ListarEventosController : ApiController
    {
        public response_listar_eventos Post(request_listar_eventos request)
        {
            response_listar_eventos respuesta = new response_listar_eventos();
            if (request == null)
            {
                respuesta.eventos = DA_clases.DA_Eventos.lista_eventos("", 0, 0);
            }
            else
            {
                respuesta.eventos = DA_clases.DA_Eventos.lista_eventos(request.fecha_evento, request.id_tipo_evento, request.id_lugar);
            }
            return respuesta;

        }
        
    }
    public class ListarTiposEventoController : ApiController
    {
        public response_listar_tipos_evento Post(request_listar_tipos_evento request)
        {
            response_listar_tipos_evento respuesta = new response_listar_tipos_evento();
            respuesta.tipos_evento = DA_clases.DA_Eventos.lista_tipos_evento();
            return respuesta;

        }
    }
    public class ObtenerEventoController : ApiController
    {
        public Eventos_app Post(request_obtener_evento request)
        {
            Eventos_app response = new Eventos_app();
            response = DA_clases.DA_Eventos.obtener_evento(request.id_evento);
            return response;
        }
    }
    public class ConsultaComisionController : ApiController
    {
        public List<response_consulta_comision> Post(List<request_consulta_comision> request)
        {
            List<response_consulta_comision> response = new List<response_consulta_comision>();
            foreach (request_consulta_comision req in request)
            {
                response_consulta_comision resp = new response_consulta_comision();
                decimal monto_comision = DA_clases.DA_Tickets.consulta_comision(req.id_sector);
                monto_comision = monto_comision * req.cantidad_tickets;
                resp.monto_comision = monto_comision;
                resp.id_sector = req.id_sector;
                response.Add(resp);
            }
            return response;
        }
    }
    public class ObtenerPuntosPagoController : ApiController
    {
        public response_obtiene_puntos_pago Post(request_obtiene_puntos_pago request)
        {
            response_obtiene_puntos_pago response = new response_obtiene_puntos_pago();
            response.puntos_pago = DA_clases.DA_Puntos_Pago.Obtiene_Puntos_Pago();
            return response;
        }
    }
    public class ObtenerHistorialComprasController : ApiController
    {
        private TNTEntities db = new TNTEntities();
        public response_obtener_historial_compra Post(request_obtener_historial_compra request)
        {
            response_obtener_historial_compra response = new response_obtener_historial_compra();
            var compras = db.Compra.Where(cmp => cmp.id_usuario_compra == request.id_usuario && cmp.fecha_pago!=null).Take(5);
            response.historial_compras = new List<Compra_app_historial>();
            foreach (var compra in compras)
            {
                Compra_app_historial compra_h = new Compra_app_historial();
                if (compra.fecha_pago == null)
                {
                    compra_h.fecha_pago = "";
                }
                else
                {
                    compra_h.fecha_pago = compra.fecha_pago.Value.ToString("yyyy/MM/dd hh:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                }
                compra_h.estado = "2";
                compra_h.fecha_reserva = compra.fecha_compra.Value.ToString("yyyy/MM/dd hh:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                compra_h.id_compra = compra.id;
                compra_h.monto_total =(decimal) compra.monto_cobrar;
                var tickets = db.Ticket.FirstOrDefault(tck=>tck.codigo_recaudacion == compra.codigo_recaudacion);
                compra_h.nombre_evento = tickets.Eventos.nombre_evento;
                compra_h.codigo_recaudacion = tickets.codigo_recaudacion;
                response.historial_compras.Add(compra_h);
            }
            var reservas = db.Compra.Where(cmp => cmp.id_usuario_compra == request.id_usuario && cmp.fecha_pago == null).Take(5);
            foreach (var compra in reservas)
            {
                Compra_app_historial compra_h = new Compra_app_historial();
                if (compra.fecha_pago == null)
                {
                    compra_h.fecha_pago = "";
                }
                else
                {
                    compra_h.fecha_pago = compra.fecha_pago.Value.ToString("yyyy/MM/dd hh:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                }
                compra_h.estado = "1";
                compra_h.fecha_reserva = compra.fecha_compra.Value.ToString("yyyy/MM/dd hh:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                compra_h.id_compra = compra.id;
                compra_h.monto_total = (decimal)compra.monto_cobrar;
                var tickets = db.Ticket.FirstOrDefault(tck => tck.codigo_recaudacion == compra.codigo_recaudacion);
                compra_h.nombre_evento = tickets.Eventos.nombre_evento;
                compra_h.codigo_recaudacion = "";
                response.historial_compras.Add(compra_h);
            }

            
            return response;
        }
    }
    public class RecuperarContrasenaController : ApiController
    {
        private TNTEntities db = new TNTEntities();
        public response_recuperar_contrasena Post(request_recuperar_contrasena request)
        {
            response_recuperar_contrasena response = new response_recuperar_contrasena();
            Usuarios usuario = db.Usuarios.FirstOrDefault(us => us.email == request.email);
            if (usuario == null)
            {
                response.message = "email incorrecto";
            }
            else
            {
                try
                {
                    string old_password = usuario.password;
                    string new_pass = helpers.Helpers.Generar_pass_temporal(6, new Random());
                    usuario.password = new_pass;
                    if (ModelState.IsValid)
                    {
                        db.Entry(usuario).State = System.Data.EntityState.Modified;
                        db.SaveChanges();
                    }
                    helpers.Helpers.EnviarMail("admin@tnt.com", "admin", usuario.email, usuario.Personas.First().nombre, "cambio de password", "<h2>Usted solicito un nuevo password</h2><h3>password:</h3>" + new_pass, null);
                    response.message = old_password;
                }
                catch (Exception)
                {
                    response.message = "ocurrio un error por favor intente de nuevo";
                }
               
            }
            return response;
        }
    }
    public class EnviarTFCorreoController : ApiController
    {
        private TNTEntities db = new TNTEntities();
        public response_enviar_factura_ticket_correo Post(request_enviar_factura_ticket_correo request)
        {
            response_enviar_factura_ticket_correo response = new response_enviar_factura_ticket_correo();
            try
            {
                Compra compra = db.Compra.FirstOrDefault(com => com.codigo_recaudacion == request.codigo_recaudacion);
                compra = db.Compra.Find(compra.id);
                //verificamos si la compra esta paga
                if (compra.pagado == 0)
                {
                    response.resultado = false;
                    return response;
                }
                compra.fecha_pago = DateTime.Now;
                string template = @"<table border=""1"" style=""width:100%;"">
                    <tr><td>{0}</td><td>LOGO EMPRESA</td></tr>
                    <tr><td>NIT: {1} </td></tr>
                    <tr><td>Direccion: {2} </td></tr>
                    <tr><td>Factura nro: {3} </td></tr>
                    <tr><td>Codigo Autorizacion: {4} </td></tr>
                    <tr><td>Codigo Control: 2A-4B-3C-2D-3A </td></tr>
                    <tr><td>ORIGINAL: CLIENTE </td></tr>
                </table>
                <table border=""1"" style=""width:100%;"">
                <tr><td><img src='http://qrickit.com/api/qr?d={5}'></td><td><h1>{6}</h1><h2>{7}</h2><h3>{8}</h3></td></tr>
                </table>
                <table border=""1"" style=""width:100%;text-align: center;"">
                <tr><td><h2>{9}</h2><h3>{10}</h3>Valido para Credito fiscal</td></tr>
                <tr><td>Fecha Limite de Emision:22/04/2016</td></tr>
                <tr>
                <td style=""text-align:left"">Creado por TNT impreso el {11}</td>
                </tr>
                </table>
                <br/>
                <br/>
                <br/>
                <br/>
                <br/>
                <br/>
                <br/>
                <br/>
                <br/>
                <br/>
                <br/>
                <br/>
                <br/>
                <br/>
                <br/>
                <br/>
                <br/>
                <br/>
                <br/>
                <br/>
                <br/>
                <br/>";
                //obtencion de datos de ticket (o tickets) 
                var tickets = db.Ticket.Where(tick => tick.codigo_recaudacion == compra.codigo_recaudacion);

                StringBuilder srb = new StringBuilder();
                string result = "";
                foreach (Ticket ticket in tickets)
                {
                    template = String.Format(template, new object[] { 
                        ticket.Eventos.Empresas.nombre_empresa, 
                        ticket.Eventos.Empresas.nit,
                        ticket.Eventos.Empresas.direccion,
                        compra.numero_factura,
                        ticket.Eventos.Empresas.dosificacion_codigo_autorizacion,
                        ticket.codigo,
                        ticket.Eventos.nombre_evento,
                        ticket.Eventos.fecha_evento,
                        ticket.Eventos.Lugares.nombre_lugar +",hrs:"+ticket.Eventos.hora_evento,
                        "Bs. " + ticket.sectores.precio_unitario,
                        ticket.sectores.descripcion,
                        DateTime.Now.ToShortDateString()                        
                    });
                    result += template;

                }

                byte[] attach = helpers.Helpers.PdfSharpConvert(result);
                Usuarios usuario = db.Usuarios.FirstOrDefault(us => us.id == compra.id_usuario_compra);
                string email_to = usuario.email;
                helpers.Helpers.EnviarMail("admin@tnt.com", "admin", email_to, usuario.Personas.First().nombre, "tickets", "<h1>EMAIL TICKETS</h1>", attach);
                //envio de factura
                LocalReport lr = new LocalReport();
                string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory + @"Reports\", "factura.rdlc");
                Empresas empresa = tickets.First().Eventos.Empresas;
                string fechaEmision = DateTime.Now.AddMonths(3).ToString("dd/MM/yyyy");
                string codigoControl = helpers.Helpers.Obtener_codigo_control(empresa.dosificacion_codigo_autorizacion, compra.id.ToString(), tickets.First().nit_usuario.Trim(), DateTime.Now.ToString("yyyyMMdd"), compra.monto_parcial.ToString(), empresa.dosificacion_llave);
                string codigoQR = empresa.nit + "|" + compra.id.ToString() + "|" + empresa.dosificacion_codigo_autorizacion + "|" + DateTime.Now.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture) + "|" + compra.monto_parcial.ToString() + "|" + compra.monto_parcial.ToString() + "|" + codigoControl + "|" + tickets.First().nit_usuario + "|" + "0|0|0|0";
                string qrCodigo = "http://qrickit.com/api/qr?d=" + codigoQR;

                lr.ReportPath = path;
                lr.EnableExternalImages = true;
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-ES");
                lr.SetParameters(new ReportParameter("ParamEmpresaNombre", empresa.nombre_empresa));
                lr.SetParameters(new ReportParameter("ParamDireccion", empresa.direccion));
                lr.SetParameters(new ReportParameter("ParamTelefono", empresa.telefono));
                lr.SetParameters(new ReportParameter("ParamDepartamento", empresa.departamento));
                lr.SetParameters(new ReportParameter("ParamNitEmpresa", empresa.nit));
                lr.SetParameters(new ReportParameter("ParamNroFactura", compra.id.ToString()));
                lr.SetParameters(new ReportParameter("ParamNumeroAutorizacion", empresa.dosificacion_codigo_autorizacion));
                lr.SetParameters(new ReportParameter("ParamRubroEmpresa", empresa.dosificacion_actividad_comercial));
                lr.SetParameters(new ReportParameter("ParamLugarFecha", "La Paz," + DateTime.Now.ToLongDateString()));
                lr.SetParameters(new ReportParameter("ParamCliente", tickets.First().nombre_usuario));
                lr.SetParameters(new ReportParameter("ParamNitCliente", tickets.First().nit_usuario));
                lr.SetParameters(new ReportParameter("ParamDescripcionFactura", "TICKETS EVENTO:" + tickets.First().Eventos.nombre_evento));
                lr.SetParameters(new ReportParameter("ParamMonto", compra.monto_parcial.ToString()));
                lr.SetParameters(new ReportParameter("ParamMontoLiteral", helpers.NumLetra.Convertir(compra.monto_parcial.ToString(), false)));
                lr.SetParameters(new ReportParameter("ParamCodigoControl", codigoControl));
                lr.SetParameters(new ReportParameter("ParamFechaEmision", fechaEmision));
                lr.SetParameters(new ReportParameter("ParamURLQR", qrCodigo));



                string reportType = "PDF"; //puede ser PDF,Excel,Word,Image
                string mimeType;
                string encoding;
                string fileNameExtension;
                string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + reportType + "</OutputFormat>" +
                "  <PageWidth>8.5in</PageWidth>" +
                "  <PageHeight>11in</PageHeight>" +
                "  <MarginTop>0.5in</MarginTop>" +
                "  <MarginLeft>0in</MarginLeft>" +
                "  <MarginRight>0in</MarginRight>" +
                "  <MarginBottom>0.5in</MarginBottom>" +
                "</DeviceInfo>";

                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes;

                renderedBytes = lr.Render(
                    reportType,
                    deviceInfo,
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);

                helpers.Helpers.EnviarMail("admin@tnt.com", "admin", email_to, usuario.Personas.First().nombre, "factura", "<h1>FACTURA</h1>", renderedBytes);
                response.resultado = true;
            }
            catch (Exception ex)
            {

                response.resultado = false;
            }
            return response;
        }
    }
}
