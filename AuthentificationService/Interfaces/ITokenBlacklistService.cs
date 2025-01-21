using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace AuthentificationService.Interfaces
{
    public interface ITokenBlacklistService
    {
        Task AddToBlacklistAsync(string token, TimeSpan expiration);
        Task<bool> IsTokenBlacklistedAsync(string token);
    }
}
