using CNWEB22CT1.Models;
using CNWEB22CT1.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CNWEB22CT1.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(DataContext context, IWebHostEnvironment webHostEnvironment)
        {
            _dataContext = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {

            return View(await _dataContext.Products.OrderByDescending(p=>p.Id).Include(p=>p.Category).Include(p => p.Brand).ToListAsync());
        }
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name");
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductModel product)
        {
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", product.CategoryId);
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", product.BrandId);
            if (ModelState.IsValid)
            {
                product.Slug = product.Name.Replace(" ","-");
                var slug = await _dataContext.Products.FirstOrDefaultAsync(p => p.Slug == product.Slug);
                if(slug != null)
                {
                    ModelState.AddModelError("", "Sản phẩm đã có trong database");
                    return View(product);
                }
               
                if(product.ImageUpload != null)
                    {
                        string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath,"media/products");
                        string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                        string filePath = Path.Combine(uploadDir, imageName);

                        FileStream fs = new FileStream(filePath, FileMode.Create);
                        await product.ImageUpload.CopyToAsync(fs);
                        fs.Close();
                        product.Image = imageName;
                    }
                
                _dataContext.Add(product);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Thêm sản phẩm thành công!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "Model đang bị lỗi gì đó!";
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage); 
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);
            }
            
            return View(product);
        }
        public async Task<IActionResult> Edit(int Id)
        {
            ProductModel product = await _dataContext.Products.FindAsync(Id);
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", product.CategoryId);
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", product.BrandId);
            return View(product);
        }
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(ProductModel product)
		{
			ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", product.CategoryId);
			ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", product.BrandId);

			var existed_product = await _dataContext.Products.FindAsync(product.Id); // Lấy sản phẩm hiện tại

			if (existed_product == null)
			{
				TempData["error"] = "Không tìm thấy sản phẩm!";
				return RedirectToAction("Index");
			}

			if (ModelState.IsValid)
			{
				product.Slug = product.Name.Replace(" ", "-");

				// Kiểm tra nếu người dùng chọn hình ảnh mới
				if (product.ImageUpload != null)
				{
					string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
					string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
					string filePath = Path.Combine(uploadDir, imageName);

					// Xóa hình ảnh cũ nếu tồn tại
					if (!string.IsNullOrEmpty(existed_product.Image))
					{
						string oldFilePath = Path.Combine(uploadDir, existed_product.Image);
						try
						{
							if (System.IO.File.Exists(oldFilePath))
							{
								System.IO.File.Delete(oldFilePath);
							}
						}
						catch (Exception ex)
						{
							ModelState.AddModelError("", "Error while deleting the product image.");
						}
					}

					// Lưu hình ảnh mới
					using (var fs = new FileStream(filePath, FileMode.Create))
					{
						await product.ImageUpload.CopyToAsync(fs);
					}
					existed_product.Image = imageName; // Cập nhật tên hình ảnh mới
				}

				// Cập nhật các thông tin khác
				existed_product.Name = product.Name;
				existed_product.Description = product.Description;
				existed_product.Price = product.Price;
				existed_product.CategoryId = product.CategoryId;
				existed_product.BrandId = product.BrandId;
				existed_product.Slug = product.Slug;

				_dataContext.Update(existed_product);
				await _dataContext.SaveChangesAsync();

				TempData["success"] = "Cập nhật sản phẩm thành công!";
				return RedirectToAction("Index");
			}

			TempData["error"] = "Model đang bị lỗi!";
			return View(product);
		}

		public async Task<IActionResult> Delete(int Id)
		{
			ProductModel product = await _dataContext.Products.FindAsync(Id);
			if (product == null)
			{
				TempData["error"] = "Sản phẩm không tồn tại!";
				return RedirectToAction("Index");
			}

			// Kiểm tra nếu có hình ảnh và xóa file
			if (!string.IsNullOrEmpty(product.Image) && !string.Equals(product.Image, "noname.jpg"))
			{
				string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
				string oldFilePath = Path.Combine(uploadDir, product.Image);

				try
				{
					if (System.IO.File.Exists(oldFilePath))
					{
						System.IO.File.Delete(oldFilePath);
					}
				}
				catch (Exception ex)
				{
					ModelState.AddModelError("", "Có lỗi xảy ra khi xóa hình ảnh.");
				}
			}

			// Xóa sản phẩm khỏi database
			_dataContext.Products.Remove(product);
			await _dataContext.SaveChangesAsync();

			TempData["success"] = "Sản phẩm đã được xóa thành công!";
			return RedirectToAction("Index");
		}

	}
}
