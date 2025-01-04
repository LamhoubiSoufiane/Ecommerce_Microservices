using DAO_Service.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAO_Service.Data
{
    public class EcommDbContext : DbContext
    {
        public EcommDbContext(DbContextOptions<EcommDbContext> options) : base(options) { }

        public DbSet<Achat> Achats { get; set; }
        public DbSet<Categorie> Categories { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<LignePanier> LignesPanier { get; set; }
        public DbSet<Produit> Produits { get; set; }
    }
}
