using Hotel_Managment_API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hotel_Managment_API.DBContext
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options):base(options)
        {

        }
        public DbSet<HotelTB> hotels => Set<HotelTB>();
        public DbSet<HotelBranchTB> hotelBranchTBs => Set<HotelBranchTB>();
        public DbSet<ImageMasterTB> imageMasterTBs => Set<ImageMasterTB>();

        public DbSet<RoomCategoryTB> roomCategoryTB => Set<RoomCategoryTB>();

        public DbSet<RoomTB> RoomTB => Set<RoomTB>();

        public DbSet<UserRole> UserRole => Set<UserRole>();

        public DbSet<Hotel_Managment_API.Models.UserRegistration> UserRegistration { get; set; }

        public DbSet<Hotel_Managment_API.Models.RelationshipTB> RelationshipTB { get; set; }

        public DbSet<Hotel_Managment_API.Models.User> User { get; set; }


    }
}
