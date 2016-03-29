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
    public class EmpresasController : Controller
    {
        private TNTEntities db = new TNTEntities();

        //
        // GET: /Empresas/

        public ActionResult Index()
        {
            var empresas = db.Empresas.Include(e => e.Usuarios);
            return View(empresas.ToList());
        }

        //
        // GET: /Empresas/Details/5

        public ActionResult Details(int id = 0)
        {
            Empresas empresas = db.Empresas.Find(id);
            if (empresas == null)
            {
                return HttpNotFound();
            }
            return View(empresas);
        }

        //
        // GET: /Empresas/Create

        public ActionResult Create()
        {
            ViewBag.id_usuario = new SelectList(db.Usuarios, "id", "email");
            return View();
        }

        //
        // POST: /Empresas/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Empresas empresas)
        {
            if (ModelState.IsValid)
            {
                empresas.fecha_registro = DateTime.Now;
                empresas.id_usuario = db.Usuarios.FirstOrDefault(us => us.email == User.Identity.Name).id;
                db.Empresas.Add(empresas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_usuario = new SelectList(db.Usuarios, "id", "email", empresas.id_usuario);
            return View(empresas);
        }

        //
        // GET: /Empresas/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Empresas empresas = db.Empresas.Find(id);
            if (empresas == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_usuario = new SelectList(db.Usuarios, "id", "email", empresas.id_usuario);
            return View(empresas);
        }

        //
        // POST: /Empresas/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Empresas empresas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(empresas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_usuario = new SelectList(db.Usuarios, "id", "email", empresas.id_usuario);
            return View(empresas);
        }

        //
        // GET: /Empresas/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Empresas empresas = db.Empresas.Find(id);
            if (empresas == null)
            {
                return HttpNotFound();
            }
            return View(empresas);
        }

        //
        // POST: /Empresas/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Empresas empresas = db.Empresas.Find(id);
            db.Empresas.Remove(empresas);
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