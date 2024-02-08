using System.ComponentModel.DataAnnotations.Schema;

namespace HotelListingAPI.Data
{
    public class Hotel
    {
        public int Id { get; set; }
        public string Name { get; set; }    
        public string Address { get; set; }
        public double Rating { get; set; }


        [ForeignKey(nameof(CountryId))] //Here we decleared the pointer to our foreign Key to let EF know that we are having a foreign Key here:
        public int CountryId { get; set; }


        public Country Country { get; set; }
    }
}
