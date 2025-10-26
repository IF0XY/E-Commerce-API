using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public sealed class AddressNotFoundException(string userName) : Exception($"User {userName} Has no Address")
    {
    }
}
