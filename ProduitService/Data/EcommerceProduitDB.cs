using Microsoft.EntityFrameworkCore;

namespace ProduitService.Data
{
    public class EcommerceProduitDB : DbContext
    {
        public EcommerceProduitDB(DbContextOptions<EcommerceProduitDB> options) : base(options) { }

        //public DbSet<Categorie> Categories { get; set; }
        public DbSet<Produit> Produits { get; set; }
    }
}
