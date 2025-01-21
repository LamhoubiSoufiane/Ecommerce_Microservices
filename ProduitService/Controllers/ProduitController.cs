using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProduitService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProduitService.Controllers
{
    /// <summary>
    /// Contrôleur pour la gestion des produits
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ProduitController : ControllerBase
    {
        private readonly IProduitService _produitService;

        public ProduitController(IProduitService produitService)
        {
            _produitService = produitService;
        }

        /// <summary>
        /// Récupère tous les produits
        /// </summary>
        /// <returns>Liste des produits</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Produit>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Produit>>> GetProduits()
        {
            return Ok(await _produitService.GetAllProductsAsync());
        }

        /// <summary>
        /// Obtenir un produit par son ID
        /// </summary>
        /// <param name="id">ID du produit</param>
        /// <returns>Le produit correspondant à l'ID</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Produit), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Produit>> GetProduit(int id)
        {
            var produit = await _produitService.GetProductByIdAsync(id);
            if (produit == null)
            {
                return NotFound();
            }
            return Ok(produit);
        }

        /// <summary>
        /// Obtenir les produits par catégorie
        /// </summary>
        /// <param name="id">ID de la catégorie</param>
        /// <returns>Liste des produits de la catégorie</returns>
        [HttpGet("productsByCat/{id}")]
        [ProducesResponseType(typeof(IEnumerable<Produit>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Produit>>> GetProductsByCategorie(int id)
        {
            return Ok(await _produitService.GetProductsByCategorieIdAsync(id));
        }

        /// <summary>
        /// Créer un nouveau produit
        /// </summary>
        /// <param name="produit">Données du produit</param>
        /// <param name="upload">Image du produit</param>
        /// <returns>Le produit créé</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Produit), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Produit>> CreateProduit([FromForm] Produit produit, [FromForm] IFormFile upload)
        {
            try
            {
                var uploadResult = await _produitService.UploadImageAsync(upload);
                if (uploadResult != null)
                {
                    produit.imageUrl = uploadResult;
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            await _produitService.CreateProductAsync(produit);
            return CreatedAtAction(nameof(GetProduit), new { id = produit.id }, produit);
        }

        /// <summary>
        /// Mettre à jour un produit
        /// </summary>
        /// <param name="produit">Données du produit</param>
        /// <returns>Le produit mis à jour</returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> PutProduit(Produit produit)
        {
            await _produitService.UpdateProductAsync(produit);
            return NoContent();
        }

        /// <summary>
        /// Supprimer un produit
        /// </summary>
        /// <param name="id">ID du produit</param>
        /// <returns>Le produit supprimé</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProduit(int id)
        {
            var res = await _produitService.DeleteProductAsync(id);
            if (res == false) return NotFound();
            return NoContent();
        }
    }
}
