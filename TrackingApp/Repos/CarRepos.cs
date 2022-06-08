using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TrackingApp.Interface;
using TrackingApp.Models;
using TrackingApp.ViewModel;

namespace TrackingApp.Repos
{
    public class CarRepos : ICar
    {
        private readonly TrackingAppContext _TrackingAppContext;
        private readonly Random _random = new Random();
        public CarRepos(TrackingAppContext trackingAppContext)
        {
            _TrackingAppContext = trackingAppContext;
        }


        public async Task<ResponseDictionary> GetCurrentLocation(string carId)
        {
            ResponseDictionary returnedMessage = null;

            try
            {
                var result = await _TrackingAppContext.Location
                       .Include("Car")
                       .Where(x => x.Car.CarId == carId)
                       .OrderByDescending(x => x.DateTimeAdded)
                       .Select(y => new LocationVM
                       {
                           CarId = y.Car.CarId,
                           Longtitude = y.Longtitude,
                           Latitude = y.Latitude
                       })
                       .FirstOrDefaultAsync();

                if (result == null)
                {
                    return new ResponseDictionary
                    {
                        data = result,
                        ResponseCode = "01",
                        ResponseMessage = "No record found for the Car Id"
                    };
                }
                else
                {
                    return new ResponseDictionary
                    {
                        data = result,
                        ResponseCode = "00",
                        ResponseMessage = "Succesfull"
                    };
                }


            }
            catch (Exception ex)
            {
                return returnedMessage = new ResponseDictionary
                {
                    ExceptionMessage = ex.Message,
                    ResponseCode = "01",
                    ResponseMessage = "Failed"
                };
            }
        }

        public async Task<ResponseDictionary> SaveCar(CarVM carVM)
        {
            ResponseDictionary returnedMessage = null;

            try
            {
                var checkRecord = _TrackingAppContext.Car
                       .Where(x => x.Name == carVM.Name)
                       .FirstOrDefault();

                if (checkRecord == null)
                {
                    var records = new Car
                    {
                        Name = carVM.Name,
                        CarId = _random.Next(1, 1000000).ToString(),
                        DateTimeAdded = DateTime.Now
                    };

                    _TrackingAppContext.Add(records);
                    var result = await _TrackingAppContext.SaveChangesAsync();

                    if (result > 0)
                    {
                        returnedMessage = new ResponseDictionary
                        {
                            ResponseCode = "00",
                            ResponseMessage = "Succesfully",
                            data = new { carId = records.CarId }
                        };
                    }
                    else
                    {
                        returnedMessage = new ResponseDictionary
                        {
                            ResponseCode = "01",
                            ResponseMessage = "No record affected"
                        };
                    }
                }
                else
                {
                    returnedMessage = new ResponseDictionary
                    {
                        ResponseCode = "01",
                        ResponseMessage = "Unknown Car"
                    };
                }
                return returnedMessage;
            }
            catch (Exception ex)
            {
                return returnedMessage = new ResponseDictionary
                {
                    ExceptionMessage = ex.Message,
                    ResponseCode = "01",
                    ResponseMessage = "Failed"
                };
            }
        }

        public async Task<ResponseDictionary> SaveLocation(LocationVM locationVM)
        {
            ResponseDictionary returnedMessage = null;

            try
            {
                var checkRecord = _TrackingAppContext.Car
                       .Where(x => x.CarId == locationVM.CarId)
                       .FirstOrDefault();

                if (checkRecord != null)
                {
                    var records = new Location
                    {
                        Latitude = locationVM.Latitude,
                        Longtitude = locationVM.Longtitude,
                        CarId = checkRecord.Id,
                        DateTimeAdded = DateTime.Now
                    };
                    _TrackingAppContext.Add(records);
                    var result = await _TrackingAppContext.SaveChangesAsync();

                    if (result > 0)
                    {
                        returnedMessage = new ResponseDictionary
                        {
                            ResponseCode = "00",
                            ResponseMessage = "Succesfully"
                        };
                    }
                    else
                    {
                        returnedMessage = new ResponseDictionary
                        {
                            ResponseCode = "01",
                            ResponseMessage = "No record affected"
                        };
                    }
                }
                else
                {
                    returnedMessage = new ResponseDictionary
                    {
                        ResponseCode = "01",
                        ResponseMessage = "Unknown Car"
                    };
                }
                return returnedMessage;
            }
            catch (Exception ex)
            {
                return returnedMessage = new ResponseDictionary
                {
                    ExceptionMessage = ex.Message,
                    ResponseCode = "01",
                    ResponseMessage = "Failed"
                };
            }
        }

        public async Task<ResponseDictionary> GetLocation(string from, string to, string carId)
        {
            ResponseDictionary returnedMessage = null;

            try
            {
                var result = await _TrackingAppContext.Location
                       .Include("Car")
                       .Where(x => x.DateTimeAdded >= Convert.ToDateTime(from) && x.DateTimeAdded <= Convert.ToDateTime(to).AddDays(1).AddMilliseconds(-1) && x.Car.CarId == carId)
                       .OrderByDescending(x => x.DateTimeAdded)
                       .Select(y => new LocationVM
                       {
                           CarId = y.Car.CarId,
                           Longtitude = y.Longtitude,
                           Latitude = y.Latitude
                       })
                       .ToListAsync();

                if (result == null)
                {
                    return new ResponseDictionary
                    {
                        data = result,
                        ResponseCode = "01",
                        ResponseMessage = "No record found for the Car Id"
                    };
                }
                else
                {
                    return new ResponseDictionary
                    {
                        data = result,
                        ResponseCode = "00",
                        ResponseMessage = "Succesfull"
                    };
                }


            }
            catch (Exception ex)
            {
                return returnedMessage = new ResponseDictionary
                {
                    ExceptionMessage = ex.Message,
                    ResponseCode = "01",
                    ResponseMessage = "Failed"
                };
            }
        }

        public ResponseDictionary Login(string userName)
        {
            ResponseDictionary returnedMessage = null;
            try
            {
                if (userName == "admin")
                {
                    returnedMessage = this.CheckUserStatus(userName);
                }
                else
                {
                    returnedMessage = new ResponseDictionary
                    {
                        ResponseCode = "01",
                        ResponseMessage = "Unknown User Details. Kindly login with correct details"
                    };
                }

                return returnedMessage;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ResponseDictionary CheckUserStatus(string userName)
        {
            try
            {
                ResponseDictionary returnedMessage = null;

                var token = this.GetAuthenticationResult(userName);
                returnedMessage = new ResponseDictionary
                {
                    ResponseCode = "00",
                    ResponseMessage = "Succesfully",
                    data = new { Token = token },
                };
                return returnedMessage;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetAuthenticationResult(string userName)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("12345678wertygfcxc"));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim>
                {
                new Claim(JwtRegisteredClaimNames.Sub, "admin"),
                new Claim(JwtRegisteredClaimNames.Jti,  Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, userName),
                new Claim("UserName", userName)
                };

                var token = new JwtSecurityToken("wwww.me.com",
                    "wwww.me.com",
                    claims,
                    expires: DateTime.Now.AddMinutes(Convert.ToDouble("5")),
                    signingCredentials: credentials);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ResponseDictionary> GetCars()
        {
            ResponseDictionary returnedMessage = null;

            try
            {
                var result = await _TrackingAppContext.Car
                       .OrderByDescending(x => x.DateTimeAdded)
                       .ToListAsync();

                if (result == null)
                {
                    return new ResponseDictionary
                    {
                        data = result,
                        ResponseCode = "01",
                        ResponseMessage = "No record found for the Car Id"
                    };
                }
                else
                {
                    return new ResponseDictionary
                    {
                        data = result,
                        ResponseCode = "00",
                        ResponseMessage = "Succesfull"
                    };
                }
            }
            catch (Exception ex)
            {
                return returnedMessage = new ResponseDictionary
                {
                    ExceptionMessage = ex.Message,
                    ResponseCode = "01",
                    ResponseMessage = "Failed"
                };
            }
        }
    }
}
