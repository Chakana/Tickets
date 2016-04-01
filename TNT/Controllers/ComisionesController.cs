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
    public class ComisionesController : Controller
    {
        private TNTEntities db = new TNTEntities();

        //
        // GET: /Comisiones/

        public ActionResult Index()
        {
            var comisiones = db.comisiones.Include(c => c.Usuarios);
            return View(comisiones.ToList());
        }

        //
        // GET: /Comisiones/Details/5

        public ActionResult Details(int id = 0)
        {
            comisiones comisiones = db.comisiones.Find(id);
            if (comisiones == null)
            {
                return HttpNotFound();
            }
            return View(comisiones);
        }

        //
        // GET: /Comisiones/Create

        public ActionResult Create()
        {
            ViewBag.usuario_modificacion = new SelectList(db.Usuarios, "id", "email");
            return View();
        }

        //
        // POST: /Comisiones/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(comisiones comisiones)
        {
            if (ModelState.IsValid)
            {
                db.comisiones.Add(comisiones);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.usuario_modificacion = new SelectList(db.Usuarios, "id", "email", comisiones.usuario_modificacion);
            return View(comisiones);
        }

        //
        // GET: /Comisiones/Edit/5

        public ActionResult Edit(int id = 0)
        {
            comisiones comisiones = db.comisiones.Find(id);
            if (comisiones == null)
            {
                return HttpNotFound();
            }
            ViewBag.usuario_modificacion = new SelectList(db.Usuarios, "id", "email", comisiones.usuario_modificacion);
            return View(comisiones);
        }

        //
        // POST: /Comisiones/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(comisiones comisiones)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comisiones).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.usuario_modificacion = new SelectList(db.Usuarios, "id", "email", comisiones.usuario_modificacion);
            return View(comisiones);
        }

        //
        // GET: /Comisiones/Delete/5

        public ActionResult Delete(int id = 0)
        {
            comisiones comisiones = db.comisiones.Find(id);
            if (comisiones == null)
            {
                return HttpNotFound();
            }
            return View(comisiones);
        }

        //
        // POST: /Comisiones/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            comisiones comisiones = db.comisiones.Find(id);
            db.comisiones.Remove(comisiones);
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