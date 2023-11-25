using AutoMapper;
using ga.Data;
using ga.Models.Country;

namespace ga.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Country, CreateCountryDto>().ReverseMap();
        }
    }
}
