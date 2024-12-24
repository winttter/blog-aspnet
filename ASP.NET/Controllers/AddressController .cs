using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ASP.NET.Models;
using ASP.NET.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using ASP.NET.ModelsDTO.Post;
using System.Security.Claims;
using ASP.NET.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ASP.NET.ModelsDTO.Comment;

namespace ASP.NET.Controllers
{
    [Route("api/address")]
    [ApiController]

    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;
        
        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet("search")]
        public async Task<IActionResult> FindChildren(int parentObjectId = 0, string query = "")
        {
            return Ok(await _addressService.FindChildren(parentObjectId, query));
        }

        [HttpGet("chain")]
        public async Task<IActionResult> GetChain(Guid objectGuid)
        {
            return Ok(await _addressService.GetChain(objectGuid));
        }
    }
}
