# 💻 Laptop Shop Web - ASP.NET Project

> Một website bán laptop được xây dựng bằng ASP.NET MVC/.NET Core với các tính năng như quản lý sản phẩm, giỏ hàng, thanh toán và quản trị viên.

---

## 🛠️ Công nghệ sử dụng

- **Ngôn ngữ:** C#
- **Framework:** ASP.NET MVC / ASP.NET Core
- **Cơ sở dữ liệu:** SQL Server
- **ORM:** Entity Framework
- **Giao diện:** HTML, CSS, Bootstrap, Razor View
- **Xác thực:** ASP.NET Identity

---

## 🎯 Chức năng chính

### 👨‍💼 Người dùng (Khách hàng)

- ✅ Đăng ký / Đăng nhập / Đăng xuất
- 🔍 Tìm kiếm và lọc laptop theo hãng, cấu hình, giá
- 🛒 Thêm vào giỏ hàng, cập nhật số lượng, xóa sản phẩm
- 📦 Đặt hàng và theo dõi đơn hàng
- 🧾 Xem lịch sử mua hàng

### 👨‍🔧 Quản trị viên (Admin)

- 📦 Quản lý sản phẩm (CRUD)
- 🧑‍💼 Quản lý người dùng
- 📊 Quản lý đơn hàng
- 📈 Thống kê doanh thu, số lượng bán, tồn kho

---

## 🔧 Cài đặt & chạy dự án

### Yêu cầu:

- Visual Studio 2022 trở lên
- .NET 6.0 SDK trở lên
- SQL Server (Express hoặc bản đầy đủ)

### Cách chạy:

1. Clone project:
   ```bash
      git clone https://github.com/thaipro113/Laptop-Shopping.git
2. Mở solution bằng Visual Studio

3.Cấu hình chuỗi kết nối trong appsettings.json (ASP.NET Core) hoặc Web.config (ASP.NET MVC):

json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=LaptopShopDB;Trusted_Connection=True;"
}

4. Tạo database và chạy migration:

dotnet ef database update

5.Chạy project:

dotnet run

