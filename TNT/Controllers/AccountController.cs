using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TNT.Models;

namespace TNT.Controllers
{
    public class AccountController : Controller
    {
        private TNTEntities db = new TNTEntities();

        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        public JsonResult LoginJSON(string email, string password)
        {
            using (TNTEntities entities = new TNTEntities())
            {
                string username = email;

                // Now if our password was enctypted or hashed we would have done the
                // same operation on the user entered password here, But for now
                // since the password is in plain text lets just authenticate directly

                bool userValid = entities.Usuarios.Any(user => user.email == username && user.password == password);

                // User found in the database
                if (userValid)
                {
                    //obtenemos su id
                    Usuarios usuario = entities.Usuarios.FirstOrDefault(user => user.email == username);
                    Personas persona = entities.Personas.FirstOrDefault(per => per.id_usuario == usuario.id);
                    Session.Add("persona_id", persona.id);
                    Session.Add("id", usuario.id);
                    FormsAuthentication.SetAuthCookie(username, false);
                    RedirectToAction("Index", "Home");
                    return Json(true);

                }
                else
                {
                    return Json(false);
                }
            }
        }
        [HttpPost]
        public ActionResult Login(Usuarios model, string returnUrl)
        {
            // Lets first check if the Model is valid or not
            if (ModelState.IsValid)
            {

                using (TNTEntities entities = new TNTEntities())
                {
                    string username = model.email;
                    string password = model.password;

                    // Now if our password was enctypted or hashed we would have done the
                    // same operation on the user entered password here, But for now
                    // since the password is in plain text lets just authenticate directly

                    bool userValid = entities.Usuarios.Any(user => user.email == username && user.password == password);

                    // User found in the database
                    if (userValid)
                    {
                        //obtenemos su id
                        Usuarios usuario = entities.Usuarios.FirstOrDefault(user => user.email == username);
                        
                        if(usuario.rol == "usuario")
                        {
                            Personas persona = entities.Personas.FirstOrDefault(per => per.id_usuario == usuario.id);
                            Session.Add("persona_id", persona.id);
                            Session.Add("full_name", persona.nombre_completo);
                        }else if(usuario.rol == "empresa")
                        {
                            Empresas empresa = entities.Empresas.FirstOrDefault(e => e.id_usuario == usuario.id);
                            Session.Add("persona_id", empresa.id);
                            Session.Add("full_name", empresa.nombre_empresa);
                        }
                        
                        Session.Add("id", usuario.id);
                        
                        FormsAuthentication.SetAuthCookie(username, false);

                        if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                            && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            TempData["IsWelcome"] = true;
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "El usuario o contraseña son invalidos.");
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Nuevo_Usuario model)
        {
            if (ModelState.IsValid)
            {
                // Intento de registrar al usuario
                try
                {
                    Usuarios usuario = new Usuarios();
                    usuario.email = model.email;
                    usuario.password = model.password;
                    usuario.rol = "usuario";
                    db.Usuarios.Add(usuario);
                    db.SaveChanges();
                    Personas persona = new Personas();
                    persona.apellidos = model.apellidos;
                    persona.cedula_identidad = model.carnet_identidad;
                    persona.direccion = model.direccion;
                    persona.id_usuario = usuario.id;
                    persona.nombre = model.nombre;
                    persona.numero_celular = model.numero_celular;
                    persona.fecha_modificacion = DateTime.Now;
                    persona.fecha_registro = DateTime.Now;
                    //persona.fecha_nacimiento = DateTime.Now;                     
                    db.Personas.Add(persona);
                    db.SaveChanges();
                    //return RedirectToAction("Index", "Home");
                    return this.Login(new Usuarios(){
                        email = model.email,
                        password = model.password
                    }, Url.Action("Index", "Home"));
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e.Message);
                }
            }

            // Si llegamos a este punto, es que se ha producido un error y volvemos a mostrar el formulario
            return View(model);
        }
        public ActionResult LogOff()
        {
            Session.RemoveAll();
            Session.Abandon();
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }
        public ActionResult RegistrarEmpresa()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult RegistrarEmpresa(Empresa_registro model)
        {
            if (ModelState.IsValid)
            {
                // Intento de registrar al usuario
                try
                {
                    Usuarios usuario = new Usuarios();
                    usuario.email = model.email;
                    usuario.password = model.password;
                    usuario.rol = "empresa";
                    db.Usuarios.Add(usuario);
                    db.SaveChanges();
                    Empresas empresa = new Empresas();
                    empresa.departamento = model.departamento;
                    empresa.direccion = model.direccion;
                    empresa.dosificacion_actividad_comercial = model.dosificacion_actividad_comercial;
                    empresa.dosificacion_codigo_autorizacion = model.dosificacion_codigo_autorizacion;
                    empresa.fecha_registro = DateTime.Now;
                    empresa.id_usuario = usuario.id;
                    empresa.nit = model.nit;
                    empresa.nombre_empresa = model.nombre_empresa;
                    empresa.representante_legal = model.representante_legal;
                    empresa.telefono = model.telefono;
                    db.Empresas.Add(empresa);
                    db.SaveChanges();
                    return RedirectToAction("IndexEmpresas", "Home");
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e.Message);
                }
            }

            // Si llegamos a este punto, es que se ha producido un error y volvemos a mostrar el formulario
            return View(model);
        }
        public ActionResult ReiniciarPassword()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ReiniciarPassword(ReinicioPassword model)
        {
            //generar token 
            string token = helpers.Helpers.Generar_token(32, new Random());
            //guardar token con el email enviado
            Usuarios usuario = db.Usuarios.Where(us => us.email == model.email).First();
            if (usuario != null)
            {
                usuario.reiniciar_contraseña = true;
                usuario.token_reinicio = token;
                db.Entry(usuario).State = System.Data.EntityState.Modified;
                db.SaveChanges();
                UrlHelper u = new UrlHelper(this.ControllerContext.RequestContext);
                string url = u.Action("RecuperarPassword", "Account", new { token = token }, Request.Url.Scheme);
                string contenido_email = String.Format("Por favor ingrese al siguiente link para reiniciar su contraseña en TNT.:<a href=\"{0}\">{0}</a>", url);
                helpers.Helpers.EnviarMail("admin@tnt.com", "administrador", model.email, model.email, "reinicio de password", contenido_email, null);
            }
            ModelState.AddModelError("email", "correo enviado a: " + model.email);
            return View(model);
        }
        public ActionResult RecuperarPassword(string token)
        {
            ViewBag.token = token;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult RecuperarPassword(string token, NuevoPassword model)
        {

            Usuarios usuario = db.Usuarios.FirstOrDefault(us => us.token_reinicio == model.token);
            if (usuario != null)
            {
                usuario.reiniciar_contraseña = false;
                usuario.token_reinicio = "";
                usuario.password = model.password;
                db.Entry(usuario).State = System.Data.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Login", "Account");
            }
            ModelState.AddModelError("password", "error al reiniciar password, por favor intente de nuevo");
            return View(model);
        }
    }
}
