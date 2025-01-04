using CategorieService.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CategorieService.Services
{
    public interface ICategorieService
    {
        Task<IEnumerable<Categorie>> GetAllCategoriesAsync();
        Task<Categorie> GetCategorieByIdAsync(int id);
        Task<Categorie> CreateCategorieAsync(Categorie categorie);
        Task<Categorie> UpdateCategorieAsync(Categorie categorie);
        Task<bool> DeleteCategorieAsync(int id);
        Task<string> UploadImageAsync(IFormFile file);
    }
}
