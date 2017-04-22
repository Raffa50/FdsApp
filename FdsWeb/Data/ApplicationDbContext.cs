using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FdsWeb.Models;

namespace FdsWeb.Data{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> {
        public DbSet< Event > Events;
        public DbSet< Schedule > Schedules;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){
        }

        protected override void OnModelCreating(ModelBuilder builder){
            base.OnModelCreating(builder);

            builder.Entity< Event >( e => e.HasMany( o => o.Schedule ).WithOne( o => o.Event ) );

            builder.Entity< Schedule >( e => e.HasKey( o => new {o.EventId, o.DateTime} ) );
        }
    }
}
