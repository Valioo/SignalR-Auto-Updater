using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferableObjects.Server;

namespace SignalR.Server.Models
{
    public class ServerDbContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<SoftInformation> SoftInfos { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost\MSSQLSERVER01;Database=ServerAutoUpdate;Trusted_Connection=True;");
        }
    }
}
