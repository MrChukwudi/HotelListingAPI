using AutoMapper;
using HotelListingAPI.Data;
using HotelListingAPI.Models.Country;
using HotelListingAPI.Models.Hotel;

namespace HotelListingAPI.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Country, CreateCountryDto>().ReverseMap(); //ReverMap() eliminates the need to write a line for converting createCountryDto back to Country
            CreateMap<Country, GetCountryDto>().ReverseMap(); //ReverMap() eliminates the need to write a line for converting GetCountryDto back to Country
            CreateMap<Country, GetCountryDto>().ReverseMap(); //ReverMap() eliminates the need to write a line for converting GetCountryDto back to Country
            CreateMap<Country, UpdateCountryDto>().ReverseMap(); //ReverMap() eliminates the need to write a line for converting GetCountryDto back to Country
            CreateMap<Hotel, HotelDto>().ReverseMap(); //ReverMap() eliminates the need to write a line for converting GetCountryDto back to Country
        }
    }
}
