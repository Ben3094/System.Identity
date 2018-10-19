using System;
using System.Collections.Generic;
using System.Text;

namespace System.Identity
{
    public class Location
    {
        public Location(Address address, GeographicCoordinates geographicCoordinates)
        {
            this.Address = address;
            this.GeographicCoordinates = geographicCoordinates;
        }
        public readonly Address Address;
        public readonly GeographicCoordinates GeographicCoordinates;
    }

    public class Address
    {
        public Address(string street, string postalCode, string country)
        {
            this.Street = street;
            this.PostalCode = postalCode;
            this.Country = country;
        }
        public readonly string Street;
        public readonly string PostalCode;
        public readonly string Country;
    }

    public class GeographicCoordinates
    {
        public GeographicCoordinates(double latitude, double longitude, double elevation)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
            this.Elevation = elevation;
        }
        public readonly double Latitude;
        public readonly double Longitude;
        public readonly double Elevation;
    }
}
