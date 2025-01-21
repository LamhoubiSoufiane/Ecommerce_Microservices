using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProduitService.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ProduitService.Services
{
    public class ServiceProduit : IProduitService
    {
        private readonly EcommerceProduitDB _context;
        public ServiceProduit(EcommerceProduitDB context)
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
               .Where(p => p.CategorieId == id)
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
        public async Task<string> UploadImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentNullException(nameof(file), "File cannot be null.");
            }
            var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "images");

            if (!Directory.Exists(uploadsFolderPath))
            {
                Directory.CreateDirectory(uploadsFolderPath);
            }
            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
            var filePath = Path.Combine(uploadsFolderPath, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            return $"/{fileName}";

        }
        private bool ProduitExists(int id)
        {
            return _context.Produits.Any(e => e.id == id);
        }
    }
}
