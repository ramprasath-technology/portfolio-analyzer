using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Domain.DTO.ExternalData
{
    public class CompanyProfile
    {
        public string Symbol { get; set; }
        public Profile Profile { get; set; }

        public CompanyProfile()
        {
            Profile = new Profile();
        }
    }

    public class Profile
    {
        public string CompanyName { get; set; }
        public string Industry { get; set; }
        public string Sector { get; set; }
        public string Country { get; set; }
    }
}
