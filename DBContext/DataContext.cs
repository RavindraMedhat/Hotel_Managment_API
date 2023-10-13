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
    }
}
