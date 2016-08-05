using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TNT.Models
{
    public class EventosHomepage
    {
        public List<Eventos> EventosConciertos { get; set; }
        public List<Eventos> EventosArticulos { get; set; }
        public List<Eventos> EventosDeporte { get; set; }
        public List<Eventos> EventosViajes { get; set; }
        public List<Eventos> EventosCultura { get; set; }
    }
}