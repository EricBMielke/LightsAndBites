using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using LightsAndBites.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using LightsAndBites.Classes;
using Microsoft.Extensions.Configuration;
using LightsAndBites.Data;

namespace LightsAndBites.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
            public bool isSuperAdmin { get; set; }
        }

        public void OnGet(string returnUrl = null) //get handler
        {
            ReturnUrl = returnUrl;
        }

        public bool IsUserSuperAdmin(string email)
        {
            List<string> superUsers = new List<string>();

            superUsers.Add("superusertest1@test.com");
            superUsers.Add("superusertest2@test.com");
            superUsers.Add("superusertest3@test.com");
            superUsers.Add("superusertest4@test.com");

            if (superUsers.Contains(email))
            {
                Input.isSuperAdmin = true;
                return Input.isSuperAdmin;
            }
            else
            {
                Input.isSuperAdmin = false;
                return Input.isSuperAdmin;
            }
            
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null) //post handler
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
               
                var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email, isSuperAdmin = IsUserSuperAdmin(Input.Email) };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    if(!await _roleManager.RoleExistsAsync(StaticDetails.AdminEndUser))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(StaticDetails.AdminEndUser));
                    }
                    if(!await _roleManager.RoleExistsAsync(StaticDetails.SuperAdminEndUser))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(StaticDetails.SuperAdminEndUser));
                    }
                    
                    if (Input.isSuperAdmin)
                    {
                        await _userManager.AddToRoleAsync(user, StaticDetails.SuperAdminEndUser);
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, StaticDetails.AdminEndUser);
                    }
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect("~/UserProfiles/Create");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
