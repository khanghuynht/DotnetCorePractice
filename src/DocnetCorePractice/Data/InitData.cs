using DocnetCorePractice.Data.Entity;
using DocnetCorePractice.Model;
using System.Linq;

namespace DocnetCorePractice.Data
{
    public interface IInitData
    {
        List<UserEntity> GetAllUser();
        bool AddUser(UserEntity entity);
        bool DeleteUser(string id);
        bool AddCaffe(CaffeEntity caffeEntity);
        bool UpdateCaffe(string id, double price, int discount);
        bool AddOrder(OrderEntity orderEntity);
        bool AddOrderItem(OrderItemEntity orderItem);
        List<CaffeEntity>? GetAllCaffe();
        List<OrderItemEntity> GetAllOrderItem();
        List<OrderEntity> GetAllOrder();
        bool DeleteCaffe(string id);
    }
    public class InitData : IInitData
    {
        private static List<UserEntity> users = new List<UserEntity>()
        {
            new UserEntity()
            {
                Id = Guid.NewGuid().ToString("N"),
                FirstName = "huy",
                LastName = "nguyen",
                Sex = Enum.Sex.Male,
                Address = "Ho chi Minh",
                Balance = 100000,
                DateOfBirth = DateTime.Today,
                PhoneNumber = "0123456789",
                Roles = Enum.Roles.Basic,
                TotalProduct = 0,
                IsActive = true
            },
            new UserEntity()
            {
                Id = Guid.NewGuid().ToString("N"),
                FirstName = "Anh",
                LastName = "nguyen",
                Sex = Enum.Sex.Female,
                Address = "Ho chi Minh",
                Balance = 0,
                DateOfBirth = DateTime.Today.AddMonths(1).AddYears(-1),
                PhoneNumber = "0123453789",
                Roles = Enum.Roles.Vip2,
                TotalProduct = 10000,
                IsActive = true
            }
        };

        private static List<CaffeEntity> caffes = new List<CaffeEntity>()
        {
                        new CaffeEntity()
                        {
                            Id = Guid.NewGuid().ToString("N"),
                            Name = "Ca phe sua",
                            Price = 20000,
                            Discount = 10,
                            Type = Enum.ProductType.A,
                            IsActive = true
                        }
        };

        private static List<OrderItemEntity> orderItems = new List<OrderItemEntity>();
        private static List<OrderEntity> orders = new List<OrderEntity>();


        public List<UserEntity>? GetAllUser()
        {
            return users.Where(x => x.IsActive == true).ToList();
        }

        public bool AddUser(UserEntity entity)
        {
            users.Add(entity);
            return true;
        }

        public bool AddCaffe(CaffeEntity caffeEntity)
        {
            caffes.Add(caffeEntity);
            return true;
        }

        public List<CaffeEntity> GetAllCaffe()
        {
            return caffes.Where(x => x.IsActive == true).ToList();
        }


        public bool UpdateCaffe(string id, double price, int discount)
        {
            var existCaffe = caffes.Where(x => x.Id == id).FirstOrDefault();
            if (existCaffe != null)
            {
                existCaffe.Price = price;
                existCaffe.Discount = discount;
                return true;
            }
            return false;
        }

        public bool AddOrder(OrderEntity orderEntity)
        {
            orders.Add(orderEntity);
            return true;
        }

        public bool AddOrderItem(OrderItemEntity orderItem)
        {
            orderItems.Add(orderItem);
            return true;
        }

        public List<OrderItemEntity> GetAllOrderItem()
        {
            return orderItems.Where(x => x.IsActice == true).ToList();
        }

        public List<OrderEntity> GetAllOrder()
        {
            return orders.ToList();
        }

        public bool DeleteCaffe(string id)
        {
            foreach (var entity in caffes)
            {
                if (entity.Id == id)
                {
                    caffes.Remove(entity);
                    return true;
                }
            }
            return false;
        }

        public bool DeleteUser(string id)
        {
            foreach(var entity in users)
            {
                if (entity.Id == id)
                {
                    users.Remove(entity);
                    return true;
                }
            }
            return false;
        }
    }
}
