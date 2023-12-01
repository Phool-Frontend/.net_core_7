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
using ga.Contracts;
using Microsoft.AspNetCore.Authorization;

namespace ga.Controllers
{
    [ApiController]
    [Route("api/v[version:apiVersion]countries")]
    [ApiVersion("2.0", Deprecated = true)]
    public class CountriesV2Controller : ControllerBase
    {

        private readonly IMapper _mapper;
        private readonly ICountriesRepository _countriesRepository;
        private readonly ILogger<CountriesController> _logger;

        public CountriesV2Controller(IMapper mapper, ICountriesRepository _countriesRepository,
            ILogger<CountriesController> logger)
        {
            this._mapper = mapper;
            this._countriesRepository = _countriesRepository;
            this._logger = logger;
        }

        //GET :::::: All
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetCountryDto>>> GetCountries()
        {   
            var countries = await _countriesRepository.GetAllAsync();// Select * from Countries
            var records = _mapper.Map<List<GetCountryDto>>(countries);
            return Ok(records);
        }

        //GET :::::: FindOne
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<CountryDto>> GetCountry(int id)
        {
            var country = await _countriesRepository.GetDetails(id);

            if (country == null)
            {
                /**
                _logger.LogWarning($"Record found in {nameof(GetCountry)} with Id: {id}");
                return NotFound();
                **/
                throw new DirectoryNotFoundException(nameof(GetCountry), id);
            }

            var countryDto = _mapper.Map<CountryDto>(country);

            return Ok(countryDto);
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
            var country = await _countriesRepository.GetAsync(id);
            if (country == null)
            {
                return NotFound();
            }

            _mapper.Map(UpdateCountryDto, country);

            try
            {
                await _countriesRepository.UpdateAsync(country);
            }
            catch(DbUpdateConcurrencyException)
            {
                if (!await CountryExists(id))
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
        [Authorize]
        public async Task<ActionResult<Country>> PostCountry(CreateCountryDto createCountryDto)
        {
           
            var country = _mapper.Map<Country>(createCountryDto);

            await _countriesRepository.AddAsync(country);

            return CreatedAtAction("GetCountry", new { id = country.Id }, country);
        }

        //DELETE :::::: Delete
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            var country = await _countriesRepository.GetAsync(id);
            if (country == null)
            {
                //return NotFound();
                throw new DirectoryNotFoundException(nameof(GetCountries), id);
            }

            await _countriesRepository.DeleteAsync(id);

            return NoContent();
        }

        private async Task<bool> CountryExists(int id)
        {
            return await _countriesRepository.Exists(id);
        }
    }
}
