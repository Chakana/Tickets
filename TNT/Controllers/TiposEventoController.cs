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
    public class TiposEventoController : Controller
    {
        private TNTEntities db = new TNTEntities();

        //
        // GET: /TiposEvento/

        public ActionResult Index()
        {
            return View(db.Tipos_evento.ToList());
        }

        //
        // GET: /TiposEvento/Details/5

        public ActionResult Details(int id = 0)
        {
            Tipos_evento tipos_evento = db.Tipos_evento.Find(id);
            if (tipos_evento == null)
            {
                return HttpNotFound();
            }
            return View(tipos_evento);
        }

        //
        // GET: /TiposEvento/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /TiposEvento/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tipos_evento tipos_evento)
        {
            if (ModelState.IsValid)
            {
                db.Tipos_evento.Add(tipos_evento);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tipos_evento);
        }

        //
        // GET: /TiposEvento/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Tipos_evento tipos_evento = db.Tipos_evento.Find(id);
            if (tipos_evento == null)
            {
                return HttpNotFound();
            }
            return View(tipos_evento);
        }

        //
        // POST: /TiposEvento/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Tipos_evento tipos_evento)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipos_evento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipos_evento);
        }

        //
        // GET: /TiposEvento/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Tipos_evento tipos_evento = db.Tipos_evento.Find(id);
            if (tipos_evento == null)
            {
                return HttpNotFound();
            }
            return View(tipos_evento);
        }

        //
        // POST: /TiposEvento/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tipos_evento tipos_evento = db.Tipos_evento.Find(id);
            db.Tipos_evento.Remove(tipos_evento);
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