using ga.Data;
using ga.Models.Hotel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ga.Models.Country
{
    public class GetCountryDto : BaseCountryDto
    {
        public int Id { get; set; }

    }

}