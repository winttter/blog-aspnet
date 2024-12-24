using ASP.NET.Enums;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET.Models
{
    [PrimaryKey(nameof(ObjectId))]
    public class Address
    {
        public int ObjectId { get; set; }
        public Guid ObjectGuid { get; set; }
        public string? Text { get; set; }
        public string? ObjectLevelText { get; set; }
        public GarAddressLevel ObjectLevel { get; set; }
    }
}
