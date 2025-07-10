using CNWEB22CT1.Models;
using CNWEB22CT1.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CNWEB22CT1.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<AppUserModel> _userManage;
        private SignInManager<AppUserModel> _signInManager;
        public AccountController(SignInManager<AppUserModel> signInManager, UserManager<AppUserModel> userManage)
        {
            _signInManager = signInManager;
            _userManage = userManage;

        }
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (ModelState.IsValid)
            {
                // Xác thực đăng nhập
                var result = await _signInManager.PasswordSignInAsync(loginVM.Username, loginVM.Password, false, false);

                if (result.Succeeded)
                {
                    // Tìm user hiện tại
                    var user = await _userManage.FindByNameAsync(loginVM.Username);

                    // Kiểm tra nếu user thuộc role "Admin"
                    if (await _userManage.IsInRoleAsync(user, "Admin"))
                    {
                        // Chuyển hướng sang khu vực Admin
                        return Redirect(loginVM.ReturnUrl ?? "/Admin/");
                    }

                    // Nếu không phải Admin thì chuyển hướng về ReturnUrl hoặc trang chủ
                    return Redirect(loginVM.ReturnUrl ?? "/");
                }

                // Đăng nhập thất bại
                ModelState.AddModelError("", "Tên tài khoản hoặc mật khẩu sai!");
            }
            return View(loginVM);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(UserModel user)
        {
            if (ModelState.IsValid)
            {
                // Tạo tài khoản mới
                AppUserModel newUser = new AppUserModel
                {
                    UserName = user.Username,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber // Thêm số điện thoại
                };

                // Tạo tài khoản trong Identity
                IdentityResult result = await _userManage.CreateAsync(newUser, user.Password);

                if (result.Succeeded)
                {
                    // Gán role mặc định là "User"
                    var roleResult = await _userManage.AddToRoleAsync(newUser, "User");

                    if (roleResult.Succeeded)
                    {
                        TempData["success"] = "Tạo tài khoản thành công!";
                        return Redirect("/account/login");
                    }
                    else
                    {
                        // Xử lý lỗi nếu gán role không thành công
                        foreach (var error in roleResult.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
                else
                {
                    // Xử lý lỗi nếu tạo tài khoản thất bại
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return View(user);
        }


        public async Task<IActionResult> Logout(string returnUrl = "/")
        {
            await _signInManager.SignOutAsync();
            return Redirect(returnUrl);
        }
	}
}
