using HotelListingAPI.Data.Configurations;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelListingAPI.Data
{
    public class HotelListingDbContext :  IdentityDbContext<AppUser> //Use Plain DbContext inheritance here if you're using a different Db for Identity
    {

        //Here we define the contract between our App and Our Database:
        public HotelListingDbContext(DbContextOptions options): base(options) //This is the options defined in the Program.cs file while building the DbContext connection
        { 
        }

        //Creating our Database Tables and Mapping them to our Models:
        public DbSet<Hotel> Hotels { get; set; } //public DbSet<Hotel> Hotels: Create a List of Hotels in our DB and Name the List Hotels and make it publicly acessible within our project.
        public DbSet<Country> Countries { get; set; }




        //Creating Our DB Seed Data:
        protected override void OnModelCreating(ModelBuilder modelBuilder) //Call the Method we want to modify from the Base Class DbContext
        {
            base.OnModelCreating(modelBuilder);

            //Applying IndentityRole Configuration:
            modelBuilder.ApplyConfiguration(new RoleConfiguration());

            //Defining What we need to happen once a DB Table is Created:
            //1: Create some Default Country Data 
            /*modelBuilder.Entity<Country>()
                .HasData( //HasData will take a List of the Data type that is to be seeded
                new Country 
                { 
                    Id = 1,
                    Name = "Jamaica",
                    ShortName = "JM"
                },
                new Country
                {
                    Id = 2,
                    Name = "Bahamas",
                    ShortName = "BS"
                },
                new Country
                {
                    Id = 3,
                    Name = "Cayman Island",
                    ShortName = "CI"
                }
              );
            */
            modelBuilder.ApplyConfiguration(new CountryConfiguration());

            //2: Create some Default Hotel Data 
            /*modelBuilder.Entity<Hotel>()
                .HasData(
                new Hotel
                {
                    Id = 1,
                    Name = "Sandals Resort and Spa",
                    Address = "Negril",
                    CountryId = 1,
                    Rating = 4.5

                },
                 new Hotel
                 {
                     Id = 2,
                     Name = "Comfort Suites",
                     Address = "George Town",
                     CountryId = 3,
                     Rating = 4.3

                 },
                  new Hotel
                  {
                      Id = 3,
                      Name = "Grand Palldium",
                      Address = "Nassua",
                      CountryId = 2,
                      Rating = 4

                  }


                );
            */
            modelBuilder.ApplyConfiguration(new HotelConfiguration());


        }
    }
}
