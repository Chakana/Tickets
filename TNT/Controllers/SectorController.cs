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
    public class SectorController : Controller
    {
        private TNTEntities db = new TNTEntities();

        //
        // GET: /Sector/

        public ActionResult Index(string busqueda)
        {
            
            var sectores = db.sectores.Include(s => s.Eventos);
            if (User.IsInRole("empresa"))
            {

                int empresa_id = (int)Session["empresa_id"];
                sectores = sectores.Where(a => a.Eventos.id_empresa == empresa_id);
            }
            if (!String.IsNullOrEmpty(busqueda))
            {
                sectores = sectores.Where(s => s.Eventos.nombre_evento.Contains(busqueda)
                                       || s.descripcion.Contains(busqueda));
            }
            
            return View(sectores.ToList());
        }

        //
        // GET: /Sector/Details/5

        public ActionResult Details(int id = 0)
        {
            sectores sectores = db.sectores.Find(id);
            if (sectores == null)
            {
                return HttpNotFound();
            }
            return View(sectores);
        }
        public ActionResult CrearSectoresEvento(int id)
        {
            var evento = db.Eventos.Find(id);
            ViewBag.id_evento = evento.id;
            ViewBag.nombre_evento = evento.nombre_evento;
            /*if (User.IsInRole("empresa"))
            {
                int empresa_id = (int)Session["empresa_id"];
                ViewBag.id_evento = new SelectList(db.Eventos.Where(ev => ev.id == id), "id", "nombre_evento");
            }
            else
            {
                ViewBag.id_evento = new SelectList(db.Eventos, "id", "nombre_evento");
            }*/
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CrearSectoresEvento(sectores sectores)
        {
            sectores.id_evento = Int32.Parse(RouteData.Values["id"].ToString());
            if (ModelState.IsValid)
            {
                db.sectores.Add(sectores);
                db.SaveChanges();
                ViewBag.message = "Sector Creado";
            }

            ViewBag.id_evento = new SelectList(db.Eventos, "id", "nombre_evento", sectores.id_evento);
            return View();
        }

        public PartialViewResult AddSectoresPartialView()
        {
            return PartialView("AddSectoresPartialView", new sectores());
        }

        

        //
        // GET: /Sector/Create

        public ActionResult Create()
        {
            if (User.IsInRole("empresa"))
            {
                int empresa_id = (int)Session["empresa_id"];
                ViewBag.id_evento = new SelectList(db.Eventos.Where(ev => ev.id_empresa == empresa_id), "id", "nombre_evento");
            }
            else
            {
                ViewBag.id_evento = new SelectList(db.Eventos, "id", "nombre_evento");
            }
            return View();
        }

        //
        // POST: /Sector/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(sectores sectores)
        {
            if (ModelState.IsValid)
            {
                db.sectores.Add(sectores);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_evento = new SelectList(db.Eventos, "id", "nombre_evento", sectores.id_evento);
            return View(sectores);
        }
        public JsonResult ObtenerDatosSector(int id)
        {
            sectores sector = db.sectores.Find(id);

            return new JsonResult { Data = new { filas = sector.filas,columnas=sector.columnas } };
        }
        //
        // GET: /Sector/Edit/5

        public ActionResult Edit(int id = 0)
        {

            sectores sectores = db.sectores.Find(id);
            if (sectores == null)
            {
                return HttpNotFound();
            }

            if (User.IsInRole("empresa"))
            {
                int empresa_id = (int)Session["empresa_id"];
                ViewBag.id_evento = new SelectList(db.Eventos.Where(ev => ev.id_empresa == empresa_id), "id", "nombre_evento");
            }
            else
            {
                ViewBag.id_evento = new SelectList(db.Eventos, "id", "nombre_evento");
            }
            return View(sectores);

        }

        //
        // POST: /Sector/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(sectores sectores)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sectores).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_evento = new SelectList(db.Eventos, "id", "nombre_evento", sectores.id_evento);
            return View(sectores);
        }

        //
        // GET: /Sector/Delete/5

        public ActionResult Delete(int id = 0)
        {
            sectores sectores = db.sectores.Find(id);
            if (sectores == null)
            {
                return HttpNotFound();
            }
            return View(sectores);
        }

        //
        // POST: /Sector/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            sectores sectores = db.sectores.Find(id);
            db.sectores.Remove(sectores);
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