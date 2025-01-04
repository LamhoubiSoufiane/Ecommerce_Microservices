using AchatService.Interfaces;
using AchatService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AchatService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AchatController : ControllerBase
    {
        private readonly IAchatService _achatService;

        public AchatController(IAchatService achatService)
        {
            _achatService = achatService;
        }

        [HttpPost]
        //[Authorize]
        public async Task<ActionResult<Achat>> CreateAchat()
        {
            try
            {
                var userId = "1";
                    //User.Identity.Name;
                var achat = await _achatService.CreateAchatAsync(userId);
                return Ok(achat);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("user")]
        //[Authorize]
        public async Task<ActionResult<IEnumerable<Achat>>> GetUserAchats()
        {
            try
            {
                //var userId = User.Identity.Name;
                var userId = "1";
                var achats = await _achatService.GetUserAchatsAsync(userId);
                return Ok(achats);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        //[Authorize]
        public async Task<ActionResult<Achat>> GetAchat(int id)
        {
            try
            {
                var achat = await _achatService.GetAchatByIdAsync(id);
                if (achat == null)
                    return NotFound();

                // Vérifier que l'utilisateur actuel est bien le propriétaire de l'achat
                /*if (achat.user_Id != User.Identity.Name)
                    return Forbid();*/

                return Ok(achat);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /*[HttpPut("{id}/status")]
        [Authorize]
        public async Task<IActionResult> UpdateAchatStatus(int id, [FromBody] string status)
        {
            try
            {
                var achat = await _achatService.GetAchatByIdAsync(id);
                if (achat == null)
                    return NotFound();

                // Vérifier que l'utilisateur actuel est bien le propriétaire de l'achat
                if (achat.user_Id != User.Identity.Name)
                    return Forbid();

                var result = await _achatService.UpdateAchatStatusAsync(id, status);
                if (result)
                    return Ok("Statut mis à jour avec succès");
                return BadRequest("Erreur lors de la mise à jour du statut");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }*/
    }
}
