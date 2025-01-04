using DAO_Service.Data;
using DAO_Service.Interfaces;
using DAO_Service.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAO_Service.Services
{
    public class ServiceCategorie : ICategorieService
    {
        private readonly EcommDbContext _context;

        public ServiceCategorie(EcommDbContext context)
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


        private bool CategorieExists(int id)
        {
            return _context.Categories.Any(e => e.id == id);
        }
    }
}
