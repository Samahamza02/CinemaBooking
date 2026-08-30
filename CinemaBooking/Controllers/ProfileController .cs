using CinemaBooking.Models;
using CinemaBooking.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace CinemaBooking.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();
            var applicationUserVM = new ApplicationUserVM()
            {
                Name = user.Name,
                Adderss = user.Adderss,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
            };
          //  var applicationUserVM = user.Adapt<ApplicationUserVM>();
            return View(applicationUserVM);
        }
        public async Task<IActionResult> UpdateProfile(ApplicationUserVM applicationUserVM)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();
            user.Name = applicationUserVM.Name;
            user.Adderss = applicationUserVM.Adderss;
            user.PhoneNumber = applicationUserVM.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(" ,", result.Errors.Select(e => e.Description));
                TempData["Error_Notification"] = errors;
                return RedirectToAction(nameof(Index), applicationUserVM);
            }
            else
            {
                TempData["Successful_Notification"] = "profile  updated Successfully";
                return RedirectToAction(nameof(Index), applicationUserVM);
            }

        }
        public async Task<IActionResult> UpdatePassword(ApplicationUserVM applicationUserVM)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var result = await _userManager.ChangePasswordAsync(user, applicationUserVM.CurrentPassword, applicationUserVM.NewPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join(" ,", result.Errors.Select(e => e.Description));
                TempData["Error_Notification"] = errors;
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["Successful_Notification"] = "Password changed Successfully";
                return RedirectToAction(nameof(Index));
            }

        }
    }
}
