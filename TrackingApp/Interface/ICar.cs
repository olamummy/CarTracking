using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrackingApp.ViewModel;

namespace TrackingApp.Interface
{
    public interface ICar
    {
        ResponseDictionary Login(string userName);

        Task<ResponseDictionary> GetCurrentLocation(string carId);

        Task<ResponseDictionary> SaveLocation(LocationVM locationVM);

        Task<ResponseDictionary> SaveCar(CarVM carVM);

        Task<ResponseDictionary> GetLocation(string from, string to, string carId);

        Task<ResponseDictionary> GetCars();
    }
}
