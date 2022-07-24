using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PortfolioProject_WebAPI_HotelListing.DataModels;
using PortfolioProject_WebAPI_HotelListing.DTOs;
using PortfolioProject_WebAPI_HotelListing.IRepository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PortfolioProject_WebAPI_HotelListing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork; // <- abstracted db access
        private readonly ILogger<CountryController> _logger; // <- logger has to be injected with the controler that is goint to be logged
        private readonly IMapper _mapper; // DtoAccess

        public HotelController(IUnitOfWork unitOfWork,
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
        public async Task<IActionResult> GetHotels()
        {
            try
            {
                var hotels = await _unitOfWork.Hotels.GetAll();
                var results = _mapper.Map<IList<HotelDTO>>(hotels);
                return Ok(results); // <- logs 200 with ok
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetHotels)}"); // <- exceptions comes first in the logger
                return StatusCode(500, "Internal server error. Please Try Again Later."); // <- tells user that there is server issue
            }
        }

        [HttpGet("{id:int}", Name = "GetHotel")]
        #region Status.Codes
        [ProducesResponseType(StatusCodes.Status200OK)]                     // <- these attributes gives more info for dev (in swagger)
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        #endregion
        public async Task<IActionResult> GetHotel(int id)
        {
            try
            {
                var hotel = await _unitOfWork.Hotels.Get(q => q.Id == id, include: q=> q.Include(x => x.Country));
                var result = _mapper.Map<HotelDTO>(hotel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetHotel)}"); // <- exceptions comes first in the logger
                return StatusCode(500, "Internal server error. Please Try Again Later.");    // <- tells user that there is server issue
            }
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateHotel([FromBody] CreateHotelDTO hotelDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(CreateHotel)}");
                return BadRequest(ModelState);
            }
            try
            {
                var hotel = _mapper.Map<Hotel>(hotelDTO);
                await _unitOfWork.Hotels.Insert(hotel);
                await _unitOfWork.Save();

                return CreatedAtRoute("GetHotel", new { id = hotel.Id}, hotel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(CreateHotel)}");
                return StatusCode(500, "Internal server error. Please Try Again Later.");
            }
        }

        [Authorize]
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateHotel(int id, [FromBody] UpdateHotelDTO hotelDTO)
        {
            if (!ModelState.IsValid || id < 1)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(UpdateHotel)}");
                return BadRequest(ModelState);
            }
            try
            {
                var hotel = await _unitOfWork.Hotels.Get(h => h.Id == id);
                if (hotel == null)
                {
                    _logger.LogError($"Invalid Update attempt in {nameof(UpdateHotel)}");
                    return BadRequest("Submited data is invalid");
                }

                _mapper.Map(hotelDTO, hotel);
                _unitOfWork.Hotels.Update(hotel);
                await _unitOfWork.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(UpdateHotel)}");
                return StatusCode(500, "Internal server error. Please Try Again Later.");
            }
        }

        [Authorize]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            if (id < 1)
            {
                _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteHotel)}");
                return BadRequest(ModelState);
            }
            try
            {
                var hotel = await _unitOfWork.Hotels.Get(h => h.Id == id);
                if (hotel == null)
                {
                    _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteHotel)}");
                    return BadRequest("Submited data is invalid");
                }

                await _unitOfWork.Hotels.Delete(id);
                await _unitOfWork.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteHotel)}");
                return StatusCode(500, "Internal server error. Please Try Again Later.");
            }
        }
    }
}
