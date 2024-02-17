using HotelListingAPI.Models.Hotel;

namespace HotelListingAPI.Models.Country
{
    //For Listing a specific Country with all of its Details:
    public class CountryDto : BaseCountryDto
    {
        public int Id { get; set; }
        
        public List<HotelDto> Hotels { get; set; } //Rule of Thumb: ==> DTOs never ever share Data with real models, they only share with Dtos

    }

    

}