using ASP.NET.Enums;
using ASP.NET.Models;
using ASP.NET.ModelsDTO.Comment;
using System.Net;

namespace ASP.NET.Mappers
{
    public static class AddressesMapper
    {
        public static SearchAddressModel ToDto(this Address address)
        {
            return new SearchAddressModel
            {
                ObjectId = address.ObjectId,
                ObjectGuid = address.ObjectGuid,
                Text = address.ObjectLevelText + " " + address.Text,
                ObjectLevel = address.ObjectLevel - 1,
                ObjectLevelText = Translations.GetTranslations()[address.ObjectLevel - 1]
            };
        }

        public static SearchAddressModel ToDto(this House house)
        {
            return new SearchAddressModel
            {
                ObjectId = house.ObjectId,
                ObjectGuid = house.ObjectGuid,
                Text = house.HouseNum + (house.AddNum1.Length == 0 ? "" : ", " + house.AddNum1) + (house.AddNum2.Length == 0 ? "" : ", " + house.AddNum2),
                ObjectLevel = GarAddressLevel.Building,
                ObjectLevelText = Translations.GetTranslations()[GarAddressLevel.Building]
            };
        }
    }
}
