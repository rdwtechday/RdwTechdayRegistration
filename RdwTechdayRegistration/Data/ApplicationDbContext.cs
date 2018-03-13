using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RdwTechdayRegistration.Models;

namespace RdwTechdayRegistration.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
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
                .ToTable("Sessies")
                .HasOne(s => s.Ruimte)
                .WithMany(r => r.Sessies)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Sessie>()
                .HasOne(s => s.Track)
                .WithMany(r => r.Sessies)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Sessie>()
                .HasMany(s => s.ApplicationUserTijdvakken)
                .WithOne(r => r.Sessie)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Ruimte>()
                .ToTable("Ruimtes");

            builder.Entity<Tijdvak>()
                .ToTable("Tijdvakken");

            builder.Entity<Maxima>()
                .ToTable("Maxima");

            builder.Entity<ApplicationUserTijdvak>()
                .ToTable("ApplicationUserTijdvakken")
                .HasKey(c => new { c.ApplicationUserId, c.TijdvakId });

            builder.Entity<ApplicationUserTijdvak>()
                .HasOne(bc => bc.ApplicationUser)
                .WithMany(bc => bc.ApplicationUserTijdvakken)
                .HasForeignKey(bc => bc.ApplicationUserId);

            builder.Entity<ApplicationUserTijdvak>()
                .HasOne(bc => bc.Tijdvak)
                .WithMany(c => c.ApplicationUserTijdvakken)
                .HasForeignKey(bc => bc.TijdvakId);

            builder.Entity<SessieTijdvak>()
                .ToTable("SessieTijdvakken")
                .HasKey(c => new { c.SessieId, c.TijdvakId });

            builder.Entity<SessieTijdvak>()
                .HasOne(bc => bc.Sessie)
                .WithMany(c => c.SessieTijdvakken)
                .HasForeignKey(bc => bc.SessieId);

            builder.Entity<SessieTijdvak>()
                .HasOne(bc => bc.Tijdvak)
                .WithMany(c => c.SessieTijdvakken)
                .HasForeignKey(bc => bc.TijdvakId);

            builder.Entity<TrackTijdvak>()
                .ToTable("TrackTijdvakken");

            builder.Entity<TrackTijdvak>()
                .HasKey(c => new { c.TrackID, c.TijdvakID });
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ApplicationUserTijdvak> ApplicationUserTijdvakken { get; set; }
        public DbSet<Sessie> Sessies { get; set; }
        public DbSet<Ruimte> Ruimtes { get; set; }
        public DbSet<Tijdvak> Tijdvakken { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<Maxima> Maxima { get; set; }
        public DbSet<SessieTijdvak> SessieTijdvakken { get; set; }
        public DbSet<IdentityRole> IdentityRoles { get; set; }
    }
}