using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface ICacheRepoitory
    {
        // Get
        Task<string?> GetAsync(string CachKey);
        // Set
        Task SetAsync(string CachKey, string CachValue, TimeSpan TimeToLive);
    }
}
