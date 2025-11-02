using Domain.Contracts;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presistence.Repositories
{
    public class CacheRepository(IConnectionMultiplexer connection) : ICacheRepoitory
    {
        private readonly IDatabase _database = connection.GetDatabase();

        public async Task<string?> GetAsync(string CachKey)
        {
            var cachValue = await _database.StringGetAsync(CachKey);
            return cachValue.IsNullOrEmpty ? null : cachValue.ToString();
        }

        public async Task SetAsync(string CachKey, string CachValue, TimeSpan TimeToLive)
        {
            await _database.StringSetAsync(CachKey, CachValue, TimeToLive);
        }
    }
}
