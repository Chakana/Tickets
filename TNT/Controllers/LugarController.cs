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
    public class LugarController : Controller
    {
        private TNTEntities db = new TNTEntities();

        //
        // GET: /Lugar/

        public ActionResult Index()
        {
            return View(db.Lugares.ToList());
        }

        //
        // GET: /Lugar/Details/5

        public ActionResult Details(int id = 0)
        {
            Lugares lugares = db.Lugares.Find(id);
            if (lugares == null)
            {
                return HttpNotFound();
            }
            return View(lugares);
        }

        //
        // GET: /Lugar/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Lugar/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Lugares lugares)
        {
            if (ModelState.IsValid)
            {
                db.Lugares.Add(lugares);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(lugares);
        }

        //
        // GET: /Lugar/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Lugares lugares = db.Lugares.Find(id);
            if (lugares == null)
            {
                return HttpNotFound();
            }
            return View(lugares);
        }

        //
        // POST: /Lugar/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Lugares lugares)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lugares).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(lugares);
        }

        //
        // GET: /Lugar/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Lugares lugares = db.Lugares.Find(id);
            if (lugares == null)
            {
                return HttpNotFound();
            }
            return View(lugares);
        }

        //
        // POST: /Lugar/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Lugares lugares = db.Lugares.Find(id);
            db.Lugares.Remove(lugares);
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