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
    public class PuntosPagosController : Controller
    {
        private TNTEntities db = new TNTEntities();

        //
        // GET: /PuntosPagos/

        public ActionResult Index()
        {
            return View(db.Puntos_pago.ToList());
        }

        //
        // GET: /PuntosPagos/Details/5

        public ActionResult Details(int id = 0)
        {
            Puntos_pago puntos_pago = db.Puntos_pago.Find(id);
            if (puntos_pago == null)
            {
                return HttpNotFound();
            }
            return View(puntos_pago);
        }

        //
        // GET: /PuntosPagos/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /PuntosPagos/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Puntos_pago puntos_pago)
        {
            if (ModelState.IsValid)
            {
                db.Puntos_pago.Add(puntos_pago);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(puntos_pago);
        }

        //
        // GET: /PuntosPagos/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Puntos_pago puntos_pago = db.Puntos_pago.Find(id);
            if (puntos_pago == null)
            {
                return HttpNotFound();
            }
            return View(puntos_pago);
        }

        //
        // POST: /PuntosPagos/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Puntos_pago puntos_pago)
        {
            if (ModelState.IsValid)
            {
                db.Entry(puntos_pago).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(puntos_pago);
        }

        //
        // GET: /PuntosPagos/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Puntos_pago puntos_pago = db.Puntos_pago.Find(id);
            if (puntos_pago == null)
            {
                return HttpNotFound();
            }
            return View(puntos_pago);
        }

        //
        // POST: /PuntosPagos/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Puntos_pago puntos_pago = db.Puntos_pago.Find(id);
            db.Puntos_pago.Remove(puntos_pago);
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