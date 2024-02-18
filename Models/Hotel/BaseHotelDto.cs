using System.ComponentModel.DataAnnotations;

namespace HotelListingAPI.Models.Hotel
{
    public abstract class BaseHotelDto
    {

        [Required]
        public string Name { get; set; }

        [Required]
        public string ShortName { get; set; }

        public double? Rating { get; set; } //Using Nullable to force this Data type to accept null values if nothing is passed in.

        [Required]
        public int CountryId { get; set; }
    }
}
