using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Abstraction
{
    public interface ICacheService
    {
        // Get
        Task<string?> GetAsync(string cachKey);
        // Set
        Task SetAsync(string cachKey, object cachValue, TimeSpan timeToLive);
    }
}
