using Microsoft.AspNetCore.Mvc;

namespace CNWEB22CT1.Controllers
{
	public class ContactController : Controller
	{
		[HttpGet]
		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Index(string name, string email, string subject, string message)
		{
			if (ModelState.IsValid)
			{
				// Xử lý gửi thông tin liên hệ (ví dụ: lưu vào database hoặc gửi email)
				ViewBag.Message = "Thank you for contacting us. We will get back to you soon!";
			}
			else
			{
				ViewBag.Message = "Please fill out the form correctly.";
			}
			return View();
		}
	}
}
