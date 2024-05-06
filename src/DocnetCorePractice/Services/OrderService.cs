using DocnetCorePractice.Data;
using DocnetCorePractice.Data.Entity;
using DocnetCorePractice.Model;

namespace DocnetCorePractice.Services
{
    public static class Cal
    {
        public static double GetTotalPrice(CreateOrderRequestModel model, List<CaffeEntity> caffes)
        {
            double result = 0;
            foreach (var item in model.Items)
            {
                var entity = caffes.Where(_ => _.Id == item.CaffeId).FirstOrDefault();
                double priceAfterDiscount = (entity.Price * item.Volumn) - (entity.Price * item.Volumn * ((double)entity.Discount / 100));
                result += priceAfterDiscount;
            }
            return result;
        }
    }
    public interface IOrderService
    {
        CreateOrderResponse CreateOrder(CreateOrderRequestModel orderRequestModel);
    }
    public class OrderService : IOrderService
    {
        private readonly IInitData _initData;
        public OrderService(IInitData initData)
        {
            _initData = initData;
        }
        public CreateOrderResponse CreateOrder(CreateOrderRequestModel orderRequestModel)
        {
            var userEntity = _initData.GetAllUser();
            var existUser = userEntity.Any(x => x.Id == orderRequestModel.UserId);
            if (!existUser)
            {
                throw new ArgumentException("userID not exist");
            }

            var caffeEntity = _initData.GetAllCaffe();
            if (caffeEntity is not null)
            {
                foreach (var item in orderRequestModel.Items)
                {
                    var existCaffe = caffeEntity.Any(x => item.CaffeId == x.Id);
                    if (!existCaffe)
                    {
                        throw new ArgumentException("CaffeId does not exist");
                    }
                }
            }
            var orderID = Guid.NewGuid().ToString("N");
            var totalPrice = Cal.GetTotalPrice(orderRequestModel, caffeEntity);
            var order = new OrderEntity()
            {
                Id = orderID,
                TotalPrice = totalPrice,
                Status = Enum.StatusOrder.WaitToPay,
                UserId = orderRequestModel.UserId,
                User = userEntity.Where(x => x.Id == orderRequestModel.UserId).FirstOrDefault()
            };
            var checkOrder = _initData.AddOrder(order);
            foreach (var item in orderRequestModel.Items)
            {
                var entity = caffeEntity.FirstOrDefault(x => x.Id == item.CaffeId);
                var orderItem = new OrderItemEntity()
                {
                    UserId = orderRequestModel.UserId,
                    CaffeId = item.CaffeId,
                    OrderId = orderID,
                    CaffeName = entity.Name,
                    Volumn = item.Volumn,
                    UnitPrice = entity.Price,
                    IsActice = true,
                    IsDeleted = false
                };
                var checkItemOrder = _initData.AddOrderItem(orderItem);
            }
            var responseItem = new List<ResponseItem>();
            foreach(var item in orderRequestModel.Items)
            {
                var entity = caffeEntity.FirstOrDefault(x => x.Id == item.CaffeId);
                var ritem = new ResponseItem()
                {
                    Name = entity.Name,
                    UnitPrice = entity.Price,
                    Volumn = item.Volumn,
                    DisCount = entity.Discount,
                    Price = (entity.Price * item.Volumn) - (entity.Price * item.Volumn * ((double)entity.Discount / 100))
                };
                responseItem.Add(ritem);
            }
            var response = new CreateOrderResponse()
            {
                UserId = orderRequestModel.UserId,
                OrderId = orderID,
                Total = totalPrice,
                Items = responseItem
            };
            return response;
        }
    }
}
