using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories.Dtos.Auth;
using Services.IService;

namespace UngCamTuanKietFall2024RazorPages.Pages.Auth
{
    public class LoginModel : PageModel
    {
        private readonly IUserService _userService;

        public LoginModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public LoginUserDto Input { get; set; } = new LoginUserDto();


        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userService.Login(Input);
            var getUser = await _userService.GetUserByEmail(Input.Email);
            if (user.Code == 1)
            {
                TempData["ErrorMessage"] = user.Message;
                return Page();
            }
            if(user.Code == 2)
            {
                HttpContext.Session.SetString("UserRole", "Admin");
                TempData["SuccessMessage"] = user.Message;
                return RedirectToPage("/Admin/AdminPage");
            }
            // Store user session data
            HttpContext.Session.SetInt32("UserId", getUser.AccountId);
            HttpContext.Session.SetInt32("UserRole", (int)getUser.AccountRole);
            TempData["SuccessMessage"] = user.Message;
            if (getUser.AccountRole == 2)
            {
                return RedirectToPage("/Lecturer/Index");
            }
            return RedirectToPage("/Staff/StaffPage");
        }
    }
}
