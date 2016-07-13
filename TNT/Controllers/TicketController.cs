using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TNT.helpers;
using TNT.Models;
using TNT.WebSintesis_externo;

namespace TNT.Controllers
{
    public class TicketController : Controller
    {
        private TNTEntities db = new TNTEntities();

        //
        // GET: /Ticket/

        public ActionResult Index()
        {
            var ticket = db.Ticket.Include(t => t.Eventos).Include(t => t.sectores);
            if (User.IsInRole("empresa"))
            {
                int empresa_id = (int)Session["empresa_id"];
                ticket=ticket.Where(tickets => tickets.Eventos.id_empresa == empresa_id);
            }
            return View(ticket.ToList());
        }
        public ActionResult TicketsEventoReservados(int id)
        {
            var ticket = db.Ticket.Include(t => t.Eventos).Include(t => t.sectores).Where(t=>t.valida==0 && t.id_evento==id);
            if (User.IsInRole("empresa"))
            {
                int empresa_id = (int)Session["empresa_id"];
                ticket = ticket.Where(tickets => tickets.Eventos.id_empresa == empresa_id);
            }
            ViewBag.nombre_evento = db.Eventos.Find(id).nombre_evento; 
            return View(ticket.ToList());
        }
        
        public ActionResult TicketsEventoComprados(int id)
        {
            var ticket = db.Ticket.Include(t => t.Eventos).Include(t => t.sectores).Where(t => t.valida == 1 && t.id_evento == id);
            if (User.IsInRole("empresa"))
            {
                int empresa_id = (int)Session["empresa_id"];
                ticket = ticket.Where(tickets => tickets.Eventos.id_empresa == empresa_id);
            }
            ViewBag.nombre_evento = db.Eventos.Find(id).nombre_evento; 
            return View(ticket.ToList());
        }
        //
        // GET: /Ticket/Details/5

        public ActionResult Details(string id = null)
        {
            Ticket ticket = db.Ticket.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }
        public ActionResult BuscarTicket()
        {
            return View();
        }
        [HttpPost]
        public JsonResult VerificadorTicket(string id_ticket,string imei)
        {
            bool valido=false;
            var imei_valido = db.imei_validos.Find(imei);
            if (imei == null)
            {
                return Json(valido);
            }

            var ticket = db.Ticket.FirstOrDefault(tck=>tck.codigo==id_ticket && tck.valida == 1 && tck.utilizada == false);
            //var ticket = db.Ticket.FirstOrDefault(tk => tk.codigo == id_ticket);
            if (ticket != null)
            {
                valido = true;
                ticket.utilizada = true;
                ticket.fecha_uso = DateTime.Now;
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();

            }
            return Json(valido);
        }
        public class respuesta_ticket
        {
            public string nombre_cliente { get; set; }
            public string sector { get; set; }
            public string cedula_identidad { get; set; }
            public string fecha_hora_reserva { get; set; }
            public string fecha_hora_pago { get; set; }
            public string codigo_recaudacion { get; set; }
        }
        public JsonResult ObtieneTicket(string id_ticket)
        {
            Ticket ticket = db.Ticket.FirstOrDefault(tk=>tk.codigo==id_ticket);
            respuesta_ticket respuesta= new respuesta_ticket();
            Compra compra = db.Compra.FirstOrDefault(cr=>cr.codigo_recaudacion==ticket.codigo_recaudacion);
            Personas usuarios = compra.Usuarios.Personas.First();
            respuesta.nombre_cliente = usuarios.nombre + " " + usuarios.apellidos;
            respuesta.cedula_identidad = usuarios.cedula_identidad;
            respuesta.codigo_recaudacion = compra.codigo_recaudacion;
            respuesta.fecha_hora_reserva = compra.fecha_compra.Value.ToString("yyyy-MM-dd hh:mm:ss");
            respuesta.fecha_hora_pago = compra.fecha_pago.Value.ToString("yyyy-MM-dd hh:mm:ss");
            respuesta.sector = ticket.sectores.descripcion;
            return Json(respuesta);
        }
        //
        // GET: /Ticket/Create
        public ActionResult ComprarTicketsUsuario(int id)
        {
            var eventos = db.Eventos.Where(ev=>ev.id == id).Include(e => e.Empresas).Include(e => e.Lugares).Include(e => e.Tipos_evento);
            eventos.Where(ev => ev.fecha_evento >= DateTime.Now);
            ViewBag.id_evento = new SelectList(eventos, "id", "nombre_evento");
            var sectores = db.sectores.Where(sec => sec.id_evento == id);
            ViewBag.sectores = sectores;
            ViewBag.id_sector = new SelectList(sectores, "id", "descripcion");
            ViewBag.nombre_evento = eventos.First().nombre_evento;
            ViewBag.fecha_evento = eventos.First().fecha_evento;
            ViewBag.hora_evento = eventos.First().hora_evento;
            ViewBag.descripcion = eventos.First().descripcion;
            ViewBag.nombre_lugar = eventos.First().Lugares.nombre_lugar;
            ViewBag.img_url = eventos.First().img_url;

            return View();
        }
        public void CompraMultiplesTicketMismoSector(Ticket ticket)
        {
            string[] butacas = ticket.butaca.Split(',');
            string inicial = User.Identity.Name.Substring(1, 1);
            string codigo_recaudacion = Helpers.Generar_codigo_recaudacion(inicial); //no podemos repetir el codigo de recaudacion (no debe existir en la tabla compra)
            decimal costo_total = 0m;
            decimal total_comision = 0m;
            Eventos_app datos_evento = DA_clases.DA_Eventos.obtener_evento(ticket.id_evento);
            Empresas_app datos_empresa = DA_clases.DA_Empresas.obtener_empresa(datos_evento.id_empresa);
            Usuarios datos_usuario = db.Usuarios.FirstOrDefault(us=>us.email == User.Identity.Name);
            sectores sector = db.sectores.Find(ticket.id_sector);
            decimal costo_total_sector = DA_clases.DA_Tickets.obtiene_costo_sector(ticket.id_evento, ticket.id_sector.Value,butacas.Length);
            var comisiones = db.comisiones.Where(com => com.id_empresa == datos_empresa.id_empresa);
            if (comisiones.Count() == 0)
            {
                comisiones = db.comisiones.Where(com => com.id_empresa == 1);
            }

            var comision = comisiones.FirstOrDefault(com => com.rango_inferior >= costo_total_sector);
            if (comision == null)//si no se encuentra comision inferior quiere decir que supera el limite inferior asi q tomamos el maximo
            {
                comision = comisiones.OrderByDescending(com => com.rango_superior).First();

            }
            decimal monto_comision = comision.monto_comision * butacas.Length; //Implementar   
            total_comision += monto_comision;
            try
            {

                foreach (string butaca in butacas)
                {
                    string codigo_ticket = Helpers.Generar_codigo_ticket(30, new Random());
                    Ticket ticket_nuevo = new Ticket();
                    ticket_nuevo.butaca = butaca;
                    ticket_nuevo.codigo = codigo_ticket;
                    ticket_nuevo.codigo_recaudacion = codigo_recaudacion;
                    ticket_nuevo.id_evento = ticket.id_evento;
                    ticket_nuevo.id_sector = ticket.id_sector;
                    ticket_nuevo.nit_usuario = datos_usuario.Personas.First().cedula_identidad;
                    ticket_nuevo.nombre_usuario = User.Identity.Name;
                    ticket_nuevo.utilizada = false;
                    ticket_nuevo.valida = 0;
                    ticket_nuevo.costo_sin_comision = costo_total_sector / butacas.Length;
                    ticket_nuevo.fecha_modificacion = DateTime.Now;
                    db.Ticket.Add(ticket_nuevo);
                    db.SaveChanges();
                }
                costo_total += costo_total_sector;
                sector.asientos_disponibles = sector.asientos_disponibles - butacas.Length;
                db.Entry(sector).State = System.Data.EntityState.Modified;
                db.SaveChanges();
                bool envio = Helpers.Envio_sintesis_compra_simple(
                          codigo_recaudacion, User.Identity.Name, User.Identity.Name, datos_usuario.Personas.First().cedula_identidad, datos_evento.nombre_evento,
                          datos_empresa.nombre_empresa, datos_empresa.nit_empresa, (double)costo_total, (double)total_comision);
                if (!envio)
                {
                    //guardar en SINTESIS relegados
                    Models.EnviosRelegadosSINTESIS relegado = new EnviosRelegadosSINTESIS();
                    relegado.codigo_recaudacion = codigo_recaudacion;
                    relegado.costo_total = costo_total;
                    relegado.email_usuario = User.Identity.Name;
                    relegado.fecha_hora = DateTime.Now;
                    relegado.nit_empresa = datos_usuario.Personas.First().cedula_identidad;
                    relegado.nit_usuario = ticket.nit_usuario;
                    relegado.nombre_empresa = datos_empresa.nombre_empresa;
                    relegado.nombre_evento = datos_evento.nombre_evento;
                    relegado.nombre_usuario = User.Identity.Name;
                    relegado.pendiente = true;
                    relegado.total_comision = total_comision;
                    db.EnviosRelegadosSINTESIS.Add(relegado);
                    db.SaveChanges();
                    helpers.Helpers.EnviarMail("admin@tnt.com", "admin", "eisenob@gmail.com", "admin", "CODIGO RECAUDACION NO ENVIADO SINTESIS - PENDIENTE", "<h1>Codigo de recaudacion no enviado y pendiente de envio</h1><h2>" + codigo_recaudacion + "</h2>", null);

                }
                string email_usuario = User.Identity.Name;
                Usuarios usuario = db.Usuarios.FirstOrDefault(us => us.email == email_usuario);
                Compra compra_nueva = new Compra();
                compra_nueva.codigo_recaudacion = codigo_recaudacion;
                compra_nueva.fecha_compra = DateTime.Now;
                compra_nueva.id_usuario_compra = usuario.id;
                compra_nueva.monto_cobrar = costo_total + total_comision;
                compra_nueva.monto_comision = total_comision;
                compra_nueva.monto_parcial = costo_total;
                compra_nueva.pagado = 0;
                db.Compra.Add(compra_nueva);
                db.SaveChanges();
                //enviar correo a usuario con codigo de recaudacion
                string template = "<h2>Se efectuo una compra del evento:" + datos_evento.nombre_evento + "</h2>Este es el codigo de recaudacion:" + codigo_recaudacion + " con esto puede pasar a cancelar el monto de:" + (costo_total + total_comision) + " a el punto de pago mas cercano";
                Helpers.EnviarMail("admin@tnt.com", "admin", usuario.email, usuario.Personas.First().nombre, "codigo de recaudacion", template, null);
                ViewBag.codigo_recaudacion = codigo_recaudacion;
                ViewBag.monto_pagar = (costo_total + total_comision).ToString();
                ViewBag.message = "compra exitosa, por favor imprima el voucher haciendo clic en el icono de imprimir";

            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                List<string> errorMessages = new List<string>();
                foreach (System.Data.Entity.Validation.DbEntityValidationResult validationResult in ex.EntityValidationErrors)
                {
                    string entityName = validationResult.Entry.Entity.GetType().Name;
                    foreach (System.Data.Entity.Validation.DbValidationError error in validationResult.ValidationErrors)
                    {
                        errorMessages.Add(entityName + "." + error.PropertyName + ": " + error.ErrorMessage);
                    }
                }
                ViewBag.message = errorMessages.ToString();

            }
        }
        public void CompraIndividualTicket(Ticket ticket)
        {
            Eventos evento = db.Eventos.Find(ticket.id_evento);
            Empresas empresa = db.Empresas.Find(evento.id_empresa);
            sectores sector = db.sectores.Find(ticket.id_sector);
            Usuarios usuario_actual = db.Usuarios.FirstOrDefault(us => us.email == User.Identity.Name);
            Personas datos_usuario_actual = db.Personas.FirstOrDefault(per => per.id_usuario == usuario_actual.id);
            decimal costo_total = sector.precio_unitario * 1;
            var comisiones = db.comisiones.Where(com => com.id_empresa == empresa.id);
            if (comisiones.Count() == 0)
            {
                comisiones = db.comisiones.Where(com => com.id_empresa == 1);
            }

            var comision = comisiones.FirstOrDefault(com => com.rango_inferior >= costo_total);
            if (comision == null)//si no se encuentra comision inferior quiere decir que supera el limite inferior asi q tomamos el maximo
            {
                comision = comisiones.OrderByDescending(com => com.rango_superior).First();

            }
            Random random = new Random();
            string inicial = User.Identity.Name.Substring(1, 1);
            string codigo_recaudacion = Helpers.Generar_codigo_recaudacion(inicial); //no podemos repetir el codigo de recaudacion (no debe existir en la tabla compra)
            string codigo_ticket = Helpers.Generar_codigo_ticket(50, new Random());


            if (ModelState.IsValid)
            {
                bool envio = Helpers.Envio_sintesis_compra_simple_nuevo(
                 codigo_recaudacion, usuario_actual.email, datos_usuario_actual.nombre + " " + datos_usuario_actual.apellidos, datos_usuario_actual.cedula_identidad, evento.nombre_evento,
                 empresa.nombre_empresa, empresa.nit, (double)costo_total, (double)comision.monto_comision);
                if (envio)
                {
                    Compra compra = new Compra();
                    compra.codigo_recaudacion = codigo_recaudacion;
                    compra.fecha_compra = DateTime.Now;
                    compra.id_usuario_compra = (int)Session["id"];
                    compra.monto_cobrar = costo_total + comision.monto_comision;
                    compra.pagado = 0;
                    compra.monto_comision = comision.monto_comision;
                    compra.monto_parcial = costo_total;
                    db.Compra.Add(compra);
                    ticket.codigo_recaudacion = codigo_recaudacion;
                    ticket.utilizada = false;
                    ticket.valida = 0;
                    ticket.codigo = codigo_ticket;
                    ticket.nombre_usuario = datos_usuario_actual.nombre;
                    ticket.nit_usuario = datos_usuario_actual.cedula_identidad;
                    ticket.costo_sin_comision = costo_total;
                    ticket.fecha_modificacion = DateTime.Now;
                    db.Ticket.Add(ticket);
                    try
                    {
                        db.SaveChanges();
                        ViewBag.codigo_recaudacion = codigo_recaudacion;
                        ViewBag.monto_pagar = (costo_total + comision.monto_comision).ToString();
                        ViewBag.message = "compra exitosa, por favor imprima el voucher haciendo clic en el icono de imprimir";
                        string template = "<h2>Se efectuo una compra del evento:" + evento.nombre_evento + "</h2>Este es el codigo de recaudacion:" + codigo_recaudacion + " con esto puede pasar a cancelar el monto de:" + (costo_total + comision.monto_comision).ToString() + " al punto de pago mas cercano";
                        Helpers.EnviarMail("admin@tnt.com", "admin", usuario_actual.email, usuario_actual.Personas.First().nombre, "codigo de recaudacion", template, null);

                    }
                    catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                    {
                        List<string> errorMessages = new List<string>();
                        foreach (System.Data.Entity.Validation.DbEntityValidationResult validationResult in ex.EntityValidationErrors)
                        {
                            string entityName = validationResult.Entry.Entity.GetType().Name;
                            foreach (System.Data.Entity.Validation.DbValidationError error in validationResult.ValidationErrors)
                            {
                                errorMessages.Add(entityName + "." + error.PropertyName + ": " + error.ErrorMessage);
                            }
                        }
                        ViewBag.message = errorMessages.ToString();

                    }
                }
                else
                {
                    ViewBag.message = "Error en la compra, por favor reintente";
                }

            }
        }
        public bool AsientoDisponible(int id_sector, string butaca)
        {
            var tickets = db.Ticket.Where(tck => tck.id_sector == id_sector && tck.butaca == butaca);
            if (tickets.Count() > 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ComprarTicketsUsuario(Ticket ticket)
        {
            var eventos = db.Eventos.Where(ev => ev.id == ticket.id_evento).Include(e => e.Empresas).Include(e => e.Lugares).Include(e => e.Tipos_evento);
            eventos.Where(ev => ev.fecha_evento >= DateTime.Now);
            ViewBag.id_evento = new SelectList(eventos, "id", "nombre_evento");
            var sectores = db.sectores.Where(sec => sec.id_evento == ticket.id_evento);
            ViewBag.sectores = sectores;
            ViewBag.id_sector = new SelectList(sectores, "id", "descripcion");
            //validar si usuario ya hizo mas de 5 compras para este evento
            var tickets_comprados_usuario = db.Ticket.Where(tick => tick.nombre_usuario == User.Identity.Name);
            if (tickets_comprados_usuario.Count() > 5)
            {
                ModelState.AddModelError("id_evento", "Usted ya compro 5 tickets para este evento, no puede comprar más");
                return View(ticket);
            }
            if (ticket.butaca != null)
            {
                string[] butacas = ticket.butaca.Split(',');
                var asientosOcupados = db.Ticket.Where(tick => tick.id_sector == ticket.id_sector);
                foreach (var asiento in asientosOcupados)
                {
                    if(butacas.Contains(asiento.butaca)){
                        ViewBag.message = "Asientos ocupados,por favor elija otros.";
                        return View(ticket);
                    }
                }
            
                if (butacas.Length > 1)
                {
                    CompraMultiplesTicketMismoSector(ticket);
                }
                else
                {
                    CompraIndividualTicket(ticket);
                }
            }
            else
            {

                CompraIndividualTicket(ticket);
            }
           
           
            return View(ticket);
        }
        public ActionResult Create()
        {
            if (User.IsInRole("empresa"))
            {
                int empresa_id = (int)Session["empresa_id"];
                ViewBag.id_evento = new SelectList(db.Eventos.Where(ev=>ev.id_empresa==empresa_id), "id", "nombre_evento");
                ViewBag.id_sector = new SelectList(db.sectores.Where(sec=>sec.Eventos.id_empresa == empresa_id), "id", "descripcion");
            }
            else
            {
                ViewBag.id_evento = new SelectList(db.Eventos, "id", "nombre_evento");
                ViewBag.id_sector = new SelectList(db.sectores, "id", "descripcion");
            }
            
            return View();
        }

        //
        // POST: /Ticket/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Ticket ticket)
        {
            //compra de ticket 
            Eventos evento = db.Eventos.Find(ticket.id_evento);
            Empresas empresa = db.Empresas.Find(evento.id_empresa);
            sectores sector = db.sectores.Find(ticket.id_sector);
            decimal costo_total = sector.precio_unitario * 1;            
            Random random = new Random();
            string iniciales = "Q";
            string codigo_recaudacion = Helpers.Generar_codigo_recaudacion(iniciales); //no podemos repetir el codigo de recaudacion (no debe existir en la tabla compra)
            string codigo_ticket = Helpers.Generar_codigo_ticket(50, new Random());
            
            if (ModelState.IsValid)
            {
                ComelecSoapClient cli = new ComelecSoapClient();
                DatosPlanilla datosPlanilla = new DatosPlanilla();
                DPlanilla[] dPlanilla = new DPlanilla[2];
                RespPlanilla respPlanilla = new RespPlanilla();
                

                datosPlanilla.codigoCliente = "1";
                datosPlanilla.codigoEmpresa = 321;
                datosPlanilla.codigoProducto = "1";
                datosPlanilla.codigoRecaudacion = codigo_recaudacion;
                datosPlanilla.correoElectronico = "lquenta@gmail.com";
                datosPlanilla.descripcionRecaudacion = "prueba de compra1";
                datosPlanilla.fecha = 20151125;
                datosPlanilla.hora = 005900;
                datosPlanilla.fechaVencimiento = 0;
                datosPlanilla.horaVencimiento = 0;
                datosPlanilla.moneda = "BS";
                datosPlanilla.nombre = "YGDRASSIL";
                datosPlanilla.nit_CI_cliente = "4850644";
                datosPlanilla.transaccion = "A";

                dPlanilla[0] = new DPlanilla();
                dPlanilla[0].numeroPago = 1;
                dPlanilla[0].descripcion = evento.nombre_evento;
                dPlanilla[0].nombreFactura = empresa.nombre_empresa ;
                dPlanilla[0].nitFactura = empresa.nit;
                dPlanilla[0].montoPago =(double)costo_total;
                dPlanilla[0].montoCreditoFiscal =(double) costo_total;

                dPlanilla[1] = new DPlanilla();
                dPlanilla[1].numeroPago = 1;
                dPlanilla[1].descripcion = "Comision";
                dPlanilla[1].nombreFactura = "YGDRASSIL";
                dPlanilla[1].nitFactura = "111111119";
                dPlanilla[1].montoPago = 10; //TODO: estructurar comision
                dPlanilla[1].montoCreditoFiscal = 10;

                datosPlanilla.planillas = dPlanilla;
                respPlanilla = cli.cmeRegistroPlan(datosPlanilla, "wsiggdrasil");
                if (respPlanilla.codError == 0)
                {
                    Compra compra = new Compra();
                    compra.codigo_recaudacion = codigo_recaudacion;
                    compra.fecha_compra = DateTime.Now;
                    compra.id_usuario_compra = 1;
                    compra.monto_cobrar = costo_total;
                    compra.pagado = 0;
                    db.Compra.Add(compra);                
                    ticket.codigo_recaudacion = codigo_recaudacion;
                    ticket.utilizada = false;
                    ticket.valida = 0;
                    ticket.codigo = codigo_ticket;
                    ticket.nombre_usuario = "N/A";
                    ticket.nit_usuario = "0";
                    db.Ticket.Add(ticket);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (System.Data.Entity.Validation.DbEntityValidationException  ex)
                    {
                        List<string> errorMessages = new List<string>();
                        foreach (System.Data.Entity.Validation.DbEntityValidationResult validationResult in ex.EntityValidationErrors)
                        {
                            string entityName = validationResult.Entry.Entity.GetType().Name;
                            foreach (System.Data.Entity.Validation.DbValidationError error in validationResult.ValidationErrors)
                            {
                                errorMessages.Add(entityName + "." + error.PropertyName + ": " + error.ErrorMessage);
                            }
                        }
                        ViewBag.error_message = errorMessages.ToString();
                        
                    }
                    
                    ViewBag.success_message = "Compra exitosa";
                }
                else
                {
                    ViewBag.error_message = "Error en la compra, por favor reintente";
                }
               
               
                return RedirectToAction("Index");
            }

            ViewBag.id_evento = new SelectList(db.Eventos, "id", "nombre_evento", ticket.id_evento);
            ViewBag.id_sector = new SelectList(db.sectores, "id", "descripcion", ticket.id_sector);
            return View(ticket);
        }

        //
        // GET: /Ticket/Edit/5

        public ActionResult Edit(string id = null)
        {
            Ticket ticket = db.Ticket.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_evento = new SelectList(db.Eventos, "id", "nombre_evento", ticket.id_evento);
            ViewBag.id_sector = new SelectList(db.sectores, "id", "descripcion", ticket.id_sector);
            return View(ticket);
        }

        //
        // POST: /Ticket/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_evento = new SelectList(db.Eventos, "id", "nombre_evento", ticket.id_evento);
            ViewBag.id_sector = new SelectList(db.sectores, "id", "descripcion", ticket.id_sector);
            return View(ticket);
        }

        //
        // GET: /Ticket/Delete/5

        public ActionResult Delete(string id = null)
        {
            Ticket ticket = db.Ticket.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        //
        // POST: /Ticket/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Ticket ticket = db.Ticket.Find(id);
            db.Ticket.Remove(ticket);
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