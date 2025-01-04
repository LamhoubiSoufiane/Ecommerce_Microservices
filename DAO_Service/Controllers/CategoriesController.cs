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
    [Route("api/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategorieService _categorieService;

        public CategoriesController(ICategorieService categorieService)
        {
            _categorieService = categorieService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categorie>>> GetCategories()
        {
            return Ok(await _categorieService.GetAllCategoriesAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Categorie>> GetCategorie(int id)
        {
            var categorie = await _categorieService.GetCategorieByIdAsync(id);
            if (categorie == null)
            {
                return NotFound();
            }
            return Ok(categorie);
        }


        [HttpPost]
        public async Task<ActionResult<Categorie>> CreateCategorie(Categorie categorie)
        {
            var categorieResp = await _categorieService.CreateCategorieAsync(categorie);
            if (categorieResp == null) return NotFound();
            return Ok(categorieResp);
            //return CreatedAtAction(nameof(GetCategorie), new { id = categorie.id }, categorie);
        }

        // PUT: api/Categorie/5
        [HttpPut]
        public async Task<IActionResult> PutCategorie(Categorie categorie)
        {
            await _categorieService.UpdateCategorieAsync(categorie);
            return NoContent();
        }

        // DELETE: api/Categorie/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategorie(int id)
        {
            await _categorieService.DeleteCategorieAsync(id);
            return NoContent();
        }
    }
}
