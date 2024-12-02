using System.Collections.Generic;
using System;
using API.Eventos.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Eventos.Persistence
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
         : base(options)
        {
        }

        // Construtor sem parâmetros, necessário para o design-time
        public ApplicationDbContext() { }

        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Participante> Participantes { get; set; }
        public DbSet<Inscricao> Inscricoes { get; set; }
        public DbSet<Organizador> Organizadores { get; set; }
        public DbSet<Local> Locais { get; set; }
        public DbSet<Patrocinador> Patrocinadores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurações para muitos-para-muitos
            modelBuilder.Entity<Evento>()
        .HasMany(e => e.Patrocinadores)
        .WithMany(p => p.Eventos)
        .UsingEntity<Dictionary<string, object>>(
            "EventoPatrocinador",
            e => e.HasOne<Patrocinador>().WithMany().HasForeignKey("PatrocinadorId"),
            p => p.HasOne<Evento>().WithMany().HasForeignKey("EventoId"));

        }

    }
}
