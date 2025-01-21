using CategorieService.Models;
using CategorieService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CategorieService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategorieController : ControllerBase
    {
        private readonly ICategorieService _categorieService;

        public CategorieController(ICategorieService categorieService)
        {
            _categorieService = categorieService;
        }

        // GET: api/Categorie
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categorie>>> GetCategories()
        {
            return Ok(await _categorieService.GetAllCategoriesAsync());
        }

        // GET: api/Categorie/5
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
        

        

        /*
        public IActionResult GetImage(string filename)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "images", filename);
            if (!System.IO.File.Exists(path))
                return NotFound();

            var image = System.IO.File.OpenRead(path);
            return File(image, "image/jpeg"); 
        }*/

        [HttpPost]
        public async Task<ActionResult<Categorie>> CreateCategorie([FromForm] Categorie categorie, [FromForm] IFormFile upload)
        {
            try
            {
                var uploadResult = await _categorieService.UploadImageAsync(upload);
                if (uploadResult != null)
                {
                    categorie.imageUrl = uploadResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            var categorieResp = await _categorieService.CreateCategorieAsync(categorie);
            if (categorieResp == null) return NotFound();
            return Ok(categorieResp);
            //return CreatedAtAction(nameof(GetCategorie), new { id = categorie.id }, categorieResp);
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
            var res = await _categorieService.DeleteCategorieAsync(id);
            if (res == false) return NotFound();
            return NoContent();
        }
    }
}
