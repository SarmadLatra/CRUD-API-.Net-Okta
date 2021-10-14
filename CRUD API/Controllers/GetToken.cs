using CRUD_API.Models;
using CRUD_API.Services.Interfaces;
using CRUD_API.TokenServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUD_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetToken : ControllerBase
    {
        private readonly IOktaToken _oktaService;
        public GetToken(IOktaToken oktaService)
        {
            this._oktaService = oktaService;
        }
        [HttpGet("getToken")]
        public async Task<string> getToken()
        {
            return await _oktaService.GetToken();
        }
    }
}
