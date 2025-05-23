﻿using Microsoft.EntityFrameworkCore;
using Store.Maro.Core.Entities;
using Store.Maro.Core.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Store.Maro.Repository.Data.Context
{
    public class AppDbContext :DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); 
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductType> Types { get; set; }
        public DbSet<ProductBrand> Brands { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<DeliveryMethod > DeliveryMethods  { get; set; }
        public DbSet<OrderItem> OrderItems{ get; set; }


    }
}
