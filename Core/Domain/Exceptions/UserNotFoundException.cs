using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class UserNotFoundException(string email) : NotFoundException($"email {email} is not found, Create a New One")
    {
    }
}
