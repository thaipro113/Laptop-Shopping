using CNWEB22CT1.Models;
using CNWEB22CT1.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CNWEB22CT1.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize]
	public class OrderController : Controller
    {
		private readonly DataContext _dataContext;
		public OrderController(DataContext context)
		{
			_dataContext = context;
		}
		public async Task<IActionResult> Index()
		{

			return View(await _dataContext.Orders.OrderByDescending(p => p.Id).ToListAsync());
		}
        public IActionResult ApproveOrder(string orderCode)
        {
            // Tìm đơn hàng theo OrderCode
            var order = _dataContext.Orders.FirstOrDefault(o => o.OrderCode == orderCode);
            if (order != null)
            {
                // Thay đổi trạng thái đơn hàng
                order.Status = 2; // Giả sử 2 là trạng thái "Thành công"
                order.ApprovedDate = DateTime.Now;
                _dataContext.SaveChanges();

                TempData["success"] = "Đơn hàng đã được duyệt thành công.";
            }
            else
            {
                TempData["error"] = "Không tìm thấy đơn hàng.";
            }

            return RedirectToAction("Index"); // Quay lại danh sách đơn hàng
        }

        public async Task<IActionResult> ViewOrder(string ordercode)
        {
            // Lấy danh sách các chi tiết đơn hàng
            var DetailsOrder = await _dataContext.OrderDetails
                .Include(o => o.Product) // Gồm cả thông tin sản phẩm
                .Where(od => od.OrderCode == ordercode) // Lọc theo mã đơn hàng
                .ToListAsync();

            // Truyền danh sách chi tiết đơn hàng vào view
            return View(DetailsOrder);
        }
        public async Task<IActionResult> Delete(int Id)
        {
            OrderModel order = await _dataContext.Orders.FindAsync(Id);

            // Xóa sản phẩm khỏi database
            _dataContext.Orders.Remove(order);
            await _dataContext.SaveChangesAsync();

            TempData["success"] = "Đơn hàng đã được xóa thành công!";
            return RedirectToAction("Index");
        }
    }
}

/*using CNWEB22CT1.Models;
using CNWEB22CT1.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CNWEB22CT1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly TimeZoneInfo vietNamTimeZone;

        public OrderController(DataContext context)
        {
            _dataContext = context;
            // Lấy múi giờ Việt Nam
            vietNamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _dataContext.Orders
                .OrderByDescending(p => p.Id)
                .ToListAsync();

            // Chuyển đổi thời gian CreatedDate và ApprovedDate sang múi giờ Việt Nam và giảm 1 giờ
            orders = orders.Select(order =>
            {
                order.CreatedDate = TimeZoneInfo.ConvertTimeFromUtc(order.CreatedDate, vietNamTimeZone).AddHours(-1);
                if (order.ApprovedDate.HasValue)
                {
                    order.ApprovedDate = TimeZoneInfo.ConvertTimeFromUtc(order.ApprovedDate.Value, vietNamTimeZone).AddHours(-1);
                }
                return order;
            }).ToList();

            return View(orders);
        }

        public IActionResult ApproveOrder(string orderCode)
        {
            // Tìm đơn hàng theo OrderCode
            var order = _dataContext.Orders.FirstOrDefault(o => o.OrderCode == orderCode);
            if (order != null)
            {
                // Thay đổi trạng thái đơn hàng
                order.Status = 2; // Giả sử 2 là trạng thái "Thành công"
                order.ApprovedDate = DateTime.UtcNow; // Lưu UTC thời gian duyệt đơn
                _dataContext.SaveChanges();

                TempData["success"] = "Đơn hàng đã được duyệt thành công.";
            }
            else
            {
                TempData["error"] = "Không tìm thấy đơn hàng.";
            }

            return RedirectToAction("Index"); // Quay lại danh sách đơn hàng
        }

        public async Task<IActionResult> ViewOrder(string orderCode)
        {
            // Lấy danh sách các chi tiết đơn hàng
            var detailsOrder = await _dataContext.OrderDetails
                .Include(o => o.Product) // Gồm cả thông tin sản phẩm
                .Where(od => od.OrderCode == orderCode) // Lọc theo mã đơn hàng
                .ToListAsync();


            // Truyền danh sách chi tiết đơn hàng vào view
            return View(detailsOrder);
        }

        public async Task<IActionResult> Delete(int id)
        {
            OrderModel order = await _dataContext.Orders.FindAsync(id);

            if (order != null)
            {
                // Xóa sản phẩm khỏi database
                _dataContext.Orders.Remove(order);
                await _dataContext.SaveChangesAsync();

                TempData["success"] = "Đơn hàng đã được xóa thành công!";
            }
            else
            {
                TempData["error"] = "Không tìm thấy đơn hàng để xóa.";
            }

            return RedirectToAction("Index");
        }
    }
}
*/