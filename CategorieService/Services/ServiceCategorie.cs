using CategorieService.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using CategorieService.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CategorieService.Services
{
    public class ServiceCategorie : ICategorieService
    {
        private readonly EcommerceCategorieDB _context;

        public ServiceCategorie(EcommerceCategorieDB context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Categorie>> GetAllCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Categorie> GetCategorieByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<Categorie> CreateCategorieAsync(Categorie categorie)
        {
            _context.Categories.Add(categorie);
            await _context.SaveChangesAsync();
            return categorie;
        }

        public async Task<Categorie> UpdateCategorieAsync(Categorie categorie)
        {
            _context.Entry(categorie).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return categorie;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategorieExists(categorie.id))
                    return null;
                throw;
            }
        }

        public async Task<bool> DeleteCategorieAsync(int id)
        {
            var categorie = await _context.Categories.FindAsync(id);
            if (categorie == null)
                return false;

            _context.Categories.Remove(categorie);
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
        private bool CategorieExists(int id)
        {
            return _context.Categories.Any(e => e.id == id);
        }
    }
}
