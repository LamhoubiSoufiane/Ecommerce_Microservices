using DAO_Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAO_Service.Interfaces
{
    public interface ICategorieService
    {
        Task<IEnumerable<Categorie>> GetAllCategoriesAsync();
        Task<Categorie> GetCategorieByIdAsync(int id);
        Task<Categorie> CreateCategorieAsync(Categorie categorie);
        Task<Categorie> UpdateCategorieAsync(Categorie categorie);
        Task<bool> DeleteCategorieAsync(int id);
    }
}
