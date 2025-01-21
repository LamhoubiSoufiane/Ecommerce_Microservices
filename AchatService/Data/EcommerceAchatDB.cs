using AchatService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AchatService.Data
{
    public class EcommerceAchatDB : DbContext
    {
        public EcommerceAchatDB(DbContextOptions<EcommerceAchatDB> options) : base(options) { }

        public DbSet<Achat> Achats { get; set; }
        public DbSet<LignePanier> LignePaniers { get; set; }
    }
}
