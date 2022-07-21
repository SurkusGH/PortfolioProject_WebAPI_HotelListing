using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

        [HttpGet("{id:int}")]
        #region Status.Codes
        [ProducesResponseType(StatusCodes.Status200OK)]                     // <- these attributes gives more info for dev (in swagger)
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        #endregion
        public async Task<IActionResult> GetHotel(int id)
        {
            try
            {
                var hotel = await _unitOfWork.Hotels.Get(q => q.Id == id, new List<string> { "Country" });
                var result = _mapper.Map<HotelDTO>(hotel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetHotel)}"); // <- exceptions comes first in the logger
                return StatusCode(500, "Internal server error. Please Try Again Later.");    // <- tells user that there is server issue
            }
        }
    }
}
