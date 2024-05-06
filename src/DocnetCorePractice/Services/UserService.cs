using DocnetCorePractice.Data;
using DocnetCorePractice.Data.Entity;
using DocnetCorePractice.Enum;
using DocnetCorePractice.Model;
using DocnetCorePractice.Repository;
using Microsoft.AspNetCore.Server.IIS.Core;

namespace DocnetCorePractice.Service
{
    public interface IUserService
    {
        List<UserModel> GetAllUser();
        List<UserModel>? AddUser(UserModel userModel);

        UserModel GetUser(DateTime dateTime, Roles applicationEnum);
        List<UserModel>? DeleteUser(string userId);
    }
    public class UserService : IUserService
    {
        private readonly IInitData _initData;
        private readonly IUserRepository _userRepository;

        public UserService(IInitData initData,IUserRepository userRepository)
        {
            _initData = initData;
            _userRepository = userRepository;
        }

        public List<UserModel>? GetAllUser()
        {
            var entity = _userRepository.GetAllUser();
            if (entity == null || !entity.Any())
            {
                return null;
            }
            var result = new List<UserModel>();
            entity.ForEach(x =>
            {
                var model = new UserModel
                {
                    Id = x.Id,
                    Address = x.Address,
                    DateOfBirth = x.DateOfBirth,
                    Balance = x.Balance,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    PhoneNumber = x.PhoneNumber,
                    Sex = x.Sex,
                    TotalProduct = x.TotalProduct
                };
                result.Add(model);
            });
            return result;
        }

        public List<UserModel>? AddUser(UserModel userModel)
        {
/*            var userEntityList = _userRepository.GetAllUser();
            var exist = userEntityList.Where(x => x.PhoneNumber == userModel.PhoneNumber
            || x.Id == userModel.Id).Any();*/
            var invalidBirthDate = userModel.DateOfBirth > DateTime.Today;
            var balanceAndTotalProduct = userModel.Balance < 0 || userModel.TotalProduct < 0;
            var checkPhoneNumer = userModel.PhoneNumber.Length == 10;
            if (/*exist ||*/ invalidBirthDate || balanceAndTotalProduct || !checkPhoneNumer)
            {
                throw new ArgumentException("Id existed\ndate of birth < datetime.Now\nBalance and totalProduct > 0" +
                    "\nphonenumber = 10");
            }
            var entity = new UserEntity()
            {
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                Address = userModel.Address,
                DateOfBirth = userModel.DateOfBirth,
                Balance = userModel.Balance,
                IsActive = true,
                PhoneNumber = userModel.PhoneNumber,
                Sex = userModel.Sex,
                TotalProduct = userModel.TotalProduct,
                Roles = Enum.Roles.Basic,
            };
            var save = _userRepository.AddUser(entity);
            if(save != 1)
            {
                throw new ArgumentException("Add failed");
            }
            return GetAllUser();
        }

        public UserModel? GetUser(DateTime dateTime, Roles applicationEnum)
        {
            var userList = _initData.GetAllUser();
            var validUser = userList.Where(x => x.DateOfBirth.Date == dateTime.Date && x.Roles == applicationEnum).FirstOrDefault();
            if (validUser != null)
            {
                var userModel = new UserModel()
                {
                    Id = validUser.Id,
                    FirstName = validUser.FirstName,
                    LastName = validUser.LastName,
                    Address = validUser.Address,
                    Balance = validUser.Balance,
                    DateOfBirth = validUser.DateOfBirth,
                    PhoneNumber = validUser.PhoneNumber,
                    Sex = validUser.Sex,
                    TotalProduct = validUser.TotalProduct,
                };
                return userModel;
            }
            return null;
        }

        public List<UserModel>? DeleteUser(string userId)
        {
            var userList = _initData.GetAllUser();
            var existUser = userList.Where(x => x.Id == userId).FirstOrDefault() ?? throw new ArgumentException("User is not existed");
            if (existUser.Balance == 0)
            {
                _initData.DeleteUser(userId);
            }
            else
            {
                throw new ArgumentException("User's balanace is not 0");
            }
            return GetAllUser();
        }
    }
}
