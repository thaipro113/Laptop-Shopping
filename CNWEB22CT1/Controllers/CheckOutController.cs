using CNWEB22CT1.Models;
using CNWEB22CT1.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;

namespace CNWEB22CT1.Controllers
{
    public class CheckOutController : Controller
    {
        private readonly DataContext _dataContext;
        public CheckOutController(DataContext context)
        {
            _dataContext = context;
        }
		public async Task<IActionResult> Checkout()
		{
			var userEmail = User.FindFirstValue(ClaimTypes.Email);
			if (userEmail == null)
			{
				return RedirectToAction("Login", "Account");
			}
			else
			{
				var ordercode = Guid.NewGuid().ToString();
				var orderItem = new OrderModel
				{
					OrderCode = ordercode,
					UserName = userEmail,
					Status = 1,
					CreatedDate = DateTime.Now
				};

				// Thêm đơn hàng vào cơ sở dữ liệu
				_dataContext.Add(orderItem);
				_dataContext.SaveChanges();

				// Lấy danh sách sản phẩm trong giỏ hàng từ session
				List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();

				// Lưu thông tin chi tiết đơn hàng vào cơ sở dữ liệu
				foreach (var cart in cartItems)
				{
					var orderdetails = new OrderDetails
					{
						UserName = userEmail,
						OrderCode = ordercode,
						ProductId = cart.ProductId,
						Price = cart.Price,
						Quantity = cart.Quantity
					};
					_dataContext.Add(orderdetails);
				}

				// Lưu tất cả thay đổi
				_dataContext.SaveChanges();

				// Xóa giỏ hàng trong session
				HttpContext.Session.Remove("Cart");

				// Gửi thông báo thành công
				TempData["success"] = "Check out thành công, vui lòng chờ duyệt đơn hàng";
				return RedirectToAction("Index", "Cart");
			}

			return View();
		}

	}
}
