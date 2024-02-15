using AutoMapper;
using HotelListingAPI.Data;
using HotelListingAPI.Models.Country;

namespace HotelListingAPI.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Country, CreateCountryDto>().ReverseMap(); //ReverMap() eliminates the need to write a line for converting createCountryDto back to Country
        }
    }
}
