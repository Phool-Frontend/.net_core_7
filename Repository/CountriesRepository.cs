using ga.Contracts;
using ga.Data;

namespace ga.Repository
{
    public class CountriesRepository : GenericRepository<Country>, ICountriesRepository
    {
        public CountriesRepository(HotelListingDbContext context) : base(context)
        {

        }
    }
}
