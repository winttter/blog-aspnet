using ASP.NET.Enums;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET.Models
{
    [PrimaryKey(nameof(ObjectGuid))]
    public class Address
    {

        public Guid ObjectGuid { get; set; }
        public int ObjectId { get; set; }
        public string? Text { get; set; }
        public GarAddressLevel ObjectLevel { get; set; }
        public string? ObjectLevelText { get; set; }
        public bool IsActual { get; set; }
        public bool IsActive { get; set; }
        public Guid? PrevId { get; set; }
        public Guid? NextId { get; set; }

    }
}
