using CategorieService.Models;
using Microsoft.EntityFrameworkCore;


namespace CategorieService.Data
{
    public class EcommerceCategorieDB : DbContext
    {
        public EcommerceCategorieDB(DbContextOptions<EcommerceCategorieDB> options) : base(options) { }

        public DbSet<Categorie> Categories { get; set; }
        public DbSet<Produit> Produits { get; set; }

    }
}
