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
    public class PersonasController : Controller
    {
        private TNTEntities db = new TNTEntities();

        //
        // GET: /Personas/

        public ActionResult Index()
        {
            var personas = db.Personas.Include(p => p.Usuarios);
            return View(personas.ToList());
        }

        //
        // GET: /Personas/Details/5

        public ActionResult Details(int id = 0)
        {
            Personas personas = db.Personas.Find(id);
            if (personas == null)
            {
                return HttpNotFound();
            }
            return View(personas);
        }

        //
        // GET: /Personas/Create

        public ActionResult Create()
        {
            ViewBag.id_usuario = new SelectList(db.Usuarios, "id", "email");
            return View();
        }

        //
        // POST: /Personas/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Personas personas)
        {
            if (ModelState.IsValid)
            {
                db.Personas.Add(personas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_usuario = new SelectList(db.Usuarios, "id", "email", personas.id_usuario);
            return View(personas);
        }

        //
        // GET: /Personas/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Personas personas = db.Personas.Find(id);
            if (personas == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_usuario = new SelectList(db.Usuarios, "id", "email", personas.id_usuario);
            return View(personas);
        }

        //
        // POST: /Personas/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Personas personas)
        {
            if (ModelState.IsValid)
            {
                personas.fecha_modificacion = DateTime.Now;
                db.Entry(personas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_usuario = new SelectList(db.Usuarios, "id", "email", personas.id_usuario);
            return View(personas);
        }

        //
        // GET: /Personas/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Personas personas = db.Personas.Find(id);
            if (personas == null)
            {
                return HttpNotFound();
            }
            return View(personas);
        }

        //
        // POST: /Personas/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Personas personas = db.Personas.Find(id);
            db.Personas.Remove(personas);
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