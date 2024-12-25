using ASP.NET.Enums;

namespace ASP.NET.ModelsDTO.Address
{
    public class SearchAddressModel
    {
        public int ObjectId { get; set; }
        public Guid ObjectGuid { get; set; }
        public string? Text { get; set; }
        public GarAddressLevel ObjectLevel { get; set; }
        public string? ObjectLevelText { get; set; }
    }
}
