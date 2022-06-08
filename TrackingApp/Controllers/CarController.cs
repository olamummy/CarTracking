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
using SeerBitDotNetAPILibrary.Interface;
using SeerBitDotNetAPILibrary.Model.Request;
using TrackingApp.Interface;
using TrackingApp.ViewModel;

namespace TrackingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {

        private readonly ICar _ICar;
        private readonly IAuthentication _IAuthentication;
        private readonly IAuthorise _IAuthorise;

        public CarController(ICar iCar, IAuthentication iAuthentication, IAuthorise _iNon3DS)
        {
            this._ICar = iCar;
            _IAuthentication = iAuthentication;
            this._IAuthorise = _iNon3DS;
        }


        [Route("SaveLocation")]
        [HttpPost]
        public async Task<IActionResult> SaveLocation([FromBody] LocationVM locationVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await this._ICar.SaveLocation(locationVM);
                    return Ok(result);
                }
                else
                {
                    var result = new ResponseDictionary()
                    {
                        ResponseCode = "01",
                        ResponseMessage = "Invalid data",
                        ExceptionMessage = ModelState.ToString()

                    };
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadGateway);
                response.ReasonPhrase = ex.Message;
                return BadRequest(response);
            }
        }

        
        [Route("SaveCar")]
        [HttpPost]
        public async Task<IActionResult> SaveCar([FromBody] CarVM carVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await this._ICar.SaveCar(carVM);
                    return Ok(result);
                }
                else
                {
                    var result = new ResponseDictionary()
                    {
                        ResponseCode = "01",
                        ResponseMessage = "Invalid data",
                        ExceptionMessage = ModelState.ToString()

                    };
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadGateway);
                response.ReasonPhrase = ex.Message;
                return BadRequest(response);
            }
        }

        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            var k = await _IAuthentication.GetKey("SBTESTSECK_9Cb8dbqR5Rc2JwZaa77P5QYHzQaeGUcrkEMD1dEi", "SBTESTPUBK_9sN3TuLgW6a9redEfY48cKKkUa09Pz2u");

           var testc = new AuthoriseRequest { amount ="100" };
            await _IAuthorise.Authorise(testc, k);

            new accoun
            return new string[] { "value1", "value2" };
        }
    }
}