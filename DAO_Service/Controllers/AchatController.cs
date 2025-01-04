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
    [Route("api/achats")]
    [ApiController]
    public class AchatController : ControllerBase
    {
        private readonly IAchatService _achatService;
        public AchatController(IAchatService achatService)
        {
            _achatService = achatService;
        }

        [HttpPost]
        public async Task<ActionResult<Achat>> CreateAchat(Achat achat)
        {
            var Resp = await _achatService.CreateAchatAsync(achat);
            if (Resp == null) return NotFound();
            return Ok(Resp);
            //return CreatedAtAction(nameof(GetCategorie), new { id = categorie.id }, categorie);
        }
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<Achat>> GetAchatsByUserIdAsync(string userId)
        {
            var achat = await _achatService.GetUserAchatsAsync(userId);
            if (achat == null)
            {
                return NotFound();
            }
            return Ok(achat);
        }




    }
}
