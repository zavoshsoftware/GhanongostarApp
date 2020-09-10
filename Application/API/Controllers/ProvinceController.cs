using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using API.Models;
using Helper;
using Models;
using Newtonsoft.Json.Linq;
using Models.Input;

namespace API.Controllers
{
    public class ProvinceController : Infrastructure.BaseControllerWithUnitOfWork
    {
        readonly StatusManagement status = new StatusManagement();

        [Route("Provinces/get")]
        public ProvinceViewModel GetProvinces()
        {
            ProvinceViewModel result = new ProvinceViewModel();

            result.Result = GetPrivinceList();
            result.Status = status.ReturnStatus(0, Resources.Messages.Success, true);

            return result;
        }

        [Route("city/get")]
        [HttpPost]
        public CityViewModel GetCity(CityInputViewModel province)
        {
            CityViewModel result = new CityViewModel();

            result.Result = GetCityList(province.ProvinceId);
            result.Status = status.ReturnStatus(0, Resources.Messages.Success, true);

            return result;
        }

        #region Helper

        public List<ProvinceItem> GetPrivinceList()
        {
            List<ProvinceItem> provinces = new List<ProvinceItem>();

            List<Province> provincesDb = UnitOfWork.ProvinceRepository.Get().OrderBy(current => current.Title).ToList();

            foreach (Province province in provincesDb)
            {
                provinces.Add(new ProvinceItem()
                {
                    Id = province.Id,
                    Title = province.Title
                });
            }

            return provinces;
        }
      


        public List<CityItem> GetCityList(Guid provinceId)
        {
            List<CityItem> cities = new List<CityItem>();

            

            List<City> citiesDb = UnitOfWork.CityRepository.Get(current => current.ProvinceId == provinceId).OrderBy(current=>current.Title).ToList();

            foreach (City city in citiesDb)
            {
                cities.Add(new CityItem()
                {
                    Id = city.Id,
                    Title = city.Title
                });
            }

            return cities;
        }
        #endregion

    }
}