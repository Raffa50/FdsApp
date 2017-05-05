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
        public DbSet< Event > Events { get; set; }
        public DbSet< Schedule > Schedules { get; set; }
        public DbSet< EventType > EventTypes { get; set; }
        public DbSet< UserJoinEvent > UserJoinEvents { get; set; }

        public ApplicationDbContext( DbContextOptions< ApplicationDbContext > options ) : base( options ) { }

        protected override void OnModelCreating( ModelBuilder builder ) {
            base.OnModelCreating( builder );

            builder.Entity< Event >( e => e.HasMany( o => o.Schedule ).WithOne( o => o.Event ) );
            builder.Entity< Event >( e => e.HasMany( o => o.UserJoined ).WithOne( o => o.Event )
                .OnDelete( DeleteBehavior.Restrict ) );

            builder.Entity< EventType >( e => e.HasMany( o => o.Events ).WithOne( o => o.EventType ) );

            builder.Entity< Schedule >( e => e.HasAlternateKey( o => new {o.EventId, o.DateTime} ) );
            builder.Entity< Schedule >( e => e.HasMany( o => o.UserJoined ).WithOne( o => o.Schedule )
                .OnDelete( DeleteBehavior.Restrict ) );

            builder.Entity< UserJoinEvent >( e => e.HasAlternateKey( o => new {o.EventId, o.ApplicationUserId} ) );

            builder.Entity< ApplicationUser >( e => e.HasMany( o => o.UserJoined ).WithOne( o => (ApplicationUser)o.ApplicationUser ).HasForeignKey( k => k.ApplicationUserId )
                .OnDelete( DeleteBehavior.Restrict ) );
            builder.Entity< ApplicationUser >( e => e.HasMany( o => o.CreatedEvents )
                .WithOne( o => (ApplicationUser) o.ApplicationUser ).HasForeignKey( k => k.ApplicationUserId ) );
        }

        public DbSet<FdsWeb.Models.ApplicationUser> ApplicationUser { get; set; }
    }
}