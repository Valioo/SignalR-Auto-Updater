using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferableObjects.Client;

namespace SignalRClient.Context
{
    internal class ClientInfoContext : DbContext
    {
        public DbSet<ClientInfo> ClientInfos { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost\MSSQLSERVER01;Database=ClientAutoUpdate;Trusted_Connection=True;");
        }
    }
}
