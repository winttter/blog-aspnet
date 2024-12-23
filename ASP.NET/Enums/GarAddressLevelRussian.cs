namespace ASP.NET.Enums
{
    public static class Translations
    {
        public static Dictionary<GarAddressLevel, string> GetTranslations()
        {
            return new Dictionary<GarAddressLevel, string>
            {
                { GarAddressLevel.Region, "Регион" },
                { GarAddressLevel.AdministrativeArea, "Административный район" },
                { GarAddressLevel.MunicipalArea, "Муниципальный округ" },
                { GarAddressLevel.RuralUrbanSettlement, "Сельское или городское поселение" },
                { GarAddressLevel.City, "Город" },
                { GarAddressLevel.Locality, "Населённый пункт" },
                { GarAddressLevel.ElementOfPlanningStructure, "Элемент планировочной структуры" },
                { GarAddressLevel.ElementOfRoadNetwork, "Элемент дорожной сети" },
                { GarAddressLevel.Land, "Земельный участок" },
                { GarAddressLevel.Building, "Здание" },
                { GarAddressLevel.Room, "Помещение" },
                { GarAddressLevel.RoomInRooms, "Помещение в помещениях" },
                { GarAddressLevel.AutonomousRegionLevel, "Уровень автономного региона" },
                { GarAddressLevel.IntracityLevel, "Внутригородской уровень" },
                { GarAddressLevel.AdditionalTerritoriesLevel, "Уровень дополнительных территорий" },
                { GarAddressLevel.LevelOfObjectsInAdditionalTerritories, "Уровень объектов на дополнительных территориях" },
                { GarAddressLevel.CarPlace, "Парковка" }
            };
        }
    }
}
