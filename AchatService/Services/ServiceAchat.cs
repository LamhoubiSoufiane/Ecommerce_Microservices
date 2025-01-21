using AchatService.Data;
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
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AchatService.Services
{
    public class ServiceAchat : IAchatService
    {
        private readonly HttpClient _httpPanierClient;
        private readonly string _panierServiceUrl;
        private readonly EcommerceAchatDB _context;

        public ServiceAchat(EcommerceAchatDB context,IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _context = context;
            //_httpClient = httpClientFactory.CreateClient("DAO_Service");
            _httpPanierClient = httpClientFactory.CreateClient("Panier_Service");
            _panierServiceUrl = "api/panier";
            //_baseUrl = "api/achats";
        }   

        public async Task<Achat> CreateAchatAsync(string userId)
        {
            try
            {
                
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
                _context.Achats.Add(achat);
                await _context.SaveChangesAsync();
                return achat;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la création de l'achat : {ex.Message}");
            }
        }

        public async Task<IEnumerable<Achat>> GetUserAchatsAsync(string userId)
        {
            return await _context.Achats
                .Where(a => a.user_Id == userId)
                .ToListAsync();
        }

        /*public async Task<Achat> GetAchatByIdAsync(int achatId)
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
        }*/
    }
}
