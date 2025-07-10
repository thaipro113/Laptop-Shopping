using CNWEB22CT1.Models;
using CNWEB22CT1.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CNWEB22CT1.Controllers
{
	public class WishlistController : Controller
	{
		private readonly DataContext _context;

		public WishlistController(DataContext context)
		{
			_context = context;
		}

		// Thêm sản phẩm vào wishlist
		public IActionResult Add(int id)
		{
			var userEmail = User.FindFirstValue(ClaimTypes.Email);
			if (userEmail == null)
				return RedirectToAction("Login", "Account");

			// Kiểm tra sản phẩm đã tồn tại trong wishlist chưa
			var existingItem = _context.Wishlists
				.FirstOrDefault(w => w.ProductId == id && w.UserName == userEmail);

			if (existingItem == null)
			{
				var wishlistItem = new WishlistModel
				{
					ProductId = id,
					UserName = userEmail
				};
				_context.Wishlists.Add(wishlistItem);
				_context.SaveChanges();

				// Thêm thông báo vào TempData
				TempData["success"] = "Sản phẩm đã được thêm vào danh sách yêu thích!";
			}
			else
			{
				TempData["error"] = "Sản phẩm đã tồn tại trong danh sách yêu thích!";
			}

			return RedirectToAction("Index", "Home"); // Quay về trang chủ
		}


		// Hiển thị danh sách wishlist
		public IActionResult Index()
		{
			var userEmail = User.FindFirstValue(ClaimTypes.Email);
			if (userEmail == null)
				return RedirectToAction("Login", "Account");

			var wishlist = _context.Wishlists
				.Include(w => w.Product)
				.Where(w => w.UserName == userEmail)
				.ToList();

			return View(wishlist);
		}

		// Xóa sản phẩm khỏi wishlist
		public IActionResult Remove(int id)
		{
			var wishlistItem = _context.Wishlists.Find(id);
			if (wishlistItem != null)
			{
				_context.Wishlists.Remove(wishlistItem);
				_context.SaveChanges();
			}
			return RedirectToAction("Index");
		}
	}
}
