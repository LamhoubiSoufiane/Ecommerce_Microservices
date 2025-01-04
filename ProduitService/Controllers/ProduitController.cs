using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProduitService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProduitService.Controllers
{
    [Route("api/[controller]")]
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
                return null;
            }
            await _produitService.CreateProductAsync(produit);
            return CreatedAtAction(nameof(GetProduit), new { id = produit.id }, produit);
        }


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
            var res = await _produitService.DeleteProductAsync(id);
            if (res == false) return NotFound();
            return NoContent();
        }
    }



}
