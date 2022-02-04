using GiveAwayApp.Areas.Identity.Data;
using GiveAwayApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GiveAwayApp.Data
{
    public class GiveAwayAppContext : IdentityDbContext<GiveAwayAppUser>
    {
        public GiveAwayAppContext(DbContextOptions<GiveAwayAppContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<GiveAwayAppUser>() // konfiguration af BrugereSpil mellemtabellen for at tilføje oprettelses dato.
                .HasMany(s => s.Spil)
                .WithMany(b => b.Brugere)
                .UsingEntity<BrugereSpil>(
                    bs => bs.HasOne(spilProp => spilProp.Spil)
                        .WithMany()
                        .HasForeignKey(bsProp => bsProp.SpilId),
                    bs => bs.HasOne(brugerProp => brugerProp.Bruger)
                        .WithMany()
                        .HasForeignKey(bsProp => bsProp.BrugerId),
                    bs =>
                    {
                        bs.HasKey(pKey => new { pKey.BrugerId, pKey.SpilId }); // laver sammensat primary key
                        bs.Property(bsProp => bsProp.OprettelsesDato)
                            .HasDefaultValueSql("GETUTCDATE()"); // laver oprettelsesdatokolonnen i BrugereSpil tabellen.
                    }
                );

            builder.Entity<Lodtrækning>()
                .HasOne(p => p.ValgteSpil)
                .WithOne()
                .HasForeignKey<Lodtrækning>(p => p.ValgteSpilId);
        }
        public virtual DbSet<Spil> Spil { get; set; }
        public virtual DbSet<GiveAwayAppUser> Brugere { get; set; }
        public virtual DbSet<BrugereSpil> BrugereSpil { get; set; }
        public virtual DbSet<Statistik> Statistik { get; set; }
        public virtual DbSet<Lodtrækning> Lodtrækning { get; set; }
    }
}
