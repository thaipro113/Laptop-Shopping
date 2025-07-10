using CNWEB22CT1.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace CNWEB22CT1.Repository
{
    public class SeedData
    {
        public static void SeedingData(DataContext _context)
        {
            // Chỉ migrate nếu có migration chưa áp dụng
            if (_context.Database.GetPendingMigrations().Any())
            {
                _context.Database.Migrate();
            }

            // Kiểm tra và seed dữ liệu vào Categories
            if (!_context.Categories.Any())
            {
                _context.Categories.AddRange(
                    new CategoryModel { Name = "Macbook", Slug = "macbook", Description = "Macbook là thương hiệu nổi tiếng nhất thế giới", Status = 1 },
                    new CategoryModel { Name = "TUF", Slug = "tuf", Description = "TUF gaming là thương hiệu nổi tiếng nhất thế giới", Status = 1 }
                );
                _context.SaveChanges();
            }

            // Kiểm tra và seed dữ liệu vào Brands
            if (!_context.Brands.Any())
            {
                _context.Brands.AddRange(
                    new BrandModel { Name = "Apple", Slug = "apple", Description = "Apple là thương hiệu nổi tiếng nhất thế giới", Status = 1 },
                    new BrandModel { Name = "Asus", Slug = "asus", Description = "Asus là thương hiệu nổi tiếng nhất thế giới", Status = 1 }
                );
                _context.SaveChanges();
            }

            // Lấy các Id của Category và Brand để tạo Product
            var macbook = _context.Categories.FirstOrDefault(c => c.Slug == "macbook");
            var tuf = _context.Categories.FirstOrDefault(c => c.Slug == "tuf");
            var apple = _context.Brands.FirstOrDefault(b => b.Slug == "apple");
            var asus = _context.Brands.FirstOrDefault(b => b.Slug == "asus");

            if (macbook == null || tuf == null || apple == null || asus == null)
            {
                throw new InvalidOperationException("Dữ liệu Category hoặc Brand không được tạo thành công!");
            }

            // Kiểm tra và seed dữ liệu vào Products
            if (!_context.Products.Any())
            {
                _context.Products.AddRange(
                    new ProductModel
                    {
                        Name = "Macbook",
                        Slug = "macbook",
                        Description = "Macbook là loại máy tính xách tay nổi tiếng nhất thế giới",
                        Image = "1.jpg",
                        CategoryId = macbook.Id,
                        Price = 1200,
                        BrandId = apple.Id
                    },
                    new ProductModel
                    {
                        Name = "Tuf Gaming",
                        Slug = "tuf",
                        Description = "TUF Gaming là loại máy tính xách tay nổi tiếng nhất thế giới",
                        Image = "1.jpg",
                        CategoryId = tuf.Id,
                        Price = 1200,
                        BrandId = asus.Id
                    }
                );
                _context.SaveChanges();
            }
        }
    }
}
