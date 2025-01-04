using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        public ServiceProduit(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient("DAO_Service");
            _baseUrl = "api/produits"; // URL du service de données
        }

        public async Task<IEnumerable<Produit>> GetAllProductsAsync()
        {
            var response = await _httpClient.GetAsync(_baseUrl);
            response.EnsureSuccessStatusCode();
            var produits = await response.Content.ReadFromJsonAsync<List<Produit>>();
            return produits;

        }
        public async Task<Produit> GetProductByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Produit>();
        }
        public async Task<Produit> CreateProductAsync(Produit produit)
        {
            produit.dateAjout = DateTime.Now;
            var response = await _httpClient.PostAsJsonAsync(_baseUrl, produit);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Produit>();
        }
        public async Task<IEnumerable<Produit>> GetProductsByCategorieIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/productsByCat/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<Produit>>();
        }

        public async Task<Produit> UpdateProductAsync(Produit produit)
        {
            var response = await _httpClient.PutAsJsonAsync(_baseUrl, produit);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Produit>();
        }
        public async Task<bool> DeleteProductAsync(int id)
        {
            var produit = await GetProductByIdAsync(id);
            if (produit == null)
            {
                return false;
            }
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/{id}");
            return response.IsSuccessStatusCode;
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
            return $"/images/{fileName}";

        }
    }
}
