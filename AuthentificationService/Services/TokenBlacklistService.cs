using AuthentificationService.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.StackExchangeRedis;


namespace AuthentificationService.Services
{
    public class TokenBlacklistService : ITokenBlacklistService
    {
        private readonly IDistributedCache _cache;
        public TokenBlacklistService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task AddToBlacklistAsync(string token, TimeSpan expiration)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration
            };

            await _cache.SetStringAsync($"Blacklist_{token}", "invalid", options);
        }

        public async Task<bool> IsTokenBlacklistedAsync(string token)
        {
            var result = await _cache.GetStringAsync($"Blacklist_{token}");
            return result != null;
        }

    }
}
