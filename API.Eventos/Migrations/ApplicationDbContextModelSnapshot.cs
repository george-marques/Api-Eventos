﻿// <auto-generated />
using System;
using API.Eventos.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace API.Eventos.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("API.Eventos.Entities.Evento", b =>
                {
                    b.Property<int>("EventoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EventoId"));

                    b.Property<int>("Capacidade")
                        .HasColumnType("int");

                    b.Property<DateTime>("Data")
                        .HasColumnType("datetime2");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("LocalId")
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("OrganizadorId")
                        .HasColumnType("int");

                    b.HasKey("EventoId");

                    b.HasIndex("LocalId");

                    b.HasIndex("OrganizadorId");

                    b.ToTable("Eventos");
                });

            modelBuilder.Entity("API.Eventos.Entities.Inscricao", b =>
                {
                    b.Property<int>("InscricaoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("InscricaoId"));

                    b.Property<DateTime>("DataInscricao")
                        .HasColumnType("datetime2");

                    b.Property<int>("EventoId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("ParticipanteId")
                        .HasColumnType("int");

                    b.HasKey("InscricaoId");

                    b.HasIndex("ParticipanteId");

                    b.ToTable("Inscricoes");
                });

            modelBuilder.Entity("API.Eventos.Entities.Local", b =>
                {
                    b.Property<int>("LocalId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LocalId"));

                    b.Property<int>("CapacidadeMaxima")
                        .HasColumnType("int");

                    b.Property<string>("Endereco")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("LocalId");

                    b.ToTable("Locais");
                });

            modelBuilder.Entity("API.Eventos.Entities.Organizador", b =>
                {
                    b.Property<int>("OrganizadorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrganizadorId"));

                    b.Property<string>("Contato")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("OrganizadorId");

                    b.ToTable("Organizadores");
                });

            modelBuilder.Entity("API.Eventos.Entities.Participante", b =>
                {
                    b.Property<int>("ParticipanteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ParticipanteId"));

                    b.Property<string>("CPF")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("ParticipanteId");

                    b.ToTable("Participantes");
                });

            modelBuilder.Entity("API.Eventos.Entities.Patrocinador", b =>
                {
                    b.Property<int>("PatrocinadorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PatrocinadorId"));

                    b.Property<string>("Contato")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("PatrocinadorId");

                    b.ToTable("Patrocinadores");
                });

            modelBuilder.Entity("EventoPatrocinador", b =>
                {
                    b.Property<int>("EventoId")
                        .HasColumnType("int");

                    b.Property<int>("PatrocinadorId")
                        .HasColumnType("int");

                    b.HasKey("EventoId", "PatrocinadorId");

                    b.HasIndex("PatrocinadorId");

                    b.ToTable("EventoPatrocinador");
                });

            modelBuilder.Entity("API.Eventos.Entities.Evento", b =>
                {
                    b.HasOne("API.Eventos.Entities.Local", null)
                        .WithMany("Eventos")
                        .HasForeignKey("LocalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.Eventos.Entities.Organizador", null)
                        .WithMany("Eventos")
                        .HasForeignKey("OrganizadorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("API.Eventos.Entities.Inscricao", b =>
                {
                    b.HasOne("API.Eventos.Entities.Participante", null)
                        .WithMany("Inscricoes")
                        .HasForeignKey("ParticipanteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EventoPatrocinador", b =>
                {
                    b.HasOne("API.Eventos.Entities.Evento", null)
                        .WithMany()
                        .HasForeignKey("EventoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.Eventos.Entities.Patrocinador", null)
                        .WithMany()
                        .HasForeignKey("PatrocinadorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("API.Eventos.Entities.Local", b =>
                {
                    b.Navigation("Eventos");
                });

            modelBuilder.Entity("API.Eventos.Entities.Organizador", b =>
                {
                    b.Navigation("Eventos");
                });

            modelBuilder.Entity("API.Eventos.Entities.Participante", b =>
                {
                    b.Navigation("Inscricoes");
                });
#pragma warning restore 612, 618
        }
    }
}
