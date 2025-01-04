using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PanierService.Interfaces;
using PanierService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PanierService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowFrontend")]
    public class PanierController : ControllerBase
    {
        private readonly IPanierService _panierService;

        public PanierController(IPanierService panierService)
        {
            _panierService = panierService;
        }

        [HttpPost]
        public async Task<IActionResult> AjouterAuPanier([FromBody] LignePanier ligne)
        {
            var panierToken = _panierService.GetOrCreatePanierToken();
            var result = await _panierService.AjouterAuPanierAsync(panierToken, ligne);
            if (!result) return BadRequest("Erreur lors de l'ajout au panier.");
            return Ok("Ajouté au panier.");
        }

        [HttpGet]
        public async Task<IActionResult> GetPanier()
        {
            var panierToken = _panierService.GetOrCreatePanierToken();
            var panier = await _panierService.GetPanierAsync(panierToken);
            return Ok(panier);
        }

        [HttpDelete]
        public async Task<IActionResult> SupprimerPanier()
        {
            var panierToken = _panierService.GetOrCreatePanierToken();
            var result = await _panierService.SupprimerPanierAsync(panierToken);
            if (!result) return BadRequest("Erreur lors de la suppression du panier.");
            return Ok("Panier supprimé.");
        }

        /*public IActionResult GetPanier()
        {
            var panierItems = _panierService.GetCartItems();
            return View("~/Views/Panier/Index.cshtml", panierItems);
        }*/
        [HttpDelete("{idproduit}")]
        public async Task<IActionResult> SupprimerProduitDuPanier(int idproduit)
        {
            var panierToken = _panierService.GetOrCreatePanierToken();
            var panierItems =await _panierService.SupprimerLigneAsync(panierToken, idproduit);
            return Ok("Produit supprimé");
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> EffectuerAchat()
        {
            var panierToken = _panierService.GetOrCreatePanierToken();
            var achatCree = await _panierService.CheckoutAsync(panierToken);
            return Ok("achat effectué");
        }



    }
}
