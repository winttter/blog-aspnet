using Microsoft.EntityFrameworkCore;

namespace ASP.NET.Models
{
    [PrimaryKey(nameof(ObjectGuid))]
    public class House
    {
        public Guid ObjectGuid { get; set; }
        public int ObjectId { get; set; }
        public string HouseNum { get; set; }
        public bool IsActual { get; set; }
        public bool IsActive { get; set; }
    }
}
