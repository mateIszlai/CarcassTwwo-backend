using CarcassTwwo.Models.Requests;
using CarcassTwwo.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

namespace CarcassTwwo.Controllers
{
    public class IdentityController:Controller
    {

        private IPasswordHasher _passwordHasher;
        private string hash;
        public IdentityController(IPasswordHasher passwordHasher, IConfiguration configuration)
        {
            _passwordHasher = passwordHasher;
            hash = configuration["PASSWORD_HASH"];
        }

        [EnableCors("CorsPolicy")]
        [HttpPost("/identity")]
        public IActionResult Login([FromBody]LoginRequest request)
        {
            (bool Verified, bool NeedsUpdate) = _passwordHasher.Check(hash, request.Password);
            if (Verified) return Ok();
            else return Unauthorized();
        }
    }
}
