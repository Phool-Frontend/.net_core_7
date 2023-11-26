using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ga.Data;
using ga.Models.Country;
using AutoMapper;

namespace ga.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CountriesController : ControllerBase
    {

        private readonly HotelListingDbContext _context;
        private readonly IMapper _mapper;

        public CountriesController(HotelListingDbContext context, IMapper mapper)
        {
            _context = context;
            this._mapper = mapper;
        }

        //GET :::::: All
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetCountryDto>>> GetCountries()
        {   
            var countries = await _context.Countries.ToListAsync();// Select * from Countries
            var records = _mapper.Map<List<GetCountryDto>>(countries);
            return Ok(records);
        }

        //GET :::::: FindOne
        [HttpGet("{id}")]
        public async Task<ActionResult<CountryDto>> GetCountry(int id)
        {
            var country = await _context.Countries.Include(q => q.Hotels).FirstOrDefaultAsync(q => q.Id == id);

            if (country == null)
            {
                return NotFound();
            }

            var countryDto = _mapper.Map<CountryDto>(country);

            return Ok(country);
        }

        //PUT :::::: Update 
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCountry(int id, UpdateCountryDto UpdateCountryDto)
        {
            if(id != UpdateCountryDto.Id)
            {
                return BadRequest("Invalid Record Id");
            }

            //_context.Entry(UpdateCountryDto).State = EntityState.Modified;
            var country = await _context.Countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }

            _mapper.Map(UpdateCountryDto, country);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                if (!CountryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        //POST :::::: Add
        [HttpPost]
        public async Task<ActionResult<Country>> PostCountry(CreateCountryDto createCountryDto)
        {
           
            var country = _mapper.Map<Country>(createCountryDto);

            _context.Countries.Add(country);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCountry", new { id = country.Id }, country);
        }

        //DELETE :::::: Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            var country = await _context.Countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }

            _context.Countries.Remove(country);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CountryExists(int id)
        {
            return _context.Countries.Any(c => c.Id == id);
        }
    }
}
