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
                //obtencion html template para cada ticket
                //conversion de html template a pdf
                //envio de pdf a correo de usuario
                
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