using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD_API.Services.Interfaces
{
   public interface IOktaToken
    {
        Task<string> GetToken();

    }
}
