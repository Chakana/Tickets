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
        public ActionResult Login()
        {
            return View();
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

                    bool userValid = entities.Usuarios.Any(user => user.email== username && user.password == password);

                    // User found in the database
                    if (userValid)
                    {
                        //obtenemos su id
                        Usuarios usuario = entities.Usuarios.FirstOrDefault(user => user.email == username);
                        Session.Add("id", usuario.id);
                        FormsAuthentication.SetAuthCookie(username, false);
                      
                        if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                            && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
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
                    Usuarios usuario=new Usuarios();
                    usuario.email = model.email;
                    usuario.password = model.password;
                    usuario.rol = "usuario";
                    db.Usuarios.Add(usuario);
                    db.SaveChanges();
                    Personas persona=new Personas();
                    persona.apellidos="";
                    persona.cedula_identidad=model.nit;
                    persona.direccion="";
                    persona.id_usuario=usuario.id;
                    persona.nombre="";
                    persona.numero_celular=model.numero_celular;
                    persona.fecha_modificacion = DateTime.Now;
                    persona.fecha_registro = DateTime.Now;
                    persona.fecha_nacimiento = null;                    
                    db.Personas.Add(persona);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");
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

    }
}
