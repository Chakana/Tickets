using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TNT.Models;

namespace TNT.Controllers
{
    public class EventController : Controller
    {
        private TNTEntities db = new TNTEntities();

        //
        // GET: /Event/

        public ActionResult Index()
        {
            
            if (User.IsInRole("empresa"))
            {
                int empresa_id = (int)Session["empresa_id"];
                var eventos = db.Eventos.Where(evento => evento.id_empresa == empresa_id).Include(e => e.Lugares).Include(e => e.Tipos_evento);
                eventos = eventos.Where(ev => ev.fecha_evento >= DateTime.Now && ev.habilitado == true);
                return View(eventos.ToList());
            }
            if (User.IsInRole("admin"))
            {
                var eventos = db.Eventos.Include(e => e.Empresas).Include(e => e.Lugares).Include(e => e.Tipos_evento);
                eventos=eventos.Where(ev => ev.fecha_evento >= DateTime.Now && ev.habilitado == true);
                return View(eventos.ToList());
            }
            return View();
        }

        public ActionResult VerEventosUsuario()
        {
            var eventos = db.Eventos.Where( e=>e.sectores.Count >0 && e.habilitado==true).Include(e => e.Empresas).Include(e => e.Lugares).Include(e => e.Tipos_evento);
            eventos = eventos.Where(ev => ev.fecha_evento >= DateTime.Now);
            return View(eventos.ToList());            
        }
        public ActionResult VerTodosEventos()
        {
            if (User.IsInRole("admin"))
            {
                var eventos = db.Eventos.Include(e => e.Empresas).Include(e => e.Lugares).Include(e => e.Tipos_evento).OrderByDescending(ev=>ev.id);                
                return View(eventos.ToList());
            }
            return View();
        }
        public ActionResult PendientesHabilitacion()
        {
            if (User.IsInRole("admin"))
            {
                var eventos = db.Eventos.Where(e=>e.habilitado==false).Include(e => e.Empresas).Include(e => e.Lugares).Include(e => e.Tipos_evento);
                eventos = eventos.Where(ev => ev.fecha_evento >= DateTime.Now);
                return View(eventos.ToList());
            }
            return View();
        }
        
        //
        // GET: /Event/Details/5

        public ActionResult Details(int id = 0)
        {
            Eventos eventos = db.Eventos.Find(id);
            if (eventos == null)
            {
                return HttpNotFound();
            }
            return View(eventos);
        }

        //
        // GET: /Event/Create

        public ActionResult Create()
        {
            if (User.IsInRole("empresa"))
            {
                int empresa_id = (int)Session["empresa_id"];
                ViewBag.id_empresa = new SelectList(db.Empresas.Where(empresas => empresas.id == empresa_id), "id", "nombre_empresa");
            }
            else
            {
                ViewBag.id_empresa = new SelectList(db.Empresas, "id", "nombre_empresa");
            }
            
            ViewBag.id_lugar = new SelectList(db.Lugares, "id", "nombre_lugar");
            ViewBag.id_tipo_evento = new SelectList(db.Tipos_evento, "id", "descripcion");
            return View();
        }
        public JsonResult ObtenerReservasEvento(int id,int sector)
        {
            Eventos evento = db.Eventos.Find(id);
            sectores info_sector = db.sectores.Find(sector);
            List<int> resultado;
            if (info_sector.es_sector_numerado == false)
            {
                resultado = new List<int>{0};
            }
            else
            {
                resultado = evento.Ticket.Where(evt => evt.id_sector == sector).Select(evT => Int32.Parse(evT.butaca)).ToList();
            }
             
           
            return new JsonResult { Data = resultado };
        }
        [Authorize(Roles = "admin")]
        public JsonResult HabilitarEvento(int id)
        {
            bool status = false;
            Eventos evento = db.Eventos.Find(id);
            evento.habilitado = true;
            db.Entry(evento).State = EntityState.Modified;
            db.SaveChanges();
            status = true;
            return new JsonResult { Data = new { status = status } };

        }
        public JsonResult GrabarEventoSector(NuevoEvento evento)
        {
            bool status = false;
            //if (ModelState.IsValid)
            {
                if (User.IsInRole("empresa"))
                {
                    evento.id_empresa = (int)Session["empresa_id"];
                }
                Eventos NuevoEvento=new Eventos();
                NuevoEvento.descripcion = evento.descripcion;
                NuevoEvento.fecha_evento = evento.fecha_evento;
                NuevoEvento.habilitado = false;
                NuevoEvento.hora_evento = evento.hora_evento;
                NuevoEvento.id_empresa = evento.id_empresa;
                NuevoEvento.id_lugar = evento.id_lugar;
                NuevoEvento.id_tipo_evento = evento.id_tipo_evento;
                NuevoEvento.img_url = evento.img_url;
                NuevoEvento.nombre_evento = evento.nombre_evento;
                NuevoEvento.departamento_facturacion = evento.departamento_facturacion;
                NuevoEvento.direccion_facturacion = evento.direccion_facturacion;
                NuevoEvento.nit_facturacion = evento.nit_facturacion;
                NuevoEvento.nombre_empresa_facturacion = evento.nombre_empresa_facturacion;
                NuevoEvento.numero_autorizacion_facturacion = evento.numero_autorizacion_facturacion;
                NuevoEvento.rubro_facturacion = evento.rubro_facturacion;
                NuevoEvento.telefono_facturacion = evento.telefono_facturacion;
                try
                {
                    db.Eventos.Add(NuevoEvento);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw;
                }
                
                List<sectores> sectores = new List<Models.sectores>();
                foreach (var sector in evento.sectores)
                {
                    Models.sectores nuevoSector = new Models.sectores()
                    {
                        asientos_disponibles = sector.asientos_disponibles,
                        descripcion = sector.descripcion,
                        id_evento = NuevoEvento.id,
                        img_url = "",
                        precio_unitario = sector.precio_unitario,
                        filas = sector.filas,
                        columnas = sector.columnas,
                        es_sector_numerado = (sector.filas > 0 && sector.columnas > 0)
                    };
                    db.sectores.Add(nuevoSector);
                }
                db.SaveChanges();
                //enviamos notifiacion por correo de evento pendiente de aprobacion
                helpers.Helpers.EnviarMail("admin@tnt.com", "admin", "eisenob@gmail.com", "admin", "EVENTO PENDIENTE", "<h1>Evento Pendiente de ser aprobado</h1><h2>" + NuevoEvento.nombre_evento + "</h2>", null);
                status = true;
            }
            return new JsonResult { Data = new { status = status } };

        }
        //
        // POST: /Event/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Eventos eventos)
        {
           
            if (ModelState.IsValid)
            {
               if (User.IsInRole("empresa"))
               {
                   eventos.id_empresa = (int)Session["empresa_id"];
               }
                db.Eventos.Add(eventos);
                db.SaveChanges();
                //enviamos notifiacion por correo de evento pendiente de aprobacion
                helpers.Helpers.EnviarMail("admin@tnt.com", "admin", "eisenob@gmail.com", "admin", "EVENTO PENDIENTE", "<h1>Evento Pendiente de ser aprobado</h1><h2>" + eventos.nombre_evento + "</h2>", null);
                return RedirectToAction("CrearSectoresEvento", "Sector", new { id = eventos.id });
            }

            if (User.IsInRole("empresa"))
            {
                int empresa_id = (int)Session["empresa_id"];
                ViewBag.id_empresa = new SelectList(db.Empresas.Where(empresas => empresas.id == empresa_id), "id", "nombre_empresa");
            }
            else
            {
                ViewBag.id_empresa = new SelectList(db.Empresas, "id", "nombre_empresa");
            }
            ViewBag.id_lugar = new SelectList(db.Lugares, "id", "nombre_lugar", eventos.id_lugar);
            ViewBag.id_tipo_evento = new SelectList(db.Tipos_evento, "id", "descripcion", eventos.id_tipo_evento);
            return View(eventos);
        }

        //
        // GET: /Event/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Eventos eventos = db.Eventos.Find(id);
            if (eventos == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_empresa = new SelectList(db.Empresas, "id", "nombre_empresa", eventos.id_empresa);
            ViewBag.id_lugar = new SelectList(db.Lugares, "id", "nombre_lugar", eventos.id_lugar);
            ViewBag.id_tipo_evento = new SelectList(db.Tipos_evento, "id", "descripcion", eventos.id_tipo_evento);
            return View(eventos);
        }

        //
        // POST: /Event/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Eventos eventos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eventos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_empresa = new SelectList(db.Empresas, "id", "nombre_empresa", eventos.id_empresa);
            ViewBag.id_lugar = new SelectList(db.Lugares, "id", "nombre_lugar", eventos.id_lugar);
            ViewBag.id_tipo_evento = new SelectList(db.Tipos_evento, "id", "descripcion", eventos.id_tipo_evento);
            return View(eventos);
        }

        //
        // GET: /Event/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Eventos eventos = db.Eventos.Find(id);
            if (eventos == null)
            {
                return HttpNotFound();
            }
            return View(eventos);
        }

        //
        // POST: /Event/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Eventos eventos = db.Eventos.Find(id);
            //si hay tickets no se puede borrar
            if (eventos.Ticket.Count > 0)
            {
                return RedirectToAction("Index");    
            }
            //borrar sectores
            var sectores_list = db.sectores.Where(ev => ev.id_evento == id);
            foreach (sectores sector in sectores_list)
            {
                db.sectores.Remove(sector);
                
            }
            db.SaveChanges();
            db.Eventos.Remove(eventos);
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