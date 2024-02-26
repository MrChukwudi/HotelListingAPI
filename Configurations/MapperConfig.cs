using AutoMapper;
using HotelListingAPI.Data;
using HotelListingAPI.Models.Country;
using HotelListingAPI.Models.Hotel;
using HotelListingAPI.Models.Users;

namespace HotelListingAPI.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Country, CreateCountryDto>().ReverseMap(); //ReverMap() eliminates the need to write a line for converting createCountryDto back to Country
            CreateMap<Country, GetCountryDto>().ReverseMap(); 
            CreateMap<Country, GetCountryDto>().ReverseMap(); 
            CreateMap<Country, UpdateCountryDto>().ReverseMap(); 



            CreateMap<Hotel, HotelDto>().ReverseMap(); 
            CreateMap<Hotel, CreateHotelDto>().ReverseMap();



            CreateMap<ApiUser, ApiUserDto>().ReverseMap();
        }
    }
}
