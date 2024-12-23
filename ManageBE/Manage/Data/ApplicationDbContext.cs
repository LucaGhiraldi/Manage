using Manage.Models;
using Manage.Models.NetWorth;
using Manage.Models.NetWorth.Base;
using Manage.Models.NetWorth.Enum;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Runtime.ConstrainedExecution;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Manage.Data
{
    public class ApplicationDbContext : IdentityDbContext<Utente>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Utente> Utenti { get; set; }
        public DbSet<Documenti> Documenti { get; set; }
        public DbSet<FileDocumenti> FileDocumenti { get; set; }
        public DbSet<CategoriaDocumenti> CategoriaDocumenti { get; set; }
        public DbSet<SottoCategoriaDocumenti> SottoCategoriaDocumenti { get; set; }

        public DbSet<InvestimentoBase> Investimenti { get; set; }
        public DbSet<Transazione> Transazioni { get; set; }
        // Per le classi di supporto
        public DbSet<TassoContoDeposito> TassiContoDeposito { get; set; }
        public DbSet<RendimentoBuoniFruttiferi> RendimentiBuoniFruttiferi { get; set; }
        public DbSet<CedolaTitoloDiStato> CedoleTitoloDiStato { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Azione>();
            modelBuilder.Entity<BuoniFruttiferiPostali>();
            modelBuilder.Entity<ContoDeposito>();
            modelBuilder.Entity<Cryptovaluta>();
            modelBuilder.Entity<FondoPensione>();
            modelBuilder.Entity<Immobile>();
            modelBuilder.Entity<Obbligazione>();
            modelBuilder.Entity<TitoloDiStato>();

            base.OnModelCreating(modelBuilder);

            // Relazione uno-a-molti tra Utente e Documenti
            modelBuilder.Entity<Utente>()
                .HasMany(u => u.Documenti)
                .WithOne(d => d.Utente)
                .HasForeignKey(d => d.UtenteId)
                .IsRequired(false);  // Non obbligatorio perché alcune categorie sono predefinite

            // Relazione uno-a-molti tra Utente e CategoriaDocumenti
            modelBuilder.Entity<Utente>()
                .HasMany(u => u.CategorieDocumenti)
                .WithOne(c => c.Utente)
                .HasForeignKey(c => c.UtenteId);

            // Relazione uno-a-molti tra CategoriaDocumenti e Documenti
            modelBuilder.Entity<CategoriaDocumenti>()
                .HasMany(c => c.Documenti)
                .WithOne(d => d.Categoria)
                .HasForeignKey(d => d.CategoriaDocumentiId);

            // Sotto categorie
            modelBuilder.Entity<SottoCategoriaDocumenti>()
                .HasOne(s => s.CategoriaDocumenti)
                .WithMany(c => c.SottoCategorie)
                .HasForeignKey(s => s.CategoriaDocumentiId)
                .OnDelete(DeleteBehavior.Cascade); // Cancellazione a cascata

            modelBuilder.Entity<Documenti>()
                .HasOne(d => d.SottoCategoria)
                .WithMany(s => s.Documenti)
                .HasForeignKey(d => d.SottoCategoriaDocumentiId);

            // Relazione uno-a-molti tra Documenti e FileDocumenti
            modelBuilder.Entity<Documenti>()
                .HasMany(d => d.FileDocumenti)
                .WithOne(f => f.Documento)
                .HasForeignKey(f => f.DocumentiId);

            // Dati predefiniti per CategoriaDocumenti
            modelBuilder.Entity<CategoriaDocumenti>().HasData(
                new CategoriaDocumenti
                {
                    Id = 1,
                    NomeCategoria = "Bollette",
                    DescrizioneCategoria = "Documenti relativi alle bollette",
                    DataInserimentoCategoria = DateTime.Now,
                    IsPredefinita = true,
                    UtenteId = null
                },
                new CategoriaDocumenti
                {
                    Id = 2,
                    NomeCategoria = "Fatture",
                    DescrizioneCategoria = "Documenti relativi alle fatture",
                    DataInserimentoCategoria = DateTime.Now,
                    IsPredefinita = true,
                    UtenteId = null
                }
            );

            modelBuilder.Entity<SottoCategoriaDocumenti>().HasData(
                new SottoCategoriaDocumenti
                {
                    Id = 1,
                    NomeSottoCategoria = "Bollette Internet",
                    DescrizioneSottoCategoria = "Bollette internet",
                    CategoriaDocumentiId = 1,
                    DataInserimentoSottoCategoria = DateTime.Now
                },
                new SottoCategoriaDocumenti
                {
                    Id = 2,
                    NomeSottoCategoria = "Bollette Energia",
                    DescrizioneSottoCategoria = "Bollette energia elettrica",
                    CategoriaDocumentiId = 1,
                    DataInserimentoSottoCategoria = DateTime.Now
                },
                new SottoCategoriaDocumenti
                {
                    Id = 3,
                    NomeSottoCategoria = "Fatture Internet",
                    DescrizioneSottoCategoria = "Fatture telefoniche",
                    CategoriaDocumentiId = 2,
                    DataInserimentoSottoCategoria = DateTime.Now
                },
                new SottoCategoriaDocumenti
                {
                    Id = 4,
                    NomeSottoCategoria = "Fatture Energia",
                    DescrizioneSottoCategoria = "Fatture energia elettrica",
                    CategoriaDocumentiId = 2,
                    DataInserimentoSottoCategoria = DateTime.Now
                }
            );

            // Relazione uno-a-molti tra Utente e Investment
            modelBuilder.Entity<Utente>()
                .HasMany(u => u.Investimenti)
                .WithOne(i => i.Utente)
                .HasForeignKey(i => i.UtenteId);

            // Configurazione della relazione Investimento -> Transazioni
            modelBuilder.Entity<Transazione>()
                .HasOne(t => t.Investimento)
                .WithMany(i => i.Transazioni)
                .HasForeignKey(t => t.InvestimentoId);

            // Configura la gerarchia e usa un discriminatore shadow
            modelBuilder.Entity<InvestimentoBase>()
                .HasDiscriminator<string>("TipoInvestimentoShadow") // Nome della colonna shadow
                .HasValue<TitoloDiStato>("TitoliDiStato")
                .HasValue<ContoDeposito>("ContoDeposito")
                .HasValue<BuoniFruttiferiPostali>("BuoniFruttiferiPostali")
                .HasValue<FondoPensione>("FondoPensione")
                .HasValue<Azione>("Azioni")
                .HasValue<Obbligazione>("Obbligazioni")
                .HasValue<Cryptovaluta>("Cryptovalute");

            // Configura la conversione solo per la proprietà `TipoInvestimentoEnum`
            modelBuilder.Entity<InvestimentoBase>()
                .Property(i => i.TipoInvestimento)
                .HasConversion(
                    v => v.ToString(), // Enum -> string
                    v => (TipoInvestimentoEnum)Enum.Parse(typeof(TipoInvestimentoEnum), v) // String -> Enum
                ).HasColumnName("TipoInvestimento"); // Colonna mappata

            modelBuilder.Entity<ContoDeposito>()
                .HasMany(cd => cd.Tassi)
                .WithOne(tc => tc.ContoDeposito)
                .HasForeignKey(tc => tc.ContoDepositoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BuoniFruttiferiPostali>()
                .HasMany(bfp => bfp.Rendimenti)
                .WithOne(rb => rb.BuoniFruttiferiPostali)
                .HasForeignKey(rb => rb.BuoniFruttiferiPostaliId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TitoloDiStato>()
                .HasMany(ts => ts.Cedole)
                .WithOne(c => c.TitoloDiStato)
                .HasForeignKey(c => c.TitoloDiStatoId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
