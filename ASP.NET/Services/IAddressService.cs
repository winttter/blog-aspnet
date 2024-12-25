using Microsoft.EntityFrameworkCore;
using ASP.NET.Models;
using ASP.NET.ModelsDTO.Post;
using ASP.NET.Enums;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using ASP.NET.Mappers;
using ASP.NET.ModelsDTO.Comment;
using Microsoft.AspNetCore.Http.HttpResults;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using ASP.NET.ModelsDTO.Address;
//using ASP.NET.Migrations;
namespace ASP.NET.Services
{
    public interface IAddressService
    {
        Task<List<SearchAddressModel>> FindChildren(int parentObjectId, string query);
        Task<List<SearchAddressModel>> GetChain(Guid objectGuid);
    }

    public class AddressService : IAddressService
    {
        private readonly TestContext _context;

        public AddressService(TestContext context)
        {
            _context = context;
        }

        public async Task<List<SearchAddressModel>> FindChildren(int parentObjectId, string query)
        {
            var addresses = _context.Hierarchy.Where(h => h.ParentObjId == parentObjectId).ToList();

            if (!_context.Hierarchy.Any(h => h.Id == parentObjectId))
            {
                throw new Exception("404*object does not exist");
            }

            var result = new List<SearchAddressModel>();
            foreach (var address in addresses)
            {
                var tempAddress = await _context.Addresses.FirstOrDefaultAsync(a => a.ObjectId == address.Id);
                if (tempAddress != null)
                {
                    if (tempAddress.Text.ToLower().Contains(query.ToLower()))
                    { 
                        result.Add(tempAddress.ToDto()); 
                    }
                }
                else
                {
                    var tempHouse = await _context.Houses.FirstOrDefaultAsync(h => h.ObjectId == address.Id);
                    if (tempHouse != null && tempHouse.HouseNum.ToLower().Contains(query.ToLower()))
                    {
                        result.Add(tempHouse.ToDto());
                    }
                }
            }

            return result;
        }

        public async Task<List<SearchAddressModel>> GetChain(Guid objectGuid)
        {
            int? address = (await _context.Addresses.FirstOrDefaultAsync(a => a.ObjectGuid == objectGuid))?.ObjectId;
            if (address == null)
            {
                address = (await _context.Houses.FirstOrDefaultAsync(h => h.ObjectGuid == objectGuid))?.ObjectId;

                if (address == null)
                {
                    throw new Exception("404*address not found");
                }
            }

            var tempPath = await _context.Hierarchy.FirstOrDefaultAsync(h => h.Id == address);
            if (tempPath == null)
            {
                throw new Exception("404*path not found");
            }

            List<int> path = tempPath.Path.Split('.').Select(p => int.Parse(p)).ToList();

            
            var result = new List<SearchAddressModel>();
            foreach (var p in path)
            {
                var tempAddress = await _context.Addresses.FirstOrDefaultAsync(a => a.ObjectId == p);
                if (tempAddress != null)
                {
                    result.Add(tempAddress.ToDto());
                    
                }
                else
                {
                    var tempHouse = await _context.Houses.FirstOrDefaultAsync(h => h.ObjectId == p);
                    if (tempHouse != null)
                    {
                        result.Add(tempHouse.ToDto());
                    }
                }
            }
            return result;
        }
    }
}
