using Microsoft.EntityFrameworkCore;

namespace ASP.NET.Models
{
    [PrimaryKey(nameof(ObjectId))]
    public class House
    {
        public int ObjectId { get; set; }
        public Guid ObjectGuid { get; set; }
        public string HouseNum { get; set; }
        public string AddNum1 { get; set; }
        public string AddNum2 { get; set; }
    }
}
