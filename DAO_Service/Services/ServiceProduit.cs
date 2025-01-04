using DAO_Service.Data;
using DAO_Service.Interfaces;
using DAO_Service.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAO_Service.Services
{
    public class ServiceProduit : IProduitService
    {
        private readonly EcommDbContext _context;

        public ServiceProduit(EcommDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Produit>> GetAllProductsAsync()
        {
            return await _context.Produits.ToListAsync();
        }
        public async Task<Produit> GetProductByIdAsync(int id)
        {
            return await _context.Produits.FindAsync(id);
        }
        public async Task<Produit> CreateProductAsync(Produit produit)
        {
            _context.Produits.Add(produit);
            await _context.SaveChangesAsync();
            return produit;
        }
        public async Task<IEnumerable<Produit>> GetProductsByCategorieIdAsync(int id)
        {
            var produits = await _context.Produits
                .Where(p => p.categorieId == id)
                .ToListAsync();
            return produits;
        }

        public async Task<Produit> UpdateProductAsync(Produit produit)
        {
            _context.Entry(produit).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return produit;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProduitExists(produit.id))
                    return null;
                throw;
            }
        }
        public async Task<bool> DeleteProductAsync(int id)
        {
            var produit = await _context.Produits.FindAsync(id);
            if (produit == null)
                return false;

            _context.Produits.Remove(produit);
            await _context.SaveChangesAsync();
            return true;
        }
        private bool ProduitExists(int id)
        {
            return _context.Produits.Any(e => e.id == id);
        }
    }
}
