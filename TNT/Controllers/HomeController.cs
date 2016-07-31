using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace TNT.Controllers
{
    public class HomeController : Controller
    {
       
        
        public ActionResult Index()
        {
            TNT.Models.TNTEntities entities = new Models.TNTEntities();
            if (Request.IsAuthenticated)
            {
                
                int user_id;
                try
                {
                    user_id = (int)Session["id"];
                }
                catch (Exception)
                {
                    if (Request.IsAuthenticated)
                    {
                        Request.Cookies.Remove(User.Identity.Name);
                    }
                    System.Web.Security.FormsAuthentication.SignOut();
                    return RedirectToAction("Login", "Account");
                    throw;
                }
                if (User.IsInRole("empresa"))
                {
                    TNT.Models.Empresas empresa = entities.Empresas.FirstOrDefault(empresas => empresas.id_usuario == user_id);
                    if (Session["empresa_id"] == null)
                    {
                        Session.Add("empresa_id", empresa.id);
                    }
                    ViewBag.NombreEmpresa = empresa.nombre_empresa;
                    return View("EmpresasIndex");
                }
                if (User.IsInRole("usuario"))
                {
                    var eventos = entities.Eventos.Where(ev => ev.sectores.Count > 0 && ev.habilitado == true && ev.fecha_evento >= DateTime.Now);
                    //return View("UserIndex", eventos.ToList());
                    return View("Index", eventos.ToList());


                }
                if (User.IsInRole("admin"))
                {
                    return View("AdminIndex");
                }
                ViewBag.Message = "This can be viewed only by authenticated users only";
                return View();
            }
            else
            {
                //cargar eventos para la vista publica
                var eventos = entities.Eventos.Where(ev => ev.sectores.Count > 0 && ev.habilitado == true && ev.fecha_evento >= DateTime.Now);
                return View("index",eventos.ToList());
            }
            
        }

        [Authorize(Roles = "admin")]
        public ActionResult AdminIndex()
        {
            ViewBag.Message = "This can be viewed only by users in Admin role only";
            return View();
        }
        [Authorize(Roles = "usuarios")]
        public ActionResult UserIndex()
        {
            ViewBag.Message = "This can be viewed only by users in Admin role only";
            return View();
        }

        public ActionResult FileUpload(HttpPostedFileBase file)
        {
            if (file != null)
            {
                string pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(
                                       Server.MapPath("~/images/profile"), pic);
                // file is uploaded
                file.SaveAs(path);

                // save the image path path to the database or you can send image
                // directly to database
                // in-case if you want to store byte[] ie. for DB
                using (MemoryStream ms = new MemoryStream())
                {
                    file.InputStream.CopyTo(ms);
                    byte[] array = ms.GetBuffer();
                }

            }
            // after successfully uploading redirect the user
            return View();
        }
        [HttpPost]
        public JsonResult Upload()
        {
            string pathResult = "";
            for (int i = 0; i < Request.Files.Count; i++)
            {
                var file = Request.Files[i];

                WebImage image = new WebImage(file.InputStream);
                image.Resize(800, 300);
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/Upload/"), fileName);
                //file.SaveAs(path);
                image.Save(path);
                string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
                pathResult = baseUrl + "Upload/" + fileName;
                //pathResult = Url.Content("/Upload") + fileName;
            }
            
            return Json(pathResult);

        }
    }
}
