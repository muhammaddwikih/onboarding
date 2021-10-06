using onboarding.api.Nation.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace onboarding.api.Movie.DTO
{
    public class MovieDTO
    {
        public string Title { get; set; }
        public string Genre { get; set; }
        public int ImdbId { get; set; }
    }

    public class MovieWithNationalDTO : MovieDTO
    {
        public NationDTO Nation { get; set; }
    }
}
