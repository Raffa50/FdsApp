using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FdsWeb.Models;

namespace FdsWeb.Data{
    public static class DbInitializer {
        public static void EnsureSeedData( this ApplicationDbContext ctx ) {
            if( ctx.EventTypes.Any() )
                return;

            ctx.EventTypes.AddRange( new EventType() {Type = "Conferenza"}, new EventType(){ Type = "Mostra" }, new EventType(){ Type = "Laboratorio" }, new EventType() { Type = "Spettacolo" }, new EventType(){ Type = "Speciale" } );
            ctx.SaveChanges();
        }
    }
}
