using DocnetCorePractice.Attribute;
using DocnetCorePractice.Data;
using DocnetCorePractice.Data.Entity;
using DocnetCorePractice.Enum;
using DocnetCorePractice.Model;
using DocnetCorePractice.Service;
using DocnetCorePractice.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Serilog;
using System.Reflection.Metadata.Ecma335;
using System.Xml.Linq;
using ILogger = Serilog.ILogger;

namespace DocnetCorePractice.Controllers
{
    [ApiController]
    public class ActionController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        public readonly ILogger _logger;
        public IUserService _userService;
        public ActionController(IServiceProvider serviceProvider)
        {
            _authenticationService = serviceProvider.GetRequiredService<IAuthenticationService>();
            _userService = serviceProvider.GetRequiredService<IUserService>();
            _logger = Log.Logger;
        }

        [HttpPost]
        [Route("/api/[controller]/login")]
        public IActionResult Login(RequestLoginModel request)
        {
            return Ok(_authenticationService.Authenticator(request));
        }


        [Authorize(AuthenticationSchemes = "Bearer")]
        [ApiKey]
        [HttpGet("/api/[controller]/getalluser")]
        public IActionResult GetAllUser()
        {
            var result = _userService.GetAllUser();
            return Ok(result);
        }


        [HttpPost("/api/[controller]/adduser")]
        public IActionResult AddUser([FromBody] UserModel model)
        {
            try
            {
                var result = _userService.AddUser(model);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet]
        [Route("/api/[controller]/get-user")]
        public IActionResult GetUser(DateTime dateTime, Roles role)
        {
            var result = _userService.GetUser(dateTime, role);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("No user result");
            }
        }

        [HttpDelete]
        [Route("/api/[controller]/delete-user")]
        public IActionResult DeleteUser(string userId)
        {
            try
            {
                var result = _userService.DeleteUser(userId);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /* [HttpPost]
         [Route("/api/[controller]/insert-caffe")]
         public IActionResult InsertCaffe([FromBody] CaffeModel caffeModel)
         {
             _logger.Information("Start InsertCaffe");
             var existCaffeId = caffes.Where(_ => _.Id == caffeModel.Id).Any();
             var existCaffeName = caffes.Where(_ => _.Name == caffeModel.Name).Any();
             if (existCaffeId)
             {
                 _logger.Information("Insert failed");
                 return BadRequest($"id: {existCaffeId} already existed");
             }
             else if (existCaffeName)
             {
                 _logger.Information("Insert failed because name already exist");
                 return BadRequest($"Name: {existCaffeName} already existed");
             }
             else if (caffeModel.Price < 0 || caffeModel.Discount < 0)
             {
                 return BadRequest("Caffe price and discount must >= 0");
             }
             var caffeEntity = new CaffeEntity()
             {
                 Id = caffeModel.Id,
                 Name = caffeModel.Name,
                 Price = caffeModel.Price,
                 Type = Enum.ProductType.B,
                 Discount = caffeModel.Discount,
                 IsActive = caffeModel.Price > 5000,
             };
             caffes.Add(caffeEntity);
             var json = JsonConvert.SerializeObject(caffeEntity);
             _logger.Information(json);
             return Ok(caffeEntity);
         }

         [HttpGet]
         [Route("/api/[controller]/get-all-caffe")]
         public IActionResult GetAllCaffe()
         {
             var isActiveCaffe = caffes.Where(x => x.IsActive == true);
             if (isActiveCaffe.IsNullOrEmpty())
             {
                 return NoContent();
             }
             else
             {
                 return Ok(isActiveCaffe);
             }

         }
         [HttpPost]
         [Route("/api/[controller]/Create-Order-request")]
         public IActionResult CreateOrder([FromBody] CreateOrderRequestModel request)
         {
             _logger.Information("Start Create Order: ");
             var isExistUserId = _userService.GetAllUser().Where(_ => _.Id == request.UserId).Any();
             bool check = false;
             foreach(var item in request.Items)
             {
                 var isExistCaffeId = caffes.FirstOrDefault(caffe => caffe.Id == item.CaffeId);
                 if(isExistCaffeId!=null)
                 {
                     check = true;
                     break;
                 }
             }
             if (check && isExistUserId)
             {

             }
             return Ok(request);
         }
         private double GetTotalPrice(CreateOrderRequestModel model)
         {
             double result = 0;
             foreach (var item in model.Items)
             {
                 var entity = caffes.Where(_ => _.Id == item.CaffeId).FirstOrDefault();
                 var priceAfterDiscount = entity.Price * item.Volumn * (entity.Discount / 100);
                 result += priceAfterDiscount;
             }
             return result;
         }*/
    }
}

// 1. Viết API insert thêm caffe mới vào menu với input là CaffeModel, kiểm tra điều kiện:
//      - Name và Id không tồn tại trong CaffeEntity (nếu không thỏa return code 400)
//      - Price hoặc discount >= 0 (nếu không thỏa return code 400)
//   Nếu có điều kiện nào vi phạm thì không insert và return failed.


// 2. Viết API get all caffe có IsActive = true theo CaffeModel. nếu không có caffe nào thì return code 204

// 3. Viết API get detail caffe có input là Id với điều kiện isActive bằng true. Nếu không có user nào thì return code 204

// 4. Viết API update caffe với input là Id, price và discount. kiểm tra điều kiện:
//      - Id tồn tại trong CaffeEntity (nếu không thỏa return code 404)
//      - Price hoặc discount >= 0 (nếu không thỏa return code 400)
//   Nếu có điều kiện nào vi phạm thì không insert và return failed.

// 5. Viết API Delete caffe với input là Id. Caffe sẽ được delete nếu thỏa điều kiện sau:
//      - Id tồn tại trong CaffeEntity (nếu không thỏa return code 400)

// 6.Viết API insert thêm user mới với input là UserModel, kiểm tra điều kiện:
//      - PhoneNumber và Id không tồn tại trong UserEntity (nếu không thỏa return code 400)
//      - ngày sinh không được nhập quá Datatime.Now (nếu không thỏa return code 400)
//      - PhoneNumber phải đúng 10 ký tự (nếu không thỏa return code 400)
//      - Balance hoặc TotalProduct >= 0 (nếu không thỏa return code 400)
//  Nếu có điều kiện nào vi phạm thì không insert và return failed.

// 7.Viết API get all user data trả về được parse theo UserModel. nếu không có user nào thì return code 204

// 8.Với input là ngày sinh(có kiều dữ liệu DateTime) và role(có kiểu dữ liệu Enum), Viết API get users với điều kiện:
//      - là thành viên vip(có thể là vip1 hoặc vip2) và sinh trong tháng 6
//  Nếu không có user nào thì return code 204

// 9.Viết API update user với input là UserModel, kiểm tra điều kiện:
//      - Id phải tồn tại trong UserEntity (nếu không thỏa return code 404)
//      - ngày sinh không được nhập quá Datatime.Now (nếu không thỏa return code 400)
//      - PhoneNumber phải đúng 10 ký tự (nếu không thỏa return code 400)
//      - Balance hoặc TotalProduct >= 0 (nếu không thỏa return code 400)
//  Nếu có điều kiện nào vi phạm thì không update và return code 400 cho client.

// 10. Viết API Delete user với input là Id. User sẽ được delete nếu thỏa các điều kiện sau:
//      - Id tồn tại trong UserEntity (nếu không thỏa return code 400)
//      - Balance của user bằng 0 (nếu không thỏa return code 400)

// (Lưu ý: các API phải được đặt trong try/catch, nếu APi lỗi sẽ return về code 500)

// 11. Tạo CreateOrderRequest model để nhập các thông tin cho việc insert một order mới theo json:
//{
//  "userId": "string",
//  "items": [
//    {
//      "caffeeId": "string",
//      "volumn": 0,
//    }
//  ]
//}

// 12. Tạo CreateOrderResponse model để trả về thông tin cho việc insert một order mới theo json:
//{
//  "userId": "string",
//  "orderId": "string",
//  "total": 0,
//  "items": [
//    {
//      "name": "string",
//      "unitPrice": 0,
//      "volumn": 0,
//      "discount": 0,
//      "price": 0
//    }
//  ]
//}

// 13. Viết function tính tổng tiền của một order với input là  CreateOrderResponse model.

// 14. Viết API tạo 1 order mới với input là CreateOrderRequest model được tạo ở bài 11. Yêu cầu:
//        - Kiểm tra nếu userId nếu không tồn tại thì return code 404
//        - Kiểm tra nếu list Items là rỗng thì return code 400
//        - khởi tạo một order và insert vào OrderEntity với status là WaitToPay
//        - lần lượt insert các item vào OrderItemEntity
//        - Dùng function trong bài 13 để tính TotalPrice và update vào OrderEntity
//        - Return code 200 theo CreateOrderResponse model.

// 15. Tạo UpdateOrderRequest model để nhập các thông tin cho việc update một order theo json:
//{
//  "orderId": "string",
//  "addItems": [
//    {
//      "caffeeId": "string",
//      "volumn": 0,
//    }
//  ],
//  "updateItems": [
//    {
//      "orderItemId": "string",
//      "volumn": 0
//    }
//  ],
//  "removeItems": [
//    {
//      "orderItemId": "string"
//    }
//  ]
//}

// 16. Viết API update 1 order với input là UpdateOrderRequest model được tạo ở bài 15. Yêu cầu:
//       - Kiểm tra nếu orderId nếu không tồn tại thì return code 404
//       - Kiểm tra Order, nếu Status không phải là WaitToPay => return code 400
//       - Thực hiện thêm các items từ list addItems vào OrderItemEntity
//       - Thực hiện update các items và tính lại ItemPrice từ list updateItems vào OrderItemEntity
//       - Thực hiện xóa các items từ list removeItems vào OrderItemEntity bằng cách thay đổi IsDeleted = true
//       - Thực hiện tính toán lại Totalprice và cập nhật lại OrderEntity.
//       - Return code 202

// 17 Viết API approved order với input là orderId. Yêu cầu:
//       - Kiểm tra nếu orderId nếu không tồn tại thì return code 404
//       - Kiểm tra Order, nếu Status không phải là WaitToPay => return code 400
//       - Kiểm tra Balance của User, nếu Balance < TotalPrice=> return code 400
//       - Thực hiện tính toán lại balance cho user
//       - Update lại status cho Order => Success
//       - Return code 200

