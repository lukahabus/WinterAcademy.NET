using GDi.WinterAcademy.Zadatak.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GDi.WinterAcademy.Zadatak.Infrastructure
{
    public class WinterAcademyZadatakDbContext : DbContext
    {
        public DbSet<Sensor> Sensors { get; set; }
        public DbSet<SensorType> SensorTypes { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        public WinterAcademyZadatakDbContext(DbContextOptions options) : base(options)
        {


        }
    }
}
