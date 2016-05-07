using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TNT.Models;

namespace TNT.Controllers
{
    public class CompraController : Controller
    {
        private TNTEntities db = new TNTEntities();

        //
        // GET: /Compra/

        public ActionResult Index()
        {
            var compra = db.Compra.Include(c => c.Usuarios);
            return View(compra.ToList());
        }
        public ActionResult SimuladorPago()
        {
            var compra = db.Compra.Where(com=>com.pagado == 0).Include(c => c.Usuarios);
            return View(compra.ToList());
        }
        public ActionResult PagoManualDeuda(int id)
        {
            Compra compra = db.Compra.Find(id);
            if (compra == null)
            {
                return HttpNotFound();
            }
            return View(compra);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PagoManualDeuda(Compra compra)
        {
            compra = db.Compra.Find(compra.id);
            if (ModelState.IsValid)
            {
                compra.pagado = 1;
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
                    db.Entry(ticket).State = EntityState.Modified;
                    ticket.valida = 1;
                    result += template;
                    
                }

                byte[] attach = helpers.Helpers.PdfSharpConvert(result);
                Usuarios usuario = db.Usuarios.FirstOrDefault(us => us.id==compra.id_usuario_compra);
                string email_to = usuario.email;
                helpers.Helpers.EnviarMail("admin@tnt.com", "admin", email_to, usuario.Personas.First().nombre, "tickets", "<h1>EMAIL TICKETS</h1>", attach);
                //envio de factura
                LocalReport lr = new LocalReport();
                string path = System.IO.Path.Combine(Server.MapPath("~/Reports"), "factura.rdlc");
                Empresas empresa = tickets.First().Eventos.Empresas;
                string fechaEmision =DateTime.Now.AddMonths(3).ToString("dd/MM/yyyy");
                string codigoControl = helpers.Helpers.Obtener_codigo_control(empresa.dosificacion_codigo_autorizacion,compra.id.ToString(),tickets.First().nit_usuario,DateTime.Now.ToString("yyyyMMdd"),compra.monto_parcial.ToString(),empresa.dosificacion_llave);
                string qrCodigo = "http://qrickit.com/api/qr?d=" + "camilo";
                lr.ReportPath = path;
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-ES");
                lr.SetParameters(new ReportParameter("ParamEmpresaNombre",empresa.nombre_empresa));
                lr.SetParameters(new ReportParameter("ParamDireccion", empresa.direccion ));
                lr.SetParameters(new ReportParameter("ParamTelefono", empresa.telefono));
                lr.SetParameters(new ReportParameter("ParamDepartamento", empresa.departamento ));
                lr.SetParameters(new ReportParameter("ParamNitEmpresa", empresa.nit ));
                lr.SetParameters(new ReportParameter("ParamNroFactura", compra.id.ToString()));
                lr.SetParameters(new ReportParameter("ParamNumeroAutorizacion", empresa.dosificacion_codigo_autorizacion));
                lr.SetParameters(new ReportParameter("ParamRubroEmpresa", empresa.dosificacion_actividad_comercial));
                lr.SetParameters(new ReportParameter("ParamLugarFecha", "La Paz,"+DateTime.Now.ToLongDateString()));
                lr.SetParameters(new ReportParameter("ParamCliente", tickets.First().nombre_usuario));
                lr.SetParameters(new ReportParameter("ParamNitCliente", tickets.First().nit_usuario));
                lr.SetParameters(new ReportParameter("ParamDescripcionFactura", "TICKETS EVENTO:"+tickets.First().Eventos.nombre_evento));
                lr.SetParameters(new ReportParameter("ParamMonto", compra.monto_parcial.ToString()));
                lr.SetParameters(new ReportParameter("ParamMontoLiteral",helpers.NumLetra.Convertir(compra.monto_parcial.ToString(),false)));
                lr.SetParameters(new ReportParameter("ParamCodigoControl",codigoControl));
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
                db.Entry(compra).State = EntityState.Modified;  
                db.SaveChanges();
                return RedirectToAction("SimuladorPago");
            }
            ViewBag.id_usuario_compra = new SelectList(db.Usuarios, "id", "email", compra.id_usuario_compra);
            return View(compra);
        }
        //
        // GET: /Compra/Details/5

        public ActionResult Details(int id = 0)
        {
            Compra compra = db.Compra.Find(id);
            if (compra == null)
            {
                return HttpNotFound();
            }
            return View(compra);
        }

        //
        // GET: /Compra/Create

        public ActionResult Create()
        {
            ViewBag.id_usuario_compra = new SelectList(db.Usuarios, "id", "email");
            return View();
        }

        //
        // POST: /Compra/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Compra compra)
        {
            if (ModelState.IsValid)
            {
                db.Compra.Add(compra);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_usuario_compra = new SelectList(db.Usuarios, "id", "email", compra.id_usuario_compra);
            return View(compra);
        }

        //
        // GET: /Compra/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Compra compra = db.Compra.Find(id);
            if (compra == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_usuario_compra = new SelectList(db.Usuarios, "id", "email", compra.id_usuario_compra);
            return View(compra);
        }

        //
        // POST: /Compra/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Compra compra)
        {
            if (ModelState.IsValid)
            {
                db.Entry(compra).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_usuario_compra = new SelectList(db.Usuarios, "id", "email", compra.id_usuario_compra);
            return View(compra);
        }

        //
        // GET: /Compra/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Compra compra = db.Compra.Find(id);
            if (compra == null)
            {
                return HttpNotFound();
            }
            return View(compra);
        }

        //
        // POST: /Compra/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Compra compra = db.Compra.Find(id);
            db.Compra.Remove(compra);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}