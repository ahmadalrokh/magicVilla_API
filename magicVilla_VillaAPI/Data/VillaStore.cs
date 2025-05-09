using magicVilla_VillaAPI.Models.Dto;

namespace magicVilla_VillaAPI.Data
{
    public static class VillaStore
    {
        public static List<VillaDto> villaList = new()
        {
            new VillaDto {Id=1,Name="pool view",Sqft=3,Occupancy=230 },
            new VillaDto {Id=2,Name="Bech view",Sqft=4,Occupancy=300 }
        };
    }
}
