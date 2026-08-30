using CinemaBooking.Models;
using CinemaBooking.Repositories;
using CinemaBooking.Utilities.DBSeeder;
using CinemaBooking.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CinemaBooking.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly IRepository<ApplicationUserOtp> _applicationUserOtpRepository;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender, IRepository<ApplicationUserOtp> applicationUserOtpRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _applicationUserOtpRepository = applicationUserOtpRepository;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View(registerVM);
            }
            // mapping 
            var user = new ApplicationUser()
            {
                Name = registerVM.Name,
                Adderss = registerVM.Address,
                Email = registerVM.Email,
                UserName = registerVM.UserName
            };
            var result = await _userManager.CreateAsync(user, registerVM.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(registerVM);
            }
            TempData["Successful_Notification"] = "user Created Successfully";
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var link = Url.Action("ConfirmEmail", "Account", new { area = "Identity", userId = user.Id, token }, Request.Scheme);
            await _emailSender.SendEmailAsync(
                registerVM.Email,
                "Ecommerce531 Confirm Email",
                $"<h1> please click <a href={link}>here<a/> to Confirm You Email  </h1>");
            await _userManager.AddToRoleAsync(user, CD.CUSTOMER_ROLE);
            return RedirectToAction(nameof(Login));
        }
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) return NotFound();
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                TempData["Error_Notification"] = "Proplem happend in Confirm Email ";
                return RedirectToAction(nameof(Login));
            }
            TempData["Successful_Notification"] = "Email Confiramtion Successfully";
            return RedirectToAction(nameof(Login));
        }
        [HttpGet]
        public IActionResult ResendEmailConfirmation()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResendEmailConfirmation(ResendEmailConfirmationVM resendEmailConfirmationVM)
        {
            var user = await _userManager.FindByEmailAsync(resendEmailConfirmationVM.UserNameOrEmail) ??
                        await _userManager.FindByNameAsync(resendEmailConfirmationVM.UserNameOrEmail);
            if (user is null)
            {
                ModelState.AddModelError("", "invalid userName or Email");
                return View(resendEmailConfirmationVM);
            }
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var link = Url.Action("ConfirmEmail", "Account", new { area = "Identity", userId = user.Id, token }, Request.Scheme);
            await _emailSender.SendEmailAsync(
                user.Email,
                "Ecommerce531 Confirm Email",
                $"<h1> please click <a href={link}>here<a/> to Confirm You Email  </h1>");
            return RedirectToAction(nameof(Login));
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            var user = await _userManager.FindByEmailAsync(loginVM.UserNameOrEmail) ??
                await _userManager.FindByNameAsync(loginVM.UserNameOrEmail);

            if (user is null)
            {
                ModelState.AddModelError("", "invalid userName or Password");
                return View(loginVM);
            }

            var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.RememberMe, true);

            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "too many attempts try again Later");
                }
                else if (result.IsNotAllowed)
                {
                    ModelState.AddModelError("", "please Confirm Your Email First");
                }
                else
                {
                    ModelState.AddModelError("", "invalid userName or Password");
                }
                return View(loginVM);
            }
            TempData["Successful_Notification"] = "Login Successfully";


            return RedirectToAction("Index", "Home", new { area = "Customer" });
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordVM forgetPasswordVM)
        {
            var user = await _userManager.FindByEmailAsync(forgetPasswordVM.UserNameOrEmail) ??
                await _userManager.FindByNameAsync(forgetPasswordVM.UserNameOrEmail);
            if (user is null)
            {
                ModelState.AddModelError("", "invalid userName");
                return View(forgetPasswordVM);
            }
            var otps = await _applicationUserOtpRepository.GetAllAsync(o => o.ApplicationUserId == user.Id);
            var count = otps.Count(o => (DateTime.UtcNow - o.CreatedAt).TotalHours <= 24);
            if (count > 5)
            {
                ModelState.AddModelError("", "Too Many attempts Please Try Again Later");
                return View(forgetPasswordVM);
            }
            var otp = new Random().Next(1000, 9999).ToString();
            var applicationUserOtp = new ApplicationUserOtp(otp, user.Id);
            await _applicationUserOtpRepository.InsertAsync(applicationUserOtp);
            await _applicationUserOtpRepository.CommitAsync();
            await _emailSender.SendEmailAsync(
                user.Email,
                "Ecommerce531 Reset Password",
                $"<h1> use This OTP {otp}to Reset Your Password </h1>");
            return RedirectToAction(nameof(VerifyOtp), new { userId = user.Id });
        }
        [HttpGet]
        public IActionResult VerifyOtp(string userId)
        {
            return View(new VerifyOtpVM { UserId = userId });
        }
        [HttpPost]
        public async Task<IActionResult> VerifyOtp(VerifyOtpVM verifyOtpVM)
        {
            var user = await _userManager.FindByIdAsync(verifyOtpVM.UserId);
            if (user is null)
            {
                ModelState.AddModelError("", "invalid user");
                return View(verifyOtpVM);
            }
            var otps = await _applicationUserOtpRepository.GetAllAsync(o =>
                o.ApplicationUserId == user.Id &&
                o.IsValid == true &&
                o.ValidTo >= DateTime.UtcNow
                );
            var applicationUserotp = otps.OrderByDescending(o => o.CreatedAt).FirstOrDefault();
            if (applicationUserotp == null || applicationUserotp.OTP != verifyOtpVM.OTP)
            {
                ModelState.AddModelError("", "invalid / Expired  otp ");
                return View(verifyOtpVM);
            }
            applicationUserotp.IsValid = false;
            await _applicationUserOtpRepository.CommitAsync();
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            return RedirectToAction(nameof(ResetPassword), new { userId = user.Id, token });
        }
        [HttpGet]
        public IActionResult ResetPassword(string userId, string token)
        {
            return View(new ResetPasswordVM { UserId = userId, Token = token });
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM resetPasswordVM)
        {
            if (!ModelState.IsValid)
            {
                return View(resetPasswordVM);
            }
            var user = await _userManager.FindByIdAsync(resetPasswordVM.UserId);
            if (user is null)
            {
                ModelState.AddModelError("", "invalid user ");
                return View(resetPasswordVM);
            }
            var result = await _userManager.ResetPasswordAsync(user, resetPasswordVM.Token, resetPasswordVM.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(resetPasswordVM);
            }
            TempData["Successful_Notification"] = "Reset Password Successfully";
            return RedirectToAction(nameof(Login));

        }

        public async Task<IActionResult> AccessDenied()
        {
            return View();
        }
    }
}
