using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FdsWeb.Models;

namespace FdsWeb.Data{
    public static class DbInitializer {
        public static void EnsureSeedData( this ApplicationDbContext ctx ) { //TODO delete
            /*if( ctx.Users.Any() )
                return;

            var user1 = new ApplicationUser() {
                Email = "pino@email.it",
                UserName = "Pino",
                PhoneNumber = "314",
                PasswordHash = "AQAAAAEAACcQAAAAEPYuzZQu65Ht8CCedYYRy7W1fI7YTkXyZuZ+Cs4bF9KMFP+0k+LO/VhKjxe4tdHv5A==" //ciaociao
            };

            ctx.Users.Add( user1 );
            ctx.SaveChanges();

            Console.WriteLine("Database seeded");*/
        }
    }
}
