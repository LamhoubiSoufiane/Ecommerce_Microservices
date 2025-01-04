using PanierService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PanierService.Interfaces
{
    public interface IPanierService
    {
        string GetOrCreatePanierToken();
        Task<bool> AjouterAuPanierAsync(string panierToken,LignePanier lignePanier);
        Task<List<LignePanier>> GetPanierAsync(string panierToken);
        Task<bool> SupprimerPanierAsync(string panierToken);

        Task<List<LignePanier>> SupprimerLigneAsync(string panierToken,int idproduit);

        Task<Achat> CheckoutAsync(string panierToken);

    }
}
