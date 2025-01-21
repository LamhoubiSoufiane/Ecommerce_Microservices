using AchatService.Interfaces;
using AchatService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AchatService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class AchatController : ControllerBase
    {
        private readonly IAchatService _achatService;

        public AchatController(IAchatService achatService)
        {
            _achatService = achatService;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Achat>> CreateAchat()
        {
            try
            {
                //var userId = "1";
                var userId = User.Identity.Name;
                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("User ID is missing.");
                }
                var achat = await _achatService.CreateAchatAsync(userId);
                return Ok(achat);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Description de l'endpoint
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Achat>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Achat>>> GetAchats()
        {
            try
            {
                var userId = User.Identity.Name;
                //var userId = "1";
                var achats = await _achatService.GetUserAchatsAsync(userId);
                return Ok(achats);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[HttpGet("{id}")]
        //[Authorize]
        /*public async Task<ActionResult<Achat>> GetAchat(int id)
        {
            try
            {
                var achat = await _achatService.GetAchatByIdAsync(id);
                if (achat == null)
                    return NotFound();

                // Vérifier que l'utilisateur actuel est bien le propriétaire de l'achat
                if (achat.user_Id != User.Identity.Name)
                    return Forbid();

                return Ok(achat);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }*/

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
