﻿// <auto-generated />
using System;
using AchatService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AchatService.Migrations
{
    [DbContext(typeof(EcommerceAchatDB))]
    [Migration("20250105170833_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("AchatService.Models.Achat", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime>("DateAchat")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("user_Id")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("Achats");
                });

            modelBuilder.Entity("AchatService.Models.LignePanier", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("id_achat")
                        .HasColumnType("int");

                    b.Property<int>("id_produit")
                        .HasColumnType("int");

                    b.Property<double>("prixdevente")
                        .HasColumnType("float");

                    b.Property<int>("quantite_ligne")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("id_achat");

                    b.ToTable("LignePaniers");
                });

            modelBuilder.Entity("AchatService.Models.LignePanier", b =>
                {
                    b.HasOne("AchatService.Models.Achat", "achat")
                        .WithMany("lignesPanier")
                        .HasForeignKey("id_achat")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("achat");
                });

            modelBuilder.Entity("AchatService.Models.Achat", b =>
                {
                    b.Navigation("lignesPanier");
                });
#pragma warning restore 612, 618
        }
    }
}
