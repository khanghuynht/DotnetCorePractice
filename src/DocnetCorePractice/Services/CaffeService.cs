using DocnetCorePractice.Data;
using DocnetCorePractice.Data.Entity;
using DocnetCorePractice.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace DocnetCorePractice.Services
{
    public interface ICaffeService
    {
        List<CaffeModel>? GetAllCaffe();
        CaffeModel AddCaffe(CaffeModel caffeModel);
        CaffeModel? GetDetailCaffe(string id);
        CaffeModel UpdateCaffe(string id, double price, int discount);
        List<CaffeModel>? DeleteCaffe(string id);
    }
    public class CaffeService : ICaffeService
    {
        private readonly IInitData _initData;
        public CaffeService(IInitData initData)
        {
            _initData = initData;
        }
        public CaffeModel AddCaffe(CaffeModel caffeModel)
        {
            var entity = _initData.GetAllCaffe();
            var existCaffeId = entity.Where(_ => _.Id == caffeModel.Id).Any();
            var existCaffeName = entity.Where(_ => _.Name == caffeModel.Name).Any();
            if (existCaffeId || existCaffeName || caffeModel.Price < 0 || caffeModel.Discount < 0)
            {
                throw new ArgumentException("CaffeId or caffeModel already exist and price,discount >= 0 ");
            }
            var caffeEntity = new CaffeEntity()
            {
                Name = caffeModel.Name,
                Price = caffeModel.Price,
                Type = Enum.ProductType.A,
                Discount = caffeModel.Discount,
                IsActive = true,
            };
            _initData.AddCaffe(caffeEntity);
            return caffeModel;
        }

        public List<CaffeModel>? DeleteCaffe(string id)
        {
            var entity = _initData.GetAllCaffe();
            var existCaffe = entity.Where(entity => entity.Id == id).Any();
            if (existCaffe)
            {
                bool check = _initData.DeleteCaffe(id);
                if (check)
                {
                    return GetAllCaffe();
                }
                else throw new ArgumentException("Error occured while deleting");
            }
            else
            {
                throw new ArgumentException("Id is not existed");
            }
        }

        public List<CaffeModel>? GetAllCaffe()
        {
            var entity = _initData.GetAllCaffe();
            if (entity == null || !entity.Any())
            {
                return null;
            }
            var isActiveCaffe = entity.Where(x => x.IsActive == true);
            var result = new List<CaffeModel>();
            foreach (var x in entity)
            {
                var caffe = new CaffeModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price,
                    Type = x.Type,
                    Discount = x.Discount,
                };
                result.Add(caffe);
            }
            return result;
        }

        public CaffeModel GetDetailCaffe(string id)
        {
            var _entity = _initData.GetAllCaffe();
            var caffeEntity = _entity.Where(x => x.Id == id && x.IsActive == true).FirstOrDefault();
            CaffeModel caffeModel = null;
            if (caffeEntity != null)
            {
                caffeModel = new CaffeModel()
                {
                    Id = caffeEntity.Id,
                    Name = caffeEntity.Name,
                    Price = caffeEntity.Price,
                    Type = caffeEntity.Type,
                    Discount = caffeEntity.Discount,
                };
            }
            return caffeModel;
        }

        public CaffeModel UpdateCaffe(string id, double price, int discount)
        {
            if (price < 0 || discount < 0)
            {
                throw new ArgumentException("price or discount must >= 0");
            }
            bool updateCaffe = _initData.UpdateCaffe(id, price, discount);
            if (updateCaffe)
            {
                var result = GetDetailCaffe(id);
                return result;
            }
            else
            {
                throw new ArgumentException("Id is not existed");
            }
        }


    }
}

