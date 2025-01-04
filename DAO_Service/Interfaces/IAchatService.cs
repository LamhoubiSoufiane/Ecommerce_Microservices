using DAO_Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAO_Service.Interfaces
{
    public interface IAchatService
    {
        Task<Achat> CreateAchatAsync(Achat achat);
        Task<List<Achat>> GetUserAchatsAsync(string userId);
        //Task<Achat> GetAchatByIdAsync(int achatId);
        //Task<bool> UpdateAchatStatusAsync(int achatId, string status);
    }
}
