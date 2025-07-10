using CNWEB22CT1.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CNWEB22CT1.Controllers
{
	public class OrderController : Controller
	{
		private readonly DataContext _dataContext;

		public OrderController(DataContext context)
		{
			_dataContext = context;
		}

        // Hiển thị danh sách đơn hàng của User
        public IActionResult UserOrders()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var orders = _dataContext.Orders
                .Where(o => o.UserName == userEmail)
                .OrderByDescending(o => o.CreatedDate) // Sắp xếp theo ngày tạo giảm dần
                .ToList();

            return View(orders);
        }

        // Hiển thị chi tiết một đơn hàng
        public IActionResult OrderDetails(string orderCode)
		{
			var userEmail = User.FindFirstValue(ClaimTypes.Email);
			if (userEmail == null)
			{
				return RedirectToAction("Login", "Account");
			}

			var orderDetails = _dataContext.OrderDetails
				.Include(od => od.Product) // Bao gồm thông tin sản phẩm
				.Where(od => od.OrderCode == orderCode && od.UserName == userEmail)
				.ToList();

			return View(orderDetails);
		}

	}
}
/*
using CNWEB22CT1.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CNWEB22CT1.Controllers
{
	public class OrderController : Controller
	{
		private readonly DataContext _dataContext;

		public OrderController(DataContext context)
		{
			_dataContext = context;
		}

        // Hiển thị danh sách đơn hàng của User
        public IActionResult UserOrders()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Lấy múi giờ Việt Nam
            var vietNamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

            var orders = _dataContext.Orders
                .Where(o => o.UserName == userEmail)
                .OrderByDescending(o => o.CreatedDate) // Sắp xếp theo ngày tạo giảm dần
                .ToList()
                .Select(o =>
                {
                    // Chuyển đổi thời gian từ UTC sang múi giờ Việt Nam và giảm đi 1 giờ
                    o.CreatedDate = TimeZoneInfo.ConvertTimeFromUtc(o.CreatedDate, vietNamTimeZone).AddHours(-1);
                   
                    return o;
                }).ToList();

            return View(orders);
        }


        // Hiển thị chi tiết một đơn hàng
        public IActionResult OrderDetails(string orderCode)
		{
			var userEmail = User.FindFirstValue(ClaimTypes.Email);
			if (userEmail == null)
			{
				return RedirectToAction("Login", "Account");
			}

			var orderDetails = _dataContext.OrderDetails
				.Include(od => od.Product) // Bao gồm thông tin sản phẩm
				.Where(od => od.OrderCode == orderCode && od.UserName == userEmail)
				.ToList();

			return View(orderDetails);
		}

	}
}

 */