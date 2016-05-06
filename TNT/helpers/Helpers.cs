using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TNT.WebSintesis_externo;
using System.Globalization;
using System.Web.Mvc;
using System.Net.Mail;
using System.Net.Mime;

namespace TNT.helpers
{

    public class DecimalModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext,
            ModelBindingContext bindingContext)
        {
            ValueProviderResult valueResult = bindingContext.ValueProvider
                .GetValue(bindingContext.ModelName);
            ModelState modelState = new ModelState { Value = valueResult };
            object actualValue = null;
            try
            {
                actualValue = Convert.ToDecimal(valueResult.AttemptedValue,
                    CultureInfo.CurrentCulture);
            }
            catch (FormatException e)
            {
                modelState.Errors.Add(e);
            }

            bindingContext.ModelState.Add(bindingContext.ModelName, modelState);
            return actualValue;
        }
    }

    public class Helpers
    {
        public static string Generar_codigo_recaudacion(string iniciales)
        {
            int length = 4;
            Random random = new Random();
            string fecha = DateTime.Now.ToString("yyyyMMdd");
            //var chars = "0123456789";
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var result = new string(
                Enumerable.Repeat(chars, length)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            //result = "TNT" + fecha + iniciales + result;
            result = "321" + fecha + result;
            return result;
        }
        public static string Generar_codigo_ticket(int length, Random random)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var result = new string(
                Enumerable.Repeat(chars, length)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            //result = CryptoClass.Encrypt(result, System.Configuration.ConfigurationManager.AppSettings["passphrase"]);
            return result;
        }
        public static string Generar_pass_temporal(int length, Random random)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var result = new string(
                Enumerable.Repeat(chars, length)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            //result = CryptoClass.Encrypt(result, System.Configuration.ConfigurationManager.AppSettings["passphrase"]);
            return result;
        } 
        public static Byte[] PdfSharpConvert(String html)
        {
            Byte[] res = null;
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                var pdf = TheArtOfDev.HtmlRenderer.PdfSharp.PdfGenerator.GeneratePdf(html, PdfSharp.PageSize.A4);
                pdf.Save(ms);
                res = ms.ToArray();
            }
            return res;
        }
        public static bool EnviarMail(string email_from, string from, string email_to, string to, string subject, string contenido, byte[] attach)
        {
            bool enviado = false;
            try
            {
                MailMessage mailMsg = new MailMessage();

                // To
                mailMsg.To.Add(new MailAddress(email_to, to));

                // From
                mailMsg.From = new MailAddress(email_from, from);

                // Subject and multipart/alternative Body
                mailMsg.Subject = subject;
                //string text = "text body";
                string html = contenido;
                //mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
                mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));
                if (attach!=null)
                {
                    string filename="tickets.pdf";
                    Attachment attach_item=new Attachment(new System.IO.MemoryStream(attach), filename);
                    mailMsg.Attachments.Add(attach_item);
                }
                // Init SmtpClient and send
                SmtpClient smtpClient = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("lquenta", "aspire4520");
                smtpClient.Credentials = credentials;
                smtpClient.Send(mailMsg);
                enviado = true;
            }
            catch (Exception ex)
            {                
                Console.WriteLine(ex.Message);
            }
            return enviado;
        }
        public static bool Envio_sintesis_compra_simple(string codigo_recaudacion,string email_cliente,string nombre_cliente,string nit_cliente,string nombre_evento,string nombre_empresa,string nit_empresa,double montoTicket,double comision)
        {
            ComelecSoapClient cli = new ComelecSoapClient();
            DatosPlanilla datosPlanilla = new DatosPlanilla();
            DPlanilla[] dPlanilla = new DPlanilla[2];
            RespPlanilla respPlanilla = new RespPlanilla();
            DateTime now_date = new DateTime();


            datosPlanilla.codigoCliente = "1";
            datosPlanilla.codigoEmpresa = 321;
            datosPlanilla.codigoProducto = "1";
            datosPlanilla.codigoRecaudacion = codigo_recaudacion;
            datosPlanilla.correoElectronico = email_cliente;
            datosPlanilla.descripcionRecaudacion = "COMPRA TICKETS"; //SACAR DE CONFIG
            datosPlanilla.fecha = Int32.Parse(now_date.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture));
            datosPlanilla.hora = Int32.Parse(now_date.ToString("hhmmss", System.Globalization.CultureInfo.InvariantCulture));
            datosPlanilla.fechaVencimiento = 0;
            datosPlanilla.horaVencimiento = 0;
            datosPlanilla.moneda = "BS";
            datosPlanilla.nombre = nombre_cliente;
            datosPlanilla.nit_CI_cliente = nit_cliente;
            datosPlanilla.transaccion = "A";

            dPlanilla[0] = new DPlanilla();
            dPlanilla[0].numeroPago = 1;
            dPlanilla[0].descripcion = nombre_evento;
            dPlanilla[0].nombreFactura = nombre_empresa;
            dPlanilla[0].nitFactura = nit_empresa;
            dPlanilla[0].montoPago = montoTicket;
            dPlanilla[0].montoCreditoFiscal = 0;

            dPlanilla[1] = new DPlanilla();
            dPlanilla[1].numeroPago = 2;
            dPlanilla[1].descripcion = "Comision"; // TODO : SACAR DE CONFIG
            dPlanilla[1].nombreFactura = "YGDRASSIL"; //TODO : SACAR DE CONFIG
            dPlanilla[1].nitFactura = "111111119"; // SACAR DE CONFIG
            dPlanilla[1].montoPago = comision; 
            dPlanilla[1].montoCreditoFiscal = comision;

            datosPlanilla.planillas = dPlanilla;
            try
            {
                respPlanilla = cli.cmeRegistroPlan(datosPlanilla, "wsiggdrasil");
            }
            catch (Exception ex)
            {
                return false;    
            }            
            
            if (respPlanilla.codError == 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}