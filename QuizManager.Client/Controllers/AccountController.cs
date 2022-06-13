using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuizManager.Client.ViewModels;
using QuizManager.Models;
using QuizManager.Models.Helpers;
using System.Security.Claims;

namespace QuizManager.Client.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet("Identity/Account/Login")]
        public async Task<IActionResult> Login(string returnUrl)
        {
            await EnsureDefaultUsersRegistered();

            var viewModel = new AccountLoginViewModel()
            {
                ReturnUrl = returnUrl
            };

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            return View(viewModel);
        }

        [HttpPost("Identity/Account/Login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AccountLoginViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            // Check we can find the user by the username supplied
            var user = await _signInManager.UserManager.FindByNameAsync(viewModel.Username);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, AccountConstants.UserNotFound);
                return View(viewModel);
            }

            // Don't lockout user on failed logins
            var result = await _signInManager.CheckPasswordSignInAsync(user, viewModel.Password, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, AccountConstants.IncorrectPassword);
                return View(viewModel);
            }

            // Check which claim the user has, from the their permission level, default to Restricted
            string userPermissionLevelClaim = user.PermissionLevel switch
            {
                PermissionLevel.Restricted => UserPermissionLevelClaimConstants.Restricted,
                PermissionLevel.View => UserPermissionLevelClaimConstants.View,
                PermissionLevel.Edit => UserPermissionLevelClaimConstants.Edit,
                _ => UserPermissionLevelClaimConstants.Restricted
            };

            // Login the user with their claims
            await _signInManager.SignInWithClaimsAsync(user, viewModel.RememberLogin, new List<Claim> 
            { 
                new Claim(UserPermissionLevelClaimConstants.ClaimType, userPermissionLevelClaim) 
            });

            // Retain the specific url to redirect to once the user is signed in
            string returnUrl = (!string.IsNullOrEmpty(viewModel.ReturnUrl)) 
                ? viewModel.ReturnUrl 
                : "/";

            return Redirect(returnUrl);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return Redirect("/");
        }

        [HttpGet("Identity/Account/AccessDenied")]
        public IActionResult AccessDenied()
        {
            return View();
        }

        private async Task EnsureDefaultUsersRegistered()
        {
            // Already have the users, so return
            if (_userManager.Users.Count() == 3)
                return;

            // Create users with different permission levels
            List<User> users = new List<User>()
            {
                new User() { UserName = "RestrictedUser", PermissionLevel = PermissionLevel.Restricted },
                new User() { UserName = "ViewUser", PermissionLevel = PermissionLevel.View },
                new User() { UserName = "EditUser", PermissionLevel = PermissionLevel.Edit }
            };

            // Add each user to the database
            foreach (User user in users)
                await _userManager.CreateAsync(user, "Password1!");
        }
    }
}