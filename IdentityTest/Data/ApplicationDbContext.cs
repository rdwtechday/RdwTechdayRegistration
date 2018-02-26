using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using IdentityTest.Models;
using Microsoft.AspNetCore.Identity;
using RdwTechdayRegistration.Models;

namespace IdentityTest.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser,IdentityRole,string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<Sessie>()
                .ToTable("Sessies");

            builder.Entity<RegistratieVerzoek>()
                .ToTable("RegistratieVerzoeken")
                .HasAlternateKey(c => c.Email);

            builder.Entity<Tijdvak>()
                .ToTable("Tijdvakken");

            builder.Entity<Ruimte>()
                .ToTable("Ruimtes");

            builder.Entity<Maxima>()
                .ToTable("Maxima");

            builder.Entity<TrackTijdvak>()
                .ToTable("TrackTijdvakken");

            builder.Entity<DeelnemerSessies>()
               .HasKey(bc => new { bc.DeelnemerId, bc.SessieId });

            builder.Entity<DeelnemerSessies>()
                .HasOne(bc => bc.Deelnemer)
                .WithMany(b => b.DeelnemerSessies)
                .HasForeignKey(bc => bc.DeelnemerId);

            builder.Entity<DeelnemerSessies>()
                .HasOne(bc => bc.Sessie)
                .WithMany(c => c.DeelnemerSessies)
                .HasForeignKey(bc => bc.SessieId);

            builder.Entity<TrackTijdvak>()
                .HasKey(c => new { c.TrackID, c.TijdvakID });
        }

        public DbSet<Deelnemer> Deelnemers { get; set; }
        public DbSet<Sessie> Sessies { get; set; }
        public DbSet<RegistratieVerzoek> RegistratieVerzoeken { get; set; }
        public DbSet<Ruimte> Ruimtes { get; set; }
        public DbSet<Tijdvak> Tijdvakken { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<TrackTijdvak> TrackTijdvakken { get; set; }
        public DbSet<Maxima> Maxima { get; set; }

        public DbSet<IdentityRole> IdentityRoles { get; set; }
    }
}
