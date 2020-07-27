using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace UrlTestwork.Models
{
    public class CuttLinkContext : DbContext
    {
        // Таблица ссылок пользователей.
        public DbSet<CuttLink> CuttLinks { get; set; }
        public CuttLinkContext()
        {
            //Database.EnsureCreated();   // создаем базу данных при первом обращении
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {            
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            optionsBuilder.UseMySql(configuration.GetConnectionString("DefaultConnection"));
        }
    }
}
