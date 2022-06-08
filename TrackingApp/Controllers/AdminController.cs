using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrackingApp.Interface;
using TrackingApp.ViewModel;

namespace TrackingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ICar _ICar;

        public AdminController(ICar iCar)
        {
            this._ICar = iCar;
        }

        [Authorize]
        [Route("GetCurrentLocation/{carId}")]
        [HttpGet]
        public async Task<IActionResult> GetCurrentLocation(string carId)
        {
            try
            {
                var result = await this._ICar.GetCurrentLocation(carId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadGateway);
                response.ReasonPhrase = ex.Message;
                return BadRequest(response);
            }
        }

        [Authorize]
        [Route("GetCarLocations/{carId}/{startDate}/{endDate}")]
        [HttpGet]
        public async Task<IActionResult> GetCurrentLocation(string startDate, string endDate, string carId)
        {
            try
            {
                var result = await this._ICar.GetLocation(startDate, endDate, carId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadGateway);
                response.ReasonPhrase = ex.Message;
                return BadRequest(response);
            }
        }


        [Route("AuthenticateAdmin")]
        [HttpPost]
        public IActionResult AuthenticateUser([FromBody] VMLogin request)
        {
            try
            {
                var result =  _ICar.Login(request.UserName);

                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [Route("GetCars")]
        [HttpGet]
        public async Task<IActionResult> GetCars()
        {
            try
            {
                var result = await this._ICar.GetCars();
                return Ok(result);
            }
            catch (Exception ex)
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadGateway);
                response.ReasonPhrase = ex.Message;
                return BadRequest(response);
            }
        }
    }
}