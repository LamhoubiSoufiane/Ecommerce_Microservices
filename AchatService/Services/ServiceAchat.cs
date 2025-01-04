using AchatService.Interfaces;
using AchatService.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace AchatService.Services
{
    public class ServiceAchat : IAchatService
    {
        private readonly HttpClient _httpClient;
        private readonly HttpClient _httpPanierClient;
        private readonly string _panierServiceUrl;
        private readonly string _baseUrl;

        public ServiceAchat(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient("DAO_Service");
            _httpPanierClient = httpClientFactory.CreateClient("Panier_Service");
            _panierServiceUrl = "api/panier";
            _baseUrl = "api/achats";
    }

        public async Task<Achat> CreateAchatAsync(string userId)
        {
            try
            {
                // 1. Récupérer le panier depuis le service panier
                var panierResponse = await _httpPanierClient.GetAsync(_panierServiceUrl);
                if (!panierResponse.IsSuccessStatusCode)
                    throw new Exception("Erreur lors de la récupération du panier");

                var panierContent = await panierResponse.Content.ReadAsStringAsync();
                var lignesPanier = JsonConvert.DeserializeObject<List<LignePanier>>(panierContent);
                if (lignesPanier == null || lignesPanier.Count == 0)
                    throw new Exception("Le panier est vide");

                // 2. Créer l'achat
                var achat = new Achat
                {
                    DateAchat = DateTime.Now,
                    user_Id = userId,
                    lignesPanier = lignesPanier,
                    Status = "En attente"
                };

              
                var response = await _httpClient.PostAsJsonAsync(_baseUrl, achat);
                response.EnsureSuccessStatusCode();
                if (!response.IsSuccessStatusCode)
                    throw new Exception("Erreur lors de l'enregistrement de l'achat");
                await _httpClient.DeleteAsync(_panierServiceUrl);

                return await response.Content.ReadFromJsonAsync<Achat>(); ;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la création de l'achat : {ex.Message}");
            }
        }

        public async Task<List<Achat>> GetUserAchatsAsync(string userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/user/{userId}");
                if (!response.IsSuccessStatusCode)
                    throw new Exception("Erreur lors de la récupération des achats");

                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Achat>>(content);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la récupération des achats : {ex.Message}");
            }
        }

        public async Task<Achat> GetAchatByIdAsync(int achatId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/{achatId}");
                if (!response.IsSuccessStatusCode)
                    throw new Exception("Erreur lors de la récupération de l'achat");

                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Achat>(content);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la récupération de l'achat : {ex.Message}");
            }
        }

        public async Task<bool> UpdateAchatStatusAsync(int achatId, string status)
        {
            try
            {
                var achat = await GetAchatByIdAsync(achatId);
                if (achat == null)
                    throw new Exception("Achat non trouvé");

                achat.Status = status;
                var achatJson = JsonConvert.SerializeObject(achat);
                var content = new StringContent(achatJson, Encoding.UTF8, "application/json");
                
                var response = await _httpClient.PutAsync($"{_baseUrl}/api/Achat/{achatId}", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la mise à jour du statut : {ex.Message}");
            }
        }
    }
}
