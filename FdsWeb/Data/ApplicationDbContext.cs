using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fds.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FdsWeb.Models;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FdsWeb.Data {
    public class ApplicationDbContext : IdentityDbContext< ApplicationUser > {
        public DbSet< Event<ApplicationUser>> Events { get; set; }
        public DbSet< Schedule<ApplicationUser>> Schedules { get; set; }
        public DbSet< EventType<ApplicationUser>> EventTypes { get; set; }
        public DbSet< UserJoinEvent<ApplicationUser>> UserJoinEvents { get; set; }

        public ApplicationDbContext( DbContextOptions< ApplicationDbContext > options ) : base( options ) { }

        protected override void OnModelCreating( ModelBuilder builder ) {
            base.OnModelCreating( builder );

            builder.Entity< Event<ApplicationUser>>( e => e.HasMany( o => o.Schedule ).WithOne( o => o.Event ) );
            builder.Entity< Event<ApplicationUser>>( e => e.HasMany( o => o.UserJoined ).WithOne( o => o.Event )
                .OnDelete( DeleteBehavior.Restrict ) );

            builder.Entity< EventType<ApplicationUser>>( e => e.HasMany( o => o.Events ).WithOne( o => o.EventType ) );

            builder.Entity< Schedule<ApplicationUser>>( e => e.HasAlternateKey( o => new {o.EventId, o.DateTime} ) );
            builder.Entity< Schedule<ApplicationUser>>( e => e.HasMany( o => o.UserJoined ).WithOne( o => o.Schedule )
                .OnDelete( DeleteBehavior.Restrict ) );

            builder.Entity< UserJoinEvent<ApplicationUser>>( e => e.HasAlternateKey( o => new {o.EventId, o.ApplicationUserId} ) );

            builder.Entity< ApplicationUser >( e => e.HasMany( o => o.UserJoined ).WithOne( o => o.ApplicationUser )
                .OnDelete( DeleteBehavior.Restrict ) );
        }

        public DbSet<FdsWeb.Models.ApplicationUser> ApplicationUser { get; set; }
    }
}