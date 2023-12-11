﻿// <auto-generated />
using System;
using Hotel_Managment_API.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Hotel_Managment_API.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20231211062819_login2")]
    partial class login2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.32")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Hotel_Managment_API.Models.HotelBranchTB", b =>
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

            modelBuilder.Entity("Hotel_Managment_API.Models.HotelTB", b =>
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

            modelBuilder.Entity("Hotel_Managment_API.Models.ImageMasterTB", b =>
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

            modelBuilder.Entity("Hotel_Managment_API.Models.RelationshipTB", b =>
                {
                    b.Property<int>("Relationship_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Branch_ID")
                        .HasColumnType("int");

                    b.Property<int>("Hotel_ID")
                        .HasColumnType("int");

                    b.Property<int>("Role_ID")
                        .HasColumnType("int");

                    b.Property<int>("User_ID")
                        .HasColumnType("int");

                    b.HasKey("Relationship_ID");

                    b.ToTable("RelationshipTB");
                });

            modelBuilder.Entity("Hotel_Managment_API.Models.RoomCategoryTB", b =>
                {
                    b.Property<int>("Category_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active_Flag")
                        .HasColumnType("bit");

                    b.Property<int>("Branch_ID")
                        .HasColumnType("int");

                    b.Property<string>("Category_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<bool>("Delete_Flag")
                        .HasColumnType("bit");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(300)")
                        .HasMaxLength(300);

                    b.Property<float>("sortedfield")
                        .HasColumnType("real");

                    b.HasKey("Category_ID");

                    b.ToTable("roomCategoryTB");
                });

            modelBuilder.Entity("Hotel_Managment_API.Models.RoomTB", b =>
                {
                    b.Property<int>("Room_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active_Flag")
                        .HasColumnType("bit");

                    b.Property<int>("Branch_ID")
                        .HasColumnType("int");

                    b.Property<int>("Category_ID")
                        .HasColumnType("int");

                    b.Property<bool>("Delete_Flag")
                        .HasColumnType("bit");

                    b.Property<int>("Hotel_ID")
                        .HasColumnType("int");

                    b.Property<bool>("Iminity_Bath")
                        .HasColumnType("bit");

                    b.Property<int>("Iminity_NoOfBed")
                        .HasColumnType("int");

                    b.Property<bool>("Iminity_Pool")
                        .HasColumnType("bit");

                    b.Property<string>("Room_Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(300)")
                        .HasMaxLength(300);

                    b.Property<string>("Room_No")
                        .IsRequired()
                        .HasColumnType("nvarchar(7)")
                        .HasMaxLength(7);

                    b.Property<float>("Room_Price")
                        .HasColumnType("real");

                    b.Property<float>("sortedfield")
                        .HasColumnType("real");

                    b.HasKey("Room_ID");

                    b.ToTable("RoomTB");
                });

            modelBuilder.Entity("Hotel_Managment_API.Models.User", b =>
                {
                    b.Property<string>("EmailID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("User_ID")
                        .HasColumnType("int");

                    b.HasKey("EmailID");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Hotel_Managment_API.Models.UserRegistration", b =>
                {
                    b.Property<int>("User_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active_Flag")
                        .HasColumnType("bit");

                    b.Property<string>("ConatactNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DOB")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Delete_Flag")
                        .HasColumnType("bit");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("First_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Last_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("sortedfield")
                        .HasColumnType("real");

                    b.HasKey("User_ID");

                    b.ToTable("UserRegistration");
                });

            modelBuilder.Entity("Hotel_Managment_API.Models.UserRole", b =>
                {
                    b.Property<int>("Role_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active_Flag")
                        .HasColumnType("bit");

                    b.Property<bool>("Delete_Flag")
                        .HasColumnType("bit");

                    b.Property<string>("Role_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<float>("sortedfield")
                        .HasColumnType("real");

                    b.HasKey("Role_ID");

                    b.ToTable("UserRole");
                });
#pragma warning restore 612, 618
        }
    }
}
