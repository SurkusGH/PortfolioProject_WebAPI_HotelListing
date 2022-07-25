using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PortfolioProject_WebAPI_HotelListing.DataModels;
using PortfolioProject_WebAPI_HotelListing.DTOs;
using PortfolioProject_WebAPI_HotelListing.IRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PortfolioProject_WebAPI_HotelListing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork; // <- abstracted db access
        private readonly ILogger<CountryController> _logger; // <- logger has to be injected with the controler that is goint to be logged
        private readonly IMapper _mapper; // DtoAccess

        public CountryController(IUnitOfWork unitOfWork,
                                 ILogger<CountryController> logger,
                                 IMapper mapper) // <- injection setup ctor for the whole controller
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        #region Status.Codes
        [ProducesResponseType(StatusCodes.Status200OK)]                     // <- these attributes gives more info for dev (in swagger)
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        #endregion
        public async Task<IActionResult> GetCountries([FromQuery] RequestParams requestParams)
        {

            var countries = await _unitOfWork.Countries.GetPagedList(requestParams);
            var results = _mapper.Map<IList<CountryDTO>>(countries);
            return Ok(results); // <- logs 200 with ok

        }

        [HttpGet("{id:int}", Name = "GetCountry")]
        #region Status.Codes
        [ProducesResponseType(StatusCodes.Status200OK)]                     // <- these attributes gives more info for dev (in swagger)
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        #endregion
        public async Task<IActionResult> GetCountry(int id)
        {

            var country = await _unitOfWork.Countries.Get(q => q.Id == id, include: q => q.Include(x => x.Hotels)); ;
            var result = _mapper.Map<CountryDTO>(country);
            return Ok(result);


        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCountry([FromBody] CreateCountryDTO countryDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(CreateCountry)}");
                return BadRequest(ModelState);
            }

            var country = _mapper.Map<Country>(countryDTO);
            await _unitOfWork.Countries.Insert(country);
            await _unitOfWork.Save();

            return CreatedAtRoute("GetCountry", new { id = country.Id }, country);

        }

        [Authorize]
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCountry(int id, [FromBody] UpdateCountryDTO countryDTO)
        {
            if (!ModelState.IsValid || id < 1)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(UpdateCountry)}");
                return BadRequest(ModelState);
            }

            var country = await _unitOfWork.Countries.Get(c => c.Id == id);
            if (country == null)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(UpdateCountry)}");
                return BadRequest("Submited data is invalid");
            }

            _mapper.Map(countryDTO, country);
            _unitOfWork.Countries.Update(country);
            await _unitOfWork.Save();

            return NoContent();

        }

        [Authorize]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            if (id < 1)
            {
                _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteCountry)}");
                return BadRequest(ModelState);
            }

            var country = await _unitOfWork.Countries.Get(h => h.Id == id);
            if (country == null)
            {
                _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteCountry)}");
                return BadRequest("Submited data is invalid");
            }

            await _unitOfWork.Countries.Delete(id);
            await _unitOfWork.Save();

            return NoContent();
        }
    }
}
