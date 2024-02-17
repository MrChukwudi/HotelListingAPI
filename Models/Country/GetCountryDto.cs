using System.ComponentModel.DataAnnotations.Schema;

namespace HotelListingAPI.Models.Country
{
    //For Listing all Countries, where we have no need to display the list Hotels in each Coutry:
    public class GetCountryDto : BaseCountryDto
    {
        public int Id { get; set; }
        
    }

    

}