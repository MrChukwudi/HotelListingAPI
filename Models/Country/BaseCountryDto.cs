namespace HotelListingAPI.Models.Country
{
    public abstract class BaseCountryDto //abstract class is a class that you cannot instantiate, but can use for inheritance purposes.
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
    }
}
