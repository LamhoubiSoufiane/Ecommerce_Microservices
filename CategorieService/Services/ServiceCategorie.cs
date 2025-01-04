using CategorieService.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;

namespace CategorieService.Services
{
    public class ServiceCategorie : ICategorieService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public ServiceCategorie(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient("DAO_Service");
            _baseUrl = "api/categories"; // URL du service de données
        }

        public async Task<IEnumerable<Categorie>> GetAllCategoriesAsync()
        {
            var response=await _httpClient.GetAsync(_baseUrl);
            response.EnsureSuccessStatusCode();
            var categories = await response.Content.ReadFromJsonAsync<List<Categorie>>();
            return categories;
        }

        public async Task<Categorie> GetCategorieByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Categorie>();
        }
        /*public async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            var response = await _httpClient.PostAsJsonAsync(endpoint, data);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<TResponse>();
            return result;
        }*/
        public async Task<Categorie> CreateCategorieAsync(Categorie categorie)
        {
            //return await PostAsync<Categorie, Categorie>(_baseUrl, categorie);
            var response = await _httpClient.PostAsJsonAsync(_baseUrl, categorie);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Categorie>();
        }

        public async Task<Categorie> UpdateCategorieAsync(Categorie categorie)
        {
            var response = await _httpClient.PutAsJsonAsync(_baseUrl, categorie);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Categorie>();
        }

        public async Task<bool> DeleteCategorieAsync(int id)
        {
            var categorie = await GetCategorieByIdAsync(id);
            if (categorie == null)
            {
                return false;
            }
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/{id}");
            response.EnsureSuccessStatusCode();
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
