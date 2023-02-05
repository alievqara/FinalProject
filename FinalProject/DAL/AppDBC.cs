using FinalProject.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace FinalProject.DAL
{
    public class AppDBC : IdentityDbContext<User>
    {
        public AppDBC(DbContextOptions<AppDBC> options) : base(options)
        {

        }
        public DbSet<Kassa> Kassa { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<SettingData> SettingData { get; set; }
        public DbSet<Musteri> Musteriler { get; set; }
        public DbSet<Phone> Phones { get; set; }
        public DbSet<Computer> Computers { get; set; }
        public DbSet<OtherProduct> OtherProducts { get; set; }
        public DbSet<Category> Categories { get; set; }




    }



}

