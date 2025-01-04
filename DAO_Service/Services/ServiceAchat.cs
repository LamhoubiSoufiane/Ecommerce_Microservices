using DAO_Service.Data;
using DAO_Service.Interfaces;
using DAO_Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAO_Service.Services
{
    public class ServiceAchat : IAchatService
    {
        private readonly EcommDbContext _context;
        public ServiceAchat(EcommDbContext context)
        {
            _context = context;
        }
        public async Task<Achat> CreateAchatAsync(Achat achat)
        {
            _context.Achats.Add(achat);
            await _context.SaveChangesAsync();
            return achat;
        }

        public async Task<List<Achat>> GetUserAchatsAsync(string userId)
        {
            return new List<Achat>();
            
        }

    }
}
