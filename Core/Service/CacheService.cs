using Domain.Contracts;
using Service.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Service
{
    public class CacheService(ICacheRepoitory cachRepoitory) : ICacheService
    {
        public async Task<string?> GetAsync(string cachKey)
        {
            return await cachRepoitory.GetAsync(cachKey);
        }

        public async Task SetAsync(string cachKey, object cachValue, TimeSpan timeToLive)
        {
            var value = JsonSerializer.Serialize(cachValue);
            await cachRepoitory.SetAsync(cachKey, value, timeToLive);
        }
    }
}
