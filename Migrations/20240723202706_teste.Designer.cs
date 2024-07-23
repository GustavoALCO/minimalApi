﻿// <auto-generated />
using CursoAsp.DdContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CursoAsp.Migrations
{
    [DbContext(typeof(RangoDbContext))]
    [Migration("20240723202706_teste")]
    partial class teste
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.7");

            modelBuilder.Entity("CursoAsp.Entities.Ingredientes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Ingredientes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Nome = "Carne de Vaca"
                        },
                        new
                        {
                            Id = 2,
                            Nome = "Cebola"
                        },
                        new
                        {
                            Id = 3,
                            Nome = "Cerveja Escura"
                        },
                        new
                        {
                            Id = 4,
                            Nome = "Fatia de Pão Integral"
                        },
                        new
                        {
                            Id = 5,
                            Nome = "Mostarda"
                        },
                        new
                        {
                            Id = 6,
                            Nome = "Chicória"
                        },
                        new
                        {
                            Id = 7,
                            Nome = "Maionese"
                        },
                        new
                        {
                            Id = 8,
                            Nome = "Vários Temperos"
                        },
                        new
                        {
                            Id = 9,
                            Nome = "Mexilhões"
                        },
                        new
                        {
                            Id = 10,
                            Nome = "Aipo"
                        },
                        new
                        {
                            Id = 11,
                            Nome = "Batatas Fritas"
                        },
                        new
                        {
                            Id = 12,
                            Nome = "Tomate"
                        },
                        new
                        {
                            Id = 13,
                            Nome = "Extrato de Tomate"
                        },
                        new
                        {
                            Id = 14,
                            Nome = "Folha de Louro"
                        },
                        new
                        {
                            Id = 15,
                            Nome = "Cenoura"
                        },
                        new
                        {
                            Id = 16,
                            Nome = "Alho"
                        },
                        new
                        {
                            Id = 17,
                            Nome = "Vinho Tinto"
                        },
                        new
                        {
                            Id = 18,
                            Nome = "Leite de Coco"
                        },
                        new
                        {
                            Id = 19,
                            Nome = "Gengibre"
                        },
                        new
                        {
                            Id = 20,
                            Nome = "Pimenta Malagueta"
                        },
                        new
                        {
                            Id = 21,
                            Nome = "Tamarindo"
                        },
                        new
                        {
                            Id = 22,
                            Nome = "Peixe Firme"
                        },
                        new
                        {
                            Id = 23,
                            Nome = "Pasta de Gengibre e Alho"
                        },
                        new
                        {
                            Id = 24,
                            Nome = "Garam Masala"
                        });
                });

            modelBuilder.Entity("CursoAsp.Entities.Rango", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Rangos");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Nome = "Ensopado Flamengo de Carne de Vaca com Chicória"
                        },
                        new
                        {
                            Id = 2,
                            Nome = "Mexilhões com Batatas Fritas"
                        },
                        new
                        {
                            Id = 3,
                            Nome = "Ragu alla Bolognese"
                        },
                        new
                        {
                            Id = 4,
                            Nome = "Rendang"
                        },
                        new
                        {
                            Id = 5,
                            Nome = "Masala de Peixe"
                        });
                });

            modelBuilder.Entity("IngredientesRango", b =>
                {
                    b.Property<int>("IngredientesId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RangosId")
                        .HasColumnType("INTEGER");

                    b.HasKey("IngredientesId", "RangosId");

                    b.HasIndex("RangosId");

                    b.ToTable("IngredientesRango");

                    b.HasData(
                        new
                        {
                            IngredientesId = 1,
                            RangosId = 1
                        },
                        new
                        {
                            IngredientesId = 2,
                            RangosId = 1
                        },
                        new
                        {
                            IngredientesId = 3,
                            RangosId = 1
                        },
                        new
                        {
                            IngredientesId = 4,
                            RangosId = 1
                        },
                        new
                        {
                            IngredientesId = 5,
                            RangosId = 1
                        },
                        new
                        {
                            IngredientesId = 6,
                            RangosId = 1
                        },
                        new
                        {
                            IngredientesId = 7,
                            RangosId = 1
                        },
                        new
                        {
                            IngredientesId = 8,
                            RangosId = 1
                        },
                        new
                        {
                            IngredientesId = 14,
                            RangosId = 1
                        },
                        new
                        {
                            IngredientesId = 9,
                            RangosId = 2
                        },
                        new
                        {
                            IngredientesId = 19,
                            RangosId = 2
                        },
                        new
                        {
                            IngredientesId = 11,
                            RangosId = 2
                        },
                        new
                        {
                            IngredientesId = 12,
                            RangosId = 2
                        },
                        new
                        {
                            IngredientesId = 13,
                            RangosId = 2
                        },
                        new
                        {
                            IngredientesId = 2,
                            RangosId = 2
                        },
                        new
                        {
                            IngredientesId = 21,
                            RangosId = 2
                        },
                        new
                        {
                            IngredientesId = 8,
                            RangosId = 2
                        },
                        new
                        {
                            IngredientesId = 1,
                            RangosId = 3
                        },
                        new
                        {
                            IngredientesId = 12,
                            RangosId = 3
                        },
                        new
                        {
                            IngredientesId = 17,
                            RangosId = 3
                        },
                        new
                        {
                            IngredientesId = 14,
                            RangosId = 3
                        },
                        new
                        {
                            IngredientesId = 2,
                            RangosId = 3
                        },
                        new
                        {
                            IngredientesId = 16,
                            RangosId = 3
                        },
                        new
                        {
                            IngredientesId = 23,
                            RangosId = 3
                        },
                        new
                        {
                            IngredientesId = 8,
                            RangosId = 3
                        },
                        new
                        {
                            IngredientesId = 1,
                            RangosId = 4
                        },
                        new
                        {
                            IngredientesId = 18,
                            RangosId = 4
                        },
                        new
                        {
                            IngredientesId = 16,
                            RangosId = 4
                        },
                        new
                        {
                            IngredientesId = 20,
                            RangosId = 4
                        },
                        new
                        {
                            IngredientesId = 22,
                            RangosId = 4
                        },
                        new
                        {
                            IngredientesId = 2,
                            RangosId = 4
                        },
                        new
                        {
                            IngredientesId = 21,
                            RangosId = 4
                        },
                        new
                        {
                            IngredientesId = 8,
                            RangosId = 4
                        },
                        new
                        {
                            IngredientesId = 24,
                            RangosId = 5
                        },
                        new
                        {
                            IngredientesId = 10,
                            RangosId = 5
                        },
                        new
                        {
                            IngredientesId = 23,
                            RangosId = 5
                        },
                        new
                        {
                            IngredientesId = 2,
                            RangosId = 5
                        },
                        new
                        {
                            IngredientesId = 12,
                            RangosId = 5
                        },
                        new
                        {
                            IngredientesId = 18,
                            RangosId = 5
                        },
                        new
                        {
                            IngredientesId = 14,
                            RangosId = 5
                        },
                        new
                        {
                            IngredientesId = 20,
                            RangosId = 5
                        },
                        new
                        {
                            IngredientesId = 13,
                            RangosId = 5
                        });
                });

            modelBuilder.Entity("IngredientesRango", b =>
                {
                    b.HasOne("CursoAsp.Entities.Ingredientes", null)
                        .WithMany()
                        .HasForeignKey("IngredientesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CursoAsp.Entities.Rango", null)
                        .WithMany()
                        .HasForeignKey("RangosId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
