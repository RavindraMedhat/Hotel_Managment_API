﻿// <auto-generated />
using Hotel_Managment_API.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Hotel_Managment_API.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20231009114920_apiscall")]
    partial class apiscall
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.32")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Hotel_Management.Models.HotelBranchTB", b =>
                {
                    b.Property<int>("Branch_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active_Flag")
                        .HasColumnType("bit");

                    b.Property<string>("Branch_Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Branch_Contact_No")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Branch_Contect_Person")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Branch_Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(300)")
                        .HasMaxLength(300);

                    b.Property<string>("Branch_Email_Adderss")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Branch_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Branch_map_coordinate")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Delete_Flag")
                        .HasColumnType("bit");

                    b.Property<int>("Hotel_ID")
                        .HasColumnType("int");

                    b.Property<float>("sortedfield")
                        .HasColumnType("real");

                    b.HasKey("Branch_ID");

                    b.ToTable("hotelBranchTBs");
                });

            modelBuilder.Entity("Hotel_Management.Models.HotelTB", b =>
                {
                    b.Property<int>("Hotel_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active_Flag")
                        .HasColumnType("bit");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Contact_No")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Contect_Person")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<bool>("Delete_Flag")
                        .HasColumnType("bit");

                    b.Property<string>("Email_Adderss")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Hotel_Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(300)")
                        .HasMaxLength(300);

                    b.Property<string>("Hotel_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Hotel_map_coordinate")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Standard_check_In_Time")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Standard_check_out_Time")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("sortedfield")
                        .HasColumnType("real");

                    b.HasKey("Hotel_ID");

                    b.ToTable("hotels");
                });

            modelBuilder.Entity("Hotel_Management.Models.ImageMasterTB", b =>
                {
                    b.Property<int>("Image_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active_Flag")
                        .HasColumnType("bit");

                    b.Property<bool>("Delete_Flag")
                        .HasColumnType("bit");

                    b.Property<string>("Image_URl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReferenceTB_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Reference_ID")
                        .HasColumnType("int");

                    b.Property<float>("sortedfield")
                        .HasColumnType("real");

                    b.HasKey("Image_ID");

                    b.ToTable("imageMasterTBs");
                });
#pragma warning restore 612, 618
        }
    }
}
