using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PanierService.Interfaces;
using PanierService.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StackExchange.Redis;
using System.Net.Http;
using System.Net.Http.Json;

namespace PanierService.Services
{
    public class ServicePanier : IPanierService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDatabase _database;
        private readonly HttpClient _httpAchatClient;
        private readonly string _achatServiceUrl;
        public List<LignePanier> lignesPanier { get; set; }

        public ServicePanier(IHttpClientFactory httpClientFactory,IHttpContextAccessor httpContextAccessor, IConnectionMultiplexer redis)
        {
            _httpContextAccessor = httpContextAccessor;
            _database = redis.GetDatabase();
            _httpAchatClient = httpClientFactory.CreateClient("Achat_Service");
            _achatServiceUrl = "api/achat";

        }

        public string GetOrCreatePanierToken()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context == null)
            {
                throw new InvalidOperationException("HttpContext is not available.");
            }
            // Vérifier si le cookie existe
            if (context.Request.Cookies.TryGetValue("panierToken", out var token))
            {
                return token;
            }

            // Générer un nouveau token
            var newToken = Guid.NewGuid().ToString();
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = context.Request.IsHttps, // S'assurer que le cookie est sécurisé en HTTPS
                SameSite = SameSiteMode.Lax,
                Expires = DateTime.UtcNow.AddDays(30) // Le cookie expirera dans 30 jours
            };
            context.Response.Cookies.Append("panierToken", newToken, cookieOptions);

            return newToken;
        }



        public async Task<bool> AjouterAuPanierAsync(string panierToken, LignePanier lignePanier)
        {

            if (string.IsNullOrWhiteSpace(panierToken))
            {
                throw new ArgumentException("Le token du panier ne peut pas être vide ou null.", nameof(panierToken));
            }
            if (lignePanier == null)
            {
                throw new ArgumentNullException(nameof(lignePanier), "La ligne du panier ne peut pas être null.");
            }
            var panierKey = $"panier:{panierToken}";
            try
            {
                // Récupérer le panier actuel ou initialiser une nouvelle liste
                var panierData = await _database.StringGetAsync(panierKey);
                var panier = panierData.HasValue
                    ? JsonConvert.DeserializeObject<List<LignePanier>>(panierData)
                    : new List<LignePanier>();

                var existingItem = panier.Find(i => i.id_produit == lignePanier.id_produit);
                if (existingItem != null)
                {
                    existingItem.quantite_ligne += lignePanier.quantite_ligne;
                }
                else
                {
                    panier.Add(lignePanier);
                }
                // Mettre à jour le panier dans Redis avec une durée de vie de 30 jours
                var serializedPanier = JsonConvert.SerializeObject(panier);
                return await _database.StringSetAsync(panierKey, serializedPanier, TimeSpan.FromDays(30));
            }
            catch (Exception ex)
            {
                // Log l'exception (par exemple, avec un système de journalisation)
                Console.Error.WriteLine($"Erreur lors de l'ajout au panier : {ex.Message}");
                return false;
            }
            
        }

        public async Task<List<LignePanier>> GetPanierAsync(string panierToken)
        {
            var panierKey = $"panier:{panierToken}";

            var panierJson = await _database.StringGetAsync(panierKey);
            if (panierJson.IsNullOrEmpty) return null;
            return JsonConvert.DeserializeObject<List<LignePanier>>(panierJson);
        }
        public async Task<bool> SupprimerPanierAsync(string panierToken)
        {
            var panierKey = $"panier:{panierToken}";
            return await _database.KeyDeleteAsync(panierKey);
        }

      
        public async Task<List<LignePanier>> SupprimerLigneAsync(string panierToken, int idproduit)
        {
            var panierKey = $"panier:{panierToken}";
            try
            {
                var panierData = await _database.StringGetAsync(panierKey);
                if (!panierData.HasValue)
                    return null;

                var panier = JsonConvert.DeserializeObject<List<LignePanier>>(panierData);
                
                // Trouver et supprimer le produit
                panier.RemoveAll(item => item.id_produit == idproduit);

                // Mettre à jour le panier dans Redis
                var serializedPanier = JsonConvert.SerializeObject(panier);
                await _database.StringSetAsync(panierKey, serializedPanier, TimeSpan.FromDays(30));

                return panier;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erreur lors de la suppression du produit : {ex.Message}");
                return null;
            }
        }

        public async Task<Achat> CheckoutAsync(string panierToken)
        {
            var panierKey = $"panier:{panierToken}";
            try
            {
                var panierData = await _database.StringGetAsync(panierKey);
                if (!panierData.HasValue)
                    return null;

                var panier = JsonConvert.DeserializeObject<List<LignePanier>>(panierData);
                var achatResponse = await _httpAchatClient.PostAsJsonAsync(_achatServiceUrl, panierData);
                achatResponse.EnsureSuccessStatusCode();

                await SupprimerPanierAsync(panierToken);
                return await achatResponse.Content.ReadFromJsonAsync<Achat>();

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erreur lors de la suppression du produit : {ex.Message}");
                return null;
            }
        }
    }
}
