namespace HotelListingAPI.Models.Hotel
{
    public class HotelDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public double Rating { get; set; }
        public int CountryId { get; set; }
    }
}
