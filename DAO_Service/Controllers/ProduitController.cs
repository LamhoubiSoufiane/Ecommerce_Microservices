using DAO_Service.Interfaces;
using DAO_Service.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAO_Service.Controllers
{
    [Route("api/produits")]
    [ApiController]
    public class ProduitController : ControllerBase
    {
        private readonly IProduitService _produitService;

        public ProduitController(IProduitService produitService)
        {
            _produitService = produitService;
        }
        // GET: api/produit
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produit>>> GetProduits()
        {
            return Ok(await _produitService.GetAllProductsAsync());
        }


        // GET: api/produit/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Produit>> GetProduit(int id)
        {
            var produit = await _produitService.GetProductByIdAsync(id);
            if (produit == null)
            {
                return NotFound();
            }
            return Ok(produit);
        }
        // GET: api/produit/productsByCat/5
        [HttpGet("productsByCat/{id}")]
        public async Task<ActionResult<IEnumerable<Produit>>> GetProductsByCategorie(int id)
        {
            return Ok(await _produitService.GetProductsByCategorieIdAsync(id));
        }


        [HttpPost]
        public async Task<ActionResult<Produit>> CreateProduit(Produit produit)
        {
            await _produitService.CreateProductAsync(produit);
            return CreatedAtAction(nameof(GetProduit), new { id = produit.id }, produit);
        }

        // PUT: api/Categorie/5
        [HttpPut]
        public async Task<IActionResult> PutProduit(Produit produit)
        {
            await _produitService.UpdateProductAsync(produit);
            return NoContent();
        }

        // DELETE: api/produit/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduit(int id)
        {
            await _produitService.DeleteProductAsync(id);
            return NoContent();
        }
    }
}
