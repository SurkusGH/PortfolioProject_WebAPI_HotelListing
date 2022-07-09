using AutoMapper;
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
        public async Task<IActionResult> GetCountries()
        {
            try
            {
                var countries = await _unitOfWork.Countries.GetAll();
                var results = _mapper.Map<IList<CountryDTO>>(countries);
                return Ok(results); // <- logs 200 with ok
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetCountries)}"); // <- exceptions comes first in the logger
                return StatusCode(500, "Internal server error. Please Try Again Later."); // <- tels user that there server issue
            }
        }
    }
}
