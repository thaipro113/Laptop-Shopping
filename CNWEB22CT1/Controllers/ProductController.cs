using CNWEB22CT1.Models;
using CNWEB22CT1.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CNWEB22CT1.Controllers
{
    public class ProductController : Controller
    {
		private readonly DataContext _dataContext;
		public ProductController(DataContext context)
		{
			_dataContext = context;
		}
		public IActionResult Index()
		{
			var products = _dataContext.Products
				.Include("Category")
				.Include("Brand")
				//.OrderByDescending(p => p.Id)
				.ToList();
			return View(products);
		}
		public async Task<IActionResult> Details(int Id)
		{
			if (Id == null) return RedirectToAction("Index");

			var productsById = _dataContext.Products.FirstOrDefault(p => p.Id == Id);
			if (productsById == null) return NotFound();

			var recommendedProducts = _dataContext.Products
				.Where(p => p.CategoryId == productsById.CategoryId && p.Id != Id) // Sản phẩm cùng danh mục, trừ chính nó
				.Take(6)
				.ToList();


			ViewBag.RecommendedProducts = recommendedProducts;

			return View(productsById);
		}



		public async Task<IActionResult> Search(string searchTerm)
		{
			var products = await _dataContext.Products
			.Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm))
			.ToListAsync();
			ViewBag.Keyword = searchTerm;

			return View(products);
		}

    }
}
