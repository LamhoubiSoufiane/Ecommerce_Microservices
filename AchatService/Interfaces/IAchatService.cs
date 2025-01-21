using AchatService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AchatService.Interfaces
{
    public interface IAchatService
    {
        Task<Achat> CreateAchatAsync(string userId);
        Task<IEnumerable<Achat>> GetUserAchatsAsync(string userId);
        //Task<Achat> GetAchatByIdAsync(int achatId);
        //Task<bool> UpdateAchatStatusAsync(int achatId, string status);
    }
}
