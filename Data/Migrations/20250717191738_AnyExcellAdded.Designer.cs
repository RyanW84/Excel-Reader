﻿// <auto-generated />
using ExcelReader.RyanW84.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ExcelReader.RyanW84.Data.Migrations
{
    [DbContext(typeof(ExcelReaderDbContext))]
    [Migration("20250717191738_AnyExcellAdded")]
    partial class AnyExcellAdded
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ExcelReader.RyanW84.Models.ExcelBeginner", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("age")
                        .HasColumnType("int");

                    b.Property<string>("colour")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("height")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("sex")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ExcelBeginner");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Name",
                            age = 1,
                            colour = "NA",
                            height = "F000",
                            sex = "NA"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Name2",
                            age = 2,
                            colour = "NA",
                            height = "F200",
                            sex = "NA"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Name3",
                            age = 3,
                            colour = "NA",
                            height = "F300",
                            sex = "NA"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
