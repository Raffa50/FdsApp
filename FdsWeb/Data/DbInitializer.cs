using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fds.Models;
using FdsWeb.Models;

namespace FdsWeb.Data {
    public static class DbInitializer {
        public static void EnsureSeedData( this ApplicationDbContext ctx ) {
            if( ctx.EventTypes.Any() )
                return;

            ctx.EventTypes.AddRange( new EventType<ApplicationUser>() {Type = "Conferenza"}, new EventType<ApplicationUser>() {Type = "Mostra"},
                new EventType<ApplicationUser>() {Type = "Laboratorio"}, new EventType<ApplicationUser>() {Type = "Spettacolo"},
                new EventType<ApplicationUser>() {Type = "Speciale"} );
            ctx.SaveChanges();
        }
    }
}