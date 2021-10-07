using onboarding.api.Movie.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace onboarding.api.Nation.DTO
{
    public class NationDTO
    {
        public Guid? Id { get; set; }
        public string NationName { get; set; }

    }
    public class NationWithMovieDTO : NationDTO
    {
        public List<MovieDTO> Movie { get; set; }
    }
}
