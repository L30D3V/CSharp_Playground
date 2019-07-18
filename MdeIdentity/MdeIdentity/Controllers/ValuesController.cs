using System;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace MdeIdentity.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        [Authorize("Authorized")]
        [HttpGet("pes-metros/{alturaPes}")]
        public object Get(double alturaPes)
        {
            return new
            {
                AlturaPes = alturaPes,
                AlturaMetros = Math.Round(alturaPes * 0.3048, 4)
            };
        }

        [Authorize("Admin")]
        [HttpGet("verify-admin")]
        public bool VerifyAdmin()
        {
            return true;
        }

        [Authorize("Manager")]
        [HttpGet("verify-manager")]
        public bool VerifyManager()
        {
            return true;
        }

        [Authorize("Member")]
        [HttpGet("verify-member")]
        public bool VerifyMember()
        {
            return true;
        }
    }
}
