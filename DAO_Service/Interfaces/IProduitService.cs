using DAO_Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAO_Service.Interfaces
{
    public interface IProduitService
    {
        Task<IEnumerable<Produit>> GetAllProductsAsync();
        Task<Produit> GetProductByIdAsync(int id);
        Task<IEnumerable<Produit>> GetProductsByCategorieIdAsync(int id);
        Task<Produit> CreateProductAsync(Produit produit);
        Task<Produit> UpdateProductAsync(Produit produit);
        Task<bool> DeleteProductAsync(int id);
    }
}
